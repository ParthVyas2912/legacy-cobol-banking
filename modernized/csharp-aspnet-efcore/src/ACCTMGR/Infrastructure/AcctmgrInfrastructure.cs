using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ACCTMGR.Application.Interfaces;
using ACCTMGR.Domain;

namespace ACCTMGR.Infrastructure
{
    public class AcctmgrDbContext : DbContext
    {
        public DbSet<Acctfile> Acctfiles { get; set; }
        public DbSet<AcctAudit> AcctAudits { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public AcctmgrDbContext(DbContextOptions<AcctmgrDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acctfile>(entity =>
            {
                entity.ToTable("acctfile", "public");
                entity.HasKey(e => e.AcctNumber);
                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasColumnType("decimal(10, 0)"); // Corrected to match PIC 9(10)
                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasColumnType("decimal(8, 0)"); // Corrected to match PIC 9(8)
                entity.Property(e => e.AcctOpenedDate)
                    .HasColumnName("acct_opened_date")
                    .HasColumnType("date"); // Correct as is
                entity.Property(e => e.AcctType)
                    .HasColumnName("acct_type")
                    .HasMaxLength(2); // Correct as is for PIC X(02)
                entity.Property(e => e.AcctStatus)
                    .HasColumnName("acct_status")
                    .HasMaxLength(1); // Correct as is for PIC X(01)
                entity.Property(e => e.AcctBalance)
                    .HasColumnName("acct_balance")
                    .HasPrecision(15, 2); // Correct as is for PIC S9(13)V99 COMP-3
                entity.Property(e => e.AcctAvailBal)
                    .HasColumnName("acct_avail_bal")
                    .HasPrecision(15, 2); // Correct as is for PIC S9(13)V99 COMP-3
                entity.Property(e => e.AcctIntRate)
                    .HasColumnName("acct_int_rate")
                    .HasPrecision(6, 4); // Correct as is for PIC 9(2)V9(4) COMP-3
                entity.Property(e => e.AcctLastTxnDt)
                    .HasColumnName("acct_last_txn_dt")
                    .HasColumnType("date"); // Correct as is
                entity.Property(e => e.AcctCurrency)
                    .HasColumnName("acct_currency")
                    .HasMaxLength(3); // Correct as is for PIC X(03)
                entity.Property(e => e.AcctBranchCode)
                    .HasColumnName("acct_branch_code")
                    .HasColumnType("decimal(4, 0)"); // Corrected to match PIC 9(4)
                entity.Property(e => e.AcctFlags)
                    .HasColumnName("acct_flags")
                    .HasMaxLength(4); // Correct as is for PIC X(04)
                entity.Property(e => e.AcctFiller)
                    .HasColumnName("acct_filler")
                    .HasMaxLength(50); // Correct as is for PIC X(50)
            });

            modelBuilder.Entity<AcctAudit>(entity =>
            {
                entity.ToTable("acct_audit", "public");
                entity.Property(e => e.AcctNum)
                    .HasColumnName("acct_num")
                    .HasMaxLength(10); // Added HasMaxLength to match ACCT-NUMBER PIC 9(10)
                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasMaxLength(50); // Added HasMaxLength for consistency
                entity.Property(e => e.Actor)
                    .HasColumnName("actor")
                    .HasMaxLength(50); // Added HasMaxLength for consistency
                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("timestamptz"); // Correct as is
                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10); // Added HasMaxLength to match ACCT-NUMBER PIC 9(10)
                entity.Property(e => e.CommFunction)
                    .HasColumnName("comm_function")
                    .HasMaxLength(20); // Added HasMaxLength for consistency
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer", "public");
                entity.Property(e => e.ActiveAcctCount)
                    .HasColumnName("active_acct_count")
                    .HasColumnType("decimal(5, 0)"); // Corrected to match PIC 9(5)
                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasColumnType("decimal(8, 0)"); // Corrected to match PIC 9(8)
            });
        }
    }

    public class Acctfile
    {
        public decimal AcctNumber { get; set; } // Corrected to match PIC 9(10)
        public decimal AcctCustomerId { get; set; } // Corrected to match PIC 9(8)
        public DateTime AcctOpenedDate { get; set; } // Correct as is
        public string AcctType { get; set; } = string.Empty; // Correct as is for PIC X(02)
        public string AcctStatus { get; set; } = string.Empty; // Correct as is for PIC X(01)
        public decimal AcctBalance { get; set; } // Correct as is for PIC S9(13)V99 COMP-3
        public decimal AcctAvailBal { get; set; } // Correct as is for PIC S9(13)V99 COMP-3
        public decimal AcctIntRate { get; set; } // Correct as is for PIC 9(2)V9(4) COMP-3
        public DateTime AcctLastTxnDt { get; set; } // Correct as is
        public string AcctCurrency { get; set; } = string.Empty; // Correct as is for PIC X(03)
        public decimal AcctBranchCode { get; set; } // Corrected to match PIC 9(4)
        public string AcctFlags { get; set; } = string.Empty; // Correct as is for PIC X(04)
        public string AcctFiller { get; set; } = string.Empty; // Correct as is for PIC X(50)
    }

    public class AcctAudit
    {
        public string AcctNum { get; set; } = string.Empty; // Corrected to match ACCT-NUMBER PIC 9(10)
        public string Action { get; set; } = string.Empty; // Correct as is
        public string Actor { get; set; } = string.Empty; // Correct as is
        public DateTime Ts { get; set; } // Correct as is
        public string AcctNumber { get; set; } = string.Empty; // Corrected to match ACCT-NUMBER PIC 9(10)
        public string CommFunction { get; set; } = string.Empty; // Correct as is
    }

    public class Customer
    {
        public decimal ActiveAcctCount { get; set; } // Corrected to match PIC 9(5)
        public decimal AcctCustomerId { get; set; } // Corrected to match PIC 9(8)
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly AcctmgrDbContext _dbContext;

        public CustomerRepository(AcctmgrDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AccountManager.CustomerRecord?> ReadCustomerAsync(string customerId)
        {
            var customer = await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.AcctCustomerId.ToString() == customerId);

            if (customer == null)
                return null;

            return new AccountManager.CustomerRecord
            {
                CustomerId = customer.AcctCustomerId.ToString()
            };
        }

        public async Task WriteCustomerAsync(AccountManager.CustomerRecord customer)
        {
            var entity = new Customer
            {
                AcctCustomerId = decimal.Parse(customer.CustomerId)
            };

            _dbContext.Customers.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly AcctmgrDbContext _dbContext;

        public AccountRepository(AcctmgrDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AccountManager.CustomerRecord.AccountRecord[]> ReadAccountsAsync(string customerId)
        {
            var accounts = await _dbContext.Acctfiles
                .Where(a => a.AcctCustomerId.ToString() == customerId)
                .ToListAsync();

            return accounts.Select(a => new AccountManager.CustomerRecord.AccountRecord
            {
                AccountNumber = a.AcctNumber.ToString(),
                Balance = a.AcctBalance,
                AccountType = Enum.TryParse<AccountManager.CustomerRecord.AccountRecord.AccountTypeEnum>(a.AcctType, out var accountType) ? accountType : default
            }).ToArray();
        }

        public async Task WriteAccountsAsync(string customerId, AccountManager.CustomerRecord.AccountRecord[] accounts)
        {
            var entities = accounts.Select(a => new Acctfile
            {
                AcctNumber = decimal.Parse(a.AccountNumber),
                AcctCustomerId = decimal.Parse(customerId),
                AcctBalance = a.Balance,
                AcctType = a.AccountType.ToString()
            }).ToList();

            _dbContext.Acctfiles.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }
    }
}