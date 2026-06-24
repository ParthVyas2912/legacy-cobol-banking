-- MainframeForge — Load acctfile from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading acctfile...'

COPY public.acctfile
FROM '/data/migration/staging/acctfile.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'acctfile: ' || COUNT(*) || ' rows loaded' FROM public.acctfile;
