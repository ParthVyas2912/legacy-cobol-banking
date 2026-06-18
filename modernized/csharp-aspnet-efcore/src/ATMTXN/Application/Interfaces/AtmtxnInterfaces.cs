using System.Collections.Generic;
using System.Threading.Tasks;
using ATMTXN.Domain;

namespace ATMTXN.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<AtmtxnDomainModels.TransactionRecord>> ReadAllTransactionsAsync();
        Task WriteTransactionAsync(AtmtxnDomainModels.TransactionRecord transactionRecord);
    }

    public interface ICicsService
    {
        Task<AtmtxnDomainModels.TransactionStatus> ProcessTransactionAsync(AtmtxnDomainModels.TransactionRecord transactionRecord);
    }

    public interface IAtmtxnService
    {
        Task ExecuteAsync();
    }
}