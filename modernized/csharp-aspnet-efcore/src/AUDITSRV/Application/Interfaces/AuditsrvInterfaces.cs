using System.Collections.Generic;
using System.Threading.Tasks;
using AUDITSRV.Domain;

namespace AUDITSRV.Application.Interfaces
{
    public interface IAuditService
    {
        Task ExecuteAsync();
    }

    public interface IAuditRepository
    {
        Task<IEnumerable<AuditsrvDomainModels.AuditRecord>> ReadAuditRecordsAsync();
        Task WriteAuditRecordAsync(AuditsrvDomainModels.AuditRecord auditRecord);
        Task UpdateAuditRecordAsync(AuditsrvDomainModels.AuditRecord auditRecord);
        Task DeleteAuditRecordAsync(string auditId);
    }

    public interface ICicsService
    {
        Task CallCicsTransactionAsync(string transactionId, object requestPayload);
    }
}