using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using KYCSYNC.Application.Interfaces;
using KYCSYNC.Domain;

namespace KYCSYNC.Infrastructure
{
    public class KycsyncDbContext : DbContext
    {
        public DbSet<KycAudit> KycAudits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KycAudit>(entity =>
            {
                entity.ToTable("kyc_audit", "public");

                entity.Property(e => e.CustId)
                    .HasColumnName("cust_id")
                    .HasColumnType("TEXT")
                    .HasMaxLength(30); // COBOL: CUST_ID PIC X(30)

                entity.Property(e => e.CustKycStatus)
                    .HasColumnName("cust_kyc_status")
                    .HasColumnType("TEXT")
                    .HasMaxLength(30); // COBOL: CUST_KYC_STATUS PIC X(30)

                entity.Property(e => e.CustRiskScore)
                    .HasColumnName("cust_risk_score")
                    .HasColumnType("TEXT")
                    .HasMaxLength(30); // COBOL: CUST_RISK_SCORE PIC X(30)
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("YourConnectionStringHere");
            }
        }
    }

    public class KycAudit
    {
        public string CustId { get; set; } // COBOL: CUST_ID PIC X(30)
        public string CustKycStatus { get; set; } // COBOL: CUST_KYC_STATUS PIC X(30)
        public string CustRiskScore { get; set; } // COBOL: CUST_RISK_SCORE PIC X(30)
    }

    public class CustomerRecordRepository : ICustomerRecordRepository
    {
        private readonly KycsyncDbContext _context;

        public CustomerRecordRepository(KycsyncDbContext context)
        {
            _context = context;
        }

        public KycsyncDomainModels.CustomerRecord ReadCustomerRecord(string customerId)
        {
            var kycAudit = _context.KycAudits.SingleOrDefault(a => a.CustId == customerId);
            if (kycAudit == null) return null;

            return new KycsyncDomainModels.CustomerRecord
            {
                CustomerId = kycAudit.CustId,
                Status = MapStatus(kycAudit.CustRiskScore)
            };
        }

        public void WriteCustomerRecord(KycsyncDomainModels.CustomerRecord customerRecord)
        {
            var kycAudit = new KycAudit
            {
                CustId = customerRecord.CustomerId,
                CustKycStatus = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                CustRiskScore = customerRecord.Status.ToString()
            };

            _context.KycAudits.Add(kycAudit);
            _context.SaveChanges();
        }

        public void DeleteCustomerRecord(string customerId)
        {
            var kycAudit = _context.KycAudits.SingleOrDefault(a => a.CustId == customerId);
            if (kycAudit != null)
            {
                _context.KycAudits.Remove(kycAudit);
                _context.SaveChanges();
            }
        }

        private KycsyncDomainModels.CustomerRecord.StatusEnum MapStatus(string riskScore)
        {
            return riskScore switch
            {
                "A" => KycsyncDomainModels.CustomerRecord.StatusEnum.Active,
                "I" => KycsyncDomainModels.CustomerRecord.StatusEnum.Inactive,
                "S" => KycsyncDomainModels.CustomerRecord.StatusEnum.Suspended,
                _ => throw new ArgumentOutOfRangeException(nameof(riskScore), "Unknown risk score")
            };
        }
    }

    public class KycsyncCicsService : IKycsyncCicsService
    {
        public bool SynchronizeCustomerData(KycsyncDomainModels.CustomerRecord customerRecord)
        {
            // Simulate synchronization logic with an external system
            return true;
        }

        public KycsyncDomainModels.CustomerRecord.StatusEnum GetCustomerStatus(string customerId)
        {
            // Simulate retrieval of customer status from an external system
            return KycsyncDomainModels.CustomerRecord.StatusEnum.Active;
        }
    }
}