using System;
using System.Collections.Generic;
using ATMTXN.Domain;

namespace ATMTXN.Application.Interfaces
{
    /// <summary>
    /// Interface for handling ATM transaction operations.
    /// </summary>
    public interface IAtmtxnService
    {
        /// <summary>
        /// Initializes the transaction processing environment.
        /// </summary>
        void InitializeTransactionEnvironment();

        /// <summary>
        /// Validates the transaction details.
        /// </summary>
        /// <param name="transaction">The transaction to validate.</param>
        /// <returns>True if the transaction is valid, otherwise false.</returns>
        bool ValidateTransaction(AtmtxnDomainModels.Transaction transaction);

        /// <summary>
        /// Processes the transaction.
        /// </summary>
        /// <param name="transaction">The transaction to process.</param>
        /// <returns>True if the transaction was processed successfully, otherwise false.</returns>
        bool ProcessTransaction(AtmtxnDomainModels.Transaction transaction);

        /// <summary>
        /// Finalizes the transaction processing environment.
        /// </summary>
        void FinalizeTransactionEnvironment();
    }

    /// <summary>
    /// Interface for repository operations related to ATM transactions.
    /// </summary>
    public interface IAtmtxnRepository
    {
        /// <summary>
        /// Reads transaction details from the data store.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to read.</param>
        /// <returns>The transaction details.</returns>
        AtmtxnDomainModels.Transaction ReadTransaction(string transactionId);

        /// <summary>
        /// Writes transaction details to the data store.
        /// </summary>
        /// <param name="transaction">The transaction to write.</param>
        void WriteTransaction(AtmtxnDomainModels.Transaction transaction);

        /// <summary>
        /// Updates the account balance in the data store.
        /// </summary>
        /// <param name="accountNumber">The account number to update.</param>
        /// <param name="newBalance">The new balance to set.</param>
        void UpdateAccountBalance(string accountNumber, decimal newBalance);

        /// <summary>
        /// Retrieves account details from the data store.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve.</param>
        /// <returns>The account details.</returns>
        AtmtxnDomainModels.Transaction.AccountDetails GetAccountDetails(string accountNumber);
    }

    /// <summary>
    /// Interface for CICS operations related to ATM transactions.
    /// </summary>
    public interface IAtmtxnCicsService
    {
        /// <summary>
        /// Sends a transaction request to the CICS system.
        /// </summary>
        /// <param name="transaction">The transaction to send.</param>
        /// <returns>True if the request was successful, otherwise false.</returns>
        bool SendTransactionRequest(AtmtxnDomainModels.Transaction transaction);

        /// <summary>
        /// Receives a response from the CICS system.
        /// </summary>
        /// <returns>The response from the CICS system.</returns>
        string ReceiveCicsResponse();
    }
}