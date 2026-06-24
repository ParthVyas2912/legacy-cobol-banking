using System.Collections.Generic;
using System.Threading.Tasks;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    public interface IBcheodService
    {
        Task ExecuteAsync();
    }

    public interface IRootRecordRepository
    {
        Task<BcheodDomainModels.RootRecord?> GetByIdAsync(string recordId);
        Task<IEnumerable<BcheodDomainModels.RootRecord>> GetAllAsync();
        Task AddAsync(BcheodDomainModels.RootRecord rootRecord);
        Task UpdateAsync(BcheodDomainModels.RootRecord rootRecord);
        Task DeleteAsync(string recordId);
    }

    public interface IHeaderRepository
    {
        Task<BcheodDomainModels.Header?> GetByFileDateAsync(string fileDate);
        Task AddAsync(BcheodDomainModels.Header header);
        Task UpdateAsync(BcheodDomainModels.Header header);
    }

    public interface IDetailRepository
    {
        Task<IEnumerable<BcheodDomainModels.Detail>> GetByAccountNumberAsync(string accountNumber);
        Task AddAsync(BcheodDomainModels.Detail detail);
        Task UpdateAsync(BcheodDomainModels.Detail detail);
    }

    public interface ITrailerRepository
    {
        Task<BcheodDomainModels.Trailer?> GetByRecordCountAsync(string recordCount);
        Task AddAsync(BcheodDomainModels.Trailer trailer);
        Task UpdateAsync(BcheodDomainModels.Trailer trailer);
    }
}