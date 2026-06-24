-- MainframeForge — Load custdb_acctlnk from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading custdb_acctlnk...'

COPY public.custdb_acctlnk
FROM '/data/migration/staging/custdb_acctlnk.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'custdb_acctlnk: ' || COUNT(*) || ' rows loaded' FROM public.custdb_acctlnk;
