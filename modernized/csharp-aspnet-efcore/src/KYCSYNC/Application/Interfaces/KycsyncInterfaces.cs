using System.Collections.Generic;
using KYCSYNC.Domain;

namespace KYCSYNC.Application.Interfaces
{
    public interface IKycsyncService
    {
        /// <summary>
        /// Initializes the KYCSYNC process.
        /// </summary>
        void InitializeKycsync();

        /// <summary>
        /// Validates the input data for the KYCSYNC process.
        /// </summary>
        /// <param name="customerRecord">The customer record to validate.</param>
        /// <returns>True if the data is valid, otherwise false.</returns>
        bool ValidateInputData(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Synchronizes customer data with the external system.
        /// </summary>
        /// <param name="customerRecord">The customer record to synchronize.</param>
        void SynchronizeCustomerData(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Finalizes the KYCSYNC process.
        /// </summary>
        void FinalizeKycsync();
    }

    public interface ICustomerRepository
    {
        /// <summary>
        /// Retrieves a customer record by customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve.</param>
        /// <returns>The customer record, or null if not found.</returns>
        KycsyncDomainModels.CustomerRecord GetCustomerById(string customerId);

        /// <summary>
        /// Updates a customer record in the repository.
        /// </summary>
        /// <param name="customerRecord">The customer record to update.</param>
        void UpdateCustomer(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Retrieves all customer records.
        /// </summary>
        /// <returns>A list of all customer records.</returns>
        List<KycsyncDomainModels.CustomerRecord> GetAllCustomers();

        /// <summary>
        /// Deletes a customer record by customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to delete.</param>
        void DeleteCustomerById(string customerId);
    }

    public interface IAccountRepository
    {
        /// <summary>
        /// Retrieves all accounts for a given customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer whose accounts to retrieve.</param>
        /// <returns>A list of account records for the customer.</returns>
        List<KycsyncDomainModels.AccountRecord> GetAccountsByCustomerId(string customerId);

        /// <summary>
        /// Updates an account record in the repository.
        /// </summary>
        /// <param name="accountRecord">The account record to update.</param>
        void UpdateAccount(KycsyncDomainModels.AccountRecord accountRecord);

        /// <summary>
        /// Deletes an account record by account ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to delete.</param>
        void DeleteAccountById(string accountId);
    }

    public interface IExternalSystemService
    {
        /// <summary>
        /// Sends customer data to the external system for synchronization.
        /// </summary>
        /// <param name="customerRecord">The customer record to send.</param>
        void SendCustomerData(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Retrieves updated customer data from the external system.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve data for.</param>
        /// <returns>The updated customer record from the external system.</returns>
        KycsyncDomainModels.CustomerRecord RetrieveUpdatedCustomerData(string customerId);

        /// <summary>
        /// Sends account data to the external system for synchronization.
        /// </summary>
        /// <param name="accountRecord">The account record to send.</param>
        void SendAccountData(KycsyncDomainModels.AccountRecord accountRecord);

        /// <summary>
        /// Retrieves updated account data from the external system.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve data for.</param>
        /// <returns>The updated account record from the external system.</returns>
        KycsyncDomainModels.AccountRecord RetrieveUpdatedAccountData(string accountId);
    }
}