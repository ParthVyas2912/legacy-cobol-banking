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
                    .HasMaxLength(10) // Corrected to match PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.StmtDescription)
                    .HasColumnName("stmt_description")
                    .HasMaxLength(40) // Corrected to match PIC X(40)
                    .IsRequired();

                entity.Property(e => e.StmtAmount)
                    .HasColumnName("stmt_amount")
                    .HasPrecision(13, 2) // Corrected to match PIC -Z(11)9.99
                    .IsRequired();

                entity.Property(e => e.StmtBalance)
                    .HasColumnName("stmt_balance")
                    .HasPrecision(15, 2) // Corrected to match PIC -Z(13)9.99
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

        public async Task<IEnumerable<StmtrptDomainModels.AccountRecord>> GetAllAccountsAsync()
        {
            // This method would need to map the database entities to the domain models
            // Assuming a mapping function exists to convert Stmtout to AccountRecord
            return await _context.Stmtouts
                .Select(stmt => new StmtrptDomainModels.AccountRecord
                {
                    AccountNumber = stmt.StmtAcctNum,
                    AccountName = stmt.StmtDescription, // Assuming description is used as account name
                    AccountBalance = stmt.StmtBalance,
                    AvailableBalance = stmt.StmtAmount, // Assuming amount is used as available balance
                    AccountStatus = StmtrptDomainModels.AccountStatusEnum.Active // Defaulting to Active
                })
                .ToListAsync();
        }

        public async Task UpdateAccountAsync(StmtrptDomainModels.AccountRecord account)
        {
            // Assuming a mapping function exists to convert AccountRecord to Stmtout
            var stmtout = await _context.Stmtouts
                .FirstOrDefaultAsync(s => s.StmtAcctNum == account.AccountNumber);

            if (stmtout != null)
            {
                stmtout.StmtDescription = account.AccountName;
                stmtout.StmtBalance = account.AccountBalance;
                stmtout.StmtAmount = account.AvailableBalance;

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

        public async Task<IEnumerable<StmtrptDomainModels.TransactionRecord>> GetTransactionsByAccountAsync(string accountNumber)
        {
            // Assuming a mapping function exists to convert Stmtout to TransactionRecord
            return await _context.Stmtouts
                .Where(stmt => stmt.StmtAcctNum == accountNumber)
                .Select(stmt => new StmtrptDomainModels.TransactionRecord
                {
                    TransactionDate = "000000", // Placeholder for transaction date
                    TransactionDescription = stmt.StmtDescription,
                    TransactionAmount = stmt.StmtAmount,
                    TransactionType = stmt.StmtAmount >= 0
                        ? StmtrptDomainModels.TransactionTypeEnum.Credit
                        : StmtrptDomainModels.TransactionTypeEnum.Debit
                })
                .ToListAsync();
        }
    }
}