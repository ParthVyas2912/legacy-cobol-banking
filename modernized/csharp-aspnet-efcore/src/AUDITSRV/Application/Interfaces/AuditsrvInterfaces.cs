using System.Collections.Generic;
using AUDITSRV.Domain;

namespace AUDITSRV.Application.Interfaces
{
    public interface IAuditRepository
    {
        /// <summary>
        /// Reads an audit record by its unique identifier.
        /// </summary>
        /// <param name="recordId">The unique identifier of the audit record.</param>
        /// <returns>The audit record if found; otherwise, null.</returns>
        AuditsrvDomainModels.AuditRecord ReadAuditRecord(string recordId);

        /// <summary>
        /// Writes an audit record to the repository.
        /// </summary>
        /// <param name="auditRecord">The audit record to write.</param>
        void WriteAuditRecord(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Updates an existing audit record in the repository.
        /// </summary>
        /// <param name="auditRecord">The audit record to update.</param>
        void UpdateAuditRecord(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Deletes an audit record by its unique identifier.
        /// </summary>
        /// <param name="recordId">The unique identifier of the audit record to delete.</param>
        void DeleteAuditRecord(string recordId);

        /// <summary>
        /// Reads all audit records from the repository.
        /// </summary>
        /// <returns>A list of all audit records.</returns>
        List<AuditsrvDomainModels.AuditRecord> ReadAllAuditRecords();
    }

    public interface ICicsAuditService
    {
        /// <summary>
        /// Initiates a CICS transaction for audit processing.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the CICS transaction.</param>
        /// <returns>A boolean indicating whether the transaction was successfully initiated.</returns>
        bool InitiateCicsTransaction(string transactionId);

        /// <summary>
        /// Completes a CICS transaction for audit processing.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the CICS transaction.</param>
        /// <returns>A boolean indicating whether the transaction was successfully completed.</returns>
        bool CompleteCicsTransaction(string transactionId);

        /// <summary>
        /// Retrieves the status of a CICS transaction.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the CICS transaction.</param>
        /// <returns>The status of the CICS transaction.</returns>
        string GetCicsTransactionStatus(string transactionId);
    }

    public interface IAuditProcessingService
    {
        /// <summary>
        /// Validates the audit record data.
        /// </summary>
        /// <param name="auditRecord">The audit record to validate.</param>
        /// <returns>A boolean indicating whether the audit record is valid.</returns>
        bool ValidateAuditRecord(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Processes the audit record for reporting or storage.
        /// </summary>
        /// <param name="auditRecord">The audit record to process.</param>
        void ProcessAuditRecord(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Generates a summary report for a given audit record.
        /// </summary>
        /// <param name="auditRecord">The audit record for which to generate the report.</param>
        /// <returns>A string containing the summary report.</returns>
        string GenerateAuditSummary(AuditsrvDomainModels.AuditRecord auditRecord);
    }
}