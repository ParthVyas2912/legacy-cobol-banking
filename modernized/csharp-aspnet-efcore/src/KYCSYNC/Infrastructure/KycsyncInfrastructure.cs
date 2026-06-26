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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KycAudit>(entity =>
            {
                entity.ToTable("kyc_audit", "public");

                entity.Property(e => e.CustId)
                    .HasColumnName("cust_id")
                    .HasColumnType("TEXT")
                    .HasMaxLength(30); // Corrected to match PIC X(30)

                entity.Property(e => e.CustKycStatus)
                    .HasColumnName("cust_kyc_status")
                    .HasColumnType("DATE"); // Corrected to match COBOL PIC 9(8) (date format)

                entity.Property(e => e.CustRiskScore)
                    .HasColumnName("cust_risk_score")
                    .HasColumnType("TEXT")
                    .HasMaxLength(30); // Corrected to match PIC X(30)
            });
        }
    }

    public class KycAudit
    {
        public string CustId { get; set; } // COBOL: CUST_ID PIC X(30)
        public DateTime CustKycStatus { get; set; } // COBOL: CUST_KYC_STATUS PIC 9(8)
        public string CustRiskScore { get; set; } // COBOL: CUST_RISK_SCORE PIC X(30)
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly KycsyncDbContext _context;

        public CustomerRepository(KycsyncDbContext context)
        {
            _context = context;
        }

        public KycsyncDomainModels.Customer GetCustomerById(string customerId)
        {
            return _context.KycAudits
                .Where(a => a.CustId == customerId)
                .Select(a => new KycsyncDomainModels.Customer
                {
                    CustomerId = a.CustId,
                    KycStatus = a.CustKycStatus,
                    RiskScore = a.CustRiskScore
                })
                .FirstOrDefault();
        }

        public void UpdateCustomer(KycsyncDomainModels.Customer customer)
        {
            var entity = _context.KycAudits.FirstOrDefault(a => a.CustId == customer.CustomerId);
            if (entity != null)
            {
                entity.CustKycStatus = customer.KycStatus;
                entity.CustRiskScore = customer.RiskScore;
                _context.SaveChanges();
            }
        }

        public void AddCustomer(KycsyncDomainModels.Customer customer)
        {
            var entity = new KycAudit
            {
                CustId = customer.CustomerId,
                CustKycStatus = customer.KycStatus,
                CustRiskScore = customer.RiskScore
            };
            _context.KycAudits.Add(entity);
            _context.SaveChanges();
        }

        public void DeleteCustomer(string customerId)
        {
            var entity = _context.KycAudits.FirstOrDefault(a => a.CustId == customerId);
            if (entity != null)
            {
                _context.KycAudits.Remove(entity);
                _context.SaveChanges();
            }
        }
    }

    public class KycStatusRepository : IKycStatusRepository
    {
        private readonly KycsyncDbContext _context;

        public KycStatusRepository(KycsyncDbContext context)
        {
            _context = context;
        }

        public KycsyncDomainModels.KycStatus GetKycStatusByCustomerId(string customerId)
        {
            var entity = _context.KycAudits.FirstOrDefault(a => a.CustId == customerId);
            if (entity != null)
            {
                return new KycsyncDomainModels.KycStatus
                {
                    CustomerId = entity.CustId,
                    StatusDate = entity.CustKycStatus,
                    RiskScore = entity.CustRiskScore
                };
            }
            return null;
        }

        public void UpdateKycStatus(string customerId, KycsyncDomainModels.KycStatus kycStatus)
        {
            var entity = _context.KycAudits.FirstOrDefault(a => a.CustId == customerId);
            if (entity != null)
            {
                entity.CustKycStatus = kycStatus.StatusDate;
                entity.CustRiskScore = kycStatus.RiskScore;
                _context.SaveChanges();
            }
        }

        public void DeleteKycStatus(string customerId)
        {
            var entity = _context.KycAudits.FirstOrDefault(a => a.CustId == customerId);
            if (entity != null)
            {
                _context.KycAudits.Remove(entity);
                _context.SaveChanges();
            }
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly KycsyncDbContext _context;

        public TransactionRepository(KycsyncDbContext context)
        {
            _context = context;
        }

        public IEnumerable<KycsyncDomainModels.Transaction> GetTransactionsByCustomerId(string customerId)
        {
            return _context.KycAudits
                .Where(a => a.CustId == customerId)
                .Select(a => new KycsyncDomainModels.Transaction
                {
                    CustomerId = a.CustId,
                    TransactionDate = a.CustKycStatus, // Assuming CustKycStatus represents transaction date
                    RiskScore = a.CustRiskScore
                })
                .ToList();
        }

        public void AddTransaction(KycsyncDomainModels.Transaction transaction)
        {
            var entity = new KycAudit
            {
                CustId = transaction.CustomerId,
                CustKycStatus = transaction.TransactionDate,
                CustRiskScore = transaction.RiskScore
            };
            _context.KycAudits.Add(entity);
            _context.SaveChanges();
        }

        public void DeleteTransaction(string transactionId)
        {
            var entity = _context.KycAudits.FirstOrDefault(a => a.CustId == transactionId);
            if (entity != null)
            {
                _context.KycAudits.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void UpdateTransaction(KycsyncDomainModels.Transaction transaction)
        {
            var entity = _context.KycAudits.FirstOrDefault(a => a.CustId == transaction.CustomerId);
            if (entity != null)
            {
                entity.CustKycStatus = transaction.TransactionDate;
                entity.CustRiskScore = transaction.RiskScore;
                _context.SaveChanges();
            }
        }
    }

    public class CicsService : ICicsService
    {
        public bool SendKycVerificationRequest(KycsyncDomainModels.KycSyncRequest request)
        {
            // Simulate sending a KYC verification request to CICS
            Console.WriteLine($"Sending KYC verification request for Customer ID: {request.CustomerId}");
            return true; // Simulate success
        }

        public KycsyncDomainModels.KycStatus GetKycVerificationResponse(string customerId)
        {
            // Simulate retrieving a KYC verification response from CICS
            Console.WriteLine($"Retrieving KYC verification response for Customer ID: {customerId}");
            return new KycsyncDomainModels.KycStatus
            {
                CustomerId = customerId,
                StatusDate = DateTime.UtcNow,
                RiskScore = "Low"
            };
        }

        public bool CancelKycVerificationRequest(string customerId)
        {
            // Simulate canceling a KYC verification request in CICS
            Console.WriteLine($"Canceling KYC verification request for Customer ID: {customerId}");
            return true; // Simulate success
        }
    }
}