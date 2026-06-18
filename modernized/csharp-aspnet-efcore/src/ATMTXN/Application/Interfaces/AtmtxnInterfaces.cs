using System.Threading.Tasks;
using ATMTXN.Domain;

namespace ATMTXN.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<AtmtxnDomainModels.Transaction> GetTransactionByIdAsync(string transactionId);
        Task SaveTransactionAsync(AtmtxnDomainModels.Transaction transaction);
        Task UpdateTransactionStatusAsync(string transactionId, AtmtxnDomainModels.TransactionStatusEnum status);
    }

    public interface ICicsService
    {
        Task<AtmtxnDomainModels.TransactionStatusEnum> ProcessTransactionAsync(AtmtxnDomainModels.Transaction transaction);
    }

    public interface IAtmtxnService
    {
        Task ExecuteAsync();
    }
}