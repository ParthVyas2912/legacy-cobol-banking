using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ATMTXN.Domain;

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
                    .HasMaxLength(12); // COBOL: TRANSACTION-ID PIC X(12)

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(16); // COBOL: ACCOUNT-NUMBER PIC X(16)

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .HasMaxLength(1); // COBOL: TRANSACTION-TYPE PIC X(1)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(13, 2); // COBOL: TRANSACTION-AMOUNT PIC S9(11)V99 COMP-3

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasColumnType("int"); // COBOL: TRANSACTION-DATE PIC 9(8)

                entity.Property(e => e.TransactionTime)
                    .HasColumnName("transaction_time")
                    .HasColumnType("int"); // COBOL: TRANSACTION-TIME PIC 9(6)

                entity.Property(e => e.TransactionBalanceAfter)
                    .HasColumnName("transaction_balance_after")
                    .HasPrecision(15, 2); // COBOL: TRANSACTION-BALANCE-AFTER PIC S9(13)V99 COMP-3

                entity.Property(e => e.TransactionStatus)
                    .HasColumnName("transaction_status")
                    .HasMaxLength(1); // COBOL: TRANSACTION-STATUS PIC X(1)

                entity.Property(e => e.TransactionErrorCode)
                    .HasColumnName("transaction_error_code")
                    .HasMaxLength(4); // COBOL: TRANSACTION-ERROR-CODE PIC X(4)
            });
        }
    }

    public interface ITransactionRepository
    {
        IEnumerable<AtmtxnDomainModels.TransactionRecord> GetAllTransactions();
        AtmtxnDomainModels.TransactionRecord GetTransactionById(string transactionId);
        void AddTransaction(AtmtxnDomainModels.TransactionRecord transaction);
        void UpdateTransaction(AtmtxnDomainModels.TransactionRecord transaction);
        void DeleteTransaction(string transactionId);
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AtmtxnDbContext _context;

        public TransactionRepository(AtmtxnDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<AtmtxnDomainModels.TransactionRecord> GetAllTransactions()
        {
            return _context.TransactionRecords.ToList();
        }

        public AtmtxnDomainModels.TransactionRecord GetTransactionById(string transactionId)
        {
            return _context.TransactionRecords.FirstOrDefault(t => t.TransactionId == transactionId);
        }

        public void AddTransaction(AtmtxnDomainModels.TransactionRecord transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            _context.TransactionRecords.Add(transaction);
            _context.SaveChanges();
        }

        public void UpdateTransaction(AtmtxnDomainModels.TransactionRecord transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            _context.TransactionRecords.Update(transaction);
            _context.SaveChanges();
        }

        public void DeleteTransaction(string transactionId)
        {
            var transaction = GetTransactionById(transactionId);
            if (transaction == null)
                throw new KeyNotFoundException($"Transaction with ID {transactionId} not found.");

            _context.TransactionRecords.Remove(transaction);
            _context.SaveChanges();
        }
    }
}