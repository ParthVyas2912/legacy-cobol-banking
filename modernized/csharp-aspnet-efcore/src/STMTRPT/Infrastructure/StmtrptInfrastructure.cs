using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using STMTRPT.Application.Interfaces;
using STMTRPT.Domain;

namespace STMTRPT.Infrastructure
{
    public class StmtrptDbContext : DbContext
    {
        public DbSet<Stmtout> Stmtouts { get; set; }

        public StmtrptDbContext(DbContextOptions<StmtrptDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stmtout>(entity =>
            {
                entity.ToTable("stmtout", "public");

                entity.Property(e => e.StmtAcctNum)
                    .HasColumnName("stmt_acct_num")
                    .HasColumnType("char(10)") // Corrected to match COBOL PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.StmtDescription)
                    .HasColumnName("stmt_description")
                    .HasMaxLength(40) // Correctly matches COBOL PIC X(40)
                    .IsRequired();

                entity.Property(e => e.StmtAmount)
                    .HasColumnName("stmt_amount")
                    .HasPrecision(13, 2) // Corrected precision to match COBOL PIC -Z(11)9.99
                    .IsRequired();

                entity.Property(e => e.StmtBalance)
                    .HasColumnName("stmt_balance")
                    .HasPrecision(15, 2) // Corrected precision to match COBOL PIC -Z(13)9.99
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

    public class AccountRepository : IAccountRepository
    {
        private readonly StmtrptDbContext _context;

        public AccountRepository(StmtrptDbContext context)
        {
            _context = context;
        }

        public async Task<StmtrptDomainModels.AccountSummary[]> GetAllAccountsAsync()
        {
            var accounts = await _context.Stmtouts
                .GroupBy(s => s.StmtAcctNum)
                .Select(g => new StmtrptDomainModels.AccountSummary
                {
                    AccountNumber = g.Key,
                    AccountBalance = g.Sum(x => x.StmtBalance),
                    Transactions = g.Select(x => new StmtrptDomainModels.Transaction
                    {
                        Description = x.StmtDescription,
                        TransactionAmount = x.StmtAmount
                    }).ToArray()
                })
                .ToArrayAsync();

            return accounts;
        }

        public async Task<StmtrptDomainModels.Transaction[]> GetTransactionsByAccountAsync(string accountNumber)
        {
            var transactions = await _context.Stmtouts
                .Where(s => s.StmtAcctNum == accountNumber)
                .Select(s => new StmtrptDomainModels.Transaction
                {
                    Description = s.StmtDescription,
                    TransactionAmount = s.StmtAmount
                })
                .ToArrayAsync();

            return transactions;
        }
    }

    public class ReportRepository : IReportRepository
    {
        private readonly StmtrptDbContext _context;

        public ReportRepository(StmtrptDbContext context)
        {
            _context = context;
        }

        public async Task SaveReportHeaderAsync(StmtrptDomainModels.ReportHeader header)
        {
            // Implementation for saving report header (e.g., to a separate table or logging system)
            await Task.CompletedTask;
        }

        public async Task SaveAccountSummaryAsync(StmtrptDomainModels.AccountSummary accountSummary)
        {
            // Implementation for saving account summary (e.g., to a separate table or logging system)
            await Task.CompletedTask;
        }

        public async Task SaveReportFooterAsync(StmtrptDomainModels.ReportFooter footer)
        {
            // Implementation for saving report footer (e.g., to a separate table or logging system)
            await Task.CompletedTask;
        }

        public async Task<StmtrptDomainModels.StatementReport> GetStatementReportAsync()
        {
            // Implementation for retrieving the full statement report
            return await Task.FromResult(new StmtrptDomainModels.StatementReport());
        }
    }
}