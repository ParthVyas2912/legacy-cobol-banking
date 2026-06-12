using System;
using System.Collections.Generic;
using STMTRPT.Domain;

namespace STMTRPT.Application.Interfaces
{
    /// <summary>
    /// Interface for handling the main STMTRPT program logic.
    /// Each COBOL paragraph is represented as a method.
    /// </summary>
    public interface IStmtrptService
    {
        /// <summary>
        /// Initializes the program and sets up required resources.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Reads input data and prepares it for processing.
        /// </summary>
        void ReadInputData();

        /// <summary>
        /// Processes the statement report data.
        /// </summary>
        void ProcessStatementReport();

        /// <summary>
        /// Formats the statement report for output.
        /// </summary>
        void FormatStatementReport();

        /// <summary>
        /// Writes the formatted report to the output destination.
        /// </summary>
        void WriteOutputReport();

        /// <summary>
        /// Finalizes the program and releases resources.
        /// </summary>
        void Finalize();
    }

    /// <summary>
    /// Interface for handling file I/O operations related to STMTRPT.
    /// </summary>
    public interface IStmtrptRepository
    {
        /// <summary>
        /// Reads a statement report from the input file.
        /// </summary>
        /// <returns>A <see cref="StmtrptDomainModels.StatementReport"/> object.</returns>
        StmtrptDomainModels.StatementReport ReadStatementReport();

        /// <summary>
        /// Writes a formatted statement report to the output file.
        /// </summary>
        /// <param name="report">The <see cref="StmtrptDomainModels.StatementReport"/> to write.</param>
        void WriteStatementReport(StmtrptDomainModels.StatementReport report);

        /// <summary>
        /// Reads a single transaction record from the input file.
        /// </summary>
        /// <returns>A <see cref="StmtrptDomainModels.Transaction"/> object.</returns>
        StmtrptDomainModels.Transaction ReadTransactionRecord();

        /// <summary>
        /// Writes a single transaction record to the output file.
        /// </summary>
        /// <param name="transaction">The <see cref="StmtrptDomainModels.Transaction"/> to write.</param>
        void WriteTransactionRecord(StmtrptDomainModels.Transaction transaction);
    }

    /// <summary>
    /// Interface for handling CICS operations related to STMTRPT.
    /// </summary>
    public interface IStmtrptCicsService
    {
        /// <summary>
        /// Retrieves additional data or performs operations via CICS.
        /// </summary>
        /// <param name="transactionId">The transaction ID for which data is retrieved.</param>
        /// <returns>A <see cref="StmtrptDomainModels.Transaction"/> object.</returns>
        StmtrptDomainModels.Transaction RetrieveTransactionData(string transactionId);

        /// <summary>
        /// Logs a message or event to the CICS system.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogMessage(string message);

        /// <summary>
        /// Commits a transaction in the CICS system.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back a transaction in the CICS system.
        /// </summary>
        void RollbackTransaction();
    }
}