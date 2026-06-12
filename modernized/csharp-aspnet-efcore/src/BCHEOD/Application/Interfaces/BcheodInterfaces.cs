using System;
using System.Collections.Generic;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    public interface IBcheodRepository
    {
        /// <summary>
        /// Reads account details from the data source.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve details for.</param>
        /// <returns>The account details if found, otherwise null.</returns>
        BcheodDomainModels.AccountDetails GetAccountDetails(string accountNumber);

        /// <summary>
        /// Updates account details in the data source.
        /// </summary>
        /// <param name="accountDetails">The account details to update.</param>
        void UpdateAccountDetails(BcheodDomainModels.AccountDetails accountDetails);

        /// <summary>
        /// Reads customer details from the data source.
        /// </summary>
        /// <param name="customerId">The customer ID to retrieve details for.</param>
        /// <returns>The customer details if found, otherwise null.</returns>
        BcheodDomainModels.CustomerDetails GetCustomerDetails(string customerId);

        /// <summary>
        /// Writes transaction details to the data source.
        /// </summary>
        /// <param name="transactionDetails">The transaction details to write.</param>
        void WriteTransactionDetails(BcheodDomainModels.TransactionDetails transactionDetails);

        /// <summary>
        /// Retrieves a list of transactions for a given account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A list of transaction details.</returns>
        List<BcheodDomainModels.TransactionDetails> GetTransactionsByAccount(string accountNumber);
    }

    public interface IBcheodService
    {
        /// <summary>
        /// Processes a transaction for a given account.
        /// </summary>
        /// <param name="transactionDetails">The transaction details to process.</param>
        /// <returns>A boolean indicating whether the transaction was successful.</returns>
        bool ProcessTransaction(BcheodDomainModels.TransactionDetails transactionDetails);

        /// <summary>
        /// Validates the status of an account.
        /// </summary>
        /// <param name="accountNumber">The account number to validate.</param>
        /// <returns>A boolean indicating whether the account is active.</returns>
        bool ValidateAccountStatus(string accountNumber);

        /// <summary>
        /// Retrieves the balance for a given account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve the balance for.</param>
        /// <returns>The account balance.</returns>
        decimal GetAccountBalance(string accountNumber);

        /// <summary>
        /// Retrieves a list of transactions for a given account.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A list of transaction details.</returns>
        List<BcheodDomainModels.TransactionDetails> GetAccountTransactions(string accountNumber);

        /// <summary>
        /// Retrieves customer details along with their associated accounts.
        /// </summary>
        /// <param name="customerId">The customer ID to retrieve details for.</param>
        /// <returns>The customer details including accounts.</returns>
        BcheodDomainModels.CustomerDetails GetCustomerWithAccounts(string customerId);
    }
}