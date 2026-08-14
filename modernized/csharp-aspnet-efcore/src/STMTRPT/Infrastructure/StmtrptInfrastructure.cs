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
                    .HasMaxLength(10)
                    .IsFixedLength(); // Ensure fixed length for PIC 9(10)

                entity.Property(e => e.StmtDescription)
                    .HasColumnName("stmt_description")
                    .HasMaxLength(40)
                    .IsUnicode(false); // Ensure non-Unicode for PIC X(40)

                entity.Property(e => e.StmtAmount)
                    .HasColumnName("stmt_amount")
                    .HasPrecision(14, 2); // COBOL: STMT-AMOUNT PIC -Z(11)9.99

                entity.Property(e => e.StmtBalance)
                    .HasColumnName("stmt_balance")
                    .HasPrecision(16, 2); // COBOL: STMT-BALANCE PIC -Z(13)9.99
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

        public StmtrptDomainModels.StatementReport ReadStatementReport(string reportId)
        {
            var stmtout = _context.Stmtouts.FirstOrDefault(s => s.StmtAcctNum == reportId);
            if (stmtout == null)
            {
                throw new KeyNotFoundException($"Statement report with ID {reportId} not found.");
            }

            return new StmtrptDomainModels.StatementReport
            {
                StmtAcctNum = stmtout.StmtAcctNum,
                StmtDescription = stmtout.StmtDescription,
                StmtAmount = stmtout.StmtAmount,
                StmtBalance = stmtout.StmtBalance
            };
        }

        public void WriteStatementReport(StmtrptDomainModels.StatementReport report)
        {
            var stmtout = new Stmtout
            {
                StmtAcctNum = report.StmtAcctNum,
                StmtDescription = report.StmtDescription,
                StmtAmount = report.StmtAmount,
                StmtBalance = report.StmtBalance
            };

            _context.Stmtouts.Add(stmtout);
            _context.SaveChanges();
        }

        public void DeleteStatementReport(string reportId)
        {
            var stmtout = _context.Stmtouts.FirstOrDefault(s => s.StmtAcctNum == reportId);
            if (stmtout == null)
            {
                throw new KeyNotFoundException($"Statement report with ID {reportId} not found.");
            }

            _context.Stmtouts.Remove(stmtout);
            _context.SaveChanges();
        }

        public void UpdateStatementReport(StmtrptDomainModels.StatementReport report)
        {
            var stmtout = _context.Stmtouts.FirstOrDefault(s => s.StmtAcctNum == report.StmtAcctNum);
            if (stmtout == null)
            {
                throw new KeyNotFoundException($"Statement report with ID {report.StmtAcctNum} not found.");
            }

            stmtout.StmtDescription = report.StmtDescription;
            stmtout.StmtAmount = report.StmtAmount;
            stmtout.StmtBalance = report.StmtBalance;

            _context.Stmtouts.Update(stmtout);
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

        public StmtrptDomainModels.StatementReport GetStatementReportFromCICS(string reportId)
        {
            var response = _cicsClient.GetReport(reportId);
            if (response == null)
            {
                throw new KeyNotFoundException($"Statement report with ID {reportId} not found in CICS.");
            }

            return new StmtrptDomainModels.StatementReport
            {
                StmtAcctNum = response.StmtAcctNum,
                StmtDescription = response.StmtDescription,
                StmtAmount = response.StmtAmount,
                StmtBalance = response.StmtBalance
            };
        }

        public void SendStatementReportToCICS(StmtrptDomainModels.StatementReport report)
        {
            var cicsReport = new CicsReport
            {
                StmtAcctNum = report.StmtAcctNum,
                StmtDescription = report.StmtDescription,
                StmtAmount = report.StmtAmount,
                StmtBalance = report.StmtBalance
            };

            _cicsClient.SendReport(cicsReport);
        }

        public void DeleteStatementReportFromCICS(string reportId)
        {
            _cicsClient.DeleteReport(reportId);
        }

        public void UpdateStatementReportInCICS(StmtrptDomainModels.StatementReport report)
        {
            var cicsReport = new CicsReport
            {
                StmtAcctNum = report.StmtAcctNum,
                StmtDescription = report.StmtDescription,
                StmtAmount = report.StmtAmount,
                StmtBalance = report.StmtBalance
            };

            _cicsClient.UpdateReport(cicsReport);
        }
    }
}