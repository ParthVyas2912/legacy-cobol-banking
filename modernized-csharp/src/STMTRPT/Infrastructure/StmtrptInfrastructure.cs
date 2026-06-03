using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using STMTRPT.Application.Interfaces;
using STMTRPT.Domain;

namespace STMTRPT.Infrastructure
{
    public class StmtrptDbContext : DbContext
    {
        public DbSet<StmtrptDomainModels.CustomerRecord> CustomerRecords { get; set; }
        public DbSet<StmtrptDomainModels.TransactionLedger> TransactionLedgers { get; set; }
        public DbSet<StmtrptDomainModels.StatementFile> StatementFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StmtrptDomainModels.CustomerRecord>(entity =>
            {
                entity.ToTable("customer_records");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasMaxLength(10); // COBOL: CUSTOMER-ID PIC X(10)

                entity.Property(e => e.CustomerName)
                    .HasColumnName("customer_name")
                    .HasMaxLength(30); // COBOL: CUSTOMER-NAME PIC X(30)

                entity.Property(e => e.RiskScore)
                    .HasColumnName("risk_score")
                    .HasPrecision(3, 0); // COBOL: RISK-SCORE PIC 9(03) COMP-3

                entity.Property(e => e.AccountCount)
                    .HasColumnName("account_count")
                    .HasPrecision(3, 0); // COBOL: ACCOUNT-COUNT PIC 9(03) COMP-3

                entity.Property(e => e.CustomerStatus)
                    .HasColumnName("customer_status")
                    .HasMaxLength(20); // COBOL: CUSTOMER-STATUS PIC X(20)

                entity.OwnsOne(e => e.CustomerAddress, address =>
                {
                    address.Property(a => a.Street)
                        .HasColumnName("street")
                        .HasMaxLength(50); // COBOL: STREET PIC X(50)

                    address.Property(a => a.City)
                        .HasColumnName("city")
                        .HasMaxLength(30); // COBOL: CITY PIC X(30)

                    address.Property(a => a.State)
                        .HasColumnName("state")
                        .HasMaxLength(2); // COBOL: STATE PIC X(2)

                    address.Property(a => a.ZipCode)
                        .HasColumnName("zip_code")
                        .HasMaxLength(10); // COBOL: ZIP-CODE PIC X(10)
                });
            });

            modelBuilder.Entity<StmtrptDomainModels.TransactionLedger>(entity =>
            {
                entity.ToTable("txn_ledger");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasPrecision(10, 0); // COBOL: TRANSACTION-ID PIC 9(10)

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(10); // COBOL: ACCOUNT-NUMBER PIC X(10)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(15, 2); // COBOL: TRANSACTION-AMOUNT PIC 9(13)V99 COMP-3

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasMaxLength(8); // COBOL: TRANSACTION-DATE PIC X(8)

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(30); // COBOL: DESCRIPTION PIC X(30)
            });

            modelBuilder.Entity<StmtrptDomainModels.StatementFile>(entity =>
            {
                entity.ToTable("stmt_file");

                entity.Property(e => e.StatementId)
                    .HasColumnName("statement_id")
                    .HasMaxLength(10); // COBOL: STATEMENT-ID PIC X(10)

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasMaxLength(10); // COBOL: CUSTOMER-ID PIC X(10)

                entity.Property(e => e.TotalBalance)
                    .HasColumnName("total_balance")
                    .HasPrecision(15, 2); // COBOL: TOTAL-BALANCE PIC 9(13)V99 COMP-3

                entity.Property(e => e.StatementDate)
                    .HasColumnName("statement_date")
                    .HasMaxLength(8); // COBOL: STATEMENT-DATE PIC X(8)
            });
        }
    }

    public class StatementFileRepository : IStatementFileRepository
    {
        private readonly StmtrptDbContext _context;

        public StatementFileRepository(StmtrptDbContext context)
        {
            _context = context;
        }

        public void WriteStatementFile(StmtrptDomainModels.StatementFile statementFile)
        {
            _context.StatementFiles.Add(statementFile);
            _context.SaveChanges();
        }
    }

    public class TransactionLedgerRepository : ITransactionLedgerRepository
    {
        private readonly StmtrptDbContext _context;

        public TransactionLedgerRepository(StmtrptDbContext context)
        {
            _context = context;
        }

        public List<StmtrptDomainModels.TransactionLedger> FetchTransactionLedgers()
        {
            return _context.TransactionLedgers.ToList();
        }
    }

    public class CustomerRecordRepository : ICustomerRecordRepository
    {
        private readonly StmtrptDbContext _context;

        public CustomerRecordRepository(StmtrptDbContext context)
        {
            _context = context;
        }

        public List<StmtrptDomainModels.CustomerRecord> FetchCustomerRecords()
        {
            return _context.CustomerRecords.ToList();
        }
    }

    public class ImsService : IImsService
    {
        public StmtrptDomainModels.CustomerRecord ExecuteImsGn(string functionCode)
        {
            // Simulating IMS GN call logic based on the provided function code
            // This is a placeholder implementation based on the provided algorithm and source code
            if (string.IsNullOrWhiteSpace(functionCode))
            {
                throw new ArgumentException("Function code cannot be null or empty.", nameof(functionCode));
            }

            // Simulate fetching a customer record based on the function code
            // In the real implementation, this would involve calling the IMS system
            if (functionCode == "GET_CUSTOMER")
            {
                return new StmtrptDomainModels.CustomerRecord
                {
                    CustomerId = "CUST12345",
                    CustomerName = "John Doe",
                    RiskScore = 85,
                    AccountCount = 3,
                    CustomerStatus = "Active",
                    CustomerAddress = new StmtrptDomainModels.CustomerAddress
                    {
                        Street = "123 Main St",
                        City = "Springfield",
                        State = "IL",
                        ZipCode = "62704"
                    }
                };
            }

            throw new InvalidOperationException($"Unsupported function code: {functionCode}");
        }
    }
}