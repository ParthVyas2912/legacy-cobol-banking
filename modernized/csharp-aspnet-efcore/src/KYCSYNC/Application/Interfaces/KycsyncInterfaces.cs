using System.Collections.Generic;
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
        Task<IEnumerable<KycsyncDomainModels.CustomerRecord>> ReadAllCustomersAsync();
        Task WriteCustomerAsync(KycsyncDomainModels.CustomerRecord customerRecord);
    }

    public interface IKycStatusService
    {
        Task<KycsyncDomainModels.KycStatus.KycStatusEnum> GetKycStatusAsync(string customerId);
        Task UpdateKycStatusAsync(string customerId, KycsyncDomainModels.KycStatus.KycStatusEnum status);
    }
}