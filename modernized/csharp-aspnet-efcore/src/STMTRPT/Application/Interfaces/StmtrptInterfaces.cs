using System.Collections.Generic;
using STMTRPT.Domain;

namespace STMTRPT.Application.Interfaces
{
    public interface IStmtrptRepository
    {
        /// <summary>
        /// Reads customer details from the data source.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve.</param>
        /// <returns>The customer details, including accounts and transactions.</returns>
        StmtrptDomainModels.CustomerDetails ReadCustomerDetails(string customerId);

        /// <summary>
        /// Writes updated account summary information to the data source.
        /// </summary>
        /// <param name="accountSummary">The account summary to update.</param>
        void WriteAccountSummary(StmtrptDomainModels.AccountSummary accountSummary);

        /// <summary>
        /// Reads transaction details for a specific account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A list of transactions for the specified account.</returns>
        List<StmtrptDomainModels.Transaction> ReadTransactions(string accountNumber);

        /// <summary>
        /// Writes a new transaction to the data source.
        /// </summary>
        /// <param name="transaction">The transaction details to write.</param>
        void WriteTransaction(StmtrptDomainModels.Transaction transaction);
    }

    public interface IStmtrptService
    {
        /// <summary>
        /// Retrieves a statement report for a specific account.
        /// </summary>
        /// <param name="accountNumber">The account number for which to generate the statement.</param>
        /// <returns>The account summary including transactions.</returns>
        StmtrptDomainModels.AccountSummary GetStatementReport(string accountNumber);

        /// <summary>
        /// Validates the customer ID and retrieves associated details.
        /// </summary>
        /// <param name="customerId">The customer ID to validate.</param>
        /// <returns>The customer details if validation is successful.</returns>
        StmtrptDomainModels.CustomerDetails ValidateAndRetrieveCustomer(string customerId);

        /// <summary>
        /// Processes a transaction for a specific account.
        /// </summary>
        /// <param name="accountNumber">The account number to process the transaction for.</param>
        /// <param name="transaction">The transaction details.</param>
        void ProcessTransaction(string accountNumber, StmtrptDomainModels.Transaction transaction);

        /// <summary>
        /// Generates a summary report for all accounts associated with a customer.
        /// </summary>
        /// <param name="customerId">The customer ID for which to generate the summary report.</param>
        /// <returns>A list of account summaries for the customer.</returns>
        List<StmtrptDomainModels.AccountSummary> GenerateCustomerSummaryReport(string customerId);
    }
}