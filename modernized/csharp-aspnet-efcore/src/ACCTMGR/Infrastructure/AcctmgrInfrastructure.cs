using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ACCTMGR.Domain;
using ACCTMGR.Application.Interfaces;

namespace ACCTMGR.Infrastructure
{
    public class AcctmgrDbContext : DbContext
    {
        public AcctmgrDbContext(DbContextOptions<AcctmgrDbContext> options) : base(options) { }

        public DbSet<AccountManager> Accounts { get; set; }
        public DbSet<AccountManager.Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountManager>(entity =>
            {
                entity.ToTable("acctfile", "public");

                entity.HasKey(e => e.AccountNumber);
                entity.Property(e => e.AccountNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10) // PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.AccountName)
                    .HasColumnName("acct_name")
                    .HasMaxLength(30) // PIC X(30)
                    .IsRequired();

                entity.Property(e => e.AccountBalance)
                    .HasColumnName("acct_balance")
                    .HasPrecision(15, 2) // PIC S9(13)V99 COMP-3
                    .IsRequired();

                entity.Property(e => e.AccountType)
                    .HasColumnName("acct_type")
                    .HasMaxLength(3) // PIC X(03)
                    .IsRequired();

                entity.Property(e => e.AccountStatus)
                    .HasColumnName("acct_status")
                    .HasMaxLength(1) // PIC X(01)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasConversion<string>()
                    .IsRequired();
            });

            modelBuilder.Entity<AccountManager.Transaction>(entity =>
            {
                entity.ToTable("acct_transactions", "public");

                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.TransactionId)
                    .HasColumnName("txn_id")
                    .HasMaxLength(6) // PIC X(06)
                    .IsRequired();

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("txn_amount")
                    .HasPrecision(13, 2) // PIC S9(11)V99 COMP-3
                    .IsRequired();

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("txn_date")
                    .HasMaxLength(8) // PIC X(08)
                    .IsRequired();

                entity.Property(e => e.TransactionType)
                    .HasColumnName("txn_type")
                    .HasMaxLength(1) // PIC X(01)
                    .IsRequired();
            });
        }
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly AcctmgrDbContext _context;

        public AccountRepository(AcctmgrDbContext context)
        {
            _context = context;
        }

        public async Task<AccountManager?> GetByAccountNumberAsync(string accountNumber)
        {
            return await _context.Accounts
                .Include(a => a.Transactions)
                .SingleOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task<List<AccountManager>> GetByAccountTypeAsync(string accountType)
        {
            return await _context.Accounts
                .Where(a => a.AccountType == accountType)
                .ToListAsync();
        }

        public async Task AddAccountAsync(AccountManager account)
        {
            await _context.Accounts.AddAsync(account);
        }

        public async Task UpdateAccountAsync(AccountManager account)
        {
            _context.Accounts.Update(account);
        }

        public async Task DeleteAccountAsync(string accountNumber)
        {
            var account = await GetByAccountNumberAsync(accountNumber);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AcctmgrDbContext _context;

        public TransactionRepository(AcctmgrDbContext context)
        {
            _context = context;
        }

        public async Task<List<AccountManager.Transaction>> GetTransactionsByAccountAsync(string accountNumber)
        {
            return await _context.Transactions
                .Where(t => t.TransactionId.StartsWith(accountNumber))
                .ToListAsync();
        }

        public async Task AddTransactionAsync(AccountManager.Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

    public static class AcctmgrInfrastructureExtensions
    {
        public static IServiceCollection AddAcctmgrInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AcctmgrDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}