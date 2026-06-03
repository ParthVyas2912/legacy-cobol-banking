import { StmtrptService } from 'src/STMTRPT/Application/StmtrptService';
import { StmtrptRepository } from 'src/STMTRPT/Infrastructure/StmtrptInfrastructure';
import { StmtrptDomainModels } from 'src/STMTRPT/Domain/StmtrptDomainModels';
import { DataSource } from 'typeorm';

describe('STMTRPT Parity Tests', () => {
    let service: StmtrptService;
    let repository: StmtrptRepository;
    let dataSource: DataSource;

    beforeAll(async () => {
        // Initialize the data source
        dataSource = new DataSource({
            type: 'sqlite',
            database: ':memory:',
            synchronize: true,
            entities: [
                StmtrptDomainModels.CustomerRecord,
                'txn_ledger', // Ensure this matches the entity definition
                'custrec' // Ensure this matches the entity definition
            ],
        });
        await dataSource.initialize();

        // Initialize the repository and service
        repository = new StmtrptRepository(new StmtrptDomainModels(dataSource));
        service = new StmtrptService(repository, {
            executeImsFunction: jest.fn(),
            executeSqlQuery: jest.fn(),
        });
    });

    afterAll(async () => {
        await dataSource.destroy();
    });

    beforeEach(async () => {
        // Setup: Insert at least one row into TXN_LEDGER
        const txnLedgerRepo = dataSource.getRepository('txn_ledger');
        await txnLedgerRepo.insert({
            stmt_acct_num: '0000000001',
            stmt_description: 'AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA',
            stmt_amount: 1.0,
            stmt_balance: 1.0,
        });
    });

    afterEach(async () => {
        // Teardown: Truncate TXN_LEDGER
        const txnLedgerRepo = dataSource.getRepository('txn_ledger');
        await txnLedgerRepo.clear();
    });

    it('STMTRPT: happy path', async () => {
        // Execute the main program logic
        await service.main();

        // Verify the expected outputs
        const stmtFileRepo = dataSource.getRepository('custrec');
        const stmtFileRecords = await stmtFileRepo.find();

        expect(stmtFileRecords).toHaveLength(1);
        expect(stmtFileRecords[0]).toMatchObject({
            cust_id: '0000000001',
            cust_name: 'AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA',
            cust_risk_score: 1,
            cust_acct_count: 1,
        });

        // Verify side effects
        const txnLedgerRepo = dataSource.getRepository('txn_ledger');
        const txnLedgerRecords = await txnLedgerRepo.find();
        expect(txnLedgerRecords).toHaveLength(0); // Assuming processing clears the ledger
    });
});