#!/usr/bin/env bash
# MainframeForge — Validation runner
#
# Usage: ./run_validation.sh <db-url> [--source-dir <staging-dir>]
#
set -euo pipefail

DB_URL="${1:?Usage: $0 <db-url> [--source-dir <staging-dir>]}"
SOURCE_DIR="${3:-/data/migration/staging}"
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

echo "═══════════════════════════════════════════════════════"
echo " MainframeForge — Post-Migration Validation"
echo "═══════════════════════════════════════════════════════"
echo ""

echo "Step 1: SQL validation queries..."
psql "$DB_URL" -f "$SCRIPT_DIR/validate_migration.sql"

echo ""
echo "Step 2: Record-level spot-check..."
python3 "$SCRIPT_DIR/spot_check.py" --source-dir "$SOURCE_DIR" --db-url "$DB_URL" --sample 100

echo ""
echo "═══════════════════════════════════════════════════════"
echo " Validation complete."
echo "═══════════════════════════════════════════════════════"