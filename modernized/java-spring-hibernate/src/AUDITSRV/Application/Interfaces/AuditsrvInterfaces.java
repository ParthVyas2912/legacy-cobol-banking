package src.AUDITSRV.Application.Interfaces;

import src.AUDITSRV.Domain.AuditsrvDomainModels.AuditRecord;
import java.util.List;

/**
 * Application interfaces for AUDITSRV program.
 * Each COBOL paragraph is represented as a method.
 * File I/O operations are represented as repository interfaces.
 * CICS operations are represented as service interfaces.
 */
public interface AuditsrvInterfaces {

    /**
     * Reads audit records from the input file.
     * 
     * @return List of AuditRecord objects read from the file.
     */
    List<AuditRecord> readAuditRecords();

    /**
     * Writes audit records to the output file.
     * 
     * @param auditRecords List of AuditRecord objects to be written to the file.
     */
    void writeAuditRecords(List<AuditRecord> auditRecords);

    /**
     * Processes a single audit record.
     * 
     * @param auditRecord The AuditRecord object to be processed.
     */
    void processAuditRecord(AuditRecord auditRecord);

    /**
     * Validates an audit record.
     * 
     * @param auditRecord The AuditRecord object to be validated.
     * @return true if the record is valid, false otherwise.
     */
    boolean validateAuditRecord(AuditRecord auditRecord);

    /**
     * Logs an error message for an invalid audit record.
     * 
     * @param auditRecord The AuditRecord object that caused the error.
     * @param errorMessage The error message to be logged.
     */
    void logError(AuditRecord auditRecord, String errorMessage);

    /**
     * Finalizes the audit processing, performing any necessary cleanup or summary operations.
     */
    void finalizeAuditProcessing();
}