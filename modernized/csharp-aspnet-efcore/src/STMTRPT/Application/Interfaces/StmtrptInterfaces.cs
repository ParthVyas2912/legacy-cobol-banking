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
        /// <param name="reportId">The unique identifier of the report to read.</param>
        /// <returns>The statement report data.</returns>
        StmtrptDomainModels.StatementReport ReadStatementReport(string reportId);

        /// <summary>
        /// Writes a statement report to the file system.
        /// </summary>
        /// <param name="report">The statement report data to write.</param>
        void WriteStatementReport(StmtrptDomainModels.StatementReport report);

        /// <summary>
        /// Deletes a statement report from the file system.
        /// </summary>
        /// <param name="reportId">The unique identifier of the report to delete.</param>
        void DeleteStatementReport(string reportId);

        /// <summary>
        /// Updates an existing statement report in the file system.
        /// </summary>
        /// <param name="report">The updated statement report data.</param>
        void UpdateStatementReport(StmtrptDomainModels.StatementReport report);
    }

    /// <summary>
    /// Interface for handling CICS operations related to statement reports.
    /// </summary>
    public interface IStatementReportService
    {
        /// <summary>
        /// Retrieves a statement report from the CICS system.
        /// </summary>
        /// <param name="reportId">The unique identifier of the report to retrieve.</param>
        /// <returns>The statement report data.</returns>
        StmtrptDomainModels.StatementReport GetStatementReportFromCICS(string reportId);

        /// <summary>
        /// Sends a statement report to the CICS system.
        /// </summary>
        /// <param name="report">The statement report data to send.</param>
        void SendStatementReportToCICS(StmtrptDomainModels.StatementReport report);

        /// <summary>
        /// Deletes a statement report from the CICS system.
        /// </summary>
        /// <param name="reportId">The unique identifier of the report to delete.</param>
        void DeleteStatementReportFromCICS(string reportId);

        /// <summary>
        /// Updates an existing statement report in the CICS system.
        /// </summary>
        /// <param name="report">The updated statement report data.</param>
        void UpdateStatementReportInCICS(StmtrptDomainModels.StatementReport report);
    }

    /// <summary>
    /// Interface for processing statement reports.
    /// </summary>
    public interface IStatementReportProcessor
    {
        /// <summary>
        /// Generates a statement report based on the provided data.
        /// </summary>
        /// <param name="customerName">The name of the customer.</param>
        /// <param name="accountBalance">The account balance of the customer.</param>
        /// <param name="transactions">The list of transactions to include in the report.</param>
        /// <returns>The generated statement report.</returns>
        StmtrptDomainModels.StatementReport GenerateStatementReport(
            string customerName,
            decimal accountBalance,
            List<StmtrptDomainModels.Transaction> transactions);

        /// <summary>
        /// Validates a statement report for correctness and completeness.
        /// </summary>
        /// <param name="report">The statement report to validate.</param>
        /// <returns>True if the report is valid, otherwise false.</returns>
        bool ValidateStatementReport(StmtrptDomainModels.StatementReport report);

        /// <summary>
        /// Formats a statement report for display or printing.
        /// </summary>
        /// <param name="report">The statement report to format.</param>
        /// <returns>A formatted string representation of the report.</returns>
        string FormatStatementReport(StmtrptDomainModels.StatementReport report);

        /// <summary>
        /// Calculates the total balance from a list of transactions.
        /// </summary>
        /// <param name="transactions">The list of transactions to calculate the balance from.</param>
        /// <returns>The calculated total balance.</returns>
        decimal CalculateTotalBalance(List<StmtrptDomainModels.Transaction> transactions);
    }
}