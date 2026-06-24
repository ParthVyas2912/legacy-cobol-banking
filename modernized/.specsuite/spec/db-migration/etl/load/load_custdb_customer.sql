-- MainframeForge — Load custdb_customer from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading custdb_customer...'

COPY public.custdb_customer
FROM '/data/migration/staging/custdb_customer.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'custdb_customer: ' || COUNT(*) || ' rows loaded' FROM public.custdb_customer;
