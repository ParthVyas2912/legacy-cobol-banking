#!/usr/bin/env python3
"""
MainframeForge — Record converter for ACCTFILE (ACCT-RECORD)

Reads fixed-length 118-byte records from a mainframe
UNLOAD flat file, decodes EBCDIC / COMP-3 / packed fields, and writes
a pipe-delimited CSV suitable for PostgreSQL COPY or SQL Server BULK INSERT.

Source record: ACCT-RECORD (118 bytes)
Target table:  acctfile
"""

import sys
import csv
import decimal
import struct
from pathlib import Path

# ─── COMP-3 (packed decimal) decoder ───

def unpack_comp3(data: bytes, integer_digits: int, frac_digits: int) -> decimal.Decimal:
    """Decode COBOL COMP-3 (packed decimal) to Python Decimal."""
    if len(data) == 0:
        return decimal.Decimal(0)
    digits = []
    for byte in data[:-1]:
        digits.append(str((byte >> 4) & 0x0F))
        digits.append(str(byte & 0x0F))
    # Last byte: high nibble is last digit, low nibble is sign
    digits.append(str((data[-1] >> 4) & 0x0F))
    sign_nibble = data[-1] & 0x0F
    raw = ''.join(digits).lstrip('0') or '0'
    if frac_digits > 0:
        if len(raw) <= frac_digits:
            raw = raw.zfill(frac_digits + 1)
        raw = raw[:-frac_digits] + '.' + raw[-frac_digits:]
    result = decimal.Decimal(raw)
    if sign_nibble in (0x0D, 0x0B):  # negative
        result = -result
    return result


def unpack_comp(data: bytes, signed: bool = True) -> int:
    """Decode COBOL COMP / COMP-4 / COMP-5 (binary integer)."""
    n = len(data)
    if n == 2:
        return struct.unpack('>h' if signed else '>H', data)[0]
    elif n == 4:
        return struct.unpack('>i' if signed else '>I', data)[0]
    elif n == 8:
        return struct.unpack('>q' if signed else '>Q', data)[0]
    else:
        val = int.from_bytes(data, 'big')
        if signed and (data[0] & 0x80):
            val -= 1 << (n * 8)
        return val


def decode_display(data: bytes, encoding: str = 'cp037') -> str:
    """Decode EBCDIC (or ASCII) display field, strip trailing spaces."""
    try:
        return data.decode(encoding).rstrip()
    except (UnicodeDecodeError, LookupError):
        return data.decode('latin-1').rstrip()


def decode_zoned(data: bytes, encoding: str = 'cp037', frac_digits: int = 0) -> decimal.Decimal:
    """Decode COBOL zoned decimal (DISPLAY numeric) from EBCDIC."""
    text = data.decode(encoding)
    # Last byte encodes sign in the zone nibble (EBCDIC)
    raw = text.replace('{', '0').replace('}', '0')  # simplified
    # Handle standard EBCDIC overpunch sign
    for old, new, neg in [
        ('{', '0', False), ('A','1',False), ('B','2',False), ('C','3',False),
        ('D','4',False), ('E','5',False), ('F','6',False), ('G','7',False),
        ('H','8',False), ('I','9',False),
        ('}', '0', True), ('J','1',True), ('K','2',True), ('L','3',True),
        ('M','4',True), ('N','5',True), ('O','6',True), ('P','7',True),
        ('Q','8',True), ('R','9',True),
    ]:
        if text[-1] == old:
            raw = text[:-1] + new
            if frac_digits > 0:
                raw = raw[:-frac_digits] + '.' + raw[-frac_digits:]
            result = decimal.Decimal(raw)
            return -result if neg else result
    # Fallback: assume unsigned
    raw = text
    if frac_digits > 0:
        raw = raw[:-frac_digits] + '.' + raw[-frac_digits:]
    return decimal.Decimal(raw)


# ─── Record decoder ───

RECORD_LENGTH = 118

def decode_record(raw: bytes) -> dict:
    """Decode a single ACCT-RECORD record (118 bytes)."""
    d = {}
    d['acct_number'] = decode_display(raw[0:10])
    d['acct_customer_id'] = decode_display(raw[10:18])
    d['acct_opened_date'] = decode_display(raw[18:26])
    d['acct_type'] = decode_display(raw[26:28])
    d['acct_status'] = decode_display(raw[28:29])
    d['acct_balance'] = unpack_comp3(raw[29:37], 13, 2)
    d['acct_avail_bal'] = unpack_comp3(raw[37:45], 13, 2)
    d['acct_int_rate'] = unpack_comp3(raw[45:49], 2, 4)
    d['acct_last_txn_dt'] = decode_display(raw[49:57])
    d['acct_currency'] = decode_display(raw[57:60])
    d['acct_branch_code'] = decode_display(raw[60:64])
    d['acct_flags'] = decode_display(raw[64:68])
    d['acct_filler'] = decode_display(raw[68:118])
    return d

COLUMNS = ["acct_number","acct_customer_id","acct_opened_date","acct_type","acct_status","acct_balance","acct_avail_bal","acct_int_rate","acct_last_txn_dt","acct_currency","acct_branch_code","acct_flags","acct_filler"]

def main():
    if len(sys.argv) < 2:
        print(f'Usage: {sys.argv[0]} <input-file> [output-csv]', file=sys.stderr)
        sys.exit(1)
    input_path = Path(sys.argv[1])
    output_path = Path(sys.argv[2]) if len(sys.argv) > 2 else input_path.with_suffix('.csv')

    with open(input_path, 'rb') as fin, open(output_path, 'w', newline='') as fout:
        writer = csv.DictWriter(fout, fieldnames=COLUMNS, delimiter='|',
                                extrasaction='ignore', quoting=csv.QUOTE_MINIMAL)
        writer.writeheader()
        record_count = 0
        error_count = 0
        while True:
            chunk = fin.read(RECORD_LENGTH)
            if not chunk:
                break
            if len(chunk) < RECORD_LENGTH:
                print(f'Warning: short record at offset {fin.tell() - len(chunk)} ({len(chunk)} < {RECORD_LENGTH})', file=sys.stderr)
                error_count += 1
                break
            try:
                row = decode_record(chunk)
                writer.writerow(row)
                record_count += 1
            except Exception as e:
                error_count += 1
                print(f'Error at record {record_count + error_count}: {e}', file=sys.stderr)
                if error_count > 100:
                    print('Too many errors — aborting.', file=sys.stderr)
                    sys.exit(2)
        print(f'Converted {record_count} records ({error_count} errors) → {output_path}')

if __name__ == '__main__':
    main()