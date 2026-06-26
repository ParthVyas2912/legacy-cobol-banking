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
        /// Validates the input data for KYCSYNC.
        /// </summary>
        /// <param name="customerRecord">The customer record to validate.</param>
        /// <returns>True if validation succeeds, otherwise false.</returns>
        bool ValidateInputData(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Synchronizes customer data with the KYC system.
        /// </summary>
        /// <param name="customerRecord">The customer record to synchronize.</param>
        void SynchronizeCustomerData(KycsyncDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Updates the KYC status for a customer.
        /// </summary>
        /// <param name="kycStatus">The KYC status to update.</param>
        void UpdateKycStatus(KycsyncDomainModels.KycStatus kycStatus);

        /// <summary>
        /// Generates a report of the KYCSYNC process.
        /// </summary>
        /// <returns>A string containing the report details.</returns>
        string GenerateReport();
    }

    public interface IKycsyncRepository
    {
        /// <summary>
        /// Reads customer records from the data source.
        /// </summary>
        /// <returns>A list of customer records.</returns>
        List<KycsyncDomainModels.CustomerRecord> ReadCustomerRecords();

        /// <summary>
        /// Writes updated customer records to the data source.
        /// </summary>
        /// <param name="customerRecords">The customer records to write.</param>
        void WriteCustomerRecords(List<KycsyncDomainModels.CustomerRecord> customerRecords);

        /// <summary>
        /// Reads transaction records from the data source.
        /// </summary>
        /// <returns>A list of transaction records.</returns>
        List<KycsyncDomainModels.TransactionRecord> ReadTransactionRecords();

        /// <summary>
        /// Writes updated transaction records to the data source.
        /// </summary>
        /// <param name="transactionRecords">The transaction records to write.</param>
        void WriteTransactionRecords(List<KycsyncDomainModels.TransactionRecord> transactionRecords);

        /// <summary>
        /// Reads KYC status records from the data source.
        /// </summary>
        /// <returns>A list of KYC status records.</returns>
        List<KycsyncDomainModels.KycStatus> ReadKycStatusRecords();

        /// <summary>
        /// Writes updated KYC status records to the data source.
        /// </summary>
        /// <param name="kycStatusRecords">The KYC status records to write.</param>
        void WriteKycStatusRecords(List<KycsyncDomainModels.KycStatus> kycStatusRecords);
    }

    public interface ICicsService
    {
        /// <summary>
        /// Sends a request to the external KYC system.
        /// </summary>
        /// <param name="requestData">The request data to send.</param>
        /// <returns>The response data from the external system.</returns>
        string SendKycRequest(string requestData);

        /// <summary>
        /// Retrieves the response from the external KYC system.
        /// </summary>
        /// <param name="requestId">The ID of the request to retrieve the response for.</param>
        /// <returns>The response data from the external system.</returns>
        string RetrieveKycResponse(string requestId);
    }
}