using System;
using System.Collections.Generic;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    public interface IIntcalcProcessor
    {
        /// <summary>
        /// Initializes the calculation process.
        /// </summary>
        void InitializeCalculation();

        /// <summary>
        /// Validates the input data for the calculation request.
        /// </summary>
        /// <param name="request">The calculation request to validate.</param>
        void ValidateInput(IntcalcDomainModels.CalculationRequest request);

        /// <summary>
        /// Performs the interest calculation based on the request details.
        /// </summary>
        /// <param name="request">The calculation request containing all necessary details.</param>
        /// <returns>The calculated interest amount.</returns>
        decimal PerformCalculation(IntcalcDomainModels.CalculationRequest request);

        /// <summary>
        /// Applies adjustments to the calculated interest amount.
        /// </summary>
        /// <param name="calculatedAmount">The initially calculated interest amount.</param>
        /// <param name="adjustments">The list of adjustments to apply.</param>
        /// <returns>The adjusted interest amount.</returns>
        decimal ApplyAdjustments(decimal calculatedAmount, IEnumerable<IntcalcDomainModels.Adjustment> adjustments);

        /// <summary>
        /// Finalizes the calculation process and prepares the result.
        /// </summary>
        /// <param name="finalAmount">The final calculated and adjusted interest amount.</param>
        void FinalizeCalculation(decimal finalAmount);
    }

    public interface IIntcalcRepository
    {
        /// <summary>
        /// Reads the calculation request data from the file.
        /// </summary>
        /// <param name="requestId">The ID of the request to read.</param>
        /// <returns>The calculation request data.</returns>
        IntcalcDomainModels.CalculationRequest ReadCalculationRequest(string requestId);

        /// <summary>
        /// Writes the calculation result to the file.
        /// </summary>
        /// <param name="requestId">The ID of the request to write the result for.</param>
        /// <param name="result">The calculated result to write.</param>
        void WriteCalculationResult(string requestId, decimal result);
    }

    public interface IIntcalcCicsService
    {
        /// <summary>
        /// Retrieves external data required for the calculation.
        /// </summary>
        /// <param name="request">The calculation request for which external data is needed.</param>
        /// <returns>Additional data required for the calculation.</returns>
        IntcalcDomainModels.InterestDetails RetrieveExternalData(IntcalcDomainModels.CalculationRequest request);

        /// <summary>
        /// Logs the calculation process to the external system.
        /// </summary>
        /// <param name="requestId">The ID of the request being logged.</param>
        /// <param name="status">The status of the calculation process.</param>
        void LogCalculationProcess(string requestId, string status);
    }
}