using System;
using AUDITSRV.Domain;

namespace AUDITSRV.Application.Interfaces
{
    /// <summary>
    /// Interface for Audit Service operations.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Initializes the audit service and prepares it for processing.
        /// </summary>
        void InitializeService();

        /// <summary>
        /// Processes the audit records based on the input parameters.
        /// </summary>
        /// <param name="auditRecord">The audit record to process.</param>
        void ProcessAuditRecord(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Finalizes the audit service and performs any necessary cleanup.
        /// </summary>
        void FinalizeService();
    }

    /// <summary>
    /// Interface for Audit Repository operations.
    /// </summary>
    public interface IAuditRepository
    {
        /// <summary>
        /// Reads an audit record from the data store.
        /// </summary>
        /// <param name="auditId">The ID of the audit record to read.</param>
        /// <returns>The audit record corresponding to the given ID.</returns>
        AuditsrvDomainModels.AuditRecord ReadAuditRecord(string auditId);

        /// <summary>
        /// Writes an audit record to the data store.
        /// </summary>
        /// <param name="auditRecord">The audit record to write.</param>
        void WriteAuditRecord(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Updates an existing audit record in the data store.
        /// </summary>
        /// <param name="auditRecord">The audit record to update.</param>
        void UpdateAuditRecord(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Deletes an audit record from the data store.
        /// </summary>
        /// <param name="auditId">The ID of the audit record to delete.</param>
        void DeleteAuditRecord(string auditId);

        /// <summary>
        /// Retrieves all audit records from the data store.
        /// </summary>
        /// <returns>A collection of all audit records.</returns>
        IEnumerable<AuditsrvDomainModels.AuditRecord> GetAllAuditRecords();

        /// <summary>
        /// Retrieves audit records filtered by user ID.
        /// </summary>
        /// <param name="userId">The user ID to filter by.</param>
        /// <returns>A collection of audit records for the specified user ID.</returns>
        IEnumerable<AuditsrvDomainModels.AuditRecord> GetAuditRecordsByUserId(string userId);

        /// <summary>
        /// Retrieves audit records filtered by status.
        /// </summary>
        /// <param name="status">The status to filter by.</param>
        /// <returns>A collection of audit records with the specified status.</returns>
        IEnumerable<AuditsrvDomainModels.AuditRecord> GetAuditRecordsByStatus(AuditsrvDomainModels.AuditStatus status);
    }
}