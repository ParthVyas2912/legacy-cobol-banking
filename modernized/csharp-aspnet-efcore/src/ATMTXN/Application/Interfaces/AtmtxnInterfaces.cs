using System;
using System.Collections.Generic;
using ATMTXN.Domain;

namespace ATMTXN.Application.Interfaces
{
    /// <summary>
    /// Interface for handling ATM transaction operations.
    /// Each method corresponds to a COBOL paragraph or operation.
    /// </summary>
    public interface IAtmtxnService
    {
        /// <summary>
        /// Initializes the transaction processing environment.
        /// </summary>
        void InitializeTransactionEnvironment();

        /// <summary>
        /// Validates the input transaction details.
        /// </summary>
        /// <param name="transaction">The transaction record to validate.</param>
        /// <returns>True if the transaction is valid; otherwise, false.</returns>
        bool ValidateTransaction(AtmtxnDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Processes the transaction based on its type (e.g., withdrawal, deposit, transfer).
        /// </summary>
        /// <param name="transaction">The transaction record to process.</param>
        void ProcessTransaction(AtmtxnDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Calculates any applicable fees for the transaction.
        /// </summary>
        /// <param name="transaction">The transaction record for which to calculate fees.</param>
        void CalculateTransactionFees(AtmtxnDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Updates the account balance after processing the transaction.
        /// </summary>
        /// <param name="transaction">The transaction record containing updated account details.</param>
        void UpdateAccountBalance(AtmtxnDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Logs the transaction details for audit and tracking purposes.
        /// </summary>
        /// <param name="transaction">The transaction record to log.</param>
        void LogTransaction(AtmtxnDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Finalizes the transaction processing and performs any necessary cleanup.
        /// </summary>
        void FinalizeTransactionProcessing();
    }

    /// <summary>
    /// Interface for repository operations related to transaction records.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads a transaction record from the data store.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to retrieve.</param>
        /// <returns>The transaction record, or null if not found.</returns>
        AtmtxnDomainModels.TransactionRecord ReadTransaction(string transactionId);

        /// <summary>
        /// Writes a transaction record to the data store.
        /// </summary>
        /// <param name="transaction">The transaction record to write.</param>
        void WriteTransaction(AtmtxnDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Updates an existing transaction record in the data store.
        /// </summary>
        /// <param name="transaction">The transaction record to update.</param>
        void UpdateTransaction(AtmtxnDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Deletes a transaction record from the data store.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to delete.</param>
        void DeleteTransaction(string transactionId);
    }

    /// <summary>
    /// Interface for CICS operations related to ATM transactions.
    /// </summary>
    public interface ICicsTransactionService
    {
        /// <summary>
        /// Sends a transaction request to the CICS system.
        /// </summary>
        /// <param name="transaction">The transaction record to send.</param>
        /// <returns>True if the request was successful; otherwise, false.</returns>
        bool SendTransactionRequest(AtmtxnDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Retrieves a response from the CICS system for a previously sent transaction.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to retrieve the response for.</param>
        /// <returns>The transaction record with updated details, or null if no response is available.</returns>
        AtmtxnDomainModels.TransactionRecord RetrieveTransactionResponse(string transactionId);
    }
}