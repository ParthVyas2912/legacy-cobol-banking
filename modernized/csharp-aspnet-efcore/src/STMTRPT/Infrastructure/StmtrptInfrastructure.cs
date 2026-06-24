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
                    .HasMaxLength(10) // COBOL: STMT-ACCT-NUM PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.StmtDescription)
                    .HasColumnName("stmt_description")
                    .HasMaxLength(40) // COBOL: STMT-DESCRIPTION PIC X(40)
                    .IsRequired();

                entity.Property(e => e.StmtAmount)
                    .HasColumnName("stmt_amount")
                    .HasColumnType("NUMERIC(13, 2)") // Corrected: COBOL: STMT-AMOUNT PIC -Z(11)9.99
                    .IsRequired();

                entity.Property(e => e.StmtBalance)
                    .HasColumnName("stmt_balance")
                    .HasColumnType("NUMERIC(15, 2)") // Corrected: COBOL: STMT-BALANCE PIC -Z(13)9.99
                    .IsRequired();
            });
        }
    }

    public class Stmtout
    {
        public string StmtAcctNum { get; set; } = string.Empty; // COBOL: STMT-ACCT-NUM PIC 9(10)
        public string StmtDescription { get; set; } = string.Empty; // COBOL: STMT-DESCRIPTION PIC X(40)
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

        public async Task<StmtrptDomainModels.StatementReport?> GetByAccountNumberAsync(string accountNumber)
        {
            var stmtout = await _context.Stmtouts
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.StmtAcctNum == accountNumber);

            if (stmtout == null)
            {
                return null;
            }

            return new StmtrptDomainModels.StatementReport
            {
                AccountNumber = stmtout.StmtAcctNum,
                AccountBalance = stmtout.StmtBalance,
                Transactions = new List<StmtrptDomainModels.StatementReport.Transaction>
                {
                    new StmtrptDomainModels.StatementReport.Transaction
                    {
                        Description = stmtout.StmtDescription,
                        Amount = stmtout.StmtAmount
                    }
                }
            };
        }

        public async Task AddAsync(StmtrptDomainModels.StatementReport statementReport)
        {
            var stmtout = new Stmtout
            {
                StmtAcctNum = statementReport.AccountNumber,
                StmtBalance = statementReport.AccountBalance,
                StmtDescription = statementReport.Transactions.FirstOrDefault()?.Description ?? string.Empty,
                StmtAmount = statementReport.Transactions.FirstOrDefault()?.Amount ?? 0
            };

            await _context.Stmtouts.AddAsync(stmtout);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StmtrptDomainModels.StatementReport statementReport)
        {
            var stmtout = await _context.Stmtouts
                .SingleOrDefaultAsync(s => s.StmtAcctNum == statementReport.AccountNumber);

            if (stmtout != null)
            {
                stmtout.StmtBalance = statementReport.AccountBalance;
                stmtout.StmtDescription = statementReport.Transactions.FirstOrDefault()?.Description ?? string.Empty;
                stmtout.StmtAmount = statementReport.Transactions.FirstOrDefault()?.Amount ?? 0;

                _context.Stmtouts.Update(stmtout);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByAccountNumberAsync(string accountNumber)
        {
            var stmtout = await _context.Stmtouts
                .SingleOrDefaultAsync(s => s.StmtAcctNum == accountNumber);

            if (stmtout != null)
            {
                _context.Stmtouts.Remove(stmtout);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly StmtrptDbContext _context;

        public TransactionRepository(StmtrptDbContext context)
        {
            _context = context;
        }

        public async Task<List<StmtrptDomainModels.StatementReport.Transaction>> GetTransactionsByAccountNumberAsync(string accountNumber)
        {
            var stmtouts = await _context.Stmtouts
                .AsNoTracking()
                .Where(s => s.StmtAcctNum == accountNumber)
                .ToListAsync();

            return stmtouts.Select(s => new StmtrptDomainModels.StatementReport.Transaction
            {
                Description = s.StmtDescription,
                Amount = s.StmtAmount
            }).ToList();
        }

        public async Task AddTransactionAsync(StmtrptDomainModels.StatementReport.Transaction transaction)
        {
            var stmtout = new Stmtout
            {
                StmtAcctNum = transaction.TransactionId,
                StmtDescription = transaction.Description,
                StmtAmount = transaction.Amount
            };

            await _context.Stmtouts.AddAsync(stmtout);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransactionAsync(StmtrptDomainModels.StatementReport.Transaction transaction)
        {
            var stmtout = await _context.Stmtouts
                .SingleOrDefaultAsync(s => s.StmtAcctNum == transaction.TransactionId);

            if (stmtout != null)
            {
                stmtout.StmtDescription = transaction.Description;
                stmtout.StmtAmount = transaction.Amount;

                _context.Stmtouts.Update(stmtout);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTransactionByIdAsync(string transactionId)
        {
            var stmtout = await _context.Stmtouts
                .SingleOrDefaultAsync(s => s.StmtAcctNum == transactionId);

            if (stmtout != null)
            {
                _context.Stmtouts.Remove(stmtout);
                await _context.SaveChangesAsync();
            }
        }
    }
}