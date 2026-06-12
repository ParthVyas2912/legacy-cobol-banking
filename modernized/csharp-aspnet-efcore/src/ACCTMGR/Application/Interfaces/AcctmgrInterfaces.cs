using System.Collections.Generic;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Reads an account by its account number.
        /// </summary>
        /// <param name="accountNumber">The account number to search for.</param>
        /// <returns>The account details if found, otherwise null.</returns>
        AcctmgrDomainModels.Account GetAccountByNumber(string accountNumber);

        /// <summary>
        /// Updates the details of an account.
        /// </summary>
        /// <param name="account">The account object containing updated details.</param>
        void UpdateAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Adds a new account to the repository.
        /// </summary>
        /// <param name="account">The account object to add.</param>
        void AddAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Deletes an account by its account number.
        /// </summary>
        /// <param name="accountNumber">The account number of the account to delete.</param>
        void DeleteAccount(string accountNumber);

        /// <summary>
        /// Retrieves all accounts in the repository.
        /// </summary>
        /// <returns>A list of all accounts.</returns>
        List<AcctmgrDomainModels.Account> GetAllAccounts();
    }

    public interface ITransactionRepository
    {
        /// <summary>
        /// Retrieves all transactions for a given account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A list of transactions associated with the account.</returns>
        List<AcctmgrDomainModels.Account.Transaction> GetTransactionsByAccountNumber(string accountNumber);

        /// <summary>
        /// Adds a transaction to an account.
        /// </summary>
        /// <param name="accountNumber">The account number to add the transaction to.</param>
        /// <param name="transaction">The transaction details to add.</param>
        void AddTransaction(string accountNumber, AcctmgrDomainModels.Account.Transaction transaction);

        /// <summary>
        /// Deletes a transaction by its transaction code.
        /// </summary>
        /// <param name="accountNumber">The account number associated with the transaction.</param>
        /// <param name="transactionCode">The transaction code of the transaction to delete.</param>
        void DeleteTransaction(string accountNumber, string transactionCode);
    }

    public interface ICicsService
    {
        /// <summary>
        /// Executes a CICS operation to validate an account.
        /// </summary>
        /// <param name="accountNumber">The account number to validate.</param>
        /// <returns>True if the account is valid, otherwise false.</returns>
        bool ValidateAccount(string accountNumber);

        /// <summary>
        /// Executes a CICS operation to process a transaction.
        /// </summary>
        /// <param name="transaction">The transaction details to process.</param>
        /// <returns>True if the transaction was successfully processed, otherwise false.</returns>
        bool ProcessTransaction(AcctmgrDomainModels.Account.Transaction transaction);

        /// <summary>
        /// Executes a CICS operation to retrieve account status.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve the status for.</param>
        /// <returns>The status of the account.</returns>
        AcctmgrDomainModels.Account.StatusEnum GetAccountStatus(string accountNumber);
    }
}