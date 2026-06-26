using System;
using System.Collections.Generic;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    /// <summary>
    /// Interface for the Fraud Check application logic.
    /// Each method corresponds to a paragraph in the original COBOL program.
    /// </summary>
    public interface IFrdchkService
    {
        /// <summary>
        /// Initializes the fraud check process.
        /// </summary>
        void InitializeFraudCheck();

        /// <summary>
        /// Validates the input data for the fraud check.
        /// </summary>
        /// <param name="request">The fraud check request data.</param>
        void ValidateInputData(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Performs the fraud check logic.
        /// </summary>
        /// <param name="request">The fraud check request data.</param>
        void PerformFraudCheck(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Generates the fraud check report.
        /// </summary>
        /// <param name="request">The fraud check request data.</param>
        /// <returns>A string containing the fraud check report.</returns>
        string GenerateFraudCheckReport(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Finalizes the fraud check process.
        /// </summary>
        void FinalizeFraudCheck();
    }

    /// <summary>
    /// Interface for repository operations related to fraud check file I/O.
    /// </summary>
    public interface IFrdchkRepository
    {
        /// <summary>
        /// Reads fraud check data from a file.
        /// </summary>
        /// <param name="filePath">The path to the input file.</param>
        /// <returns>A list of fraud check requests.</returns>
        List<FrdchkDomainModels.FraudCheckRequest> ReadFraudCheckData(string filePath);

        /// <summary>
        /// Writes fraud check results to a file.
        /// </summary>
        /// <param name="filePath">The path to the output file.</param>
        /// <param name="results">The fraud check results to write.</param>
        void WriteFraudCheckResults(string filePath, List<string> results);
    }

    /// <summary>
    /// Interface for CICS operations related to fraud check.
    /// </summary>
    public interface IFrdchkCicsService
    {
        /// <summary>
        /// Sends a fraud check request to the CICS system.
        /// </summary>
        /// <param name="request">The fraud check request data.</param>
        /// <returns>A response from the CICS system.</returns>
        string SendFraudCheckRequestToCics(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Retrieves fraud check results from the CICS system.
        /// </summary>
        /// <param name="requestId">The ID of the fraud check request.</param>
        /// <returns>The fraud check results.</returns>
        string RetrieveFraudCheckResultsFromCics(string requestId);
    }
}