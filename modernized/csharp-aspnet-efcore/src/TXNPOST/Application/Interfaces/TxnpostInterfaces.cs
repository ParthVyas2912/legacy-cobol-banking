using System;
using TXNPOST.Domain;

namespace TXNPOST.Application.Interfaces
{
    // Interface for file I/O operations related to transactions
    public interface ITransactionRepository
    {
        TxnpostDomainModels.Transaction GetTransactionById(string transactionId);
        void SaveTransaction(TxnpostDomainModels.Transaction transaction);
        void UpdateTransactionStatus(string transactionId, TxnpostDomainModels.Transaction.StatusEnum status);
        void DeleteTransaction(string transactionId); // Added based on COBOL DELETE operations
    }

    // Interface for file I/O operations related to batches
    public interface IBatchRepository
    {
        TxnpostDomainModels.Batch GetBatchById(string batchId);
        void SaveBatch(TxnpostDomainModels.Batch batch);
        void UpdateBatchTotals(string batchId, decimal totalAmount, int recordCount);
        void DeleteBatch(string batchId); // Added based on COBOL DELETE operations
        TxnpostDomainModels.Batch[] GetBatchesByDateRange(DateTime startDate, DateTime endDate); // Added for batch range queries
    }

    // Interface for CICS service operations
    public interface ICicsService
    {
        void CommitTransaction(string transactionId);
        void RollbackTransaction(string transactionId);
        void NotifyExternalSystem(string transactionId, TxnpostDomainModels.Transaction.StatusEnum status);
        string GenerateUniqueTransactionId(); // Added for CICS-generated unique IDs
    }

    // Interface for the main TXNPOST application logic
    public interface ITxnpostApplicationService
    {
        void ProcessTransaction(TxnpostDomainModels.Transaction transaction);
        void ProcessBatch(TxnpostDomainModels.Batch batch);
        void FinalizeBatch(string batchId);
        void ReprocessFailedTransactions(string batchId); // Added for reprocessing logic
    }
}