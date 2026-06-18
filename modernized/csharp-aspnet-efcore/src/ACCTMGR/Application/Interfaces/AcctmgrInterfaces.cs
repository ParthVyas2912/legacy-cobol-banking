using System;
using System.Collections.Generic;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Reads account details based on the account number.
        /// </summary>
        /// <param name="accountNumber">The account number to search for.</param>
        /// <returns>The account details if found, otherwise null.</returns>
        AcctmgrDomainModels.Account GetAccountByNumber(string accountNumber);

        /// <summary>
        /// Updates the account details in the repository.
        /// </summary>
        /// <param name="account">The account object containing updated details.</param>
        void UpdateAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Adds a new account to the repository.
        /// </summary>
        /// <param name="account">The account object to add.</param>
        void AddAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Deletes an account from the repository based on the account number.
        /// </summary>
        /// <param name="accountNumber">The account number of the account to delete.</param>
        void DeleteAccount(string accountNumber);
    }

    public interface ITransactionRepository
    {
        /// <summary>
        /// Retrieves all transactions for a given account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A list of transactions associated with the account.</returns>
        List<AcctmgrDomainModels.Transaction> GetTransactionsByAccountNumber(string accountNumber);

        /// <summary>
        /// Adds a new transaction to the repository.
        /// </summary>
        /// <param name="transaction">The transaction object to add.</param>
        void AddTransaction(AcctmgrDomainModels.Transaction transaction);
    }

    public interface IAccountService
    {
        /// <summary>
        /// Opens a new account with the specified details.
        /// </summary>
        /// <param name="account">The account details to create.</param>
        void OpenAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Closes an existing account.
        /// </summary>
        /// <param name="accountNumber">The account number to close.</param>
        void CloseAccount(string accountNumber);

        /// <summary>
        /// Deposits an amount into the specified account.
        /// </summary>
        /// <param name="accountNumber">The account number to deposit into.</param>
        /// <param name="amount">The amount to deposit.</param>
        void Deposit(string accountNumber, decimal amount);

        /// <summary>
        /// Withdraws an amount from the specified account.
        /// </summary>
        /// <param name="accountNumber">The account number to withdraw from.</param>
        /// <param name="amount">The amount to withdraw.</param>
        void Withdraw(string accountNumber, decimal amount);

        /// <summary>
        /// Transfers an amount from one account to another.
        /// </summary>
        /// <param name="fromAccountNumber">The account number to transfer from.</param>
        /// <param name="toAccountNumber">The account number to transfer to.</param>
        /// <param name="amount">The amount to transfer.</param>
        void Transfer(string fromAccountNumber, string toAccountNumber, decimal amount);

        /// <summary>
        /// Retrieves the balance of the specified account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve the balance for.</param>
        /// <returns>The balance of the account.</returns>
        decimal GetAccountBalance(string accountNumber);

        /// <summary>
        /// Retrieves the status of the specified account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve the status for.</param>
        /// <returns>The status of the account.</returns>
        AcctmgrDomainModels.AccountStatus GetAccountStatus(string accountNumber);

        /// <summary>
        /// Retrieves the account details along with its transactions.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve details for.</param>
        /// <returns>The account details including transactions.</returns>
        AcctmgrDomainModels.Account GetAccountWithTransactions(string accountNumber);
    }
}