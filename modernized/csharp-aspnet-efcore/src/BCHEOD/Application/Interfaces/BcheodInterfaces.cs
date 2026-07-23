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
        /// <returns>The policy details if found, otherwise null.</returns>
        BcheodDomainModels.Policy GetPolicyByNumber(string policyNumber);

        /// <summary>
        /// Saves or updates a policy.
        /// </summary>
        /// <param name="policy">The policy details to save or update.</param>
        void SavePolicy(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Deletes a policy by its policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number of the policy to delete.</param>
        void DeletePolicy(string policyNumber);
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
        /// <returns>The claim details if found, otherwise null.</returns>
        BcheodDomainModels.Claim GetClaimByNumber(string claimNumber);

        /// <summary>
        /// Saves or updates a claim.
        /// </summary>
        /// <param name="claim">The claim details to save or update.</param>
        void SaveClaim(BcheodDomainModels.Claim claim);

        /// <summary>
        /// Deletes a claim by its claim number.
        /// </summary>
        /// <param name="claimNumber">The claim number of the claim to delete.</param>
        void DeleteClaim(string claimNumber);
    }

    /// <summary>
    /// Interface for CICS service operations.
    /// </summary>
    public interface ICicsService
    {
        /// <summary>
        /// Executes a CICS transaction to process a policy.
        /// </summary>
        /// <param name="policy">The policy to process.</param>
        /// <returns>A status message indicating the result of the transaction.</returns>
        string ProcessPolicyTransaction(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Executes a CICS transaction to process a claim.
        /// </summary>
        /// <param name="claim">The claim to process.</param>
        /// <returns>A status message indicating the result of the transaction.</returns>
        string ProcessClaimTransaction(BcheodDomainModels.Claim claim);
    }
}