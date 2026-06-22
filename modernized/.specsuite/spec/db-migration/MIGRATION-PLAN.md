# Data Migration Plan

**Target:** PostgreSQL
**Schema:** `public`
**Generated:** 2026-06-22T17:08:24.818Z

## Tables

| # | Table | Source | Source ID | Columns | PK | Used by |
|---|-------|--------|-----------|---------|----|---------| 
| 1 | `acctfile` | vsam | ACCTFILE | 13 | acct_number | ACCTMGR, INTCALC |
| 2 | `auditvsam` | vsam | AUDITVSAM | 6 | audit_key | AUDITSRV |
| 3 | `intjrnl` | qsam | INTJRNL | 12 | — | INTCALC |
| 4 | `stmtout` | qsam | STMTOUT | 4 | — | STMTRPT |
| 5 | `txnin` | qsam | TXNIN | 12 | — | TXNPOST |
| 6 | `auditout` | qsam | AUDITOUT | 0 | — | TXNPOST |
| 7 | `custdb_customer` | ims | CUSTDB.CUSTOMER | 7 | customer_id |  |
| 8 | `custdb_acctlnk` | ims | CUSTDB.ACCTLNK | 4 | acctlnk_id |  |
| 9 | `custdb_address` | ims | CUSTDB.ADDRESS | 7 | address_id |  |
| 10 | `acct_audit` | db2 | ACCT_AUDIT | 6 | — | ACCTMGR |
| 11 | `customer` | db2 | CUSTOMER | 2 | — | ACCTMGR |
| 12 | `eod_runs` | db2 | EOD_RUNS | 4 | — | BCHEOD |
| 13 | `txn_ledger` | db2 | TXN_LEDGER | 13 | — | FRDCHK, TXNPOST |
| 14 | `fraud_flags` | db2 | FRAUD_FLAGS | 6 | — | FRDCHK |
| 15 | `kyc_audit` | db2 | KYC_AUDIT | 3 | — | KYCSYNC |

## Indexes

| Index | Table | Columns | Unique |
|-------|-------|---------|--------|
| `idx_auditvsam_audit_acct_number` | `auditvsam` | audit_acct_number |  |
| `idx_custdb_customer_custid` | `custdb_customer` | custid | ✓ |
| `idx_custdb_acctlnk_acctnum` | `custdb_acctlnk` | acctnum | ✓ |
| `idx_custdb_address_effdate` | `custdb_address` | effdate | ✓ |

## Constraints

| Constraint | Table | Kind |
|------------|-------|------|
| `chk_acctfile_acct_type` | `acctfile` | check |
| `chk_acctfile_acct_status` | `acctfile` | check |
| `chk_acctfile_acct_flags` | `acctfile` | check |
| `chk_intjrnl_txn_code` | `intjrnl` | check |
| `chk_intjrnl_txn_channel` | `intjrnl` | check |
| `chk_intjrnl_txn_status` | `intjrnl` | check |
| `chk_txnin_txn_code` | `txnin` | check |
| `chk_txnin_txn_channel` | `txnin` | check |
| `chk_txnin_txn_status` | `txnin` | check |
| `fk_custdb_acctlnk_customer` | `custdb_acctlnk` | foreign-key |
| `fk_custdb_address_customer` | `custdb_address` | foreign-key |

## Enum Types

- `acctfile_acct_type_enum`: `CK` | `SV` | `LN`
- `acctfile_acct_status_enum`: `A` | `F` | `C`
- `acctfile_acct_flags_enum`: `YYYY` | `YYNN` | `YYYN`
- `intjrnl_txn_code_enum`: `DEP` | `WDR` | `XFR` | `FEE` | `INT` | `REV`
- `intjrnl_txn_channel_enum`: `ATM ` | `BRCH` | `ONLN` | `BTCH`
- `intjrnl_txn_status_enum`: `P` | `O` | `F`
- `txnin_txn_code_enum`: `DEP` | `WDR` | `XFR` | `FEE` | `INT` | `REV`
- `txnin_txn_channel_enum`: `ATM ` | `BRCH` | `ONLN` | `BTCH`
- `txnin_txn_status_enum`: `P` | `O` | `F`

## Type Mapping Reference

| COBOL | SQL | Notes |
|-------|-----|-------|
| `PIC 9(n)` | `BIGINT` / `NUMERIC(n)` | unsigned integer |
| `PIC S9(p)V9(s) COMP-3` | `NUMERIC(p+s, s)` | packed decimal → exact numeric |
| `PIC X(n)` | `VARCHAR(n)` | character field |
| `PIC 9(8)` + `domainContext: date` | `DATE` | YYYYMMDD → DATE conversion |
| `OCCURS n TIMES` | child table / ARRAY / JSON | configurable via `--occurs-strategy` |
| `REDEFINES` | JSONB / VARCHAR | configurable via `--redefines-strategy` |
| `88-level conditions` | CHECK constraint + ENUM type | values from condition names |
| IMS segment | table + FK to parent | hierarchical → relational |
| IMS key field (`SEQ,U`) | PRIMARY KEY + UNIQUE INDEX | |
| VSAM record key | PRIMARY KEY | from `SELECT ... RECORD KEY IS` |
| VSAM alternate key | INDEX (UNIQUE or not) | from `ALTERNATE RECORD KEY ... WITH DUPLICATES` |

## Next Steps

1. Review generated DDL in `schema/001_tables.sql`
2. Adjust DB2 column types (marked with `-- Review type mapping` comments)
3. Run against target database: `psql -f schema/000_enums.sql && psql -f schema/001_tables.sql && psql -f schema/002_indexes.sql && psql -f schema/003_constraints.sql`
4. Generate ETL pipeline (Phase 2): `specforge data-extract <spec-dir> --target <db-url>`
