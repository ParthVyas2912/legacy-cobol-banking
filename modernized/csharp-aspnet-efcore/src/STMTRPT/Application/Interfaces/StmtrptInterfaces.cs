using System.Collections.Generic;
using System.Threading.Tasks;
using STMTRPT.Domain;

namespace STMTRPT.Application.Interfaces
{
    public interface IStmtrptService
    {
        Task ExecuteAsync();
    }

    public interface IAccountRepository
    {
        Task<List<StmtrptDomainModels.StatementReport>> GetAccountRecordsAsync();
        Task SaveStatementReportAsync(StmtrptDomainModels.StatementReport report);
    }

    public interface ITransactionRepository
    {
        Task<List<StmtrptDomainModels.StatementReport.Transaction>> GetTransactionsByAccountAsync(int accountNumber);
    }

    public interface ICicsService
    {
        Task<string> GetAccountStatusAsync(int accountNumber);
        Task UpdateAccountStatusAsync(int accountNumber, StmtrptDomainModels.StatementReport.AccountStatusEnum status);
    }
}