using System.Collections.Generic;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    public interface IBcheodRepository
    {
        /// <summary>
        /// Reads a policy record by policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number to search for.</param>
        /// <returns>The policy record if found, otherwise null.</returns>
        BcheodDomainModels.Policy ReadPolicyByNumber(string policyNumber);

        /// <summary>
        /// Writes a policy record to the repository.
        /// </summary>
        /// <param name="policy">The policy record to write.</param>
        void WritePolicy(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Updates a policy record in the repository.
        /// </summary>
        /// <param name="policy">The policy record to update.</param>
        void UpdatePolicy(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Deletes a policy record by policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number of the record to delete.</param>
        void DeletePolicy(string policyNumber);

        /// <summary>
        /// Reads all policies for a specific agent.
        /// </summary>
        /// <param name="agentCode">The agent code to filter policies.</param>
        /// <returns>A list of policies associated with the given agent code.</returns>
        List<BcheodDomainModels.Policy> ReadPoliciesByAgent(string agentCode);
    }

    public interface IBcheodService
    {
        /// <summary>
        /// Processes a policy payment.
        /// </summary>
        /// <param name="policyNumber">The policy number for which the payment is being processed.</param>
        /// <param name="payment">The payment details.</param>
        void ProcessPolicyPayment(string policyNumber, BcheodDomainModels.PolicyPayment payment);

        /// <summary>
        /// Cancels a policy by policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number to cancel.</param>
        void CancelPolicy(string policyNumber);

        /// <summary>
        /// Activates a policy by policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number to activate.</param>
        void ActivatePolicy(string policyNumber);

        /// <summary>
        /// Retrieves the status of a policy.
        /// </summary>
        /// <param name="policyNumber">The policy number to check.</param>
        /// <returns>The current status of the policy.</returns>
        BcheodDomainModels.PolicyStatus GetPolicyStatus(string policyNumber);

        /// <summary>
        /// Generates a summary report for a specific agent.
        /// </summary>
        /// <param name="agentCode">The agent code for which the report is generated.</param>
        /// <returns>A summary report containing policy details.</returns>
        string GenerateAgentSummaryReport(string agentCode);
    }
}