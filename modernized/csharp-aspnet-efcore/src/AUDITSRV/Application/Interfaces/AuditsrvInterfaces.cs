using System.Threading.Tasks;
using AUDITSRV.Domain;

namespace AUDITSRV.Application.Interfaces
{
    public interface IAuditsrvApplicationService
    {
        Task ExecuteAsync();
    }

    public interface IAuditRepository
    {
        Task<AuditsrvDomainModels.AuditRecord> ReadNextRecordAsync();
        Task WriteRecordAsync(AuditsrvDomainModels.AuditRecord record);
        Task CommitAsync();
    }

    public interface ICicsService
    {
        Task<bool> SendAuditNotificationAsync(AuditsrvDomainModels.AuditRecord record);
    }
}