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
        /// Reads an audit record by its ID.
        /// </summary>
        /// <param name="auditId">The ID of the audit record to read.</param>
        /// <returns>The audit record if found; otherwise, null.</returns>
        AuditsrvDomainModels.AuditRecord ReadAuditById(string auditId);

        /// <summary>
        /// Writes an audit record to the repository.
        /// </summary>
        /// <param name="auditRecord">The audit record to write.</param>
        void WriteAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Deletes an audit record by its ID.
        /// </summary>
        /// <param name="auditId">The ID of the audit record to delete.</param>
        void DeleteAuditById(string auditId);
    }

    /// <summary>
    /// Interface for CICS service operations related to audits.
    /// </summary>
    public interface IAuditCicsService
    {
        /// <summary>
        /// Sends an audit record to an external CICS system.
        /// </summary>
        /// <param name="auditRecord">The audit record to send.</param>
        void SendAuditToCics(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Retrieves an audit record from an external CICS system by its ID.
        /// </summary>
        /// <param name="auditId">The ID of the audit record to retrieve.</param>
        /// <returns>The audit record if found; otherwise, null.</returns>
        AuditsrvDomainModels.AuditRecord RetrieveAuditFromCics(string auditId);
    }

    /// <summary>
    /// Interface for application-level operations related to audits.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Processes an audit record.
        /// </summary>
        /// <param name="auditRecord">The audit record to process.</param>
        void ProcessAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Validates an audit record.
        /// </summary>
        /// <param name="auditRecord">The audit record to validate.</param>
        /// <returns>True if the audit record is valid; otherwise, false.</returns>
        bool ValidateAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Generates a report for a specific audit ID.
        /// </summary>
        /// <param name="auditId">The ID of the audit to generate a report for.</param>
        /// <returns>A string representing the generated report.</returns>
        string GenerateAuditReport(string auditId);
    }
}