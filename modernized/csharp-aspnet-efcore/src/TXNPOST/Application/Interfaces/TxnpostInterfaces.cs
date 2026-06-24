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
        Task<TxnpostDomainModels.Transaction?> GetTransactionByIdAsync(string transactionId);
        Task<List<TxnpostDomainModels.Transaction>> GetAllTransactionsAsync();
        Task AddTransactionAsync(TxnpostDomainModels.Transaction transaction);
        Task UpdateTransactionAsync(TxnpostDomainModels.Transaction transaction);
        Task DeleteTransactionAsync(string transactionId);
    }

    public interface ITransactionDetailRepository
    {
        Task<TxnpostDomainModels.TransactionDetail?> GetTransactionDetailByIdAsync(string detailId);
        Task<List<TxnpostDomainModels.TransactionDetail>> GetTransactionDetailsByTransactionIdAsync(string transactionId);
        Task AddTransactionDetailAsync(TxnpostDomainModels.TransactionDetail transactionDetail);
        Task UpdateTransactionDetailAsync(TxnpostDomainModels.TransactionDetail transactionDetail);
        Task DeleteTransactionDetailAsync(string detailId);
    }

    public interface ICicsService
    {
        Task LinkAsync(string programName, object requestData, object responseData);
    }
}