using System.Threading.Tasks;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    public interface IIntcalcApplicationService
    {
        Task ExecuteAsync();
    }

    public interface IInterestCalculationRepository
    {
        Task<IntcalcDomainModels.InterestCalculationRecord> ReadNextRecordAsync();
        Task WriteRecordAsync(IntcalcDomainModels.InterestCalculationRecord record);
        Task<bool> HasMoreRecordsAsync();
    }

    public interface ICicsService
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}