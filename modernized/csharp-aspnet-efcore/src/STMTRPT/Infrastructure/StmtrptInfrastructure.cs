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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection string here
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

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
                    .HasColumnType("decimal(13, 2)") // COBOL: STMT-AMOUNT PIC -Z(11)9.99
                    .IsRequired();

                entity.Property(e => e.StmtBalance)
                    .HasColumnName("stmt_balance")
                    .HasColumnType("decimal(15, 2)") // COBOL: STMT-BALANCE PIC -Z(13)9.99
                    .IsRequired();
            });
        }
    }

    public class Stmtout
    {
        public string StmtAcctNum { get; set; } // COBOL: STMT-ACCT-NUM PIC 9(10)
        public string StmtDescription { get; set; } // COBOL: STMT-DESCRIPTION PIC X(40)
        public decimal StmtAmount { get; set; } // COBOL: STMT-AMOUNT PIC -Z(11)9.99
        public decimal StmtBalance { get; set; } // COBOL: STMT-BALANCE PIC -Z(13)9.99
    }

    public class StatementReportRepository : IStatementReportRepository
    {
        private readonly StmtrptDbContext _context;

        public StatementReportRepository(StmtrptDbContext context)
        {
            _context = context;
        }

        public StmtrptDomainModels.StatementReport ReadStatementReport(string accountNumber)
        {
            var stmtout = _context.Stmtouts.FirstOrDefault(s => s.StmtAcctNum == accountNumber);
            if (stmtout == null)
            {
                throw new KeyNotFoundException($"Statement for account number {accountNumber} not found.");
            }

            return new StmtrptDomainModels.StatementReport
            {
                AccountNumber = stmtout.StmtAcctNum,
                Balance = stmtout.StmtBalance,
                Transactions = new List<StmtrptDomainModels.StatementReport.Transaction>
                {
                    new StmtrptDomainModels.StatementReport.Transaction
                    {
                        Description = stmtout.StmtDescription,
                        Amount = stmtout.StmtAmount,
                        Type = stmtout.StmtAmount >= 0
                            ? StmtrptDomainModels.StatementReport.Transaction.TransactionType.Credit
                            : StmtrptDomainModels.StatementReport.Transaction.TransactionType.Debit
                    }
                }
            };
        }

        public void WriteStatementReport(StmtrptDomainModels.StatementReport report)
        {
            var stmtout = new Stmtout
            {
                StmtAcctNum = report.AccountNumber,
                StmtBalance = report.Balance,
                StmtDescription = report.Transactions.FirstOrDefault()?.Description,
                StmtAmount = report.Transactions.FirstOrDefault()?.Amount ?? 0
            };

            _context.Stmtouts.Add(stmtout);
            _context.SaveChanges();
        }
    }

    public class StatementReportService : IStatementReportService
    {
        public StmtrptDomainModels.StatementReport GetAccountDetails(string accountNumber)
        {
            // Simulate CICS system call to retrieve account details
            return new StmtrptDomainModels.StatementReport
            {
                AccountNumber = accountNumber,
                Balance = 1000.00m,
                StatementDate = DateTime.Now,
                Transactions = new List<StmtrptDomainModels.StatementReport.Transaction>
                {
                    new StmtrptDomainModels.StatementReport.Transaction
                    {
                        TransactionCode = "TX001",
                        Amount = 100.00m,
                        Description = "Sample Transaction",
                        Type = StmtrptDomainModels.StatementReport.Transaction.TransactionType.Credit
                    }
                }
            };
        }

        public void UpdateAccountDetails(StmtrptDomainModels.StatementReport report)
        {
            // Simulate CICS system call to update account details
            Console.WriteLine($"Account {report.AccountNumber} updated with balance {report.Balance}");
        }
    }
}