import { DataSource } from 'typeorm';
import { Repository } from 'typeorm';
import { BcheodDomainModels } from 'src/BCHEOD/Domain/BcheodDomainModels';
import { BcheodInterfaces } from 'src/BCHEOD/Application/Interfaces/BcheodInterfaces';
import Decimal from 'decimal.js';

// Entity definition for EOD_RUNS table
import { Entity, PrimaryGeneratedColumn, Column } from 'typeorm';

@Entity('eod_runs')
export class EodRunEntity {
    @PrimaryGeneratedColumn()
    id!: number;

    @Column({ name: 'run_date', type: 'integer', width: 8 })
    // COBOL: RUN-DATE PIC 9(8)
    runDate!: number;

    @Column({ name: 'run_time', type: 'integer', width: 4 })
    // COBOL: RUN-TIME PIC 9(4)
    runTime!: number;

    @Column({ name: 'status', type: 'varchar', length: 1 })
    // COBOL: STATUS PIC X(1)
    status!: string;

    @Column({ name: 'total_amount', type: 'decimal', precision: 15, scale: 2 })
    // COBOL: TOTAL-AMOUNT PIC S9(13)V99 COMP-3
    totalAmount!: Decimal;

    @Column({ name: 'description', type: 'varchar', length: 20 })
    // COBOL: DESCRIPTION PIC X(20)
    description!: string;
}

// Repository implementation for EOD_RUNS
export class EodRunsRepository implements BcheodInterfaces.IEodRunsRepository {
    private readonly repository: Repository<EodRunEntity>;

    constructor(dataSource: DataSource) {
        this.repository = dataSource.getRepository(EodRunEntity);
    }

    async fetchAllRuns(): Promise<BcheodDomainModels.EodRunBatch> {
        const entities = await this.repository.find();
        const runs = entities.map(
            (entity) =>
                new BcheodDomainModels.EodRun(
                    entity.runDate,
                    entity.runTime,
                    entity.status as BcheodDomainModels.RunStatus,
                    new Decimal(entity.totalAmount),
                    entity.description
                )
        );
        return new BcheodDomainModels.EodRunBatch(runs);
    }

    async updateRun(run: BcheodDomainModels.EodRun): Promise<void> {
        const entity = new EodRunEntity();
        entity.runDate = run.runDate;
        entity.runTime = run.runTime;
        entity.status = run.status;
        entity.totalAmount = run.totalAmount;
        entity.description = run.description;

        await this.repository.save(entity);
    }

    async insertRun(run: BcheodDomainModels.EodRun): Promise<void> {
        const entity = new EodRunEntity();
        entity.runDate = run.runDate;
        entity.runTime = run.runTime;
        entity.status = run.status;
        entity.totalAmount = run.totalAmount;
        entity.description = run.description;

        await this.repository.insert(entity);
    }
}

// DataSource configuration for TypeORM
export const AppDataSource = new DataSource({
    type: 'postgres',
    host: process.env.DB_HOST,
    port: parseInt(process.env.DB_PORT || '5432', 10),
    username: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_NAME,
    entities: [EodRunEntity],
    synchronize: false, // Set to true only for development
    logging: false,
});

// Bridge client for INTCALC service
export class IntCalcService implements BcheodInterfaces.IIntCalcService {
    async calculateInterest(input: any): Promise<any> {
        // Placeholder for actual implementation
        // Replace with actual service call logic
        throw new Error('Method not implemented.');
    }
}

// Bridge client for TXNPOST service
export class TxnPostService implements BcheodInterfaces.ITxnPostService {
    async postTransaction(input: any): Promise<any> {
        // Placeholder for actual implementation
        // Replace with actual service call logic
        throw new Error('Method not implemented.');
    }
}