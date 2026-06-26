using System.Collections.Generic;
using ATMTXN.Domain;

namespace ATMTXN.Application.Interfaces
{
    /// <summary>
    /// Interface for ATM transaction processing logic.
    /// Each COBOL paragraph is represented as a method.
    /// </summary>
    public interface IAtmtxnProcessor
    {
        /// <summary>
        /// Initializes the transaction processing environment.
        /// </summary>
        void InitializeEnvironment();

        /// <summary>
        /// Validates the input transaction details.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to validate.</param>
        /// <returns>True if the transaction is valid, otherwise false.</returns>
        bool ValidateTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Processes the transaction based on its type.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to process.</param>
        void ProcessTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Finalizes the transaction and updates the status.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to finalize.</param>
        void FinalizeTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Logs the transaction details for audit purposes.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to log.</param>
        void LogTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord);
    }

    /// <summary>
    /// Interface for file I/O operations related to ATM transactions.
    /// </summary>
    public interface IAtmtxnFileRepository
    {
        /// <summary>
        /// Reads a transaction record from the file.
        /// </summary>
        /// <param name="transactionId">The transaction ID to read.</param>
        /// <returns>The transaction record if found, otherwise null.</returns>
        AtmtxnDomainModels.TransactionRecord ReadTransaction(string transactionId);

        /// <summary>
        /// Writes a transaction record to the file.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to write.</param>
        void WriteTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Updates a transaction record in the file.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to update.</param>
        void UpdateTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Deletes a transaction record from the file.
        /// </summary>
        /// <param name="transactionId">The transaction ID to delete.</param>
        void DeleteTransaction(string transactionId);
    }

    /// <summary>
    /// Interface for CICS operations related to ATM transactions.
    /// </summary>
    public interface IAtmtxnCicsService
    {
        /// <summary>
        /// Sends a transaction request to the CICS system.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to send.</param>
        /// <returns>The response status code from the CICS system.</returns>
        AtmtxnDomainModels.StatusCodeEnum SendTransactionRequest(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Retrieves the status of a transaction from the CICS system.
        /// </summary>
        /// <param name="transactionId">The transaction ID to check.</param>
        /// <returns>The status code of the transaction.</returns>
        AtmtxnDomainModels.StatusCodeEnum GetTransactionStatus(string transactionId);
    }
}