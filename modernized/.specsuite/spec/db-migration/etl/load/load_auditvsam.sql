-- MainframeForge — Load auditvsam from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading auditvsam...'

COPY public.auditvsam
FROM '/data/migration/staging/auditvsam.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'auditvsam: ' || COUNT(*) || ' rows loaded' FROM public.auditvsam;
