-- ═══════════════════════════════════════════════════════
-- MainframeForge — Post-Migration Validation Queries
-- ═══════════════════════════════════════════════════════
--
-- Run these after data loading to verify migration completeness.
-- Compare results against mainframe source row counts.

-- ─── Row counts ───

SELECT 'acctfile' AS "table", COUNT(*) AS row_count FROM public.acctfile;
SELECT 'auditvsam' AS "table", COUNT(*) AS row_count FROM public.auditvsam;
SELECT 'intjrnl' AS "table", COUNT(*) AS row_count FROM public.intjrnl;
SELECT 'stmtout' AS "table", COUNT(*) AS row_count FROM public.stmtout;
SELECT 'txnin' AS "table", COUNT(*) AS row_count FROM public.txnin;
SELECT 'auditout' AS "table", COUNT(*) AS row_count FROM public.auditout;
SELECT 'custdb_customer' AS "table", COUNT(*) AS row_count FROM public.custdb_customer;
SELECT 'custdb_acctlnk' AS "table", COUNT(*) AS row_count FROM public.custdb_acctlnk;
SELECT 'custdb_address' AS "table", COUNT(*) AS row_count FROM public.custdb_address;
SELECT 'acct_audit' AS "table", COUNT(*) AS row_count FROM public.acct_audit;
SELECT 'customer' AS "table", COUNT(*) AS row_count FROM public.customer;
SELECT 'eod_runs' AS "table", COUNT(*) AS row_count FROM public.eod_runs;
SELECT 'txn_ledger' AS "table", COUNT(*) AS row_count FROM public.txn_ledger;
SELECT 'fraud_flags' AS "table", COUNT(*) AS row_count FROM public.fraud_flags;
SELECT 'kyc_audit' AS "table", COUNT(*) AS row_count FROM public.kyc_audit;

-- ─── Monetary balance reconciliation ───
-- Compare SUM of balance fields against mainframe totals.

SELECT 'acctfile.acct_balance' AS field,
       SUM("acct_balance") AS total,
       COUNT(*) AS non_null_count
FROM public.acctfile
WHERE "acct_balance" IS NOT NULL;

SELECT 'acctfile.acct_avail_bal' AS field,
       SUM("acct_avail_bal") AS total,
       COUNT(*) AS non_null_count
FROM public.acctfile
WHERE "acct_avail_bal" IS NOT NULL;

SELECT 'acctfile.acct_int_rate' AS field,
       SUM("acct_int_rate") AS total,
       COUNT(*) AS non_null_count
FROM public.acctfile
WHERE "acct_int_rate" IS NOT NULL;

SELECT 'intjrnl.txn_amount' AS field,
       SUM("txn_amount") AS total,
       COUNT(*) AS non_null_count
FROM public.intjrnl
WHERE "txn_amount" IS NOT NULL;

SELECT 'intjrnl.txn_balance_after' AS field,
       SUM("txn_balance_after") AS total,
       COUNT(*) AS non_null_count
FROM public.intjrnl
WHERE "txn_balance_after" IS NOT NULL;

SELECT 'stmtout.stmt_amount' AS field,
       SUM("stmt_amount") AS total,
       COUNT(*) AS non_null_count
FROM public.stmtout
WHERE "stmt_amount" IS NOT NULL;

SELECT 'stmtout.stmt_balance' AS field,
       SUM("stmt_balance") AS total,
       COUNT(*) AS non_null_count
FROM public.stmtout
WHERE "stmt_balance" IS NOT NULL;

SELECT 'txnin.txn_amount' AS field,
       SUM("txn_amount") AS total,
       COUNT(*) AS non_null_count
FROM public.txnin
WHERE "txn_amount" IS NOT NULL;

SELECT 'txnin.txn_balance_after' AS field,
       SUM("txn_balance_after") AS total,
       COUNT(*) AS non_null_count
FROM public.txnin
WHERE "txn_balance_after" IS NOT NULL;

-- ─── Primary key uniqueness verification ───

SELECT 'acctfile' AS "table",
       COUNT(*) AS total_rows,
       COUNT(DISTINCT ("acct_number")) AS distinct_keys,
       CASE WHEN COUNT(*) = COUNT(DISTINCT ("acct_number")) THEN 'OK' ELSE 'DUPLICATE KEYS' END AS status
FROM public.acctfile;

SELECT 'auditvsam' AS "table",
       COUNT(*) AS total_rows,
       COUNT(DISTINCT ("audit_key")) AS distinct_keys,
       CASE WHEN COUNT(*) = COUNT(DISTINCT ("audit_key")) THEN 'OK' ELSE 'DUPLICATE KEYS' END AS status
FROM public.auditvsam;

SELECT 'custdb_customer' AS "table",
       COUNT(*) AS total_rows,
       COUNT(DISTINCT ("customer_id")) AS distinct_keys,
       CASE WHEN COUNT(*) = COUNT(DISTINCT ("customer_id")) THEN 'OK' ELSE 'DUPLICATE KEYS' END AS status
FROM public.custdb_customer;

SELECT 'custdb_acctlnk' AS "table",
       COUNT(*) AS total_rows,
       COUNT(DISTINCT ("acctlnk_id")) AS distinct_keys,
       CASE WHEN COUNT(*) = COUNT(DISTINCT ("acctlnk_id")) THEN 'OK' ELSE 'DUPLICATE KEYS' END AS status
FROM public.custdb_acctlnk;

SELECT 'custdb_address' AS "table",
       COUNT(*) AS total_rows,
       COUNT(DISTINCT ("address_id")) AS distinct_keys,
       CASE WHEN COUNT(*) = COUNT(DISTINCT ("address_id")) THEN 'OK' ELSE 'DUPLICATE KEYS' END AS status
FROM public.custdb_address;

-- ─── Foreign key referential integrity ───

-- FK: custdb_acctlnk.customer_id → custdb_customer.customer_id
SELECT 'custdb_acctlnk → custdb_customer' AS fk_check,
       COUNT(*) AS orphaned_rows
FROM public.custdb_acctlnk c
LEFT JOIN public.custdb_customer p ON c."customer_id" = p."customer_id"
WHERE p."customer_id" IS NULL;

-- FK: custdb_address.customer_id → custdb_customer.customer_id
SELECT 'custdb_address → custdb_customer' AS fk_check,
       COUNT(*) AS orphaned_rows
FROM public.custdb_address c
LEFT JOIN public.custdb_customer p ON c."customer_id" = p."customer_id"
WHERE p."customer_id" IS NULL;
