using System.Threading.Tasks;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    public interface IFrdchkService
    {
        /// <summary>
        /// Main entry point for the FRDCHK program.
        /// </summary>
        Task ExecuteAsync();
    }

    public interface ITransactionRepository
    {
        /// <summary>
        /// Reads a transaction record from the input file.
        /// </summary>
        /// <returns>A transaction record.</returns>
        Task<FrdchkDomainModels.TransactionRecord> ReadTransactionAsync();

        /// <summary>
        /// Writes a transaction record to the output file.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to write.</param>
        Task WriteTransactionAsync(FrdchkDomainModels.TransactionRecord transactionRecord);
    }

    public interface ICicsService
    {
        /// <summary>
        /// Performs a CICS operation to validate a transaction.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to validate.</param>
        /// <returns>The validation result as a boolean.</returns>
        Task<bool> ValidateTransactionAsync(FrdchkDomainModels.TransactionRecord transactionRecord);

        /// <summary>
        /// Performs a CICS operation to update the transaction status.
        /// </summary>
        /// <param name="transactionRecord">The transaction record to update.</param>
        Task UpdateTransactionStatusAsync(FrdchkDomainModels.TransactionRecord transactionRecord);
    }
}