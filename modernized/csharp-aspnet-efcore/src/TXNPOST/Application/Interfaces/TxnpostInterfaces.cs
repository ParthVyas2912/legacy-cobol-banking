using System;
using System.Collections.Generic;
using TXNPOST.Domain;

namespace TXNPOST.Application.Interfaces
{
    /// <summary>
    /// Interface for transaction processing operations.
    /// Each method corresponds to a paragraph in the original COBOL program.
    /// </summary>
    public interface ITxnpostProcessor
    {
        /// <summary>
        /// Initializes the transaction processing environment.
        /// </summary>
        void InitializeEnvironment();

        /// <summary>
        /// Validates the input transaction data.
        /// </summary>
        /// <param name="transaction">The transaction to validate.</param>
        void ValidateTransaction(TxnpostDomainModels.Transaction transaction);

        /// <summary>
        /// Posts the transaction to the account.
        /// </summary>
        /// <param name="transaction">The transaction to post.</param>
        void PostTransaction(TxnpostDomainModels.Transaction transaction);

        /// <summary>
        /// Updates the account balance after posting the transaction.
        /// </summary>
        /// <param name="transaction">The transaction that was posted.</param>
        void UpdateAccountBalance(TxnpostDomainModels.Transaction transaction);

        /// <summary>
        /// Logs the transaction details for audit purposes.
        /// </summary>
        /// <param name="transaction">The transaction to log.</param>
        void LogTransaction(TxnpostDomainModels.Transaction transaction);

        /// <summary>
        /// Finalizes the transaction processing and cleans up resources.
        /// </summary>
        void FinalizeProcessing();

        /// <summary>
        /// Handles errors encountered during transaction processing.
        /// </summary>
        /// <param name="errorMessage">The error message to log or process.</param>
        void HandleProcessingError(string errorMessage);
    }

    /// <summary>
    /// Interface for repository operations related to transaction data.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Retrieves a transaction by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to retrieve.</param>
        /// <returns>The transaction with the specified ID.</returns>
        TxnpostDomainModels.Transaction GetTransactionById(string transactionId);

        /// <summary>
        /// Saves a transaction to the repository.
        /// </summary>
        /// <param name="transaction">The transaction to save.</param>
        void SaveTransaction(TxnpostDomainModels.Transaction transaction);

        /// <summary>
        /// Updates an existing transaction in the repository.
        /// </summary>
        /// <param name="transaction">The transaction to update.</param>
        void UpdateTransaction(TxnpostDomainModels.Transaction transaction);

        /// <summary>
        /// Deletes a transaction from the repository.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to delete.</param>
        void DeleteTransaction(string transactionId);

        /// <summary>
        /// Retrieves all transactions for a specific account.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>A list of transactions associated with the account.</returns>
        List<TxnpostDomainModels.Transaction> GetTransactionsByAccountId(string accountId);
    }

    /// <summary>
    /// Interface for CICS-related service operations.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Commits the current transaction in the CICS environment.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back the current transaction in the CICS environment.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Sends a message to the CICS queue.
        /// </summary>
        /// <param name="message">The message to send.</param>
        void SendMessage(string message);

        /// <summary>
        /// Receives a message from the CICS queue.
        /// </summary>
        /// <returns>The received message.</returns>
        string ReceiveMessage();

        /// <summary>
        /// Retrieves the status of a specific transaction in the CICS environment.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction.</param>
        /// <returns>The status of the transaction.</returns>
        TxnpostDomainModels.TransactionStatus GetTransactionStatus(string transactionId);
    }
}