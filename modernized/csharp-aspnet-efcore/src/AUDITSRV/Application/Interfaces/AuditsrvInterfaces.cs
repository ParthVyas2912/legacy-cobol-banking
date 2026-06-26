using System;
using System.Collections.Generic;
using AUDITSRV.Domain;

namespace AUDITSRV.Application.Interfaces
{
    /// <summary>
    /// Interface for repository operations related to audit records.
    /// </summary>
    public interface IAuditRepository
    {
        /// <summary>
        /// Reads an audit record by its unique identifier.
        /// </summary>
        /// <param name="auditId">The unique identifier of the audit record.</param>
        /// <returns>The audit record if found; otherwise, null.</returns>
        AuditsrvDomainModels.AuditRecord ReadAuditRecord(string auditId);

        /// <summary>
        /// Writes an audit record to the repository.
        /// </summary>
        /// <param name="auditRecord">The audit record to write.</param>
        void WriteAuditRecord(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Deletes an audit record by its unique identifier.
        /// </summary>
        /// <param name="auditId">The unique identifier of the audit record to delete.</param>
        void DeleteAuditRecord(string auditId);

        /// <summary>
        /// Retrieves all audit records.
        /// </summary>
        /// <returns>A collection of all audit records.</returns>
        IEnumerable<AuditsrvDomainModels.AuditRecord> GetAllAuditRecords();
    }

    /// <summary>
    /// Interface for CICS service operations related to audits.
    /// </summary>
    public interface IAuditCicsService
    {
        /// <summary>
        /// Invokes a CICS transaction to process an audit record.
        /// </summary>
        /// <param name="auditRecord">The audit record to process.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        AuditsrvDomainModels.AuditStatus ProcessAuditRecord(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Retrieves the status of a specific audit record from the CICS system.
        /// </summary>
        /// <param name="auditId">The unique identifier of the audit record.</param>
        /// <returns>The status of the audit record.</returns>
        AuditsrvDomainModels.AuditStatus GetAuditStatus(string auditId);
    }

    /// <summary>
    /// Interface for application-level operations related to audits.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Validates an audit record before processing.
        /// </summary>
        /// <param name="auditRecord">The audit record to validate.</param>
        /// <returns>True if the audit record is valid; otherwise, false.</returns>
        bool ValidateAuditRecord(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Processes an audit record by performing validation, repository operations, and invoking CICS services.
        /// </summary>
        /// <param name="auditRecord">The audit record to process.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        AuditsrvDomainModels.AuditStatus ProcessAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Retrieves a detailed audit report for a specific audit record.
        /// </summary>
        /// <param name="auditId">The unique identifier of the audit record.</param>
        /// <returns>The detailed audit record if found; otherwise, null.</returns>
        AuditsrvDomainModels.AuditRecord GetAuditReport(string auditId);
    }
}