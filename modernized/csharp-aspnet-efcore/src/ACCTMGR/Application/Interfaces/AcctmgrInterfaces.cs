using System;
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
        /// Opens a new account.
        /// </summary>
        /// <param name="account">The account details to be created.</param>
        void OpenAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Closes an existing account.
        /// </summary>
        /// <param name="accountNumber">The account number to be closed.</param>
        void CloseAccount(string accountNumber);

        /// <summary>
        /// Suspends an account.
        /// </summary>
        /// <param name="accountNumber">The account number to be suspended.</param>
        void SuspendAccount(string accountNumber);

        /// <summary>
        /// Reactivates a suspended account.
        /// </summary>
        /// <param name="accountNumber">The account number to be reactivated.</param>
        void ReactivateAccount(string accountNumber);

        /// <summary>
        /// Retrieves account details by account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve details for.</param>
        /// <returns>The account details.</returns>
        AcctmgrDomainModels.Account GetAccountDetails(string accountNumber);

        /// <summary>
        /// Processes a transaction for an account.
        /// </summary>
        /// <param name="transaction">The transaction details to be processed.</param>
        void ProcessTransaction(AcctmgrDomainModels.Transaction transaction);

        /// <summary>
        /// Retrieves the transaction history for an account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transaction history for.</param>
        /// <returns>A list of transactions for the account.</returns>
        List<AcctmgrDomainModels.Transaction> GetTransactionHistory(string accountNumber);
    }

    /// <summary>
    /// Interface for account repository operations.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Adds a new account to the repository.
        /// </summary>
        /// <param name="account">The account to be added.</param>
        void AddAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Updates an existing account in the repository.
        /// </summary>
        /// <param name="account">The account to be updated.</param>
        void UpdateAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Deletes an account from the repository.
        /// </summary>
        /// <param name="accountNumber">The account number to be deleted.</param>
        void DeleteAccount(string accountNumber);

        /// <summary>
        /// Retrieves an account by account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve.</param>
        /// <returns>The account details.</returns>
        AcctmgrDomainModels.Account GetAccount(string accountNumber);
    }

    /// <summary>
    /// Interface for transaction repository operations.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Adds a new transaction to the repository.
        /// </summary>
        /// <param name="transaction">The transaction to be added.</param>
        void AddTransaction(AcctmgrDomainModels.Transaction transaction);

        /// <summary>
        /// Retrieves transactions for a specific account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A list of transactions for the account.</returns>
        List<AcctmgrDomainModels.Transaction> GetTransactionsByAccount(string accountNumber);
    }
}