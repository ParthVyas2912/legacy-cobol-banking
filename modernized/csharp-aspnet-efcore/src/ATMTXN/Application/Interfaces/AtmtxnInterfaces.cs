using System;
using ATMTXN.Domain;

namespace ATMTXN.Application.Interfaces
{
    public interface IAtmtxnRepository
    {
        /// <summary>
        /// Reads a transaction record by its Transaction ID.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction.</param>
        /// <returns>The transaction details if found, otherwise null.</returns>
        AtmtxnDomainModels.Transaction ReadTransactionById(string transactionId);

        /// <summary>
        /// Writes a new transaction record to the data store.
        /// </summary>
        /// <param name="transaction">The transaction details to be written.</param>
        void WriteTransaction(AtmtxnDomainModels.Transaction transaction);

        /// <summary>
        /// Updates an existing transaction record in the data store.
        /// </summary>
        /// <param name="transaction">The transaction details to be updated.</param>
        void UpdateTransaction(AtmtxnDomainModels.Transaction transaction);

        /// <summary>
        /// Deletes a transaction record by its Transaction ID.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction to be deleted.</param>
        void DeleteTransaction(string transactionId);
    }

    public interface IAtmtxnService
    {
        /// <summary>
        /// Processes a withdrawal transaction.
        /// </summary>
        /// <param name="transaction">The transaction details for the withdrawal.</param>
        /// <returns>True if the transaction was successful, otherwise false.</returns>
        bool ProcessWithdrawal(AtmtxnDomainModels.Transaction transaction);

        /// <summary>
        /// Processes a deposit transaction.
        /// </summary>
        /// <param name="transaction">The transaction details for the deposit.</param>
        /// <returns>True if the transaction was successful, otherwise false.</returns>
        bool ProcessDeposit(AtmtxnDomainModels.Transaction transaction);

        /// <summary>
        /// Processes a transfer transaction.
        /// </summary>
        /// <param name="transaction">The transaction details for the transfer.</param>
        /// <returns>True if the transaction was successful, otherwise false.</returns>
        bool ProcessTransfer(AtmtxnDomainModels.Transaction transaction);

        /// <summary>
        /// Validates the account details for a transaction.
        /// </summary>
        /// <param name="accountDetails">The account details to be validated.</param>
        /// <returns>True if the account details are valid, otherwise false.</returns>
        bool ValidateAccount(AtmtxnDomainModels.AccountDetails accountDetails);

        /// <summary>
        /// Validates the terminal details for a transaction.
        /// </summary>
        /// <param name="terminalDetails">The terminal details to be validated.</param>
        /// <returns>True if the terminal details are valid, otherwise false.</returns>
        bool ValidateTerminal(AtmtxnDomainModels.TerminalDetails terminalDetails);
    }
}