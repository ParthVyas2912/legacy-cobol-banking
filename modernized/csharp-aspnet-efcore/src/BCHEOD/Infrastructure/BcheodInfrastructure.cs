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
        public DbSet<BcheodDomainModels.Insured> Insureds { get; set; }

        public BcheodDbContext(DbContextOptions<BcheodDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EodRuns>(entity =>
            {
                entity.ToTable("eod_runs", "public");

                entity.Property(e => e.StartedAt)
                    .HasColumnName("started_at")
                    .HasColumnType("TIMESTAMPTZ");
                // COBOL: STARTED_AT TIMESTAMPTZ

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("TIMESTAMPTZ");
                // COBOL: STATUS TIMESTAMPTZ

                entity.Property(e => e.EndedAt)
                    .HasColumnName("ended_at")
                    .HasColumnType("TIMESTAMPTZ");
                // COBOL: ENDED_AT TIMESTAMPTZ

                entity.Property(e => e.WsStatusCode)
                    .HasColumnName("ws_status_code")
                    .HasColumnType("TIMESTAMPTZ");
                // COBOL: WS_STATUS_CODE TIMESTAMPTZ
            });

            modelBuilder.Entity<BcheodDomainModels.Policy>(entity =>
            {
                entity.ToTable("policies", "public");

                entity.HasKey(e => e.PolicyNumber);

                entity.Property(e => e.PolicyNumber)
                    .HasColumnName("policy_number")
                    .HasMaxLength(10);
                // COBOL: POLICY_NUMBER PIC X(10)

                entity.Property(e => e.PolicyHolderName)
                    .HasColumnName("policy_holder_name")
                    .HasMaxLength(30);
                // COBOL: POLICY_HOLDER_NAME PIC X(30)

                entity.Property(e => e.PolicyAmount)
                    .HasColumnName("policy_amount")
                    .HasPrecision(9, 2);
                // COBOL: POLICY_AMOUNT PIC 9(7)V99 COMP-3

                entity.Property(e => e.PolicyStartDate)
                    .HasColumnName("policy_start_date")
                    .HasColumnType("DATE");
                // COBOL: POLICY_START_DATE PIC 9(8)

                entity.Property(e => e.PolicyEndDate)
                    .HasColumnName("policy_end_date")
                    .HasColumnType("DATE");
                // COBOL: POLICY_END_DATE PIC 9(8)

                entity.Property(e => e.Status)
                    .HasColumnName("status");
                // COBOL: STATUS (88-level equivalent)
            });

            modelBuilder.Entity<BcheodDomainModels.Claim>(entity =>
            {
                entity.ToTable("claims", "public");

                entity.HasKey(e => e.ClaimNumber);

                entity.Property(e => e.ClaimNumber)
                    .HasColumnName("claim_number")
                    .HasMaxLength(15);
                // COBOL: CLAIM_NUMBER PIC X(15)

                entity.Property(e => e.ClaimAmount)
                    .HasColumnName("claim_amount")
                    .HasPrecision(11, 2);
                // COBOL: CLAIM_AMOUNT PIC 9(9)V99 COMP-3

                entity.Property(e => e.ClaimDate)
                    .HasColumnName("claim_date")
                    .HasColumnType("DATE");
                // COBOL: CLAIM_DATE PIC 9(8)

                entity.Property(e => e.Status)
                    .HasColumnName("status");
                // COBOL: STATUS (88-level equivalent)
            });

            modelBuilder.Entity<BcheodDomainModels.Insured>(entity =>
            {
                entity.ToTable("insureds", "public");

                entity.HasKey(e => e.InsuredId);

                entity.Property(e => e.InsuredId)
                    .HasColumnName("insured_id")
                    .HasMaxLength(20);
                // COBOL: INSURED_ID PIC X(20)

                entity.Property(e => e.InsuredName)
                    .HasColumnName("insured_name")
                    .HasMaxLength(30);
                // COBOL: INSURED_NAME PIC X(30)

                entity.OwnsOne(e => e.InsuredAddress, address =>
                {
                    address.Property(a => a.Street)
                        .HasColumnName("street")
                        .HasMaxLength(50);
                    // COBOL: STREET PIC X(50)

                    address.Property(a => a.City)
                        .HasColumnName("city")
                        .HasMaxLength(30);
                    // COBOL: CITY PIC X(30)

                    address.Property(a => a.State)
                        .HasColumnName("state")
                        .HasMaxLength(20);
                    // COBOL: STATE PIC X(20)

                    address.Property(a => a.ZipCode)
                        .HasColumnName("zip_code")
                        .HasMaxLength(10);
                    // COBOL: ZIP_CODE PIC X(10)
                });
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

    public class BcheodPolicyRepository : IBcheodPolicyRepository
    {
        private readonly BcheodDbContext _context;

        public BcheodPolicyRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public BcheodDomainModels.Policy GetPolicyByNumber(string policyNumber)
        {
            return _context.Policies.SingleOrDefault(p => p.PolicyNumber == policyNumber);
        }

        public void SavePolicy(BcheodDomainModels.Policy policy)
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
    }

    public class BcheodClaimRepository : IBcheodClaimRepository
    {
        private readonly BcheodDbContext _context;

        public BcheodClaimRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public List<BcheodDomainModels.Claim> GetClaimsByPolicyNumber(string policyNumber)
        {
            return _context.Claims.Where(c => c.PolicyNumber == policyNumber).ToList();
        }

        public void SaveClaim(BcheodDomainModels.Claim claim)
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
    }

    public class BcheodInsuredRepository : IBcheodInsuredRepository
    {
        private readonly BcheodDbContext _context;

        public BcheodInsuredRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public BcheodDomainModels.Insured GetInsuredById(string insuredId)
        {
            return _context.Insureds.SingleOrDefault(i => i.InsuredId == insuredId);
        }

        public void SaveInsured(BcheodDomainModels.Insured insured)
        {
            var existingInsured = _context.Insureds.SingleOrDefault(i => i.InsuredId == insured.InsuredId);
            if (existingInsured != null)
            {
                _context.Entry(existingInsured).CurrentValues.SetValues(insured);
            }
            else
            {
                _context.Insureds.Add(insured);
            }
            _context.SaveChanges();
        }

        public void DeleteInsured(string insuredId)
        {
            var insured = _context.Insureds.SingleOrDefault(i => i.InsuredId == insuredId);
            if (insured != null)
            {
                _context.Insureds.Remove(insured);
                _context.SaveChanges();
            }
        }
    }
}