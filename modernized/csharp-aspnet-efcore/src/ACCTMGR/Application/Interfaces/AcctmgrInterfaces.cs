using System.Collections.Generic;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Reads account details by account number.
        /// </summary>
        /// <param name="accountNumber">The account number to search for.</param>
        /// <returns>The account details.</returns>
        AcctmgrDomainModels.Account GetAccountByNumber(string accountNumber);

        /// <summary>
        /// Updates account details.
        /// </summary>
        /// <param name="account">The account details to update.</param>
        void UpdateAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Retrieves all accounts.
        /// </summary>
        /// <returns>A list of all accounts.</returns>
        List<AcctmgrDomainModels.Account> GetAllAccounts();

        /// <summary>
        /// Deletes an account by account number.
        /// </summary>
        /// <param name="accountNumber">The account number to delete.</param>
        void DeleteAccount(string accountNumber);
    }

    public interface ITransactionRepository
    {
        /// <summary>
        /// Adds a new transaction.
        /// </summary>
        /// <param name="transaction">The transaction to add.</param>
        void AddTransaction(AcctmgrDomainModels.Transaction transaction);

        /// <summary>
        /// Retrieves transactions by account number.
        /// </summary>
        /// <param name="accountNumber">The account number to search for.</param>
        /// <returns>A list of transactions for the specified account.</returns>
        List<AcctmgrDomainModels.Transaction> GetTransactionsByAccountNumber(string accountNumber);

        /// <summary>
        /// Retrieves all transactions.
        /// </summary>
        /// <returns>A list of all transactions.</returns>
        List<AcctmgrDomainModels.Transaction> GetAllTransactions();
    }

    public interface ICicsService
    {
        /// <summary>
        /// Executes a CICS transaction for account processing.
        /// </summary>
        /// <param name="transactionId">The transaction ID to process.</param>
        /// <returns>A status indicating the result of the transaction.</returns>
        string ExecuteCicsTransaction(string transactionId);

        /// <summary>
        /// Rolls back a CICS transaction.
        /// </summary>
        /// <param name="transactionId">The transaction ID to roll back.</param>
        /// <returns>A status indicating the result of the rollback.</returns>
        string RollbackCicsTransaction(string transactionId);
    }

    public interface IAccountManager
    {
        /// <summary>
        /// Opens a new account.
        /// </summary>
        /// <param name="account">The account details to open.</param>
        void OpenAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Closes an existing account.
        /// </summary>
        /// <param name="accountNumber">The account number to close.</param>
        void CloseAccount(string accountNumber);

        /// <summary>
        /// Processes a transaction for an account.
        /// </summary>
        /// <param name="transaction">The transaction details to process.</param>
        void ProcessTransaction(AcctmgrDomainModels.Transaction transaction);

        /// <summary>
        /// Retrieves account details by account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve.</param>
        /// <returns>The account details.</returns>
        AcctmgrDomainModels.Account RetrieveAccountDetails(string accountNumber);

        /// <summary>
        /// Updates account status.
        /// </summary>
        /// <param name="accountNumber">The account number to update.</param>
        /// <param name="status">The new status to set.</param>
        void UpdateAccountStatus(string accountNumber, AcctmgrDomainModels.AccountStatusEnum status);
    }
}