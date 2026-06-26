using System.Threading.Tasks;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    /// <summary>
    /// Interface for the INTCALC application logic.
    /// Each method corresponds to a paragraph in the original COBOL program.
    /// </summary>
    public interface IIntcalcService
    {
        /// <summary>
        /// Initializes the calculation process.
        /// </summary>
        Task InitializeCalculationAsync();

        /// <summary>
        /// Validates the input data for the calculation.
        /// </summary>
        /// <param name="request">The calculation request containing input data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ValidateInputAsync(IntcalcDomainModels.CalculationRequest request);

        /// <summary>
        /// Performs the interest calculation based on the provided input.
        /// </summary>
        /// <param name="request">The calculation request containing input data.</param>
        /// <returns>The result of the calculation.</returns>
        Task<IntcalcDomainModels.CalculationResult> PerformCalculationAsync(IntcalcDomainModels.CalculationRequest request);

        /// <summary>
        /// Finalizes the calculation process and prepares the result.
        /// </summary>
        /// <param name="result">The calculation result to finalize.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task FinalizeCalculationAsync(IntcalcDomainModels.CalculationResult result);
    }

    /// <summary>
    /// Interface for file I/O operations related to INTCALC.
    /// </summary>
    public interface IIntcalcRepository
    {
        /// <summary>
        /// Reads input data from a file.
        /// </summary>
        /// <returns>The calculation request read from the file.</returns>
        Task<IntcalcDomainModels.CalculationRequest> ReadInputAsync();

        /// <summary>
        /// Writes the calculation result to a file.
        /// </summary>
        /// <param name="result">The calculation result to write.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task WriteOutputAsync(IntcalcDomainModels.CalculationResult result);
    }

    /// <summary>
    /// Interface for CICS operations related to INTCALC.
    /// </summary>
    public interface IIntcalcCicsService
    {
        /// <summary>
        /// Retrieves additional data required for the calculation from a CICS transaction.
        /// </summary>
        /// <param name="accountNumber">The account number for which data is retrieved.</param>
        /// <returns>The additional data required for the calculation.</returns>
        Task<IntcalcDomainModels.CalculationRequest> RetrieveCicsDataAsync(string accountNumber);

        /// <summary>
        /// Logs the calculation result to a CICS transaction.
        /// </summary>
        /// <param name="result">The calculation result to log.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task LogCicsResultAsync(IntcalcDomainModels.CalculationResult result);
    }
}