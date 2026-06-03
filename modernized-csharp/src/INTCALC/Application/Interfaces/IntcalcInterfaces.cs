using System;
using System.Threading.Tasks;

namespace INTCALC.Application.Interfaces
{
    /// <summary>
    /// Interface for the main application logic of the INTCALC program.
    /// </summary>
    public interface IIntcalcApplication
    {
        /// <summary>
        /// Represents the main entry point of the program.
        /// Corresponds to the 0000-MAIN paragraph in COBOL.
        /// </summary>
        Task ExecuteMainAsync();

        /// <summary>
        /// Represents the looping logic for processing accounts.
        /// Corresponds to the 1000-LOOP paragraph in COBOL.
        /// </summary>
        Task ProcessLoopAsync();

        /// <summary>
        /// Represents the interest accrual logic.
        /// Corresponds to the 2000-ACCRUE paragraph in COBOL.
        /// </summary>
        Task AccrueInterestAsync();
    }

    /// <summary>
    /// Interface for repository operations related to account file I/O.
    /// </summary>
    public interface IAccountFileRepository
    {
        /// <summary>
        /// Reads account data from the ACCT-FILE.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ReadAccountFileAsync();

        /// <summary>
        /// Writes interest journal data to the INT-JOURNAL.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task WriteInterestJournalAsync();

        /// <summary>
        /// Opens the account file for processing.
        /// Corresponds to the OPEN statement in the 0000-MAIN paragraph in COBOL.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task OpenAccountFileAsync();

        /// <summary>
        /// Closes the account file after processing.
        /// Corresponds to the CLOSE statement in the 0000-MAIN paragraph in COBOL.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CloseAccountFileAsync();
    }
}