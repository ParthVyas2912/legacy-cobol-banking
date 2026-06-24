-- ═══════════════════════════════════════════════════════
-- MainframeForge — Post-Migration Validation Suite
-- Target: PostgreSQL
-- Schema: public
-- Generated: 2026-06-24T20:23:53.737Z
-- ═══════════════════════════════════════════════════════
--
-- This script validates the migrated data against mainframe-side
-- expectations. Run it after all data has been loaded. Compare
-- the output against the expected_counts.json template (fill in
-- mainframe counts from IDCAMS LISTCAT / DB2 RUNSTATS / IMS
-- DBT utilities before running).


-- ═══ SECTION 1: Row Counts ═══

-- Compare each count against expected_counts.json values.

SELECT 'acctfile' AS "table", 'vsam' AS source, COUNT(*) AS row_count FROM public.acctfile;
SELECT 'auditvsam' AS "table", 'vsam' AS source, COUNT(*) AS row_count FROM public.auditvsam;
SELECT 'intjrnl' AS "table", 'qsam' AS source, COUNT(*) AS row_count FROM public.intjrnl;
SELECT 'stmtout' AS "table", 'qsam' AS source, COUNT(*) AS row_count FROM public.stmtout;
SELECT 'txnin' AS "table", 'qsam' AS source, COUNT(*) AS row_count FROM public.txnin;
SELECT 'auditout' AS "table", 'qsam' AS source, COUNT(*) AS row_count FROM public.auditout;
SELECT 'custdb_customer' AS "table", 'ims' AS source, COUNT(*) AS row_count FROM public.custdb_customer;
SELECT 'custdb_acctlnk' AS "table", 'ims' AS source, COUNT(*) AS row_count FROM public.custdb_acctlnk;
SELECT 'custdb_address' AS "table", 'ims' AS source, COUNT(*) AS row_count FROM public.custdb_address;
SELECT 'acct_audit' AS "table", 'db2' AS source, COUNT(*) AS row_count FROM public.acct_audit;
SELECT 'customer' AS "table", 'db2' AS source, COUNT(*) AS row_count FROM public.customer;
SELECT 'eod_runs' AS "table", 'db2' AS source, COUNT(*) AS row_count FROM public.eod_runs;
SELECT 'txn_ledger' AS "table", 'db2' AS source, COUNT(*) AS row_count FROM public.txn_ledger;
SELECT 'fraud_flags' AS "table", 'db2' AS source, COUNT(*) AS row_count FROM public.fraud_flags;
SELECT 'kyc_audit' AS "table", 'db2' AS source, COUNT(*) AS row_count FROM public.kyc_audit;

-- ═══ SECTION 2: Primary Key Uniqueness ═══

-- Every row must pass: row_count = distinct_keys (status = OK).

SELECT 'acctfile' AS "table",
       COUNT(*) AS total_rows,
       COUNT(DISTINCT "acct_number") AS distinct_keys,
       CASE WHEN COUNT(*) = COUNT(DISTINCT "acct_number")
            THEN 'OK' ELSE 'DUPLICATE KEYS ❌' END AS status
FROM public.acctfile;
SELECT 'auditvsam' AS "table",
       COUNT(*) AS total_rows,
       COUNT(DISTINCT "audit_key") AS distinct_keys,
       CASE WHEN COUNT(*) = COUNT(DISTINCT "audit_key")
            THEN 'OK' ELSE 'DUPLICATE KEYS ❌' END AS status
FROM public.auditvsam;
SELECT 'custdb_customer' AS "table",
       COUNT(*) AS total_rows,
       COUNT(DISTINCT "customer_id") AS distinct_keys,
       CASE WHEN COUNT(*) = COUNT(DISTINCT "customer_id")
            THEN 'OK' ELSE 'DUPLICATE KEYS ❌' END AS status
FROM public.custdb_customer;
SELECT 'custdb_acctlnk' AS "table",
       COUNT(*) AS total_rows,
       COUNT(DISTINCT "acctlnk_id") AS distinct_keys,
       CASE WHEN COUNT(*) = COUNT(DISTINCT "acctlnk_id")
            THEN 'OK' ELSE 'DUPLICATE KEYS ❌' END AS status
FROM public.custdb_acctlnk;
SELECT 'custdb_address' AS "table",
       COUNT(*) AS total_rows,
       COUNT(DISTINCT "address_id") AS distinct_keys,
       CASE WHEN COUNT(*) = COUNT(DISTINCT "address_id")
            THEN 'OK' ELSE 'DUPLICATE KEYS ❌' END AS status
FROM public.custdb_address;

-- ═══ SECTION 3: Monetary Balance Reconciliation ═══

-- Compare SUM of every monetary / decimal column against mainframe totals.

SELECT 'acctfile.acct_balance' AS field,
       'S9(13)V99 comp-3' AS cobol_type,
       '' AS domain,
       COUNT(*) AS non_null_count,
       SUM("acct_balance") AS total,
       MIN("acct_balance") AS min_value,
       MAX("acct_balance") AS max_value,
       AVG("acct_balance")::NUMERIC(20,4) AS avg_value
FROM public.acctfile
WHERE "acct_balance" IS NOT NULL;
SELECT 'acctfile.acct_avail_bal' AS field,
       'S9(13)V99 comp-3' AS cobol_type,
       '' AS domain,
       COUNT(*) AS non_null_count,
       SUM("acct_avail_bal") AS total,
       MIN("acct_avail_bal") AS min_value,
       MAX("acct_avail_bal") AS max_value,
       AVG("acct_avail_bal")::NUMERIC(20,4) AS avg_value
FROM public.acctfile
WHERE "acct_avail_bal" IS NOT NULL;
SELECT 'acctfile.acct_int_rate' AS field,
       '9(2)V9(4) comp-3' AS cobol_type,
       '' AS domain,
       COUNT(*) AS non_null_count,
       SUM("acct_int_rate") AS total,
       MIN("acct_int_rate") AS min_value,
       MAX("acct_int_rate") AS max_value,
       AVG("acct_int_rate")::NUMERIC(20,4) AS avg_value
FROM public.acctfile
WHERE "acct_int_rate" IS NOT NULL;
SELECT 'intjrnl.txn_amount' AS field,
       'S9(11)V99 comp-3' AS cobol_type,
       '' AS domain,
       COUNT(*) AS non_null_count,
       SUM("txn_amount") AS total,
       MIN("txn_amount") AS min_value,
       MAX("txn_amount") AS max_value,
       AVG("txn_amount")::NUMERIC(20,4) AS avg_value
FROM public.intjrnl
WHERE "txn_amount" IS NOT NULL;
SELECT 'intjrnl.txn_balance_after' AS field,
       'S9(13)V99 comp-3' AS cobol_type,
       '' AS domain,
       COUNT(*) AS non_null_count,
       SUM("txn_balance_after") AS total,
       MIN("txn_balance_after") AS min_value,
       MAX("txn_balance_after") AS max_value,
       AVG("txn_balance_after")::NUMERIC(20,4) AS avg_value
FROM public.intjrnl
WHERE "txn_balance_after" IS NOT NULL;
SELECT 'txnin.txn_amount' AS field,
       'S9(11)V99 comp-3' AS cobol_type,
       '' AS domain,
       COUNT(*) AS non_null_count,
       SUM("txn_amount") AS total,
       MIN("txn_amount") AS min_value,
       MAX("txn_amount") AS max_value,
       AVG("txn_amount")::NUMERIC(20,4) AS avg_value
FROM public.txnin
WHERE "txn_amount" IS NOT NULL;
SELECT 'txnin.txn_balance_after' AS field,
       'S9(13)V99 comp-3' AS cobol_type,
       '' AS domain,
       COUNT(*) AS non_null_count,
       SUM("txn_balance_after") AS total,
       MIN("txn_balance_after") AS min_value,
       MAX("txn_balance_after") AS max_value,
       AVG("txn_balance_after")::NUMERIC(20,4) AS avg_value
FROM public.txnin
WHERE "txn_balance_after" IS NOT NULL;

-- ═══ SECTION 4: Foreign Key Referential Integrity ═══

-- Every FK must have 0 orphaned_rows.

-- FK: custdb_acctlnk.customer_id → custdb_customer.customer_id
SELECT 'custdb_acctlnk → custdb_customer' AS fk_check,
       COUNT(*) AS orphaned_rows,
       CASE WHEN COUNT(*) = 0 THEN 'OK' ELSE 'ORPHANS ❌' END AS status
FROM public.custdb_acctlnk c
LEFT JOIN public.custdb_customer p ON c."customer_id" = p."customer_id"
WHERE p."customer_id" IS NULL;
-- FK: custdb_address.customer_id → custdb_customer.customer_id
SELECT 'custdb_address → custdb_customer' AS fk_check,
       COUNT(*) AS orphaned_rows,
       CASE WHEN COUNT(*) = 0 THEN 'OK' ELSE 'ORPHANS ❌' END AS status
FROM public.custdb_address c
LEFT JOIN public.custdb_customer p ON c."customer_id" = p."customer_id"
WHERE p."customer_id" IS NULL;

-- ═══ SECTION 5: Enum Domain Validation ═══

-- Values outside the 88-level domain appear as out_of_domain > 0.


-- ═══ SECTION 6: Date Field Sanity ═══

-- Dates stored as VARCHAR(8) YYYYMMDD should parse. Check for invalids.

SELECT 'acctfile.acct_opened_date' AS field,
       COUNT(*) AS total,
       COUNT(*) FILTER (WHERE "acct_opened_date" ~ '^[0-9]{8}$'
                          AND "acct_opened_date"::DATE IS NOT NULL) AS valid_dates,
       COUNT(*) FILTER (WHERE "acct_opened_date" !~ '^[0-9]{8}$'
                          OR LENGTH(TRIM("acct_opened_date")) = 0) AS invalid_dates
FROM public.acctfile;
SELECT 'acctfile.acct_last_txn_dt' AS field,
       COUNT(*) AS total,
       COUNT(*) FILTER (WHERE "acct_last_txn_dt" ~ '^[0-9]{8}$'
                          AND "acct_last_txn_dt"::DATE IS NOT NULL) AS valid_dates,
       COUNT(*) FILTER (WHERE "acct_last_txn_dt" !~ '^[0-9]{8}$'
                          OR LENGTH(TRIM("acct_last_txn_dt")) = 0) AS invalid_dates
FROM public.acctfile;
SELECT 'intjrnl.txn_post_date' AS field,
       COUNT(*) AS total,
       COUNT(*) FILTER (WHERE "txn_post_date" ~ '^[0-9]{8}$'
                          AND "txn_post_date"::DATE IS NOT NULL) AS valid_dates,
       COUNT(*) FILTER (WHERE "txn_post_date" !~ '^[0-9]{8}$'
                          OR LENGTH(TRIM("txn_post_date")) = 0) AS invalid_dates
FROM public.intjrnl;
SELECT 'intjrnl.txn_effective_date' AS field,
       COUNT(*) AS total,
       COUNT(*) FILTER (WHERE "txn_effective_date" ~ '^[0-9]{8}$'
                          AND "txn_effective_date"::DATE IS NOT NULL) AS valid_dates,
       COUNT(*) FILTER (WHERE "txn_effective_date" !~ '^[0-9]{8}$'
                          OR LENGTH(TRIM("txn_effective_date")) = 0) AS invalid_dates
FROM public.intjrnl;
SELECT 'txnin.txn_post_date' AS field,
       COUNT(*) AS total,
       COUNT(*) FILTER (WHERE "txn_post_date" ~ '^[0-9]{8}$'
                          AND "txn_post_date"::DATE IS NOT NULL) AS valid_dates,
       COUNT(*) FILTER (WHERE "txn_post_date" !~ '^[0-9]{8}$'
                          OR LENGTH(TRIM("txn_post_date")) = 0) AS invalid_dates
FROM public.txnin;
SELECT 'txnin.txn_effective_date' AS field,
       COUNT(*) AS total,
       COUNT(*) FILTER (WHERE "txn_effective_date" ~ '^[0-9]{8}$'
                          AND "txn_effective_date"::DATE IS NOT NULL) AS valid_dates,
       COUNT(*) FILTER (WHERE "txn_effective_date" !~ '^[0-9]{8}$'
                          OR LENGTH(TRIM("txn_effective_date")) = 0) AS invalid_dates
FROM public.txnin;

-- ═══ SECTION 7: Numeric Overflow Check ═══

-- Verify that no values exceed the column's declared precision.

SELECT 'acctfile.acct_balance' AS field,
       'NUMERIC(15,2)' AS declared_type,
       COUNT(*) FILTER (WHERE ABS("acct_balance") > 9999999999999.99) AS overflow_count
FROM public.acctfile;
SELECT 'acctfile.acct_avail_bal' AS field,
       'NUMERIC(15,2)' AS declared_type,
       COUNT(*) FILTER (WHERE ABS("acct_avail_bal") > 9999999999999.99) AS overflow_count
FROM public.acctfile;
SELECT 'acctfile.acct_int_rate' AS field,
       'NUMERIC(6,4)' AS declared_type,
       COUNT(*) FILTER (WHERE ABS("acct_int_rate") > 99.9999) AS overflow_count
FROM public.acctfile;
SELECT 'intjrnl.txn_amount' AS field,
       'NUMERIC(13,2)' AS declared_type,
       COUNT(*) FILTER (WHERE ABS("txn_amount") > 99999999999.99) AS overflow_count
FROM public.intjrnl;
SELECT 'intjrnl.txn_balance_after' AS field,
       'NUMERIC(15,2)' AS declared_type,
       COUNT(*) FILTER (WHERE ABS("txn_balance_after") > 9999999999999.99) AS overflow_count
FROM public.intjrnl;
SELECT 'txnin.txn_amount' AS field,
       'NUMERIC(13,2)' AS declared_type,
       COUNT(*) FILTER (WHERE ABS("txn_amount") > 99999999999.99) AS overflow_count
FROM public.txnin;
SELECT 'txnin.txn_balance_after' AS field,
       'NUMERIC(15,2)' AS declared_type,
       COUNT(*) FILTER (WHERE ABS("txn_balance_after") > 9999999999999.99) AS overflow_count
FROM public.txnin;
SELECT 'custdb_customer.riskscr' AS field,
       'NUMERIC(3,0)' AS declared_type,
       COUNT(*) FILTER (WHERE ABS("riskscr") > 999) AS overflow_count
FROM public.custdb_customer;

-- ═══ SECTION 8: Cross-Table Consistency ═══

-- Tables that share the same COBOL record layout should have matching schemas.

-- Tables sharing record layout AUDIT-RECORD:
SELECT 'auditvsam' AS "table", COUNT(*) AS rows FROM public.auditvsam;
SELECT 'auditout' AS "table", COUNT(*) AS rows FROM public.auditout;

-- Tables sharing record layout TXN-RECORD:
SELECT 'intjrnl' AS "table", COUNT(*) AS rows FROM public.intjrnl;
SELECT 'txnin' AS "table", COUNT(*) AS rows FROM public.txnin;


-- ═══ VALIDATION SUMMARY ═══

-- Total validation queries: 47
-- Run time will vary with data volume.
-- All 'status' columns should show 'OK'.
-- All 'orphaned_rows' should be 0.
-- All 'overflow_count' should be 0.
-- All 'invalid_dates' should be 0.