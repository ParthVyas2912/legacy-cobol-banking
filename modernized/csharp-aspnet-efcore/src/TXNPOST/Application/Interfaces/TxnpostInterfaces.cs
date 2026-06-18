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
        Task WriteTransactionAsync(TxnpostDomainModels.TransactionRecord transactionRecord);
        Task UpdateTransactionStatusAsync(string transactionId, TxnpostDomainModels.StatusEnum status);
    }

    public interface ICicsService
    {
        Task<bool> ValidateAccountAsync(string accountNumber);
        Task<bool> PostToLedgerAsync(TxnpostDomainModels.TransactionRecord transactionRecord);
        Task<bool> NotifyExternalSystemAsync(TxnpostDomainModels.TransactionRecord transactionRecord);
    }
}