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
        public DbSet<AtmtxnDomainModels.TransactionRecord> TransactionRecords { get; set; }

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
                    .HasConversion<string>(); // COBOL: TRANSACTION-TYPE 88-level equivalent

                entity.Property(e => e.AtmLocation)
                    .HasColumnName("atm_location")
                    .HasMaxLength(50); // COBOL: ATM-LOCATION PIC X(50)

                entity.Property(e => e.StatusCode)
                    .HasColumnName("status_code")
                    .HasConversion<string>(); // COBOL: STATUS-CODE 88-level equivalent
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure the database connection string here
            optionsBuilder.UseSqlServer("YourConnectionStringHere");
        }
    }

    public class AtmtxnFileRepository : IAtmtxnFileRepository
    {
        private readonly AtmtxnDbContext _dbContext;

        public AtmtxnFileRepository(AtmtxnDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AtmtxnDomainModels.TransactionRecord ReadTransaction(string transactionId)
        {
            return _dbContext.TransactionRecords.SingleOrDefault(tr => tr.TransactionId == transactionId);
        }

        public void WriteTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord)
        {
            _dbContext.TransactionRecords.Add(transactionRecord);
            _dbContext.SaveChanges();
        }

        public void UpdateTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord)
        {
            _dbContext.TransactionRecords.Update(transactionRecord);
            _dbContext.SaveChanges();
        }

        public void DeleteTransaction(string transactionId)
        {
            var transactionRecord = _dbContext.TransactionRecords.SingleOrDefault(tr => tr.TransactionId == transactionId);
            if (transactionRecord != null)
            {
                _dbContext.TransactionRecords.Remove(transactionRecord);
                _dbContext.SaveChanges();
            }
        }
    }

    public class AtmtxnCicsService : IAtmtxnCicsService
    {
        public AtmtxnDomainModels.StatusCodeEnum SendTransactionRequest(AtmtxnDomainModels.TransactionRecord transactionRecord)
        {
            // Simulate sending a transaction request to the CICS system
            // In a real implementation, this would involve calling a CICS API
            Console.WriteLine($"Sending transaction request for Transaction ID: {transactionRecord.TransactionId}");
            return AtmtxnDomainModels.StatusCodeEnum.Success;
        }

        public AtmtxnDomainModels.StatusCodeEnum GetTransactionStatus(string transactionId)
        {
            // Simulate retrieving the status of a transaction from the CICS system
            // In a real implementation, this would involve calling a CICS API
            Console.WriteLine($"Retrieving transaction status for Transaction ID: {transactionId}");
            return AtmtxnDomainModels.StatusCodeEnum.Success;
        }
    }
}