import { KycsyncService } from 'src/KYCSYNC/Application/KycsyncService';
import { KycAuditRepository, AppDataSource } from 'src/KYCSYNC/Infrastructure/KycsyncInfrastructure';
import { KycsyncDomainModels } from 'src/KYCSYNC/Domain/KycsyncDomainModels';
import Decimal from 'decimal.js';

describe('KYCSYNC Parity Tests', () => {
    let kycsyncService: KycsyncService;
    let kycAuditRepository: KycAuditRepository;

    beforeAll(async () => {
        // Initialize the data source
        await AppDataSource.initialize();
        kycAuditRepository = new KycAuditRepository(AppDataSource);
        kycsyncService = new KycsyncService();
    });

    afterAll(async () => {
        // Clean up the data source
        await AppDataSource.destroy();
    });

    beforeEach(async () => {
        // Clear the KYC_AUDIT table before each test
        const repository = AppDataSource.getRepository('kyc_audit');
        await repository.clear();
    });

    it('KYCSYNC: happy path', async () => {
        // Execute the KYCSYNC service
        await kycsyncService.execute();

        // Fetch all records from KYC_AUDIT table
        const kycAudits = await kycAuditRepository.fetchAllKycAudits();

        // Assert that the KYC_AUDIT table has the expected data
        expect(kycAudits).toHaveLength(1);
        expect(kycAudits[0]).toMatchObject({
            custId: 'example-cust-id',
            custKycStatus: 'example-status',
            custRiskScore: new Decimal('100'),
            custAcctCount: new Decimal('10'),
            wsNextCount: new Decimal('1000'),
        });
    });
});