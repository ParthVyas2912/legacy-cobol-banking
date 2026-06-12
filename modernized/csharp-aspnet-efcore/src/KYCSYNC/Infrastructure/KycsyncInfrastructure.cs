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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KycAudit>(entity =>
            {
                entity.ToTable("kyc_audit", "public");

                entity.Property(e => e.CustId)
                    .HasColumnName("cust_id")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(10);
                // COBOL: CUST_ID PIC X(10)

                entity.Property(e => e.CustKycStatus)
                    .HasColumnName("cust_kyc_status")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: CUST_KYC_STATUS PIC X(30)

                entity.Property(e => e.CustRiskScore)
                    .HasColumnName("cust_risk_score")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: CUST_RISK_SCORE PIC X(30)
            });

            modelBuilder.Entity<KycTransaction>(entity =>
            {
                entity.ToTable("kyc_transaction", "public");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(15);
                // COBOL: TRANSACTION_ID PIC X(15)

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("NUMERIC")
                    .HasPrecision(13, 2);
                // COBOL: AMOUNT PIC S9(13)V99 COMP-3

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("DATE");
                // COBOL: DATE PIC 9(8) (YYYYMMDD format)
            });
        }
    }

    public class KycAudit
    {
        public string CustId { get; set; } // COBOL: CUST_ID PIC X(10)
        public string CustKycStatus { get; set; } // COBOL: CUST_KYC_STATUS PIC X(30)
        public string CustRiskScore { get; set; } // COBOL: CUST_RISK_SCORE PIC X(30)
        public List<KycTransaction> Transactions { get; set; } = new List<KycTransaction>();
    }

    public class KycTransaction
    {
        public string TransactionId { get; set; } // COBOL: TRANSACTION_ID PIC X(15)
        public decimal Amount { get; set; } // COBOL: AMOUNT PIC S9(13)V99 COMP-3
        public DateTime Date { get; set; } // COBOL: DATE PIC 9(8) (YYYYMMDD format)
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly KycsyncDbContext _context;

        public CustomerRepository(KycsyncDbContext context)
        {
            _context = context;
        }

        public KycsyncDomainModels.CustomerRecord GetCustomerById(string customerId)
        {
            var customer = _context.KycAudits
                .Where(a => a.CustId == customerId)
                .Select(a => new KycsyncDomainModels.CustomerRecord
                {
                    CustomerId = a.CustId,
                    KycStatus = a.CustKycStatus,
                    RiskScore = a.CustRiskScore
                })
                .FirstOrDefault();

            return customer;
        }

        public void SaveCustomer(KycsyncDomainModels.CustomerRecord customerRecord)
        {
            var kycAudit = new KycAudit
            {
                CustId = customerRecord.CustomerId,
                CustKycStatus = customerRecord.KycStatus,
                CustRiskScore = customerRecord.RiskScore
            };

            _context.KycAudits.Add(kycAudit);
            _context.SaveChanges();
        }

        public void UpdateCustomer(KycsyncDomainModels.CustomerRecord customerRecord)
        {
            var kycAudit = _context.KycAudits.FirstOrDefault(a => a.CustId == customerRecord.CustomerId);
            if (kycAudit != null)
            {
                kycAudit.CustKycStatus = customerRecord.KycStatus;
                kycAudit.CustRiskScore = customerRecord.RiskScore;

                _context.SaveChanges();
            }
        }

        public void DeleteCustomer(string customerId)
        {
            var kycAudit = _context.KycAudits.FirstOrDefault(a => a.CustId == customerId);
            if (kycAudit != null)
            {
                _context.KycAudits.Remove(kycAudit);
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

        public List<KycsyncDomainModels.CustomerRecord.Transaction> GetTransactionsByCustomerId(string customerId)
        {
            var transactions = _context.KycAudits
                .Where(a => a.CustId == customerId)
                .SelectMany(a => a.Transactions)
                .Select(t => new KycsyncDomainModels.CustomerRecord.Transaction
                {
                    TransactionId = t.TransactionId,
                    Amount = t.Amount,
                    Date = t.Date
                })
                .ToList();

            return transactions;
        }

        public void SaveTransaction(KycsyncDomainModels.CustomerRecord.Transaction transaction)
        {
            var kycAudit = _context.KycAudits.FirstOrDefault(a => a.CustId == transaction.CustomerId);
            if (kycAudit != null)
            {
                kycAudit.Transactions.Add(new KycTransaction
                {
                    TransactionId = transaction.TransactionId,
                    Amount = transaction.Amount,
                    Date = transaction.Date
                });

                _context.SaveChanges();
            }
        }

        public void UpdateTransaction(KycsyncDomainModels.CustomerRecord.Transaction transaction)
        {
            var kycAudit = _context.KycAudits.FirstOrDefault(a => a.CustId == transaction.CustomerId);
            var kycTransaction = kycAudit?.Transactions.FirstOrDefault(t => t.TransactionId == transaction.TransactionId);
            if (kycTransaction != null)
            {
                kycTransaction.Amount = transaction.Amount;
                kycTransaction.Date = transaction.Date;

                _context.SaveChanges();
            }
        }

        public void DeleteTransaction(string transactionId)
        {
            var kycTransaction = _context.KycAudits
                .SelectMany(a => a.Transactions)
                .FirstOrDefault(t => t.TransactionId == transactionId);

            if (kycTransaction != null)
            {
                _context.Remove(kycTransaction);
                _context.SaveChanges();
            }
        }
    }

    public class CicsService : ICicsService
    {
        public string SendCicsRequest(string requestData)
        {
            // Simulate sending a request to the CICS system
            return $"Request sent to CICS with data: {requestData}";
        }

        public string RetrieveCicsData(string key)
        {
            // Simulate retrieving data from the CICS system
            return $"Data retrieved from CICS for key: {key}";
        }
    }
}