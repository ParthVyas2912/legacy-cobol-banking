using System;
using System.Collections.Generic;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    /// <summary>
    /// Interface for the main Fraud Check application logic.
    /// Each COBOL paragraph is represented as a method.
    /// </summary>
    public interface IFrdchkApplicationService
    {
        /// <summary>
        /// Initializes the fraud check process.
        /// </summary>
        void InitializeFraudCheck();

        /// <summary>
        /// Validates the input request for fraud check.
        /// </summary>
        /// <param name="request">The fraud check request object.</param>
        void ValidateFraudCheckRequest(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Executes the fraud detection logic.
        /// </summary>
        /// <param name="request">The fraud check request object.</param>
        /// <returns>A boolean indicating whether fraud is detected.</returns>
        bool ExecuteFraudDetection(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Finalizes the fraud check process and prepares the output.
        /// </summary>
        /// <param name="request">The fraud check request object.</param>
        /// <returns>A string containing the result of the fraud check.</returns>
        string FinalizeFraudCheck(FrdchkDomainModels.FraudCheckRequest request);

        /// <summary>
        /// Logs fraud detection details.
        /// </summary>
        /// <param name="details">The fraud check details object.</param>
        void LogFraudCheckDetails(FrdchkDomainModels.FraudCheckDetails details);
    }

    /// <summary>
    /// Interface for repository operations related to fraud check data.
    /// File I/O operations in COBOL are represented here.
    /// </summary>
    public interface IFrdchkRepository
    {
        /// <summary>
        /// Reads fraud check configuration data from a file.
        /// </summary>
        /// <returns>A dictionary containing configuration key-value pairs.</returns>
        Dictionary<string, string> ReadFraudCheckConfiguration();

        /// <summary>
        /// Writes the fraud check results to a file.
        /// </summary>
        /// <param name="result">The result of the fraud check.</param>
        void WriteFraudCheckResult(string result);

        /// <summary>
        /// Reads fraud indicators from a file.
        /// </summary>
        /// <returns>A list of fraud indicators.</returns>
        List<FrdchkDomainModels.FraudIndicator> ReadFraudIndicators();

        /// <summary>
        /// Writes fraud check details to a file.
        /// </summary>
        /// <param name="details">The fraud check details object.</param>
        void WriteFraudCheckDetails(FrdchkDomainModels.FraudCheckDetails details);
    }

    /// <summary>
    /// Interface for CICS service operations related to fraud check.
    /// CICS operations in COBOL are represented here.
    /// </summary>
    public interface IFrdchkCicsService
    {
        /// <summary>
        /// Retrieves customer data from the CICS system.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <returns>A fraud check request object populated with customer data.</returns>
        FrdchkDomainModels.FraudCheckRequest RetrieveCustomerData(string customerId);

        /// <summary>
        /// Sends fraud detection alerts to the CICS system.
        /// </summary>
        /// <param name="alertMessage">The alert message to be sent.</param>
        void SendFraudAlert(string alertMessage);

        /// <summary>
        /// Updates fraud detection status in the CICS system.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <param name="status">The fraud detection status.</param>
        void UpdateFraudStatus(string customerId, string status);
    }
}