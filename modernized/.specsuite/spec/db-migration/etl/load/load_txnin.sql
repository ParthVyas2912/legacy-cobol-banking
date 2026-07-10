-- MainframeForge — Load txnin from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading txnin...'

COPY public.txnin
FROM '/data/migration/staging/txnin.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'txnin: ' || COUNT(*) || ' rows loaded' FROM public.txnin;
