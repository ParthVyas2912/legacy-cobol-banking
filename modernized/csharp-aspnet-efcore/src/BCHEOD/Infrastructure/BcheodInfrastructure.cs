using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCHEOD.Application.Interfaces;
using BCHEOD.Domain;

namespace BCHEOD.Infrastructure
{
    public class BcheodDbContext : DbContext
    {
        public BcheodDbContext(DbContextOptions<BcheodDbContext> options) : base(options)
        {
        }

        public DbSet<EodRuns> EodRuns { get; set; }
        public DbSet<BcheodDomainModels.Account> Accounts { get; set; }
        public DbSet<BcheodDomainModels.Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EodRuns>(entity =>
            {
                entity.ToTable("eod_runs", "public");

                entity.Property(e => e.StartedAt)
                    .HasColumnName("started_at")
                    .HasColumnType("timestamptz");
                // COBOL: STARTED_AT TIMESTAMPTZ

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("timestamptz");
                // COBOL: STATUS TIMESTAMPTZ

                entity.Property(e => e.EndedAt)
                    .HasColumnName("ended_at")
                    .HasColumnType("timestamptz");
                // COBOL: ENDED_AT TIMESTAMPTZ

                entity.Property(e => e.WsStatusCode)
                    .HasColumnName("ws_status_code")
                    .HasColumnType("timestamptz");
                // COBOL: WS_STATUS_CODE TIMESTAMPTZ
            });

            modelBuilder.Entity<BcheodDomainModels.Account>(entity =>
            {
                entity.ToTable("accounts", "public");

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(20);
                // COBOL: ACCOUNT_NUMBER X(20)

                entity.Property(e => e.AccountName)
                    .HasColumnName("account_name")
                    .HasMaxLength(50);
                // COBOL: ACCOUNT_NAME X(50)

                entity.Property(e => e.Balance)
                    .HasColumnName("balance")
                    .HasPrecision(15, 2);
                // COBOL: BALANCE 9(13)V99 COMP-3

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(1);
                // COBOL: STATUS X(1)
            });

            modelBuilder.Entity<BcheodDomainModels.Transaction>(entity =>
            {
                entity.ToTable("transactions", "public");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(30);
                // COBOL: TRANSACTION_ID X(30)

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(20);
                // COBOL: ACCOUNT_NUMBER X(20)

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(15, 2);
                // COBOL: AMOUNT 9(13)V99 COMP-3

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .HasMaxLength(1);
                // COBOL: TRANSACTION_TYPE X(1)

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasColumnType("date");
                // COBOL: TRANSACTION_DATE DATE
            });
        }
    }

    public class EodRuns
    {
        public DateTime StartedAt { get; set; }
        public DateTime Status { get; set; }
        public DateTime EndedAt { get; set; }
        public DateTime WsStatusCode { get; set; }
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly BcheodDbContext _context;

        public AccountRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BcheodDomainModels.Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task UpdateAccountAsync(BcheodDomainModels.Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly BcheodDbContext _context;

        public TransactionRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BcheodDomainModels.Transaction>> GetTransactionsByAccountAsync(string accountNumber)
        {
            return await _context.Transactions
                .Where(t => t.AccountNumber == accountNumber)
                .ToListAsync();
        }

        public async Task AddTransactionAsync(BcheodDomainModels.Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
    }

    public class CicsService : ICicsService
    {
        public async Task CallExternalServiceAsync(string serviceName, object requestPayload)
        {
            // Implementation for calling an external CICS service
            await Task.CompletedTask;
        }
    }
}