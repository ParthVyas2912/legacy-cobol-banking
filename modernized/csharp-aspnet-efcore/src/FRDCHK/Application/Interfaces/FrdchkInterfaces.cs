using System.Collections.Generic;
using System.Threading.Tasks;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    public interface IFrdchkService
    {
        Task ExecuteAsync();
    }

    public interface ICustomerRepository
    {
        Task<IEnumerable<FrdchkDomainModels.CustomerRecord>> GetAllCustomersAsync();
        Task<FrdchkDomainModels.CustomerRecord> GetCustomerByIdAsync(string customerId);
        Task AddCustomerAsync(FrdchkDomainModels.CustomerRecord customerRecord);
        Task UpdateCustomerAsync(FrdchkDomainModels.CustomerRecord customerRecord);
        Task DeleteCustomerAsync(string customerId);
    }

    public interface IAccountRepository
    {
        Task<IEnumerable<FrdchkDomainModels.Account>> GetAccountsByCustomerIdAsync(string customerId);
        Task AddAccountAsync(string customerId, FrdchkDomainModels.Account account);
        Task UpdateAccountAsync(string customerId, FrdchkDomainModels.Account account);
        Task DeleteAccountAsync(string customerId, string accountNumber);
    }

    public interface ICicsService
    {
        Task<string> CallCicsTransactionAsync(string transactionCode, Dictionary<string, string> parameters);
    }
}