import { DataSource, Entity, PrimaryGeneratedColumn, Column, Repository } from 'typeorm';
import { StmtrptDomainModels } from 'src/STMTRPT/Domain/StmtrptDomainModels';
import { IStmtrptRepository } from 'src/STMTRPT/Application/Interfaces/StmtrptInterfaces';

// Database Entity for TXN_LEDGER table
@Entity('txn_ledger')
class TxnLedger {
    @PrimaryGeneratedColumn()
    id: number;

    @Column('varchar', { length: 10 })
    stmt_acct_num: string; // COBOL: STMT-ACCT-NUM PIC 9(10)

    @Column('varchar', { length: 40 })
    stmt_description: string; // COBOL: STMT-DESCRIPTION PIC X(40)

    @Column('decimal', { precision: 13, scale: 2 })
    stmt_amount: number; // COBOL: STMT-AMOUNT PIC -Z(11)9.99

    @Column('decimal', { precision: 15, scale: 2 })
    stmt_balance: number; // COBOL: STMT-BALANCE PIC -Z(13)9.99
}

// Database Entity for CUSTREC table
@Entity('custrec')
class CustomerRecord {
    @PrimaryGeneratedColumn()
    id: number;

    @Column('varchar', { length: 10 })
    cust_id: string; // COBOL: CUST-ID PIC X(10)

    @Column('varchar', { length: 30 })
    cust_name: string; // COBOL: CUST-NAME PIC X(30)

    @Column('decimal', { precision: 3, scale: 0 })
    cust_risk_score: number; // COBOL: CUST-RISK-SCORE PIC 9(03) COMP-3

    @Column('decimal', { precision: 3, scale: 0 })
    cust_acct_count: number; // COBOL: CUST-ACCT-COUNT PIC 9(03) COMP-3
}

// Database Context
export class StmtrptDbContext {
    private dataSource: DataSource;

    constructor(dataSource: DataSource) {
        this.dataSource = dataSource;
    }

    getTxnLedgerRepository(): Repository<TxnLedger> {
        return this.dataSource.getRepository(TxnLedger);
    }

    getCustomerRecordRepository(): Repository<CustomerRecord> {
        return this.dataSource.getRepository(CustomerRecord);
    }
}

// Repository Implementation
export class StmtrptRepository implements IStmtrptRepository {
    private dbContext: StmtrptDbContext;

    constructor(dbContext: StmtrptDbContext) {
        this.dbContext = dbContext;
    }

    async writeToStmtFile(data: StmtrptDomainModels.CustomerRecord): Promise<void> {
        const customerRecordRepo = this.dbContext.getCustomerRecordRepository();
        const customerRecord = new CustomerRecord();
        customerRecord.cust_id = data.CUST_ID;
        customerRecord.cust_name = data.CUST_NAME;
        customerRecord.cust_risk_score = data.CUST_RISK_SCORE.toNumber();
        customerRecord.cust_acct_count = data.CUST_ACCT_COUNT.toNumber();
        await customerRecordRepo.save(customerRecord);
    }

    async readFromTxnLedger(query: any): Promise<any> {
        const txnLedgerRepo = this.dbContext.getTxnLedgerRepository();
        return await txnLedgerRepo.find(query);
    }
}