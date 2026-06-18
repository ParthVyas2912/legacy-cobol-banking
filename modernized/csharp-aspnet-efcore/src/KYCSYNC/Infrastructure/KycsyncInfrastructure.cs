using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using KYCSYNC.Domain;
using KYCSYNC.Application.Interfaces;

namespace KYCSYNC.Infrastructure
{
    public class KycsyncDbContext : DbContext
    {
        public DbSet<KycAudit> KycAudits { get; set; }

        public KycsyncDbContext(DbContextOptions<KycsyncDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<KycAudit>(entity =>
            {
                entity.ToTable("kyc_audit", "public");

                entity.Property(e => e.CustId)
                    .HasColumnName("cust_id")
                    .HasColumnType("CHAR(10)") // Corrected to match PIC X(10)
                    .HasMaxLength(10);

                entity.Property(e => e.CustKycStatus)
                    .HasColumnName("cust_kyc_status")
                    .HasColumnType("CHAR(30)") // Corrected to match PIC X(30)
                    .HasMaxLength(30);

                entity.Property(e => e.CustRiskScore)
                    .HasColumnName("cust_risk_score")
                    .HasColumnType("CHAR(30)") // Corrected to match PIC X(30)
                    .HasMaxLength(30);
            });
        }
    }

    public class KycAudit
    {
        public string CustId { get; set; } // PIC X(10)
        public string CustKycStatus { get; set; } // PIC X(30)
        public string CustRiskScore { get; set; } // PIC X(30)
    }

    public class KycsyncRepository : IKycsyncRepository
    {
        private readonly KycsyncDbContext _context;

        public KycsyncRepository(KycsyncDbContext context)
        {
            _context = context;
        }

        public KycsyncDomainModels.Customer ReadCustomer(string customerId)
        {
            var kycAudit = _context.KycAudits.SingleOrDefault(a => a.CustId == customerId);
            if (kycAudit == null) return null;

            return new KycsyncDomainModels.Customer
            {
                CustomerId = kycAudit.CustId,
                Status = MapKycStatusToAccountStatus(kycAudit.CustKycStatus),
                AccountBalance = 0, // Placeholder as AccountBalance is not in KycAudit
                FirstName = string.Empty, // Placeholder as FirstName is not in KycAudit
                LastName = string.Empty, // Placeholder as LastName is not in KycAudit
                CustomerAddress = null // Placeholder as Address is not in KycAudit
            };
        }

        public void WriteCustomer(KycsyncDomainModels.Customer customer)
        {
            var kycAudit = new KycAudit
            {
                CustId = customer.CustomerId,
                CustKycStatus = MapAccountStatusToKycStatus(customer.Status),
                CustRiskScore = string.Empty // Placeholder as RiskScore is not in Customer
            };

            _context.KycAudits.Add(kycAudit);
            _context.SaveChanges();
        }

        public List<KycsyncDomainModels.Customer> GetAllCustomers()
        {
            return _context.KycAudits.Select(kycAudit => new KycsyncDomainModels.Customer
            {
                CustomerId = kycAudit.CustId,
                Status = MapKycStatusToAccountStatus(kycAudit.CustKycStatus),
                AccountBalance = 0, // Placeholder as AccountBalance is not in KycAudit
                FirstName = string.Empty, // Placeholder as FirstName is not in KycAudit
                LastName = string.Empty, // Placeholder as LastName is not in KycAudit
                CustomerAddress = null // Placeholder as Address is not in KycAudit
            }).ToList();
        }

        public void DeleteCustomer(string customerId)
        {
            var kycAudit = _context.KycAudits.SingleOrDefault(a => a.CustId == customerId);
            if (kycAudit != null)
            {
                _context.KycAudits.Remove(kycAudit);
                _context.SaveChanges();
            }
        }

        public void UpdateCustomer(KycsyncDomainModels.Customer customer)
        {
            var kycAudit = _context.KycAudits.SingleOrDefault(a => a.CustId == customer.CustomerId);
            if (kycAudit != null)
            {
                kycAudit.CustKycStatus = MapAccountStatusToKycStatus(customer.Status);
                kycAudit.CustRiskScore = string.Empty; // Placeholder as RiskScore is not in Customer
                _context.SaveChanges();
            }
        }

        private KycsyncDomainModels.AccountStatus MapKycStatusToAccountStatus(string kycStatus)
        {
            // Placeholder mapping logic
            return KycsyncDomainModels.AccountStatus.Active;
        }

        private string MapAccountStatusToKycStatus(KycsyncDomainModels.AccountStatus status)
        {
            // Placeholder mapping logic
            return "A"; // Example mapping to 'A' for Active
        }
    }
}