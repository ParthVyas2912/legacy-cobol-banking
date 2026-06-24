using System.Collections.Generic;
using System.Threading.Tasks;
using AUDITSRV.Domain;

namespace AUDITSRV.Application.Interfaces
{
    public interface IAuditsrvApplicationService
    {
        Task ExecuteAsync();
    }

    public interface IAuditRecordRepository
    {
        Task<AuditsrvDomainModels.AuditRecord?> GetByIdAsync(string recordId);
        Task<List<AuditsrvDomainModels.AuditRecord>> GetAllAsync();
        Task AddAsync(AuditsrvDomainModels.AuditRecord record);
        Task UpdateAsync(AuditsrvDomainModels.AuditRecord record);
        Task DeleteAsync(string recordId);
        Task SaveChangesAsync();
    }

    public interface IAuditDetailRepository
    {
        Task<List<AuditsrvDomainModels.AuditDetail>> GetByRecordIdAsync(string recordId);
        Task AddAsync(AuditsrvDomainModels.AuditDetail detail);
        Task UpdateAsync(AuditsrvDomainModels.AuditDetail detail);
        Task DeleteAsync(string detailCode);
        Task SaveChangesAsync();
    }

    public interface ICicsService
    {
        Task LinkAsync(string programName, object requestData, object responseData);
    }
}