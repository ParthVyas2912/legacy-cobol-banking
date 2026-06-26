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
        /// <param name="auditId">The ID of the audit record to retrieve.</param>
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
        void DeleteAudit(string auditId);

        /// <summary>
        /// Updates an existing audit record in the repository.
        /// </summary>
        /// <param name="auditRecord">The audit record to update.</param>
        void UpdateAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Retrieves all audit records from the repository.
        /// </summary>
        /// <returns>A list of all audit records.</returns>
        List<AuditsrvDomainModels.AuditRecord> GetAllAudits();
    }

    /// <summary>
    /// Interface for CICS service operations related to audits.
    /// </summary>
    public interface IAuditCicsService
    {
        /// <summary>
        /// Initiates an audit process in the CICS system.
        /// </summary>
        /// <param name="auditRecord">The audit record to process.</param>
        void InitiateAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Retrieves the status of an audit from the CICS system.
        /// </summary>
        /// <param name="auditId">The ID of the audit to check.</param>
        /// <returns>The status of the audit.</returns>
        AuditsrvDomainModels.AuditStatusEnum GetAuditStatus(string auditId);

        /// <summary>
        /// Cancels an audit process in the CICS system.
        /// </summary>
        /// <param name="auditId">The ID of the audit to cancel.</param>
        void CancelAudit(string auditId);

        /// <summary>
        /// Retrieves detailed information about an audit from the CICS system.
        /// </summary>
        /// <param name="auditId">The ID of the audit to retrieve details for.</param>
        /// <returns>The detailed information about the audit.</returns>
        AuditsrvDomainModels.AuditDetails GetAuditDetails(string auditId);
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
        /// Generates a report for a specific audit.
        /// </summary>
        /// <param name="auditId">The ID of the audit to generate a report for.</param>
        /// <returns>A string containing the audit report.</returns>
        string GenerateAuditReport(string auditId);

        /// <summary>
        /// Archives an audit record.
        /// </summary>
        /// <param name="auditId">The ID of the audit to archive.</param>
        void ArchiveAudit(string auditId);

        /// <summary>
        /// Retrieves a summary of all audits.
        /// </summary>
        /// <returns>A summary report of all audits.</returns>
        string GetAuditSummary();
    }
}