using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    public interface IBcheodService
    {
        /// <summary>
        /// Main entry point for executing the BCHEOD program.
        /// </summary>
        Task ExecuteAsync();
    }

    public interface IBcheodRepository
    {
        /// <summary>
        /// Retrieves a record by its unique identifier.
        /// </summary>
        /// <param name="recordId">The unique identifier of the record.</param>
        /// <returns>The matching BcheodRecord, or null if not found.</returns>
        Task<BcheodDomainModels.BcheodRecord?> GetByRecordIdAsync(string recordId);

        /// <summary>
        /// Adds a new record to the repository.
        /// </summary>
        /// <param name="record">The record to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(BcheodDomainModels.BcheodRecord record);

        /// <summary>
        /// Updates an existing record in the repository.
        /// </summary>
        /// <param name="record">The record to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(BcheodDomainModels.BcheodRecord record);

        /// <summary>
        /// Deletes a record by its unique identifier.
        /// </summary>
        /// <param name="recordId">The unique identifier of the record to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(string recordId);

        /// <summary>
        /// Retrieves all records from the repository.
        /// </summary>
        /// <returns>A list of all BcheodRecords.</returns>
        Task<List<BcheodDomainModels.BcheodRecord>> GetAllAsync();
    }
}