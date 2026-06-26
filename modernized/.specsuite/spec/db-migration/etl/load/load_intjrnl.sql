-- MainframeForge — Load intjrnl from converted CSV
-- Run after the Python converter has produced the CSV.

\echo 'Loading intjrnl...'

COPY public.intjrnl
FROM '/data/migration/staging/intjrnl.csv'
WITH (
  FORMAT csv,
  HEADER true,
  DELIMITER '|',
  NULL '',
  ENCODING 'UTF8'
);

SELECT 'intjrnl: ' || COUNT(*) || ' rows loaded' FROM public.intjrnl;
