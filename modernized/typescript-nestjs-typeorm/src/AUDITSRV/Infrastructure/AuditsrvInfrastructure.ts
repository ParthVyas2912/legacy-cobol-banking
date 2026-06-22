import { Entity, PrimaryColumn, Column, Repository, DataSource } from "typeorm";
import { AuditsrvDomainModels } from "src/AUDITSRV/Domain/AuditsrvDomainModels";
import { AuditsrvInterfaces } from "src/AUDITSRV/Application/Interfaces/AuditsrvInterfaces";

@Entity("auditvsam")
export class Auditvsam {
    @PrimaryColumn("varchar", { length: 20 })
    audit_key!: string; // COBOL: AUDIT-KEY PIC X(20)

    @Column("varchar", { length: 10 })
    audit_acct_number!: string; // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)

    @Column("varchar", { length: 50 })
    audit_action!: string; // COBOL: AUDIT-ACTION PIC X(50)

    @Column("varchar", { length: 20 })
    audit_user!: string; // COBOL: AUDIT-USER PIC X(20)

    @Column("timestamptz")
    audit_ts!: Date; // COBOL: AUDIT-TS PIC X(26)

    @Column("varchar", { length: 100 })
    audit_detail!: string; // COBOL: AUDIT-DETAIL PIC X(100)

    @Column("numeric", { precision: 15, scale: 2 })
    audit_amount!: Decimal; // COBOL: AUDIT-AMOUNT PIC 9(13)V99 COMP-3

    @Column("integer")
    audit_status!: number; // COBOL: AUDIT-STATUS PIC 9(5)
}

export class AuditRepository implements AuditsrvInterfaces.IAuditRepository {
    private repository: Repository<Auditvsam>;

    constructor(dataSource: DataSource) {
        this.repository = dataSource.getRepository(Auditvsam);
    }

    async readAuditRecord(auditId: string): Promise<AuditsrvDomainModels.AuditRecord | null> {
        const auditEntity = await this.repository.findOne({ where: { audit_key: auditId } });
        if (!auditEntity) {
            return null;
        }
        return new AuditsrvDomainModels.AuditRecord(
            auditEntity.audit_key,
            auditEntity.audit_user,
            parseInt(auditEntity.audit_ts.toISOString().slice(0, 10).replace(/-/g, "")), // Convert date to YYYYMMDD
            parseInt(auditEntity.audit_ts.toISOString().slice(11, 19).replace(/:/g, "")), // Convert time to HHMMSS
            auditEntity.audit_action,
            new Decimal(auditEntity.audit_amount.toString()), // Convert numeric to Decimal
            auditEntity.audit_detail,
            auditEntity.audit_status
        );
    }

    async writeAuditRecord(auditRecord: AuditsrvDomainModels.AuditRecord): Promise<void> {
        const auditEntity = this.repository.create({
            audit_key: auditRecord.auditId,
            audit_acct_number: auditRecord.userId,
            audit_action: auditRecord.action,
            audit_user: auditRecord.userId,
            audit_ts: new Date(
                `${auditRecord.timestamp.toString().slice(0, 4)}-${auditRecord.timestamp
                    .toString()
                    .slice(4, 6)}-${auditRecord.timestamp.toString().slice(6, 8)}T${auditRecord.time
                    .toString()
                    .slice(0, 2)}:${auditRecord.time.toString().slice(2, 4)}:${auditRecord.time
                    .toString()
                    .slice(4, 6)}Z`
            ),
            audit_detail: auditRecord.description,
            audit_amount: auditRecord.amount,
            audit_status: auditRecord.status,
        });
        await this.repository.save(auditEntity);
    }

    async updateAuditRecord(auditRecord: AuditsrvDomainModels.AuditRecord): Promise<void> {
        const existingRecord = await this.repository.findOne({ where: { audit_key: auditRecord.auditId } });
        if (!existingRecord) {
            throw new Error(`Audit record with ID ${auditRecord.auditId} not found.`);
        }
        existingRecord.audit_acct_number = auditRecord.userId;
        existingRecord.audit_action = auditRecord.action;
        existingRecord.audit_user = auditRecord.userId;
        existingRecord.audit_ts = new Date(
            `${auditRecord.timestamp.toString().slice(0, 4)}-${auditRecord.timestamp
                .toString()
                .slice(4, 6)}-${auditRecord.timestamp.toString().slice(6, 8)}T${auditRecord.time
                .toString()
                .slice(0, 2)}:${auditRecord.time.toString().slice(2, 4)}:${auditRecord.time
                .toString()
                .slice(4, 6)}Z`
        );
        existingRecord.audit_detail = auditRecord.description;
        existingRecord.audit_amount = auditRecord.amount;
        existingRecord.audit_status = auditRecord.status;
        await this.repository.save(existingRecord);
    }

    async deleteAuditRecord(auditId: string): Promise<void> {
        const existingRecord = await this.repository.findOne({ where: { audit_key: auditId } });
        if (!existingRecord) {
            throw new Error(`Audit record with ID ${auditId} not found.`);
        }
        await this.repository.remove(existingRecord);
    }
}

export const createAuditsrvDataSource = (): DataSource => {
    return new DataSource({
        type: "postgres",
        host: process.env.DB_HOST,
        port: parseInt(process.env.DB_PORT || "5432"),
        username: process.env.DB_USER,
        password: process.env.DB_PASSWORD,
        database: process.env.DB_NAME,
        entities: [Auditvsam],
        synchronize: false, // Set to true only for development
        logging: false,
    });
};