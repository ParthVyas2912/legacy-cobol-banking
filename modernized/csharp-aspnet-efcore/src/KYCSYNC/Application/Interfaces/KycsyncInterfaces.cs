using System;
using System.Collections.Generic;
using KYCSYNC.Domain;

namespace KYCSYNC.Application.Interfaces
{
    public interface IKycsyncRepository
    {
        /// <summary>
        /// Reads customer data from the file.
        /// </summary>
        /// <returns>A list of customers.</returns>
        List<KycsyncDomainModels.Customer> ReadCustomerData();

        /// <summary>
        /// Writes customer data to the file.
        /// </summary>
        /// <param name="customers">The list of customers to write.</param>
        void WriteCustomerData(List<KycsyncDomainModels.Customer> customers);

        /// <summary>
        /// Reads account data from the file.
        /// </summary>
        /// <returns>A list of accounts.</returns>
        List<KycsyncDomainModels.Account> ReadAccountData();

        /// <summary>
        /// Writes account data to the file.
        /// </summary>
        /// <param name="accounts">The list of accounts to write.</param>
        void WriteAccountData(List<KycsyncDomainModels.Account> accounts);
    }

    public interface IKycsyncService
    {
        /// <summary>
        /// Synchronizes customer data with the external system.
        /// </summary>
        /// <param name="customers">The list of customers to synchronize.</param>
        /// <returns>A list of customers with updated statuses.</returns>
        List<KycsyncDomainModels.Customer> SynchronizeCustomerData(List<KycsyncDomainModels.Customer> customers);

        /// <summary>
        /// Logs synchronization results to the external system.
        /// </summary>
        /// <param name="logMessage">The log message to send.</param>
        void LogSynchronizationResult(string logMessage);

        /// <summary>
        /// Synchronizes account data with the external system.
        /// </summary>
        /// <param name="accounts">The list of accounts to synchronize.</param>
        /// <returns>A list of accounts with updated statuses.</returns>
        List<KycsyncDomainModels.Account> SynchronizeAccountData(List<KycsyncDomainModels.Account> accounts);

        /// <summary>
        /// Logs account synchronization results to the external system.
        /// </summary>
        /// <param name="logMessage">The log message to send.</param>
        void LogAccountSynchronizationResult(string logMessage);
    }

    public interface IKycsyncProcessor
    {
        /// <summary>
        /// Main processing method for the KYCSYNC program.
        /// </summary>
        void ProcessKycsync();

        /// <summary>
        /// Validates customer data before processing.
        /// </summary>
        /// <param name="customers">The list of customers to validate.</param>
        /// <returns>A boolean indicating whether the data is valid.</returns>
        bool ValidateCustomerData(List<KycsyncDomainModels.Customer> customers);

        /// <summary>
        /// Validates account data before processing.
        /// </summary>
        /// <param name="accounts">The list of accounts to validate.</param>
        /// <returns>A boolean indicating whether the data is valid.</returns>
        bool ValidateAccountData(List<KycsyncDomainModels.Account> accounts);
    }
}