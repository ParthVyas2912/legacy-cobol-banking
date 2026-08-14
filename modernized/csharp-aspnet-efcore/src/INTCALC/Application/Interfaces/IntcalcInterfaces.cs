using System.Collections.Generic;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    /// <summary>
    /// Interface for the main application logic of INTCALC.
    /// Each COBOL paragraph is represented as a method.
    /// </summary>
    public interface IIntcalcApplicationService
    {
        /// <summary>
        /// Initializes the calculation process.
        /// </summary>
        void InitializeCalculation();

        /// <summary>
        /// Validates the input data for the calculation.
        /// </summary>
        /// <param name="request">The calculation request to validate.</param>
        void ValidateInput(IntcalcDomainModels.CalculationRequest request);

        /// <summary>
        /// Performs the main interest calculation logic.
        /// </summary>
        /// <param name="request">The calculation request containing input data.</param>
        /// <returns>A list of calculation details as the result.</returns>
        List<IntcalcDomainModels.CalculationDetail> PerformCalculation(IntcalcDomainModels.CalculationRequest request);

        /// <summary>
        /// Finalizes the calculation process and prepares the output.
        /// </summary>
        /// <param name="request">The calculation request containing input data.</param>
        void FinalizeCalculation(IntcalcDomainModels.CalculationRequest request);
    }

    /// <summary>
    /// Interface for file I/O operations related to INTCALC.
    /// </summary>
    public interface IIntcalcFileRepository
    {
        /// <summary>
        /// Reads the calculation request data from a file.
        /// </summary>
        /// <param name="filePath">The path of the file to read.</param>
        /// <returns>The calculation request read from the file.</returns>
        IntcalcDomainModels.CalculationRequest ReadCalculationRequest(string filePath);

        /// <summary>
        /// Writes the calculation results to a file.
        /// </summary>
        /// <param name="filePath">The path of the file to write to.</param>
        /// <param name="details">The calculation details to write.</param>
        void WriteCalculationResults(string filePath, List<IntcalcDomainModels.CalculationDetail> details);
    }

    /// <summary>
    /// Interface for CICS operations related to INTCALC.
    /// </summary>
    public interface IIntcalcCicsService
    {
        /// <summary>
        /// Retrieves customer data from a CICS system.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>The customer data as a string.</returns>
        string RetrieveCustomerData(string customerId);

        /// <summary>
        /// Updates the calculation status in a CICS system.
        /// </summary>
        /// <param name="requestId">The unique identifier for the calculation request.</param>
        /// <param name="status">The status to update.</param>
        void UpdateCalculationStatus(string requestId, string status);
    }
}