using System;
using System.Collections.Generic;
using TXNPOST.Domain;

namespace TXNPOST.Application.Interfaces
{
    // Interface for handling transaction processing logic
    public interface ITransactionProcessor
    {
        void InitializeTransaction(TxnpostDomainModels.Transaction transaction);
        void ValidateTransaction(TxnpostDomainModels.Transaction transaction);
        void CalculateTransactionTotals(TxnpostDomainModels.Transaction transaction);
        void ApplyTransactionRules(TxnpostDomainModels.Transaction transaction);
        void FinalizeTransaction(TxnpostDomainModels.Transaction transaction);
        void LogTransactionError(TxnpostDomainModels.Transaction transaction, string errorMessage); // Added based on COBOL error handling
    }

    // Interface for file I/O operations related to transactions
    public interface ITransactionRepository
    {
        TxnpostDomainModels.Transaction ReadTransaction(string transactionId);
        void WriteTransaction(TxnpostDomainModels.Transaction transaction);
        IEnumerable<TxnpostDomainModels.Transaction> ReadAllTransactions();
        void UpdateTransaction(TxnpostDomainModels.Transaction transaction);
        void DeleteTransaction(string transactionId);
        bool TransactionExists(string transactionId); // Added to check if a transaction exists
    }

    // Interface for CICS service operations
    public interface ICicsService
    {
        void CommitTransaction(string transactionId);
        void RollbackTransaction(string transactionId);
        TxnpostDomainModels.AccountDetails FetchAccountDetails(string accountNumber);
        void UpdateAccountBalance(string accountNumber, decimal newBalance);
        void SendNotification(string accountNumber, string message); // Added for CICS notification service
    }
}