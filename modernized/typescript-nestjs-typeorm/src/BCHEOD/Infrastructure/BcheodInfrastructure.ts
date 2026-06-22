import { DataSource } from "typeorm";
import { Entity, Column, PrimaryGeneratedColumn } from "typeorm";

// ORM Entity for EodRuns table
@Entity({ name: "eod_runs" })
export class EodRuns {
    @PrimaryGeneratedColumn()
    id: number;

    @Column({ name: "started_at", type: "timestamptz" })
    // COBOL: STARTED_AT TIMESTAMPTZ
    startedAt: Date;

    @Column({ name: "status", type: "varchar", length: 50 })
    // COBOL: STATUS X(50)
    status: string;

    @Column({ name: "ended_at", type: "timestamptz" })
    // COBOL: ENDED_AT TIMESTAMPTZ
    endedAt: Date;

    @Column({ name: "ws_status_code", type: "varchar", length: 10 })
    // COBOL: WS_STATUS_CODE X(10)
    wsStatusCode: string;
}

// DataSource configuration for TypeORM
export const AppDataSource = new DataSource({
    type: "postgres",
    host: process.env.DB_HOST || "localhost",
    port: parseInt(process.env.DB_PORT || "5432", 10),
    username: process.env.DB_USER || "user",
    password: process.env.DB_PASSWORD || "password",
    database: process.env.DB_NAME || "bcheod",
    entities: [EodRuns],
    synchronize: false, // Set to true only during development
    logging: false,
});

// Repository for EodRuns
export class EodRunsRepository {
    private repository = AppDataSource.getRepository(EodRuns);

    async findAll(): Promise<EodRuns[]> {
        return this.repository.find();
    }

    async findById(id: number): Promise<EodRuns | null> {
        return this.repository.findOneBy({ id });
    }

    async save(eodRun: EodRuns): Promise<EodRuns> {
        return this.repository.save(eodRun);
    }

    async deleteById(id: number): Promise<void> {
        await this.repository.delete({ id });
    }
}