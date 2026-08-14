using System;
using System.Collections.Generic;
using KYCSYNC.Domain;

namespace KYCSYNC.Application.Interfaces
{
    /// <summary>
    /// Interface for repository operations related to Customer Records.
    /// </summary>
    public interface ICustomerRecordRepository
    {
        /// <summary>
        /// Reads a customer record by Customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve.</param>
        /// <returns>The customer record if found; otherwise, null.</returns>
        KycsyncDomainModels.CustomerRecord ReadCustomerRecord(string customerId);

        /// <summary>
        /// Writes a customer record to the repository.
        /// </summary>
        /// <param name="customerRecord">The customer record to write.</param>
        void WriteCustomerRecord(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Deletes a customer record by Customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to delete.</param>
        void DeleteCustomerRecord(string customerId);
    }

    /// <summary>
    /// Interface for CICS service operations related to KYCSYNC.
    /// </summary>
    public interface IKycsyncCicsService
    {
        /// <summary>
        /// Synchronizes customer data with an external system.
        /// </summary>
        /// <param name="customerRecord">The customer record to synchronize.</param>
        /// <returns>A boolean indicating whether the synchronization was successful.</returns>
        bool SynchronizeCustomerData(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Retrieves the status of a customer from an external system.
        /// </summary>
        /// <param name="customerId">The ID of the customer to check.</param>
        /// <returns>The status of the customer.</returns>
        KycsyncDomainModels.CustomerRecord.StatusEnum GetCustomerStatus(string customerId);
    }

    /// <summary>
    /// Interface for application-level operations related to KYCSYNC.
    /// </summary>
    public interface IKycsyncApplicationService
    {
        /// <summary>
        /// Processes a batch of customer records for synchronization.
        /// </summary>
        /// <param name="customerRecords">The list of customer records to process.</param>
        void ProcessBatchSynchronization(List<KycsyncDomainModels.CustomerRecord> customerRecords);

        /// <summary>
        /// Validates a customer record before processing.
        /// </summary>
        /// <param name="customerRecord">The customer record to validate.</param>
        /// <returns>A boolean indicating whether the record is valid.</returns>
        bool ValidateCustomerRecord(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Logs the result of a synchronization operation.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="success">Indicates whether the operation was successful.</param>
        /// <param name="message">Additional details about the operation.</param>
        void LogSynchronizationResult(string customerId, bool success, string message);
    }
}