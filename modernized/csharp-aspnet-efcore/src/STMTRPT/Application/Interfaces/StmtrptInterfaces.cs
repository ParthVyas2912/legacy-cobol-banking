using System;
using System.Collections.Generic;
using STMTRPT.Domain;

namespace STMTRPT.Application.Interfaces
{
    /// <summary>
    /// Interface for handling file I/O operations related to statement reports.
    /// </summary>
    public interface IStatementReportRepository
    {
        /// <summary>
        /// Reads a statement report from the file system.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve the statement for.</param>
        /// <returns>The statement report for the specified account number.</returns>
        StmtrptDomainModels.StatementReport ReadStatementReport(string accountNumber);

        /// <summary>
        /// Writes a statement report to the file system.
        /// </summary>
        /// <param name="report">The statement report to write.</param>
        void WriteStatementReport(StmtrptDomainModels.StatementReport report);
    }

    /// <summary>
    /// Interface for handling CICS operations related to statement reports.
    /// </summary>
    public interface IStatementReportService
    {
        /// <summary>
        /// Retrieves account details from the CICS system.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve details for.</param>
        /// <returns>The account details as a statement report.</returns>
        StmtrptDomainModels.StatementReport GetAccountDetails(string accountNumber);

        /// <summary>
        /// Updates account details in the CICS system.
        /// </summary>
        /// <param name="report">The statement report containing updated account details.</param>
        void UpdateAccountDetails(StmtrptDomainModels.StatementReport report);
    }

    /// <summary>
    /// Interface for the main application logic of the STMTRPT program.
    /// </summary>
    public interface IStatementReportProcessor
    {
        /// <summary>
        /// Processes the statement report for a given account.
        /// </summary>
        /// <param name="accountNumber">The account number to process the statement for.</param>
        void ProcessStatementReport(string accountNumber);

        /// <summary>
        /// Generates a summary of the statement report.
        /// </summary>
        /// <param name="report">The statement report to summarize.</param>
        /// <returns>A summary string of the statement report.</returns>
        string GenerateReportSummary(StmtrptDomainModels.StatementReport report);
    }
}