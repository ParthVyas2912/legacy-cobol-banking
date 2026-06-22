import { AuditsrvService } from "src/AUDITSRV/Application/AuditsrvService";
import { AuditsrvDomainModels } from "src/AUDITSRV/Domain/AuditsrvDomainModels";
import { AuditRepository, createAuditsrvDataSource } from "src/AUDITSRV/Infrastructure/AuditsrvInfrastructure";
import { DataSource } from "typeorm";
import { Decimal } from "decimal.js";

describe("AuditsrvService Parity Tests", () => {
    let auditsrvService: AuditsrvService;
    let dataSource: DataSource;
    let auditRepository: AuditRepository;

    beforeAll(async () => {
        dataSource = createAuditsrvDataSource();
        await dataSource.initialize();
        auditRepository = new AuditRepository(dataSource);
        auditsrvService = new AuditsrvService();
    });

    afterAll(async () => {
        await dataSource.destroy();
    });

    it("should execute the service without errors", async () => {
        await expect(auditsrvService.execute()).resolves.not.toThrow();
    });

    it("should write, read, update, and delete an audit record", async () => {
        const auditRecord = new AuditsrvDomainModels.AuditRecord(
            "AUDIT001",
            "USER12345",
            20231010,
            123456,
            "Test Action",
            new Decimal("1234.56"),
            "Test Description",
            1 // StatusEnum.SUCCESS is represented as 1
        );

        // Write the audit record
        await expect(auditRepository.writeAuditRecord(auditRecord)).resolves.not.toThrow();

        // Read the audit record
        const retrievedRecord = await auditRepository.readAuditRecord(auditRecord.auditId);
        expect(retrievedRecord).not.toBeNull();
        expect(retrievedRecord).toEqual(auditRecord);

        // Update the audit record
        auditRecord.action = "Updated Action";
        auditRecord.description = "Updated Description";
        auditRecord.amount = new Decimal("6543.21");
        auditRecord.status = 0; // StatusEnum.FAILURE is represented as 0
        await expect(auditRepository.updateAuditRecord(auditRecord)).resolves.not.toThrow();

        // Verify the update
        const updatedRecord = await auditRepository.readAuditRecord(auditRecord.auditId);
        expect(updatedRecord).not.toBeNull();
        expect(updatedRecord?.action).toBe("Updated Action");
        expect(updatedRecord?.description).toBe("Updated Description");
        expect(updatedRecord?.amount.equals(new Decimal("6543.21"))).toBe(true);
        expect(updatedRecord?.status).toBe(0);

        // Delete the audit record
        await expect(auditRepository.deleteAuditRecord(auditRecord.auditId)).resolves.not.toThrow();

        // Verify deletion
        const deletedRecord = await auditRepository.readAuditRecord(auditRecord.auditId);
        expect(deletedRecord).toBeNull();
    });

    it("should throw an error when updating a non-existent audit record", async () => {
        const nonExistentRecord = new AuditsrvDomainModels.AuditRecord(
            "NONEXISTENT",
            "USER99999",
            20231010,
            123456,
            "Non-existent Action",
            new Decimal("0.00"),
            "Non-existent Description",
            0 // StatusEnum.FAILURE is represented as 0
        );

        await expect(auditRepository.updateAuditRecord(nonExistentRecord)).rejects.toThrow(
            `Audit record with ID ${nonExistentRecord.auditId} not found.`
        );
    });

    it("should throw an error when deleting a non-existent audit record", async () => {
        const nonExistentAuditId = "NONEXISTENT";

        await expect(auditRepository.deleteAuditRecord(nonExistentAuditId)).rejects.toThrow(
            `Audit record with ID ${nonExistentAuditId} not found.`
        );
    });
});