using System;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    /// <summary>
    /// Interface for BCHEOD application logic.
    /// Each COBOL paragraph is represented as a method.
    /// </summary>
    public interface IBcheodApplicationService
    {
        /// <summary>
        /// Initializes the program and sets up necessary configurations.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Reads input data and prepares it for processing.
        /// </summary>
        void ReadInputData();

        /// <summary>
        /// Validates the header record for correctness.
        /// </summary>
        void ValidateHeader();

        /// <summary>
        /// Processes each detail record in the input data.
        /// </summary>
        void ProcessDetails();

        /// <summary>
        /// Calculates totals based on the processed detail records.
        /// </summary>
        void CalculateTotals();

        /// <summary>
        /// Writes the output data to the designated output file.
        /// </summary>
        void WriteOutputData();

        /// <summary>
        /// Finalizes the program and performs cleanup operations.
        /// </summary>
        void Finalize();
    }

    /// <summary>
    /// Interface for BCHEOD file repository operations.
    /// Represents file I/O operations in the COBOL program.
    /// </summary>
    public interface IBcheodFileRepository
    {
        /// <summary>
        /// Reads the header record from the input file.
        /// </summary>
        /// <returns>The header record.</returns>
        BcheodDomainModels.Header ReadHeader();

        /// <summary>
        /// Reads the detail records from the input file.
        /// </summary>
        /// <returns>An array of detail records.</returns>
        BcheodDomainModels.Detail[] ReadDetails();

        /// <summary>
        /// Writes the header record to the output file.
        /// </summary>
        /// <param name="header">The header record to write.</param>
        void WriteHeader(BcheodDomainModels.Header header);

        /// <summary>
        /// Writes the detail records to the output file.
        /// </summary>
        /// <param name="details">The detail records to write.</param>
        void WriteDetails(BcheodDomainModels.Detail[] details);
    }

    /// <summary>
    /// Interface for BCHEOD CICS service operations.
    /// Represents CICS operations in the COBOL program.
    /// </summary>
    public interface IBcheodCicsService
    {
        /// <summary>
        /// Commits the current transaction to the database.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back the current transaction in case of an error.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Logs an error message to the CICS error log.
        /// </summary>
        /// <param name="errorMessage">The error message to log.</param>
        void LogError(string errorMessage);
    }
}