-- MainframeForge — Load auditout from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading auditout...'

COPY public.auditout
FROM '/data/migration/staging/auditout.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'auditout: ' || COUNT(*) || ' rows loaded' FROM public.auditout;
