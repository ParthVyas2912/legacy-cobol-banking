using System.Collections.Generic;
using System.Threading.Tasks;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    /// <summary>
    /// Interface for the main application service of the INTCALC program.
    /// </summary>
    public interface IIntcalcService
    {
        /// <summary>
        /// Executes the main logic of the INTCALC program.
        /// </summary>
        Task ExecuteAsync();
    }

    /// <summary>
    /// Interface for repository operations related to interest calculations.
    /// </summary>
    public interface IInterestCalculationRepository
    {
        /// <summary>
        /// Retrieves an interest calculation by account number.
        /// </summary>
        /// <param name="accountNumber">The account number to search for.</param>
        /// <returns>The interest calculation associated with the account number.</returns>
        Task<IntcalcDomainModels.InterestCalculation?> GetByAccountNumberAsync(string accountNumber);

        /// <summary>
        /// Adds a new interest calculation record.
        /// </summary>
        /// <param name="calculation">The interest calculation to add.</param>
        Task AddAsync(IntcalcDomainModels.InterestCalculation calculation);

        /// <summary>
        /// Updates an existing interest calculation record.
        /// </summary>
        /// <param name="calculation">The interest calculation to update.</param>
        Task UpdateAsync(IntcalcDomainModels.InterestCalculation calculation);

        /// <summary>
        /// Retrieves all interest calculations.
        /// </summary>
        /// <returns>A list of all interest calculations.</returns>
        Task<List<IntcalcDomainModels.InterestCalculation>> GetAllAsync();
    }

    /// <summary>
    /// Interface for CICS service operations related to external interactions.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Links to an external service for additional processing.
        /// </summary>
        /// <param name="data">The data to send to the external service.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task LinkAsync(object data);
    }
}