using System.Collections.Generic;
using KYCSYNC.Domain;

namespace KYCSYNC.Application.Interfaces
{
    /// <summary>
    /// Interface for repository operations related to file I/O in the KYCSYNC program.
    /// </summary>
    public interface IKycsyncRepository
    {
        /// <summary>
        /// Reads customer records from the input file.
        /// </summary>
        /// <returns>A list of customer records.</returns>
        List<KycsyncDomainModels.CustomerRecord> ReadCustomerRecords();

        /// <summary>
        /// Writes updated customer records to the output file.
        /// </summary>
        /// <param name="customerRecords">The list of customer records to write.</param>
        void WriteCustomerRecords(List<KycsyncDomainModels.CustomerRecord> customerRecords);
    }

    /// <summary>
    /// Interface for service operations related to CICS interactions in the KYCSYNC program.
    /// </summary>
    public interface IKycsyncService
    {
        /// <summary>
        /// Retrieves customer data from the CICS system.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve.</param>
        /// <returns>The customer record retrieved from the CICS system.</returns>
        KycsyncDomainModels.CustomerRecord RetrieveCustomerData(string customerId);

        /// <summary>
        /// Updates customer data in the CICS system.
        /// </summary>
        /// <param name="customerRecord">The customer record to update.</param>
        void UpdateCustomerData(KycsyncDomainModels.CustomerRecord customerRecord);
    }

    /// <summary>
    /// Interface for the main application logic of the KYCSYNC program.
    /// </summary>
    public interface IKycsyncProcessor
    {
        /// <summary>
        /// Processes customer records by synchronizing data between the file system and the CICS system.
        /// </summary>
        void ProcessCustomerRecords();
    }
}