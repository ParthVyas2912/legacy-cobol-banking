using System.Collections.Generic;
using System.Threading.Tasks;
using KYCSYNC.Domain;

namespace KYCSYNC.Application.Interfaces
{
    /// <summary>
    /// Interface for the main KYCSYNC application service.
    /// This interface exposes only the public entry point for the application.
    /// </summary>
    public interface IKycsyncService
    {
        Task ExecuteAsync();
    }

    /// <summary>
    /// Interface for repository operations related to customer records.
    /// </summary>
    public interface ICustomerRepository
    {
        Task<IEnumerable<KycsyncDomainModels.CustomerRecord>> GetAllCustomersAsync();
        Task<KycsyncDomainModels.CustomerRecord> GetCustomerByIdAsync(string customerId);
        Task UpdateCustomerAsync(KycsyncDomainModels.CustomerRecord customerRecord);
        Task AddCustomerAsync(KycsyncDomainModels.CustomerRecord customerRecord);
        Task DeleteCustomerAsync(string customerId);
    }

    /// <summary>
    /// Interface for repository operations related to account records.
    /// </summary>
    public interface IAccountRepository
    {
        Task<IEnumerable<KycsyncDomainModels.Account>> GetAccountsByCustomerIdAsync(string customerId);
        Task UpdateAccountAsync(KycsyncDomainModels.Account account);
        Task AddAccountAsync(KycsyncDomainModels.Account account);
        Task DeleteAccountAsync(string accountNumber);
    }

    /// <summary>
    /// Interface for CICS operations related to KYCSYNC.
    /// </summary>
    public interface IKycsyncCicsService
    {
        Task<bool> ValidateCustomerAsync(string customerId);
        Task<bool> UpdateCustomerStatusAsync(string customerId, KycsyncDomainModels.AccountStatus status);
    }
}