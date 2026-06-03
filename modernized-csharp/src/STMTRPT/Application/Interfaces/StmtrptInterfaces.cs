using System.Collections.Generic;
using STMTRPT.Domain;

namespace STMTRPT.Application.Interfaces
{
    public interface IStmtrptApplication
    {
        /// <summary>
        /// Main entry point for the STMTRPT program.
        /// </summary>
        void Main();

        /// <summary>
        /// Fetches customer records for processing.
        /// </summary>
        /// <returns>A list of customer records.</returns>
        List<StmtrptDomainModels.CustomerRecord> FetchCustomers();

        /// <summary>
        /// Processes the IMS GN loop for retrieving customer data.
        /// </summary>
        void ProcessImsGnLoop();

        /// <summary>
        /// Opens a database cursor for transaction processing.
        /// </summary>
        void OpenCursor();

        /// <summary>
        /// Computes the range of transactions for processing.
        /// </summary>
        void ComputeRange();

        /// <summary>
        /// Processes the fetch loop for retrieving transactions.
        /// </summary>
        void ProcessFetchLoop();

        /// <summary>
        /// Closes the database cursor after processing.
        /// </summary>
        void CloseCursor();
    }

    public interface IStatementFileRepository
    {
        /// <summary>
        /// Writes a statement file to the output.
        /// </summary>
        /// <param name="statementFile">The statement file to write.</param>
        void WriteStatementFile(StmtrptDomainModels.StatementFile statementFile);
    }

    public interface ITransactionLedgerRepository
    {
        /// <summary>
        /// Fetches transaction ledger records from the database.
        /// </summary>
        /// <returns>A list of transaction ledger records.</returns>
        List<StmtrptDomainModels.TransactionLedger> FetchTransactionLedgers();
    }

    public interface ICustomerRecordRepository
    {
        /// <summary>
        /// Fetches customer records from the database.
        /// </summary>
        /// <returns>A list of customer records.</returns>
        List<StmtrptDomainModels.CustomerRecord> FetchCustomerRecords();
    }

    public interface IImsService
    {
        /// <summary>
        /// Executes an IMS GN call with the specified function code.
        /// </summary>
        /// <param name="functionCode">The IMS function code to execute.</param>
        /// <returns>A customer record retrieved from IMS.</returns>
        StmtrptDomainModels.CustomerRecord ExecuteImsGn(string functionCode);
    }
}