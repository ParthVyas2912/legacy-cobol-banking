-- MainframeForge — Load custdb_address from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading custdb_address...'

COPY public.custdb_address
FROM '/data/migration/staging/custdb_address.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'custdb_address: ' || COUNT(*) || ' rows loaded' FROM public.custdb_address;
