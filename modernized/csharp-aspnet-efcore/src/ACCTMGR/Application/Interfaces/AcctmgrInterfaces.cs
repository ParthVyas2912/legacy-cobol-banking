using System.Collections.Generic;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Reads account details by account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve.</param>
        /// <returns>The account details.</returns>
        AcctmgrDomainModels.Account ReadAccount(string accountNumber);

        /// <summary>
        /// Writes account details to the repository.
        /// </summary>
        /// <param name="account">The account details to write.</param>
        void WriteAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Reads all accounts from the repository.
        /// </summary>
        /// <returns>A list of all accounts.</returns>
        IEnumerable<AcctmgrDomainModels.Account> ReadAllAccounts();
    }

    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads transactions for a specific account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A list of transactions for the account.</returns>
        IEnumerable<AcctmgrDomainModels.Account.Transaction> ReadTransactions(string accountNumber);

        /// <summary>
        /// Writes a transaction to the repository.
        /// </summary>
        /// <param name="accountNumber">The account number associated with the transaction.</param>
        /// <param name="transaction">The transaction details to write.</param>
        void WriteTransaction(string accountNumber, AcctmgrDomainModels.Account.Transaction transaction);
    }

    public interface ICicsService
    {
        /// <summary>
        /// Executes a CICS operation to retrieve account information.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve.</param>
        /// <returns>The account details retrieved via CICS.</returns>
        AcctmgrDomainModels.Account RetrieveAccountViaCics(string accountNumber);

        /// <summary>
        /// Executes a CICS operation to update account information.
        /// </summary>
        /// <param name="account">The account details to update.</param>
        void UpdateAccountViaCics(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Executes a CICS operation to process a transaction.
        /// </summary>
        /// <param name="accountNumber">The account number associated with the transaction.</param>
        /// <param name="transaction">The transaction details to process.</param>
        void ProcessTransactionViaCics(string accountNumber, AcctmgrDomainModels.Account.Transaction transaction);
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
        /// Retrieves account details.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve.</param>
        /// <returns>The account details.</returns>
        AcctmgrDomainModels.Account GetAccountDetails(string accountNumber);

        /// <summary>
        /// Updates account details.
        /// </summary>
        /// <param name="account">The account details to update.</param>
        void UpdateAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Processes a transaction for an account.
        /// </summary>
        /// <param name="accountNumber">The account number associated with the transaction.</param>
        /// <param name="transaction">The transaction details to process.</param>
        void ProcessTransaction(string accountNumber, AcctmgrDomainModels.Account.Transaction transaction);
    }
}