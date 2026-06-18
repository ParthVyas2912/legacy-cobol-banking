using System.Collections.Generic;
using ATMTXN.Domain;

namespace ATMTXN.Application.Interfaces
{
    /// <summary>
    /// Interface for handling ATM transaction operations.
    /// Each method corresponds to a paragraph in the original COBOL program.
    /// </summary>
    public interface IAtmtxnService
    {
        /// <summary>
        /// Initializes the ATM transaction process.
        /// </summary>
        void InitializeTransaction();

        /// <summary>
        /// Reads the header record from the input file.
        /// </summary>
        /// <returns>The header record.</returns>
        AtmTransaction.HeaderRecord ReadHeaderRecord();

        /// <summary>
        /// Processes the header record.
        /// </summary>
        /// <param name="headerRecord">The header record to process.</param>
        void ProcessHeaderRecord(AtmTransaction.HeaderRecord headerRecord);

        /// <summary>
        /// Reads a transaction detail record from the input file.
        /// </summary>
        /// <returns>The transaction detail record.</returns>
        AtmTransaction.TransactionDetail ReadTransactionDetail();

        /// <summary>
        /// Processes a transaction detail record.
        /// </summary>
        /// <param name="transactionDetail">The transaction detail record to process.</param>
        void ProcessTransactionDetail(AtmTransaction.TransactionDetail transactionDetail);

        /// <summary>
        /// Reads the trailer record from the input file.
        /// </summary>
        /// <returns>The trailer record.</returns>
        AtmTransaction.TrailerRecord ReadTrailerRecord();

        /// <summary>
        /// Processes the trailer record.
        /// </summary>
        /// <param name="trailerRecord">The trailer record to process.</param>
        void ProcessTrailerRecord(AtmTransaction.TrailerRecord trailerRecord);

        /// <summary>
        /// Validates the transaction data.
        /// </summary>
        /// <param name="transaction">The ATM transaction to validate.</param>
        void ValidateTransaction(AtmTransaction transaction);

        /// <summary>
        /// Writes the processed transaction to the output file.
        /// </summary>
        /// <param name="transaction">The ATM transaction to write.</param>
        void WriteTransaction(AtmTransaction transaction);

        /// <summary>
        /// Finalizes the ATM transaction process.
        /// </summary>
        void FinalizeTransaction();

        /// <summary>
        /// Logs an error encountered during processing.
        /// </summary>
        /// <param name="errorMessage">The error message to log.</param>
        void LogError(string errorMessage);
    }

    /// <summary>
    /// Interface for handling file input/output operations for ATM transactions.
    /// </summary>
    public interface IAtmtxnRepository
    {
        /// <summary>
        /// Opens the input file for reading.
        /// </summary>
        void OpenInputFile();

        /// <summary>
        /// Reads a record from the input file.
        /// </summary>
        /// <returns>The raw record data as a string.</returns>
        string ReadInputRecord();

        /// <summary>
        /// Closes the input file.
        /// </summary>
        void CloseInputFile();

        /// <summary>
        /// Opens the output file for writing.
        /// </summary>
        void OpenOutputFile();

        /// <summary>
        /// Writes a record to the output file.
        /// </summary>
        /// <param name="record">The raw record data to write.</param>
        void WriteOutputRecord(string record);

        /// <summary>
        /// Closes the output file.
        /// </summary>
        void CloseOutputFile();

        /// <summary>
        /// Checks if the input file has reached the end.
        /// </summary>
        /// <returns>True if end of file is reached, otherwise false.</returns>
        bool IsEndOfFile();
    }

    /// <summary>
    /// Interface for handling CICS operations related to ATM transactions.
    /// </summary>
    public interface IAtmtxnCicsService
    {
        /// <summary>
        /// Sends a transaction to the CICS system for processing.
        /// </summary>
        /// <param name="transaction">The ATM transaction to send.</param>
        void SendTransactionToCics(AtmTransaction transaction);

        /// <summary>
        /// Receives a response from the CICS system.
        /// </summary>
        /// <returns>The response from the CICS system.</returns>
        string ReceiveCicsResponse();

        /// <summary>
        /// Logs a CICS communication error.
        /// </summary>
        /// <param name="errorMessage">The error message to log.</param>
        void LogCicsError(string errorMessage);
    }
}