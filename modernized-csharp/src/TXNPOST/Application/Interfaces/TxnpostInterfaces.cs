using System;
using TXNPOST.Domain;

namespace TXNPOST.Application.Interfaces
{
    /// <summary>
    /// Interface for the main TXNPOST application logic.
    /// </summary>
    public interface ITxnpostApplication
    {
        /// <summary>
        /// Main entry point for the TXNPOST program.
        /// </summary>
        void Main();

        /// <summary>
        /// Opens the necessary files for processing transactions.
        /// </summary>
        void OpenFiles();

        /// <summary>
        /// Processes all transactions from the input file.
        /// </summary>
        void ProcessTransactions();

        /// <summary>
        /// Applies a single transaction to the system.
        /// </summary>
        /// <param name="transaction">The transaction record to apply.</param>
        void ApplyTransaction(TxnpostDomainModels.TransactionRecord transaction);

        /// <summary>
        /// Closes all files after processing is complete.
        /// </summary>
        void CloseFiles();
    }

    /// <summary>
    /// Interface for file repository operations related to transaction input.
    /// </summary>
    public interface ITransactionInputRepository
    {
        /// <summary>
        /// Reads all transactions from the input file.
        /// </summary>
        /// <returns>An array of transaction records.</returns>
        TxnpostDomainModels.TransactionRecord[] ReadTransactions();
    }

    /// <summary>
    /// Interface for file repository operations related to audit output.
    /// </summary>
    public interface IAuditOutputRepository
    {
        /// <summary>
        /// Writes an audit record to the output file.
        /// </summary>
        /// <param name="auditOutput">The audit record to write.</param>
        void WriteAuditRecord(TxnpostDomainModels.AuditOutput auditOutput);
    }

    /// <summary>
    /// Interface for database operations related to the transaction ledger.
    /// </summary>
    public interface ITransactionLedgerService
    {
        /// <summary>
        /// Updates the transaction ledger with a new transaction.
        /// </summary>
        /// <param name="transaction">The transaction record to update.</param>
        void UpdateLedger(TxnpostDomainModels.TransactionRecord transaction);
    }

    /// <summary>
    /// Interface for managing working storage fields.
    /// </summary>
    public interface IWorkingStorageService
    {
        /// <summary>
        /// Updates the record count in working storage.
        /// </summary>
        /// <param name="count">The new record count value.</param>
        void UpdateRecordCount(decimal count);

        /// <summary>
        /// Updates the fail count in working storage.
        /// </summary>
        /// <param name="count">The new fail count value.</param>
        void UpdateFailCount(decimal count);
    }
}