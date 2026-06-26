-- ═══════════════════════════════════════════════════════
-- Enum types (from COBOL 88-level condition names)
-- ═══════════════════════════════════════════════════════

CREATE TYPE acctfile_acct_type_enum AS ENUM ('CK', 'SV', 'LN');
CREATE TYPE acctfile_acct_status_enum AS ENUM ('A', 'F', 'C');
CREATE TYPE acctfile_acct_flags_enum AS ENUM ('YYYY', 'YYNN', 'YYYN');
CREATE TYPE intjrnl_txn_code_enum AS ENUM ('DEP', 'WDR', 'XFR', 'FEE', 'INT', 'REV');
CREATE TYPE intjrnl_txn_channel_enum AS ENUM ('ATM ', 'BRCH', 'ONLN', 'BTCH');
CREATE TYPE intjrnl_txn_status_enum AS ENUM ('P', 'O', 'F');
CREATE TYPE txnin_txn_code_enum AS ENUM ('DEP', 'WDR', 'XFR', 'FEE', 'INT', 'REV');
CREATE TYPE txnin_txn_channel_enum AS ENUM ('ATM ', 'BRCH', 'ONLN', 'BTCH');
CREATE TYPE txnin_txn_status_enum AS ENUM ('P', 'O', 'F');