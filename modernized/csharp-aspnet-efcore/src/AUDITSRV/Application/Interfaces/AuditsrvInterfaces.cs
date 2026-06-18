using System.Collections.Generic;
using System.Threading.Tasks;
using AUDITSRV.Domain;

namespace AUDITSRV.Application.Interfaces
{
    public interface IAuditRepository
    {
        Task<IEnumerable<AuditsrvDomainModels.AuditRecord>> ReadAllAuditRecordsAsync();
        Task WriteAuditRecordAsync(AuditsrvDomainModels.AuditRecord auditRecord);
        Task<AuditsrvDomainModels.AuditSummary> ReadAuditSummaryAsync();
        Task WriteAuditSummaryAsync(AuditsrvDomainModels.AuditSummary auditSummary);
    }

    public interface ICicsService
    {
        Task PerformCicsOperationAsync(string operationCode, object requestData);
    }

    public interface IAuditsrvService
    {
        Task ExecuteAsync();
    }
}