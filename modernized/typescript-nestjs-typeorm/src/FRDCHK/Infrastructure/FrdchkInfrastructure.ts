import { Entity, PrimaryGeneratedColumn, Column, BaseEntity } from "typeorm";

/**
 * ORM Entity for the `txn_ledger` table.
 * Maps COBOL fields to database columns.
 */
@Entity({ name: "txn_ledger" })
export class TxnLedger extends BaseEntity {
  @PrimaryGeneratedColumn()
  id!: number;

  @Column("varchar", { name: "count", length: 30 })
  // COBOL: COUNT PIC X(30)
  count!: string;

  @Column("varchar", { name: "coalesce", length: 30 })
  // COBOL: COALESCE PIC X(30)
  coalesce!: string;

  @Column("varchar", { name: "ws_velocity_count", length: 30 })
  // COBOL: WS_VELOCITY_COUNT PIC X(30)
  ws_velocity_count!: string;

  @Column("varchar", { name: "ws_velocity_amt", length: 30 })
  // COBOL: WS_VELOCITY_AMT PIC X(30)
  ws_velocity_amt!: string;

  @Column("varchar", { name: "txn_acct_number", length: 30 })
  // COBOL: TXN_ACCT_NUMBER PIC X(30)
  txn_acct_number!: string;

  @Column("varchar", { name: "ws_current_acct", length: 30 })
  // COBOL: WS_CURRENT_ACCT PIC X(30)
  ws_current_acct!: string;

  @Column("varchar", { name: "txn_id", length: 30 })
  // COBOL: TXN_ID PIC X(30)
  txn_id!: string;

  @Column("varchar", { name: "acct", length: 30 })
  // COBOL: ACCT PIC X(30)
  acct!: string;

  @Column("varchar", { name: "amount", length: 30 })
  // COBOL: AMOUNT PIC X(30)
  amount!: string;

  @Column("varchar", { name: "code", length: 30 })
  // COBOL: CODE PIC X(30)
  code!: string;

  @Column("timestamptz", { name: "posted_at" })
  // COBOL: POSTED_AT PIC X(30)
  posted_at!: Date;

  @Column("varchar", { name: "txn_amount", length: 30 })
  // COBOL: TXN_AMOUNT PIC X(30)
  txn_amount!: string;

  @Column("varchar", { name: "txn_code", length: 30 })
  // COBOL: TXN_CODE PIC X(30)
  txn_code!: string;
}

/**
 * ORM Entity for the `fraud_flags` table.
 * Maps COBOL fields to database columns.
 */
@Entity({ name: "fraud_flags" })
export class FraudFlags extends BaseEntity {
  @PrimaryGeneratedColumn()
  id!: number;

  @Column("varchar", { name: "acct", length: 30 })
  // COBOL: ACCT PIC X(30)
  acct!: string;

  @Column("varchar", { name: "score", length: 30 })
  // COBOL: SCORE PIC X(30)
  score!: string;

  @Column("varchar", { name: "reason", length: 30 })
  // COBOL: REASON PIC X(30)
  reason!: string;

  @Column("timestamptz", { name: "ts" })
  // COBOL: TS PIC X(30)
  ts!: Date;

  @Column("varchar", { name: "txn_acct_number", length: 30 })
  // COBOL: TXN_ACCT_NUMBER PIC X(30)
  txn_acct_number!: string;

  @Column("varchar", { name: "ws_risk_score", length: 30 })
  // COBOL: WS_RISK_SCORE PIC X(30)
  ws_risk_score!: string;
}