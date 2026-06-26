using System;
using System.Collections.Generic;
using TXNPOST.Domain;

namespace TXNPOST.Application.Interfaces
{
    /// <summary>
    /// Interface for file I/O operations related to transaction records.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads a transaction record by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to read.</param>
        /// <returns>The transaction record if found; otherwise, null.</returns>
        TxnpostDomainModels.TransactionRecord ReadTransactionById(string transactionId);

        /// <summary>
        /// Writes a transaction record to the repository.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to write.</param>
        void WriteTransaction(TxnpostDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Updates an existing transaction record in the repository.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to update.</param>
        void UpdateTransaction(TxnpostDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Deletes a transaction record by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to delete.</param>
        void DeleteTransaction(string transactionId);

        /// <summary>
        /// Reads all transaction records.
        /// </summary>
        /// <returns>A list of all transaction records.</returns>
        IEnumerable<TxnpostDomainModels.TransactionRecord> ReadAllTransactions();

        /// <summary>
        /// Searches for transaction records by a specific type.
        /// </summary>
        /// <param name="transactionType">The type of transactions to search for.</param>
        /// <returns>A list of transaction records matching the specified type.</returns>
        IEnumerable<TxnpostDomainModels.TransactionRecord> SearchTransactionsByType(TxnpostDomainModels.TransactionTypeEnum transactionType);
    }

    /// <summary>
    /// Interface for CICS service operations related to transaction processing.
    /// </summary>
    public interface ITransactionProcessingService
    {
        /// <summary>
        /// Posts a transaction to the processing system.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to post.</param>
        /// <returns>The status of the transaction after processing.</returns>
        TxnpostDomainModels.StatusEnum PostTransaction(TxnpostDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Retrieves the status of a transaction by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to check.</param>
        /// <returns>The current status of the transaction.</returns>
        TxnpostDomainModels.StatusEnum GetTransactionStatus(string transactionId);

        /// <summary>
        /// Cancels a transaction by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to cancel.</param>
        /// <returns>True if the transaction was successfully canceled; otherwise, false.</returns>
        bool CancelTransaction(string transactionId);

        /// <summary>
        /// Reverses a previously posted transaction by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to reverse.</param>
        /// <returns>The status of the transaction after reversal.</returns>
        TxnpostDomainModels.StatusEnum ReverseTransaction(string transactionId);

        /// <summary>
        /// Validates a transaction record before posting.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to validate.</param>
        /// <returns>True if the transaction is valid; otherwise, false.</returns>
        bool ValidateTransaction(TxnpostDomainModels.TransactionRecord transactionRecord);
    }
}