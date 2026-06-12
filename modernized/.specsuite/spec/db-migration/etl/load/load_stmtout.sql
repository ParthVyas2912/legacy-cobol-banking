-- MainframeForge — Load stmtout from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading stmtout...'

COPY public.stmtout
FROM '/data/migration/staging/stmtout.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'stmtout: ' || COUNT(*) || ' rows loaded' FROM public.stmtout;
