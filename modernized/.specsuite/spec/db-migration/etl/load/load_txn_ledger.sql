-- MainframeForge — Load txn_ledger from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading txn_ledger...'

COPY public.txn_ledger
FROM '/data/migration/staging/txn_ledger.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'txn_ledger: ' || COUNT(*) || ' rows loaded' FROM public.txn_ledger;
