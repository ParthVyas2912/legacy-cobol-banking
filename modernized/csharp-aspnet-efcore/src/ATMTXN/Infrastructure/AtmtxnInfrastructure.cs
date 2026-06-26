using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ATMTXN.Domain;
using ATMTXN.Application.Interfaces;

namespace ATMTXN.Infrastructure
{
    public class AtmtxnDbContext : DbContext
    {
        public DbSet<AtmtxnDomainModels.Transaction> Transactions { get; set; }
        public DbSet<AtmtxnDomainModels.AccountDetails> Accounts { get; set; }

        public AtmtxnDbContext(DbContextOptions<AtmtxnDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AtmtxnDomainModels.Transaction>(entity =>
            {
                entity.ToTable("transactions");

                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(12) // COBOL: TRANSACTION-ID PIC X(12)
                    .IsRequired();

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(11, 2) // COBOL: TRANSACTION-AMOUNT PIC 9(9)V99 COMP-3
                    .IsRequired();

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .IsRequired(); // COBOL: TRANSACTION-TYPE (88-level equivalent)

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasMaxLength(8) // COBOL: TRANSACTION-DATE PIC 9(8)
                    .IsRequired();

                entity.Property(e => e.TransactionTime)
                    .HasColumnName("transaction_time")
                    .HasMaxLength(6) // COBOL: TRANSACTION-TIME PIC 9(6)
                    .IsRequired();

                entity.OwnsOne(e => e.Account, account =>
                {
                    account.Property(a => a.AccountNumber)
                        .HasColumnName("account_number")
                        .HasMaxLength(16) // COBOL: ACCOUNT-NUMBER PIC X(16)
                        .IsRequired();

                    account.Property(a => a.AccountBalance)
                        .HasColumnName("account_balance")
                        .HasPrecision(13, 2) // COBOL: ACCOUNT-BALANCE PIC 9(11)V99 COMP-3
                        .IsRequired();
                });
            });

            modelBuilder.Entity<AtmtxnDomainModels.AccountDetails>(entity =>
            {
                entity.ToTable("accounts");

                entity.HasKey(e => e.AccountNumber);
                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(16) // COBOL: ACCOUNT-NUMBER PIC X(16)
                    .IsRequired();

                entity.Property(e => e.AccountBalance)
                    .HasColumnName("account_balance")
                    .HasPrecision(13, 2) // COBOL: ACCOUNT-BALANCE PIC 9(11)V99 COMP-3
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

        public AtmtxnDomainModels.Transaction GetTransactionById(string transactionId)
        {
            return _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefault(t => t.TransactionId == transactionId);
        }

        public void SaveTransaction(AtmtxnDomainModels.Transaction transaction)
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

    public class AccountRepository : IAccountRepository
    {
        private readonly AtmtxnDbContext _context;

        public AccountRepository(AtmtxnDbContext context)
        {
            _context = context;
        }

        public AtmtxnDomainModels.AccountDetails GetAccountByNumber(string accountNumber)
        {
            return _context.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
        }

        public void UpdateAccountBalance(string accountNumber, decimal newBalance)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                account.AccountBalance = newBalance;
                _context.SaveChanges();
            }
        }
    }
}