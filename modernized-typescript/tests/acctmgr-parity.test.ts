import { AcctmgrService } from 'src/ACCTMGR/Application/AcctmgrService';
import { AcctmgrDomainModels } from 'src/ACCTMGR/Domain/AcctmgrDomainModels';
import { AcctmgrInterfaces } from 'src/ACCTMGR/Application/Interfaces/AcctmgrInterfaces';
import Decimal from 'decimal.js';

describe('AcctmgrService Parity Tests', () => {
    let accountRepositoryMock: jest.Mocked<AcctmgrInterfaces.IAccountRepository>;
    let cicsServiceMock: jest.Mocked<AcctmgrInterfaces.ICicsService>;
    let acctmgrService: AcctmgrService;

    beforeEach(() => {
        accountRepositoryMock = {
            readAccount: jest.fn(),
            writeAccount: jest.fn(),
        };

        cicsServiceMock = {
            returnControl: jest.fn(),
            writeAuditRecord: jest.fn(),
            readCustomer: jest.fn(),
            rewriteAccount: jest.fn(),
        };

        acctmgrService = new AcctmgrService(accountRepositoryMock, cicsServiceMock);
    });

    it('should execute the happy path scenario', async () => {
        // Arrange
        const sampleAccount = new AcctmgrDomainModels.AccountRecord(
            1,
            1,
            20230101,
            AcctmgrDomainModels.AccountType.SAVINGS,
            AcctmgrDomainModels.AccountStatus.ACTIVE,
            new Decimal('1222222222222.34'),
            new Decimal('1222222222222.34'),
            new Decimal('12.3444'),
            20230101,
            'USD',
            1,
            'YYYY',
            'FILLER'
        );

        accountRepositoryMock.readAccount.mockResolvedValue(sampleAccount);
        accountRepositoryMock.writeAccount.mockResolvedValue();

        // Act
        await acctmgrService.execute();

        // Assert
        expect(accountRepositoryMock.readAccount).toHaveBeenCalledWith(1);
        expect(accountRepositoryMock.writeAccount).toHaveBeenCalledWith(sampleAccount);
        expect(cicsServiceMock.returnControl).toHaveBeenCalled();
    });

    it('should handle empty input file scenario', async () => {
        // Arrange
        accountRepositoryMock.readAccount.mockResolvedValue(null);

        // Act
        await acctmgrService.execute();

        // Assert
        expect(accountRepositoryMock.readAccount).toHaveBeenCalled();
        expect(cicsServiceMock.returnControl).toHaveBeenCalled();
    });
});