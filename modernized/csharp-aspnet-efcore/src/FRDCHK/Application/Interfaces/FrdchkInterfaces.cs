using System.Collections.Generic;
using System.Threading.Tasks;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    /// <summary>
    /// Interface for the main application service of the FRDCHK program.
    /// </summary>
    public interface IFrdchkService
    {
        /// <summary>
        /// Executes the main program logic.
        /// </summary>
        Task ExecuteAsync();
    }

    /// <summary>
    /// Repository interface for managing FraudCheckRecord entities.
    /// </summary>
    public interface IFraudCheckRepository
    {
        /// <summary>
        /// Retrieves a FraudCheckRecord by its unique record ID.
        /// </summary>
        /// <param name="recordId">The unique identifier of the record.</param>
        /// <returns>The FraudCheckRecord if found; otherwise, null.</returns>
        Task<FrdchkDomainModels.FraudCheckRecord?> GetByRecordIdAsync(string recordId);

        /// <summary>
        /// Adds a new FraudCheckRecord to the repository.
        /// </summary>
        /// <param name="record">The FraudCheckRecord to add.</param>
        Task AddAsync(FrdchkDomainModels.FraudCheckRecord record);

        /// <summary>
        /// Updates an existing FraudCheckRecord in the repository.
        /// </summary>
        /// <param name="record">The FraudCheckRecord to update.</param>
        Task UpdateAsync(FrdchkDomainModels.FraudCheckRecord record);

        /// <summary>
        /// Deletes a FraudCheckRecord by its unique record ID.
        /// </summary>
        /// <param name="recordId">The unique identifier of the record to delete.</param>
        Task DeleteAsync(string recordId);

        /// <summary>
        /// Retrieves all FraudCheckRecords from the repository.
        /// </summary>
        /// <returns>A list of all FraudCheckRecords.</returns>
        Task<List<FrdchkDomainModels.FraudCheckRecord>> GetAllAsync();
    }

    /// <summary>
    /// Service interface for handling CICS operations related to fraud checks.
    /// </summary>
    public interface ICicsFraudCheckService
    {
        /// <summary>
        /// Performs a CICS transaction to validate a fraud check record.
        /// </summary>
        /// <param name="record">The FraudCheckRecord to validate.</param>
        /// <returns>A boolean indicating whether the validation was successful.</returns>
        Task<bool> ValidateFraudCheckAsync(FrdchkDomainModels.FraudCheckRecord record);

        /// <summary>
        /// Performs a CICS transaction to mark a fraud check record as reviewed.
        /// </summary>
        /// <param name="recordId">The unique identifier of the record to mark as reviewed.</param>
        Task MarkAsReviewedAsync(string recordId);
    }
}