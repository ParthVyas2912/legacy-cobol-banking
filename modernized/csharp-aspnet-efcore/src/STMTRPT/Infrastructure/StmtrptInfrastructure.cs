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
        public DbSet<Stmtout> Stmtout { get; set; }

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
                    .IsFixedLength();

                entity.Property(e => e.StmtDescription)
                    .HasColumnName("stmt_description")
                    .HasMaxLength(40) // COBOL: STMT-DESCRIPTION PIC X(40)
                    .IsFixedLength();

                entity.Property(e => e.StmtAmount)
                    .HasColumnName("stmt_amount")
                    .HasPrecision(15, 2); // COBOL: STMT-AMOUNT PIC -Z(11)9.99

                entity.Property(e => e.StmtBalance)
                    .HasColumnName("stmt_balance")
                    .HasPrecision(15, 2); // COBOL: STMT-BALANCE PIC -Z(13)9.99
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

        public StmtrptDomainModels.StatementReport ReadReportById(string reportId)
        {
            var stmtout = _context.Stmtout.FirstOrDefault(s => s.StmtAcctNum == reportId);
            if (stmtout == null)
            {
                throw new KeyNotFoundException($"Report with ID {reportId} not found.");
            }

            return new StmtrptDomainModels.StatementReport
            {
                AccountNumber = stmtout.StmtAcctNum,
                Description = stmtout.StmtDescription,
                Amount = stmtout.StmtAmount,
                Balance = stmtout.StmtBalance
            };
        }

        public void WriteReport(StmtrptDomainModels.StatementReport report)
        {
            var stmtout = new Stmtout
            {
                StmtAcctNum = report.AccountNumber,
                StmtDescription = report.Description,
                StmtAmount = report.Amount,
                StmtBalance = report.Balance
            };

            _context.Stmtout.Add(stmtout);
            _context.SaveChanges();
        }

        public void DeleteReport(string reportId)
        {
            var stmtout = _context.Stmtout.FirstOrDefault(s => s.StmtAcctNum == reportId);
            if (stmtout == null)
            {
                throw new KeyNotFoundException($"Report with ID {reportId} not found.");
            }

            _context.Stmtout.Remove(stmtout);
            _context.SaveChanges();
        }
    }

    public class StatementReportService : IStatementReportService
    {
        private readonly StmtrptDbContext _context;

        public StatementReportService(StmtrptDbContext context)
        {
            _context = context;
        }

        public string GetCustomerInfo(string accountNumber)
        {
            var stmtout = _context.Stmtout.FirstOrDefault(s => s.StmtAcctNum == accountNumber);
            if (stmtout == null)
            {
                throw new KeyNotFoundException($"Account with number {accountNumber} not found.");
            }

            return stmtout.StmtDescription; // Assuming description contains customer info
        }

        public decimal GetAccountBalance(string accountNumber)
        {
            var stmtout = _context.Stmtout.FirstOrDefault(s => s.StmtAcctNum == accountNumber);
            if (stmtout == null)
            {
                throw new KeyNotFoundException($"Account with number {accountNumber} not found.");
            }

            return stmtout.StmtBalance;
        }

        public List<StmtrptDomainModels.StatementReport.Transaction> GetRecentTransactions(string accountNumber)
        {
            var stmtouts = _context.Stmtout
                .Where(s => s.StmtAcctNum == accountNumber)
                .OrderByDescending(s => s.StmtAmount) // Assuming amount correlates to recency
                .Take(10)
                .ToList();

            return stmtouts.Select(s => new StmtrptDomainModels.StatementReport.Transaction
            {
                Description = s.StmtDescription,
                Amount = s.StmtAmount,
                Balance = s.StmtBalance
            }).ToList();
        }
    }
}