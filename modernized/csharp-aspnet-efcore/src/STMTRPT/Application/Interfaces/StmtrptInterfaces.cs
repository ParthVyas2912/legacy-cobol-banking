using System.Collections.Generic;
using STMTRPT.Domain;

namespace STMTRPT.Application.Interfaces
{
    public interface IStmtrptRepository
    {
        /// <summary>
        /// Reads account summaries from the data source.
        /// </summary>
        /// <returns>A list of account summaries.</returns>
        List<StmtrptDomainModels.AccountSummary> ReadAccountSummaries();

        /// <summary>
        /// Writes account summaries to the data source.
        /// </summary>
        /// <param name="accountSummaries">The list of account summaries to write.</param>
        void WriteAccountSummaries(List<StmtrptDomainModels.AccountSummary> accountSummaries);

        /// <summary>
        /// Reads transaction details for a specific account from the data source.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A list of transaction details for the specified account.</returns>
        List<StmtrptDomainModels.TransactionDetail> ReadTransactionDetails(string accountNumber);

        /// <summary>
        /// Writes transaction details for a specific account to the data source.
        /// </summary>
        /// <param name="accountNumber">The account number to write transactions for.</param>
        /// <param name="transactionDetails">The list of transaction details to write.</param>
        void WriteTransactionDetails(string accountNumber, List<StmtrptDomainModels.TransactionDetail> transactionDetails);
    }

    public interface IStmtrptService
    {
        /// <summary>
        /// Retrieves account details for a specific account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve details for.</param>
        /// <returns>The account summary for the specified account number.</returns>
        StmtrptDomainModels.AccountSummary GetAccountDetails(string accountNumber);

        /// <summary>
        /// Updates the status of an account.
        /// </summary>
        /// <param name="accountNumber">The account number to update.</param>
        /// <param name="status">The new status to set.</param>
        void UpdateAccountStatus(string accountNumber, StmtrptDomainModels.AccountStatus status);

        /// <summary>
        /// Processes transactions for a specific account.
        /// </summary>
        /// <param name="accountNumber">The account number to process transactions for.</param>
        /// <param name="transactions">The list of transactions to process.</param>
        void ProcessTransactions(string accountNumber, List<StmtrptDomainModels.TransactionDetail> transactions);

        /// <summary>
        /// Generates a statement report for a specific account.
        /// </summary>
        /// <param name="accountNumber">The account number to generate the statement for.</param>
        /// <returns>A string representing the generated statement report.</returns>
        string GenerateStatementReport(string accountNumber);

        /// <summary>
        /// Validates the account number format.
        /// </summary>
        /// <param name="accountNumber">The account number to validate.</param>
        /// <returns>True if the account number is valid, otherwise false.</returns>
        bool ValidateAccountNumber(string accountNumber);

        /// <summary>
        /// Calculates the total balance for a specific account based on its transactions.
        /// </summary>
        /// <param name="accountNumber">The account number to calculate the balance for.</param>
        /// <returns>The calculated total balance.</returns>
        decimal CalculateTotalBalance(string accountNumber);
    }
}