using System.Collections.Generic;
using STMTRPT.Domain;

namespace STMTRPT.Application.Interfaces
{
    /// <summary>
    /// Interface for repository operations related to file I/O in the STMTRPT program.
    /// </summary>
    public interface IStmtrptRepository
    {
        /// <summary>
        /// Reads account summaries from the input file.
        /// </summary>
        /// <returns>A list of account summaries.</returns>
        List<StmtrptDomainModels.AccountSummary> ReadAccountSummaries();

        /// <summary>
        /// Writes account summaries to the output file.
        /// </summary>
        /// <param name="accountSummaries">The list of account summaries to write.</param>
        void WriteAccountSummaries(List<StmtrptDomainModels.AccountSummary> accountSummaries);
    }

    /// <summary>
    /// Interface for service operations related to CICS interactions in the STMTRPT program.
    /// </summary>
    public interface IStmtrptService
    {
        /// <summary>
        /// Retrieves additional account details from the CICS system.
        /// </summary>
        /// <param name="accountNumber">The account number for which details are to be retrieved.</param>
        /// <returns>The account details.</returns>
        StmtrptDomainModels.AccountSummary GetAccountDetails(string accountNumber);
    }

    /// <summary>
    /// Interface for the main application logic of the STMTRPT program.
    /// </summary>
    public interface IStmtrptApplication
    {
        /// <summary>
        /// Initializes the STMTRPT program.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Processes account summaries.
        /// </summary>
        void ProcessAccountSummaries();

        /// <summary>
        /// Finalizes the STMTRPT program.
        /// </summary>
        void Finalize();
    }
}