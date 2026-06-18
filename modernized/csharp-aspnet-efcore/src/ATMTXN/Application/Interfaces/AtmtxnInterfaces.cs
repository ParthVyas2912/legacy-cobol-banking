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
        Task<AtmtxnDomainModels.Transaction> GetTransactionByIdAsync(string transactionId);
        Task SaveTransactionAsync(AtmtxnDomainModels.Transaction transaction);
        Task UpdateTransactionAsync(AtmtxnDomainModels.Transaction transaction);
    }

    public interface IAccountService
    {
        Task<decimal> GetAccountBalanceAsync(string accountNumber);
        Task UpdateAccountBalanceAsync(string accountNumber, decimal newBalance);
    }

    public interface ICicsService
    {
        Task<bool> PerformCicsOperationAsync(AtmtxnDomainModels.Transaction transaction);
    }
}