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
        /// Initializes the application.
        /// </summary>
        void InitializeApplication();

        /// <summary>
        /// Validates the input data for the interest calculation.
        /// </summary>
        /// <param name="input">The input data for the calculation.</param>
        void ValidateInput(IntcalcDomainModels.CalculationInput input);

        /// <summary>
        /// Performs the interest calculation based on the input data.
        /// </summary>
        /// <param name="input">The input data for the calculation.</param>
        /// <returns>The result of the calculation.</returns>
        IntcalcDomainModels.CalculationResult PerformCalculation(IntcalcDomainModels.CalculationInput input);

        /// <summary>
        /// Finalizes the application process.
        /// </summary>
        void FinalizeApplication();
    }

    /// <summary>
    /// Interface for file I/O operations related to INTCALC.
    /// </summary>
    public interface IIntcalcFileRepository
    {
        /// <summary>
        /// Reads input data from a file.
        /// </summary>
        /// <param name="filePath">The path to the input file.</param>
        /// <returns>The input data read from the file.</returns>
        IntcalcDomainModels.CalculationInput ReadInputFromFile(string filePath);

        /// <summary>
        /// Writes the calculation result to a file.
        /// </summary>
        /// <param name="filePath">The path to the output file.</param>
        /// <param name="result">The calculation result to write.</param>
        void WriteResultToFile(string filePath, IntcalcDomainModels.CalculationResult result);
    }

    /// <summary>
    /// Interface for CICS operations related to INTCALC.
    /// </summary>
    public interface IIntcalcCicsService
    {
        /// <summary>
        /// Retrieves customer data from the CICS system.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>The customer data.</returns>
        IntcalcDomainModels.CalculationInput RetrieveCustomerData(string customerId);

        /// <summary>
        /// Updates the customer account with the calculation result in the CICS system.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="result">The calculation result to update.</param>
        void UpdateCustomerAccount(string customerId, IntcalcDomainModels.CalculationResult result);
    }
}