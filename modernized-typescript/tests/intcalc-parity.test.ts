import { IntcalcService } from 'src/INTCALC/Application/IntcalcService';
import { IntcalcInfrastructure } from 'src/INTCALC/Infrastructure/IntcalcInfrastructure';
import { IntcalcDomainModels } from 'src/INTCALC/Domain/IntcalcDomainModels';
import { Pool } from 'pg';
import Decimal from 'decimal.js';

describe('INTCALC Parity Tests', () => {
    let dbPool: Pool;
    let acctFileRepository: IntcalcInfrastructure['createAcctFileRepository'];
    let intJournalRepository: IntcalcInfrastructure['createIntJournalRepository'];
    let intcalcService: IntcalcService;

    beforeAll(() => {
        dbPool = new Pool();
        const infrastructure = new IntcalcInfrastructure(dbPool);
        acctFileRepository = infrastructure.createAcctFileRepository();
        intJournalRepository = infrastructure.createIntJournalRepository();
        intcalcService = new IntcalcService(acctFileRepository, intJournalRepository);
    });

    afterAll(async () => {
        await dbPool.end();
    });

    beforeEach(async () => {
        // Seed ACCT-FILE with sample data
        const sampleAcctFile = new IntcalcDomainModels.AcctFile(
            1, // ACCT-NUMBER
            1, // ACCT-CUSTOMER-ID
            20230101, // ACCT-OPENED-DATE
            'CK', // ACCT-TYPE
            'A', // ACCT-STATUS
            new Decimal('1222222222222.34'), // ACCT-BALANCE
            new Decimal('1222222222222.34'), // ACCT-AVAIL-BAL
            new Decimal('12.3444'), // ACCT-INT-RATE
            20230101, // ACCT-LAST-TXN-DT
            'USD', // ACCT-CURRENCY
            1234, // ACCT-BRANCH-CODE
            'YYYY', // ACCT-FLAGS
            'FILLER DATA' // ACCT-FILLER
        );
        await acctFileRepository.writeAcctFile(sampleAcctFile);
    });

    afterEach(async () => {
        // Truncate ACCT-FILE
        await dbPool.query('TRUNCATE TABLE public.acctfile');
    });

    it('INTCALC: happy path', async () => {
        // Execute the INTCALC service
        await intcalcService.execute();

        // Verify INT-JOURNAL contains expected data
        const result = await dbPool.query('SELECT * FROM public.intjrnl');
        expect(result.rows).toHaveLength(1);

        const txnRecord = result.rows[0];
        expect(txnRecord.txn_id).toBe('1'); // TXN-ID
        expect(txnRecord.txn_amount).toBe('12222222222.34'); // TXN-AMOUNT
        expect(txnRecord.txn_balance_after).toBe('1222222222222.34'); // TXN-BALANCE-AFTER
    });

    it('INTCALC: empty input file', async () => {
        // Truncate ACCT-FILE to simulate empty input file
        await dbPool.query('TRUNCATE TABLE public.acctfile');

        // Execute the INTCALC service
        await intcalcService.execute();

        // Verify INT-JOURNAL is empty
        const result = await dbPool.query('SELECT * FROM public.intjrnl');
        expect(result.rows).toHaveLength(0);
    });
});