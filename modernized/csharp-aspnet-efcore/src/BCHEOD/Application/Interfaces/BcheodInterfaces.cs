using System.Threading.Tasks;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    /// <summary>
    /// Interface for the BCHEOD application service.
    /// This interface exposes only the main entry point for the BCHEOD program.
    /// </summary>
    public interface IBcheodService
    {
        /// <summary>
        /// Main entry point for the BCHEOD program.
        /// </summary>
        Task ExecuteAsync();
    }

    /// <summary>
    /// Interface for file input operations.
    /// </summary>
    public interface IInputFileRepository
    {
        /// <summary>
        /// Read the next record from the input file.
        /// </summary>
        /// <returns>The next record as an ExampleRecord object.</returns>
        Task<BcheodDomainModels.ExampleRecord> ReadNextRecordAsync();
    }

    /// <summary>
    /// Interface for file output operations.
    /// </summary>
    public interface IOutputFileRepository
    {
        /// <summary>
        /// Write a record to the output file.
        /// </summary>
        /// <param name="record">The record to write.</param>
        Task WriteRecordAsync(BcheodDomainModels.ExampleRecord record);
    }

    /// <summary>
    /// Interface for CICS service operations.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Perform a CICS transaction.
        /// </summary>
        /// <param name="transactionData">The data required for the transaction.</param>
        /// <returns>The result of the transaction.</returns>
        Task<string> PerformTransactionAsync(string transactionData);
    }
}