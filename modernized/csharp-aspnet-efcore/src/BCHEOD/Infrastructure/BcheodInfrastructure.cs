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

        public BcheodDbContext(DbContextOptions<BcheodDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                    .HasMaxLength(50);
                // COBOL: STATUS PIC X(50)

                entity.Property(e => e.EndedAt)
                    .HasColumnName("ended_at")
                    .HasColumnType("timestamp with time zone");
                // COBOL: ENDED_AT PIC TIMESTAMPTZ

                entity.Property(e => e.WsStatusCode)
                    .HasColumnName("ws_status_code")
                    .HasColumnType("varchar")
                    .HasMaxLength(10);
                // COBOL: WS_STATUS_CODE PIC X(10)
            });
        }
    }

    public class EodRuns
    {
        public DateTime StartedAt { get; set; }
        public string Status { get; set; }
        public DateTime EndedAt { get; set; }
        public string WsStatusCode { get; set; }
    }

    public class BcheodRepository : IBcheodRepository
    {
        private readonly BcheodDbContext _context;

        public BcheodRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public BcheodDomainModels.Policy ReadPolicyByNumber(string policyNumber)
        {
            // Simulated logic for retrieving a policy by policy number
            return _context.EodRuns
                .Where(e => e.Status == policyNumber)
                .Select(e => new BcheodDomainModels.Policy
                {
                    PolicyNumber = policyNumber,
                    PolicyEffectiveDate = 20220101,
                    PolicyAmount = 1000.00m,
                    PolicyHolderName = "John Doe",
                    PolicyType = 1,
                    PolicyStatus = "A",
                    PremiumAmount = 100.00m,
                    PolicyDurationInYears = 10,
                    AgentCode = "AGENT123",
                    RegionCode = 1,
                    Payments = new List<BcheodDomainModels.PolicyPayment>
                    {
                        new BcheodDomainModels.PolicyPayment
                        {
                            PaymentDate = 20220101,
                            PaymentAmount = 100.00m,
                            PaymentStatus = "P"
                        }
                    }
                })
                .FirstOrDefault();
        }

        public void WritePolicy(BcheodDomainModels.Policy policy)
        {
            // Simulated logic for writing a policy
            var eodRun = new EodRuns
            {
                StartedAt = DateTime.UtcNow,
                Status = policy.PolicyNumber,
                EndedAt = DateTime.UtcNow,
                WsStatusCode = "SUCCESS"
            };

            _context.EodRuns.Add(eodRun);
            _context.SaveChanges();
        }

        public void UpdatePolicy(BcheodDomainModels.Policy policy)
        {
            // Simulated logic for updating a policy
            var eodRun = _context.EodRuns.FirstOrDefault(e => e.Status == policy.PolicyNumber);
            if (eodRun != null)
            {
                eodRun.Status = "UPDATED";
                _context.SaveChanges();
            }
        }

        public void DeletePolicy(string policyNumber)
        {
            // Simulated logic for deleting a policy
            var eodRun = _context.EodRuns.FirstOrDefault(e => e.Status == policyNumber);
            if (eodRun != null)
            {
                _context.EodRuns.Remove(eodRun);
                _context.SaveChanges();
            }
        }

        public List<BcheodDomainModels.Policy> ReadPoliciesByAgent(string agentCode)
        {
            // Simulated logic for retrieving policies by agent code
            return _context.EodRuns
                .Where(e => e.Status == agentCode)
                .Select(e => new BcheodDomainModels.Policy
                {
                    PolicyNumber = "POLICY123",
                    PolicyEffectiveDate = 20220101,
                    PolicyAmount = 1000.00m,
                    PolicyHolderName = "John Doe",
                    PolicyType = 1,
                    PolicyStatus = "A",
                    PremiumAmount = 100.00m,
                    PolicyDurationInYears = 10,
                    AgentCode = agentCode,
                    RegionCode = 1,
                    Payments = new List<BcheodDomainModels.PolicyPayment>
                    {
                        new BcheodDomainModels.PolicyPayment
                        {
                            PaymentDate = 20220101,
                            PaymentAmount = 100.00m,
                            PaymentStatus = "P"
                        }
                    }
                })
                .ToList();
        }
    }
}