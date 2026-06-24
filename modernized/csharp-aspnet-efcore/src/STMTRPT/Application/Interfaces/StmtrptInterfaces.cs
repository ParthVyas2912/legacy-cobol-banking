using System.Collections.Generic;
using System.Threading.Tasks;
using STMTRPT.Domain;

namespace STMTRPT.Application.Interfaces
{
    /// <summary>
    /// Interface for the STMTRPT application service.
    /// The main entry point for executing the STMTRPT program.
    /// </summary>
    public interface IStmtrptService
    {
        /// <summary>
        /// Main entry point for executing the STMTRPT program.
        /// </summary>
        Task ExecuteAsync();
    }

    /// <summary>
    /// Repository interface for managing StatementReport entities.
    /// </summary>
    public interface IStatementReportRepository
    {
        /// <summary>
        /// Retrieves a statement report by account number.
        /// </summary>
        /// <param name="accountNumber">The account number to search for.</param>
        /// <returns>The statement report associated with the account number.</returns>
        Task<StmtrptDomainModels.StatementReport?> GetByAccountNumberAsync(string accountNumber);

        /// <summary>
        /// Adds a new statement report to the repository.
        /// </summary>
        /// <param name="report">The statement report to add.</param>
        Task AddAsync(StmtrptDomainModels.StatementReport report);

        /// <summary>
        /// Updates an existing statement report in the repository.
        /// </summary>
        /// <param name="report">The statement report to update.</param>
        Task UpdateAsync(StmtrptDomainModels.StatementReport report);

        /// <summary>
        /// Retrieves all statement reports.
        /// </summary>
        /// <returns>A list of all statement reports.</returns>
        Task<List<StmtrptDomainModels.StatementReport>> GetAllAsync();
    }
}