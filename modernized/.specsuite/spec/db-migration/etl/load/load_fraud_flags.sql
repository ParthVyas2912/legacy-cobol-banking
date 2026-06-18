-- MainframeForge — Load fraud_flags from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading fraud_flags...'

COPY public.fraud_flags
FROM '/data/migration/staging/fraud_flags.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'fraud_flags: ' || COUNT(*) || ' rows loaded' FROM public.fraud_flags;
