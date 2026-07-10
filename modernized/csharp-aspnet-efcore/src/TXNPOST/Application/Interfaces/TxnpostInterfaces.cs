using System;
using System.Collections.Generic;
using TXNPOST.Domain;

namespace TXNPOST.Application.Interfaces
{
    /// <summary>
    /// Interface for handling transaction processing logic.
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
        /// Processes the transaction and applies business rules.
        /// </summary>
        /// <param name="transaction">The transaction to process.</param>
        void ProcessTransaction(TxnpostDomainModels.Transaction transaction);

        /// <summary>
        /// Calculates fees and taxes for the transaction.
        /// </summary>
        /// <param name="transaction">The transaction for which fees and taxes are calculated.</param>
        void CalculateFeesAndTaxes(TxnpostDomainModels.Transaction transaction);

        /// <summary>
        /// Updates the transaction status based on processing results.
        /// </summary>
        /// <param name="transaction">The transaction to update.</param>
        void UpdateTransactionStatus(TxnpostDomainModels.Transaction transaction);

        /// <summary>
        /// Finalizes the transaction processing and performs cleanup.
        /// </summary>
        void FinalizeProcessing();
    }

    /// <summary>
    /// Interface for transaction repository operations.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads a transaction by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to read.</param>
        /// <returns>The transaction with the specified ID.</returns>
        TxnpostDomainModels.Transaction ReadTransaction(string transactionId);

        /// <summary>
        /// Writes a transaction to the repository.
        /// </summary>
        /// <param name="transaction">The transaction to write.</param>
        void WriteTransaction(TxnpostDomainModels.Transaction transaction);

        /// <summary>
        /// Updates a transaction in the repository.
        /// </summary>
        /// <param name="transaction">The transaction to update.</param>
        void UpdateTransaction(TxnpostDomainModels.Transaction transaction);

        /// <summary>
        /// Deletes a transaction from the repository.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to delete.</param>
        void DeleteTransaction(string transactionId);
    }

    /// <summary>
    /// Interface for CICS service operations.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Commits the current transaction in the CICS environment.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back the current transaction in the CICS environment.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Sends a message to the CICS message queue.
        /// </summary>
        /// <param name="message">The message to send.</param>
        void SendMessage(string message);

        /// <summary>
        /// Receives a message from the CICS message queue.
        /// </summary>
        /// <returns>The received message.</returns>
        string ReceiveMessage();
    }
}