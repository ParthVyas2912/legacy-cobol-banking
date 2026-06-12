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

        public AtmtxnDbContext(DbContextOptions<AtmtxnDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AtmtxnDomainModels.TransactionRecord>(entity =>
            {
                entity.ToTable("transaction_records");

                entity.Property(e => e.TransactionAmount)
                    .HasPrecision(10, 0) // COBOL: TransactionAmount PIC 9(10) COMP-3
                    .HasColumnName("transaction_amount");

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(20) // COBOL: AccountNumber PIC X(20)
                    .HasColumnName("account_number");

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date") // COBOL: TransactionDate PIC 9(6)
                    .HasPrecision(6, 0);

                entity.Property(e => e.TransactionTime)
                    .HasColumnName("transaction_time") // COBOL: TransactionTime PIC 9(4)
                    .HasPrecision(4, 0);

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(2) // COBOL: TransactionType PIC X(2)
                    .HasColumnName("transaction_type");

                entity.Property(e => e.ATMId)
                    .HasColumnName("atm_id") // COBOL: ATMId PIC 9(5)
                    .HasPrecision(5, 0);

                entity.Property(e => e.Location)
                    .HasMaxLength(30) // COBOL: Location PIC X(30)
                    .HasColumnName("location");

                entity.Property(e => e.BalanceAfterTransaction)
                    .HasPrecision(11, 2) // COBOL: BalanceAfterTransaction PIC 9(9)V99 COMP-3
                    .HasColumnName("balance_after_transaction");

                entity.Property(e => e.TransactionStatus)
                    .HasMaxLength(1) // COBOL: TransactionStatus PIC X(1)
                    .HasColumnName("transaction_status");

                entity.Property(e => e.ErrorCode)
                    .HasMaxLength(1) // COBOL: ErrorCode PIC X(1)
                    .HasColumnName("error_code");

                entity.Property(e => e.ErrorMessage)
                    .HasMaxLength(50) // COBOL: ErrorMessage PIC X(50)
                    .HasColumnName("error_message");

                entity.OwnsMany(e => e.AdditionalInfos, additionalInfo =>
                {
                    additionalInfo.ToTable("additional_infos");

                    additionalInfo.Property(ai => ai.InfoKey)
                        .HasMaxLength(15) // COBOL: InfoKey PIC X(15)
                        .HasColumnName("info_key");

                    additionalInfo.Property(ai => ai.InfoValue)
                        .HasMaxLength(50) // COBOL: InfoValue PIC X(50)
                        .HasColumnName("info_value");
                });
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

        public AtmtxnDomainModels.TransactionRecord ReadTransaction(string transactionId)
        {
            return _context.TransactionRecords
                .Include(tr => tr.AdditionalInfos)
                .FirstOrDefault(tr => tr.TransactionStatus == transactionId);
        }

        public void WriteTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord)
        {
            _context.TransactionRecords.Add(transactionRecord);
            _context.SaveChanges();
        }

        public void UpdateTransaction(AtmtxnDomainModels.TransactionRecord transactionRecord)
        {
            _context.TransactionRecords.Update(transactionRecord);
            _context.SaveChanges();
        }

        public void DeleteTransaction(string transactionId)
        {
            var transaction = _context.TransactionRecords.FirstOrDefault(tr => tr.TransactionStatus == transactionId);
            if (transaction != null)
            {
                _context.TransactionRecords.Remove(transaction);
                _context.SaveChanges();
            }
        }

        public List<AtmtxnDomainModels.TransactionRecord> GetTransactionsByAccount(string accountNumber)
        {
            return _context.TransactionRecords
                .Where(tr => tr.AccountNumber == accountNumber)
                .Include(tr => tr.AdditionalInfos)
                .ToList();
        }
    }
}