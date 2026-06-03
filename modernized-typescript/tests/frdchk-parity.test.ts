import { DataSource } from 'typeorm';
import { Decimal } from 'decimal.js';
import { FrdchkService } from 'src/FRDCHK/Application/FrdchkService';
import { FrdchkDbContext, TransactionRepositoryImpl } from 'src/FRDCHK/Infrastructure/FrdchkInfrastructure';
import { TxnLedgerEntity, FraudFlagsEntity } from 'src/FRDCHK/Infrastructure/FrdchkInfrastructure';
import { FrdchkInterfaces } from 'src/FRDCHK/Application/Interfaces/FrdchkInterfaces';

describe('FRDCHK Parity Tests', () => {
    let dataSource: DataSource;
    let dbContext: FrdchkDbContext;
    let transactionRepository: FrdchkInterfaces.TransactionRepository;
    let cicsServiceMock: jest.Mocked<FrdchkInterfaces.CicsService>;
    let frdchkService: FrdchkService;

    beforeAll(async () => {
        // Initialize the database connection
        dataSource = new DataSource({
            type: 'sqlite',
            database: ':memory:',
            entities: [TxnLedgerEntity, FraudFlagsEntity],
            synchronize: true,
        });
        await dataSource.initialize();

        dbContext = new FrdchkDbContext(dataSource);
        transactionRepository = new TransactionRepositoryImpl(dbContext);

        cicsServiceMock = {
            returnControl: jest.fn(),
            linkToProgram: jest.fn(),
        };

        frdchkService = new FrdchkService(transactionRepository, cicsServiceMock);
    });

    afterAll(async () => {
        await dataSource.destroy();
    });

    beforeEach(async () => {
        // Clear the database before each test
        await dbContext.getTxnLedgerRepository().clear();
        await dbContext.getFraudFlagsRepository().clear();
    });

    it('FRDCHK: happy path', async () => {
        // Setup: Insert a row into TXN_LEDGER
        const txnLedgerRepository = dbContext.getTxnLedgerRepository();
        await txnLedgerRepository.insert({
            txn_id: '1',
            txnAmount: '100.00',
            txnBalanceAfter: '1000.00',
            txnAcctNumber: '1234567890',
            postedAt: new Date(),
        });

        // Execute the FRDCHK program
        await frdchkService.execute();

        // Verify: Check FRAUD_FLAGS table for expected output
        const fraudFlagsRepository = dbContext.getFraudFlagsRepository();
        const fraudFlags = await fraudFlagsRepository.find();
        expect(fraudFlags).toHaveLength(1);
        expect(fraudFlags[0]).toMatchObject({
            acct: '1234567890',
            reason: 'Account frozen due to high risk',
        });

        // Verify: Check CICS link to ACCTMGR
        expect(cicsServiceMock.linkToProgram).toHaveBeenCalledWith(
            'ACCTMGR',
            expect.any(Buffer)
        );
    });
});