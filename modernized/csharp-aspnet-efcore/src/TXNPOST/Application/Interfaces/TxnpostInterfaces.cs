using System;
using System.Collections.Generic;
using TXNPOST.Domain;

namespace TXNPOST.Application.Interfaces
{
    // Interface for file I/O operations related to transaction records
    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads a transaction record by its ID.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction.</param>
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
        /// <param name="transactionId">The unique identifier of the transaction to delete.</param>
        void DeleteTransaction(string transactionId);

        /// <summary>
        /// Reads all transaction records from the repository.
        /// </summary>
        /// <returns>A list of all transaction records.</returns>
        List<TxnpostDomainModels.TransactionRecord> ReadAllTransactions();
    }

    // Interface for CICS operations related to transaction processing
    public interface ITransactionService
    {
        /// <summary>
        /// Posts a transaction to the system.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to post.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        TxnpostDomainModels.StatusEnum PostTransaction(TxnpostDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Retrieves the status of a transaction by its ID.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction.</param>
        /// <returns>The status of the transaction.</returns>
        TxnpostDomainModels.StatusEnum GetTransactionStatus(string transactionId);

        /// <summary>
        /// Cancels a transaction by its ID.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction to cancel.</param>
        /// <returns>A status indicating the result of the cancellation.</returns>
        TxnpostDomainModels.StatusEnum CancelTransaction(string transactionId);

        /// <summary>
        /// Validates a transaction record before posting.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to validate.</param>
        /// <returns>A status indicating whether the transaction is valid.</returns>
        TxnpostDomainModels.StatusEnum ValidateTransaction(TxnpostDomainModels.TransactionRecord transactionRecord);
    }
}