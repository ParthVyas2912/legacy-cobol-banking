-- MainframeForge — Load acct_audit from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading acct_audit...'

COPY public.acct_audit
FROM '/data/migration/staging/acct_audit.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'acct_audit: ' || COUNT(*) || ' rows loaded' FROM public.acct_audit;
