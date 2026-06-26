using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
                    .HasPrecision(15, 2); // COBOL: STMT-AMOUNT PIC -Z(11)9.99

                entity.Property(e => e.StmtBalance)
                    .HasColumnName("stmt_balance")
                    .HasPrecision(15, 2); // COBOL: STMT-BALANCE PIC -Z(13)9.99
            });
        }
    }

    [Table("stmtout", Schema = "public")]
    public class Stmtout
    {
        [Key]
        [Column("stmt_acct_num")]
        [MaxLength(10)] // COBOL: STMT-ACCT-NUM PIC 9(10)
        [FixedLength]
        public string StmtAcctNum { get; set; }

        [Column("stmt_description")]
        [MaxLength(40)] // COBOL: STMT-DESCRIPTION PIC X(40)
        [FixedLength]
        public string StmtDescription { get; set; }

        [Column("stmt_amount")]
        [Precision(15, 2)] // COBOL: STMT-AMOUNT PIC -Z(11)9.99
        public decimal StmtAmount { get; set; }

        [Column("stmt_balance")]
        [Precision(15, 2)] // COBOL: STMT-BALANCE PIC -Z(13)9.99
        public decimal StmtBalance { get; set; }
    }

    public class StatementReportRepository : IStatementReportRepository
    {
        private readonly StmtrptDbContext _context;

        public StatementReportRepository(StmtrptDbContext context)
        {
            _context = context;
        }

        public StmtrptDomainModels.StatementReport ReadStatementReport()
        {
            var stmtoutRecords = _context.Stmtout.ToList();

            var report = new StmtrptDomainModels.StatementReport
            {
                Statements = stmtoutRecords.Select(record => new StmtrptDomainModels.Statement
                {
                    AccountNumber = record.StmtAcctNum,
                    Description = record.StmtDescription,
                    Amount = record.StmtAmount,
                    Balance = record.StmtBalance
                }).ToList()
            };

            return report;
        }

        public void WriteStatementReport(StmtrptDomainModels.StatementReport report)
        {
            foreach (var statement in report.Statements)
            {
                var stmtout = new Stmtout
                {
                    StmtAcctNum = statement.AccountNumber,
                    StmtDescription = statement.Description,
                    StmtAmount = statement.Amount,
                    StmtBalance = statement.Balance
                };

                _context.Stmtout.Add(stmtout);
            }

            _context.SaveChanges();
        }
    }

    public class StatementReportService : IStatementReportService
    {
        private readonly ICicsClient _cicsClient;

        public StatementReportService(ICicsClient cicsClient)
        {
            _cicsClient = cicsClient;
        }

        public StmtrptDomainModels.StatementReport RetrieveCicsData(string reportId)
        {
            var cicsResponse = _cicsClient.GetReportData(reportId);

            var report = new StmtrptDomainModels.StatementReport
            {
                Statements = cicsResponse.Statements.Select(cicsStatement => new StmtrptDomainModels.Statement
                {
                    AccountNumber = cicsStatement.AccountNumber,
                    Description = cicsStatement.Description,
                    Amount = cicsStatement.Amount,
                    Balance = cicsStatement.Balance
                }).ToList()
            };

            return report;
        }

        public void SubmitCicsData(StmtrptDomainModels.StatementReport report)
        {
            var cicsRequest = new CicsRequest
            {
                Statements = report.Statements.Select(statement => new CicsStatement
                {
                    AccountNumber = statement.AccountNumber,
                    Description = statement.Description,
                    Amount = statement.Amount,
                    Balance = statement.Balance
                }).ToList()
            };

            _cicsClient.SubmitReportData(cicsRequest);
        }
    }
}