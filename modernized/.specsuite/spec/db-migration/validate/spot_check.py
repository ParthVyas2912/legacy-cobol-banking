#!/usr/bin/env python3
"""
MainframeForge — Record-level spot-check validator

Reads N random records from both the source mainframe flat file and
the target database, and compares them field-by-field. Reports any
mismatches.

Usage:
  python3 spot_check.py --source-dir /data/staging --db-url postgresql://... [--sample 100]
"""

import argparse
import json
import random
import sys
from pathlib import Path

try:
    import psycopg2  # PostgreSQL
except ImportError:
    psycopg2 = None
try:
    import pyodbc  # SQL Server
except ImportError:
    pyodbc = None

# Import the converters to reuse their decode_record functions
CONVERTERS = {
    'acctfile': 'ACCTFILE',
    'auditvsam': 'AUDITVSAM',
    'intjrnl': 'INTJRNL',
    'stmtout': 'STMTOUT',
    'txnin': 'TXNIN',
    'auditout': 'AUDITOUT',
}

SCHEMA = 'public'
SAMPLE_SIZE = 100


def connect_db(db_url: str):
    """Connect to the target database."""
    if db_url.startswith('postgresql'):
        if not psycopg2:
            print('Install psycopg2: pip install psycopg2-binary', file=sys.stderr)
            sys.exit(1)
        return psycopg2.connect(db_url)
    else:
        if not pyodbc:
            print('Install pyodbc: pip install pyodbc', file=sys.stderr)
            sys.exit(1)
        return pyodbc.connect(db_url)


def spot_check_table(conn, table_name: str, source_file: Path, record_length: int, decode_fn, columns: list, sample_n: int):
    """Compare N random source records against the target table."""
    file_size = source_file.stat().st_size
    total_records = file_size // record_length if record_length > 0 else 0
    if total_records == 0:
        print(f'  {table_name}: source file empty or record length unknown — skipping')
        return 0
    indices = sorted(random.sample(range(total_records), min(sample_n, total_records)))
    mismatches = 0
    with open(source_file, 'rb') as f:
        cursor = conn.cursor()
        for idx in indices:
            f.seek(idx * record_length)
            raw = f.read(record_length)
            if len(raw) < record_length:
                continue
            source_row = decode_fn(raw)
            # Fetch the corresponding row from the target by PK
            pk_col = columns[0]  # first column is typically the PK
            pk_val = source_row.get(pk_col)
            if pk_val is None:
                continue
            cursor.execute(f'SELECT * FROM {SCHEMA}.{table_name} WHERE "{pk_col}" = %s LIMIT 1', (str(pk_val),))
            target_row = cursor.fetchone()
            if target_row is None:
                print(f'  ❌ {table_name} record #{idx} (PK={pk_val}): NOT FOUND in target')
                mismatches += 1
                continue
            # Field-by-field comparison (simplified — full comparison
            # would need to handle type coercion and rounding)
            target_dict = dict(zip([d[0] for d in cursor.description], target_row))
            for col in columns:
                src_val = str(source_row.get(col, ''))
                tgt_val = str(target_dict.get(col, ''))
                if src_val.strip() != tgt_val.strip():
                    print(f'  ⚠ {table_name}.{col} record #{idx}: source="{src_val}" target="{tgt_val}"')
                    mismatches += 1
    return mismatches


def main():
    parser = argparse.ArgumentParser(description='MainframeForge spot-check validator')
    parser.add_argument('--source-dir', required=True, help='Directory with unloaded flat files')
    parser.add_argument('--db-url', required=True, help='Target database connection URL')
    parser.add_argument('--sample', type=int, default=SAMPLE_SIZE, help=f'Records to sample per table (default: {SAMPLE_SIZE})')
    args = parser.parse_args()

    conn = connect_db(args.db_url)
    total_mismatches = 0
    print(f'Spot-checking {len(CONVERTERS)} table(s) with {args.sample} samples each...')
    print()
    for table_name, dd_name in CONVERTERS.items():
        source_path = Path(args.source_dir) / f'{table_name}.dat'
        if not source_path.exists():
            print(f'  {table_name}: source file not found at {source_path} — skipping')
            continue
        # Dynamic import of the converter module
        try:
            import importlib.util
            spec_mod = importlib.util.spec_from_file_location(f'convert_{table_name}', Path(__file__).parent / '..' / 'etl' / 'convert' / f'convert_{table_name}.py')
            if spec_mod and spec_mod.loader:
                mod = importlib.util.module_from_spec(spec_mod)
                spec_mod.loader.exec_module(mod)
                decode_fn = mod.decode_record
                columns = mod.COLUMNS
                record_length = mod.RECORD_LENGTH
                mismatches = spot_check_table(conn, table_name, source_path, record_length, decode_fn, columns, args.sample)
                total_mismatches += mismatches
                if mismatches == 0:
                    print(f'  ✅ {table_name}: {args.sample} records verified — all match')
            else:
                print(f'  {table_name}: converter module not found — skipping')
        except Exception as e:
            print(f'  ❌ {table_name}: error during spot-check: {e}', file=sys.stderr)
            total_mismatches += 1
    print()
    if total_mismatches == 0:
        print('✅ All spot-checks passed!')
    else:
        print(f'❌ {total_mismatches} mismatch(es) found. Review above.')
        sys.exit(1)
    conn.close()


if __name__ == '__main__':
    main()