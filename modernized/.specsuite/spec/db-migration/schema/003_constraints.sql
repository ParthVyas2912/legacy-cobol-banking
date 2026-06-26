-- ═══════════════════════════════════════════════════════
-- Constraints (CHECK from 88-levels + FK from IMS hierarchy)
-- ═══════════════════════════════════════════════════════

ALTER TABLE public.acctfile ADD CONSTRAINT chk_acctfile_acct_type CHECK ("acct_type" IN ('CK', 'SV', 'LN')); -- 88-level conditions: ACCT-CHECKING, ACCT-SAVINGS, ACCT-LOAN
ALTER TABLE public.acctfile ADD CONSTRAINT chk_acctfile_acct_status CHECK ("acct_status" IN ('A', 'F', 'C')); -- 88-level conditions: ACCT-ACTIVE, ACCT-FROZEN, ACCT-CLOSED
ALTER TABLE public.acctfile ADD CONSTRAINT chk_acctfile_acct_flags CHECK ("acct_flags" IN ('YYYY', 'YYNN', 'YYYN')); -- 88-level conditions: ACCT-OVERDRAFT-OK
ALTER TABLE public.intjrnl ADD CONSTRAINT chk_intjrnl_txn_code CHECK ("txn_code" IN ('DEP', 'WDR', 'XFR', 'FEE', 'INT', 'REV')); -- 88-level conditions: TXN-DEPOSIT, TXN-WITHDRAWAL, TXN-TRANSFER, TXN-FEE, TXN-INTEREST, TXN-REVERSAL
ALTER TABLE public.intjrnl ADD CONSTRAINT chk_intjrnl_txn_channel CHECK ("txn_channel" IN ('ATM ', 'BRCH', 'ONLN', 'BTCH')); -- 88-level conditions: TXN-ATM, TXN-BRANCH, TXN-ONLINE, TXN-BATCH
ALTER TABLE public.intjrnl ADD CONSTRAINT chk_intjrnl_txn_status CHECK ("txn_status" IN ('P', 'O', 'F')); -- 88-level conditions: TXN-PENDING, TXN-POSTED, TXN-FAILED
ALTER TABLE public.txnin ADD CONSTRAINT chk_txnin_txn_code CHECK ("txn_code" IN ('DEP', 'WDR', 'XFR', 'FEE', 'INT', 'REV')); -- 88-level conditions: TXN-DEPOSIT, TXN-WITHDRAWAL, TXN-TRANSFER, TXN-FEE, TXN-INTEREST, TXN-REVERSAL
ALTER TABLE public.txnin ADD CONSTRAINT chk_txnin_txn_channel CHECK ("txn_channel" IN ('ATM ', 'BRCH', 'ONLN', 'BTCH')); -- 88-level conditions: TXN-ATM, TXN-BRANCH, TXN-ONLINE, TXN-BATCH
ALTER TABLE public.txnin ADD CONSTRAINT chk_txnin_txn_status CHECK ("txn_status" IN ('P', 'O', 'F')); -- 88-level conditions: TXN-PENDING, TXN-POSTED, TXN-FAILED
ALTER TABLE public.custdb_acctlnk ADD CONSTRAINT fk_custdb_acctlnk_customer FOREIGN KEY ("customer_id") REFERENCES public.custdb_customer ("customer_id");
ALTER TABLE public.custdb_address ADD CONSTRAINT fk_custdb_address_customer FOREIGN KEY ("customer_id") REFERENCES public.custdb_customer ("customer_id");