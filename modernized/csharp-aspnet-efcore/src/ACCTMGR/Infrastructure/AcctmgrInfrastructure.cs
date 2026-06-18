using System;
using System.Collections.Generic;
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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Acctfile>(entity =>
            {
                entity.ToTable("acctfile", "public");
                entity.HasKey(e => e.AcctNumber);

                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10)
                    .IsFixedLength(); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasMaxLength(8)
                    .IsFixedLength(); // COBOL: ACCT-CUSTOMER-ID PIC 9(8)

                entity.Property(e => e.AcctOpenedDate)
                    .HasColumnName("acct_opened_date")
                    .HasColumnType("DATE"); // COBOL: ACCT-OPENED-DATE PIC 9(8)

                entity.Property(e => e.AcctType)
                    .HasColumnName("acct_type")
                    .HasMaxLength(2)
                    .IsFixedLength(); // COBOL: ACCT-TYPE PIC X(02)

                entity.Property(e => e.AcctStatus)
                    .HasColumnName("acct_status")
                    .HasMaxLength(1)
                    .IsFixedLength(); // COBOL: ACCT-STATUS PIC X(01)

                entity.Property(e => e.AcctBalance)
                    .HasColumnName("acct_balance")
                    .HasPrecision(15, 2); // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3

                entity.Property(e => e.AcctAvailBal)
                    .HasColumnName("acct_avail_bal")
                    .HasPrecision(15, 2); // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3

                entity.Property(e => e.AcctIntRate)
                    .HasColumnName("acct_int_rate")
                    .HasPrecision(6, 4); // COBOL: ACCT-INT-RATE PIC 9(2)V9(4) COMP-3

                entity.Property(e => e.AcctLastTxnDt)
                    .HasColumnName("acct_last_txn_dt")
                    .HasColumnType("DATE"); // COBOL: ACCT-LAST-TXN-DT PIC 9(8)

                entity.Property(e => e.AcctCurrency)
                    .HasColumnName("acct_currency")
                    .HasMaxLength(3)
                    .IsFixedLength(); // COBOL: ACCT-CURRENCY PIC X(03)

                entity.Property(e => e.AcctBranchCode)
                    .HasColumnName("acct_branch_code")
                    .HasMaxLength(4)
                    .IsFixedLength(); // COBOL: ACCT-BRANCH-CODE PIC 9(4)

                entity.Property(e => e.AcctFlags)
                    .HasColumnName("acct_flags")
                    .HasMaxLength(4)
                    .IsFixedLength(); // COBOL: ACCT-FLAGS PIC X(04)

                entity.Property(e => e.AcctFiller)
                    .HasColumnName("acct_filler")
                    .HasMaxLength(50)
                    .IsFixedLength(); // COBOL: ACCT-FILLER PIC X(50)
            });

            modelBuilder.Entity<AcctAudit>(entity =>
            {
                entity.ToTable("acct_audit", "public");

                entity.Property(e => e.AcctNum)
                    .HasColumnName("acct_num")
                    .HasMaxLength(10)
                    .IsFixedLength(); // COBOL: ACCT-NUM PIC 9(10)

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasMaxLength(20); // Assuming a max length for the action field

                entity.Property(e => e.Actor)
                    .HasColumnName("actor")
                    .HasMaxLength(20); // Assuming a max length for the actor field

                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("TIMESTAMPTZ");

                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10)
                    .IsFixedLength(); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.CommFunction)
                    .HasColumnName("comm_function")
                    .HasMaxLength(20); // Assuming a max length for the comm_function field
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer", "public");

                entity.Property(e => e.ActiveAcctCount)
                    .HasColumnName("active_acct_count")
                    .HasMaxLength(5)
                    .IsFixedLength(); // Assuming COBOL: ACTIVE-ACCT-COUNT PIC 9(5)

                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasMaxLength(8)
                    .IsFixedLength(); // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
            });
        }
    }

    public class Acctfile
    {
        public string AcctNumber { get; set; } // COBOL: ACCT-NUMBER PIC 9(10)
        public string AcctCustomerId { get; set; } // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
        public DateTime AcctOpenedDate { get; set; } // COBOL: ACCT-OPENED-DATE PIC 9(8)
        public string AcctType { get; set; } // COBOL: ACCT-TYPE PIC X(02)
        public string AcctStatus { get; set; } // COBOL: ACCT-STATUS PIC X(01)
        public decimal AcctBalance { get; set; } // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3
        public decimal AcctAvailBal { get; set; } // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3
        public decimal AcctIntRate { get; set; } // COBOL: ACCT-INT-RATE PIC 9(2)V9(4) COMP-3
        public DateTime AcctLastTxnDt { get; set; } // COBOL: ACCT-LAST-TXN-DT PIC 9(8)
        public string AcctCurrency { get; set; } // COBOL: ACCT-CURRENCY PIC X(03)
        public string AcctBranchCode { get; set; } // COBOL: ACCT-BRANCH-CODE PIC 9(4)
        public string AcctFlags { get; set; } // COBOL: ACCT-FLAGS PIC X(04)
        public string AcctFiller { get; set; } // COBOL: ACCT-FILLER PIC X(50)
    }

    public class AcctAudit
    {
        public string AcctNum { get; set; } // COBOL: ACCT-NUM PIC 9(10)
        public string Action { get; set; } // Assuming COBOL: ACTION PIC X(20)
        public string Actor { get; set; } // Assuming COBOL: ACTOR PIC X(20)
        public DateTime Ts { get; set; }
        public string AcctNumber { get; set; } // COBOL: ACCT-NUMBER PIC 9(10)
        public string CommFunction { get; set; } // Assuming COBOL: COMM-FUNCTION PIC X(20)
    }

    public class Customer
    {
        public string ActiveAcctCount { get; set; } // Assuming COBOL: ACTIVE-ACCT-COUNT PIC 9(5)
        public string AcctCustomerId { get; set; } // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly AcctmgrDbContext _context;

        public CustomerRepository(AcctmgrDbContext context)
        {
            _context = context;
        }

        public async Task<AccountManager.CustomerRecord[]> ReadCustomerRecordsAsync()
        {
            var customers = await _context.Customers.ToListAsync();
            return customers.Select(c => new AccountManager.CustomerRecord
            {
                ActiveAccountCount = int.Parse(c.ActiveAcctCount),
                CustomerId = c.AcctCustomerId
            }).ToArray();
        }

        public async Task WriteCustomerRecordsAsync(AccountManager.CustomerRecord[] customerRecords)
        {
            foreach (var record in customerRecords)
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.AcctCustomerId == record.CustomerId);

                if (customer == null)
                {
                    customer = new Customer
                    {
                        AcctCustomerId = record.CustomerId,
                        ActiveAcctCount = record.ActiveAccountCount.ToString()
                    };
                    _context.Customers.Add(customer);
                }
                else
                {
                    customer.ActiveAcctCount = record.ActiveAccountCount.ToString();
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly AcctmgrDbContext _context;

        public AccountRepository(AcctmgrDbContext context)
        {
            _context = context;
        }

        public async Task<AccountManager.AccountRecord[]> ReadAccountRecordsAsync(string customerId)
        {
            var accounts = await _context.Acctfiles
                .Where(a => a.AcctCustomerId == customerId)
                .ToListAsync();

            return accounts.Select(a => new AccountManager.AccountRecord
            {
                AccountNumber = a.AcctNumber,
                CustomerId = a.AcctCustomerId,
                OpenedDate = a.AcctOpenedDate,
                AccountType = a.AcctType,
                Status = a.AcctStatus,
                Balance = a.AcctBalance,
                AvailableBalance = a.AcctAvailBal,
                InterestRate = a.AcctIntRate,
                LastTransactionDate = a.AcctLastTxnDt,
                Currency = a.AcctCurrency,
                BranchCode = a.AcctBranchCode,
                Flags = a.AcctFlags,
                Filler = a.AcctFiller
            }).ToArray();
        }

        public async Task WriteAccountRecordsAsync(string customerId, AccountManager.AccountRecord[] accountRecords)
        {
            foreach (var record in accountRecords)
            {
                var account = await _context.Acctfiles
                    .FirstOrDefaultAsync(a => a.AcctNumber == record.AccountNumber);

                if (account == null)
                {
                    account = new Acctfile
                    {
                        AcctNumber = record.AccountNumber,
                        AcctCustomerId = customerId,
                        AcctOpenedDate = record.OpenedDate,
                        AcctType = record.AccountType,
                        AcctStatus = record.Status,
                        AcctBalance = record.Balance,
                        AcctAvailBal = record.AvailableBalance,
                        AcctIntRate = record.InterestRate,
                        AcctLastTxnDt = record.LastTransactionDate,
                        AcctCurrency = record.Currency,
                        AcctBranchCode = record.BranchCode,
                        AcctFlags = record.Flags,
                        AcctFiller = record.Filler
                    };
                    _context.Acctfiles.Add(account);
                }
                else
                {
                    account.AcctCustomerId = customerId;
                    account.AcctOpenedDate = record.OpenedDate;
                    account.AcctType = record.AccountType;
                    account.AcctStatus = record.Status;
                    account.AcctBalance = record.Balance;
                    account.AcctAvailBal = record.AvailableBalance;
                    account.AcctIntRate = record.InterestRate;
                    account.AcctLastTxnDt = record.LastTransactionDate;
                    account.AcctCurrency = record.Currency;
                    account.AcctBranchCode = record.BranchCode;
                    account.AcctFlags = record.Flags;
                    account.AcctFiller = record.Filler;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}