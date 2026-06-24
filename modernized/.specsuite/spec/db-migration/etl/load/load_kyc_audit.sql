-- MainframeForge — Load kyc_audit from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading kyc_audit...'

COPY public.kyc_audit
FROM '/data/migration/staging/kyc_audit.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'kyc_audit: ' || COUNT(*) || ' rows loaded' FROM public.kyc_audit;
