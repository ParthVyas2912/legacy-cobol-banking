using System;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    /// <summary>
    /// Interface for handling customer-related operations.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Retrieves customer details by customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>The customer details.</returns>
        IntcalcDomainModels.Customer GetCustomerDetails(string customerId);

        /// <summary>
        /// Updates the account balance for a customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="newBalance">The new account balance.</param>
        void UpdateAccountBalance(string customerId, decimal newBalance);

        /// <summary>
        /// Updates the account status for a customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="status">The new account status.</param>
        void UpdateAccountStatus(string customerId, IntcalcDomainModels.AccountStatusEnum status);
    }

    /// <summary>
    /// Interface for handling account-related operations.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Retrieves account details by account number.
        /// </summary>
        /// <param name="accountNumber">The account number.</param>
        /// <returns>The account details.</returns>
        IntcalcDomainModels.Account GetAccountDetails(string accountNumber);

        /// <summary>
        /// Updates the current balance of an account.
        /// </summary>
        /// <param name="accountNumber">The account number.</param>
        /// <param name="newBalance">The new balance.</param>
        void UpdateAccountBalance(string accountNumber, decimal newBalance);

        /// <summary>
        /// Adds a transaction to an account.
        /// </summary>
        /// <param name="accountNumber">The account number.</param>
        /// <param name="transaction">The transaction to add.</param>
        void AddTransaction(string accountNumber, IntcalcDomainModels.Transaction transaction);
    }

    /// <summary>
    /// Interface for handling transaction-related operations.
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Retrieves transaction details by transaction ID.
        /// </summary>
        /// <param name="transactionId">The transaction ID.</param>
        /// <returns>The transaction details.</returns>
        IntcalcDomainModels.Transaction GetTransactionDetails(string transactionId);

        /// <summary>
        /// Processes a new transaction.
        /// </summary>
        /// <param name="transaction">The transaction to process.</param>
        void ProcessTransaction(IntcalcDomainModels.Transaction transaction);

        /// <summary>
        /// Retrieves all transactions for a specific account.
        /// </summary>
        /// <param name="accountNumber">The account number.</param>
        /// <returns>A list of transactions for the account.</returns>
        IEnumerable<IntcalcDomainModels.Transaction> GetTransactionsByAccount(string accountNumber);
    }

    /// <summary>
    /// Interface for handling file I/O operations related to customers.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Reads customer data from the file.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>The customer data.</returns>
        IntcalcDomainModels.Customer ReadCustomer(string customerId);

        /// <summary>
        /// Writes customer data to the file.
        /// </summary>
        /// <param name="customer">The customer data to write.</param>
        void WriteCustomer(IntcalcDomainModels.Customer customer);

        /// <summary>
        /// Deletes customer data from the file.
        /// </summary>
        /// <param name="customerId">The ID of the customer to delete.</param>
        void DeleteCustomer(string customerId);
    }

    /// <summary>
    /// Interface for handling file I/O operations related to accounts.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Reads account data from the file.
        /// </summary>
        /// <param name="accountNumber">The account number.</param>
        /// <returns>The account data.</returns>
        IntcalcDomainModels.Account ReadAccount(string accountNumber);

        /// <summary>
        /// Writes account data to the file.
        /// </summary>
        /// <param name="account">The account data to write.</param>
        void WriteAccount(IntcalcDomainModels.Account account);

        /// <summary>
        /// Deletes account data from the file.
        /// </summary>
        /// <param name="accountNumber">The account number to delete.</param>
        void DeleteAccount(string accountNumber);
    }

    /// <summary>
    /// Interface for handling file I/O operations related to transactions.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads transaction data from the file.
        /// </summary>
        /// <param name="transactionId">The transaction ID.</param>
        /// <returns>The transaction data.</returns>
        IntcalcDomainModels.Transaction ReadTransaction(string transactionId);

        /// <summary>
        /// Writes transaction data to the file.
        /// </summary>
        /// <param name="transaction">The transaction data to write.</param>
        void WriteTransaction(IntcalcDomainModels.Transaction transaction);

        /// <summary>
        /// Deletes transaction data from the file.
        /// </summary>
        /// <param name="transactionId">The transaction ID to delete.</param>
        void DeleteTransaction(string transactionId);
    }

    /// <summary>
    /// Interface for handling CICS operations.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Sends a request to the CICS system.
        /// </summary>
        /// <param name="requestData">The request data to send.</param>
        /// <returns>The response data from the CICS system.</returns>
        string SendCicsRequest(string requestData);

        /// <summary>
        /// Receives a response from the CICS system.
        /// </summary>
        /// <returns>The response data from the CICS system.</returns>
        string ReceiveCicsResponse();

        /// <summary>
        /// Commits a transaction in the CICS system.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back a transaction in the CICS system.
        /// </summary>
        void RollbackTransaction();
    }
}