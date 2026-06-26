using System;
using Microsoft.EntityFrameworkCore;
using ATMTXN.Domain;
using ATMTXN.Application.Interfaces;

namespace ATMTXN.Infrastructure
{
    public class AtmtxnDbContext : DbContext
    {
        public DbSet<AtmtxnDomainModels.TransactionRecord> TransactionRecords { get; set; }

        public AtmtxnDbContext(DbContextOptions<AtmtxnDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AtmtxnDomainModels.TransactionRecord>(entity =>
            {
                entity.ToTable("transaction_records");

                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(12) // COBOL: TRANSACTION-ID PIC X(12)
                    .IsRequired();

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(16) // COBOL: ACCOUNT-NUMBER PIC X(16)
                    .IsRequired();

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(15, 2) // COBOL: TRANSACTION-AMOUNT PIC S9(13)V99 COMP-3
                    .IsRequired();

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasMaxLength(8) // COBOL: TRANSACTION-DATE PIC X(8)
                    .IsRequired();

                entity.Property(e => e.TransactionTime)
                    .HasColumnName("transaction_time")
                    .HasMaxLength(6) // COBOL: TRANSACTION-TIME PIC X(6)
                    .IsRequired();

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .HasConversion<string>() // COBOL: TRANSACTION-TYPE 88-level equivalent
                    .IsRequired();

                entity.Property(e => e.StatusCode)
                    .HasColumnName("status_code")
                    .HasConversion<string>() // COBOL: STATUS-CODE 88-level equivalent
                    .IsRequired();

                entity.Property(e => e.AtmLocation)
                    .HasColumnName("atm_location")
                    .HasMaxLength(20) // COBOL: ATM-LOCATION PIC X(20)
                    .IsRequired();
            });
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AtmtxnDbContext _context;

        public TransactionRepository(AtmtxnDbContext context)
        {
            _context = context;
        }

        public AtmtxnDomainModels.TransactionRecord ReadTransactionRecord(string transactionId)
        {
            return _context.TransactionRecords.Find(transactionId);
        }

        public void WriteTransactionRecord(AtmtxnDomainModels.TransactionRecord transactionRecord)
        {
            _context.TransactionRecords.Add(transactionRecord);
            _context.SaveChanges();
        }

        public void UpdateTransactionRecord(AtmtxnDomainModels.TransactionRecord transactionRecord)
        {
            _context.TransactionRecords.Update(transactionRecord);
            _context.SaveChanges();
        }

        public void DeleteTransactionRecord(string transactionId)
        {
            var record = _context.TransactionRecords.Find(transactionId);
            if (record != null)
            {
                _context.TransactionRecords.Remove(record);
                _context.SaveChanges();
            }
        }
    }

    public class CicsService : ICicsService
    {
        public AtmtxnDomainModels.StatusCodeEnum SendTransactionRequest(AtmtxnDomainModels.TransactionRecord transactionRecord)
        {
            // Simulate sending a transaction request to the CICS system
            // In a real implementation, this would involve communication with the CICS system
            return AtmtxnDomainModels.StatusCodeEnum.Success;
        }

        public AtmtxnDomainModels.TransactionRecord RetrieveAccountDetails(string accountNumber)
        {
            // Simulate retrieving account details from the CICS system
            // In a real implementation, this would involve communication with the CICS system
            return new AtmtxnDomainModels.TransactionRecord
            {
                AccountNumber = accountNumber,
                TransactionAmount = 1000.00m,
                TransactionDate = DateTime.Now.ToString("yyyyMMdd"),
                TransactionTime = DateTime.Now.ToString("HHmmss"),
                TransactionType = AtmtxnDomainModels.TransactionTypeEnum.Inquiry,
                StatusCode = AtmtxnDomainModels.StatusCodeEnum.Success,
                AtmLocation = "Main Branch"
            };
        }

        public void SendSystemErrorNotification(string errorCode)
        {
            // Simulate sending a system error notification to the CICS system
            // In a real implementation, this would involve communication with the CICS system
            Console.WriteLine($"System error notification sent with error code: {errorCode}");
        }
    }
}