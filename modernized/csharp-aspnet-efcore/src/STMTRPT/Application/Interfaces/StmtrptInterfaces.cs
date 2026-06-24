using System.Collections.Generic;
using System.Threading.Tasks;
using STMTRPT.Domain;

namespace STMTRPT.Application.Interfaces
{
    public interface IStatementReportRepository
    {
        /// <summary>
        /// Retrieves a statement report by account number.
        /// </summary>
        /// <param name="accountNumber">The account number to search for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the statement report.</returns>
        Task<StmtrptDomainModels.StatementReport?> GetByAccountNumberAsync(string accountNumber);

        /// <summary>
        /// Adds a new statement report to the repository.
        /// </summary>
        /// <param name="statementReport">The statement report to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(StmtrptDomainModels.StatementReport statementReport);

        /// <summary>
        /// Updates an existing statement report in the repository.
        /// </summary>
        /// <param name="statementReport">The statement report to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(StmtrptDomainModels.StatementReport statementReport);

        /// <summary>
        /// Deletes a statement report by account number.
        /// </summary>
        /// <param name="accountNumber">The account number of the statement report to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteByAccountNumberAsync(string accountNumber);
    }

    public interface ITransactionRepository
    {
        /// <summary>
        /// Retrieves all transactions for a specific account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of transactions.</returns>
        Task<List<StmtrptDomainModels.StatementReport.Transaction>> GetTransactionsByAccountNumberAsync(string accountNumber);

        /// <summary>
        /// Adds a new transaction to the repository.
        /// </summary>
        /// <param name="transaction">The transaction to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddTransactionAsync(StmtrptDomainModels.StatementReport.Transaction transaction);

        /// <summary>
        /// Updates an existing transaction in the repository.
        /// </summary>
        /// <param name="transaction">The transaction to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateTransactionAsync(StmtrptDomainModels.StatementReport.Transaction transaction);

        /// <summary>
        /// Deletes a transaction by transaction ID.
        /// </summary>
        /// <param name="transactionId">The transaction ID of the transaction to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteTransactionByIdAsync(string transactionId);
    }

    public interface IStmtrptService
    {
        /// <summary>
        /// Executes the main logic of the STMTRPT program.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync();
    }
}