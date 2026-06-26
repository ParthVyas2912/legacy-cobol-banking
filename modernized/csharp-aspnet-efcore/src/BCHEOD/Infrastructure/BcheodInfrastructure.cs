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
        public DbSet<Policy> Policies { get; set; }

        public BcheodDbContext(DbContextOptions<BcheodDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<Policy>(entity =>
            {
                entity.ToTable("policies", "public");

                entity.Property(e => e.PolicyNumber)
                    .HasColumnName("policy_number")
                    .HasMaxLength(20)
                    .IsRequired();
                // COBOL: POLICY_NUMBER PIC X(20)

                entity.Property(e => e.PolicyHolderName)
                    .HasColumnName("policy_holder_name")
                    .HasMaxLength(50)
                    .IsRequired();
                // COBOL: POLICY_HOLDER_NAME PIC X(50)

                entity.Property(e => e.EffectiveDate)
                    .HasColumnName("effective_date")
                    .HasColumnType("date")
                    .IsRequired();
                // COBOL: EFFECTIVE_DATE DATE

                entity.Property(e => e.ExpirationDate)
                    .HasColumnName("expiration_date")
                    .HasColumnType("date")
                    .IsRequired();
                // COBOL: EXPIRATION_DATE DATE

                entity.Property(e => e.PremiumAmount)
                    .HasColumnName("premium_amount")
                    .HasPrecision(15, 2)
                    .IsRequired();
                // COBOL: PREMIUM_AMOUNT PIC S9(13)V99 COMP-3
            });
        }
    }

    public class EodRuns
    {
        public DateTime? StartedAt { get; set; }
        public DateTime? Status { get; set; }
        public DateTime? EndedAt { get; set; }
        public DateTime? WsStatusCode { get; set; }
    }

    public class Policy
    {
        [Required]
        [MaxLength(20)]
        public string PolicyNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string PolicyHolderName { get; set; }

        [Required]
        public DateTime EffectiveDate { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [Precision(15, 2)]
        public decimal PremiumAmount { get; set; }
    }

    public class BcheodRepository : IBcheodRepository
    {
        private readonly BcheodDbContext _context;

        public BcheodRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public BcheodDomainModels.Policy ReadPolicy(string policyNumber)
        {
            var policyEntity = _context.Policies
                .FirstOrDefault(p => p.PolicyNumber == policyNumber);

            if (policyEntity == null)
            {
                throw new KeyNotFoundException($"Policy with number {policyNumber} not found.");
            }

            return new BcheodDomainModels.Policy
            {
                PolicyNumber = policyEntity.PolicyNumber,
                PolicyHolderName = policyEntity.PolicyHolderName,
                EffectiveDate = policyEntity.EffectiveDate,
                ExpirationDate = policyEntity.ExpirationDate,
                PremiumAmount = policyEntity.PremiumAmount
            };
        }

        public void WritePolicy(BcheodDomainModels.Policy policy)
        {
            var policyEntity = new Policy
            {
                PolicyNumber = policy.PolicyNumber,
                PolicyHolderName = policy.PolicyHolderName,
                EffectiveDate = policy.EffectiveDate,
                ExpirationDate = policy.ExpirationDate,
                PremiumAmount = policy.PremiumAmount
            };

            _context.Policies.Add(policyEntity);
            _context.SaveChanges();
        }

        public void DeletePolicy(string policyNumber)
        {
            var policyEntity = _context.Policies
                .FirstOrDefault(p => p.PolicyNumber == policyNumber);

            if (policyEntity == null)
            {
                throw new KeyNotFoundException($"Policy with number {policyNumber} not found.");
            }

            _context.Policies.Remove(policyEntity);
            _context.SaveChanges();
        }

        public void UpdatePolicy(BcheodDomainModels.Policy policy)
        {
            var policyEntity = _context.Policies
                .FirstOrDefault(p => p.PolicyNumber == policy.PolicyNumber);

            if (policyEntity == null)
            {
                throw new KeyNotFoundException($"Policy with number {policy.PolicyNumber} not found.");
            }

            policyEntity.PolicyHolderName = policy.PolicyHolderName;
            policyEntity.EffectiveDate = policy.EffectiveDate;
            policyEntity.ExpirationDate = policy.ExpirationDate;
            policyEntity.PremiumAmount = policy.PremiumAmount;

            _context.Policies.Update(policyEntity);
            _context.SaveChanges();
        }

        public List<BcheodDomainModels.Policy> GetAllPolicies()
        {
            return _context.Policies
                .Select(p => new BcheodDomainModels.Policy
                {
                    PolicyNumber = p.PolicyNumber,
                    PolicyHolderName = p.PolicyHolderName,
                    EffectiveDate = p.EffectiveDate,
                    ExpirationDate = p.ExpirationDate,
                    PremiumAmount = p.PremiumAmount
                })
                .ToList();
        }
    }
}