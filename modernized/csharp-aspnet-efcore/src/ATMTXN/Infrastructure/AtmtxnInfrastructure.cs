using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ATMTXN.Application.Interfaces;
using ATMTXN.Domain;

namespace ATMTXN.Infrastructure
{
    public class AtmtxnDbContext : DbContext
    {
        public DbSet<AtmtxnDomainModels.Transaction> Transactions { get; set; }
        public DbSet<AtmtxnDomainModels.AccountDetails> AccountDetails { get; set; }
        public DbSet<AtmtxnDomainModels.TerminalDetails> TerminalDetails { get; set; }

        public AtmtxnDbContext(DbContextOptions<AtmtxnDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AtmtxnDomainModels.Transaction>(entity =>
            {
                entity.ToTable("transactions");

                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(12); // COBOL: TRANSACTION-ID PIC X(12)

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasColumnType("date"); // COBOL: TRANSACTION-DATE PIC 9(8)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(13, 2); // COBOL: TRANSACTION-AMOUNT PIC S9(11)V99 COMP-3

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .HasMaxLength(1); // COBOL: TRANSACTION-TYPE PIC X(1)

                entity.HasOne(e => e.AccountDetails)
                    .WithMany()
                    .HasForeignKey("account_number")
                    .HasConstraintName("FK_Transactions_AccountDetails"); // COBOL: ACCOUNT-NUMBER PIC X(16)

                entity.HasOne(e => e.TerminalDetails)
                    .WithMany()
                    .HasForeignKey("terminal_id")
                    .HasConstraintName("FK_Transactions_TerminalDetails"); // COBOL: TERMINAL-ID PIC X(8)
            });

            modelBuilder.Entity<AtmtxnDomainModels.AccountDetails>(entity =>
            {
                entity.ToTable("account_details");

                entity.HasKey(e => e.AccountNumber);
                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(16); // COBOL: ACCOUNT-NUMBER PIC X(16)

                entity.Property(e => e.AccountType)
                    .HasColumnName("account_type")
                    .HasMaxLength(4); // COBOL: ACCOUNT-TYPE PIC X(4)

                entity.Property(e => e.AccountBalance)
                    .HasColumnName("account_balance")
                    .HasPrecision(15, 2); // COBOL: ACCOUNT-BALANCE PIC S9(13)V99 COMP-3
            });

            modelBuilder.Entity<AtmtxnDomainModels.TerminalDetails>(entity =>
            {
                entity.ToTable("terminal_details");

                entity.HasKey(e => e.TerminalId);
                entity.Property(e => e.TerminalId)
                    .HasColumnName("terminal_id")
                    .HasMaxLength(8); // COBOL: TERMINAL-ID PIC X(8)

                entity.Property(e => e.Location)
                    .HasColumnName("location")
                    .HasMaxLength(20); // COBOL: LOCATION PIC X(20)
            });
        }
    }

    public class AtmtxnRepository : IAtmtxnRepository
    {
        private readonly AtmtxnDbContext _context;

        public AtmtxnRepository(AtmtxnDbContext context)
        {
            _context = context;
        }

        public AtmtxnDomainModels.Transaction ReadTransactionById(string transactionId)
        {
            return _context.Transactions
                .Include(t => t.AccountDetails)
                .Include(t => t.TerminalDetails)
                .FirstOrDefault(t => t.TransactionId == transactionId);
        }

        public void WriteTransaction(AtmtxnDomainModels.Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public void UpdateTransaction(AtmtxnDomainModels.Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }

        public void DeleteTransaction(string transactionId)
        {
            var transaction = _context.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                _context.SaveChanges();
            }
        }
    }
}