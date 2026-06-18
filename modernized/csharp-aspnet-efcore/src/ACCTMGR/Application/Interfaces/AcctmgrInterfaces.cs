using System.Threading.Tasks;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    public interface IAcctmgrService
    {
        /// <summary>
        /// Main entry point for the ACCTMGR program.
        /// </summary>
        Task ExecuteAsync();
    }

    public interface ICustomerRepository
    {
        /// <summary>
        /// Reads a customer record by customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID to search for.</param>
        /// <returns>The customer record if found, otherwise null.</returns>
        Task<AccountManager.CustomerRecord?> ReadCustomerAsync(string customerId);

        /// <summary>
        /// Writes a customer record.
        /// </summary>
        /// <param name="customer">The customer record to write.</param>
        Task WriteCustomerAsync(AccountManager.CustomerRecord customer);
    }

    public interface IAccountRepository
    {
        /// <summary>
        /// Reads account records for a given customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID to search for.</param>
        /// <returns>An array of account records.</returns>
        Task<AccountManager.CustomerRecord.AccountRecord[]> ReadAccountsAsync(string customerId);

        /// <summary>
        /// Writes account records for a given customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID associated with the accounts.</param>
        /// <param name="accounts">The account records to write.</param>
        Task WriteAccountsAsync(string customerId, AccountManager.CustomerRecord.AccountRecord[] accounts);
    }

    public interface ICicsService
    {
        /// <summary>
        /// Performs a CICS transaction for account updates.
        /// </summary>
        /// <param name="transactionData">The data required for the transaction.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task PerformTransactionAsync(object transactionData);
    }
}