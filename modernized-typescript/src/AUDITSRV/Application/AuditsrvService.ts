import { IAuditFileRepository, IAuditCicsService } from 'src/AUDITSRV/Application/Interfaces/AuditsrvInterfaces';
import { AuditsrvDomainModels } from 'src/AUDITSRV/Domain/AuditsrvDomainModels';

export class AuditsrvService {
    private readonly auditFileRepository: IAuditFileRepository;
    private readonly auditCicsService: IAuditCicsService;

    constructor(auditFileRepository: IAuditFileRepository, auditCicsService: IAuditCicsService) {
        this.auditFileRepository = auditFileRepository;
        this.auditCicsService = auditCicsService;
    }

    /**
     * Entry point for the AUDITSRV program.
     */
    public async execute(): Promise<void> {
        await this.start();
    }

    /**
     * Paragraph: 0000-START
     */
    private async start(): Promise<void> {
        // COBOL MOVE statements
        const auditKey = 'INITIAL_KEY';
        const auditAcctNumber = BigInt(0);
        const auditAction = 'INITIAL_ACTION';
        const auditUser = 'INITIAL_USER';
        const auditTs = 'INITIAL_TIMESTAMP';
        const auditDetail = 'INITIAL_DETAIL';

        // Create an instance of AuditFile with initial values
        const auditRecord = new AuditsrvDomainModels.AuditFile(
            auditKey,
            auditAcctNumber,
            auditAction,
            auditUser,
            auditTs,
            auditDetail
        );

        // Simulate EXEC CICS WRITE
        await this.auditCicsService.writeToCics('Audit record initialized.');

        // Simulate IF condition
        if (auditRecord.auditKey === 'INITIAL_KEY') {
            // Simulate EXEC CICS RETURN
            await this.auditCicsService.returnToCics();
        }
    }
}