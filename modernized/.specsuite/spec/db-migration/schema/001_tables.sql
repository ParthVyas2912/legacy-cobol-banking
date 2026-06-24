-- ═══════════════════════════════════════════════════════
-- MainframeForge — Data Migration Schema
-- Target: PostgreSQL
-- Schema: public
-- Generated: 2026-06-24T02:49:46.165Z
-- Tables: 15
-- ═══════════════════════════════════════════════════════

CREATE SCHEMA IF NOT EXISTS public;


-- ─── VSAM files ───

-- Source: ACCTFILE (record: ACCT-RECORD) — used by: ACCTMGR, INTCALC
DROP TABLE IF EXISTS public.acctfile CASCADE;
CREATE TABLE public.acctfile (
  "acct_number" VARCHAR(10) NOT NULL,
  "acct_customer_id" VARCHAR(8),
  "acct_opened_date" DATE,
  "acct_type" VARCHAR(2),
  "acct_status" VARCHAR(1),
  "acct_balance" NUMERIC(15, 2),
  "acct_avail_bal" NUMERIC(15, 2),
  "acct_int_rate" NUMERIC(6, 4),
  "acct_last_txn_dt" DATE,
  "acct_currency" VARCHAR(3),
  "acct_branch_code" VARCHAR(4),
  "acct_flags" VARCHAR(4),
  "acct_filler" VARCHAR(50),
  PRIMARY KEY ("acct_number")
);
COMMENT ON COLUMN public.acctfile."acct_number" IS 'Unique identifier representing a specific account in the system, assigned during account creation.';
COMMENT ON COLUMN public.acctfile."acct_customer_id" IS 'Identifier linking the account to a specific customer, facilitating customer-account relationships.';
COMMENT ON COLUMN public.acctfile."acct_opened_date" IS 'The date the account was officially opened and registered within the system.';
COMMENT ON COLUMN public.acctfile."acct_type" IS 'Code representing the type of account (e.g., Checking, Savings, or Loan) based on the predefined set of account categories.';
COMMENT ON COLUMN public.acctfile."acct_status" IS 'The current status of the account, indicating whether it is active, frozen, or closed.';
COMMENT ON COLUMN public.acctfile."acct_balance" IS 'The account''s current balance, including all credits and debits, stored with two decimal digits.';
COMMENT ON COLUMN public.acctfile."acct_avail_bal" IS 'The balance available for use in the account, accounting for any holds or pending transactions.';
COMMENT ON COLUMN public.acctfile."acct_int_rate" IS 'The interest rate applicable to the account, stored as a fixed-point number with six total decimal places.';
COMMENT ON COLUMN public.acctfile."acct_last_txn_dt" IS 'The date of the last transaction recorded on the account, used for activity tracking.';
COMMENT ON COLUMN public.acctfile."acct_currency" IS 'The currency in which the account transactions are denominated, represented by a three-character code (e.g., USD, EUR).';
COMMENT ON COLUMN public.acctfile."acct_branch_code" IS 'Numeric code identifying the bank branch where the account is registered or primarily managed.';
COMMENT ON COLUMN public.acctfile."acct_flags" IS 'Control flags associated with account-specific permissions or features, such as overdraft allowance.';
COMMENT ON COLUMN public.acctfile."acct_filler" IS 'Reserved field for future use or alignment purposes, with no meaningful data at present.';

-- Source: AUDITVSAM (record: AUDIT-RECORD) — used by: AUDITSRV
DROP TABLE IF EXISTS public.auditvsam CASCADE;
CREATE TABLE public.auditvsam (
  "audit_key" VARCHAR(20) NOT NULL,
  "audit_acct_number" VARCHAR(10),
  "audit_action" VARCHAR(16),
  "audit_user" VARCHAR(8),
  "audit_ts" TIMESTAMPTZ,
  "audit_detail" VARCHAR(120),
  PRIMARY KEY ("audit_key")
);
COMMENT ON COLUMN public.auditvsam."audit_key" IS 'A unique identifier for this audit record, used to ensure each entry in the log is distinct and locatable.';
COMMENT ON COLUMN public.auditvsam."audit_acct_number" IS 'The account number associated with the action being logged, which allows for tracking changes or activities tied to specific accounts.';
COMMENT ON COLUMN public.auditvsam."audit_action" IS 'The description of the action performed, providing semantic information about what event occurred (e.g., ''LOGIN'', ''UPDATE'').';
COMMENT ON COLUMN public.auditvsam."audit_user" IS 'The user ID of the individual or system responsible for performing the logged action, helping to identify accountability.';
COMMENT ON COLUMN public.auditvsam."audit_ts" IS 'The exact timestamp when the action occurred, allowing events to be ordered and correlated with precision.';
COMMENT ON COLUMN public.auditvsam."audit_detail" IS 'Additional details or contextual data about the action being audited, which may assist in understanding or analyzing the logged event.';


-- ─── QSAM flat files ───

-- Source: INTJRNL (record: TXN-RECORD) — used by: INTCALC
DROP TABLE IF EXISTS public.intjrnl CASCADE;
CREATE TABLE public.intjrnl (
  "txn_id" VARCHAR(12),
  "txn_acct_number" VARCHAR(10),
  "txn_post_date" DATE,
  "txn_effective_date" DATE,
  "txn_amount" NUMERIC(13, 2),
  "txn_code" VARCHAR(3),
  "txn_channel" VARCHAR(4),
  "txn_ref_number" VARCHAR(20),
  "txn_description" VARCHAR(40),
  "txn_status" VARCHAR(1),
  "txn_balance_after" NUMERIC(15, 2),
  "txn_user_id" VARCHAR(8)
);
COMMENT ON COLUMN public.intjrnl."txn_id" IS 'A unique identifier for the transaction.';
COMMENT ON COLUMN public.intjrnl."txn_acct_number" IS 'The account number associated with the transaction.';
COMMENT ON COLUMN public.intjrnl."txn_post_date" IS 'The date when the transaction was posted in the system.';
COMMENT ON COLUMN public.intjrnl."txn_effective_date" IS 'The effective date of the transaction, which may differ from the posting date.';
COMMENT ON COLUMN public.intjrnl."txn_amount" IS 'The monetary amount of the transaction with up to two decimal places.';
COMMENT ON COLUMN public.intjrnl."txn_code" IS 'The type of transaction, such as deposit, withdrawal, or fee, defined by specific codes.';
COMMENT ON COLUMN public.intjrnl."txn_channel" IS 'The channel through which the transaction was made, such as ATM, branch, or online.';
COMMENT ON COLUMN public.intjrnl."txn_ref_number" IS 'A reference number associated with the transaction for additional identification or tracing purposes.';
COMMENT ON COLUMN public.intjrnl."txn_description" IS 'A free-text description providing additional context or details about the transaction.';
COMMENT ON COLUMN public.intjrnl."txn_status" IS 'The current status of the transaction, such as pending, posted, or failed.';
COMMENT ON COLUMN public.intjrnl."txn_balance_after" IS 'The account balance immediately after this transaction, with up to two decimal places.';
COMMENT ON COLUMN public.intjrnl."txn_user_id" IS 'The user ID of the person or system that performed the transaction.';

-- Source: STMTOUT (record: STMT-LINE) — used by: STMTRPT
DROP TABLE IF EXISTS public.stmtout CASCADE;
CREATE TABLE public.stmtout (
  "stmt_acct_num" VARCHAR(10),
  "stmt_description" VARCHAR(40),
  "stmt_amount" NUMERIC(3, 0),
  "stmt_balance" NUMERIC(3, 0)
);
COMMENT ON COLUMN public.stmtout."stmt_acct_num" IS 'The unique identifier for the customer''s account associated with this statement line.';
COMMENT ON COLUMN public.stmtout."stmt_description" IS 'A free-text description providing details about the transaction or activity being recorded in this statement line.';
COMMENT ON COLUMN public.stmtout."stmt_amount" IS 'The monetary amount associated with the transaction recorded in this statement line.';
COMMENT ON COLUMN public.stmtout."stmt_balance" IS 'The customer''s account balance after the transaction recorded in this statement line has been applied.';

-- Source: TXNIN (record: TXN-RECORD) — used by: TXNPOST
DROP TABLE IF EXISTS public.txnin CASCADE;
CREATE TABLE public.txnin (
  "txn_id" VARCHAR(12),
  "txn_acct_number" VARCHAR(10),
  "txn_post_date" DATE,
  "txn_effective_date" DATE,
  "txn_amount" NUMERIC(13, 2),
  "txn_code" VARCHAR(3),
  "txn_channel" VARCHAR(4),
  "txn_ref_number" VARCHAR(20),
  "txn_description" VARCHAR(40),
  "txn_status" VARCHAR(1),
  "txn_balance_after" NUMERIC(15, 2),
  "txn_user_id" TIMESTAMPTZ
);
COMMENT ON COLUMN public.txnin."txn_id" IS 'A unique identifier for the transaction.';
COMMENT ON COLUMN public.txnin."txn_acct_number" IS 'The account number associated with the transaction, identifying which account was credited or debited.';
COMMENT ON COLUMN public.txnin."txn_post_date" IS 'The date the transaction was posted to the ledger.';
COMMENT ON COLUMN public.txnin."txn_effective_date" IS 'The date the transaction is marked as effective for account balances and records.';
COMMENT ON COLUMN public.txnin."txn_amount" IS 'The monetary value of the transaction, including two implied decimal places.';
COMMENT ON COLUMN public.txnin."txn_code" IS 'A code indicating the type of transaction, such as deposit, withdrawal, or transfer.';
COMMENT ON COLUMN public.txnin."txn_channel" IS 'The channel through which the transaction was initiated, such as ATM or branch.';
COMMENT ON COLUMN public.txnin."txn_ref_number" IS 'A reference number associated with the transaction, used for tracking and reconciliation purposes.';
COMMENT ON COLUMN public.txnin."txn_description" IS 'A brief free-text description of the transaction, such as a note or explanation.';
COMMENT ON COLUMN public.txnin."txn_status" IS 'The status of the transaction, indicating whether it is pending, posted, or failed.';
COMMENT ON COLUMN public.txnin."txn_balance_after" IS 'The account balance immediately after the transaction, including two implied decimal places.';
COMMENT ON COLUMN public.txnin."txn_user_id" IS 'The user ID of the employee or agent who facilitated or posted the transaction.';

-- Source: AUDITOUT (record: AUDIT-RECORD) — used by: TXNPOST
DROP TABLE IF EXISTS public.auditout CASCADE;
CREATE TABLE public.auditout (

);


-- ─── IMS hierarchical database ───

-- Source: CUSTDB.CUSTOMER — used by: 
DROP TABLE IF EXISTS public.custdb_customer CASCADE;
CREATE TABLE public.custdb_customer (
  "customer_id" BIGSERIAL NOT NULL,
  "custid" VARCHAR(8) NOT NULL,
  "lastname" VARCHAR(30),
  "firstname" VARCHAR(20),
  "taxid" VARCHAR(9),
  "kycstat" CHAR(1),
  "riskscr" NUMERIC(3, 0),
  PRIMARY KEY ("customer_id")
);
COMMENT ON COLUMN public.custdb_customer."customer_id" IS 'Surrogate PK — replaces IMS hierarchical positioning for CUSTOMER';
COMMENT ON COLUMN public.custdb_customer."custid" IS 'IMS sequence field (was the segment search key)';

-- Source: CUSTDB.ACCTLNK — used by: 
DROP TABLE IF EXISTS public.custdb_acctlnk CASCADE;
CREATE TABLE public.custdb_acctlnk (
  "acctlnk_id" BIGSERIAL NOT NULL,
  "customer_id" BIGINT NOT NULL,
  "acctnum" VARCHAR(10) NOT NULL,
  "role" VARCHAR(2),
  PRIMARY KEY ("acctlnk_id")
);
COMMENT ON COLUMN public.custdb_acctlnk."acctlnk_id" IS 'Surrogate PK — replaces IMS hierarchical positioning for ACCTLNK';
COMMENT ON COLUMN public.custdb_acctlnk."customer_id" IS 'FK to parent segment CUSTOMER';
COMMENT ON COLUMN public.custdb_acctlnk."acctnum" IS 'IMS sequence field (was the segment search key)';

-- Source: CUSTDB.ADDRESS — used by: 
DROP TABLE IF EXISTS public.custdb_address CASCADE;
CREATE TABLE public.custdb_address (
  "address_id" BIGSERIAL NOT NULL,
  "customer_id" BIGINT NOT NULL,
  "effdate" VARCHAR(8) NOT NULL,
  "street" VARCHAR(40),
  "city" VARCHAR(25),
  "state" VARCHAR(2),
  "zip" VARCHAR(5),
  PRIMARY KEY ("address_id")
);
COMMENT ON COLUMN public.custdb_address."address_id" IS 'Surrogate PK — replaces IMS hierarchical positioning for ADDRESS';
COMMENT ON COLUMN public.custdb_address."customer_id" IS 'FK to parent segment CUSTOMER';
COMMENT ON COLUMN public.custdb_address."effdate" IS 'IMS sequence field (was the segment search key)';


-- ─── DB2 z/OS tables ───

-- Source: ACCT_AUDIT — used by: ACCTMGR
DROP TABLE IF EXISTS public.acct_audit CASCADE;
CREATE TABLE public.acct_audit (
  "acct_num" TEXT,
  "action" TEXT,
  "actor" TEXT,
  "ts" TIMESTAMPTZ,
  "acct_number" TEXT,
  "comm_function" TEXT
);
COMMENT ON COLUMN public.acct_audit."acct_num" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.acct_audit."action" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.acct_audit."actor" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.acct_audit."ts" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.acct_audit."acct_number" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: ACCT-NUMBER. Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.acct_audit."comm_function" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: COMM-FUNCTION. Review type mapping manually if z/OS catalog is available.';

-- Source: CUSTOMER — used by: ACCTMGR
DROP TABLE IF EXISTS public.customer CASCADE;
CREATE TABLE public.customer (
  "active_acct_count" TEXT,
  "acct_customer_id" TEXT
);
COMMENT ON COLUMN public.customer."active_acct_count" IS 'DB2 column — inferred from embedded SQL. Operations: UPDATE. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.customer."acct_customer_id" IS 'DB2 column — inferred from embedded SQL. Operations: UPDATE. Host vars: ACCT-CUSTOMER-ID. Review type mapping manually if z/OS catalog is available.';

-- Source: EOD_RUNS — used by: BCHEOD
DROP TABLE IF EXISTS public.eod_runs CASCADE;
CREATE TABLE public.eod_runs (
  "started_at" TIMESTAMPTZ,
  "status" TIMESTAMPTZ,
  "ended_at" TIMESTAMPTZ,
  "ws_status_code" TIMESTAMPTZ
);
COMMENT ON COLUMN public.eod_runs."started_at" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.eod_runs."status" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT, UPDATE. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.eod_runs."ended_at" IS 'DB2 column — inferred from embedded SQL. Operations: UPDATE. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.eod_runs."ws_status_code" IS 'DB2 column — inferred from embedded SQL. Operations: UPDATE. Host vars: WS-STATUS-CODE. Review type mapping manually if z/OS catalog is available.';

-- Source: TXN_LEDGER — used by: FRDCHK, TXNPOST
DROP TABLE IF EXISTS public.txn_ledger CASCADE;
CREATE TABLE public.txn_ledger (
  "count" TEXT,
  "coalesce" TEXT,
  "ws_velocity_count" TEXT,
  "ws_velocity_amt" TEXT,
  "txn_acct_number" TEXT,
  "ws_current_acct" TEXT,
  "txn_id" TEXT,
  "acct" TEXT,
  "amount" TEXT,
  "code" TEXT,
  "posted_at" TIMESTAMPTZ,
  "txn_amount" TEXT,
  "txn_code" TEXT
);
COMMENT ON COLUMN public.txn_ledger."count" IS 'DB2 column — inferred from embedded SQL. Operations: SELECT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.txn_ledger."coalesce" IS 'DB2 column — inferred from embedded SQL. Operations: SELECT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.txn_ledger."ws_velocity_count" IS 'DB2 column — inferred from embedded SQL. Operations: SELECT. Host vars: WS-VELOCITY-COUNT. Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.txn_ledger."ws_velocity_amt" IS 'DB2 column — inferred from embedded SQL. Operations: SELECT. Host vars: WS-VELOCITY-AMT. Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.txn_ledger."txn_acct_number" IS 'DB2 column — inferred from embedded SQL. Operations: SELECT, INSERT. Host vars: TXN-ACCT-NUMBER. Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.txn_ledger."ws_current_acct" IS 'DB2 column — inferred from embedded SQL. Operations: DECLARE-CURSOR. Host vars: WS-CURRENT-ACCT. Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.txn_ledger."txn_id" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: TXN-ID. Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.txn_ledger."acct" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.txn_ledger."amount" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.txn_ledger."code" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.txn_ledger."posted_at" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.txn_ledger."txn_amount" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: TXN-AMOUNT. Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.txn_ledger."txn_code" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: TXN-CODE. Review type mapping manually if z/OS catalog is available.';

-- Source: FRAUD_FLAGS — used by: FRDCHK
DROP TABLE IF EXISTS public.fraud_flags CASCADE;
CREATE TABLE public.fraud_flags (
  "acct" TEXT,
  "score" TEXT,
  "reason" TEXT,
  "ts" TIMESTAMPTZ,
  "txn_acct_number" TEXT,
  "ws_risk_score" TEXT
);
COMMENT ON COLUMN public.fraud_flags."acct" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.fraud_flags."score" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.fraud_flags."reason" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.fraud_flags."ts" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: (none). Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.fraud_flags."txn_acct_number" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: TXN-ACCT-NUMBER. Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.fraud_flags."ws_risk_score" IS 'DB2 column — inferred from embedded SQL. Operations: INSERT. Host vars: WS-RISK-SCORE. Review type mapping manually if z/OS catalog is available.';

-- Source: KYC_AUDIT — used by: KYCSYNC
DROP TABLE IF EXISTS public.kyc_audit CASCADE;
CREATE TABLE public.kyc_audit (
  "cust_id" TEXT,
  "cust_kyc_status" TIMESTAMPTZ,
  "cust_risk_score" TEXT
);
COMMENT ON COLUMN public.kyc_audit."cust_id" IS 'DB2 column — inferred from embedded SQL. Operations: MERGE. Host vars: CUST-ID. Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.kyc_audit."cust_kyc_status" IS 'DB2 column — inferred from embedded SQL. Operations: MERGE. Host vars: CUST-KYC-STATUS. Review type mapping manually if z/OS catalog is available.';
COMMENT ON COLUMN public.kyc_audit."cust_risk_score" IS 'DB2 column — inferred from embedded SQL. Operations: MERGE. Host vars: CUST-RISK-SCORE. Review type mapping manually if z/OS catalog is available.';
