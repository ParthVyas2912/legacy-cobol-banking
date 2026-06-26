using System;
using System.Collections.Generic;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    /// <summary>
    /// Interface for account management operations.
    /// Each method corresponds to a paragraph in the original COBOL program.
    /// </summary>
    public interface IAcctmgrService
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
        /// Processes a transaction for an account.
        /// </summary>
        /// <param name="accountNumber">The account number to process the transaction for.</param>
        /// <param name="transaction">The transaction details.</param>
        void ProcessTransaction(string accountNumber, AcctmgrDomainModels.Transaction transaction);

        /// <summary>
        /// Retrieves all transactions for an account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A list of transactions.</returns>
        List<AcctmgrDomainModels.Transaction> GetTransactions(string accountNumber);

        /// <summary>
        /// Generates a report for an account.
        /// </summary>
        /// <param name="accountNumber">The account number to generate the report for.</param>
        /// <returns>The account report as a string.</returns>
        string GenerateAccountReport(string accountNumber);

        /// <summary>
        /// Validates the account data.
        /// </summary>
        /// <param name="account">The account details to validate.</param>
        /// <returns>True if valid, otherwise false.</returns>
        bool ValidateAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Validates the transaction data.
        /// </summary>
        /// <param name="transaction">The transaction details to validate.</param>
        /// <returns>True if valid, otherwise false.</returns>
        bool ValidateTransaction(AcctmgrDomainModels.Transaction transaction);
    }

    /// <summary>
    /// Interface for account repository operations.
    /// Handles file I/O operations for account data.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Reads account data from the repository.
        /// </summary>
        /// <param name="accountNumber">The account number to read.</param>
        /// <returns>The account details.</returns>
        AcctmgrDomainModels.Account ReadAccount(string accountNumber);

        /// <summary>
        /// Writes account data to the repository.
        /// </summary>
        /// <param name="account">The account details to write.</param>
        void WriteAccount(AcctmgrDomainModels.Account account);

        /// <summary>
        /// Deletes account data from the repository.
        /// </summary>
        /// <param name="accountNumber">The account number to delete.</param>
        void DeleteAccount(string accountNumber);

        /// <summary>
        /// Checks if an account exists in the repository.
        /// </summary>
        /// <param name="accountNumber">The account number to check.</param>
        /// <returns>True if the account exists, otherwise false.</returns>
        bool AccountExists(string accountNumber);
    }

    /// <summary>
    /// Interface for transaction repository operations.
    /// Handles file I/O operations for transaction data.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads transaction data for an account from the repository.
        /// </summary>
        /// <param name="accountNumber">The account number to read transactions for.</param>
        /// <returns>A list of transactions.</returns>
        List<AcctmgrDomainModels.Transaction> ReadTransactions(string accountNumber);

        /// <summary>
        /// Writes transaction data to the repository.
        /// </summary>
        /// <param name="accountNumber">The account number to write transactions for.</param>
        /// <param name="transactions">The list of transactions to write.</param>
        void WriteTransactions(string accountNumber, List<AcctmgrDomainModels.Transaction> transactions);

        /// <summary>
        /// Deletes transaction data for an account from the repository.
        /// </summary>
        /// <param name="accountNumber">The account number to delete transactions for.</param>
        void DeleteTransactions(string accountNumber);
    }

    /// <summary>
    /// Interface for CICS operations related to account management.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Commits a transaction in the CICS environment.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back a transaction in the CICS environment.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Sends a message to the CICS terminal.
        /// </summary>
        /// <param name="message">The message to send.</param>
        void SendMessage(string message);

        /// <summary>
        /// Retrieves the status of the last CICS operation.
        /// </summary>
        /// <returns>The status as a string.</returns>
        string GetLastOperationStatus();
    }
}