using System;
using System.Collections.Generic;
using STMTRPT.Domain;

namespace STMTRPT.Application.Interfaces
{
    /// <summary>
    /// Interface for handling file I/O operations related to Statement Reports.
    /// </summary>
    public interface IStatementReportRepository
    {
        /// <summary>
        /// Reads a statement report by its unique identifier.
        /// </summary>
        /// <param name="reportId">The unique identifier of the report.</param>
        /// <returns>The statement report object.</returns>
        StmtrptDomainModels.StatementReport ReadReportById(string reportId);

        /// <summary>
        /// Writes a statement report to the repository.
        /// </summary>
        /// <param name="report">The statement report object to write.</param>
        void WriteReport(StmtrptDomainModels.StatementReport report);

        /// <summary>
        /// Deletes a statement report by its unique identifier.
        /// </summary>
        /// <param name="reportId">The unique identifier of the report to delete.</param>
        void DeleteReport(string reportId);
    }

    /// <summary>
    /// Interface for handling CICS operations related to Statement Reports.
    /// </summary>
    public interface IStatementReportService
    {
        /// <summary>
        /// Retrieves customer information for a given account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve information for.</param>
        /// <returns>The customer name associated with the account.</returns>
        string GetCustomerInfo(string accountNumber);

        /// <summary>
        /// Retrieves the current balance for a given account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve the balance for.</param>
        /// <returns>The current account balance.</returns>
        decimal GetAccountBalance(string accountNumber);

        /// <summary>
        /// Retrieves a list of recent transactions for a given account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve transactions for.</param>
        /// <returns>A list of transactions associated with the account.</returns>
        List<StmtrptDomainModels.StatementReport.Transaction> GetRecentTransactions(string accountNumber);
    }

    /// <summary>
    /// Interface for the main application logic of the Statement Report program.
    /// </summary>
    public interface IStatementReportProcessor
    {
        /// <summary>
        /// Generates a statement report for a given account number and date range.
        /// </summary>
        /// <param name="accountNumber">The account number to generate the report for.</param>
        /// <param name="startDate">The start date of the report period.</param>
        /// <param name="endDate">The end date of the report period.</param>
        /// <returns>The generated statement report.</returns>
        StmtrptDomainModels.StatementReport GenerateReport(string accountNumber, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Validates the input parameters for generating a statement report.
        /// </summary>
        /// <param name="accountNumber">The account number to validate.</param>
        /// <param name="startDate">The start date to validate.</param>
        /// <param name="endDate">The end date to validate.</param>
        void ValidateReportParameters(string accountNumber, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Formats the statement report for output.
        /// </summary>
        /// <param name="report">The statement report to format.</param>
        /// <returns>A formatted string representation of the report.</returns>
        string FormatReport(StmtrptDomainModels.StatementReport report);
    }
}