using System.Collections.Generic;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    /// <summary>
    /// Interface for operations related to policies.
    /// </summary>
    public interface IPolicyRepository
    {
        /// <summary>
        /// Retrieves a policy by its policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number to search for.</param>
        /// <returns>The policy matching the given policy number.</returns>
        BcheodDomainModels.Policy GetPolicyByNumber(string policyNumber);

        /// <summary>
        /// Saves or updates a policy in the repository.
        /// </summary>
        /// <param name="policy">The policy to save or update.</param>
        void SaveOrUpdatePolicy(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Deletes a policy by its policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number of the policy to delete.</param>
        void DeletePolicy(string policyNumber);

        /// <summary>
        /// Retrieves all policies.
        /// </summary>
        /// <returns>A list of all policies.</returns>
        IEnumerable<BcheodDomainModels.Policy> GetAllPolicies();

        /// <summary>
        /// Retrieves detailed information about a policy by its policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number to search for.</param>
        /// <returns>The detailed information of the policy.</returns>
        BcheodDomainModels.PolicyDetails GetPolicyDetails(string policyNumber);
    }

    /// <summary>
    /// Interface for operations related to claims.
    /// </summary>
    public interface IClaimRepository
    {
        /// <summary>
        /// Retrieves a claim by its claim number.
        /// </summary>
        /// <param name="claimNumber">The claim number to search for.</param>
        /// <returns>The claim matching the given claim number.</returns>
        BcheodDomainModels.Claim GetClaimByNumber(string claimNumber);

        /// <summary>
        /// Saves or updates a claim in the repository.
        /// </summary>
        /// <param name="claim">The claim to save or update.</param>
        void SaveOrUpdateClaim(BcheodDomainModels.Claim claim);

        /// <summary>
        /// Deletes a claim by its claim number.
        /// </summary>
        /// <param name="claimNumber">The claim number of the claim to delete.</param>
        void DeleteClaim(string claimNumber);

        /// <summary>
        /// Retrieves all claims.
        /// </summary>
        /// <returns>A list of all claims.</returns>
        IEnumerable<BcheodDomainModels.Claim> GetAllClaims();

        /// <summary>
        /// Retrieves detailed information about a claim by its claim number.
        /// </summary>
        /// <param name="claimNumber">The claim number to search for.</param>
        /// <returns>The detailed information of the claim.</returns>
        BcheodDomainModels.ClaimDetails GetClaimDetails(string claimNumber);
    }

    /// <summary>
    /// Interface for CICS service operations related to policy processing.
    /// </summary>
    public interface IPolicyProcessingService
    {
        /// <summary>
        /// Processes a policy for approval.
        /// </summary>
        /// <param name="policy">The policy to process.</param>
        /// <returns>A boolean indicating whether the policy was approved.</returns>
        bool ProcessPolicyApproval(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Calculates the premium for a given policy.
        /// </summary>
        /// <param name="policy">The policy for which to calculate the premium.</param>
        /// <returns>The calculated premium amount.</returns>
        decimal CalculatePolicyPremium(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Validates the policy details for completeness and correctness.
        /// </summary>
        /// <param name="policyDetails">The policy details to validate.</param>
        /// <returns>A boolean indicating whether the policy details are valid.</returns>
        bool ValidatePolicyDetails(BcheodDomainModels.PolicyDetails policyDetails);
    }

    /// <summary>
    /// Interface for CICS service operations related to claim processing.
    /// </summary>
    public interface IClaimProcessingService
    {
        /// <summary>
        /// Processes a claim for settlement.
        /// </summary>
        /// <param name="claim">The claim to process.</param>
        /// <returns>A boolean indicating whether the claim was settled successfully.</returns>
        bool ProcessClaimSettlement(BcheodDomainModels.Claim claim);

        /// <summary>
        /// Calculates the payout amount for a given claim.
        /// </summary>
        /// <param name="claim">The claim for which to calculate the payout.</param>
        /// <returns>The calculated payout amount.</returns>
        decimal CalculateClaimPayout(BcheodDomainModels.Claim claim);

        /// <summary>
        /// Validates the claim details for completeness and correctness.
        /// </summary>
        /// <param name="claimDetails">The claim details to validate.</param>
        /// <returns>A boolean indicating whether the claim details are valid.</returns>
        bool ValidateClaimDetails(BcheodDomainModels.ClaimDetails claimDetails);
    }
}