using System;
using System.Collections.Generic;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    /// <summary>
    /// Interface for account management operations.
    /// Each COBOL paragraph is represented as a method.
    /// </summary>
    public interface IAcctmgrService
    {
        /// <summary>
        /// Initializes the account management system.
        /// </summary>
        void InitializeSystem();

        /// <summary>
        /// Retrieves account details based on the account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve details for.</param>
        /// <returns>The account details.</returns>
        AcctmgrDomainModels.Account GetAccountDetails(string accountNumber);

        /// <summary>
        /// Updates the balance of an account.
        /// </summary>
        /// <param name="accountNumber">The account number to update.</param>
        /// <param name="newBalance">The new balance to set.</param>
        void UpdateAccountBalance(string accountNumber, decimal newBalance);

        /// <summary>
        /// Processes a transaction for a given account.
        /// </summary>
        /// <param name="accountNumber">The account number to process the transaction for.</param>
        /// <param name="transaction">The transaction details.</param>
        void ProcessTransaction(string accountNumber, AcctmgrDomainModels.Account.Transaction transaction);

        /// <summary>
        /// Closes an account.
        /// </summary>
        /// <param name="accountNumber">The account number to close.</param>
        void CloseAccount(string accountNumber);

        /// <summary>
        /// Generates a report of all accounts.
        /// </summary>
        /// <returns>A list of all accounts.</returns>
        List<AcctmgrDomainModels.Account> GenerateAccountReport();
    }

    /// <summary>
    /// Interface for account repository operations.
    /// File I/O operations are represented as repository methods.
    /// </summary>
    public interface IAcctmgrRepository
    {
        /// <summary>
        /// Reads account data from the data store.
        /// </summary>
        /// <param name="accountNumber">The account number to read.</param>
        /// <returns>The account details.</returns>
        AcctmgrDomainModels.Account ReadAccount(string accountNumber);

        /// <summary>
        /// Writes account data to the data store.
        /// </summary>
        /// <param name="account">The account details to write.</param>
        void WriteAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Deletes account data from the data store.
        /// </summary>
        /// <param name="accountNumber">The account number to delete.</param>
        void DeleteAccount(string accountNumber);

        /// <summary>
        /// Reads all accounts from the data store.
        /// </summary>
        /// <returns>A list of all accounts.</returns>
        List<AcctmgrDomainModels.Account> ReadAllAccounts();
    }

    /// <summary>
    /// Interface for CICS operations related to account management.
    /// </summary>
    public interface IAcctmgrCicsService
    {
        /// <summary>
        /// Sends a CICS transaction for account verification.
        /// </summary>
        /// <param name="accountNumber">The account number to verify.</param>
        /// <returns>True if the account is verified, otherwise false.</returns>
        bool VerifyAccountCics(string accountNumber);

        /// <summary>
        /// Sends a CICS transaction for updating account status.
        /// </summary>
        /// <param name="accountNumber">The account number to update.</param>
        /// <param name="newStatus">The new account status.</param>
        void UpdateAccountStatusCics(string accountNumber, AcctmgrDomainModels.Account.AccountStatusEnum newStatus);
    }
}