-- ═══════════════════════════════════════════════════════
-- Indexes (from VSAM alternate keys + IMS search keys)
-- ═══════════════════════════════════════════════════════

CREATE INDEX idx_auditvsam_audit_acct_number ON public.auditvsam ("audit_acct_number");
CREATE UNIQUE INDEX idx_custdb_customer_custid ON public.custdb_customer ("custid");
CREATE UNIQUE INDEX idx_custdb_acctlnk_acctnum ON public.custdb_acctlnk ("acctnum");
CREATE UNIQUE INDEX idx_custdb_address_effdate ON public.custdb_address ("effdate");