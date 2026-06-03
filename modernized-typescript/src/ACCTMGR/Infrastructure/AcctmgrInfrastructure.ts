import { DataSource } from 'typeorm';
import { Entity, PrimaryColumn, Column, Repository } from 'typeorm';
import Decimal from 'decimal.js';
import { AcctmgrDomainModels } from 'src/ACCTMGR/Domain/AcctmgrDomainModels';
import { AcctmgrInterfaces } from 'src/ACCTMGR/Application/Interfaces/AcctmgrInterfaces';

// Entity for acctfile table
@Entity({ name: 'acctfile' })
export class AcctfileEntity {
    @PrimaryColumn({ name: 'acct_number', type: 'varchar', length: 10 })
    acctNumber!: string; // COBOL: ACCT-NUMBER PIC 9(10)

    @Column({ name: 'acct_customer_id', type: 'varchar', length: 8 })
    acctCustomerId!: string; // COBOL: ACCT-CUSTOMER-ID PIC 9(8)

    @Column({ name: 'acct_opened_date', type: 'varchar', length: 8 })
    acctOpenedDate!: string; // COBOL: ACCT-OPENED-DATE PIC 9(8)

    @Column({ name: 'acct_type', type: 'varchar', length: 2 })
    acctType!: string; // COBOL: ACCT-TYPE PIC X(02)

    @Column({ name: 'acct_status', type: 'varchar', length: 1 })
    acctStatus!: string; // COBOL: ACCT-STATUS PIC X(01)

    @Column({ name: 'acct_balance', type: 'numeric', precision: 15, scale: 2 })
    acctBalance!: Decimal; // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3

    @Column({ name: 'acct_avail_bal', type: 'numeric', precision: 15, scale: 2 })
    acctAvailBalance!: Decimal; // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3

    @Column({ name: 'acct_int_rate', type: 'numeric', precision: 6, scale: 4 })
    acctIntRate!: Decimal; // COBOL: ACCT-INT-RATE PIC 9(2)V9(4) COMP-3

    @Column({ name: 'acct_last_txn_dt', type: 'varchar', length: 8 })
    acctLastTxnDate!: string; // COBOL: ACCT-LAST-TXN-DT PIC 9(8)

    @Column({ name: 'acct_currency', type: 'varchar', length: 3 })
    acctCurrency!: string; // COBOL: ACCT-CURRENCY PIC X(03)

    @Column({ name: 'acct_branch_code', type: 'varchar', length: 5 })
    acctBranchCode!: string; // COBOL: ACCT-BRANCH-CODE PIC 9(5)

    @Column({ name: 'acct_flags', type: 'varchar', length: 4 })
    acctFlags!: string; // COBOL: ACCT-FLAGS PIC X(04)

    @Column({ name: 'acct_filler', type: 'varchar', length: 50 })
    acctFiller!: string; // COBOL: ACCT-FILLER PIC X(50)
}

// Entity for acct_audit table
@Entity({ name: 'acct_audit' })
export class AcctAuditEntity {
    @PrimaryColumn({ name: 'acct_num', type: 'varchar', length: 18 })
    acctNum!: string; // COBOL: ACCT_NUM PIC 9(18)

    @Column({ name: 'action', type: 'varchar', length: 50 })
    action!: string; // COBOL: ACTION PIC X(50)

    @Column({ name: 'actor', type: 'varchar', length: 50 })
    actor!: string; // COBOL: ACTOR PIC X(50)

    @Column({ name: 'ts', type: 'timestamptz' })
    ts!: Date; // COBOL: TS

    @Column({ name: 'acct_number', type: 'varchar', length: 10 })
    acctNumber!: string; // COBOL: ACCT_NUMBER PIC 9(10)

    @Column({ name: 'comm_function', type: 'varchar', length: 50 })
    commFunction!: string; // COBOL: COMM_FUNCTION PIC X(50)
}

// Entity for customer table
@Entity({ name: 'customer' })
export class CustomerEntity {
    @PrimaryColumn({ name: 'active_acct_count', type: 'numeric', precision: 3, scale: 0 })
    activeAcctCount!: Decimal; // COBOL: ACTIVE_ACCT_COUNT PIC 9(03) COMP-3

    @Column({ name: 'acct_customer_id', type: 'varchar', length: 8 })
    acctCustomerId!: string; // COBOL: ACCT_CUSTOMER_ID PIC 9(8)
}

// Repository for account operations
export class AccountRepository implements AcctmgrInterfaces.IAccountRepository {
    private readonly repository: Repository<AcctfileEntity>;

    constructor(dataSource: DataSource) {
        this.repository = dataSource.getRepository(AcctfileEntity);
    }

    async readAccount(acctNumber: number): Promise<AcctmgrDomainModels.AccountRecord | null> {
        const entity = await this.repository.findOneBy({ acctNumber: acctNumber.toString() });
        if (!entity) return null;

        return new AcctmgrDomainModels.AccountRecord(
            parseInt(entity.acctNumber),
            parseInt(entity.acctCustomerId),
            parseInt(entity.acctOpenedDate),
            entity.acctType as AcctmgrDomainModels.AccountType,
            entity.acctStatus as AcctmgrDomainModels.AccountStatus,
            new Decimal(entity.acctBalance),
            new Decimal(entity.acctAvailBalance),
            new Decimal(entity.acctIntRate),
            parseInt(entity.acctLastTxnDate),
            entity.acctCurrency,
            parseInt(entity.acctBranchCode),
            entity.acctFlags,
            entity.acctFiller
        );
    }

    async writeAccount(account: AcctmgrDomainModels.AccountRecord): Promise<void> {
        const entity = this.repository.create({
            acctNumber: account.acctNumber.toString(),
            acctCustomerId: account.acctCustomerId.toString(),
            acctOpenedDate: account.acctOpenedDate.toString(),
            acctType: account.acctType,
            acctStatus: account.acctStatus,
            acctBalance: account.acctBalance,
            acctAvailBalance: account.acctAvailBalance,
            acctIntRate: account.acctIntRate,
            acctLastTxnDate: account.acctLastTxnDate.toString(),
            acctCurrency: account.acctCurrency,
            acctBranchCode: account.acctBranchCode.toString(),
            acctFlags: account.acctFlags,
            acctFiller: account.acctFiller
        });

        await this.repository.save(entity);
    }
}

// DataSource configuration
export const AppDataSource = new DataSource({
    type: 'postgres',
    host: 'localhost',
    port: 5432,
    username: 'user',
    password: 'password',
    database: 'acctmgr',
    entities: [AcctfileEntity, AcctAuditEntity, CustomerEntity],
    synchronize: true,
    logging: false
});