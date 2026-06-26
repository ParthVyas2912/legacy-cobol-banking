using System;
using System.Collections.Generic;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    /// <summary>
    /// Interface for repository operations related to fraud check file I/O.
    /// </summary>
    public interface IFraudCheckRepository
    {
        /// <summary>
        /// Reads a fraud check request from the input file.
        /// </summary>
        /// <returns>A <see cref="FrdchkDomainModels.FraudCheckRequest"/> object.</returns>
        FrdchkDomainModels.FraudCheckRequest ReadFraudCheckRequest();

        /// <summary>
        /// Writes a fraud check response to the output file.
        /// </summary>
        /// <param name="response">The <see cref="FrdchkDomainModels.FraudCheckResponse"/> to write.</param>
        void WriteFraudCheckResponse(FrdchkDomainModels.FraudCheckResponse response);
    }

    /// <summary>
    /// Interface for CICS service operations related to fraud check processing.
    /// </summary>
    public interface IFraudCheckService
    {
        /// <summary>
        /// Performs a fraud check based on the provided request.
        /// </summary>
        /// <param name="request">The <see cref="FrdchkDomainModels.FraudCheckRequest"/> containing the details of the transaction to check.</param>
        /// <returns>A <see cref="FrdchkDomainModels.FraudCheckResponse"/> containing the result of the fraud check.</returns>
        FrdchkDomainModels.FraudCheckResponse PerformFraudCheck(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Logs a fraud check event to the CICS system.
        /// </summary>
        /// <param name="requestId">The ID of the fraud check request.</param>
        /// <param name="result">The result of the fraud check.</param>
        void LogFraudCheckEvent(string requestId, FrdchkDomainModels.FraudCheckResponse.FraudCheckResultEnum result);
    }

    /// <summary>
    /// Interface for application-level fraud check orchestration.
    /// </summary>
    public interface IFraudCheckProcessor
    {
        /// <summary>
        /// Executes the fraud check process, including reading input, performing the check, and writing output.
        /// </summary>
        void ExecuteFraudCheckProcess();
    }
}