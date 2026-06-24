using System.Collections.Generic;
using System.Threading.Tasks;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<AccountManager.Account?> GetAccountByNumberAsync(string accountNumber);
        Task<IEnumerable<AccountManager.Account>> GetAllAccountsAsync();
        Task AddAccountAsync(AccountManager.Account account);
        Task UpdateAccountAsync(AccountManager.Account account);
        Task DeleteAccountAsync(string accountNumber);
    }

    public interface ICustomerRepository
    {
        Task<AccountManager.Customer?> GetCustomerByIdAsync(string customerId);
        Task<IEnumerable<AccountManager.Customer>> GetAllCustomersAsync();
        Task AddCustomerAsync(AccountManager.Customer customer);
        Task UpdateCustomerAsync(AccountManager.Customer customer);
        Task DeleteCustomerAsync(string customerId);
    }

    public interface ITransactionRepository
    {
        Task<AccountManager.Transaction?> GetTransactionByIdAsync(string transactionId);
        Task<IEnumerable<AccountManager.Transaction>> GetTransactionsByAccountAsync(string accountNumber);
        Task AddTransactionAsync(AccountManager.Transaction transaction);
        Task UpdateTransactionAsync(AccountManager.Transaction transaction);
        Task DeleteTransactionAsync(string transactionId);
    }

    public interface IAccountSummaryRepository
    {
        Task<AccountManager.AccountSummary?> GetAccountSummaryAsync(string accountNumber);
        Task<IEnumerable<AccountManager.AccountSummary>> GetAllAccountSummariesAsync();
        Task AddAccountSummaryAsync(AccountManager.AccountSummary accountSummary);
        Task UpdateAccountSummaryAsync(AccountManager.AccountSummary accountSummary);
        Task DeleteAccountSummaryAsync(string accountNumber);
    }

    public interface IAcctmgrService
    {
        Task ExecuteAsync();
    }
}