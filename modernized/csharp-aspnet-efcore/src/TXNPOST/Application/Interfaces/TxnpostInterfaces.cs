using System;
using TXNPOST.Domain;

namespace TXNPOST.Application.Interfaces
{
    public interface ITxnpostRepository
    {
        /// <summary>
        /// Reads a transaction record from the file.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to read.</param>
        /// <returns>The transaction record.</returns>
        TxnpostDomainModels.TransactionRecord ReadTransaction(string transactionId);

        /// <summary>
        /// Writes a transaction record to the file.
        /// </summary>
        /// <param name="transaction">The transaction record to write.</param>
        void WriteTransaction(TxnpostDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Reads a batch record from the file.
        /// </summary>
        /// <param name="batchId">The ID of the batch to read.</param>
        /// <returns>The batch record.</returns>
        TxnpostDomainModels.BatchRecord ReadBatch(string batchId);

        /// <summary>
        /// Writes a batch record to the file.
        /// </summary>
        /// <param name="batch">The batch record to write.</param>
        void WriteBatch(TxnpostDomainModels.BatchRecord batch);
    }

    public interface ITxnpostService
    {
        /// <summary>
        /// Posts a transaction to the system.
        /// </summary>
        /// <param name="transaction">The transaction record to post.</param>
        void PostTransaction(TxnpostDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Validates a transaction before posting.
        /// </summary>
        /// <param name="transaction">The transaction record to validate.</param>
        /// <returns>True if the transaction is valid, otherwise false.</returns>
        bool ValidateTransaction(TxnpostDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Processes a batch of transactions.
        /// </summary>
        /// <param name="batch">The batch record containing transactions to process.</param>
        void ProcessBatch(TxnpostDomainModels.BatchRecord batch);

        /// <summary>
        /// Retrieves the account balance for a given account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve the balance for.</param>
        /// <returns>The account balance.</returns>
        decimal GetAccountBalance(string accountNumber);

        /// <summary>
        /// Updates the account balance for a given account number.
        /// </summary>
        /// <param name="accountNumber">The account number to update the balance for.</param>
        /// <param name="newBalance">The new balance to set.</param>
        void UpdateAccountBalance(string accountNumber, decimal newBalance);
    }
}