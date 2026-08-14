using System;
using System.Collections.Generic;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    /// <summary>
    /// Interface for the Fraud Check Application Service
    /// </summary>
    public interface IFrdchkService
    {
        /// <summary>
        /// Initializes the fraud check process.
        /// </summary>
        void InitializeFraudCheck();

        /// <summary>
        /// Processes a fraud check request.
        /// </summary>
        /// <param name="request">The fraud check request data.</param>
        /// <returns>A boolean indicating whether the process was successful.</returns>
        bool ProcessFraudCheck(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Finalizes the fraud check process.
        /// </summary>
        void FinalizeFraudCheck();
    }

    /// <summary>
    /// Interface for the Fraud Check Repository
    /// </summary>
    public interface IFrdchkRepository
    {
        /// <summary>
        /// Reads fraud check data from the file system.
        /// </summary>
        /// <param name="requestId">The request ID to locate the data.</param>
        /// <returns>The fraud check request data.</returns>
        FrdchkDomainModels.FraudCheckRequest ReadFraudCheckData(string requestId);

        /// <summary>
        /// Writes fraud check data to the file system.
        /// </summary>
        /// <param name="request">The fraud check request data to write.</param>
        void WriteFraudCheckData(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Deletes fraud check data from the file system.
        /// </summary>
        /// <param name="requestId">The request ID to delete the data for.</param>
        void DeleteFraudCheckData(string requestId);
    }

    /// <summary>
    /// Interface for CICS operations related to Fraud Check
    /// </summary>
    public interface IFrdchkCicsService
    {
        /// <summary>
        /// Sends a fraud check request to the CICS system.
        /// </summary>
        /// <param name="request">The fraud check request data.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        string SendFraudCheckRequest(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Retrieves the status of a fraud check from the CICS system.
        /// </summary>
        /// <param name="requestId">The request ID to check the status for.</param>
        /// <returns>The status of the fraud check.</returns>
        FrdchkDomainModels.TransactionStatus GetFraudCheckStatus(string requestId);
    }
}