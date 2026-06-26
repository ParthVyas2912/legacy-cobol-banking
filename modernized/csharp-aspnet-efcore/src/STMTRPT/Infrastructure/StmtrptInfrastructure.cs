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
        public DbSet<Stmtout> Stmtouts { get; set; }
        public DbSet<StmtrptDomainModels.CustomerDetails> CustomerDetails { get; set; }
        public DbSet<StmtrptDomainModels.AccountSummary> AccountSummaries { get; set; }
        public DbSet<StmtrptDomainModels.Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stmtout>(entity =>
            {
                entity.ToTable("stmtout", "public");

                entity.Property(e => e.StmtAcctNum)
                    .HasColumnName("stmt_acct_num")
                    .HasMaxLength(10) // COBOL: STMT-ACCT-NUM PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.StmtDescription)
                    .HasColumnName("stmt_description")
                    .HasMaxLength(40) // COBOL: STMT-DESCRIPTION PIC X(40)
                    .IsRequired();

                entity.Property(e => e.StmtAmount)
                    .HasColumnName("stmt_amount")
                    .HasPrecision(15, 2) // COBOL: STMT-AMOUNT PIC -Z(11)9.99
                    .IsRequired();

                entity.Property(e => e.StmtBalance)
                    .HasColumnName("stmt_balance")
                    .HasPrecision(15, 2) // COBOL: STMT-BALANCE PIC -Z(13)9.99
                    .IsRequired();
            });

            modelBuilder.Entity<StmtrptDomainModels.CustomerDetails>(entity =>
            {
                entity.ToTable("customer_details", "public");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasMaxLength(15) // Assuming COBOL: CUSTOMER-ID PIC X(15)
                    .IsRequired();

                entity.Property(e => e.CustomerName)
                    .HasColumnName("customer_name")
                    .HasMaxLength(50) // Assuming COBOL: CUSTOMER-NAME PIC X(50)
                    .IsRequired();
            });

            modelBuilder.Entity<StmtrptDomainModels.AccountSummary>(entity =>
            {
                entity.ToTable("account_summary", "public");

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(10) // Assuming COBOL: ACCOUNT-NUMBER PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.Balance)
                    .HasColumnName("balance")
                    .HasPrecision(15, 2) // Assuming COBOL: BALANCE PIC -Z(13)9.99
                    .IsRequired();

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .IsRequired();
            });

            modelBuilder.Entity<StmtrptDomainModels.Transaction>(entity =>
            {
                entity.ToTable("transaction", "public");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(20) // Assuming COBOL: TRANSACTION-ID PIC X(20)
                    .IsRequired();

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(10) // Assuming COBOL: ACCOUNT-NUMBER PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .IsRequired();

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(15, 2) // Assuming COBOL: AMOUNT PIC -Z(11)9.99
                    .IsRequired();

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .IsRequired();
            });
        }

        public StmtrptDbContext(DbContextOptions<StmtrptDbContext> options) : base(options)
        {
        }
    }

    public class Stmtout
    {
        public string StmtAcctNum { get; set; } // COBOL: STMT-ACCT-NUM PIC 9(10)
        public string StmtDescription { get; set; } // COBOL: STMT-DESCRIPTION PIC X(40)
        public decimal StmtAmount { get; set; } // COBOL: STMT-AMOUNT PIC -Z(11)9.99
        public decimal StmtBalance { get; set; } // COBOL: STMT-BALANCE PIC -Z(13)9.99
    }

    public class StmtrptRepository : IStmtrptRepository
    {
        private readonly StmtrptDbContext _context;

        public StmtrptRepository(StmtrptDbContext context)
        {
            _context = context;
        }

        public StmtrptDomainModels.CustomerDetails ReadCustomerDetails(string customerId)
        {
            var customer = _context.Set<StmtrptDomainModels.CustomerDetails>()
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
            }

            return customer;
        }

        public void WriteAccountSummary(StmtrptDomainModels.AccountSummary accountSummary)
        {
            var existingSummary = _context.Set<StmtrptDomainModels.AccountSummary>()
                .FirstOrDefault(a => a.AccountNumber == accountSummary.AccountNumber);

            if (existingSummary != null)
            {
                existingSummary.Balance = accountSummary.Balance;
                existingSummary.LastUpdated = accountSummary.LastUpdated;
                _context.Update(existingSummary);
            }
            else
            {
                _context.Add(accountSummary);
            }

            _context.SaveChanges();
        }

        public List<StmtrptDomainModels.Transaction> ReadTransactions(string accountNumber)
        {
            return _context.Set<StmtrptDomainModels.Transaction>()
                .Where(t => t.AccountNumber == accountNumber)
                .OrderBy(t => t.TransactionDate)
                .ToList();
        }

        public void WriteTransaction(StmtrptDomainModels.Transaction transaction)
        {
            _context.Set<StmtrptDomainModels.Transaction>().Add(transaction);
            _context.SaveChanges();
        }
    }
}