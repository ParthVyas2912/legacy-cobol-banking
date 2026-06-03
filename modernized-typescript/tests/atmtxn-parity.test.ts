import { AtmtxnService } from 'src/ATMTXN/Application/AtmtxnService';
import { AtmtxnInterfaces } from 'src/ATMTXN/Application/Interfaces/AtmtxnInterfaces';
import { AtmtxnInfrastructure } from 'src/ATMTXN/Infrastructure/AtmtxnInfrastructure';
import { Decimal } from 'decimal.js';

describe('ATMTXN Parity Tests', () => {
    let cicsServiceMock: jest.Mocked<AtmtxnInterfaces.CicsService>;
    let atmtxnService: AtmtxnService;

    beforeEach(() => {
        cicsServiceMock = {
            handle: jest.fn().mockResolvedValue(undefined),
            receive: jest.fn().mockResolvedValue(undefined),
            return: jest.fn().mockResolvedValue(undefined),
            link: jest.fn().mockResolvedValue(undefined),
            send: jest.fn().mockResolvedValue(undefined),
            abend: jest.fn().mockResolvedValue(undefined),
        };

        atmtxnService = new AtmtxnService(cicsServiceMock);
    });

    it('should execute the ATMTXN program happy path', async () => {
        // Arrange: Mock CICS RECEIVE input
        const cicsReceivePayload = {
            __placeholder: '<CICS RECEIVE payload from ATMMAIN>',
        };
        cicsServiceMock.receive.mockResolvedValueOnce(cicsReceivePayload);

        // Arrange: Expected outputs
        const expectedCallArea = {
            __placeholder: '<COMMAREA passed to ACCTMGR>',
        };
        const expectedTxnData = {
            __placeholder: '<COMMAREA passed to FRDCHK>',
        };
        const expectedNextPayload = {
            __placeholder: '<CICS SEND payload to ATMNEXT>',
        };

        // Act: Execute the ATMTXN service
        await atmtxnService.start();

        // Assert: Validate CICS RECEIVE was called
        expect(cicsServiceMock.receive).toHaveBeenCalledTimes(1);

        // Assert: Validate CICS LINK to ACCTMGR
        expect(cicsServiceMock.link).toHaveBeenCalledWith(expectedCallArea);
        expect(cicsServiceMock.link).toHaveBeenCalledTimes(1);

        // Assert: Validate CICS LINK to FRDCHK
        expect(cicsServiceMock.link).toHaveBeenCalledWith(expectedTxnData);
        expect(cicsServiceMock.link).toHaveBeenCalledTimes(2);

        // Assert: Validate CICS SEND to ATMNEXT
        expect(cicsServiceMock.send).toHaveBeenCalledWith(expectedNextPayload);
        expect(cicsServiceMock.send).toHaveBeenCalledTimes(1);

        // Assert: Validate program flow
        expect(cicsServiceMock.handle).not.toHaveBeenCalled();
        expect(cicsServiceMock.abend).not.toHaveBeenCalled();
    });

    it('should handle fatal errors gracefully', async () => {
        // Arrange: Simulate an error during execution
        cicsServiceMock.receive.mockRejectedValueOnce(new Error('Simulated CICS RECEIVE error'));

        // Act: Execute the ATMTXN service
        await atmtxnService.start();

        // Assert: Validate CICS ABEND was called
        expect(cicsServiceMock.abend).toHaveBeenCalledTimes(1);

        // Assert: Validate no further processing occurred
        expect(cicsServiceMock.link).not.toHaveBeenCalled();
        expect(cicsServiceMock.send).not.toHaveBeenCalled();
    });

    it('should validate transaction input correctly', async () => {
        // Arrange: Mock CICS RECEIVE input
        const cicsReceivePayload = {
            __placeholder: '<CICS RECEIVE payload from ATMMAIN>',
        };
        cicsServiceMock.receive.mockResolvedValueOnce(cicsReceivePayload);

        // Act: Execute the validation step
        await atmtxnService['validate']();

        // Assert: Validate CICS RECEIVE was called
        expect(cicsServiceMock.receive).toHaveBeenCalledTimes(1);

        // Assert: Validate no errors occurred
        expect(cicsServiceMock.abend).not.toHaveBeenCalled();
    });

    it('should post transaction data correctly', async () => {
        // Arrange: Mock transaction data
        const txnAmount = new Decimal(100.00);
        const txnBalanceAfter = new Decimal(900.00);
        const transactionRecord = new AtmtxnInfrastructure.TransactionRecordEntity(txnAmount, txnBalanceAfter);

        const dbContext = new AtmtxnInfrastructure.AtmtxnDbContext();
        const repository = dbContext.getTransactionRecordRepository();

        // Act: Save the transaction record
        await repository.save(transactionRecord);

        // Assert: Validate the transaction record was saved
        const savedRecords = await repository.findAll();
        expect(savedRecords).toHaveLength(1);
        expect(savedRecords[0].txn_amount.equals(txnAmount)).toBe(true);
        expect(savedRecords[0].txn_balance_after.equals(txnBalanceAfter)).toBe(true);
    });

    it('should send the next screen or response', async () => {
        // Arrange: Mock CICS SEND payload
        const cicsSendPayload = {
            __placeholder: '<CICS SEND payload to ATMNEXT>',
        };

        // Act: Execute the sendNext step
        await atmtxnService['sendNext']();

        // Assert: Validate CICS SEND was called
        expect(cicsServiceMock.send).toHaveBeenCalledWith(cicsSendPayload);
        expect(cicsServiceMock.send).toHaveBeenCalledTimes(1);
    });
});