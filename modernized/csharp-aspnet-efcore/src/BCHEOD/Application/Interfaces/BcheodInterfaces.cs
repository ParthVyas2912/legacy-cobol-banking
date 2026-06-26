using System.Collections.Generic;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    public interface IBcheodRepository
    {
        /// <summary>
        /// Reads a policy record from the data store based on the policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number to search for.</param>
        /// <returns>The policy record if found, otherwise null.</returns>
        BcheodDomainModels.Policy ReadPolicy(string policyNumber);

        /// <summary>
        /// Writes a policy record to the data store.
        /// </summary>
        /// <param name="policy">The policy record to write.</param>
        void WritePolicy(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Deletes a policy record from the data store based on the policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number of the record to delete.</param>
        void DeletePolicy(string policyNumber);

        /// <summary>
        /// Updates an existing policy record in the data store.
        /// </summary>
        /// <param name="policy">The policy record with updated information.</param>
        void UpdatePolicy(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Retrieves all policies from the data store.
        /// </summary>
        /// <returns>A list of all policy records.</returns>
        List<BcheodDomainModels.Policy> GetAllPolicies();
    }

    public interface IBcheodService
    {
        /// <summary>
        /// Validates the policy data for correctness and business rules.
        /// </summary>
        /// <param name="policy">The policy data to validate.</param>
        /// <returns>A list of validation errors, or an empty list if the policy is valid.</returns>
        List<string> ValidatePolicy(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Calculates the premium for a given policy.
        /// </summary>
        /// <param name="policy">The policy for which to calculate the premium.</param>
        /// <returns>The calculated premium amount.</returns>
        decimal CalculatePremium(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Processes the policy for issuance.
        /// </summary>
        /// <param name="policy">The policy to process.</param>
        /// <returns>True if the policy was successfully issued, otherwise false.</returns>
        bool IssuePolicy(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Cancels an existing policy.
        /// </summary>
        /// <param name="policyNumber">The policy number of the policy to cancel.</param>
        /// <returns>True if the policy was successfully canceled, otherwise false.</returns>
        bool CancelPolicy(string policyNumber);

        /// <summary>
        /// Retrieves a list of all policies for a given insured.
        /// </summary>
        /// <param name="insuredName">The name of the insured.</param>
        /// <returns>A list of policies associated with the insured.</returns>
        List<BcheodDomainModels.Policy> GetPoliciesByInsured(string insuredName);

        /// <summary>
        /// Retrieves the coverage details for a specific policy.
        /// </summary>
        /// <param name="policyNumber">The policy number to retrieve coverage for.</param>
        /// <returns>A list of coverage details for the policy.</returns>
        List<BcheodDomainModels.Coverage> GetCoverageByPolicy(string policyNumber);

        /// <summary>
        /// Updates the coverage details for a specific policy.
        /// </summary>
        /// <param name="policyNumber">The policy number to update coverage for.</param>
        /// <param name="coverages">The list of updated coverage details.</param>
        void UpdateCoverage(string policyNumber, List<BcheodDomainModels.Coverage> coverages);
    }
}