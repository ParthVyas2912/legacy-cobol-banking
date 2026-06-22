import { BcheodService } from "src/BCHEOD/Application/BcheodService";
import { BcheodDomainModels, TransactionType, CustomerDetails } from "src/BCHEOD/Domain/BcheodDomainModels";
import { AppDataSource, EodRunsRepository, EodRuns } from "src/BCHEOD/Infrastructure/BcheodInfrastructure";
import Decimal from "decimal.js";

describe("BCHEOD Parity Tests", () => {
    let bcheodService: BcheodService;
    let eodRunsRepository: EodRunsRepository;

    beforeAll(async () => {
        // Initialize the database connection
        await AppDataSource.initialize();
        eodRunsRepository = new EodRunsRepository();
        bcheodService = new BcheodService();
    });

    afterAll(async () => {
        // Close the database connection
        await AppDataSource.destroy();
    });

    test("BcheodService execute() should complete without errors", async () => {
        await expect(bcheodService.execute()).resolves.toBeUndefined();
    });

    test("EodRunsRepository should save and retrieve EodRuns entity", async () => {
        const eodRun = new EodRuns();
        eodRun.startedAt = new Date();
        eodRun.status = "IN_PROGRESS";
        eodRun.endedAt = new Date();
        eodRun.wsStatusCode = "200";

        const savedEodRun = await eodRunsRepository.save(eodRun);
        expect(savedEodRun.id).toBeDefined();
        expect(savedEodRun.status).toBe("IN_PROGRESS");

        const retrievedEodRun = await eodRunsRepository.findById(savedEodRun.id);
        expect(retrievedEodRun).not.toBeNull();
        expect(retrievedEodRun?.id).toBe(savedEodRun.id);
        expect(retrievedEodRun?.status).toBe("IN_PROGRESS");

        await eodRunsRepository.deleteById(savedEodRun.id);
        const deletedEodRun = await eodRunsRepository.findById(savedEodRun.id);
        expect(deletedEodRun).toBeNull();
    });

    test("BcheodDomainModels should correctly initialize with valid data", () => {
        const customerDetails = new CustomerDetails(
            "John Doe",
            "123 Main St, Springfield",
            "555-123-4567"
        );

        const transaction = new BcheodDomainModels(
            "1234567890",
            new Decimal("12345.67"),
            "20231010",
            TransactionType.DEPOSIT,
            customerDetails
        );

        expect(transaction.accountNumber).toBe("1234567890");
        expect(transaction.transactionAmount.toString()).toBe("12345.67");
        expect(transaction.transactionDate).toBe("20231010");
        expect(transaction.transactionType).toBe(TransactionType.DEPOSIT);
        expect(transaction.customerDetails.customerName).toBe("John Doe");
        expect(transaction.customerDetails.customerAddress).toBe("123 Main St, Springfield");
        expect(transaction.customerDetails.customerPhoneNumber).toBe("555-123-4567");
    });

    test("EodRunsRepository should retrieve all EodRuns entities", async () => {
        const eodRun1 = new EodRuns();
        eodRun1.startedAt = new Date();
        eodRun1.status = "COMPLETED";
        eodRun1.endedAt = new Date();
        eodRun1.wsStatusCode = "200";

        const eodRun2 = new EodRuns();
        eodRun2.startedAt = new Date();
        eodRun2.status = "FAILED";
        eodRun2.endedAt = new Date();
        eodRun2.wsStatusCode = "500";

        await eodRunsRepository.save(eodRun1);
        await eodRunsRepository.save(eodRun2);

        const allEodRuns = await eodRunsRepository.findAll();
        expect(allEodRuns.length).toBeGreaterThanOrEqual(2);

        await eodRunsRepository.deleteById(eodRun1.id);
        await eodRunsRepository.deleteById(eodRun2.id);
    });
});