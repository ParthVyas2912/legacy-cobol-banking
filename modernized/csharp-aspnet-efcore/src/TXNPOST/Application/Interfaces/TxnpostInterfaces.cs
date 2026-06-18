using System.Threading.Tasks;
using TXNPOST.Domain;

namespace TXNPOST.Application.Interfaces
{
    public interface ITxnpostService
    {
        Task ExecuteAsync();
    }

    public interface ITransactionRepository
    {
        Task<TxnpostDomainModels.TransactionRecord> ReadTransactionAsync(string transactionId);
        Task WriteTransactionAsync(TxnpostDomainModels.TransactionRecord transaction);
        Task UpdateTransactionAsync(TxnpostDomainModels.TransactionRecord transaction);
    }

    public interface IAccountRepository
    {
        Task<decimal> GetAccountBalanceAsync(string accountNumber);
        Task UpdateAccountBalanceAsync(string accountNumber, decimal newBalance);
    }

    public interface ICicsService
    {
        Task<string> CallExternalSystemAsync(string requestPayload);
    }
}