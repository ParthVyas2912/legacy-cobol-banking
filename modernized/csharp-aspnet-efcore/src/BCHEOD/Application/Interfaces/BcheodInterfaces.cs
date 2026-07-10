using System;
using System.Collections.Generic;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    /// <summary>
    /// Interface for BCHEOD application logic.
    /// Each method corresponds to a COBOL paragraph.
    /// </summary>
    public interface IBcheodApplicationService
    {
        /// <summary>
        /// Initializes the application environment.
        /// </summary>
        void InitializeEnvironment();

        /// <summary>
        /// Processes the policy data.
        /// </summary>
        /// <param name="policy">The policy data to process.</param>
        void ProcessPolicy(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Validates the policy details.
        /// </summary>
        /// <param name="policy">The policy to validate.</param>
        /// <returns>True if the policy is valid, otherwise false.</returns>
        bool ValidatePolicy(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Calculates the premium for a given policy.
        /// </summary>
        /// <param name="policy">The policy for which to calculate the premium.</param>
        /// <returns>The calculated premium amount.</returns>
        decimal CalculatePremium(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Updates the policy status.
        /// </summary>
        /// <param name="policyNumber">The policy number to update.</param>
        /// <param name="status">The new status to set.</param>
        void UpdatePolicyStatus(string policyNumber, BcheodDomainModels.PolicyStatus status);

        /// <summary>
        /// Generates a report for the policy.
        /// </summary>
        /// <param name="policy">The policy for which to generate the report.</param>
        /// <returns>A string containing the report details.</returns>
        string GeneratePolicyReport(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Handles end-of-day processing for policies.
        /// </summary>
        void EndOfDayProcessing();

        /// <summary>
        /// Archives policies that meet archival criteria.
        /// </summary>
        void ArchivePolicies();
    }

    /// <summary>
    /// Interface for repository operations related to policy data.
    /// </summary>
    public interface IPolicyRepository
    {
        /// <summary>
        /// Retrieves a policy by its policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number to retrieve.</param>
        /// <returns>The policy data.</returns>
        BcheodDomainModels.Policy GetPolicyByNumber(string policyNumber);

        /// <summary>
        /// Saves a policy to the repository.
        /// </summary>
        /// <param name="policy">The policy to save.</param>
        void SavePolicy(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Deletes a policy by its policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number to delete.</param>
        void DeletePolicy(string policyNumber);

        /// <summary>
        /// Retrieves all policies that meet certain criteria.
        /// </summary>
        /// <param name="criteria">The criteria for retrieving policies.</param>
        /// <returns>A list of policies that match the criteria.</returns>
        List<BcheodDomainModels.Policy> GetPoliciesByCriteria(object criteria);
    }

    /// <summary>
    /// Interface for CICS operations related to policy processing.
    /// </summary>
    public interface IPolicyCicsService
    {
        /// <summary>
        /// Sends a policy update to the CICS system.
        /// </summary>
        /// <param name="policy">The policy to update.</param>
        void SendPolicyUpdate(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Retrieves policy status from the CICS system.
        /// </summary>
        /// <param name="policyNumber">The policy number to check.</param>
        /// <returns>The current status of the policy.</returns>
        BcheodDomainModels.PolicyStatus GetPolicyStatus(string policyNumber);

        /// <summary>
        /// Synchronizes policy data with the CICS system.
        /// </summary>
        void SynchronizePolicyData();
    }
}