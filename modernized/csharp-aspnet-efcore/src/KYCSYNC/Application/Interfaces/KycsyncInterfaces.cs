using System.Collections.Generic;
using KYCSYNC.Domain;

namespace KYCSYNC.Application.Interfaces
{
    public interface IKycsyncRepository
    {
        /// <summary>
        /// Reads a customer record by customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID to search for.</param>
        /// <returns>The customer record if found, otherwise null.</returns>
        KycsyncDomainModels.CustomerRecord ReadCustomerRecord(string customerId);

        /// <summary>
        /// Writes a customer record to the repository.
        /// </summary>
        /// <param name="customerRecord">The customer record to write.</param>
        void WriteCustomerRecord(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Reads a transaction record by transaction ID.
        /// </summary>
        /// <param name="transactionId">The transaction ID to search for.</param>
        /// <returns>The transaction record if found, otherwise null.</returns>
        KycsyncDomainModels.TransactionRecord ReadTransactionRecord(string transactionId);

        /// <summary>
        /// Writes a transaction record to the repository.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to write.</param>
        void WriteTransactionRecord(KycsyncDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Retrieves all transactions for a given customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID to retrieve transactions for.</param>
        /// <returns>A list of transaction records associated with the customer.</returns>
        List<KycsyncDomainModels.TransactionRecord> GetTransactionsByCustomerId(string customerId);

        /// <summary>
        /// Deletes a customer record by customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID of the record to delete.</param>
        void DeleteCustomerRecord(string customerId);

        /// <summary>
        /// Deletes a transaction record by transaction ID.
        /// </summary>
        /// <param name="transactionId">The transaction ID of the record to delete.</param>
        void DeleteTransactionRecord(string transactionId);
    }

    public interface IKycsyncService
    {
        /// <summary>
        /// Synchronizes customer data with an external system.
        /// </summary>
        /// <param name="customerRecord">The customer record to synchronize.</param>
        void SynchronizeCustomerData(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Synchronizes transaction data with an external system.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to synchronize.</param>
        void SynchronizeTransactionData(KycsyncDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Validates the integrity of customer data.
        /// </summary>
        /// <param name="customerRecord">The customer record to validate.</param>
        /// <returns>True if the data is valid, otherwise false.</returns>
        bool ValidateCustomerData(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Validates the integrity of transaction data.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to validate.</param>
        /// <returns>True if the data is valid, otherwise false.</returns>
        bool ValidateTransactionData(KycsyncDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Processes a batch of customer records for synchronization.
        /// </summary>
        /// <param name="customerRecords">The list of customer records to process.</param>
        void ProcessCustomerBatch(List<KycsyncDomainModels.CustomerRecord> customerRecords);

        /// <summary>
        /// Processes a batch of transaction records for synchronization.
        /// </summary>
        /// <param name="transactionRecords">The list of transaction records to process.</param>
        void ProcessTransactionBatch(List<KycsyncDomainModels.TransactionRecord> transactionRecords);
    }
}