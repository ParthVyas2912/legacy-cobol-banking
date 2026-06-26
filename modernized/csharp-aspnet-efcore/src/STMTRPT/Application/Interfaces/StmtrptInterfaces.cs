using System;
using System.Collections.Generic;
using STMTRPT.Domain;

namespace STMTRPT.Application.Interfaces
{
    /// <summary>
    /// Interface for handling file I/O operations related to account summaries.
    /// </summary>
    public interface IAccountSummaryRepository
    {
        /// <summary>
        /// Reads account summaries from the data source.
        /// </summary>
        /// <returns>A collection of account summaries.</returns>
        IEnumerable<StmtrptDomainModels.AccountSummary> ReadAccountSummaries();

        /// <summary>
        /// Writes account summaries to the data source.
        /// </summary>
        /// <param name="accountSummaries">The collection of account summaries to write.</param>
        void WriteAccountSummaries(IEnumerable<StmtrptDomainModels.AccountSummary> accountSummaries);
    }

    /// <summary>
    /// Interface for handling CICS operations related to account transactions.
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Retrieves transactions for a specific account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A collection of transactions for the specified account.</returns>
        IEnumerable<StmtrptDomainModels.Transaction> GetTransactions(string accountNumber);

        /// <summary>
        /// Updates the transaction details for a specific account.
        /// </summary>
        /// <param name="accountNumber">The account number to update transactions for.</param>
        /// <param name="transactions">The collection of transactions to update.</param>
        void UpdateTransactions(string accountNumber, IEnumerable<StmtrptDomainModels.Transaction> transactions);
    }

    /// <summary>
    /// Interface for the main STMTRPT application logic.
    /// </summary>
    public interface IStmtrptApplicationService
    {
        /// <summary>
        /// Initializes the application and prepares it for processing.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Processes account summaries and transactions.
        /// </summary>
        void ProcessAccountSummaries();

        /// <summary>
        /// Finalizes the application and performs cleanup operations.
        /// </summary>
        void FinalizeProcessing();
    }
}