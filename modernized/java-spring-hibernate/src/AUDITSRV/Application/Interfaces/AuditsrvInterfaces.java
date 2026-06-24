package src.AUDITSRV.Application.Interfaces;

import src.AUDITSRV.Domain.AuditsrvDomainModels.AuditRecord;
import src.AUDITSRV.Domain.AuditsrvDomainModels.AuditHeader;

import java.util.List;

public interface AuditsrvInterfaces {

    /**
     * Initialize the audit service.
     */
    void initializeAuditService();

    /**
     * Process a batch of audit records.
     * 
     * @param auditHeader The header containing batch information and associated audit records.
     */
    void processAuditBatch(AuditHeader auditHeader);

    /**
     * Validate an individual audit record.
     * 
     * @param auditRecord The audit record to validate.
     * @return True if the record is valid, false otherwise.
     */
    boolean validateAuditRecord(AuditRecord auditRecord);

    /**
     * Persist a single audit record to the database.
     * 
     * @param auditRecord The audit record to persist.
     */
    void persistAuditRecord(AuditRecord auditRecord);

    /**
     * Retrieve all audit records for a specific batch.
     * 
     * @param batchId The batch ID to filter audit records.
     * @return A list of audit records associated with the batch ID.
     */
    List<AuditRecord> retrieveAuditRecordsByBatch(String batchId);

    /**
     * Generate a summary report for a batch of audit records.
     * 
     * @param batchId The batch ID for which to generate the report.
     * @return A string representation of the summary report.
     */
    String generateBatchSummaryReport(String batchId);

    /**
     * Finalize the audit service, performing any necessary cleanup.
     */
    void finalizeAuditService();
}