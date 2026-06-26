using System.Collections.Generic;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    /// <summary>
    /// Interface for the main application logic of the INTCALC program.
    /// Each method corresponds to a paragraph in the original COBOL program.
    /// </summary>
    public interface IIntcalcApplicationService
    {
        /// <summary>
        /// Initializes the application and sets up necessary configurations.
        /// </summary>
        void InitializeApplication();

        /// <summary>
        /// Validates the input data for the interest calculation process.
        /// </summary>
        /// <param name="request">The calculation request containing input data.</param>
        void ValidateInputData(IntcalcDomainModels.CalculationRequest request);

        /// <summary>
        /// Calculates the interest and generates the payment schedule.
        /// </summary>
        /// <param name="request">The calculation request containing input data.</param>
        /// <returns>A list of payment schedules.</returns>
        List<IntcalcDomainModels.CalculationRequest.PaymentSchedule> CalculateInterest(IntcalcDomainModels.CalculationRequest request);

        /// <summary>
        /// Formats the output data for presentation or storage.
        /// </summary>
        /// <param name="request">The calculation request containing input data.</param>
        /// <param name="paymentSchedules">The list of calculated payment schedules.</param>
        void FormatOutputData(IntcalcDomainModels.CalculationRequest request, List<IntcalcDomainModels.CalculationRequest.PaymentSchedule> paymentSchedules);

        /// <summary>
        /// Finalizes the application process and performs cleanup operations.
        /// </summary>
        void FinalizeApplication();
    }

    /// <summary>
    /// Interface for handling file I/O operations related to the INTCALC program.
    /// </summary>
    public interface IIntcalcFileRepository
    {
        /// <summary>
        /// Reads input data from a file.
        /// </summary>
        /// <returns>The calculation request containing input data.</returns>
        IntcalcDomainModels.CalculationRequest ReadInputData();

        /// <summary>
        /// Writes the output data to a file.
        /// </summary>
        /// <param name="request">The calculation request containing input data.</param>
        /// <param name="paymentSchedules">The list of calculated payment schedules.</param>
        void WriteOutputData(IntcalcDomainModels.CalculationRequest request, List<IntcalcDomainModels.CalculationRequest.PaymentSchedule> paymentSchedules);
    }

    /// <summary>
    /// Interface for handling CICS operations related to the INTCALC program.
    /// </summary>
    public interface IIntcalcCicsService
    {
        /// <summary>
        /// Retrieves additional data or performs operations via CICS.
        /// </summary>
        /// <param name="requestId">The request ID for the operation.</param>
        /// <returns>Additional data required for the calculation process.</returns>
        object RetrieveCicsData(string requestId);

        /// <summary>
        /// Sends the results of the calculation process to a CICS transaction.
        /// </summary>
        /// <param name="request">The calculation request containing input data.</param>
        /// <param name="paymentSchedules">The list of calculated payment schedules.</param>
        void SendCicsResults(IntcalcDomainModels.CalculationRequest request, List<IntcalcDomainModels.CalculationRequest.PaymentSchedule> paymentSchedules);
    }
}