using System;
using System.Collections.Generic;
using AUDITSRV.Domain;

namespace AUDITSRV.Application.Interfaces
{
    /// <summary>
    /// Interface for repository operations related to Audit Records.
    /// </summary>
    public interface IAuditRepository
    {
        /// <summary>
        /// Reads an audit record by its unique identifier.
        /// </summary>
        /// <param name="auditId">The unique identifier of the audit record.</param>
        /// <returns>The audit record if found, otherwise null.</returns>
        AuditsrvDomainModels.AuditRecord ReadAuditById(string auditId);

        /// <summary>
        /// Writes a new audit record to the repository.
        /// </summary>
        /// <param name="auditRecord">The audit record to be written.</param>
        void WriteAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Updates an existing audit record in the repository.
        /// </summary>
        /// <param name="auditRecord">The audit record with updated information.</param>
        void UpdateAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Deletes an audit record by its unique identifier.
        /// </summary>
        /// <param name="auditId">The unique identifier of the audit record to be deleted.</param>
        void DeleteAudit(string auditId);
    }

    /// <summary>
    /// Interface for CICS service operations related to Audit Records.
    /// </summary>
    public interface IAuditCicsService
    {
        /// <summary>
        /// Initiates a CICS transaction for processing an audit record.
        /// </summary>
        /// <param name="auditRecord">The audit record to be processed.</param>
        /// <returns>The status of the CICS transaction.</returns>
        AuditsrvDomainModels.AuditStatus ProcessAuditTransaction(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Retrieves the status of a CICS transaction by its unique identifier.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the CICS transaction.</param>
        /// <returns>The status of the transaction.</returns>
        AuditsrvDomainModels.AuditStatus GetTransactionStatus(string transactionId);
    }

    /// <summary>
    /// Interface for application-level operations related to Audit Records.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Validates an audit record for correctness and completeness.
        /// </summary>
        /// <param name="auditRecord">The audit record to be validated.</param>
        /// <returns>True if the record is valid, otherwise false.</returns>
        bool ValidateAuditRecord(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Processes an audit record, including validation, repository storage, and CICS transaction initiation.
        /// </summary>
        /// <param name="auditRecord">The audit record to be processed.</param>
        /// <returns>The final status of the audit record after processing.</returns>
        AuditsrvDomainModels.AuditStatus ProcessAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Retrieves a list of audit records based on filtering criteria.
        /// </summary>
        /// <param name="userId">The user identifier to filter audit records (optional).</param>
        /// <param name="status">The status to filter audit records (optional).</param>
        /// <returns>A list of audit records matching the criteria.</returns>
        IEnumerable<AuditsrvDomainModels.AuditRecord> GetAuditRecords(string userId = null, AuditsrvDomainModels.AuditStatus? status = null);
    }
}