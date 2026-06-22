-- ═══════════════════════════════════════════════════════
-- MainframeForge — Data Migration Schema
-- Target: PostgreSQL
-- Schema: public
-- Generated: 2026-06-22T17:08:24.816Z
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
COMMENT ON COLUMN public.acctfile."acct_number" IS 'A 10-digit unique identifier assigned to the account for tracking and operations.';
COMMENT ON COLUMN public.acctfile."acct_customer_id" IS 'An 8-digit identifier linking the account to a specific customer in the system.';
COMMENT ON COLUMN public.acctfile."acct_opened_date" IS 'The date the account was opened, represented as an 8-digit number in YYYYMMDD format.';
COMMENT ON COLUMN public.acctfile."acct_type" IS 'A 2-character code representing the type of account, such as checking, savings, or loan, using predefined values.';
COMMENT ON COLUMN public.acctfile."acct_status" IS 'A 1-character code indicating the current status of the account (e.g., active, frozen, closed).';
COMMENT ON COLUMN public.acctfile."acct_balance" IS 'The current balance of the account, stored as a signed 15-digit packed-decimal number with two decimal places.';
COMMENT ON COLUMN public.acctfile."acct_avail_bal" IS 'The available balance for transactions, represented as a signed packed-decimal with two decimal places.';
COMMENT ON COLUMN public.acctfile."acct_int_rate" IS 'The interest rate applicable to the account, stored as a packed-decimal with four decimal places.';
COMMENT ON COLUMN public.acctfile."acct_last_txn_dt" IS 'The date of the last transaction on the account, represented as an 8-digit number in YYYYMMDD format.';
COMMENT ON COLUMN public.acctfile."acct_currency" IS 'A 3-character code for the currency type used in the account (e.g., USD, EUR).';
COMMENT ON COLUMN public.acctfile."acct_branch_code" IS 'A 4-digit numeric code identifying the branch where the account was opened or is maintained.';
COMMENT ON COLUMN public.acctfile."acct_flags" IS 'A 4-character flag field indicating operational permissions and settings, such as overdraft allowance.';
COMMENT ON COLUMN public.acctfile."acct_filler" IS 'Reserved filler space for future use or padding, consisting of 50 unused characters.';

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
COMMENT ON COLUMN public.auditvsam."audit_key" IS 'A unique identifier for the audit record, used to index and retrieve the entry from the audit file.';
COMMENT ON COLUMN public.auditvsam."audit_acct_number" IS 'An identifier representing the account associated with this audit action, linking the activity to a specific account.';
COMMENT ON COLUMN public.auditvsam."audit_action" IS 'Describes the type of action or event being logged in the audit record, such as create, update, or delete operations.';
COMMENT ON COLUMN public.auditvsam."audit_user" IS 'The identifier of the user or process responsible for the recorded action.';
COMMENT ON COLUMN public.auditvsam."audit_ts" IS 'The timestamp indicating when the event or action occurred, providing temporal context for the record.';
COMMENT ON COLUMN public.auditvsam."audit_detail" IS 'Additional free-text information providing further details about the action or event, allowing for a more descriptive audit trail.';


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
COMMENT ON COLUMN public.intjrnl."txn_id" IS 'A unique identifier assigned to each transaction for tracking and auditing purposes.';
COMMENT ON COLUMN public.intjrnl."txn_acct_number" IS 'The numeric identifier for the account associated with the transaction.';
COMMENT ON COLUMN public.intjrnl."txn_post_date" IS 'The date on which the transaction is posted to the account, which may differ from the effective date.';
COMMENT ON COLUMN public.intjrnl."txn_effective_date" IS 'The date when the transaction takes effect, reflecting the value date of the financial change.';
COMMENT ON COLUMN public.intjrnl."txn_amount" IS 'The monetary value of the transaction, including two decimal precision for cents.';
COMMENT ON COLUMN public.intjrnl."txn_code" IS 'An enumeration representing the type of transaction, such as deposit, withdrawal, or fee.';
COMMENT ON COLUMN public.intjrnl."txn_channel" IS 'An enumeration indicating the channel through which the transaction was conducted, such as ATM or online.';
COMMENT ON COLUMN public.intjrnl."txn_ref_number" IS 'A free-text reference number associated with the transaction, often used for external system tracking or reconciliation.';
COMMENT ON COLUMN public.intjrnl."txn_description" IS 'A free-text field providing descriptive information about the transaction, often user or system generated.';
COMMENT ON COLUMN public.intjrnl."txn_status" IS 'A status code indicating the current processing state of the transaction, such as pending, posted, or failed.';
COMMENT ON COLUMN public.intjrnl."txn_balance_after" IS 'The account balance immediately following the application of the transaction, in monetary units.';
COMMENT ON COLUMN public.intjrnl."txn_user_id" IS 'The identifier of the user or system agent responsible for initiating or processing the transaction.';

-- Source: STMTOUT (record: STMT-LINE) — used by: STMTRPT
DROP TABLE IF EXISTS public.stmtout CASCADE;
CREATE TABLE public.stmtout (
  "stmt_acct_num" VARCHAR(10),
  "stmt_description" VARCHAR(40),
  "stmt_amount" NUMERIC(3, 0),
  "stmt_balance" NUMERIC(3, 0)
);
COMMENT ON COLUMN public.stmtout."stmt_acct_num" IS 'A unique numerical identifier for the customer account associated with this statement line.';
COMMENT ON COLUMN public.stmtout."stmt_description" IS 'A free-text description providing additional details regarding the transaction or entry on the statement.';
COMMENT ON COLUMN public.stmtout."stmt_amount" IS 'The amount of the individual transaction or adjustment recorded in this statement line.';
COMMENT ON COLUMN public.stmtout."stmt_balance" IS 'The running balance of the account after applying the associated transaction or adjustment on this statement line.';

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
  "txn_user_id" VARCHAR(8)
);
COMMENT ON COLUMN public.txnin."txn_id" IS 'A unique identifier for the transaction within the system for tracking and reference purposes.';
COMMENT ON COLUMN public.txnin."txn_acct_number" IS 'The account number associated with this transaction, identifying which account the transaction applies to.';
COMMENT ON COLUMN public.txnin."txn_post_date" IS 'The date on which the transaction was recorded in the ledger or system.';
COMMENT ON COLUMN public.txnin."txn_effective_date" IS 'The date on which the transaction is considered to have occurred, which may differ from the posting date.';
COMMENT ON COLUMN public.txnin."txn_amount" IS 'The monetary value of the transaction, including deposits, withdrawals, or other adjustments to the account.';
COMMENT ON COLUMN public.txnin."txn_code" IS 'The type of transaction, such as deposit, withdrawal, or transfer, represented by specific codes.';
COMMENT ON COLUMN public.txnin."txn_channel" IS 'The channel through which the transaction was made, such as ATM, branch, or online.';
COMMENT ON COLUMN public.txnin."txn_ref_number" IS 'A reference number for the transaction, which may be used for reconciliation or audit trails.';
COMMENT ON COLUMN public.txnin."txn_description" IS 'A textual description of the transaction, providing additional details about its purpose or processing.';
COMMENT ON COLUMN public.txnin."txn_status" IS 'The current status of the transaction, such as pending, posted, or failed.';
COMMENT ON COLUMN public.txnin."txn_balance_after" IS 'The account''s balance immediately following the processing of this transaction.';
COMMENT ON COLUMN public.txnin."txn_user_id" IS 'The identifier of the user or system that initiated or processed the transaction.';

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
