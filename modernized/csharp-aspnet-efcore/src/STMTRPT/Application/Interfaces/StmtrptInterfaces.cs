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
        Task<IEnumerable<StmtrptDomainModels.AccountRecord>> GetAllAccountsAsync();
        Task UpdateAccountAsync(StmtrptDomainModels.AccountRecord account);
    }

    public interface ITransactionRepository
    {
        Task<IEnumerable<StmtrptDomainModels.TransactionRecord>> GetTransactionsByAccountAsync(string accountNumber);
    }

    public interface IReportService
    {
        Task GenerateAccountReportAsync(StmtrptDomainModels.AccountRecord account);
    }
}