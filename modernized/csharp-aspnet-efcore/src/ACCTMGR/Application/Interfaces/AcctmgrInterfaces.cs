using System.Collections.Generic;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    /// <summary>
    /// Interface for account management operations.
    /// </summary>
    public interface IAccountManagerService
    {
        /// <summary>
        /// Initializes the account management system.
        /// </summary>
        void InitializeSystem();

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="account">The account details to create.</param>
        void CreateAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Updates an existing account.
        /// </summary>
        /// <param name="account">The account details to update.</param>
        void UpdateAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Deletes an account by account number.
        /// </summary>
        /// <param name="accountNumber">The account number to delete.</param>
        void DeleteAccount(string accountNumber);

        /// <summary>
        /// Retrieves an account by account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve.</param>
        /// <returns>The account details.</returns>
        AcctmgrDomainModels.Account GetAccount(string accountNumber);

        /// <summary>
        /// Lists all accounts.
        /// </summary>
        /// <returns>A list of all accounts.</returns>
        IEnumerable<AcctmgrDomainModels.Account> ListAccounts();

        /// <summary>
        /// Processes a deposit transaction.
        /// </summary>
        /// <param name="accountNumber">The account number to deposit into.</param>
        /// <param name="amount">The amount to deposit.</param>
        void ProcessDeposit(string accountNumber, decimal amount);

        /// <summary>
        /// Processes a withdrawal transaction.
        /// </summary>
        /// <param name="accountNumber">The account number to withdraw from.</param>
        /// <param name="amount">The amount to withdraw.</param>
        void ProcessWithdrawal(string accountNumber, decimal amount);

        /// <summary>
        /// Checks the balance of an account.
        /// </summary>
        /// <param name="accountNumber">The account number to check.</param>
        /// <returns>The account balance.</returns>
        decimal CheckBalance(string accountNumber);

        /// <summary>
        /// Changes the status of an account.
        /// </summary>
        /// <param name="accountNumber">The account number to update.</param>
        /// <param name="status">The new account status.</param>
        void ChangeAccountStatus(string accountNumber, AcctmgrDomainModels.AccountStatusEnum status);
    }

    /// <summary>
    /// Interface for account repository operations.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Reads an account by account number.
        /// </summary>
        /// <param name="accountNumber">The account number to read.</param>
        /// <returns>The account details.</returns>
        AcctmgrDomainModels.Account ReadAccount(string accountNumber);

        /// <summary>
        /// Writes an account to the repository.
        /// </summary>
        /// <param name="account">The account details to write.</param>
        void WriteAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Deletes an account from the repository.
        /// </summary>
        /// <param name="accountNumber">The account number to delete.</param>
        void DeleteAccount(string accountNumber);

        /// <summary>
        /// Retrieves all accounts from the repository.
        /// </summary>
        /// <returns>A list of all accounts.</returns>
        IEnumerable<AcctmgrDomainModels.Account> GetAllAccounts();
    }

    /// <summary>
    /// Interface for CICS transaction operations.
    /// </summary>
    public interface ICicsTransactionService
    {
        /// <summary>
        /// Executes a CICS transaction.
        /// </summary>
        /// <param name="transactionId">The transaction ID to execute.</param>
        /// <param name="parameters">The parameters for the transaction.</param>
        /// <returns>The result of the transaction.</returns>
        string ExecuteTransaction(string transactionId, IDictionary<string, string> parameters);
    }
}