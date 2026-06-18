using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCHEOD.Application.Interfaces;
using BCHEOD.Domain;

namespace BCHEOD.Infrastructure
{
    public class BcheodDbContext : DbContext
    {
        public DbSet<EodRuns> EodRuns { get; set; }
        public DbSet<BcheodDomainModels.AccountRecord> AccountRecords { get; set; }
        public DbSet<BcheodDomainModels.AccountRecord.TransactionRecord> TransactionRecords { get; set; }

        public BcheodDbContext(DbContextOptions<BcheodDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EodRuns>(entity =>
            {
                entity.ToTable("eod_runs", "public");

                entity.Property(e => e.StartedAt)
                    .HasColumnName("started_at")
                    .HasColumnType("timestamptz"); // COBOL: STARTED_AT TIMESTAMPTZ

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(20) // Assuming a max length of 20 for status
                    .IsRequired(); // Assuming status is required

                entity.Property(e => e.EndedAt)
                    .HasColumnName("ended_at")
                    .HasColumnType("timestamptz"); // COBOL: ENDED_AT TIMESTAMPTZ

                entity.Property(e => e.WsStatusCode)
                    .HasColumnName("ws_status_code")
                    .HasMaxLength(10) // Assuming a max length of 10 for ws_status_code
                    .IsRequired(); // Assuming ws_status_code is required
            });

            modelBuilder.Entity<BcheodDomainModels.AccountRecord>(entity =>
            {
                entity.ToTable("account_records", "public");

                entity.HasKey(e => e.AccountNumber);

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(20) // Assuming a max length of 20 for account_number
                    .IsRequired();

                entity.Property(e => e.AccountName)
                    .HasColumnName("account_name")
                    .HasMaxLength(100) // Assuming a max length of 100 for account_name
                    .IsRequired();

                entity.Property(e => e.Balance)
                    .HasColumnName("balance")
                    .HasPrecision(18, 2) // Assuming a precision of 18, scale of 2 for balance
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasConversion<int>() // Assuming status is an enum
                    .IsRequired();
            });

            modelBuilder.Entity<BcheodDomainModels.AccountRecord.TransactionRecord>(entity =>
            {
                entity.ToTable("transaction_records", "public");

                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .IsRequired();

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(20) // Assuming a max length of 20 for account_number
                    .IsRequired();

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .HasConversion<int>() // Assuming transaction_type is an enum
                    .IsRequired();

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(18, 2) // Assuming a precision of 18, scale of 2 for amount
                    .IsRequired();

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasColumnType("timestamptz") // Assuming transaction_date is a timestamp
                    .IsRequired();
            });
        }
    }

    public class EodRuns
    {
        public DateTime StartedAt { get; set; }
        public string Status { get; set; } // Changed to string to match the assumption of max length
        public DateTime EndedAt { get; set; }
        public string WsStatusCode { get; set; } // Changed to string to match the assumption of max length
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly BcheodDbContext _dbContext;

        public AccountRepository(BcheodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BcheodDomainModels.AccountRecord[]> ReadAllAccountsAsync()
        {
            return await _dbContext.AccountRecords.ToArrayAsync();
        }

        public async Task WriteAccountAsync(BcheodDomainModels.AccountRecord account)
        {
            var existingAccount = await _dbContext.AccountRecords
                .FirstOrDefaultAsync(a => a.AccountNumber == account.AccountNumber);

            if (existingAccount != null)
            {
                _dbContext.Entry(existingAccount).CurrentValues.SetValues(account);
            }
            else
            {
                await _dbContext.AccountRecords.AddAsync(account);
            }

            await _dbContext.SaveChangesAsync();
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly BcheodDbContext _dbContext;

        public TransactionRepository(BcheodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BcheodDomainModels.AccountRecord.TransactionRecord[]> ReadTransactionsAsync(int accountNumber)
        {
            return await _dbContext.TransactionRecords
                .Where(t => t.AccountNumber == accountNumber)
                .ToArrayAsync();
        }

        public async Task WriteTransactionAsync(int accountNumber, BcheodDomainModels.AccountRecord.TransactionRecord transaction)
        {
            await _dbContext.TransactionRecords.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
        }
    }

    public class CicsService : ICicsService
    {
        public async Task<bool> ValidateAccountAsync(int accountNumber)
        {
            await Task.Delay(100); // Simulate network latency
            return true; // Simulate a successful validation
        }

        public async Task<bool> UpdateAccountStatusAsync(int accountNumber, BcheodDomainModels.AccountStatusEnum status)
        {
            await Task.Delay(100); // Simulate network latency
            return true; // Simulate a successful update
        }
    }
}