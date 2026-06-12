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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stmtout>(entity =>
            {
                entity.ToTable("stmtout", "public");

                entity.Property(e => e.StmtAcctNum)
                    .HasColumnName("stmt_acct_num")
                    .HasMaxLength(10); // COBOL: STMT-ACCT-NUM PIC 9(10)

                entity.Property(e => e.StmtDescription)
                    .HasColumnName("stmt_description")
                    .HasMaxLength(40); // COBOL: STMT-DESCRIPTION PIC X(40)

                entity.Property(e => e.StmtAmount)
                    .HasColumnName("stmt_amount")
                    .HasPrecision(13, 2); // COBOL: STMT-AMOUNT PIC -Z(11)9.99

                entity.Property(e => e.StmtBalance)
                    .HasColumnName("stmt_balance")
                    .HasPrecision(15, 2); // COBOL: STMT-BALANCE PIC -Z(13)9.99
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

        public StmtrptDomainModels.StatementReport ReadStatementReport()
        {
            var statementReport = new StmtrptDomainModels.StatementReport
            {
                Transactions = _context.Stmtout.Select(stmt => new StmtrptDomainModels.Transaction
                {
                    AccountNumber = stmt.StmtAcctNum,
                    Description = stmt.StmtDescription,
                    Amount = stmt.StmtAmount,
                    Balance = stmt.StmtBalance
                }).ToList()
            };

            return statementReport;
        }

        public void WriteStatementReport(StmtrptDomainModels.StatementReport report)
        {
            foreach (var transaction in report.Transactions)
            {
                var stmtout = new Stmtout
                {
                    StmtAcctNum = transaction.AccountNumber,
                    StmtDescription = transaction.Description,
                    StmtAmount = transaction.Amount,
                    StmtBalance = transaction.Balance
                };

                _context.Stmtout.Add(stmtout);
            }

            _context.SaveChanges();
        }

        public StmtrptDomainModels.Transaction ReadTransactionRecord()
        {
            var stmtout = _context.Stmtout.FirstOrDefault();
            if (stmtout == null)
            {
                return null;
            }

            return new StmtrptDomainModels.Transaction
            {
                AccountNumber = stmtout.StmtAcctNum,
                Description = stmtout.StmtDescription,
                Amount = stmtout.StmtAmount,
                Balance = stmtout.StmtBalance
            };
        }

        public void WriteTransactionRecord(StmtrptDomainModels.Transaction transaction)
        {
            var stmtout = new Stmtout
            {
                StmtAcctNum = transaction.AccountNumber,
                StmtDescription = transaction.Description,
                StmtAmount = transaction.Amount,
                StmtBalance = transaction.Balance
            };

            _context.Stmtout.Add(stmtout);
            _context.SaveChanges();
        }
    }

    public class StmtrptCicsService : IStmtrptCicsService
    {
        public StmtrptDomainModels.Transaction RetrieveTransactionData(string transactionId)
        {
            // Simulate retrieving transaction data from CICS
            return new StmtrptDomainModels.Transaction
            {
                AccountNumber = "1234567890",
                Description = "Sample Transaction",
                Amount = 100.50m,
                Balance = 1000.00m
            };
        }

        public void LogMessage(string message)
        {
            // Simulate logging a message to the CICS system
            Console.WriteLine($"CICS Log: {message}");
        }

        public void CommitTransaction()
        {
            // Simulate committing a transaction in the CICS system
            Console.WriteLine("CICS Transaction Committed");
        }

        public void RollbackTransaction()
        {
            // Simulate rolling back a transaction in the CICS system
            Console.WriteLine("CICS Transaction Rolled Back");
        }
    }
}