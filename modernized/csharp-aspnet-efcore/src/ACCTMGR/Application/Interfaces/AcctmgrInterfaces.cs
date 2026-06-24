using System.Collections.Generic;
using System.Threading.Tasks;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    public interface IAccountManagerService
    {
        Task ExecuteAsync();
    }

    public interface IAccountRepository
    {
        Task<AccountManager?> GetByAccountNumberAsync(string accountNumber);
        Task<List<AccountManager>> GetByAccountTypeAsync(string accountType);
        Task AddAccountAsync(AccountManager account);
        Task UpdateAccountAsync(AccountManager account);
        Task DeleteAccountAsync(string accountNumber);
        Task SaveChangesAsync();
    }

    public interface ITransactionRepository
    {
        Task<List<AccountManager.Transaction>> GetTransactionsByAccountAsync(string accountNumber);
        Task AddTransactionAsync(AccountManager.Transaction transaction);
        Task SaveChangesAsync();
    }
}