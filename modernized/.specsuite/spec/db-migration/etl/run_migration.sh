#!/usr/bin/env bash
# ═══════════════════════════════════════════════════════
# MainframeForge — Data Migration Orchestrator
# ═══════════════════════════════════════════════════════
#
# Sequences the full extraction → conversion → loading → validation
# pipeline. Run from the db-migration/etl/ directory.
#
# Prerequisites:
#   1. Mainframe UNLOAD JCLs have been submitted and completed
#   2. Unloaded flat files have been transferred to /data/migration/staging/
#   3. Python 3.8+ is installed with 'decimal' and 'csv' (stdlib)
#   4. Target database is running and schema has been created
#      (run ../schema/000_enums.sql → 001_tables.sql → 002_indexes.sql
#       → 003_constraints.sql first)
#
# Usage:
#   ./run_migration.sh [--db-url <connection-url>]
#
set -euo pipefail

STAGING_DIR="/data/migration/staging"
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
DB_URL="${1:-}"

echo "═══════════════════════════════════════════════════════"
echo " MainframeForge — Data Migration Pipeline"
echo "═══════════════════════════════════════════════════════"
echo ""
echo "Staging directory: $STAGING_DIR"
echo "Script directory:  $SCRIPT_DIR"
echo ""

# ─── Step 1: Convert mainframe flat files to CSV ───
echo "Step 1: Converting mainframe records to CSV..."
echo ""
echo "  Converting acctfile..."
python3 "$SCRIPT_DIR/convert/convert_acctfile.py" "$STAGING_DIR/acctfile.dat" "$STAGING_DIR/acctfile.csv"

echo "  Converting auditvsam..."
python3 "$SCRIPT_DIR/convert/convert_auditvsam.py" "$STAGING_DIR/auditvsam.dat" "$STAGING_DIR/auditvsam.csv"

echo "  Converting intjrnl..."
python3 "$SCRIPT_DIR/convert/convert_intjrnl.py" "$STAGING_DIR/intjrnl.dat" "$STAGING_DIR/intjrnl.csv"

echo "  Converting stmtout..."
python3 "$SCRIPT_DIR/convert/convert_stmtout.py" "$STAGING_DIR/stmtout.dat" "$STAGING_DIR/stmtout.csv"

echo "  Converting txnin..."
python3 "$SCRIPT_DIR/convert/convert_txnin.py" "$STAGING_DIR/txnin.dat" "$STAGING_DIR/txnin.csv"

echo "  Converting auditout..."
python3 "$SCRIPT_DIR/convert/convert_auditout.py" "$STAGING_DIR/auditout.dat" "$STAGING_DIR/auditout.csv"

echo "  Converting custdb_customer..."
python3 "$SCRIPT_DIR/convert/convert_custdb_customer.py" "$STAGING_DIR/custdb_customer.dat" "$STAGING_DIR/custdb_customer.csv"

echo "  Converting custdb_acctlnk..."
python3 "$SCRIPT_DIR/convert/convert_custdb_acctlnk.py" "$STAGING_DIR/custdb_acctlnk.dat" "$STAGING_DIR/custdb_acctlnk.csv"

echo "  Converting custdb_address..."
python3 "$SCRIPT_DIR/convert/convert_custdb_address.py" "$STAGING_DIR/custdb_address.dat" "$STAGING_DIR/custdb_address.csv"

echo ""
echo "Step 2: Loading CSV into target database..."
echo ""
if [ -z "$DB_URL" ]; then
  echo "  No --db-url provided. Run load scripts manually:"
  echo "    psql -f $SCRIPT_DIR/load/load_acctfile.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_auditvsam.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_intjrnl.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_stmtout.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_txnin.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_auditout.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_custdb_customer.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_custdb_acctlnk.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_custdb_address.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_acct_audit.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_customer.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_eod_runs.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_txn_ledger.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_fraud_flags.sql"
  echo "    psql -f $SCRIPT_DIR/load/load_kyc_audit.sql"
else
  echo "  Loading acctfile..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_acctfile.sql"
  echo "  Loading auditvsam..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_auditvsam.sql"
  echo "  Loading intjrnl..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_intjrnl.sql"
  echo "  Loading stmtout..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_stmtout.sql"
  echo "  Loading txnin..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_txnin.sql"
  echo "  Loading auditout..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_auditout.sql"
  echo "  Loading custdb_customer..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_custdb_customer.sql"
  echo "  Loading custdb_acctlnk..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_custdb_acctlnk.sql"
  echo "  Loading custdb_address..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_custdb_address.sql"
  echo "  Loading acct_audit..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_acct_audit.sql"
  echo "  Loading customer..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_customer.sql"
  echo "  Loading eod_runs..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_eod_runs.sql"
  echo "  Loading txn_ledger..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_txn_ledger.sql"
  echo "  Loading fraud_flags..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_fraud_flags.sql"
  echo "  Loading kyc_audit..."
  psql "$DB_URL" -f "$SCRIPT_DIR/load/load_kyc_audit.sql"
fi

echo ""
echo "Step 3: Validating migration..."
echo ""
if [ -n "$DB_URL" ]; then
  psql "$DB_URL" -f "$SCRIPT_DIR/validate/validate_migration.sql"
else
  echo "  Skipping (no DB_URL). Run manually:"
  echo "    psql -f $SCRIPT_DIR/validate/validate_migration.sql"
fi

echo ""
echo "═══════════════════════════════════════════════════════"
echo " Migration pipeline complete."
echo "═══════════════════════════════════════════════════════"