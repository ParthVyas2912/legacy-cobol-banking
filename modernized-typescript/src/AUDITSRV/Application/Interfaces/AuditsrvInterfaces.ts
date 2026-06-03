import { AuditsrvDomainModels } from 'src/AUDITSRV/Domain/AuditsrvDomainModels';

/**
 * Interface for repository operations related to the AUDIT-FILE.
 */
export interface IAuditFileRepository {
    /**
     * Reads a record from the AUDIT-FILE based on the provided key.
     * @param auditKey The primary key of the audit record to retrieve.
     * @returns The audit record if found, otherwise null.
     */
    readAuditFile(auditKey: string): Promise<AuditsrvDomainModels.AuditFile | null>;
}

/**
 * Interface for CICS service operations used in the AUDITSRV program.
 */
export interface IAuditCicsService {
    /**
     * Writes a message to the CICS terminal.
     * @param message The message to write.
     */
    writeToCics(message: string): Promise<void>;

    /**
     * Returns control to the CICS environment.
     */
    returnToCics(): Promise<void>;
}

/**
 * Main interface for the AUDITSRV application logic.
 */
export interface IAuditsrvApplication {
    /**
     * Entry point for the AUDITSRV program.
     */
    start(): Promise<void>;
}