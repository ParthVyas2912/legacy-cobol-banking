using System.Threading.Tasks;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    public interface IBcheodService
    {
        Task ExecuteAsync();
    }

    public interface IAccountRepository
    {
        Task<BcheodDomainModels.AccountRecord[]> ReadAllAccountsAsync();
        Task WriteAccountAsync(BcheodDomainModels.AccountRecord account);
    }

    public interface ITransactionRepository
    {
        Task<BcheodDomainModels.AccountRecord.TransactionRecord[]> ReadTransactionsAsync(int accountNumber);
        Task WriteTransactionAsync(int accountNumber, BcheodDomainModels.AccountRecord.TransactionRecord transaction);
    }

    public interface ICicsService
    {
        Task<bool> ValidateAccountAsync(int accountNumber);
        Task<bool> UpdateAccountStatusAsync(int accountNumber, BcheodDomainModels.AccountStatusEnum status);
    }
}