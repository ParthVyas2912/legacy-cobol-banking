using System.Collections.Generic;
using ATMTXN.Domain;

namespace ATMTXN.Application.Interfaces
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads a transaction record from the data store.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction.</param>
        /// <returns>The transaction record.</returns>
        AtmtxnDomainModels.TransactionRecord ReadTransaction(string transactionId);

        /// <summary>
        /// Writes a transaction record to the data store.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to write.</param>
        void WriteTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Updates a transaction record in the data store.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to update.</param>
        void UpdateTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Deletes a transaction record from the data store.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction to delete.</param>
        void DeleteTransaction(string transactionId);

        /// <summary>
        /// Retrieves all transactions for a specific account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A list of transaction records.</returns>
        List<AtmtxnDomainModels.TransactionRecord> GetTransactionsByAccount(string accountNumber);
    }

    public interface ITransactionService
    {
        /// <summary>
        /// Processes a transaction based on the provided transaction record.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to process.</param>
        /// <returns>The updated transaction record after processing.</returns>
        AtmtxnDomainModels.TransactionRecord ProcessTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Validates a transaction record.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to validate.</param>
        /// <returns>A boolean indicating whether the transaction is valid.</returns>
        bool ValidateTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Retrieves the current balance for a given account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve the balance for.</param>
        /// <returns>The current balance of the account.</returns>
        decimal GetAccountBalance(string accountNumber);

        /// <summary>
        /// Logs an error related to a transaction.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction.</param>
        /// <param name="errorCode">The error code associated with the transaction.</param>
        /// <param name="errorMessage">The error message associated with the transaction.</param>
        void LogTransactionError(string transactionId, string errorCode, string errorMessage);

        /// <summary>
        /// Reverses a transaction based on the provided transaction ID.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction to reverse.</param>
        /// <returns>The updated transaction record after reversal.</returns>
        AtmtxnDomainModels.TransactionRecord ReverseTransaction(string transactionId);

        /// <summary>
        /// Generates a summary report for transactions within a specific date range.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A summary report of transactions.</returns>
        List<AtmtxnDomainModels.TransactionRecord> GenerateTransactionReport(int startDate, int endDate);
    }
}