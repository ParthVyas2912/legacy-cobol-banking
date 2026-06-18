-- MainframeForge — Load eod_runs from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading eod_runs...'

COPY public.eod_runs
FROM '/data/migration/staging/eod_runs.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'eod_runs: ' || COUNT(*) || ' rows loaded' FROM public.eod_runs;
