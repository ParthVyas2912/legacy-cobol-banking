using System;
using System.Collections.Generic;
using AUDITSRV.Domain;

namespace AUDITSRV.Application.Interfaces
{
    /// <summary>
    /// Interface for repository operations related to Audit records.
    /// </summary>
    public interface IAuditRepository
    {
        /// <summary>
        /// Reads an audit record by its ID.
        /// </summary>
        /// <param name="auditId">The ID of the audit record to retrieve.</param>
        /// <returns>The audit record if found, otherwise null.</returns>
        AuditsrvDomainModels.AuditRecord ReadAuditById(string auditId);

        /// <summary>
        /// Writes an audit record to the repository.
        /// </summary>
        /// <param name="auditRecord">The audit record to write.</param>
        void WriteAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Updates an existing audit record in the repository.
        /// </summary>
        /// <param name="auditRecord">The audit record to update.</param>
        void UpdateAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Deletes an audit record by its ID.
        /// </summary>
        /// <param name="auditId">The ID of the audit record to delete.</param>
        void DeleteAudit(string auditId);

        /// <summary>
        /// Retrieves all audit records.
        /// </summary>
        /// <returns>A list of all audit records.</returns>
        List<AuditsrvDomainModels.AuditRecord> GetAllAudits();
    }

    /// <summary>
    /// Interface for CICS service operations related to Audit processing.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Initiates the audit process.
        /// </summary>
        /// <param name="auditRecord">The audit record to process.</param>
        void InitiateAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Completes the audit process.
        /// </summary>
        /// <param name="auditId">The ID of the audit to complete.</param>
        void CompleteAudit(string auditId);

        /// <summary>
        /// Cancels an ongoing audit process.
        /// </summary>
        /// <param name="auditId">The ID of the audit to cancel.</param>
        void CancelAudit(string auditId);

        /// <summary>
        /// Retrieves the status of a specific audit.
        /// </summary>
        /// <param name="auditId">The ID of the audit to check.</param>
        /// <returns>The status of the audit.</returns>
        AuditsrvDomainModels.AuditStatus GetAuditStatus(string auditId);

        /// <summary>
        /// Generates a report for a specific audit.
        /// </summary>
        /// <param name="auditId">The ID of the audit to generate a report for.</param>
        /// <returns>The generated audit report as a string.</returns>
        string GenerateAuditReport(string auditId);
    }
}