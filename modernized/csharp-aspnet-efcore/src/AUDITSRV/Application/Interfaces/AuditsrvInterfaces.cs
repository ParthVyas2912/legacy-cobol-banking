using System.Collections.Generic;
using AUDITSRV.Domain;

namespace AUDITSRV.Application.Interfaces
{
    /// <summary>
    /// Interface for Audit Service operations.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Initializes the audit service.
        /// </summary>
        void InitializeAuditService();

        /// <summary>
        /// Processes the audit records.
        /// </summary>
        void ProcessAuditRecords();

        /// <summary>
        /// Finalizes the audit service.
        /// </summary>
        void FinalizeAuditService();
    }

    /// <summary>
    /// Interface for Audit Repository operations.
    /// </summary>
    public interface IAuditRepository
    {
        /// <summary>
        /// Reads an audit record by its record ID.
        /// </summary>
        /// <param name="recordId">The record ID of the audit record to read.</param>
        /// <returns>The audit record with the specified record ID.</returns>
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
        /// Deletes an audit record by its record ID.
        /// </summary>
        /// <param name="recordId">The record ID of the audit record to delete.</param>
        void DeleteAuditRecord(string recordId);

        /// <summary>
        /// Retrieves all audit records from the repository.
        /// </summary>
        /// <returns>A list of all audit records.</returns>
        IEnumerable<AuditsrvDomainModels.AuditRecord> GetAllAuditRecords();
    }

    /// <summary>
    /// Interface for CICS operations related to the audit service.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Sends a transaction to the CICS system.
        /// </summary>
        /// <param name="transactionId">The transaction ID to send.</param>
        /// <param name="data">The data to send with the transaction.</param>
        /// <returns>The response from the CICS system.</returns>
        string SendTransaction(string transactionId, string data);

        /// <summary>
        /// Receives a response from the CICS system.
        /// </summary>
        /// <param name="transactionId">The transaction ID for which to receive a response.</param>
        /// <returns>The response data from the CICS system.</returns>
        string ReceiveTransactionResponse(string transactionId);
    }
}