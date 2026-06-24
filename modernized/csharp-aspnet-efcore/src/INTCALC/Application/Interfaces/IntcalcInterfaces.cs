using System.Collections.Generic;
using System.Threading.Tasks;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    /// <summary>
    /// Interface for the main application service of the INTCALC program.
    /// This interface exposes only the public entry point for the INTCALC program.
    /// </summary>
    public interface IIntcalcService
    {
        /// <summary>
        /// Entry point for the INTCALC program.
        /// </summary>
        Task ExecuteAsync();
    }

    /// <summary>
    /// Interface for the repository handling interest calculation data.
    /// Corresponds to file I/O operations in the COBOL program.
    /// </summary>
    public interface IInterestCalculationRepository
    {
        /// <summary>
        /// Retrieves an interest calculation record by account number and calculation date.
        /// </summary>
        /// <param name="accountNumber">The account number.</param>
        /// <param name="calculationDate">The calculation date.</param>
        /// <returns>The interest calculation record, or null if not found.</returns>
        Task<IntcalcDomainModels.InterestCalculation?> GetByAccountAndDateAsync(string accountNumber, string calculationDate);

        /// <summary>
        /// Adds a new interest calculation record to the repository.
        /// </summary>
        /// <param name="calculation">The interest calculation record to add.</param>
        Task AddAsync(IntcalcDomainModels.InterestCalculation calculation);

        /// <summary>
        /// Updates an existing interest calculation record in the repository.
        /// </summary>
        /// <param name="calculation">The interest calculation record to update.</param>
        Task UpdateAsync(IntcalcDomainModels.InterestCalculation calculation);

        /// <summary>
        /// Retrieves all interest calculation records.
        /// </summary>
        /// <returns>A list of all interest calculation records.</returns>
        Task<List<IntcalcDomainModels.InterestCalculation>> GetAllAsync();
    }

    /// <summary>
    /// Interface for a service that interacts with external systems via CICS.
    /// Corresponds to CICS operations in the COBOL program.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Links to an external CICS service for additional processing.
        /// </summary>
        /// <param name="calculation">The interest calculation data to process.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task LinkToExternalServiceAsync(IntcalcDomainModels.InterestCalculation calculation);
    }
}