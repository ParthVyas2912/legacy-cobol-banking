using System;
using System.Collections.Generic;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    /// <summary>
    /// Interface for the main INTCALC application logic.
    /// Each method corresponds to a paragraph in the original COBOL program.
    /// </summary>
    public interface IIntcalcApplicationService
    {
        /// <summary>
        /// Initializes the calculation process.
        /// </summary>
        void InitializeCalculation();

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
        /// Generates an amortization schedule based on the calculation results.
        /// </summary>
        /// <param name="input">The input data for the calculation.</param>
        /// <param name="result">The result of the calculation.</param>
        void GenerateAmortizationSchedule(IntcalcDomainModels.CalculationInput input, IntcalcDomainModels.CalculationResult result);

        /// <summary>
        /// Finalizes the calculation process and performs any necessary cleanup.
        /// </summary>
        void FinalizeCalculation();
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
        /// Writes the calculation results to a file.
        /// </summary>
        /// <param name="filePath">The path to the output file.</param>
        /// <param name="result">The calculation results to write.</param>
        void WriteResultsToFile(string filePath, IntcalcDomainModels.CalculationResult result);

        /// <summary>
        /// Appends an amortization schedule to an existing file.
        /// </summary>
        /// <param name="filePath">The path to the output file.</param>
        /// <param name="schedule">The amortization schedule to append.</param>
        void AppendAmortizationScheduleToFile(string filePath, List<IntcalcDomainModels.AmortizationSchedule> schedule);
    }

    /// <summary>
    /// Interface for CICS operations related to INTCALC.
    /// </summary>
    public interface IIntcalcCicsService
    {
        /// <summary>
        /// Retrieves account details from a CICS system.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve details for.</param>
        /// <returns>The account details retrieved from the CICS system.</returns>
        IntcalcDomainModels.CalculationInput RetrieveAccountDetails(string accountNumber);

        /// <summary>
        /// Updates account details in a CICS system.
        /// </summary>
        /// <param name="accountNumber">The account number to update.</param>
        /// <param name="result">The calculation results to update in the account.</param>
        void UpdateAccountDetails(string accountNumber, IntcalcDomainModels.CalculationResult result);

        /// <summary>
        /// Logs a transaction in the CICS system.
        /// </summary>
        /// <param name="transactionId">The transaction ID to log.</param>
        /// <param name="details">The details of the transaction.</param>
        void LogTransaction(string transactionId, string details);
    }
}