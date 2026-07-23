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
                    .HasColumnType("decimal(13,2)"); // COBOL: STMT-AMOUNT PIC -Z(11)9.99

                entity.Property(e => e.StmtBalance)
                    .HasColumnName("stmt_balance")
                    .HasColumnType("decimal(15,2)"); // COBOL: STMT-BALANCE PIC -Z(13)9.99
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

    public class StmtrptRepository : IStmtrptRepository
    {
        private readonly StmtrptDbContext _dbContext;

        public StmtrptRepository(StmtrptDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<StmtrptDomainModels.AccountSummary> ReadAccountSummaries()
        {
            return _dbContext.Stmtout
                .Select(stmt => new StmtrptDomainModels.AccountSummary
                {
                    AccountNumber = stmt.StmtAcctNum
                })
                .ToList();
        }

        public void WriteAccountSummaries(List<StmtrptDomainModels.AccountSummary> accountSummaries)
        {
            foreach (var summary in accountSummaries)
            {
                var stmtout = new Stmtout
                {
                    StmtAcctNum = summary.AccountNumber,
                    StmtDescription = "Generated Description", // Placeholder for actual logic
                    StmtAmount = 0, // Placeholder for actual logic
                    StmtBalance = 0 // Placeholder for actual logic
                };

                _dbContext.Stmtout.Add(stmtout);
            }

            _dbContext.SaveChanges();
        }
    }

    public class StmtrptService : IStmtrptService
    {
        public StmtrptDomainModels.AccountSummary GetAccountDetails(string accountNumber)
        {
            // Placeholder for actual CICS interaction logic
            return new StmtrptDomainModels.AccountSummary
            {
                AccountNumber = accountNumber
            };
        }
    }
}