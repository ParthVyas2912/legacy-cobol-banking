import { Entity, Column, PrimaryGeneratedColumn, Repository, DataSource } from 'typeorm';
import { FrdchkDomainModels } from 'src/FRDCHK/Domain/FrdchkDomainModels';
import { FrdchkInterfaces } from 'src/FRDCHK/Application/Interfaces/FrdchkInterfaces';

@Entity('txn_ledger')
export class TxnLedgerEntity {
    @PrimaryGeneratedColumn('uuid')
    txn_id!: string;

    @Column('decimal', { precision: 13, scale: 2, name: 'txn_amount' })
    // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3
    txnAmount!: string;

    @Column('decimal', { precision: 15, scale: 2, name: 'txn_balance_after' })
    // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3
    txnBalanceAfter!: string;

    @Column('varchar', { length: 30, name: 'txn_acct_number' })
    // COBOL: TXN-ACCT-NUMBER PIC X(30)
    txnAcctNumber!: string;

    @Column('timestamptz', { name: 'posted_at' })
    // COBOL: POSTED-AT TIMESTAMP
    postedAt!: Date;
}

@Entity('fraud_flags')
export class FraudFlagsEntity {
    @PrimaryGeneratedColumn('uuid')
    id!: string;

    @Column('varchar', { length: 30, name: 'acct' })
    // COBOL: ACCT PIC X(30)
    acct!: string;

    @Column('decimal', { precision: 3, scale: 0, name: 'score' })
    // COBOL: WS-RISK-SCORE PIC 9(03) COMP-3
    score!: string;

    @Column('varchar', { length: 30, name: 'reason' })
    // COBOL: REASON PIC X(30)
    reason!: string;

    @Column('timestamptz', { name: 'ts' })
    // COBOL: TS TIMESTAMP
    ts!: Date;
}

export class FrdchkDbContext {
    private readonly txnLedgerRepository: Repository<TxnLedgerEntity>;
    private readonly fraudFlagsRepository: Repository<FraudFlagsEntity>;

    constructor(private readonly dataSource: DataSource) {
        this.txnLedgerRepository = this.dataSource.getRepository(TxnLedgerEntity);
        this.fraudFlagsRepository = this.dataSource.getRepository(FraudFlagsEntity);
    }

    public getTxnLedgerRepository(): Repository<TxnLedgerEntity> {
        return this.txnLedgerRepository;
    }

    public getFraudFlagsRepository(): Repository<FraudFlagsEntity> {
        return this.fraudFlagsRepository;
    }
}

export class TransactionRepositoryImpl implements FrdchkInterfaces.TransactionRepository {
    constructor(private readonly dbContext: FrdchkDbContext) {}

    async readTransactions(accountId: string): Promise<FrdchkDomainModels.TxnRecord[]> {
        const txnLedgerRepository = this.dbContext.getTxnLedgerRepository();
        const transactions = await txnLedgerRepository.find({ where: { txnAcctNumber: accountId } });

        return transactions.map(
            (txn) =>
                new FrdchkDomainModels.TxnRecord(
                    new Decimal(txn.txnAmount),
                    new Decimal(txn.txnBalanceAfter)
                )
        );
    }

    async updateFraudFlags(accountId: string): Promise<void> {
        const fraudFlagsRepository = this.dbContext.getFraudFlagsRepository();
        await fraudFlagsRepository.update(
            { acct: accountId },
            { reason: 'Account frozen due to high risk', ts: new Date() }
        );
    }
}