# ETL Pipeline — MainframeForge Phase 2

## Overview

This directory contains a complete extract-transform-load pipeline
for migrating mainframe data to PostgreSQL.

| Metric | Value |
|--------|-------|
| Record converters | 9 |
| Extract JCL scripts | 13 |
| Load scripts | 15 |
| Validation scripts | 1 |
| Source encoding | cp037 |
| CSV delimiter | `|` (pipe) |
| Mainframe HLQ | `MIGR.UNLOAD` |
| Staging directory | `/data/migration/staging` |

## Directory structure

```
etl/
├── convert/            Python record decoders (one per file/segment)
├── extract/            JCL to unload data from the mainframe
├── load/               SQL COPY/BULK INSERT scripts
├── validate/           Post-load validation queries
├── run_migration.sh    Orchestration script
└── README.md           This file
```

## Step-by-step

### 1. Create the target schema

```bash
# Run the Phase 1 schema scripts (in the parent db-migration/schema/ dir)
psql -f ../schema/000_enums.sql
psql -f ../schema/001_tables.sql
psql -f ../schema/002_indexes.sql
psql -f ../schema/003_constraints.sql
```

### 2. Submit mainframe extract JCL

Upload the JCL in `extract/` to the mainframe and submit:

```
UNLOAD_ACCTFILE.jcl
UNLOAD_AUDITVSAM.jcl
UNLOAD_INTJRNL.jcl
UNLOAD_STMTOUT.jcl
UNLOAD_TXNIN.jcl
UNLOAD_AUDITOUT.jcl
UNLOAD_IMS_CUSTDB.jcl
UNLOAD_DB2_ACCT_AUDIT.jcl
UNLOAD_DB2_CUSTOMER.jcl
UNLOAD_DB2_EOD_RUNS.jcl
UNLOAD_DB2_TXN_LEDGER.jcl
UNLOAD_DB2_FRAUD_FLAGS.jcl
UNLOAD_DB2_KYC_AUDIT.jcl
```

Wait for all jobs to complete with RC=0.

### 3. Transfer unloaded files

FTP/SFTP the unloaded flat files from the mainframe to `/data/migration/staging/`
on the target host. Use BINARY transfer mode (not ASCII) to preserve
COMP-3 packed bytes and EBCDIC encoding.

### 4. Run the pipeline

```bash
cd etl/
./run_migration.sh --db-url "postgresql://user:pass@host/dbname"
```

Or run steps individually:

```bash
# Convert
python3 convert/convert_acctfile.py /data/migration/staging/acctfile.dat
python3 convert/convert_auditvsam.py /data/migration/staging/auditvsam.dat
python3 convert/convert_intjrnl.py /data/migration/staging/intjrnl.dat
python3 convert/convert_stmtout.py /data/migration/staging/stmtout.dat
python3 convert/convert_txnin.py /data/migration/staging/txnin.dat
python3 convert/convert_auditout.py /data/migration/staging/auditout.dat
python3 convert/convert_custdb_customer.py /data/migration/staging/custdb_customer.dat
python3 convert/convert_custdb_acctlnk.py /data/migration/staging/custdb_acctlnk.dat
python3 convert/convert_custdb_address.py /data/migration/staging/custdb_address.dat

# Load
psql -f load/load_acctfile.sql
psql -f load/load_auditvsam.sql
psql -f load/load_intjrnl.sql
psql -f load/load_stmtout.sql
psql -f load/load_txnin.sql
psql -f load/load_auditout.sql
psql -f load/load_custdb_customer.sql
psql -f load/load_custdb_acctlnk.sql
psql -f load/load_custdb_address.sql
psql -f load/load_acct_audit.sql
psql -f load/load_customer.sql
psql -f load/load_eod_runs.sql
psql -f load/load_txn_ledger.sql
psql -f load/load_fraud_flags.sql
psql -f load/load_kyc_audit.sql

# Validate
psql -f validate/validate_migration.sql
```

### 5. Review validation output

The validation script checks:
- **Row counts** — every target table vs expected mainframe counts
- **Balance reconciliation** — SUM of monetary fields (COMP-3 V99)
- **PK uniqueness** — no duplicate primary keys
- **FK referential integrity** — no orphaned child rows (IMS → relational)

## Python converter details

Each converter in `convert/` handles:
- **EBCDIC → UTF-8** transcoding (`cp037` codepage)
- **COMP-3** (packed decimal) → Python `Decimal` → CSV numeric
- **COMP/COMP-4/COMP-5** (binary integer) → Python `int`
- **Zoned decimal** (DISPLAY numeric with EBCDIC overpunch sign)
- **FILLER** fields are skipped
- Records with decode errors are logged to stderr (tolerates up to 100)

## Customisation

- **Encoding**: change `decode_display(data, enc='...')` in converters
- **Date conversion**: add date parsing in the converter after decode
- **IMS**: the segment converters need site-specific unload format parsing
- **DB2**: adjust DSNTIAUL parameters and SSID in the extract JCL
