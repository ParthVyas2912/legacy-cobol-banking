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
        Task<StmtrptDomainModels.AccountSummary[]> GetAllAccountsAsync();
        Task<StmtrptDomainModels.Transaction[]> GetTransactionsByAccountAsync(string accountNumber);
    }

    public interface IReportRepository
    {
        Task SaveReportHeaderAsync(StmtrptDomainModels.ReportHeader header);
        Task SaveAccountSummaryAsync(StmtrptDomainModels.AccountSummary accountSummary);
        Task SaveReportFooterAsync(StmtrptDomainModels.ReportFooter footer);
        Task<StmtrptDomainModels.StatementReport> GetStatementReportAsync();
    }
}