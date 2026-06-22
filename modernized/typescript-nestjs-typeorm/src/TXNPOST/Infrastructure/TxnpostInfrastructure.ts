import { Entity, Column, PrimaryGeneratedColumn } from 'typeorm';

/**
 * ORM Entity for the `txnin` table.
 * Generated from COBOL data structures.
 */
@Entity({ name: 'public.txnin' })
export class Txnin {
  @PrimaryGeneratedColumn('uuid')
  id!: string;

  @Column({ name: 'txn_id', type: 'numeric', precision: 12, scale: 0 })
  // COBOL: TXN-ID PIC 9(12)
  txnId!: string;

  @Column({ name: 'txn_acct_number', type: 'numeric', precision: 10, scale: 0 })
  // COBOL: TXN-ACCT-NUMBER PIC 9(10)
  txnAcctNumber!: string;

  @Column({ name: 'txn_post_date', type: 'date' })
  // COBOL: TXN-POST-DATE PIC 9(8)
  txnPostDate!: Date;

  @Column({ name: 'txn_effective_date', type: 'date' })
  // COBOL: TXN-EFFECTIVE-DATE PIC 9(8)
  txnEffectiveDate!: Date;

  @Column({ name: 'txn_amount', type: 'numeric', precision: 13, scale: 2 })
  // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3
  txnAmount!: string;

  @Column({ name: 'txn_code', type: 'varchar', length: 3 })
  // COBOL: TXN-CODE PIC X(03)
  txnCode!: string;

  @Column({ name: 'txn_channel', type: 'varchar', length: 4 })
  // COBOL: TXN-CHANNEL PIC X(04)
  txnChannel!: string;

  @Column({ name: 'txn_ref_number', type: 'varchar', length: 20 })
  // COBOL: TXN-REF-NUMBER PIC X(20)
  txnRefNumber!: string;

  @Column({ name: 'txn_description', type: 'varchar', length: 40 })
  // COBOL: TXN-DESCRIPTION PIC X(40)
  txnDescription!: string;

  @Column({ name: 'txn_status', type: 'varchar', length: 1 })
  // COBOL: TXN-STATUS PIC X(01)
  txnStatus!: string;

  @Column({ name: 'txn_balance_after', type: 'numeric', precision: 15, scale: 2 })
  // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3
  txnBalanceAfter!: string;

  @Column({ name: 'txn_user_id', type: 'varchar', length: 8 })
  // COBOL: TXN-USER-ID PIC X(08)
  txnUserId!: string;
}

/**
 * ORM Entity for the `auditout` table.
 * Generated from COBOL data structures.
 */
@Entity({ name: 'public.auditout' })
export class Auditout {
  @PrimaryGeneratedColumn('uuid')
  id!: string;

  @Column({ name: 'audit_id', type: 'varchar', length: 20 })
  // COBOL: AUDIT-ID PIC X(20)
  auditId!: string;

  @Column({ name: 'audit_timestamp', type: 'timestamptz' })
  // COBOL: AUDIT-TIMESTAMP PIC 9(14)
  auditTimestamp!: Date;

  @Column({ name: 'audit_action', type: 'varchar', length: 10 })
  // COBOL: AUDIT-ACTION PIC X(10)
  auditAction!: string;

  @Column({ name: 'audit_user_id', type: 'varchar', length: 8 })
  // COBOL: AUDIT-USER-ID PIC X(08)
  auditUserId!: string;

  @Column({ name: 'audit_details', type: 'text' })
  // COBOL: AUDIT-DETAILS PIC X(100)
  auditDetails!: string;
}

/**
 * ORM Entity for the `txn_ledger` table.
 * Generated from COBOL data structures.
 */
@Entity({ name: 'public.txn_ledger' })
export class TxnLedger {
  @PrimaryGeneratedColumn('uuid')
  id!: string;

  @Column({ name: 'count', type: 'numeric', precision: 10, scale: 0 })
  // COBOL: COUNT PIC 9(10)
  count!: string;

  @Column({ name: 'coalesce', type: 'varchar', length: 20 })
  // COBOL: COALESCE PIC X(20)
  coalesce!: string;

  @Column({ name: 'ws_velocity_count', type: 'numeric', precision: 5, scale: 0 })
  // COBOL: WS-VELOCITY-COUNT PIC 9(5)
  wsVelocityCount!: string;

  @Column({ name: 'ws_velocity_amt', type: 'numeric', precision: 13, scale: 2 })
  // COBOL: WS-VELOCITY-AMT PIC S9(11)V99 COMP-3
  wsVelocityAmt!: string;

  @Column({ name: 'txn_acct_number', type: 'numeric', precision: 10, scale: 0 })
  // COBOL: TXN-ACCT-NUMBER PIC 9(10)
  txnAcctNumber!: string;

  @Column({ name: 'ws_current_acct', type: 'varchar', length: 10 })
  // COBOL: WS-CURRENT-ACCT PIC X(10)
  wsCurrentAcct!: string;

  @Column({ name: 'txn_id', type: 'numeric', precision: 12, scale: 0 })
  // COBOL: TXN-ID PIC 9(12)
  txnId!: string;

  @Column({ name: 'acct', type: 'varchar', length: 10 })
  // COBOL: ACCT PIC X(10)
  acct!: string;

  @Column({ name: 'amount', type: 'numeric', precision: 13, scale: 2 })
  // COBOL: AMOUNT PIC S9(11)V99 COMP-3
  amount!: string;

  @Column({ name: 'code', type: 'varchar', length: 3 })
  // COBOL: CODE PIC X(03)
  code!: string;

  @Column({ name: 'posted_at', type: 'timestamptz' })
  // COBOL: POSTED-AT PIC 9(14)
  postedAt!: Date;

  @Column({ name: 'txn_amount', type: 'numeric', precision: 13, scale: 2 })
  // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3
  txnAmount!: string;

  @Column({ name: 'txn_code', type: 'varchar', length: 3 })
  // COBOL: TXN-CODE PIC X(03)
  txnCode!: string;
}