using System.Collections.Generic;
using System.Threading.Tasks;
using AUDITSRV.Domain;

namespace AUDITSRV.Application.Interfaces
{
    public interface IAuditService
    {
        /// <summary>
        /// Executes the main logic of the AUDITSRV program.
        /// </summary>
        Task ExecuteAsync();
    }

    public interface IAuditRepository
    {
        /// <summary>
        /// Retrieves an audit record by its unique identifier.
        /// </summary>
        /// <param name="auditId">The unique identifier of the audit record.</param>
        /// <returns>The audit record if found; otherwise, null.</returns>
        Task<AuditsrvDomainModels.AuditRecord?> GetAuditRecordByIdAsync(string auditId);

        /// <summary>
        /// Retrieves all audit records.
        /// </summary>
        /// <returns>A list of all audit records.</returns>
        Task<List<AuditsrvDomainModels.AuditRecord>> GetAllAuditRecordsAsync();

        /// <summary>
        /// Adds a new audit record.
        /// </summary>
        /// <param name="auditRecord">The audit record to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAuditRecordAsync(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Updates an existing audit record.
        /// </summary>
        /// <param name="auditRecord">The audit record to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAuditRecordAsync(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Deletes an audit record by its unique identifier.
        /// </summary>
        /// <param name="auditId">The unique identifier of the audit record to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAuditRecordAsync(string auditId);
    }

    public interface IAuditSummaryRepository
    {
        /// <summary>
        /// Retrieves an audit summary by its unique identifier.
        /// </summary>
        /// <param name="summaryId">The unique identifier of the audit summary.</param>
        /// <returns>The audit summary if found; otherwise, null.</returns>
        Task<AuditsrvDomainModels.AuditSummary?> GetAuditSummaryByIdAsync(string summaryId);

        /// <summary>
        /// Retrieves all audit summaries.
        /// </summary>
        /// <returns>A list of all audit summaries.</returns>
        Task<List<AuditsrvDomainModels.AuditSummary>> GetAllAuditSummariesAsync();

        /// <summary>
        /// Adds a new audit summary.
        /// </summary>
        /// <param name="auditSummary">The audit summary to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAuditSummaryAsync(AuditsrvDomainModels.AuditSummary auditSummary);

        /// <summary>
        /// Updates an existing audit summary.
        /// </summary>
        /// <param name="auditSummary">The audit summary to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAuditSummaryAsync(AuditsrvDomainModels.AuditSummary auditSummary);

        /// <summary>
        /// Deletes an audit summary by its unique identifier.
        /// </summary>
        /// <param name="summaryId">The unique identifier of the audit summary to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAuditSummaryAsync(string summaryId);
    }
}