import { AuditsrvService } from 'src/AUDITSRV/Application/AuditsrvService';
import { IAuditFileRepository, IAuditCicsService } from 'src/AUDITSRV/Application/Interfaces/AuditsrvInterfaces';
import { AuditsrvDomainModels } from 'src/AUDITSRV/Domain/AuditsrvDomainModels';

describe('AuditsrvService Parity Tests', () => {
    let auditFileRepositoryMock: jest.Mocked<IAuditFileRepository>;
    let auditCicsServiceMock: jest.Mocked<IAuditCicsService>;
    let auditsrvService: AuditsrvService;

    beforeEach(() => {
        auditFileRepositoryMock = {
            readAuditFile: jest.fn(),
        };

        auditCicsServiceMock = {
            writeToCics: jest.fn(),
            returnToCics: jest.fn(),
        };

        auditsrvService = new AuditsrvService(auditFileRepositoryMock, auditCicsServiceMock);
    });

    it('AUDITSRV: happy path', async () => {
        // Arrange
        const expectedAuditRecord = new AuditsrvDomainModels.AuditFile(
            'INITIAL_KEY',
            BigInt(0),
            'INITIAL_ACTION',
            'INITIAL_USER',
            'INITIAL_TIMESTAMP',
            'INITIAL_DETAIL'
        );

        auditFileRepositoryMock.readAuditFile.mockResolvedValue(expectedAuditRecord);

        // Act
        await auditsrvService.execute();

        // Assert
        expect(auditCicsServiceMock.writeToCics).toHaveBeenCalledWith('Audit record initialized.');
        expect(auditCicsServiceMock.returnToCics).toHaveBeenCalled();
    });

    it('AUDITSRV: empty input file', async () => {
        // Arrange
        auditFileRepositoryMock.readAuditFile.mockResolvedValue(null);

        // Act
        await auditsrvService.execute();

        // Assert
        expect(auditCicsServiceMock.writeToCics).toHaveBeenCalledWith('Audit record initialized.');
        expect(auditCicsServiceMock.returnToCics).toHaveBeenCalled();
    });
});