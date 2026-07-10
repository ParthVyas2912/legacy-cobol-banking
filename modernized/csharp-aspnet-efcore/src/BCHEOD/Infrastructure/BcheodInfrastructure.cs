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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure the database connection string here
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EodRuns>(entity =>
            {
                entity.ToTable("eod_runs", "public");

                entity.Property(e => e.StartedAt)
                    .HasColumnName("started_at")
                    .HasColumnType("timestamptz")
                    .IsRequired(); // COBOL: STARTED_AT TIMESTAMPTZ

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("timestamptz")
                    .IsRequired(); // COBOL: STATUS TIMESTAMPTZ

                entity.Property(e => e.EndedAt)
                    .HasColumnName("ended_at")
                    .HasColumnType("timestamptz")
                    .IsRequired(); // COBOL: ENDED_AT TIMESTAMPTZ

                entity.Property(e => e.WsStatusCode)
                    .HasColumnName("ws_status_code")
                    .HasColumnType("timestamptz")
                    .IsRequired(); // COBOL: WS_STATUS_CODE TIMESTAMPTZ
            });
        }
    }

    public class EodRuns
    {
        public DateTime StartedAt { get; set; } // COBOL: STARTED_AT TIMESTAMPTZ
        public DateTime Status { get; set; } // COBOL: STATUS TIMESTAMPTZ
        public DateTime EndedAt { get; set; } // COBOL: ENDED_AT TIMESTAMPTZ
        public DateTime WsStatusCode { get; set; } // COBOL: WS_STATUS_CODE TIMESTAMPTZ
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
            // Retrieve a policy by its policy number
            return _context.Set<BcheodDomainModels.Policy>()
                           .FirstOrDefault(p => p.PolicyNumber == policyNumber);
        }

        public void SavePolicy(BcheodDomainModels.Policy policy)
        {
            // Save a policy to the repository
            var existingPolicy = _context.Set<BcheodDomainModels.Policy>()
                                         .FirstOrDefault(p => p.PolicyNumber == policy.PolicyNumber);

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
            // Delete a policy by its policy number
            var policy = _context.Set<BcheodDomainModels.Policy>()
                                 .FirstOrDefault(p => p.PolicyNumber == policyNumber);

            if (policy != null)
            {
                _context.Set<BcheodDomainModels.Policy>().Remove(policy);
                _context.SaveChanges();
            }
        }

        public List<BcheodDomainModels.Policy> GetPoliciesByCriteria(object criteria)
        {
            // Retrieve policies based on criteria
            // Assuming `criteria` is a dictionary of property names and values
            var query = _context.Set<BcheodDomainModels.Policy>().AsQueryable();

            if (criteria is Dictionary<string, object> filters)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(p => EF.Property<object>(p, filter.Key).Equals(filter.Value));
                }
            }

            return query.ToList();
        }
    }

    public class PolicyCicsService : IPolicyCicsService
    {
        public void SendPolicyUpdate(BcheodDomainModels.Policy policy)
        {
            // Send a policy update to the CICS system
            // Assuming a CICS client is available for communication
            var cicsClient = new CicsClient();
            cicsClient.SendPolicyUpdate(policy);
        }

        public BcheodDomainModels.PolicyStatus GetPolicyStatus(string policyNumber)
        {
            // Retrieve policy status from the CICS system
            var cicsClient = new CicsClient();
            return cicsClient.GetPolicyStatus(policyNumber);
        }

        public void SynchronizePolicyData()
        {
            // Synchronize policy data with the CICS system
            var cicsClient = new CicsClient();
            var policies = cicsClient.GetAllPolicies();

            foreach (var policy in policies)
            {
                var repository = new PolicyRepository(new BcheodDbContext());
                repository.SavePolicy(policy);
            }
        }
    }

    // Mock CICS client for demonstration purposes
    public class CicsClient
    {
        public void SendPolicyUpdate(BcheodDomainModels.Policy policy)
        {
            // Simulate sending policy update to CICS system
            Console.WriteLine($"Policy {policy.PolicyNumber} updated in CICS system.");
        }

        public BcheodDomainModels.PolicyStatus GetPolicyStatus(string policyNumber)
        {
            // Simulate retrieving policy status from CICS system
            return new BcheodDomainModels.PolicyStatus
            {
                PolicyNumber = policyNumber,
                Status = "Active"
            };
        }

        public List<BcheodDomainModels.Policy> GetAllPolicies()
        {
            // Simulate retrieving all policies from CICS system
            return new List<BcheodDomainModels.Policy>
            {
                new BcheodDomainModels.Policy { PolicyNumber = "P12345", PolicyHolder = "John Doe" },
                new BcheodDomainModels.Policy { PolicyNumber = "P67890", PolicyHolder = "Jane Smith" }
            };
        }
    }
}