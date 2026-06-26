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
        public DbSet<TransactionAudit> TransactionAudits { get; set; } // Added for transaction records

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection string here
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

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

            modelBuilder.Entity<TransactionAudit>(entity =>
            {
                entity.ToTable("transaction_audit", "public");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasColumnType("TEXT")
                    .HasMaxLength(30); // COBOL: TRANSACTION_ID PIC X(30)

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("TEXT")
                    .HasMaxLength(30); // COBOL: CUSTOMER_ID PIC X(30)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasColumnType("DECIMAL(18, 2)"); // COBOL: TRANSACTION_AMOUNT PIC 9(15)V99

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasColumnType("TIMESTAMP"); // COBOL: TRANSACTION_DATE PIC X(26)
            });
        }
    }

    public class KycAudit
    {
        public string CustId { get; set; }
        public string CustKycStatus { get; set; }
        public string CustRiskScore { get; set; }
    }

    public class TransactionAudit
    {
        public string TransactionId { get; set; }
        public string CustomerId { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class KycsyncRepository : IKycsyncRepository
    {
        private readonly KycsyncDbContext _dbContext;

        public KycsyncRepository(KycsyncDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<KycsyncDomainModels.CustomerRecord> ReadCustomerRecords()
        {
            return _dbContext.KycAudits
                .Select(audit => new KycsyncDomainModels.CustomerRecord
                {
                    CustomerId = audit.CustId,
                    KycStatus = audit.CustKycStatus,
                    RiskScore = audit.CustRiskScore
                })
                .ToList();
        }

        public void WriteCustomerRecords(List<KycsyncDomainModels.CustomerRecord> customerRecords)
        {
            var audits = customerRecords.Select(record => new KycAudit
            {
                CustId = record.CustomerId,
                CustKycStatus = record.KycStatus,
                CustRiskScore = record.RiskScore
            }).ToList();

            _dbContext.KycAudits.AddRange(audits);
            _dbContext.SaveChanges();
        }

        public List<KycsyncDomainModels.TransactionRecord> ReadTransactionRecords()
        {
            return _dbContext.TransactionAudits
                .Select(audit => new KycsyncDomainModels.TransactionRecord
                {
                    TransactionId = audit.TransactionId,
                    CustomerId = audit.CustomerId,
                    Amount = audit.TransactionAmount,
                    Date = audit.TransactionDate
                })
                .ToList();
        }

        public void WriteTransactionRecords(List<KycsyncDomainModels.TransactionRecord> transactionRecords)
        {
            var audits = transactionRecords.Select(record => new TransactionAudit
            {
                TransactionId = record.TransactionId,
                CustomerId = record.CustomerId,
                TransactionAmount = record.Amount,
                TransactionDate = record.Date
            }).ToList();

            _dbContext.TransactionAudits.AddRange(audits);
            _dbContext.SaveChanges();
        }

        public List<KycsyncDomainModels.KycStatus> ReadKycStatusRecords()
        {
            return _dbContext.KycAudits
                .Select(audit => new KycsyncDomainModels.KycStatus
                {
                    CustomerId = audit.CustId,
                    StatusTimestamp = DateTime.Parse(audit.CustKycStatus) // Assuming CustKycStatus is stored as a string
                })
                .ToList();
        }

        public void WriteKycStatusRecords(List<KycsyncDomainModels.KycStatus> kycStatusRecords)
        {
            var audits = kycStatusRecords.Select(record => new KycAudit
            {
                CustId = record.CustomerId,
                CustKycStatus = record.StatusTimestamp.ToString("o"), // ISO 8601 format
                CustRiskScore = "N/A" // Assuming risk score is not relevant for KYC status updates
            }).ToList();

            _dbContext.KycAudits.AddRange(audits);
            _dbContext.SaveChanges();
        }
    }

    public class CicsService : ICicsService
    {
        public string SendKycRequest(string requestData)
        {
            // Simulate sending a request to the external KYC system
            // Replace with actual implementation for sending requests
            return $"RequestID-{Guid.NewGuid()}";
        }

        public string RetrieveKycResponse(string requestId)
        {
            // Simulate retrieving a response from the external KYC system
            // Replace with actual implementation for retrieving responses
            return $"Response for {requestId}: KYC Approved";
        }
    }
}