using System;
using System.Collections.Generic;
using KYCSYNC.Domain;

namespace KYCSYNC.Application.Interfaces
{
    public interface IKycsyncService
    {
        /// <summary>
        /// Initializes the KYC synchronization process.
        /// </summary>
        void InitializeKycsync();

        /// <summary>
        /// Validates the input data for KYC synchronization.
        /// </summary>
        /// <param name="request">The KYC synchronization request containing customer and transaction details.</param>
        /// <returns>A boolean indicating whether the input data is valid.</returns>
        bool ValidateInputData(KycsyncDomainModels.KycSyncRequest request);

        /// <summary>
        /// Processes the KYC synchronization for the given request.
        /// </summary>
        /// <param name="request">The KYC synchronization request containing customer and transaction details.</param>
        void ProcessKycsync(KycsyncDomainModels.KycSyncRequest request);

        /// <summary>
        /// Finalizes the KYC synchronization process.
        /// </summary>
        void FinalizeKycsync();
    }

    public interface ICustomerRepository
    {
        /// <summary>
        /// Retrieves customer details by customer ID.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>The customer details.</returns>
        KycsyncDomainModels.Customer GetCustomerById(string customerId);

        /// <summary>
        /// Updates the customer details in the repository.
        /// </summary>
        /// <param name="customer">The customer details to update.</param>
        void UpdateCustomer(KycsyncDomainModels.Customer customer);

        /// <summary>
        /// Adds a new customer to the repository.
        /// </summary>
        /// <param name="customer">The customer details to add.</param>
        void AddCustomer(KycsyncDomainModels.Customer customer);

        /// <summary>
        /// Deletes a customer from the repository by customer ID.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer to delete.</param>
        void DeleteCustomer(string customerId);
    }

    public interface IKycStatusRepository
    {
        /// <summary>
        /// Retrieves the KYC status for a given customer ID.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>The KYC status details.</returns>
        KycsyncDomainModels.KycStatus GetKycStatusByCustomerId(string customerId);

        /// <summary>
        /// Updates the KYC status in the repository.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <param name="kycStatus">The KYC status to update.</param>
        void UpdateKycStatus(string customerId, KycsyncDomainModels.KycStatus kycStatus);

        /// <summary>
        /// Deletes the KYC status for a given customer ID.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        void DeleteKycStatus(string customerId);
    }

    public interface ITransactionRepository
    {
        /// <summary>
        /// Retrieves transactions for a given customer ID.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>A list of transactions associated with the customer.</returns>
        IEnumerable<KycsyncDomainModels.Transaction> GetTransactionsByCustomerId(string customerId);

        /// <summary>
        /// Adds a new transaction to the repository.
        /// </summary>
        /// <param name="transaction">The transaction details to add.</param>
        void AddTransaction(KycsyncDomainModels.Transaction transaction);

        /// <summary>
        /// Deletes a transaction from the repository by transaction ID.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction to delete.</param>
        void DeleteTransaction(string transactionId);

        /// <summary>
        /// Updates a transaction in the repository.
        /// </summary>
        /// <param name="transaction">The transaction details to update.</param>
        void UpdateTransaction(KycsyncDomainModels.Transaction transaction);
    }

    public interface ICicsService
    {
        /// <summary>
        /// Sends a request to the CICS system for KYC verification.
        /// </summary>
        /// <param name="request">The KYC synchronization request containing customer and transaction details.</param>
        /// <returns>A boolean indicating whether the request was successfully processed by the CICS system.</returns>
        bool SendKycVerificationRequest(KycsyncDomainModels.KycSyncRequest request);

        /// <summary>
        /// Retrieves the response from the CICS system for a given customer ID.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>The KYC status details as received from the CICS system.</returns>
        KycsyncDomainModels.KycStatus GetKycVerificationResponse(string customerId);

        /// <summary>
        /// Cancels a pending KYC verification request in the CICS system.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer whose request should be canceled.</param>
        /// <returns>A boolean indicating whether the cancellation was successful.</returns>
        bool CancelKycVerificationRequest(string customerId);
    }
}