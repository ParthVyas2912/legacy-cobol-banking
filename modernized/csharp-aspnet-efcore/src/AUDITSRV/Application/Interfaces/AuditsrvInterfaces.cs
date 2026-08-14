using System;
using AUDITSRV.Domain;

namespace AUDITSRV.Application.Interfaces
{
    public interface IAuditRepository
    {
        /// <summary>
        /// Reads an audit record by its ID.
        /// </summary>
        /// <param name="auditId">The ID of the audit record to retrieve.</param>
        /// <returns>The audit record if found, otherwise null.</returns>
        AuditsrvDomainModels.AuditRecord ReadAuditById(string auditId);

        /// <summary>
        /// Writes a new audit record to the repository.
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
    }

    public interface IAuditService
    {
        /// <summary>
        /// Validates an audit record.
        /// </summary>
        /// <param name="auditRecord">The audit record to validate.</param>
        /// <returns>True if the audit record is valid, otherwise false.</returns>
        bool ValidateAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Processes an audit record.
        /// </summary>
        /// <param name="auditRecord">The audit record to process.</param>
        /// <returns>The processed audit details.</returns>
        AuditsrvDomainModels.AuditDetails ProcessAudit(AuditsrvDomainModels.AuditRecord auditRecord);

        /// <summary>
        /// Generates an audit log.
        /// </summary>
        /// <returns>The generated audit log.</returns>
        AuditsrvDomainModels.AuditLog GenerateAuditLog();
    }
}