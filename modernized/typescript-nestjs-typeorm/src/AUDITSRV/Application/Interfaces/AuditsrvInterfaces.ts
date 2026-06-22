import { AuditsrvDomainModels } from "src/AUDITSRV/Domain/AuditsrvDomainModels";

// Interface for AUDITSRV program
export namespace AuditsrvInterfaces {
    // Repository interface for file I/O operations
    export interface IAuditRepository {
        /**
         * Reads an audit record from the file.
         * @param auditId The ID of the audit record to retrieve.
         * @returns The audit record if found, otherwise null.
         */
        readAuditRecord(auditId: string): Promise<AuditsrvDomainModels.AuditRecord | null>;

        /**
         * Writes an audit record to the file.
         * @param auditRecord The audit record to write.
         */
        writeAuditRecord(auditRecord: AuditsrvDomainModels.AuditRecord): Promise<void>;

        /**
         * Updates an existing audit record in the file.
         * @param auditRecord The audit record to update.
         */
        updateAuditRecord(auditRecord: AuditsrvDomainModels.AuditRecord): Promise<void>;

        /**
         * Deletes an audit record from the file.
         * @param auditId The ID of the audit record to delete.
         */
        deleteAuditRecord(auditId: string): Promise<void>;
    }

    // Service interface for CICS operations
    export interface ICicsService {
        /**
         * Sends a request to a CICS transaction.
         * @param transactionId The ID of the CICS transaction.
         * @param requestData The data to send to the transaction.
         * @returns The response from the transaction.
         */
        sendTransaction(transactionId: string, requestData: any): Promise<any>;
    }

    // Main application interface
    export interface IAuditsrvApplication {
        /**
         * Executes the main logic of the AUDITSRV program.
         */
        execute(): Promise<void>;
    }
}