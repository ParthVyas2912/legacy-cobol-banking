using System;
using System.Collections.Generic;
using KYCSYNC.Domain;

namespace KYCSYNC.Application.Interfaces
{
    /// <summary>
    /// Interface for handling KYC synchronization operations.
    /// Each method corresponds to a COBOL paragraph or operation.
    /// </summary>
    public interface IKycsyncService
    {
        /// <summary>
        /// Initializes the KYC synchronization process.
        /// </summary>
        void InitializeKycsync();

        /// <summary>
        /// Reads customer data from the input file.
        /// </summary>
        /// <returns>A list of customers read from the file.</returns>
        List<KycsyncDomainModels.Customer> ReadCustomerData();

        /// <summary>
        /// Validates the customer data for KYC compliance.
        /// </summary>
        /// <param name="customers">The list of customers to validate.</param>
        /// <returns>A list of validation errors, if any.</returns>
        List<string> ValidateCustomerData(List<KycsyncDomainModels.Customer> customers);

        /// <summary>
        /// Updates the customer records in the database.
        /// </summary>
        /// <param name="customers">The list of customers to update.</param>
        void UpdateCustomerRecords(List<KycsyncDomainModels.Customer> customers);

        /// <summary>
        /// Generates a report of the KYC synchronization process.
        /// </summary>
        /// <param name="customers">The list of customers processed.</param>
        /// <returns>A string representing the report.</returns>
        string GenerateKycsyncReport(List<KycsyncDomainModels.Customer> customers);

        /// <summary>
        /// Finalizes the KYC synchronization process.
        /// </summary>
        void FinalizeKycsync();
    }

    /// <summary>
    /// Interface for handling file I/O operations related to KYC synchronization.
    /// </summary>
    public interface IKycsyncFileRepository
    {
        /// <summary>
        /// Reads customer data from the input file.
        /// </summary>
        /// <param name="filePath">The path of the input file.</param>
        /// <returns>A list of customers read from the file.</returns>
        List<KycsyncDomainModels.Customer> ReadFromFile(string filePath);

        /// <summary>
        /// Writes the KYC synchronization report to an output file.
        /// </summary>
        /// <param name="filePath">The path of the output file.</param>
        /// <param name="reportContent">The content of the report to write.</param>
        void WriteToFile(string filePath, string reportContent);
    }

    /// <summary>
    /// Interface for handling CICS operations related to KYC synchronization.
    /// </summary>
    public interface IKycsyncCicsService
    {
        /// <summary>
        /// Retrieves customer data from the CICS system.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve.</param>
        /// <returns>The customer data retrieved from the CICS system.</returns>
        KycsyncDomainModels.Customer RetrieveCustomerData(string customerId);

        /// <summary>
        /// Updates customer data in the CICS system.
        /// </summary>
        /// <param name="customer">The customer data to update.</param>
        void UpdateCustomerData(KycsyncDomainModels.Customer customer);
    }
}