using System;
using System.Collections.Generic;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    /// <summary>
    /// Interface for repository operations related to account file I/O.
    /// </summary>
    public interface IAccountFileRepository
    {
        /// <summary>
        /// Reads an account record from the account file.
        /// </summary>
        /// <param name="accountNumber">The account number to read.</param>
        /// <returns>The account record.</returns>
        AcctmgrDomainModels.AccountRecord ReadAccount(string accountNumber);

        /// <summary>
        /// Writes an account record to the account file.
        /// </summary>
        /// <param name="accountRecord">The account record to write.</param>
        void WriteAccount(AcctmgrDomainModels.AccountRecord accountRecord);

        /// <summary>
        /// Rewrites an account record in the account file.
        /// </summary>
        /// <param name="accountRecord">The account record to rewrite.</param>
        void RewriteAccount(AcctmgrDomainModels.AccountRecord accountRecord);
    }

    /// <summary>
    /// Interface for CICS service operations.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Returns control to the CICS environment.
        /// </summary>
        void Return();

        /// <summary>
        /// Writes a record to the CICS environment.
        /// </summary>
        /// <param name="record">The record to write.</param>
        void WriteRecord(AcctmgrDomainModels.AccountRecord record);

        /// <summary>
        /// Reads a record from the CICS environment.
        /// </summary>
        /// <param name="accountNumber">The account number to read.</param>
        /// <returns>The account record.</returns>
        AcctmgrDomainModels.AccountRecord ReadRecord(string accountNumber);

        /// <summary>
        /// Rewrites a record in the CICS environment.
        /// </summary>
        /// <param name="record">The record to rewrite.</param>
        void RewriteRecord(AcctmgrDomainModels.AccountRecord record);
    }

    /// <summary>
    /// Interface for the ACCTMGR application logic.
    /// </summary>
    public interface IAcctmgrService
    {
        /// <summary>
        /// Initializes the application.
        /// </summary>
        void Start();

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="accountRecord">The account record to create.</param>
        void CreateAccount(AcctmgrDomainModels.AccountRecord accountRecord);

        /// <summary>
        /// Reads an account.
        /// </summary>
        /// <param name="accountNumber">The account number to read.</param>
        /// <returns>The account record.</returns>
        AcctmgrDomainModels.AccountRecord ReadAccount(string accountNumber);

        /// <summary>
        /// Updates an existing account.
        /// </summary>
        /// <param name="accountRecord">The account record to update.</param>
        void UpdateAccount(AcctmgrDomainModels.AccountRecord accountRecord);

        /// <summary>
        /// Applies updates to an account.
        /// </summary>
        /// <param name="accountRecord">The account record with updates to apply.</param>
        void ApplyUpdates(AcctmgrDomainModels.AccountRecord accountRecord);

        /// <summary>
        /// Closes an account.
        /// </summary>
        /// <param name="accountNumber">The account number to close.</param>
        void CloseAccount(string accountNumber);
    }
}