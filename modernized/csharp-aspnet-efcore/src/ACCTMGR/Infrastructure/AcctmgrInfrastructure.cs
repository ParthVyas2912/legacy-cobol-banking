using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ACCTMGR.Domain;
using ACCTMGR.Application.Interfaces;

namespace ACCTMGR.Infrastructure
{
    public class AcctmgrDbContext : DbContext
    {
        public DbSet<Acctfile> Acctfiles { get; set; }
        public DbSet<AcctAudit> AcctAudits { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acctfile>(entity =>
            {
                entity.ToTable("acctfile", "public");

                entity.HasKey(e => e.AcctNumber);

                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasColumnType("varchar(10)") // Corrected to varchar(10) to match PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasColumnType("varchar(8)") // Corrected to varchar(8) to match PIC 9(8)
                    .IsRequired();

                entity.Property(e => e.AcctOpenedDate)
                    .HasColumnName("acct_opened_date")
                    .HasColumnType("date") // Corrected to date to match PIC 9(8)
                    .IsRequired();

                entity.Property(e => e.AcctType)
                    .HasColumnName("acct_type")
                    .HasColumnType("char(2)") // Corrected to char(2) to match PIC X(02)
                    .IsRequired();

                entity.Property(e => e.AcctStatus)
                    .HasColumnName("acct_status")
                    .HasColumnType("char(1)") // Corrected to char(1) to match PIC X(01)
                    .IsRequired();

                entity.Property(e => e.AcctBalance)
                    .HasColumnName("acct_balance")
                    .HasColumnType("decimal(15,2)") // Corrected to decimal(15,2) to match PIC S9(13)V99 COMP-3
                    .IsRequired();

                entity.Property(e => e.AcctAvailBal)
                    .HasColumnName("acct_avail_bal")
                    .HasColumnType("decimal(15,2)") // Corrected to decimal(15,2) to match PIC S9(13)V99 COMP-3
                    .IsRequired();

                entity.Property(e => e.AcctIntRate)
                    .HasColumnName("acct_int_rate")
                    .HasColumnType("decimal(6,4)") // Corrected to decimal(6,4) to match PIC 9(2)V9(4)
                    .IsRequired();

                entity.Property(e => e.AcctLastTxnDt)
                    .HasColumnName("acct_last_txn_dt")
                    .HasColumnType("date") // Corrected to date to match PIC 9(8)
                    .IsRequired();

                entity.Property(e => e.AcctCurrency)
                    .HasColumnName("acct_currency")
                    .HasColumnType("char(3)") // Corrected to char(3) to match PIC X(03)
                    .IsRequired();

                entity.Property(e => e.AcctBranchCode)
                    .HasColumnName("acct_branch_code")
                    .HasColumnType("varchar(4)") // Corrected to varchar(4) to match PIC 9(4)
                    .IsRequired();

                entity.Property(e => e.AcctFlags)
                    .HasColumnName("acct_flags")
                    .HasColumnType("char(4)") // Corrected to char(4) to match PIC X(04)
                    .IsRequired();

                entity.Property(e => e.AcctFiller)
                    .HasColumnName("acct_filler")
                    .HasColumnType("char(50)") // Corrected to char(50) to match PIC X(50)
                    .IsRequired();
            });

            modelBuilder.Entity<AcctAudit>(entity =>
            {
                entity.ToTable("acct_audit", "public");

                entity.Property(e => e.AcctNum)
                    .HasColumnName("acct_num")
                    .HasColumnType("varchar(10)") // Corrected to varchar(10) to match AcctNumber PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasColumnType("varchar(50)") // Assuming a reasonable length for action description
                    .IsRequired();

                entity.Property(e => e.Actor)
                    .HasColumnName("actor")
                    .HasColumnType("varchar(50)") // Assuming a reasonable length for actor name
                    .IsRequired();

                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("timestamp with time zone") // Corrected to match TIMESTAMPTZ
                    .IsRequired();

                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasColumnType("varchar(10)") // Corrected to varchar(10) to match AcctNumber PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.CommFunction)
                    .HasColumnName("comm_function")
                    .HasColumnType("varchar(50)") // Assuming a reasonable length for communication function
                    .IsRequired();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer", "public");

                entity.Property(e => e.ActiveAcctCount)
                    .HasColumnName("active_acct_count")
                    .HasColumnType("int") // Corrected to int to match numeric data type
                    .IsRequired();

                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasColumnType("varchar(8)") // Corrected to varchar(8) to match PIC 9(8)
                    .IsRequired();
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
        public decimal AcctIntRate { get; set; } // COBOL: ACCT-INT-RATE PIC 9(2)V9(4)
        public DateTime AcctLastTxnDt { get; set; } // COBOL: ACCT-LAST-TXN-DT PIC 9(8)
        public string AcctCurrency { get; set; } // COBOL: ACCT-CURRENCY PIC X(03)
        public string AcctBranchCode { get; set; } // COBOL: ACCT-BRANCH-CODE PIC 9(4)
        public string AcctFlags { get; set; } // COBOL: ACCT-FLAGS PIC X(04)
        public string AcctFiller { get; set; } // COBOL: ACCT-FILLER PIC X(50)
    }

    public class AcctAudit
    {
        public string AcctNum { get; set; } // COBOL: ACCT-NUMBER PIC 9(10)
        public string Action { get; set; } // Assuming a reasonable length for action description
        public string Actor { get; set; } // Assuming a reasonable length for actor name
        public DateTime Ts { get; set; } // COBOL: TIMESTAMPTZ
        public string AcctNumber { get; set; } // COBOL: ACCT-NUMBER PIC 9(10)
        public string CommFunction { get; set; } // Assuming a reasonable length for communication function
    }

    public class Customer
    {
        public int ActiveAcctCount { get; set; } // COBOL: Numeric field
        public string AcctCustomerId { get; set; } // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly AcctmgrDbContext _context;

        public AccountRepository(AcctmgrDbContext context)
        {
            _context = context;
        }

        public AcctmgrDomainModels.Account ReadAccount(string accountNumber)
        {
            var acctfile = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == accountNumber);
            if (acctfile == null)
            {
                throw new KeyNotFoundException($"Account with number {accountNumber} not found.");
            }

            return new AcctmgrDomainModels.Account
            {
                AccountNumber = acctfile.AcctNumber,
                CustomerId = acctfile.AcctCustomerId,
                OpenedDate = acctfile.AcctOpenedDate,
                AccountType = acctfile.AcctType,
                Status = acctfile.AcctStatus,
                Balance = acctfile.AcctBalance,
                AvailableBalance = acctfile.AcctAvailBal,
                InterestRate = acctfile.AcctIntRate,
                LastTransactionDate = acctfile.AcctLastTxnDt,
                Currency = acctfile.AcctCurrency,
                BranchCode = acctfile.AcctBranchCode,
                Flags = acctfile.AcctFlags,
                Filler = acctfile.AcctFiller
            };
        }

        public void WriteAccount(AcctmgrDomainModels.Account account)
        {
            var acctfile = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == account.AccountNumber);
            if (acctfile == null)
            {
                acctfile = new Acctfile();
                _context.Acctfiles.Add(acctfile);
            }

            acctfile.AcctNumber = account.AccountNumber;
            acctfile.AcctCustomerId = account.CustomerId;
            acctfile.AcctOpenedDate = account.OpenedDate;
            acctfile.AcctType = account.AccountType;
            acctfile.AcctStatus = account.Status;
            acctfile.AcctBalance = account.Balance;
            acctfile.AcctAvailBal = account.AvailableBalance;
            acctfile.AcctIntRate = account.InterestRate;
            acctfile.AcctLastTxnDt = account.LastTransactionDate;
            acctfile.AcctCurrency = account.Currency;
            acctfile.AcctBranchCode = account.BranchCode;
            acctfile.AcctFlags = account.Flags;
            acctfile.AcctFiller = account.Filler;

            _context.SaveChanges();
        }

        public void DeleteAccount(string accountNumber)
        {
            var acctfile = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == accountNumber);
            if (acctfile == null)
            {
                throw new KeyNotFoundException($"Account with number {accountNumber} not found.");
            }

            _context.Acctfiles.Remove(acctfile);
            _context.SaveChanges();
        }

        public IEnumerable<AcctmgrDomainModels.Transaction> ReadTransactions(string accountNumber)
        {
            var transactions = _context.AcctAudits
                .Where(a => a.AcctNumber == accountNumber)
                .Select(a => new AcctmgrDomainModels.Transaction
                {
                    AccountNumber = a.AcctNum,
                    Action = a.Action,
                    Actor = a.Actor,
                    Timestamp = a.Ts,
                    CommunicationFunction = a.CommFunction
                })
                .ToList();

            return transactions;
        }

        public void WriteTransaction(string accountNumber, AcctmgrDomainModels.Transaction transaction)
        {
            var acctAudit = new AcctAudit
            {
                AcctNum = transaction.AccountNumber,
                Action = transaction.Action,
                Actor = transaction.Actor,
                Ts = transaction.Timestamp,
                AcctNumber = accountNumber,
                CommFunction = transaction.CommunicationFunction
            };

            _context.AcctAudits.Add(acctAudit);
            _context.SaveChanges();
        }

        public void LockAccount(string accountNumber)
        {
            var acctfile = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == accountNumber);
            if (acctfile == null)
            {
                throw new KeyNotFoundException($"Account with number {accountNumber} not found.");
            }

            acctfile.AcctStatus = "L"; // Assuming "L" indicates locked
            _context.SaveChanges();
        }

        public void UnlockAccount(string accountNumber)
        {
            var acctfile = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == accountNumber);
            if (acctfile == null)
            {
                throw new KeyNotFoundException($"Account with number {accountNumber} not found.");
            }

            acctfile.AcctStatus = "A"; // Assuming "A" indicates active
            _context.SaveChanges();
        }

        public IEnumerable<AcctmgrDomainModels.Account> ReadAllAccounts()
        {
            return _context.Acctfiles.Select(acctfile => new AcctmgrDomainModels.Account
            {
                AccountNumber = acctfile.AcctNumber,
                CustomerId = acctfile.AcctCustomerId,
                OpenedDate = acctfile.AcctOpenedDate,
                AccountType = acctfile.AcctType,
                Status = acctfile.AcctStatus,
                Balance = acctfile.AcctBalance,
                AvailableBalance = acctfile.AcctAvailBal,
                InterestRate = acctfile.AcctIntRate,
                LastTransactionDate = acctfile.AcctLastTxnDt,
                Currency = acctfile.AcctCurrency,
                BranchCode = acctfile.AcctBranchCode,
                Flags = acctfile.AcctFlags,
                Filler = acctfile.AcctFiller
            }).ToList();
        }
    }
}