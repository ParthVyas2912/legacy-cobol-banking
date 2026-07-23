using System;
using System.Collections.Generic;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    /// <summary>
    /// Interface for file repository operations related to customer records.
    /// </summary>
    public interface ICustomerRecordRepository
    {
        /// <summary>
        /// Reads a customer record by customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID to search for.</param>
        /// <returns>The customer record if found; otherwise, null.</returns>
        FrdchkDomainModels.CustomerRecord ReadCustomerRecord(string customerId);

        /// <summary>
        /// Writes a customer record to the repository.
        /// </summary>
        /// <param name="customerRecord">The customer record to write.</param>
        void WriteCustomerRecord(FrdchkDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Deletes a customer record by customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID of the record to delete.</param>
        void DeleteCustomerRecord(string customerId);

        /// <summary>
        /// Updates an existing customer record in the repository.
        /// </summary>
        /// <param name="customerRecord">The customer record to update.</param>
        void UpdateCustomerRecord(FrdchkDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Checks if a customer record exists by customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID to check.</param>
        /// <returns>True if the customer record exists; otherwise, false.</returns>
        bool CustomerRecordExists(string customerId);
    }

    /// <summary>
    /// Interface for CICS service operations related to fraud checks.
    /// </summary>
    public interface IFraudCheckService
    {
        /// <summary>
        /// Performs a fraud check for a given customer record.
        /// </summary>
        /// <param name="customerRecord">The customer record to check.</param>
        /// <returns>True if the record passes the fraud check; otherwise, false.</returns>
        bool PerformFraudCheck(FrdchkDomainModels.CustomerRecord customerRecord);

        /// <summary>
        /// Logs a fraud alert for a given customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID associated with the fraud alert.</param>
        /// <param name="alertMessage">The alert message to log.</param>
        void LogFraudAlert(string customerId, string alertMessage);

        /// <summary>
        /// Retrieves fraud history for a given customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID to retrieve fraud history for.</param>
        /// <returns>A list of fraud-related transactions or alerts.</returns>
        List<FrdchkDomainModels.Transaction> GetFraudHistory(string customerId);

        /// <summary>
        /// Escalates a fraud case for further investigation.
        /// </summary>
        /// <param name="customerId">The customer ID associated with the fraud case.</param>
        /// <param name="caseDetails">Details of the fraud case to escalate.</param>
        void EscalateFraudCase(string customerId, string caseDetails);

        /// <summary>
        /// Retrieves the status of a fraud case for a given customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID to retrieve the fraud case status for.</param>
        /// <returns>The status of the fraud case.</returns>
        string GetFraudCaseStatus(string customerId);
    }
}