using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using STMTRPT.Application.Interfaces;
using STMTRPT.Domain;

namespace STMTRPT.Infrastructure
{
    public class StmtrptDbContext : DbContext
    {
        public StmtrptDbContext(DbContextOptions<StmtrptDbContext> options) : base(options) { }

        public DbSet<Stmtout> Stmtout { get; set; }

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
                    .HasPrecision(14, 2) // COBOL: STMT-AMOUNT PIC -Z(11)9.99
                    .IsRequired();

                entity.Property(e => e.StmtBalance)
                    .HasColumnName("stmt_balance")
                    .HasPrecision(16, 2) // COBOL: STMT-BALANCE PIC -Z(13)9.99
                    .IsRequired();
            });
        }
    }

    public class Stmtout
    {
        [MaxLength(10)]
        [Required]
        public string StmtAcctNum { get; set; } // COBOL: STMT-ACCT-NUM PIC 9(10)

        [MaxLength(40)]
        [Required]
        public string StmtDescription { get; set; } // COBOL: STMT-DESCRIPTION PIC X(40)

        [Precision(14, 2)]
        [Required]
        public decimal StmtAmount { get; set; } // COBOL: STMT-AMOUNT PIC -Z(11)9.99

        [Precision(16, 2)]
        [Required]
        public decimal StmtBalance { get; set; } // COBOL: STMT-BALANCE PIC -Z(13)9.99
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly StmtrptDbContext _context;

        public AccountRepository(StmtrptDbContext context)
        {
            _context = context;
        }

        public async Task<List<StmtrptDomainModels.StatementReport>> GetAccountRecordsAsync()
        {
            var stmtoutRecords = await _context.Stmtout.ToListAsync();
            var statementReports = new List<StmtrptDomainModels.StatementReport>();

            foreach (var record in stmtoutRecords)
            {
                var statementReport = new StmtrptDomainModels.StatementReport
                {
                    AccountNumber = int.Parse(record.StmtAcctNum),
                    AccountHolderName = record.StmtDescription,
                    AccountBalance = record.StmtBalance,
                    Transactions = new List<StmtrptDomainModels.StatementReport.Transaction>
                    {
                        new StmtrptDomainModels.StatementReport.Transaction
                        {
                            TransactionAmount = record.StmtAmount,
                            TransactionDescription = record.StmtDescription
                        }
                    }
                };

                statementReports.Add(statementReport);
            }

            return statementReports;
        }

        public async Task SaveStatementReportAsync(StmtrptDomainModels.StatementReport report)
        {
            var stmtout = new Stmtout
            {
                StmtAcctNum = report.AccountNumber.ToString(),
                StmtDescription = report.AccountHolderName,
                StmtAmount = report.Transactions.Count > 0 ? report.Transactions[0].TransactionAmount : 0,
                StmtBalance = report.AccountBalance
            };

            _context.Stmtout.Add(stmtout);
            await _context.SaveChangesAsync();
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly StmtrptDbContext _context;

        public TransactionRepository(StmtrptDbContext context)
        {
            _context = context;
        }

        public async Task<List<StmtrptDomainModels.StatementReport.Transaction>> GetTransactionsByAccountAsync(int accountNumber)
        {
            var stmtoutRecords = await _context.Stmtout
                .Where(s => s.StmtAcctNum == accountNumber.ToString())
                .ToListAsync();

            var transactions = new List<StmtrptDomainModels.StatementReport.Transaction>();

            foreach (var record in stmtoutRecords)
            {
                transactions.Add(new StmtrptDomainModels.StatementReport.Transaction
                {
                    TransactionAmount = record.StmtAmount,
                    TransactionDescription = record.StmtDescription
                });
            }

            return transactions;
        }
    }

    public class CicsService : ICicsService
    {
        public Task<string> GetAccountStatusAsync(int accountNumber)
        {
            // Placeholder for actual CICS call
            return Task.FromResult("Active");
        }

        public Task UpdateAccountStatusAsync(int accountNumber, StmtrptDomainModels.StatementReport.AccountStatusEnum status)
        {
            // Placeholder for actual CICS call
            return Task.CompletedTask;
        }
    }
}