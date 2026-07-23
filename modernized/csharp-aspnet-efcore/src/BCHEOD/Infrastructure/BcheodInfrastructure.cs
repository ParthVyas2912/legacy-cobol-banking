using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BCHEOD.Domain;
using BCHEOD.Application.Interfaces;

namespace BCHEOD.Infrastructure
{
    public class BcheodDbContext : DbContext
    {
        public DbSet<EodRuns> EodRuns { get; set; }

        public BcheodDbContext(DbContextOptions<BcheodDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EodRuns>(entity =>
            {
                entity.ToTable("eod_runs", "public");

                entity.Property(e => e.StartedAt)
                    .HasColumnName("started_at")
                    .HasColumnType("timestamp with time zone");
                // COBOL: STARTED_AT PIC TIMESTAMPTZ

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("varchar")
                    .HasMaxLength(20);
                // COBOL: STATUS PIC X(20)

                entity.Property(e => e.EndedAt)
                    .HasColumnName("ended_at")
                    .HasColumnType("timestamp with time zone");
                // COBOL: ENDED_AT PIC TIMESTAMPTZ

                entity.Property(e => e.WsStatusCode)
                    .HasColumnName("ws_status_code")
                    .HasColumnType("integer");
                // COBOL: WS_STATUS_CODE PIC 9(5)
            });
        }
    }

    public class EodRuns
    {
        public DateTime StartedAt { get; set; }
        public string Status { get; set; }
        public DateTime EndedAt { get; set; }
        public int WsStatusCode { get; set; }
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
            // Simulated retrieval logic
            return _context.Set<BcheodDomainModels.Policy>().FirstOrDefault(p => p.PolicyNumber == policyNumber);
        }

        public void SavePolicy(BcheodDomainModels.Policy policy)
        {
            var existingPolicy = _context.Set<BcheodDomainModels.Policy>().FirstOrDefault(p => p.PolicyNumber == policy.PolicyNumber);
            if (existingPolicy != null)
            {
                _context.Entry(existingPolicy).CurrentValues.SetValues(policy);
            }
            else
            {
                _context.Set<BcheodDomainModels.Policy>().Add(policy);
            }
            _context.SaveChanges();
        }

        public void DeletePolicy(string policyNumber)
        {
            var policy = _context.Set<BcheodDomainModels.Policy>().FirstOrDefault(p => p.PolicyNumber == policyNumber);
            if (policy != null)
            {
                _context.Set<BcheodDomainModels.Policy>().Remove(policy);
                _context.SaveChanges();
            }
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
            // Simulated retrieval logic
            return _context.Set<BcheodDomainModels.Claim>().FirstOrDefault(c => c.ClaimNumber == claimNumber);
        }

        public void SaveClaim(BcheodDomainModels.Claim claim)
        {
            var existingClaim = _context.Set<BcheodDomainModels.Claim>().FirstOrDefault(c => c.ClaimNumber == claim.ClaimNumber);
            if (existingClaim != null)
            {
                _context.Entry(existingClaim).CurrentValues.SetValues(claim);
            }
            else
            {
                _context.Set<BcheodDomainModels.Claim>().Add(claim);
            }
            _context.SaveChanges();
        }

        public void DeleteClaim(string claimNumber)
        {
            var claim = _context.Set<BcheodDomainModels.Claim>().FirstOrDefault(c => c.ClaimNumber == claimNumber);
            if (claim != null)
            {
                _context.Set<BcheodDomainModels.Claim>().Remove(claim);
                _context.SaveChanges();
            }
        }
    }

    public class CicsService : ICicsService
    {
        public string ProcessPolicyTransaction(BcheodDomainModels.Policy policy)
        {
            // Simulated CICS transaction logic for policy
            return $"Policy {policy.PolicyNumber} processed successfully.";
        }

        public string ProcessClaimTransaction(BcheodDomainModels.Claim claim)
        {
            // Simulated CICS transaction logic for claim
            return $"Claim {claim.ClaimNumber} processed successfully.";
        }
    }
}