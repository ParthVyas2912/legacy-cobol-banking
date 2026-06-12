using System;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    public interface IFraudCheckService
    {
        /// <summary>
        /// Initializes the fraud check process.
        /// </summary>
        void InitializeFraudCheck();

        /// <summary>
        /// Validates the input data for fraud check.
        /// </summary>
        /// <param name="request">The fraud check request data.</param>
        void ValidateInputData(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Performs the fraud check logic.
        /// </summary>
        /// <param name="request">The fraud check request data.</param>
        void PerformFraudCheck(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Finalizes the fraud check process and updates the status.
        /// </summary>
        /// <param name="request">The fraud check request data.</param>
        void FinalizeFraudCheck(FrdchkDomainModels.FraudCheckRequest request);
    }

    public interface IFraudCheckRepository
    {
        /// <summary>
        /// Reads fraud check request data from the data source.
        /// </summary>
        /// <param name="requestId">The unique identifier of the fraud check request.</param>
        /// <returns>The fraud check request data.</returns>
        FrdchkDomainModels.FraudCheckRequest ReadFraudCheckRequest(string requestId);

        /// <summary>
        /// Writes fraud check request data to the data source.
        /// </summary>
        /// <param name="request">The fraud check request data.</param>
        void WriteFraudCheckRequest(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Updates the fraud check status in the data source.
        /// </summary>
        /// <param name="requestId">The unique identifier of the fraud check request.</param>
        /// <param name="status">The updated fraud check status.</param>
        void UpdateFraudCheckStatus(string requestId, FrdchkDomainModels.FraudCheckStatus status);

        /// <summary>
        /// Deletes a fraud check request from the data source.
        /// </summary>
        /// <param name="requestId">The unique identifier of the fraud check request.</param>
        void DeleteFraudCheckRequest(string requestId);
    }

    public interface ICicsService
    {
        /// <summary>
        /// Executes a CICS transaction for fraud check.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the CICS transaction.</param>
        /// <param name="request">The fraud check request data.</param>
        /// <returns>The result of the CICS transaction.</returns>
        string ExecuteCicsTransaction(string transactionId, FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Retrieves the status of a CICS transaction.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the CICS transaction.</param>
        /// <returns>The status of the CICS transaction.</returns>
        string GetCicsTransactionStatus(string transactionId);
    }
}