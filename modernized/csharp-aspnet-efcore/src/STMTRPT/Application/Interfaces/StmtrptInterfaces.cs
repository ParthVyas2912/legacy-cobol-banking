using System;
using STMTRPT.Domain;

namespace STMTRPT.Application.Interfaces
{
    /// <summary>
    /// Interface for handling file I/O operations related to the Statement Report.
    /// </summary>
    public interface IStatementReportRepository
    {
        /// <summary>
        /// Reads the statement report data from the input file.
        /// </summary>
        /// <returns>A populated StatementReport object.</returns>
        StmtrptDomainModels.StatementReport ReadStatementReport();

        /// <summary>
        /// Writes the statement report data to the output file.
        /// </summary>
        /// <param name="report">The StatementReport object to write.</param>
        void WriteStatementReport(StmtrptDomainModels.StatementReport report);
    }

    /// <summary>
    /// Interface for handling CICS operations related to the Statement Report.
    /// </summary>
    public interface IStatementReportService
    {
        /// <summary>
        /// Retrieves additional data for the statement report from a CICS transaction.
        /// </summary>
        /// <param name="reportId">The ID of the report to retrieve data for.</param>
        /// <returns>A populated StatementReport object with additional data.</returns>
        StmtrptDomainModels.StatementReport RetrieveCicsData(string reportId);

        /// <summary>
        /// Submits the statement report data to a CICS transaction for processing.
        /// </summary>
        /// <param name="report">The StatementReport object to submit.</param>
        void SubmitCicsData(StmtrptDomainModels.StatementReport report);
    }

    /// <summary>
    /// Interface for the main application logic of the Statement Report program.
    /// </summary>
    public interface IStatementReportProcessor
    {
        /// <summary>
        /// Initializes the processing of the statement report.
        /// </summary>
        void InitializeProcessing();

        /// <summary>
        /// Processes the statement report data.
        /// </summary>
        void ProcessStatementReport();

        /// <summary>
        /// Finalizes the processing of the statement report.
        /// </summary>
        void FinalizeProcessing();
    }
}