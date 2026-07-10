using System;
using System.Collections.Generic;
using System.IO;
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
            // Configure the database connection string here
            optionsBuilder.UseNpgsql("Host=localhost;Database=kycsync;Username=postgres;Password=password");
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

    public class KycsyncRepository : IKycsyncRepository
    {
        private readonly string _inputFilePath = "input.txt";
        private readonly string _outputFilePath = "output.txt";

        public List<KycsyncDomainModels.CustomerRecord> ReadCustomerRecords()
        {
            var customerRecords = new List<KycsyncDomainModels.CustomerRecord>();

            if (!File.Exists(_inputFilePath))
                throw new FileNotFoundException($"Input file not found: {_inputFilePath}");

            var lines = File.ReadAllLines(_inputFilePath);
            foreach (var line in lines)
            {
                // Assuming a fixed-width file format for simplicity
                var customerRecord = new KycsyncDomainModels.CustomerRecord
                {
                    CustomerId = line.Substring(0, 10).Trim(),
                    CustomerName = line.Substring(10, 30).Trim(),
                    CustomerAddress = new KycsyncDomainModels.CustomerRecord.Address
                    {
                        Street = line.Substring(40, 50).Trim(),
                        City = line.Substring(90, 20).Trim(),
                        PostalCode = line.Substring(110, 10).Trim()
                    },
                    AccountBalance = decimal.Parse(line.Substring(120, 15)) / 100,
                    AccountStatus = (KycsyncDomainModels.CustomerRecord.AccountStatusEnum)Enum.Parse(typeof(KycsyncDomainModels.CustomerRecord.AccountStatusEnum), line.Substring(135, 1)),
                    Transactions = new List<KycsyncDomainModels.CustomerRecord.Transaction>()
                };

                for (int i = 0; i < 10; i++)
                {
                    int offset = 136 + (i * 30);
                    var transaction = new KycsyncDomainModels.CustomerRecord.Transaction
                    {
                        TransactionId = line.Substring(offset, 15).Trim(),
                        TransactionAmount = decimal.Parse(line.Substring(offset + 15, 9)) / 100,
                        TransactionDate = line.Substring(offset + 24, 8).Trim()
                    };
                    customerRecord.Transactions.Add(transaction);
                }

                customerRecords.Add(customerRecord);
            }

            return customerRecords;
        }

        public void WriteCustomerRecords(List<KycsyncDomainModels.CustomerRecord> customerRecords)
        {
            var lines = customerRecords.Select(record =>
            {
                var line = record.CustomerId.PadRight(10) +
                           record.CustomerName.PadRight(30) +
                           record.CustomerAddress.Street.PadRight(50) +
                           record.CustomerAddress.City.PadRight(20) +
                           record.CustomerAddress.PostalCode.PadRight(10) +
                           ((int)(record.AccountBalance * 100)).ToString().PadLeft(15, '0') +
                           ((char)record.AccountStatus).ToString();

                foreach (var transaction in record.Transactions)
                {
                    line += transaction.TransactionId.PadRight(15) +
                            ((int)(transaction.TransactionAmount * 100)).ToString().PadLeft(9, '0') +
                            transaction.TransactionDate.PadRight(8);
                }

                return line;
            });

            File.WriteAllLines(_outputFilePath, lines);
        }
    }

    public class KycsyncService : IKycsyncService
    {
        public KycsyncDomainModels.CustomerRecord RetrieveCustomerData(string customerId)
        {
            // Simulate retrieving data from a CICS system
            return new KycsyncDomainModels.CustomerRecord
            {
                CustomerId = customerId,
                CustomerName = "John Doe",
                CustomerAddress = new KycsyncDomainModels.CustomerRecord.Address
                {
                    Street = "123 Main St",
                    City = "Metropolis",
                    PostalCode = "12345"
                },
                AccountBalance = 12345.67m,
                AccountStatus = KycsyncDomainModels.CustomerRecord.AccountStatusEnum.Active,
                Transactions = new List<KycsyncDomainModels.CustomerRecord.Transaction>
                {
                    new KycsyncDomainModels.CustomerRecord.Transaction
                    {
                        TransactionId = "TXN001",
                        TransactionAmount = 100.50m,
                        TransactionDate = "20231010"
                    }
                }
            };
        }

        public void UpdateCustomerData(KycsyncDomainModels.CustomerRecord customerRecord)
        {
            // Simulate updating data in a CICS system
            Console.WriteLine($"Customer data for {customerRecord.CustomerId} updated successfully.");
        }
    }
}