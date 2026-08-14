using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ATMTXN.Domain;
using ATMTXN.Application.Interfaces;

namespace ATMTXN.Infrastructure
{
    public class AtmtxnDbContext : DbContext
    {
        public DbSet<AtmtxnDomainModels.TransactionRecord> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AtmtxnDomainModels.TransactionRecord>(entity =>
            {
                entity.ToTable("transaction_records");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(12); // COBOL: TRANSACTION-ID PIC X(12)

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(16); // COBOL: ACCOUNT-NUMBER PIC X(16)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(15, 2); // COBOL: TRANSACTION-AMOUNT PIC S9(13)V99 COMP-3

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasMaxLength(8); // COBOL: TRANSACTION-DATE PIC X(8)

                entity.Property(e => e.TransactionTime)
                    .HasColumnName("transaction_time")
                    .HasMaxLength(6); // COBOL: TRANSACTION-TIME PIC X(6)

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .HasConversion<int>(); // COBOL: TRANSACTION-TYPE (88-level equivalent)

                entity.Property(e => e.AtmLocation)
                    .HasColumnName("atm_location")
                    .HasMaxLength(50); // COBOL: ATM-LOCATION PIC X(50)

                entity.Property(e => e.StatusCode)
                    .HasColumnName("status_code")
                    .HasConversion<int>(); // COBOL: STATUS-CODE (88-level equivalent)
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("YourConnectionStringHere");
            }
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AtmtxnDbContext _dbContext;

        public TransactionRepository(AtmtxnDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AtmtxnDomainModels.TransactionRecord ReadTransaction()
        {
            // Simulate reading a transaction record from the database
            return _dbContext.Transactions.FirstOrDefault();
        }

        public void WriteTransaction(AtmtxnDomainModels.TransactionRecord transaction)
        {
            // Simulate writing a transaction record to the database
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
        }
    }

    public class TransactionCicsService : ITransactionCicsService
    {
        public bool ValidateAccount(string accountNumber)
        {
            // Simulate CICS account validation
            // In a real implementation, this would involve calling a CICS API
            return !string.IsNullOrEmpty(accountNumber) && accountNumber.Length == 16;
        }

        public bool UpdateAccountBalance(string accountNumber, decimal amount)
        {
            // Simulate updating account balance through CICS
            // In a real implementation, this would involve calling a CICS API
            return !string.IsNullOrEmpty(accountNumber) && amount != 0;
        }

        public void LogTransaction(AtmtxnDomainModels.TransactionRecord transaction)
        {
            // Simulate logging a transaction in the CICS system
            // In a real implementation, this would involve calling a CICS API
            Console.WriteLine($"Transaction logged: {transaction.TransactionId}");
        }
    }
}