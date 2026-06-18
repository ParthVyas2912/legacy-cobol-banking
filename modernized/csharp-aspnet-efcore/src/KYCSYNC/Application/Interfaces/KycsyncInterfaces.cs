using System.Threading.Tasks;
using KYCSYNC.Domain;

namespace KYCSYNC.Application.Interfaces
{
    public interface IKycsyncService
    {
        Task ExecuteAsync();
    }

    public interface ICustomerRepository
    {
        Task<KycsyncDomainModels.CustomerRecord[]> ReadAllCustomersAsync();
        Task WriteCustomerRecordAsync(KycsyncDomainModels.CustomerRecord customerRecord);
    }

    public interface ITransactionRepository
    {
        Task<KycsyncDomainModels.CustomerRecord.Transaction[]> ReadTransactionsByCustomerIdAsync(string customerId);
        Task WriteTransactionAsync(string customerId, KycsyncDomainModels.CustomerRecord.Transaction transaction);
    }

    public interface ICicsService
    {
        Task<bool> UpdateCustomerStatusAsync(string customerId, KycsyncDomainModels.CustomerRecord.AccountStatusEnum status);
        Task<bool> LogTransactionAsync(string transactionId, KycsyncDomainModels.CustomerRecord.TransactionTypeEnum transactionType, decimal amount);
    }
}