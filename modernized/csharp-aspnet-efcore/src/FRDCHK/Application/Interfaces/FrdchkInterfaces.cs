using System;
using System.Collections.Generic;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    /// <summary>
    /// Interface for handling fraud check operations.
    /// Each COBOL paragraph is represented as a method.
    /// </summary>
    public interface IFrdchkService
    {
        /// <summary>
        /// Initializes the fraud check process.
        /// </summary>
        void InitializeFraudCheck();

        /// <summary>
        /// Validates the input request for fraud check.
        /// </summary>
        /// <param name="request">The fraud check request object.</param>
        void ValidateRequest(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Processes the fraud check logic.
        /// </summary>
        /// <param name="request">The fraud check request object.</param>
        /// <returns>A boolean indicating whether fraud was detected.</returns>
        bool ProcessFraudCheck(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Finalizes the fraud check process and generates a report.
        /// </summary>
        /// <param name="request">The fraud check request object.</param>
        void FinalizeFraudCheck(FrdchkDomainModels.FraudCheckRequest request);
    }

    /// <summary>
    /// Interface for handling file I/O operations related to fraud check.
    /// </summary>
    public interface IFrdchkRepository
    {
        /// <summary>
        /// Reads fraud check data from a file.
        /// </summary>
        /// <param name="filePath">The file path to read from.</param>
        /// <returns>A list of fraud check requests.</returns>
        List<FrdchkDomainModels.FraudCheckRequest> ReadFraudCheckData(string filePath);

        /// <summary>
        /// Writes fraud check results to a file.
        /// </summary>
        /// <param name="filePath">The file path to write to.</param>
        /// <param name="results">The fraud check results to write.</param>
        void WriteFraudCheckResults(string filePath, List<string> results);
    }

    /// <summary>
    /// Interface for handling CICS operations related to fraud check.
    /// </summary>
    public interface IFrdchkCicsService
    {
        /// <summary>
        /// Sends a fraud check request to the CICS system.
        /// </summary>
        /// <param name="request">The fraud check request object.</param>
        /// <returns>A response from the CICS system.</returns>
        string SendFraudCheckRequest(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Retrieves fraud check results from the CICS system.
        /// </summary>
        /// <param name="requestId">The request ID for which to retrieve results.</param>
        /// <returns>The fraud check results.</returns>
        string RetrieveFraudCheckResults(string requestId);
    }
}