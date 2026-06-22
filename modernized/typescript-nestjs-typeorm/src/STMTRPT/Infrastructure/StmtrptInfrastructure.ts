import { DataSource } from "typeorm";
import { Entity, Column, PrimaryGeneratedColumn } from "typeorm";
import { Decimal } from "decimal.js";

/**
 * Entity representing the `stmtout` table in the database.
 */
@Entity({ name: "stmtout" })
export class Stmtout {
    @PrimaryGeneratedColumn()
    id: number;

    @Column({ name: "stmt_acct_num", type: "varchar", length: 10 })
    // COBOL: STMT-ACCT-NUM PIC 9(10)
    stmtAcctNum: string;

    @Column({ name: "stmt_description", type: "varchar", length: 40 })
    // COBOL: STMT-DESCRIPTION PIC X(40)
    stmtDescription: string;

    @Column({ name: "stmt_amount", type: "numeric", precision: 13, scale: 2 })
    // COBOL: STMT-AMOUNT PIC -Z(11)9.99
    stmtAmount: Decimal;

    @Column({ name: "stmt_balance", type: "numeric", precision: 15, scale: 2 })
    // COBOL: STMT-BALANCE PIC -Z(13)9.99
    stmtBalance: Decimal;
}

/**
 * Repository interface for interacting with the `stmtout` table.
 */
export interface IStmtoutRepository {
    /**
     * Retrieves a statement record by account number.
     * @param accountNumber The account number to search for.
     * @returns The statement record if found, otherwise null.
     */
    findByAccountNumber(accountNumber: string): Promise<Stmtout | null>;

    /**
     * Saves a statement record to the database.
     * @param stmtout The statement record to save.
     */
    save(stmtout: Stmtout): Promise<void>;

    /**
     * Retrieves all statement records.
     * @returns An array of all statement records.
     */
    findAll(): Promise<Stmtout[]>;
}

/**
 * Implementation of the IStmtoutRepository interface using TypeORM.
 */
export class StmtoutRepository implements IStmtoutRepository {
    private readonly dataSource: DataSource;

    constructor(dataSource: DataSource) {
        this.dataSource = dataSource;
    }

    async findByAccountNumber(accountNumber: string): Promise<Stmtout | null> {
        const repository = this.dataSource.getRepository(Stmtout);
        return repository.findOne({ where: { stmtAcctNum: accountNumber } });
    }

    async save(stmtout: Stmtout): Promise<void> {
        const repository = this.dataSource.getRepository(Stmtout);
        await repository.save(stmtout);
    }

    async findAll(): Promise<Stmtout[]> {
        const repository = this.dataSource.getRepository(Stmtout);
        return repository.find();
    }
}

/**
 * Data source configuration for the STMTRPT program.
 */
export const StmtrptDataSource = new DataSource({
    type: "postgres",
    host: "localhost",
    port: 5432,
    username: "your_username",
    password: "your_password",
    database: "your_database",
    entities: [Stmtout],
    synchronize: false, // Set to true only during development
});