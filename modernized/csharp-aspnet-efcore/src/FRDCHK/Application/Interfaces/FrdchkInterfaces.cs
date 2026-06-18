using System.Collections.Generic;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    public interface IFrdchkRepository
    {
        /// <summary>
        /// Reads a customer record by customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID to search for.</param>
        /// <returns>The customer record if found, otherwise null.</returns>
        FrdchkDomainModels.CustomerRecord ReadCustomerRecord(string customerId);

        /// <summary>
        /// Writes a customer record to the repository.
        /// </summary>
        /// <param name="customerRecord">The customer record to write.</param>
        void WriteCustomerRecord(FrdchkDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Deletes a customer record by customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID of the record to delete.</param>
        void DeleteCustomerRecord(string customerId);

        /// <summary>
        /// Reads all flagged accounts from the repository.
        /// </summary>
        /// <returns>A list of flagged accounts.</returns>
        List<FrdchkDomainModels.Account> ReadFlaggedAccounts();

        /// <summary>
        /// Updates the balance of a specific account.
        /// </summary>
        /// <param name="accountNumber">The account number to update.</param>
        /// <param name="newBalance">The new balance to set.</param>
        void UpdateAccountBalance(string accountNumber, decimal newBalance);
    }

    public interface IFrdchkService
    {
        /// <summary>
        /// Validates a customer record for fraud detection.
        /// </summary>
        /// <param name="customerRecord">The customer record to validate.</param>
        /// <returns>A list of validation errors, if any.</returns>
        List<string> ValidateCustomerRecord(FrdchkDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Processes a transaction for a given account.
        /// </summary>
        /// <param name="accountNumber">The account number associated with the transaction.</param>
        /// <param name="amount">The transaction amount.</param>
        /// <returns>A boolean indicating whether the transaction was processed successfully.</returns>
        bool ProcessTransaction(string accountNumber, decimal amount);

        /// <summary>
        /// Retrieves a list of flagged accounts for further review.
        /// </summary>
        /// <returns>A list of flagged accounts.</returns>
        List<FrdchkDomainModels.Account> GetFlaggedAccounts();

        /// <summary>
        /// Flags an account for review based on suspicious activity.
        /// </summary>
        /// <param name="accountNumber">The account number to flag.</param>
        void FlagAccount(string accountNumber);

        /// <summary>
        /// Unflags an account after review.
        /// </summary>
        /// <param name="accountNumber">The account number to unflag.</param>
        void UnflagAccount(string accountNumber);

        /// <summary>
        /// Generates a fraud detection report for a specific customer.
        /// </summary>
        /// <param name="customerId">The customer ID to generate the report for.</param>
        /// <returns>A string containing the fraud detection report.</returns>
        string GenerateFraudReport(string customerId);
    }
}