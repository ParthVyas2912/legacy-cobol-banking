using System;
using TXNPOST.Domain;

namespace TXNPOST.Application.Interfaces
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads a transaction record by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to retrieve.</param>
        /// <returns>The transaction record if found, otherwise null.</returns>
        TxnpostDomainModels.TransactionRecord ReadTransactionById(string transactionId);

        /// <summary>
        /// Writes a transaction record to the data store.
        /// </summary>
        /// <param name="transaction">The transaction record to write.</param>
        void WriteTransaction(TxnpostDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Updates an existing transaction record in the data store.
        /// </summary>
        /// <param name="transaction">The transaction record to update.</param>
        void UpdateTransaction(TxnpostDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Deletes a transaction record by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to delete.</param>
        void DeleteTransaction(string transactionId);
    }

    public interface IBatchRepository
    {
        /// <summary>
        /// Reads a batch record by its ID.
        /// </summary>
        /// <param name="batchId">The ID of the batch to retrieve.</param>
        /// <returns>The batch record if found, otherwise null.</returns>
        TxnpostDomainModels.BatchRecord ReadBatchById(string batchId);

        /// <summary>
        /// Writes a batch record to the data store.
        /// </summary>
        /// <param name="batch">The batch record to write.</param>
        void WriteBatch(TxnpostDomainModels.BatchRecord batch);

        /// <summary>
        /// Updates an existing batch record in the data store.
        /// </summary>
        /// <param name="batch">The batch record to update.</param>
        void UpdateBatch(TxnpostDomainModels.BatchRecord batch);

        /// <summary>
        /// Deletes a batch record by its ID.
        /// </summary>
        /// <param name="batchId">The ID of the batch to delete.</param>
        void DeleteBatch(string batchId);
    }

    public interface ICicsService
    {
        /// <summary>
        /// Sends a transaction to the CICS system for processing.
        /// </summary>
        /// <param name="transaction">The transaction record to process.</param>
        /// <returns>The status of the transaction after processing.</returns>
        TxnpostDomainModels.TransactionRecord.TransactionStatusEnum ProcessTransaction(TxnpostDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Retrieves the status of a transaction from the CICS system.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to query.</param>
        /// <returns>The status of the transaction.</returns>
        TxnpostDomainModels.TransactionRecord.TransactionStatusEnum GetTransactionStatus(string transactionId);

        /// <summary>
        /// Sends a batch of transactions to the CICS system for processing.
        /// </summary>
        /// <param name="batch">The batch record containing transactions to process.</param>
        /// <returns>The updated batch record after processing.</returns>
        TxnpostDomainModels.BatchRecord ProcessBatch(TxnpostDomainModels.BatchRecord batch);
    }
}