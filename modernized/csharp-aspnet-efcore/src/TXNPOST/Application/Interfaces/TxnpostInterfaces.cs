using System;
using System.Collections.Generic;
using TXNPOST.Domain;

namespace TXNPOST.Application.Interfaces
{
    // Interface for file I/O operations
    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads a transaction record by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to read.</param>
        /// <returns>The transaction record if found, otherwise null.</returns>
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
        /// Reads all transaction records within a specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A list of transaction records within the specified date range.</returns>
        List<TxnpostDomainModels.TransactionRecord> ReadTransactionsByDateRange(DateTime startDate, DateTime endDate);
    }

    // Interface for CICS operations
    public interface ITransactionService
    {
        /// <summary>
        /// Posts a transaction to the CICS system.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to post.</param>
        /// <returns>True if the transaction was successfully posted, otherwise false.</returns>
        bool PostTransaction(TxnpostDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Retrieves the status of a transaction from the CICS system.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to check.</param>
        /// <returns>The status of the transaction.</returns>
        TxnpostDomainModels.StatusEnum GetTransactionStatus(string transactionId);

        /// <summary>
        /// Cancels a transaction in the CICS system.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to cancel.</param>
        /// <returns>True if the transaction was successfully canceled, otherwise false.</returns>
        bool CancelTransaction(string transactionId);

        /// <summary>
        /// Retrieves a transaction record from the CICS system by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to retrieve.</param>
        /// <returns>The transaction record if found, otherwise null.</returns>
        TxnpostDomainModels.TransactionRecord RetrieveTransactionFromCICS(string transactionId);
    }

    // Interface for application-level transaction processing
    public interface ITransactionProcessor
    {
        /// <summary>
        /// Validates a transaction record.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to validate.</param>
        /// <returns>True if the transaction is valid, otherwise false.</returns>
        bool ValidateTransaction(TxnpostDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Processes a transaction by posting it and updating its status.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to process.</param>
        void ProcessTransaction(TxnpostDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Generates a report of all transactions within a specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A list of transaction records within the specified date range.</returns>
        List<TxnpostDomainModels.TransactionRecord> GenerateTransactionReport(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Reverses a transaction by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to reverse.</param>
        /// <returns>True if the transaction was successfully reversed, otherwise false.</returns>
        bool ReverseTransaction(string transactionId);
    }
}