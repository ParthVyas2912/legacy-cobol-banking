using System.Collections.Generic;
using ATMTXN.Domain;

namespace ATMTXN.Application.Interfaces
{
    /// <summary>
    /// Interface for handling ATM transaction operations.
    /// Each COBOL paragraph is represented as a method.
    /// </summary>
    public interface IAtmtxnService
    {
        /// <summary>
        /// Initializes the transaction processing environment.
        /// </summary>
        void InitializeTransactionEnvironment();

        /// <summary>
        /// Validates the transaction request.
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
        /// Updates the account balance after a transaction.
        /// </summary>
        /// <param name="transactionRecord">The transaction record with updated balance.</param>
        void UpdateAccountBalance(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Logs the transaction details for audit purposes.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to log.</param>
        void LogTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Finalizes the transaction processing environment.
        /// </summary>
        void FinalizeTransactionEnvironment();
    }

    /// <summary>
    /// Interface for handling file I/O operations related to transaction records.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads a transaction record from the file.
        /// </summary>
        /// <param name="transactionId">The transaction ID to read.</param>
        /// <returns>The transaction record.</returns>
        AtmtxnDomainModels.TransactionRecord ReadTransactionRecord(string transactionId);

        /// <summary>
        /// Writes a transaction record to the file.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to write.</param>
        void WriteTransactionRecord(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Updates a transaction record in the file.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to update.</param>
        void UpdateTransactionRecord(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Deletes a transaction record from the file.
        /// </summary>
        /// <param name="transactionId">The transaction ID to delete.</param>
        void DeleteTransactionRecord(string transactionId);
    }

    /// <summary>
    /// Interface for handling CICS operations related to ATM transactions.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Sends a transaction request to the CICS system.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to send.</param>
        /// <returns>The response from the CICS system.</returns>
        AtmtxnDomainModels.StatusCodeEnum SendTransactionRequest(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Retrieves account details from the CICS system.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve details for.</param>
        /// <returns>The account details as a transaction record.</returns>
        AtmtxnDomainModels.TransactionRecord RetrieveAccountDetails(string accountNumber);

        /// <summary>
        /// Sends a system error notification to the CICS system.
        /// </summary>
        /// <param name="errorCode">The error code to send.</param>
        void SendSystemErrorNotification(string errorCode);
    }
}