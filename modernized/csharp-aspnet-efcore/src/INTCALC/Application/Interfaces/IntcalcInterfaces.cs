using System;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    public interface IIntcalcService
    {
        /// <summary>
        /// Initializes the calculation process.
        /// </summary>
        void InitializeCalculation();

        /// <summary>
        /// Validates the input data for the calculation.
        /// </summary>
        /// <param name="input">The input data for the calculation.</param>
        /// <returns>True if the input is valid, otherwise false.</returns>
        bool ValidateInput(IntcalcDomainModels.CalculationInput input);

        /// <summary>
        /// Performs the interest calculation based on the provided input.
        /// </summary>
        /// <param name="input">The input data for the calculation.</param>
        /// <returns>The calculation output containing the results.</returns>
        IntcalcDomainModels.CalculationOutput PerformCalculation(IntcalcDomainModels.CalculationInput input);

        /// <summary>
        /// Finalizes the calculation process and prepares the output.
        /// </summary>
        /// <param name="output">The calculation output to finalize.</param>
        void FinalizeCalculation(IntcalcDomainModels.CalculationOutput output);
    }

    public interface IIntcalcRepository
    {
        /// <summary>
        /// Reads the input data for the calculation from a data source.
        /// </summary>
        /// <returns>The input data for the calculation.</returns>
        IntcalcDomainModels.CalculationInput ReadInput();

        /// <summary>
        /// Writes the calculation output to a data source.
        /// </summary>
        /// <param name="output">The calculation output to write.</param>
        void WriteOutput(IntcalcDomainModels.CalculationOutput output);
    }

    public interface ICicsService
    {
        /// <summary>
        /// Executes a CICS transaction for the calculation process.
        /// </summary>
        /// <param name="transactionId">The ID of the CICS transaction to execute.</param>
        /// <param name="input">The input data for the transaction.</param>
        /// <returns>The output data from the transaction.</returns>
        IntcalcDomainModels.CalculationOutput ExecuteTransaction(string transactionId, IntcalcDomainModels.CalculationInput input);
    }
}