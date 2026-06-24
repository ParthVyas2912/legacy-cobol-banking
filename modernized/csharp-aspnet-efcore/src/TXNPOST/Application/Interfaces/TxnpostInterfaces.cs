using System.Collections.Generic;
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
        Task<TxnpostDomainModels.TransactionRecord?> GetTransactionByIdAsync(string transactionId);
        Task<List<TxnpostDomainModels.TransactionRecord>> GetAllTransactionsAsync();
        Task AddTransactionAsync(TxnpostDomainModels.TransactionRecord transaction);
        Task UpdateTransactionAsync(TxnpostDomainModels.TransactionRecord transaction);
        Task DeleteTransactionAsync(string transactionId);
    }

    public interface ITransactionDetailRepository
    {
        Task<List<TxnpostDomainModels.TransactionDetail>> GetTransactionDetailsByTransactionIdAsync(string transactionId);
        Task AddTransactionDetailAsync(TxnpostDomainModels.TransactionDetail transactionDetail);
        Task UpdateTransactionDetailAsync(TxnpostDomainModels.TransactionDetail transactionDetail);
        Task DeleteTransactionDetailAsync(string detailId);
    }
}