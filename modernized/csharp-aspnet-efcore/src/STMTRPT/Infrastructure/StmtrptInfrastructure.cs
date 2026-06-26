using System;
using System.ComponentModel.DataAnnotations;
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
                    .HasMaxLength(10) // COBOL: STMT-ACCT-NUM PIC 9(10)
                    .IsFixedLength();

                entity.Property(e => e.StmtDescription)
                    .HasColumnName("stmt_description")
                    .HasMaxLength(40) // COBOL: STMT-DESCRIPTION PIC X(40)
                    .IsFixedLength();

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
        [Key]
        [StringLength(10)]
        public string StmtAcctNum { get; set; } // COBOL: STMT-ACCT-NUM PIC 9(10)

        [StringLength(40)]
        public string StmtDescription { get; set; } // COBOL: STMT-DESCRIPTION PIC X(40)

        [Precision(13, 2)]
        public decimal StmtAmount { get; set; } // COBOL: STMT-AMOUNT PIC -Z(11)9.99

        [Precision(15, 2)]
        public decimal StmtBalance { get; set; } // COBOL: STMT-BALANCE PIC -Z(13)9.99
    }

    public class StmtrptRepository : IStmtrptRepository
    {
        private readonly StmtrptDbContext _dbContext;

        public StmtrptRepository(StmtrptDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public StmtrptDomainModels.StatementReport ReadInputData()
        {
            // Implementation for reading input data from the database.
            var stmtoutRecords = _dbContext.Stmtout.ToList();

            var statementReport = new StmtrptDomainModels.StatementReport
            {
                Statements = stmtoutRecords.Select(record => new StmtrptDomainModels.Statement
                {
                    AccountNumber = record.StmtAcctNum,
                    Description = record.StmtDescription,
                    Amount = record.StmtAmount,
                    Balance = record.StmtBalance
                }).ToList()
            };

            return statementReport;
        }

        public void WriteOutputData(StmtrptDomainModels.StatementReport report)
        {
            // Implementation for writing output data to the database.
            foreach (var statement in report.Statements)
            {
                var stmtoutRecord = new Stmtout
                {
                    StmtAcctNum = statement.AccountNumber,
                    StmtDescription = statement.Description,
                    StmtAmount = statement.Amount,
                    StmtBalance = statement.Balance
                };

                _dbContext.Stmtout.Add(stmtoutRecord);
            }

            _dbContext.SaveChanges();
        }
    }

    public class StmtrptCicsService : IStmtrptCicsService
    {
        public StmtrptDomainModels.ReportDetail RetrieveCicsData(string transactionId)
        {
            // Implementation for retrieving data from a CICS transaction.
            // Simulated retrieval logic for demonstration purposes.
            return new StmtrptDomainModels.ReportDetail
            {
                TransactionId = transactionId,
                DetailMessage = $"Data retrieved for transaction ID: {transactionId}"
            };
        }

        public void LogCicsMessage(string message)
        {
            // Implementation for logging a message to the CICS system.
            // Simulated logging logic for demonstration purposes.
            Console.WriteLine($"CICS Log: {message}");
        }
    }
}