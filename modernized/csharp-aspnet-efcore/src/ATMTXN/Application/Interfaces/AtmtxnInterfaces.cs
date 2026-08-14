using System.Collections.Generic;
using ATMTXN.Domain;

namespace ATMTXN.Application.Interfaces
{
    /// <summary>
    /// Interface for ATM transaction processing.
    /// Each method corresponds to a COBOL paragraph.
    /// </summary>
    public interface IAtmtxnProcessor
    {
        /// <summary>
        /// Initializes the transaction processing environment.
        /// </summary>
        void InitializeEnvironment();

        /// <summary>
        /// Reads input transaction data.
        /// </summary>
        void ReadInputTransaction();

        /// <summary>
        /// Validates the transaction data.
        /// </summary>
        void ValidateTransaction();

        /// <summary>
        /// Processes the transaction based on its type.
        /// </summary>
        void ProcessTransaction();

        /// <summary>
        /// Writes the transaction output data.
        /// </summary>
        void WriteOutputTransaction();

        /// <summary>
        /// Finalizes the transaction processing environment.
        /// </summary>
        void FinalizeEnvironment();
    }

    /// <summary>
    /// Interface for file I/O operations related to transactions.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads a transaction record from the input file.
        /// </summary>
        /// <returns>A transaction record.</returns>
        AtmtxnDomainModels.TransactionRecord ReadTransaction();

        /// <summary>
        /// Writes a transaction record to the output file.
        /// </summary>
        /// <param name="transaction">The transaction record to write.</param>
        void WriteTransaction(AtmtxnDomainModels.TransactionRecord transaction);
    }

    /// <summary>
    /// Interface for CICS operations related to ATM transactions.
    /// </summary>
    public interface ITransactionCicsService
    {
        /// <summary>
        /// Performs account validation through CICS.
        /// </summary>
        /// <param name="accountNumber">The account number to validate.</param>
        /// <returns>True if the account is valid; otherwise, false.</returns>
        bool ValidateAccount(string accountNumber);

        /// <summary>
        /// Updates the account balance through CICS.
        /// </summary>
        /// <param name="accountNumber">The account number to update.</param>
        /// <param name="amount">The amount to update the balance by.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        bool UpdateAccountBalance(string accountNumber, decimal amount);

        /// <summary>
        /// Logs a transaction in the CICS system.
        /// </summary>
        /// <param name="transaction">The transaction record to log.</param>
        void LogTransaction(AtmtxnDomainModels.TransactionRecord transaction);
    }
}