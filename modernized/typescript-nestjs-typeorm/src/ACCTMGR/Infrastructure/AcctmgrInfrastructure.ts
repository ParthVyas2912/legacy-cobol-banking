import { Entity, PrimaryColumn, Column, CreateDateColumn, UpdateDateColumn, BaseEntity, PrimaryGeneratedColumn } from 'typeorm';
import { IAccountRepository, ITransactionRepository } from 'src/ACCTMGR/Application/Interfaces/AcctmgrInterfaces';
import { AccountRecord, TransactionRecord, AccountStatus, TransactionType } from 'src/ACCTMGR/Domain/AcctmgrDomainModels';
import Decimal from 'decimal.js';

@Entity('acctfile')
export class Acctfile extends BaseEntity {
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
    acctBalance!: Decimal;

    @Column({ name: 'acct_avail_bal', type: 'numeric', precision: 15, scale: 2 })
    // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3
    acctAvailBal!: Decimal;

    @Column({ name: 'acct_int_rate', type: 'numeric', precision: 6, scale: 4 })
    // COBOL: ACCT-INT-RATE PIC 9(2)V9(4)
    acctIntRate!: Decimal;

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

@Entity('acct_audit')
export class AcctAudit extends BaseEntity {
    @PrimaryGeneratedColumn()
    id!: number;

    @Column({ name: 'acct_num', type: 'varchar', length: 10 })
    // COBOL: ACCT-NUMBER PIC 9(10)
    acctNum!: string;

    @Column({ name: 'action', type: 'varchar', length: 1 })
    // COBOL: ACTION PIC X(01)
    action!: string;

    @Column({ name: 'actor', type: 'varchar', length: 30 })
    // COBOL: ACTOR PIC X(30)
    actor!: string;

    @CreateDateColumn({ name: 'ts', type: 'timestamptz' })
    // COBOL: TS PIC 9(8)
    ts!: Date;

    @Column({ name: 'acct_number', type: 'varchar', length: 10 })
    // COBOL: ACCT-NUMBER PIC 9(10)
    acctNumber!: string;

    @Column({ name: 'comm_function', type: 'varchar', length: 20 })
    // COBOL: COMM-FUNCTION PIC X(20)
    commFunction!: string;
}

@Entity('customer')
export class Customer extends BaseEntity {
    @PrimaryGeneratedColumn()
    id!: number;

    @Column({ name: 'active_acct_count', type: 'numeric', precision: 5, scale: 0 })
    // COBOL: ACTIVE-ACCT-COUNT PIC 9(5)
    activeAcctCount!: number;

    @Column({ name: 'acct_customer_id', type: 'varchar', length: 8 })
    // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
    acctCustomerId!: string;
}

export class AccountRepository implements IAccountRepository {
    async readAllAccounts(): Promise<AccountRecord[]> {
        const records = await Acctfile.find();
        return records.map(record => new AccountRecord(
            record.acctNumber,
            '', // accountName is not mapped in the COBOL contract
            new Decimal(record.acctBalance),
            parseInt(record.acctLastTxnDt.toISOString().replace(/-/g, '').substring(0, 8)),
            record.acctStatus as AccountStatus
        ));
    }

    async writeAccount(account: AccountRecord): Promise<void> {
        const entity = new Acctfile();
        entity.acctNumber = account.accountId;
        entity.acctBalance = account.accountBalance;
        entity.acctLastTxnDt = new Date(
            `${account.lastTransactionDate.toString().substring(0, 4)}-${account.lastTransactionDate.toString().substring(4, 6)}-${account.lastTransactionDate.toString().substring(6, 8)}`
        );
        entity.acctStatus = account.accountStatus;
        await entity.save();
    }

    async updateAccount(account: AccountRecord): Promise<void> {
        const entity = await Acctfile.findOneByOrFail({ acctNumber: account.accountId });
        entity.acctBalance = account.accountBalance;
        entity.acctLastTxnDt = new Date(
            `${account.lastTransactionDate.toString().substring(0, 4)}-${account.lastTransactionDate.toString().substring(4, 6)}-${account.lastTransactionDate.toString().substring(6, 8)}`
        );
        entity.acctStatus = account.accountStatus;
        await entity.save();
    }

    async deleteAccount(accountId: string): Promise<void> {
        await Acctfile.delete({ acctNumber: accountId });
    }
}

export class TransactionRepository implements ITransactionRepository {
    async readAllTransactions(): Promise<TransactionRecord[]> {
        const records = await AcctAudit.find();
        return records.map(record => new TransactionRecord(
            record.acctNum,
            record.acctNumber,
            new Decimal(0), // transactionAmount is not mapped in the COBOL contract
            parseInt(record.ts.toISOString().replace(/-/g, '').substring(0, 8)),
            record.action as TransactionType
        ));
    }

    async writeTransaction(transaction: TransactionRecord): Promise<void> {
        const entity = new AcctAudit();
        entity.acctNum = transaction.transactionId;
        entity.acctNumber = transaction.accountId;
        entity.ts = new Date(
            `${transaction.transactionDate.toString().substring(0, 4)}-${transaction.transactionDate.toString().substring(4, 6)}-${transaction.transactionDate.toString().substring(6, 8)}`
        );
        entity.action = transaction.transactionType;
        await entity.save();
    }

    async updateTransaction(transaction: TransactionRecord): Promise<void> {
        const entity = await AcctAudit.findOneByOrFail({ acctNum: transaction.transactionId });
        entity.ts = new Date(
            `${transaction.transactionDate.toString().substring(0, 4)}-${transaction.transactionDate.toString().substring(4, 6)}-${transaction.transactionDate.toString().substring(6, 8)}`
        );
        entity.action = transaction.transactionType;
        await entity.save();
    }

    async deleteTransaction(transactionId: string): Promise<void> {
        await AcctAudit.delete({ acctNum: transactionId });
    }
}