import { BcheodService } from 'src/BCHEOD/Application/BcheodService';
import { AppDataSource } from 'src/BCHEOD/Infrastructure/BcheodInfrastructure';
import { BcheodDomainModels } from 'src/BCHEOD/Domain/BcheodDomainModels';
import { DataSource } from 'typeorm';
import Decimal from 'decimal.js';

describe('BCHEOD Parity Tests', () => {
    let service: BcheodService;
    let dataSource: DataSource;

    beforeAll(async () => {
        dataSource = AppDataSource;
        await dataSource.initialize();
        service = new BcheodService();
    });

    afterAll(async () => {
        await dataSource.destroy();
    });

    it('BCHEOD: happy path', async () => {
        // Arrange
        const eodRun = new BcheodDomainModels.EodRun(
            20231010, // runDate
            1234, // runTime
            BcheodDomainModels.RunStatus.NotStarted, // status
            new Decimal(1000.00), // totalAmount
            'Initial Run' // description
        );

        const repository = dataSource.getRepository('eod_runs');
        await repository.insert({
            run_date: eodRun.runDate,
            run_time: eodRun.runTime,
            status: eodRun.status,
            total_amount: eodRun.totalAmount.toString(),
            description: eodRun.description
        });

        // Act
        await service.execute();

        // Assert
        const updatedRun = await repository.findOneBy({ run_date: eodRun.runDate });
        expect(updatedRun).not.toBeNull();
        expect(updatedRun?.status).toBe(BcheodDomainModels.RunStatus.Completed);
    });

    it('BCHEOD: expected abend (critical modernization finding present)', async () => {
        // Arrange
        const eodRun = new BcheodDomainModels.EodRun(
            20231010, // runDate
            1234, // runTime
            BcheodDomainModels.RunStatus.NotStarted, // status
            new Decimal(1000.00), // totalAmount
            'Critical Error' // description
        );

        const repository = dataSource.getRepository('eod_runs');
        await repository.insert({
            run_date: eodRun.runDate,
            run_time: eodRun.runTime,
            status: eodRun.status,
            total_amount: eodRun.totalAmount.toString(),
            description: eodRun.description
        });

        // Act & Assert
        await expect(service.execute()).rejects.toThrow('Critical modernization finding present');
    });
});