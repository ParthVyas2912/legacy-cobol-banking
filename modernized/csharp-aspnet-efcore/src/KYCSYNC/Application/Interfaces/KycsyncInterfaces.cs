using System.Collections.Generic;
using KYCSYNC.Domain;

namespace KYCSYNC.Application.Interfaces
{
    public interface IKycsyncRepository
    {
        /// <summary>
        /// Reads customer data from the file.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve.</param>
        /// <returns>The customer data.</returns>
        KycsyncDomainModels.Customer ReadCustomer(string customerId);

        /// <summary>
        /// Writes customer data to the file.
        /// </summary>
        /// <param name="customer">The customer data to write.</param>
        void WriteCustomer(KycsyncDomainModels.Customer customer);

        /// <summary>
        /// Retrieves all customers from the file.
        /// </summary>
        /// <returns>A list of all customers.</returns>
        List<KycsyncDomainModels.Customer> GetAllCustomers();

        /// <summary>
        /// Deletes customer data from the file.
        /// </summary>
        /// <param name="customerId">The ID of the customer to delete.</param>
        void DeleteCustomer(string customerId);

        /// <summary>
        /// Updates customer data in the file.
        /// </summary>
        /// <param name="customer">The updated customer data.</param>
        void UpdateCustomer(KycsyncDomainModels.Customer customer);
    }

    public interface IKycsyncService
    {
        /// <summary>
        /// Synchronizes customer data with the external system.
        /// </summary>
        /// <param name="customer">The customer data to synchronize.</param>
        void SynchronizeCustomer(KycsyncDomainModels.Customer customer);

        /// <summary>
        /// Retrieves the status of the synchronization process.
        /// </summary>
        /// <returns>The status of the synchronization.</returns>
        string GetSynchronizationStatus();

        /// <summary>
        /// Validates the customer data before synchronization.
        /// </summary>
        /// <param name="customer">The customer data to validate.</param>
        /// <returns>True if the data is valid, otherwise false.</returns>
        bool ValidateCustomerData(KycsyncDomainModels.Customer customer);

        /// <summary>
        /// Logs synchronization errors for a customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="errorMessage">The error message to log.</param>
        void LogSynchronizationError(string customerId, string errorMessage);
    }

    public interface IKycsyncApplication
    {
        /// <summary>
        /// Initializes the KYCSYNC application.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Processes the customer synchronization workflow.
        /// </summary>
        void ProcessCustomerSync();

        /// <summary>
        /// Finalizes the KYCSYNC application.
        /// </summary>
        void Finalize();

        /// <summary>
        /// Generates a report of the synchronization process.
        /// </summary>
        /// <returns>The report as a string.</returns>
        string GenerateSyncReport();

        /// <summary>
        /// Handles errors encountered during the synchronization process.
        /// </summary>
        /// <param name="errorMessage">The error message to handle.</param>
        void HandleSyncError(string errorMessage);
    }
}