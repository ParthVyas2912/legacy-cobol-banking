import { DataSource, Repository, Entity, PrimaryColumn, Column } from 'typeorm';
import { AuditsrvDomainModels } from 'src/AUDITSRV/Domain/AuditsrvDomainModels';
import { IAuditFileRepository } from 'src/AUDITSRV/Application/Interfaces/AuditsrvInterfaces';

/**
 * Entity definition for the Auditvsam table.
 */
@Entity({ name: 'auditvsam' })
export class AuditvsamEntity {
    @PrimaryColumn({ name: 'audit_key', type: 'varchar', length: 20 })
    // COBOL: AUDIT-KEY PIC X(20)
    auditKey!: string;

    @Column({ name: 'audit_acct_number', type: 'numeric', precision: 10, scale: 0 })
    // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)
    auditAcctNumber!: string;

    @Column({ name: 'audit_action', type: 'varchar', length: 16 })
    // COBOL: AUDIT-ACTION PIC X(16)
    auditAction!: string;

    @Column({ name: 'audit_user', type: 'varchar', length: 8 })
    // COBOL: AUDIT-USER PIC X(08)
    auditUser!: string;

    @Column({ name: 'audit_ts', type: 'varchar', length: 26 })
    // COBOL: AUDIT-TS PIC X(26)
    auditTs!: string;

    @Column({ name: 'audit_detail', type: 'varchar', length: 120 })
    // COBOL: AUDIT-DETAIL PIC X(120)
    auditDetail!: string;
}

/**
 * Repository implementation for the Auditvsam table.
 */
export class AuditFileRepository implements IAuditFileRepository {
    private readonly repository: Repository<AuditvsamEntity>;

    constructor(dataSource: DataSource) {
        this.repository = dataSource.getRepository(AuditvsamEntity);
    }

    /**
     * Reads a record from the AUDIT-FILE based on the provided key.
     * @param auditKey The primary key of the audit record to retrieve.
     * @returns The audit record if found, otherwise null.
     */
    async readAuditFile(auditKey: string): Promise<AuditsrvDomainModels.AuditFile | null> {
        const entity = await this.repository.findOne({ where: { auditKey } });
        if (!entity) {
            return null;
        }
        return new AuditsrvDomainModels.AuditFile(
            entity.auditKey,
            BigInt(entity.auditAcctNumber),
            entity.auditAction,
            entity.auditUser,
            entity.auditTs,
            entity.auditDetail
        );
    }
}

/**
 * DataSource configuration for the AUDITSRV application.
 */
export const AuditsrvDataSource = new DataSource({
    type: 'postgres',
    host: process.env.DB_HOST,
    port: parseInt(process.env.DB_PORT || '5432', 10),
    username: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_NAME,
    entities: [AuditvsamEntity],
    synchronize: false, // Ensure this is false in production
    logging: true,
});