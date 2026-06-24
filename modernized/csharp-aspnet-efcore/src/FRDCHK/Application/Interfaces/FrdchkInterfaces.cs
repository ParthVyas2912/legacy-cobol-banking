using System.Collections.Generic;
using System.Threading.Tasks;
using FRDCHK.Domain.Models;

namespace FRDCHK.Application.Interfaces
{
    public interface IFraudCheckService
    {
        Task ExecuteAsync();
    }

    public interface IFraudCheckRepository
    {
        Task<FraudCheckRecord?> GetFraudCheckRecordByIdAsync(string recordId);
        Task<List<FraudCheckRecord>> GetAllFraudCheckRecordsAsync();
        Task AddFraudCheckRecordAsync(FraudCheckRecord record);
        Task UpdateFraudCheckRecordAsync(FraudCheckRecord record);
        Task DeleteFraudCheckRecordAsync(string recordId);
    }
}