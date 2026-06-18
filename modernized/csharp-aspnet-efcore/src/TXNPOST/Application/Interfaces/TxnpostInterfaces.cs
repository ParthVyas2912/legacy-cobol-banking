using System.Collections.Generic;
using System.Threading.Tasks;
using TXNPOST.Domain;

namespace TXNPOST.Application.Interfaces
{
    /// <summary>
    /// Interface for the main TXNPOST application service.
    /// The public entry point for the application.
    /// </summary>
    public interface ITxnpostService
    {
        Task ExecuteAsync();
    }

    /// <summary>
    /// Interface for repository operations related to transaction records.
    /// </summary>
    public interface ITransactionRepository
    {
        Task<TxnpostDomainModels.TransactionRecord> GetTransactionByIdAsync(string transactionId);
        Task<IEnumerable<TxnpostDomainModels.TransactionRecord>> GetPendingTransactionsAsync();
        Task SaveTransactionAsync(TxnpostDomainModels.TransactionRecord transactionRecord);
    }

    /// <summary>
    /// Interface for repository operations related to account data.
    /// </summary>
    public interface IAccountRepository
    {
        Task<decimal> GetAccountBalanceAsync(string accountNumber);
        Task UpdateAccountBalanceAsync(string accountNumber, decimal newBalance);
    }

    /// <summary>
    /// Interface for logging operations.
    /// </summary>
    public interface ITransactionLogService
    {
        Task LogTransactionAsync(TxnpostDomainModels.TransactionRecord transactionRecord);
    }

    /// <summary>
    /// Interface for external CICS operations.
    /// </summary>
    public interface ICicsService
    {
        Task<bool> PostTransactionToCicsAsync(TxnpostDomainModels.TransactionRecord transactionRecord);
    }
}