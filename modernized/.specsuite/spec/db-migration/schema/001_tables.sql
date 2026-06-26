-- ═══════════════════════════════════════════════════════
-- MainframeForge — Data Migration Schema
-- Target: PostgreSQL
-- Schema: public
-- Generated: 2026-06-26T16:40:24.151Z
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
COMMENT ON COLUMN public.acctfile."acct_number" IS 'A unique identifier for the account, assigned at the time of account creation.';
COMMENT ON COLUMN public.acctfile."acct_customer_id" IS 'A unique identifier for the customer who owns the account, used to associate the account with its owner.';
COMMENT ON COLUMN public.acctfile."acct_opened_date" IS 'The date on which the account was opened, recorded in YYYYMMDD format.';
COMMENT ON COLUMN public.acctfile."acct_type" IS 'The type of the account, such as checking, savings, or loan, represented by predefined status codes.';
COMMENT ON COLUMN public.acctfile."acct_status" IS 'The current operational status of the account, such as active, frozen, or closed, indicated by predefined status codes.';
COMMENT ON COLUMN public.acctfile."acct_balance" IS 'The total balance in the account, stored as a packed decimal with two implied decimal places.';
COMMENT ON COLUMN public.acctfile."acct_avail_bal" IS 'The available balance in the account, excluding any holds, stored as a packed decimal with two implied decimal places.';
COMMENT ON COLUMN public.acctfile."acct_int_rate" IS 'The interest rate applied to the account, represented as a packed decimal with two whole digits and four fractional digits.';
COMMENT ON COLUMN public.acctfile."acct_last_txn_dt" IS 'The date of the last transaction performed on the account, recorded in YYYYMMDD format.';
COMMENT ON COLUMN public.acctfile."acct_currency" IS 'The currency of the account, represented as a three-letter ISO code.';
COMMENT ON COLUMN public.acctfile."acct_branch_code" IS 'The identifier of the branch where the account is maintained, represented as a four-digit numeric code.';
COMMENT ON COLUMN public.acctfile."acct_flags" IS 'A set of control flags indicating specific settings or permissions on the account, such as overdraft permissions.';
COMMENT ON COLUMN public.acctfile."acct_filler" IS 'Reserved space for future expansion or alignment, currently unused within the record.';

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
COMMENT ON COLUMN public.auditvsam."audit_key" IS 'A unique identifier for the audit record used to distinguish each logged action in the system.';
COMMENT ON COLUMN public.auditvsam."audit_acct_number" IS 'The account number associated with the action being recorded in the audit trail.';
COMMENT ON COLUMN public.auditvsam."audit_action" IS 'A textual description of the specific action being logged, such as ''LOGIN'', ''UPDATE'', or ''DELETE''.';
COMMENT ON COLUMN public.auditvsam."audit_user" IS 'The user ID of the individual or system responsible for the action being logged.';
COMMENT ON COLUMN public.auditvsam."audit_ts" IS 'The timestamp indicating when the action was performed, recorded in a format that includes both date and time.';
COMMENT ON COLUMN public.auditvsam."audit_detail" IS 'Additional details or extended information about the action being logged, providing supplementary context where necessary.';


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
COMMENT ON COLUMN public.intjrnl."txn_id" IS 'A unique identifier assigned to each transaction for tracking purposes.';
COMMENT ON COLUMN public.intjrnl."txn_acct_number" IS 'The account number associated with the transaction.';
COMMENT ON COLUMN public.intjrnl."txn_post_date" IS 'The date on which the transaction is recorded in the system.';
COMMENT ON COLUMN public.intjrnl."txn_effective_date" IS 'The date on which the transaction takes effect for accounting purposes.';
COMMENT ON COLUMN public.intjrnl."txn_amount" IS 'The monetary amount of the transaction, potentially including two decimal places.';
COMMENT ON COLUMN public.intjrnl."txn_code" IS 'A code representing the type or category of the transaction, such as deposit, withdrawal, or fee.';
COMMENT ON COLUMN public.intjrnl."txn_channel" IS 'A code specifying the channel through which the transaction occurred, such as ATM or online.';
COMMENT ON COLUMN public.intjrnl."txn_ref_number" IS 'A reference number associated with the transaction, often for external or internal tracking.';
COMMENT ON COLUMN public.intjrnl."txn_description" IS 'A free-text field providing additional details or comments about the transaction.';
COMMENT ON COLUMN public.intjrnl."txn_status" IS 'A single-character code indicating the current state of the transaction, such as pending, posted, or failed.';
COMMENT ON COLUMN public.intjrnl."txn_balance_after" IS 'The account balance after the transaction has been applied, including two decimal places.';
COMMENT ON COLUMN public.intjrnl."txn_user_id" IS 'A user identifier representing the person or system responsible for the transaction.';

-- Source: STMTOUT (record: STMT-LINE) — used by: STMTRPT
DROP TABLE IF EXISTS public.stmtout CASCADE;
CREATE TABLE public.stmtout (
  "stmt_acct_num" VARCHAR(10),
  "stmt_description" VARCHAR(40),
  "stmt_amount" NUMERIC(3, 0),
  "stmt_balance" NUMERIC(3, 0)
);
COMMENT ON COLUMN public.stmtout."stmt_acct_num" IS 'The unique account number associated with the statement, identifying the customer or entity tied to the transaction.';
COMMENT ON COLUMN public.stmtout."stmt_description" IS 'A free-text field providing a description of the transaction or statement line, typically explaining the nature of the transaction.';
COMMENT ON COLUMN public.stmtout."stmt_amount" IS 'The monetary amount for the specific transaction in the statement line. This may represent a credit or debit.';
COMMENT ON COLUMN public.stmtout."stmt_balance" IS 'The resulting account balance after applying the transaction amount to the previous balance.';

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
COMMENT ON COLUMN public.txnin."txn_id" IS 'Identifier for the transaction, unique within the input dataset, used for tracking and database operations.';
COMMENT ON COLUMN public.txnin."txn_acct_number" IS 'Identifier for the customer account associated with the transaction.';
COMMENT ON COLUMN public.txnin."txn_post_date" IS 'The date when the transaction is posted or recorded in the system.';
COMMENT ON COLUMN public.txnin."txn_effective_date" IS 'The date when the transaction takes effect, which could differ from the posting date.';
COMMENT ON COLUMN public.txnin."txn_amount" IS 'Monetary amount associated with the transaction, represented with two decimal places.';
COMMENT ON COLUMN public.txnin."txn_code" IS 'Type or category of the transaction, such as deposit, withdrawal, or fee. Enum values are provided for specific categories.';
COMMENT ON COLUMN public.txnin."txn_channel" IS 'The channel through which the transaction was initiated (e.g., ATM, branch, online). Enum values are provided for the valid channels.';
COMMENT ON COLUMN public.txnin."txn_ref_number" IS 'Free-text reference or confirmation number for the transaction, often provided by external systems or customers.';
COMMENT ON COLUMN public.txnin."txn_description" IS 'Free-text description providing details about the transaction, intended for display or audit purposes.';
COMMENT ON COLUMN public.txnin."txn_status" IS 'Current processing status of the transaction, such as pending, posted, or failed. Enum values define valid statuses.';
COMMENT ON COLUMN public.txnin."txn_balance_after" IS 'The account balance after applying this transaction, represented with two decimal places.';
COMMENT ON COLUMN public.txnin."txn_user_id" IS 'Identifier for the user or system responsible for entering or initiating the transaction.';

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
