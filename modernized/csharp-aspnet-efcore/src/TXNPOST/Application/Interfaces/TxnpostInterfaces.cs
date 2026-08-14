using System;
using System.Collections.Generic;
using TXNPOST.Domain;

namespace TXNPOST.Application.Interfaces
{
    /// <summary>
    /// Interface for transaction processing operations.
    /// Each COBOL paragraph is represented as a method.
    /// </summary>
    public interface ITxnpostProcessor
    {
        /// <summary>
        /// Initializes the transaction processing environment.
        /// </summary>
        void InitializeEnvironment();

        /// <summary>
        /// Validates the input transaction record.
        /// </summary>
        /// <param name="transaction">The transaction record to validate.</param>
        /// <returns>True if the transaction is valid, otherwise false.</returns>
        bool ValidateTransaction(TxnpostDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Processes the transaction record.
        /// </summary>
        /// <param name="transaction">The transaction record to process.</param>
        void ProcessTransaction(TxnpostDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Finalizes the transaction processing and performs cleanup.
        /// </summary>
        void FinalizeProcessing();

        /// <summary>
        /// Logs the transaction processing details.
        /// </summary>
        /// <param name="transaction">The transaction record to log.</param>
        void LogTransactionDetails(TxnpostDomainModels.TransactionRecord transaction);
    }

    /// <summary>
    /// Interface for file I/O operations related to transactions.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads a transaction record from the input file.
        /// </summary>
        /// <returns>The transaction record read from the file.</returns>
        TxnpostDomainModels.TransactionRecord ReadTransaction();

        /// <summary>
        /// Writes a transaction record to the output file.
        /// </summary>
        /// <param name="transaction">The transaction record to write.</param>
        void WriteTransaction(TxnpostDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Updates a transaction record in the file.
        /// </summary>
        /// <param name="transaction">The transaction record to update.</param>
        void UpdateTransaction(TxnpostDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Deletes a transaction record from the file.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to delete.</param>
        void DeleteTransaction(string transactionId);
    }

    /// <summary>
    /// Interface for CICS service operations related to transactions.
    /// </summary>
    public interface ICicsTransactionService
    {
        /// <summary>
        /// Sends a transaction to the CICS system for processing.
        /// </summary>
        /// <param name="transaction">The transaction record to send.</param>
        void SendTransactionToCics(TxnpostDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Retrieves the status of a transaction from the CICS system.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to check.</param>
        /// <returns>The status of the transaction.</returns>
        TxnpostDomainModels.TransactionStatus GetTransactionStatus(string transactionId);

        /// <summary>
        /// Cancels a transaction in the CICS system.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to cancel.</param>
        void CancelTransactionInCics(string transactionId);
    }
}