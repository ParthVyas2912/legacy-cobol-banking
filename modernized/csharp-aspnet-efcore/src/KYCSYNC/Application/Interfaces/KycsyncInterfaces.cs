using System;
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
        /// Validates the input data for KYCSYNC processing.
        /// </summary>
        /// <param name="customerRecord">The customer record to validate.</param>
        /// <returns>True if validation is successful, otherwise false.</returns>
        bool ValidateInputData(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Processes the KYCSYNC logic for a given customer record.
        /// </summary>
        /// <param name="customerRecord">The customer record to process.</param>
        void ProcessCustomerRecord(KycsyncDomainModels.CustomerRecord customerRecord);

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
        /// <param name="customerId">The customer ID.</param>
        /// <returns>The customer record.</returns>
        KycsyncDomainModels.CustomerRecord GetCustomerById(string customerId);

        /// <summary>
        /// Saves a customer record to the repository.
        /// </summary>
        /// <param name="customerRecord">The customer record to save.</param>
        void SaveCustomer(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Updates a customer record in the repository.
        /// </summary>
        /// <param name="customerRecord">The customer record to update.</param>
        void UpdateCustomer(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Deletes a customer record from the repository.
        /// </summary>
        /// <param name="customerId">The customer ID of the record to delete.</param>
        void DeleteCustomer(string customerId);
    }

    public interface ITransactionRepository
    {
        /// <summary>
        /// Retrieves transactions for a given customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <returns>A list of transactions.</returns>
        List<KycsyncDomainModels.CustomerRecord.Transaction> GetTransactionsByCustomerId(string customerId);

        /// <summary>
        /// Saves a transaction to the repository.
        /// </summary>
        /// <param name="transaction">The transaction to save.</param>
        void SaveTransaction(KycsyncDomainModels.CustomerRecord.Transaction transaction);

        /// <summary>
        /// Updates a transaction in the repository.
        /// </summary>
        /// <param name="transaction">The transaction to update.</param>
        void UpdateTransaction(KycsyncDomainModels.CustomerRecord.Transaction transaction);

        /// <summary>
        /// Deletes a transaction from the repository.
        /// </summary>
        /// <param name="transactionId">The transaction ID of the record to delete.</param>
        void DeleteTransaction(string transactionId);
    }

    public interface ICicsService
    {
        /// <summary>
        /// Sends a request to the CICS system.
        /// </summary>
        /// <param name="requestData">The request data to send.</param>
        /// <returns>The response data from the CICS system.</returns>
        string SendCicsRequest(string requestData);

        /// <summary>
        /// Retrieves data from the CICS system.
        /// </summary>
        /// <param name="key">The key to retrieve data for.</param>
        /// <returns>The data retrieved from the CICS system.</returns>
        string RetrieveCicsData(string key);
    }
}