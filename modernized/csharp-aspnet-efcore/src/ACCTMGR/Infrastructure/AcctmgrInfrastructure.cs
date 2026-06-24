using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ACCTMGR.Domain;
using ACCTMGR.Application.Interfaces;

namespace ACCTMGR.Infrastructure
{
    public class AcctmgrDbContext : DbContext
    {
        public AcctmgrDbContext(DbContextOptions<AcctmgrDbContext> options) : base(options) { }

        public DbSet<AccountManager.Account> Accounts { get; set; }
        public DbSet<AccountManager.Customer> Customers { get; set; }
        public DbSet<AccountManager.Transaction> Transactions { get; set; }
        public DbSet<AccountManager.AccountSummary> AccountSummaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountManager.Account>(entity =>
            {
                entity.ToTable("acctfile", "public");

                entity.HasKey(e => e.AccountNumber);
                entity.Property(e => e.AccountNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.AccountStatus)
                    .HasColumnName("acct_status")
                    .HasMaxLength(1); // COBOL: ACCT-STATUS PIC X(01)

                entity.Property(e => e.Balance)
                    .HasColumnName("acct_balance")
                    .HasPrecision(15, 2); // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3
            });

            modelBuilder.Entity<AccountManager.Customer>(entity =>
            {
                entity.ToTable("customer", "public");

                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.CustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasMaxLength(15); // COBOL: ACCT-CUSTOMER-ID PIC X(15)

                entity.Property(e => e.CustomerName)
                    .HasColumnName("customer_name")
                    .HasMaxLength(50); // COBOL: CUSTOMER-NAME PIC X(50)

                entity.Property(e => e.CustomerType)
                    .HasColumnName("customer_type")
                    .HasMaxLength(1); // COBOL: CUSTOMER-TYPE PIC X(01)
            });

            modelBuilder.Entity<AccountManager.Transaction>(entity =>
            {
                entity.ToTable("acct_audit", "public");

                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(12); // COBOL: TRANSACTION-ID PIC X(12)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(13, 2); // COBOL: TRANSACTION-AMOUNT PIC S9(11)V99 COMP-3

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasMaxLength(8); // COBOL: TRANSACTION-DATE PIC X(8)

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .HasMaxLength(1); // COBOL: TRANSACTION-TYPE PIC X(01)
            });

            modelBuilder.Entity<AccountManager.AccountSummary>(entity =>
            {
                entity.ToTable("acct_summary", "public");

                entity.HasKey(e => e.AccountNumber);
                entity.Property(e => e.AccountNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10); // COBOL: ACCT-NUMBER PIC X(10)

                entity.Property(e => e.TotalCredits)
                    .HasColumnName("total_credits")
                    .HasPrecision(15, 2); // COBOL: TOTAL-CREDITS PIC S9(13)V99 COMP-3

                entity.Property(e => e.TotalDebits)
                    .HasColumnName("total_debits")
                    .HasPrecision(15, 2); // COBOL: TOTAL-DEBITS PIC S9(13)V99 COMP-3

                entity.Property(e => e.NetBalance)
                    .HasColumnName("net_balance")
                    .HasPrecision(15, 2); // COBOL: NET-BALANCE PIC S9(13)V99 COMP-3
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

        public async Task<AccountManager.Account?> GetAccountByNumberAsync(string accountNumber)
        {
            return await _context.Accounts
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task<IEnumerable<AccountManager.Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAccountAsync(AccountManager.Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(AccountManager.Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(string accountNumber)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly AcctmgrDbContext _context;

        public CustomerRepository(AcctmgrDbContext context)
        {
            _context = context;
        }

        public async Task<AccountManager.Customer?> GetCustomerByIdAsync(string customerId)
        {
            return await _context.Customers
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task<IEnumerable<AccountManager.Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddCustomerAsync(AccountManager.Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(AccountManager.Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(string customerId)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}