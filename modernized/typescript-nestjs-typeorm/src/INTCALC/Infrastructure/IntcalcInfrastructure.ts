import { DataSource } from 'typeorm';
import { Entity, Column, PrimaryColumn } from 'typeorm';

/**
 * Entity for the `acctfile` table.
 * Represents the account file data mapped from COBOL.
 */
@Entity({ name: 'acctfile', schema: 'public' })
export class Acctfile {
  @PrimaryColumn({ name: 'acct_number', type: 'varchar', length: 10 })
  // COBOL: ACCT-NUMBER PIC 9(10)
  acctNumber!: string;

  @Column({ name: 'acct_customer_id', type: 'varchar', length: 8 })
  // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
  acctCustomerId!: string;

  @Column({ name: 'acct_opened_date', type: 'date' })
  // COBOL: ACCT-OPENED-DATE PIC 9(8)
  acctOpenedDate!: Date;

  @Column({ name: 'acct_type', type: 'varchar', length: 2 })
  // COBOL: ACCT-TYPE PIC X(02)
  acctType!: string;

  @Column({ name: 'acct_status', type: 'varchar', length: 1 })
  // COBOL: ACCT-STATUS PIC X(01)
  acctStatus!: string;

  @Column({ name: 'acct_balance', type: 'numeric', precision: 15, scale: 2 })
  // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3
  acctBalance!: string;

  @Column({ name: 'acct_avail_bal', type: 'numeric', precision: 15, scale: 2 })
  // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3
  acctAvailBal!: string;

  @Column({ name: 'acct_int_rate', type: 'numeric', precision: 6, scale: 4 })
  // COBOL: ACCT-INT-RATE PIC 9(2)V9(4) COMP-3
  acctIntRate!: string;

  @Column({ name: 'acct_last_txn_dt', type: 'date' })
  // COBOL: ACCT-LAST-TXN-DT PIC 9(8)
  acctLastTxnDt!: Date;

  @Column({ name: 'acct_currency', type: 'varchar', length: 3 })
  // COBOL: ACCT-CURRENCY PIC X(03)
  acctCurrency!: string;

  @Column({ name: 'acct_branch_code', type: 'varchar', length: 4 })
  // COBOL: ACCT-BRANCH-CODE PIC 9(4)
  acctBranchCode!: string;

  @Column({ name: 'acct_flags', type: 'varchar', length: 4 })
  // COBOL: ACCT-FLAGS PIC X(04)
  acctFlags!: string;

  @Column({ name: 'acct_filler', type: 'varchar', length: 50 })
  // COBOL: ACCT-FILLER PIC X(50)
  acctFiller!: string;
}

/**
 * Entity for the `intjrnl` table.
 * Represents the interest journal data mapped from COBOL.
 */
@Entity({ name: 'intjrnl', schema: 'public' })
export class Intjrnl {
  @PrimaryColumn({ name: 'txn_id', type: 'varchar', length: 12 })
  // COBOL: TXN-ID PIC 9(12)
  txnId!: string;

  @Column({ name: 'txn_acct_number', type: 'varchar', length: 10 })
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
 * DataSource configuration for the INTCALC program.
 */
export const IntcalcDataSource = new DataSource({
  type: 'postgres',
  host: process.env.DB_HOST,
  port: parseInt(process.env.DB_PORT || '5432', 10),
  username: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  database: process.env.DB_NAME,
  schema: 'public',
  entities: [Acctfile, Intjrnl],
  synchronize: false,
  logging: false,
});