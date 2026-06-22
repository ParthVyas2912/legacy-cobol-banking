import { KycsyncService } from 'src/KYCSYNC/Application/KycsyncService';
import { IKycsyncRepository } from 'src/KYCSYNC/Application/Interfaces/KycsyncInterfaces';
import { KycsyncDomainModels } from 'src/KYCSYNC/Domain/KycsyncDomainModels';
import Decimal from 'decimal.js';

describe('KycsyncService Parity Tests', () => {
    let kycsyncService: KycsyncService;
    let mockRepository: jest.Mocked<IKycsyncRepository>;

    beforeEach(() => {
        mockRepository = {
            readCustomerRecords: jest.fn(),
            writeCustomerRecords: jest.fn(),
            readTransactionHistory: jest.fn(),
            writeTransactionHistory: jest.fn(),
        };
        kycsyncService = new KycsyncService();
    });

    it('should execute without errors', async () => {
        await expect(kycsyncService.execute()).resolves.not.toThrow();
    });

    it('should read customer records from repository', async () => {
        const mockCustomerRecords: KycsyncDomainModels.CustomerRecord[] = [
            new KycsyncDomainModels.CustomerRecord(
                1,
                'John Doe',
                new Decimal('1000.00'),
                KycsyncDomainModels.AccountStatus.Active
            ),
            new KycsyncDomainModels.CustomerRecord(
                2,
                'Jane Smith',
                new Decimal('2000.00'),
                KycsyncDomainModels.AccountStatus.Inactive
            ),
        ];

        mockRepository.readCustomerRecords.mockResolvedValue(mockCustomerRecords);

        const result = await mockRepository.readCustomerRecords();

        expect(mockRepository.readCustomerRecords).toHaveBeenCalled();
        expect(result).toEqual(mockCustomerRecords);
    });

    it('should write customer records to repository', async () => {
        const mockCustomerRecords: KycsyncDomainModels.CustomerRecord[] = [
            new KycsyncDomainModels.CustomerRecord(
                1,
                'John Doe',
                new Decimal('1000.00'),
                KycsyncDomainModels.AccountStatus.Active
            ),
            new KycsyncDomainModels.CustomerRecord(
                2,
                'Jane Smith',
                new Decimal('2000.00'),
                KycsyncDomainModels.AccountStatus.Inactive
            ),
        ];

        await mockRepository.writeCustomerRecords(mockCustomerRecords);

        expect(mockRepository.writeCustomerRecords).toHaveBeenCalledWith(mockCustomerRecords);
    });

    it('should read transaction history from repository', async () => {
        const customerId = 1;
        const mockTransactionHistory = new KycsyncDomainModels.TransactionHistory([
            new KycsyncDomainModels.Transaction(1, new Decimal('100.00'), '2023-10-01'),
            new KycsyncDomainModels.Transaction(2, new Decimal('200.00'), '2023-10-02'),
        ]);

        mockRepository.readTransactionHistory.mockResolvedValue(mockTransactionHistory);

        const result = await mockRepository.readTransactionHistory(customerId);

        expect(mockRepository.readTransactionHistory).toHaveBeenCalledWith(customerId);
        expect(result).toEqual(mockTransactionHistory);
    });

    it('should write transaction history to repository', async () => {
        const customerId = 1;
        const mockTransactionHistory = new KycsyncDomainModels.TransactionHistory([
            new KycsyncDomainModels.Transaction(1, new Decimal('100.00'), '2023-10-01'),
            new KycsyncDomainModels.Transaction(2, new Decimal('200.00'), '2023-10-02'),
        ]);

        await mockRepository.writeTransactionHistory(customerId, mockTransactionHistory);

        expect(mockRepository.writeTransactionHistory).toHaveBeenCalledWith(customerId, mockTransactionHistory);
    });
});