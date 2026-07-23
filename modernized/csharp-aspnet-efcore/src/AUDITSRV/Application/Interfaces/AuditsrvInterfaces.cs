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
        void DeleteAudit(string auditId);
    }

    /// <summary>
    /// Interface for CICS service operations related to audits.
    /// </summary>
    public interface IAuditCicsService
    {
        /// <summary>
        /// Executes a CICS transaction to retrieve audit data.
        /// </summary>
        /// <param name="transactionId">The ID of the CICS transaction.</param>
        /// <returns>A list of audit records retrieved by the transaction.</returns>
        List<AuditsrvDomainModels.AuditRecord> ExecuteRetrieveAuditTransaction(string transactionId);

        /// <summary>
        /// Executes a CICS transaction to update audit data.
        /// </summary>
        /// <param name="transactionId">The ID of the CICS transaction.</param>
        /// <param name="auditRecord">The audit record to update.</param>
        void ExecuteUpdateAuditTransaction(string transactionId, AuditsrvDomainModels.AuditRecord auditRecord);
    }

    /// <summary>
    /// Interface for application-level operations related to audits.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Retrieves an audit record by its ID.
        /// </summary>
        /// <param name="auditId">The ID of the audit record to retrieve.</param>
        /// <returns>The audit record if found; otherwise, null.</returns>
        AuditsrvDomainModels.AuditRecord GetAuditById(string auditId);

        /// <summary>
        /// Creates or updates an audit record.
        /// </summary>
        /// <param name="auditRecord">The audit record to create or update.</param>
        void SaveAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Deletes an audit record by its ID.
        /// </summary>
        /// <param name="auditId">The ID of the audit record to delete.</param>
        void RemoveAudit(string auditId);
    }
}