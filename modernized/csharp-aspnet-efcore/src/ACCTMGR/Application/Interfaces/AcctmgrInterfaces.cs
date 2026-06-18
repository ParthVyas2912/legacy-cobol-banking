using System.Threading.Tasks;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    public interface IAcctmgrService
    {
        Task ExecuteAsync();
    }

    public interface ICustomerRepository
    {
        Task<AccountManager.CustomerRecord[]> ReadCustomerRecordsAsync();
        Task WriteCustomerRecordsAsync(AccountManager.CustomerRecord[] customerRecords);
    }

    public interface IAccountRepository
    {
        Task<AccountManager.AccountRecord[]> ReadAccountRecordsAsync(string customerId);
        Task WriteAccountRecordsAsync(string customerId, AccountManager.AccountRecord[] accountRecords);
    }

    public interface IReportService
    {
        Task GenerateCustomerReportAsync(AccountManager.CustomerRecord[] customerRecords);
        Task GenerateAccountSummaryReportAsync(AccountManager.AccountRecord[] accountRecords);
    }
}