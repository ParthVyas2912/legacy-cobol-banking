using System;
using System.Collections.Generic;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    public interface IBcheodPolicyRepository
    {
        /// <summary>
        /// Reads a policy by its policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number to search for.</param>
        /// <returns>The policy details if found, otherwise null.</returns>
        BcheodDomainModels.Policy GetPolicyByNumber(string policyNumber);

        /// <summary>
        /// Saves or updates a policy in the repository.
        /// </summary>
        /// <param name="policy">The policy to save or update.</param>
        void SavePolicy(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Deletes a policy by its policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number of the policy to delete.</param>
        void DeletePolicy(string policyNumber);
    }

    public interface IBcheodClaimRepository
    {
        /// <summary>
        /// Retrieves all claims associated with a specific policy number.
        /// </summary>
        /// <param name="policyNumber">The policy number to retrieve claims for.</param>
        /// <returns>A list of claims associated with the policy.</returns>
        List<BcheodDomainModels.Claim> GetClaimsByPolicyNumber(string policyNumber);

        /// <summary>
        /// Saves or updates a claim in the repository.
        /// </summary>
        /// <param name="claim">The claim to save or update.</param>
        void SaveClaim(BcheodDomainModels.Claim claim);

        /// <summary>
        /// Deletes a claim by its claim number.
        /// </summary>
        /// <param name="claimNumber">The claim number of the claim to delete.</param>
        void DeleteClaim(string claimNumber);
    }

    public interface IBcheodInsuredRepository
    {
        /// <summary>
        /// Retrieves insured details by their ID.
        /// </summary>
        /// <param name="insuredId">The ID of the insured to retrieve.</param>
        /// <returns>The insured details if found, otherwise null.</returns>
        BcheodDomainModels.Insured GetInsuredById(string insuredId);

        /// <summary>
        /// Saves or updates insured details in the repository.
        /// </summary>
        /// <param name="insured">The insured details to save or update.</param>
        void SaveInsured(BcheodDomainModels.Insured insured);

        /// <summary>
        /// Deletes an insured by their ID.
        /// </summary>
        /// <param name="insuredId">The ID of the insured to delete.</param>
        void DeleteInsured(string insuredId);
    }

    public interface IBcheodPolicyService
    {
        /// <summary>
        /// Validates a policy based on business rules.
        /// </summary>
        /// <param name="policy">The policy to validate.</param>
        /// <returns>True if the policy is valid, otherwise false.</returns>
        bool ValidatePolicy(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Calculates the premium for a given policy.
        /// </summary>
        /// <param name="policy">The policy to calculate the premium for.</param>
        /// <returns>The calculated premium amount.</returns>
        decimal CalculatePremium(BcheodDomainModels.Policy policy);

        /// <summary>
        /// Processes the renewal of a policy.
        /// </summary>
        /// <param name="policyNumber">The policy number to renew.</param>
        /// <returns>The updated policy after renewal.</returns>
        BcheodDomainModels.Policy RenewPolicy(string policyNumber);

        /// <summary>
        /// Retrieves detailed policy information, including associated claims.
        /// </summary>
        /// <param name="policyNumber">The policy number to retrieve details for.</param>
        /// <returns>The detailed policy information including claims.</returns>
        BcheodDomainModels.PolicyDetails GetPolicyDetails(string policyNumber);
    }

    public interface IBcheodClaimService
    {
        /// <summary>
        /// Validates a claim based on business rules.
        /// </summary>
        /// <param name="claim">The claim to validate.</param>
        /// <returns>True if the claim is valid, otherwise false.</returns>
        bool ValidateClaim(BcheodDomainModels.Claim claim);

        /// <summary>
        /// Processes a claim for a specific policy.
        /// </summary>
        /// <param name="claim">The claim to process.</param>
        /// <returns>The updated claim after processing.</returns>
        BcheodDomainModels.Claim ProcessClaim(BcheodDomainModels.Claim claim);

        /// <summary>
        /// Calculates the total claim amount for a specific policy.
        /// </summary>
        /// <param name="policyNumber">The policy number to calculate claims for.</param>
        /// <returns>The total claim amount.</returns>
        decimal CalculateTotalClaimAmount(string policyNumber);

        /// <summary>
        /// Retrieves claim details by its claim number.
        /// </summary>
        /// <param name="claimNumber">The claim number to retrieve.</param>
        /// <returns>The claim details if found, otherwise null.</returns>
        BcheodDomainModels.Claim GetClaimByNumber(string claimNumber);
    }

    public interface IBcheodInsuredService
    {
        /// <summary>
        /// Validates insured details based on business rules.
        /// </summary>
        /// <param name="insured">The insured details to validate.</param>
        /// <returns>True if the insured details are valid, otherwise false.</returns>
        bool ValidateInsured(BcheodDomainModels.Insured insured);

        /// <summary>
        /// Updates the address of an insured.
        /// </summary>
        /// <param name="insuredId">The ID of the insured to update.</param>
        /// <param name="newAddress">The new address to set.</param>
        void UpdateInsuredAddress(string insuredId, BcheodDomainModels.Address newAddress);

        /// <summary>
        /// Retrieves all policies associated with a specific insured ID.
        /// </summary>
        /// <param name="insuredId">The ID of the insured to retrieve policies for.</param>
        /// <returns>A list of policies associated with the insured.</returns>
        List<BcheodDomainModels.Policy> GetPoliciesByInsuredId(string insuredId);
    }
}