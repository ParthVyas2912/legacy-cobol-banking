using System;
using System.Collections.Generic;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    /// <summary>
    /// Interface for account management operations.
    /// Each method corresponds to a paragraph in the original COBOL program.
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
        /// <param name="account">The updated account details.</param>
        void UpdateAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Deletes an account based on the account number.
        /// </summary>
        /// <param name="accountNumber">The account number to delete.</param>
        void DeleteAccount(string accountNumber);

        /// <summary>
        /// Retrieves account details based on the account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve.</param>
        /// <returns>The account details.</returns>
        AcctmgrDomainModels.Account GetAccountDetails(string accountNumber);

        /// <summary>
        /// Processes a deposit transaction for an account.
        /// </summary>
        /// <param name="accountNumber">The account number to deposit into.</param>
        /// <param name="amount">The amount to deposit.</param>
        void ProcessDeposit(string accountNumber, decimal amount);

        /// <summary>
        /// Processes a withdrawal transaction for an account.
        /// </summary>
        /// <param name="accountNumber">The account number to withdraw from.</param>
        /// <param name="amount">The amount to withdraw.</param>
        void ProcessWithdrawal(string accountNumber, decimal amount);

        /// <summary>
        /// Processes a transfer transaction between two accounts.
        /// </summary>
        /// <param name="fromAccountNumber">The source account number.</param>
        /// <param name="toAccountNumber">The destination account number.</param>
        /// <param name="amount">The amount to transfer.</param>
        void ProcessTransfer(string fromAccountNumber, string toAccountNumber, decimal amount);

        /// <summary>
        /// Retrieves the transaction history for an account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A list of transactions for the account.</returns>
        IEnumerable<AcctmgrDomainModels.Transaction> GetTransactionHistory(string accountNumber);

        /// <summary>
        /// Validates the account number format.
        /// </summary>
        /// <param name="accountNumber">The account number to validate.</param>
        /// <returns>True if the account number is valid, otherwise false.</returns>
        bool ValidateAccountNumber(string accountNumber);

        /// <summary>
        /// Validates the transaction amount.
        /// </summary>
        /// <param name="amount">The transaction amount to validate.</param>
        /// <returns>True if the amount is valid, otherwise false.</returns>
        bool ValidateTransactionAmount(decimal amount);

        /// <summary>
        /// Generates a report for all accounts.
        /// </summary>
        /// <returns>A report containing details of all accounts.</returns>
        string GenerateAccountReport();

        /// <summary>
        /// Generates a report for all transactions.
        /// </summary>
        /// <returns>A report containing details of all transactions.</returns>
        string GenerateTransactionReport();

        /// <summary>
        /// Locks an account to prevent transactions.
        /// </summary>
        /// <param name="accountNumber">The account number to lock.</param>
        void LockAccount(string accountNumber);

        /// <summary>
        /// Unlocks an account to allow transactions.
        /// </summary>
        /// <param name="accountNumber">The account number to unlock.</param>
        void UnlockAccount(string accountNumber);

        /// <summary>
        /// Checks the balance of an account.
        /// </summary>
        /// <param name="accountNumber">The account number to check.</param>
        /// <returns>The current balance of the account.</returns>
        decimal CheckAccountBalance(string accountNumber);
    }

    /// <summary>
    /// Interface for account repository operations.
    /// Each method corresponds to file I/O operations in the original COBOL program.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Reads account data from the repository.
        /// </summary>
        /// <param name="accountNumber">The account number to read.</param>
        /// <returns>The account data.</returns>
        AcctmgrDomainModels.Account ReadAccount(string accountNumber);

        /// <summary>
        /// Writes account data to the repository.
        /// </summary>
        /// <param name="account">The account data to write.</param>
        void WriteAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Deletes account data from the repository.
        /// </summary>
        /// <param name="accountNumber">The account number to delete.</param>
        void DeleteAccount(string accountNumber);

        /// <summary>
        /// Reads transaction data from the repository.
        /// </summary>
        /// <param name="accountNumber">The account number to read transactions for.</param>
        /// <returns>A list of transactions for the account.</returns>
        IEnumerable<AcctmgrDomainModels.Transaction> ReadTransactions(string accountNumber);

        /// <summary>
        /// Writes transaction data to the repository.
        /// </summary>
        /// <param name="accountNumber">The account number associated with the transaction.</param>
        /// <param name="transaction">The transaction data to write.</param>
        void WriteTransaction(string accountNumber, AcctmgrDomainModels.Transaction transaction);

        /// <summary>
        /// Locks an account in the repository.
        /// </summary>
        /// <param name="accountNumber">The account number to lock.</param>
        void LockAccount(string accountNumber);

        /// <summary>
        /// Unlocks an account in the repository.
        /// </summary>
        /// <param name="accountNumber">The account number to unlock.</param>
        void UnlockAccount(string accountNumber);

        /// <summary>
        /// Reads all accounts from the repository.
        /// </summary>
        /// <returns>A list of all accounts.</returns>
        IEnumerable<AcctmgrDomainModels.Account> ReadAllAccounts();
    }

    /// <summary>
    /// Interface for CICS service operations.
    /// Each method corresponds to a CICS operation in the original COBOL program.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Sends a message to the CICS system.
        /// </summary>
        /// <param name="message">The message to send.</param>
        void SendMessage(string message);

        /// <summary>
        /// Receives a message from the CICS system.
        /// </summary>
        /// <returns>The received message.</returns>
        string ReceiveMessage();

        /// <summary>
        /// Commits a transaction in the CICS system.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back a transaction in the CICS system.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Retrieves the status of a CICS transaction.
        /// </summary>
        /// <param name="transactionId">The transaction ID to check.</param>
        /// <returns>The status of the transaction.</returns>
        string GetTransactionStatus(string transactionId);
    }
}