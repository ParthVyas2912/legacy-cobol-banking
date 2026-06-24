using System.Collections.Generic;
using System.Threading.Tasks;
using ATMTXN.Domain;

namespace ATMTXN.Application.Interfaces
{
    public interface IAtmtxnService
    {
        Task ExecuteAsync();
    }

    public interface ITransactionRepository
    {
        Task<AtmtxnDomainModels.TransactionRecord?> GetTransactionByIdAsync(string transactionId);
        Task<IEnumerable<AtmtxnDomainModels.TransactionRecord>> GetAllTransactionsAsync();
        Task AddTransactionAsync(AtmtxnDomainModels.TransactionRecord transaction);
        Task UpdateTransactionAsync(AtmtxnDomainModels.TransactionRecord transaction);
        Task DeleteTransactionAsync(string transactionId);
    }

    public interface ITransactionDetailRepository
    {
        Task<IEnumerable<AtmtxnDomainModels.TransactionDetail>> GetTransactionDetailsByTransactionIdAsync(string transactionId);
        Task AddTransactionDetailAsync(AtmtxnDomainModels.TransactionDetail detail);
        Task UpdateTransactionDetailAsync(AtmtxnDomainModels.TransactionDetail detail);
        Task DeleteTransactionDetailAsync(string transactionId, string itemCode);
    }
}