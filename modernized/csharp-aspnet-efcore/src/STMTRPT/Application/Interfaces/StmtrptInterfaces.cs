using STMTRPT.Domain;

namespace STMTRPT.Application.Interfaces
{
    /// <summary>
    /// Interface for the STMTRPT application logic.
    /// Each method corresponds to a paragraph in the COBOL program.
    /// </summary>
    public interface IStmtrptApplicationService
    {
        /// <summary>
        /// Initializes the report generation process.
        /// </summary>
        void InitializeReport();

        /// <summary>
        /// Processes the report header.
        /// </summary>
        void ProcessReportHeader();

        /// <summary>
        /// Processes the report details.
        /// </summary>
        void ProcessReportDetails();

        /// <summary>
        /// Processes the report footer.
        /// </summary>
        void ProcessReportFooter();

        /// <summary>
        /// Finalizes the report generation process.
        /// </summary>
        void FinalizeReport();
    }

    /// <summary>
    /// Interface for file I/O operations related to STMTRPT.
    /// </summary>
    public interface IStmtrptRepository
    {
        /// <summary>
        /// Reads the input data required for report generation.
        /// </summary>
        /// <returns>The input data as a StatementReport object.</returns>
        StmtrptDomainModels.StatementReport ReadInputData();

        /// <summary>
        /// Writes the generated report to the output.
        /// </summary>
        /// <param name="report">The generated StatementReport object.</param>
        void WriteOutputData(StmtrptDomainModels.StatementReport report);
    }

    /// <summary>
    /// Interface for CICS operations related to STMTRPT.
    /// </summary>
    public interface IStmtrptCicsService
    {
        /// <summary>
        /// Retrieves additional data from a CICS transaction.
        /// </summary>
        /// <param name="transactionId">The transaction ID to retrieve data for.</param>
        /// <returns>The retrieved data as a ReportDetail object.</returns>
        StmtrptDomainModels.ReportDetail RetrieveCicsData(string transactionId);

        /// <summary>
        /// Logs a message or event to the CICS system.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogCicsMessage(string message);
    }
}