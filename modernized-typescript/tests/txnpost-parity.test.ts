import { TxnpostService } from 'src/TXNPOST/Application/TxnpostService';
import { ITxnpostRepository, ITxnpostService } from 'src/TXNPOST/Application/Interfaces/TxnpostInterfaces';
import { TxnpostRepository } from 'src/TXNPOST/Infrastructure/TxnpostInfrastructure';
import { TxnpostDataSource } from 'src/TXNPOST/Infrastructure/TxnpostInfrastructure';
import { DataSource } from 'typeorm';

describe('TXNPOST Parity Tests', () => {
    let txnpostService: TxnpostService;
    let repository: ITxnpostRepository;
    let service: ITxnpostService;
    let dataSource: DataSource;

    beforeAll(async () => {
        dataSource = TxnpostDataSource;
        await dataSource.initialize();

        repository = new TxnpostRepository();
        service = new TxnpostService(dataSource);

        txnpostService = new TxnpostService(repository, service);
    });

    afterAll(async () => {
        await dataSource.destroy();
    });

    it('TXNPOST: happy path', async () => {
        // Arrange
        const sampleTransaction = {
            txnId: '000000000001',
            txnAcctNumber: '0000000001',
            txnPostDate: '00000001',
            txnEffectiveDate: '00000001',
            txnAmount: '12222222222.34',
            txnCode: 'AAA',
            txnChannel: 'AAAA',
            txnRefNumber: 'AAAAAAAAAAAAAAAAAAAA',
            txnDescription: 'AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA',
            txnStatus: 'A',
            txnBalanceAfter: '1222222222222.34',
            txnUserId: 'AAAAAAAA',
        };

        jest.spyOn(repository, 'openFiles').mockResolvedValue();
        jest.spyOn(repository, 'readTransaction').mockResolvedValueOnce(sampleTransaction).mockResolvedValueOnce(null);
        jest.spyOn(service, 'applyTransaction').mockResolvedValue(true);
        jest.spyOn(repository, 'writeAuditRecord').mockResolvedValue();
        jest.spyOn(repository, 'closeFiles').mockResolvedValue();

        // Act
        await txnpostService.execute();

        // Assert
        expect(repository.openFiles).toHaveBeenCalledTimes(1);
        expect(repository.readTransaction).toHaveBeenCalledTimes(2);
        expect(service.applyTransaction).toHaveBeenCalledWith(sampleTransaction);
        expect(repository.writeAuditRecord).toHaveBeenCalledWith({
            transaction: sampleTransaction,
            success: true,
        });
        expect(repository.closeFiles).toHaveBeenCalledTimes(1);
    });

    it('TXNPOST: empty input file', async () => {
        // Arrange
        jest.spyOn(repository, 'openFiles').mockResolvedValue();
        jest.spyOn(repository, 'readTransaction').mockResolvedValue(null);
        jest.spyOn(repository, 'closeFiles').mockResolvedValue();

        // Act
        await txnpostService.execute();

        // Assert
        expect(repository.openFiles).toHaveBeenCalledTimes(1);
        expect(repository.readTransaction).toHaveBeenCalledTimes(1);
        expect(repository.closeFiles).toHaveBeenCalledTimes(1);
    });
});