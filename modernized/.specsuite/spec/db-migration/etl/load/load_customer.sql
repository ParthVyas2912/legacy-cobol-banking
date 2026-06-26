-- MainframeForge — Load customer from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading customer...'

COPY public.customer
FROM '/data/migration/staging/customer.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'customer: ' || COUNT(*) || ' rows loaded' FROM public.customer;
