using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BCHEOD.Application.Interfaces;
using BCHEOD.Domain;

namespace BCHEOD.Infrastructure
{
    public class BcheodDbContext : DbContext
    {
        public DbSet<EodRuns> EodRuns { get; set; }
        public DbSet<BcheodDomainModels.Policy> Policies { get; set; }
        public DbSet<BcheodDomainModels.Claim> Claims { get; set; }

        public BcheodDbContext(DbContextOptions<BcheodDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EodRuns>(entity =>
            {
                entity.ToTable("eod_runs", "public");

                entity.Property(e => e.StartedAt)
                    .HasColumnName("started_at")
                    .HasColumnType("timestamptz");
                // COBOL: STARTED_AT TIMESTAMPTZ

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("timestamptz");
                // COBOL: STATUS TIMESTAMPTZ

                entity.Property(e => e.EndedAt)
                    .HasColumnName("ended_at")
                    .HasColumnType("timestamptz");
                // COBOL: ENDED_AT TIMESTAMPTZ

                entity.Property(e => e.WsStatusCode)
                    .HasColumnName("ws_status_code")
                    .HasColumnType("timestamptz");
                // COBOL: WS_STATUS_CODE TIMESTAMPTZ
            });

            modelBuilder.Entity<BcheodDomainModels.Policy>(entity =>
            {
                entity.ToTable("policies", "public");

                entity.Property(e => e.PolicyNumber)
                    .HasColumnName("policy_number")
                    .HasMaxLength(20);
                // COBOL: POLICY_NUMBER PIC X(20)

                entity.Property(e => e.PremiumAmount)
                    .HasColumnName("premium_amount")
                    .HasPrecision(15, 2);
                // COBOL: PREMIUM_AMOUNT PIC S9(13)V99 COMP-3

                entity.Property(e => e.PolicyHolderName)
                    .HasColumnName("policy_holder_name")
                    .HasMaxLength(50);
                // COBOL: POLICY_HOLDER_NAME PIC X(50)
            });

            modelBuilder.Entity<BcheodDomainModels.Claim>(entity =>
            {
                entity.ToTable("claims", "public");

                entity.Property(e => e.ClaimNumber)
                    .HasColumnName("claim_number")
                    .HasMaxLength(20);
                // COBOL: CLAIM_NUMBER PIC X(20)

                entity.Property(e => e.ClaimAmount)
                    .HasColumnName("claim_amount")
                    .HasPrecision(15, 2);
                // COBOL: CLAIM_AMOUNT PIC S9(13)V99 COMP-3

                entity.Property(e => e.ClaimantName)
                    .HasColumnName("claimant_name")
                    .HasMaxLength(50);
                // COBOL: CLAIMANT_NAME PIC X(50)
            });
        }
    }

    public class EodRuns
    {
        public DateTime StartedAt { get; set; }
        public DateTime Status { get; set; }
        public DateTime EndedAt { get; set; }
        public DateTime WsStatusCode { get; set; }
    }

    public class PolicyRepository : IPolicyRepository
    {
        private readonly BcheodDbContext _context;

        public PolicyRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public BcheodDomainModels.Policy GetPolicyByNumber(string policyNumber)
        {
            return _context.Policies.SingleOrDefault(p => p.PolicyNumber == policyNumber);
        }

        public void SaveOrUpdatePolicy(BcheodDomainModels.Policy policy)
        {
            var existingPolicy = _context.Policies.SingleOrDefault(p => p.PolicyNumber == policy.PolicyNumber);
            if (existingPolicy != null)
            {
                _context.Entry(existingPolicy).CurrentValues.SetValues(policy);
            }
            else
            {
                _context.Policies.Add(policy);
            }
            _context.SaveChanges();
        }

        public void DeletePolicy(string policyNumber)
        {
            var policy = _context.Policies.SingleOrDefault(p => p.PolicyNumber == policyNumber);
            if (policy != null)
            {
                _context.Policies.Remove(policy);
                _context.SaveChanges();
            }
        }

        public IEnumerable<BcheodDomainModels.Policy> GetAllPolicies()
        {
            return _context.Policies.ToList();
        }

        public BcheodDomainModels.PolicyDetails GetPolicyDetails(string policyNumber)
        {
            var policy = _context.Policies.SingleOrDefault(p => p.PolicyNumber == policyNumber);
            if (policy == null) return null;

            return new BcheodDomainModels.PolicyDetails
            {
                PolicyNumber = policy.PolicyNumber,
                PolicyHolderName = policy.PolicyHolderName,
                PremiumAmount = policy.PremiumAmount
            };
        }
    }

    public class ClaimRepository : IClaimRepository
    {
        private readonly BcheodDbContext _context;

        public ClaimRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public BcheodDomainModels.Claim GetClaimByNumber(string claimNumber)
        {
            return _context.Claims.SingleOrDefault(c => c.ClaimNumber == claimNumber);
        }

        public void SaveOrUpdateClaim(BcheodDomainModels.Claim claim)
        {
            var existingClaim = _context.Claims.SingleOrDefault(c => c.ClaimNumber == claim.ClaimNumber);
            if (existingClaim != null)
            {
                _context.Entry(existingClaim).CurrentValues.SetValues(claim);
            }
            else
            {
                _context.Claims.Add(claim);
            }
            _context.SaveChanges();
        }

        public void DeleteClaim(string claimNumber)
        {
            var claim = _context.Claims.SingleOrDefault(c => c.ClaimNumber == claimNumber);
            if (claim != null)
            {
                _context.Claims.Remove(claim);
                _context.SaveChanges();
            }
        }

        public IEnumerable<BcheodDomainModels.Claim> GetAllClaims()
        {
            return _context.Claims.ToList();
        }

        public BcheodDomainModels.ClaimDetails GetClaimDetails(string claimNumber)
        {
            var claim = _context.Claims.SingleOrDefault(c => c.ClaimNumber == claimNumber);
            if (claim == null) return null;

            return new BcheodDomainModels.ClaimDetails
            {
                ClaimNumber = claim.ClaimNumber,
                ClaimantName = claim.ClaimantName,
                ClaimAmount = claim.ClaimAmount
            };
        }
    }

    public class PolicyProcessingService : IPolicyProcessingService
    {
        public bool ProcessPolicyApproval(BcheodDomainModels.Policy policy)
        {
            // Example logic for policy approval
            return policy.PremiumAmount > 0 && !string.IsNullOrEmpty(policy.PolicyHolderName);
        }

        public decimal CalculatePolicyPremium(BcheodDomainModels.Policy policy)
        {
            // Example logic for premium calculation
            return policy.PremiumAmount * 1.1m; // Adding 10% as an example
        }

        public bool ValidatePolicyDetails(BcheodDomainModels.PolicyDetails policyDetails)
        {
            // Example validation logic
            return !string.IsNullOrEmpty(policyDetails.PolicyNumber) &&
                   !string.IsNullOrEmpty(policyDetails.PolicyHolderName) &&
                   policyDetails.PremiumAmount > 0;
        }
    }

    public class ClaimProcessingService : IClaimProcessingService
    {
        public bool ProcessClaimSettlement(BcheodDomainModels.Claim claim)
        {
            // Example logic for claim settlement
            return claim.ClaimAmount > 0 && !string.IsNullOrEmpty(claim.ClaimantName);
        }

        public decimal CalculateClaimPayout(BcheodDomainModels.Claim claim)
        {
            // Example logic for claim payout calculation
            return claim.ClaimAmount * 0.9m; // Deducting 10% as an example
        }

        public bool ValidateClaimDetails(BcheodDomainModels.ClaimDetails claimDetails)
        {
            // Example validation logic
            return !string.IsNullOrEmpty(claimDetails.ClaimNumber) &&
                   !string.IsNullOrEmpty(claimDetails.ClaimantName) &&
                   claimDetails.ClaimAmount > 0;
        }
    }
}