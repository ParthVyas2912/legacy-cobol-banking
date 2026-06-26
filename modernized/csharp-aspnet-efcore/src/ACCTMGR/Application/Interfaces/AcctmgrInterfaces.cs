using System.Collections.Generic;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    /// <summary>
    /// Interface for account management operations.
    /// </summary>
    public interface IAccountManager
    {
        /// <summary>
        /// Initializes the account management system.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Processes account transactions.
        /// </summary>
        void ProcessTransactions();

        /// <summary>
        /// Generates account reports.
        /// </summary>
        void GenerateReports();

        /// <summary>
        /// Finalizes the account management system.
        /// </summary>
        void Finalize();
    }

    /// <summary>
    /// Interface for account repository operations.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Reads account data from the data source.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve.</param>
        /// <returns>The account details.</returns>
        AcctmgrDomainModels.Account ReadAccount(string accountNumber);

        /// <summary>
        /// Writes account data to the data source.
        /// </summary>
        /// <param name="account">The account details to save.</param>
        void WriteAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Retrieves all accounts from the data source.
        /// </summary>
        /// <returns>A list of all accounts.</returns>
        List<AcctmgrDomainModels.Account> GetAllAccounts();
    }

    /// <summary>
    /// Interface for CICS service operations.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Sends a transaction to the CICS system.
        /// </summary>
        /// <param name="transactionData">The transaction data to send.</param>
        /// <returns>The response from the CICS system.</returns>
        string SendTransaction(string transactionData);

        /// <summary>
        /// Retrieves data from the CICS system.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <returns>The data retrieved from the CICS system.</returns>
        string RetrieveData(string query);
    }
}