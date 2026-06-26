using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using KYCSYNC.Domain;

namespace KYCSYNC.Infrastructure
{
    public class KycsyncDbContext : DbContext
    {
        public DbSet<KycAudit> KycAudits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure the database connection string here
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
                    .HasMaxLength(30);
                // COBOL: CUST_ID PIC X(30)

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
        }
    }

    public class KycAudit
    {
        public string CustId { get; set; } // COBOL: CUST_ID PIC X(30)
        public string CustKycStatus { get; set; } // COBOL: CUST_KYC_STATUS PIC X(30)
        public string CustRiskScore { get; set; } // COBOL: CUST_RISK_SCORE PIC X(30)
    }

    public class KycsyncRepository
    {
        private readonly KycsyncDbContext _dbContext;

        public KycsyncRepository(KycsyncDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<KycAudit> GetAllKycAudits()
        {
            return _dbContext.KycAudits.ToList();
        }

        public void AddKycAudit(KycAudit audit)
        {
            _dbContext.KycAudits.Add(audit);
            _dbContext.SaveChanges();
        }

        public void UpdateKycAudit(KycAudit audit)
        {
            _dbContext.KycAudits.Update(audit);
            _dbContext.SaveChanges();
        }

        public void DeleteKycAudit(string custId)
        {
            var audit = _dbContext.KycAudits.FirstOrDefault(a => a.CustId == custId);
            if (audit != null)
            {
                _dbContext.KycAudits.Remove(audit);
                _dbContext.SaveChanges();
            }
        }
    }

    public class KycsyncFileRepository : Application.Interfaces.IKycsyncFileRepository
    {
        public List<KycsyncDomainModels.Customer> ReadFromFile(string filePath)
        {
            var customers = new List<KycsyncDomainModels.Customer>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file at path {filePath} does not exist.");

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var fields = line.Split(',');
                if (fields.Length != 3)
                    throw new FormatException("Each line in the file must have exactly 3 fields: CustId, CustKycStatus, CustRiskScore.");

                customers.Add(new KycsyncDomainModels.Customer
                {
                    CustId = fields[0],
                    CustKycStatus = fields[1],
                    CustRiskScore = fields[2]
                });
            }

            return customers;
        }

        public void WriteToFile(string filePath, string reportContent)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            File.WriteAllText(filePath, reportContent);
        }
    }

    public class KycsyncCicsService : Application.Interfaces.IKycsyncCicsService
    {
        public KycsyncDomainModels.Customer RetrieveCustomerData(string customerId)
        {
            // Simulate a CICS call to retrieve customer data
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty.", nameof(customerId));

            // Mocked data for demonstration purposes
            return new KycsyncDomainModels.Customer
            {
                CustId = customerId,
                CustKycStatus = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                CustRiskScore = "Low"
            };
        }

        public void UpdateCustomerData(KycsyncDomainModels.Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (string.IsNullOrWhiteSpace(customer.CustId))
                throw new ArgumentException("Customer ID cannot be null or empty.", nameof(customer.CustId));

            // Simulate a CICS call to update customer data
            // In a real implementation, this would involve sending the updated customer data to the CICS system
            Console.WriteLine($"Customer data for ID {customer.CustId} has been updated in the CICS system.");
        }
    }
}