using System;
using System.Threading.Tasks;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    /// <summary>
    /// Interface for the main application logic of the INTCALC program.
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
        /// <returns>A boolean indicating whether the input is valid.</returns>
        Task<bool> ValidateInputAsync(IntcalcDomainModels.CalculationRequest request);

        /// <summary>
        /// Performs the calculation based on the provided request.
        /// </summary>
        /// <param name="request">The calculation request containing input data.</param>
        /// <returns>The result of the calculation.</returns>
        Task<IntcalcDomainModels.CalculationResult> PerformCalculationAsync(IntcalcDomainModels.CalculationRequest request);

        /// <summary>
        /// Finalizes the calculation process and performs any necessary cleanup.
        /// </summary>
        Task FinalizeCalculationAsync();
    }

    /// <summary>
    /// Interface for handling file-based operations related to calculation requests.
    /// </summary>
    public interface ICalculationRequestRepository
    {
        /// <summary>
        /// Reads a calculation request from the data store.
        /// </summary>
        /// <param name="requestId">The identifier of the calculation request.</param>
        /// <returns>The calculation request object.</returns>
        Task<IntcalcDomainModels.CalculationRequest> ReadRequestAsync(string requestId);

        /// <summary>
        /// Writes a calculation request to the data store.
        /// </summary>
        /// <param name="request">The calculation request to write.</param>
        Task WriteRequestAsync(IntcalcDomainModels.CalculationRequest request);

        /// <summary>
        /// Deletes a calculation request from the data store.
        /// </summary>
        /// <param name="requestId">The identifier of the calculation request to delete.</param>
        Task DeleteRequestAsync(string requestId);
    }

    /// <summary>
    /// Interface for handling file-based operations related to calculation results.
    /// </summary>
    public interface ICalculationResultRepository
    {
        /// <summary>
        /// Reads a calculation result from the data store.
        /// </summary>
        /// <param name="requestId">The identifier of the calculation request.</param>
        /// <returns>The calculation result object.</returns>
        Task<IntcalcDomainModels.CalculationResult> ReadResultAsync(string requestId);

        /// <summary>
        /// Writes a calculation result to the data store.
        /// </summary>
        /// <param name="result">The calculation result to write.</param>
        Task WriteResultAsync(IntcalcDomainModels.CalculationResult result);

        /// <summary>
        /// Deletes a calculation result from the data store.
        /// </summary>
        /// <param name="requestId">The identifier of the calculation result to delete.</param>
        Task DeleteResultAsync(string requestId);
    }

    /// <summary>
    /// Interface for handling CICS operations related to INTCALC.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Sends a request to a CICS transaction.
        /// </summary>
        /// <param name="transactionId">The identifier of the CICS transaction.</param>
        /// <param name="data">The data to send to the transaction.</param>
        /// <returns>The response from the CICS transaction.</returns>
        Task<string> SendTransactionAsync(string transactionId, string data);

        /// <summary>
        /// Retrieves the status of a CICS transaction.
        /// </summary>
        /// <param name="transactionId">The identifier of the CICS transaction.</param>
        /// <returns>The status of the CICS transaction.</returns>
        Task<string> GetTransactionStatusAsync(string transactionId);
    }
}