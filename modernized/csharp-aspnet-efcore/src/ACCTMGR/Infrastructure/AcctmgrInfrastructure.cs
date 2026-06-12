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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapping for Acctfile
            modelBuilder.Entity<Acctfile>(entity =>
            {
                entity.ToTable("acctfile", "public");
                entity.HasKey(e => e.AcctNumber);

                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasMaxLength(8); // COBOL: ACCT-CUSTOMER-ID PIC 9(8)

                entity.Property(e => e.AcctOpenedDate)
                    .HasColumnName("acct_opened_date")
                    .HasColumnType("DATE"); // COBOL: ACCT-OPENED-DATE PIC 9(8)

                entity.Property(e => e.AcctType)
                    .HasColumnName("acct_type")
                    .HasMaxLength(2); // COBOL: ACCT-TYPE PIC X(02)

                entity.Property(e => e.AcctStatus)
                    .HasColumnName("acct_status")
                    .HasMaxLength(1); // COBOL: ACCT-STATUS PIC X(01)

                entity.Property(e => e.AcctBalance)
                    .HasColumnName("acct_balance")
                    .HasPrecision(15, 2); // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3

                entity.Property(e => e.AcctAvailBal)
                    .HasColumnName("acct_avail_bal")
                    .HasPrecision(15, 2); // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3

                entity.Property(e => e.AcctIntRate)
                    .HasColumnName("acct_int_rate")
                    .HasPrecision(6, 4); // COBOL: ACCT-INT-RATE PIC 9(2)V9(4)

                entity.Property(e => e.AcctLastTxnDt)
                    .HasColumnName("acct_last_txn_dt")
                    .HasColumnType("DATE"); // COBOL: ACCT-LAST-TXN-DT PIC 9(8)

                entity.Property(e => e.AcctCurrency)
                    .HasColumnName("acct_currency")
                    .HasMaxLength(3); // COBOL: ACCT-CURRENCY PIC X(03)

                entity.Property(e => e.AcctBranchCode)
                    .HasColumnName("acct_branch_code")
                    .HasMaxLength(4); // COBOL: ACCT-BRANCH-CODE PIC 9(4)

                entity.Property(e => e.AcctFlags)
                    .HasColumnName("acct_flags")
                    .HasMaxLength(4); // COBOL: ACCT-FLAGS PIC X(04)

                entity.Property(e => e.AcctFiller)
                    .HasColumnName("acct_filler")
                    .HasMaxLength(50); // COBOL: ACCT-FILLER PIC X(50)
            });

            // Mapping for AcctAudit
            modelBuilder.Entity<AcctAudit>(entity =>
            {
                entity.ToTable("acct_audit", "public");

                entity.Property(e => e.AcctNum)
                    .HasColumnName("acct_num")
                    .HasMaxLength(10); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasMaxLength(20); // Assuming a maximum length for action description

                entity.Property(e => e.Actor)
                    .HasColumnName("actor")
                    .HasMaxLength(50); // Assuming a maximum length for actor name

                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("TIMESTAMPTZ");

                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.CommFunction)
                    .HasColumnName("comm_function")
                    .HasMaxLength(30); // Assuming a maximum length for communication function
            });

            // Mapping for Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer", "public");

                entity.Property(e => e.ActiveAcctCount)
                    .HasColumnName("active_acct_count")
                    .HasPrecision(5, 0); // Assuming COBOL: ACTIVE-ACCT-COUNT PIC 9(5)

                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasMaxLength(8); // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
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

        public AcctmgrDomainModels.Account GetAccountByNumber(string accountNumber)
        {
            var acctfile = _context.Acctfiles.FirstOrDefault(a => a.AcctNumber == accountNumber);
            if (acctfile == null) return null;

            return new AcctmgrDomainModels.Account
            {
                AccountNumber = acctfile.AcctNumber,
                Balance = acctfile.AcctBalance,
                AccountType = acctfile.AcctType,
                Status = (AcctmgrDomainModels.Account.StatusEnum)acctfile.AcctStatus[0]
            };
        }

        public void UpdateAccount(AcctmgrDomainModels.Account account)
        {
            var acctfile = _context.Acctfiles.FirstOrDefault(a => a.AcctNumber == account.AccountNumber);
            if (acctfile == null) throw new InvalidOperationException("Account not found.");

            acctfile.AcctBalance = account.Balance;
            acctfile.AcctType = account.AccountType;
            acctfile.AcctStatus = account.Status.ToString();

            _context.SaveChanges();
        }

        public void AddAccount(AcctmgrDomainModels.Account account)
        {
            var acctfile = new Acctfile
            {
                AcctNumber = account.AccountNumber,
                AcctBalance = account.Balance,
                AcctType = account.AccountType,
                AcctStatus = account.Status.ToString()
            };

            _context.Acctfiles.Add(acctfile);
            _context.SaveChanges();
        }

        public void DeleteAccount(string accountNumber)
        {
            var acctfile = _context.Acctfiles.FirstOrDefault(a => a.AcctNumber == accountNumber);
            if (acctfile == null) throw new InvalidOperationException("Account not found.");

            _context.Acctfiles.Remove(acctfile);
            _context.SaveChanges();
        }

        public List<AcctmgrDomainModels.Account> GetAllAccounts()
        {
            return _context.Acctfiles.Select(a => new AcctmgrDomainModels.Account
            {
                AccountNumber = a.AcctNumber,
                Balance = a.AcctBalance,
                AccountType = a.AcctType,
                Status = (AcctmgrDomainModels.Account.StatusEnum)a.AcctStatus[0]
            }).ToList();
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AcctmgrDbContext _context;

        public TransactionRepository(AcctmgrDbContext context)
        {
            _context = context;
        }

        public List<AcctmgrDomainModels.Account.Transaction> GetTransactionsByAccountNumber(string accountNumber)
        {
            // Placeholder for actual transaction retrieval logic
            return new List<AcctmgrDomainModels.Account.Transaction>();
        }

        public void AddTransaction(string accountNumber, AcctmgrDomainModels.Account.Transaction transaction)
        {
            // Placeholder for actual transaction addition logic
        }

        public void DeleteTransaction(string accountNumber, string transactionCode)
        {
            // Placeholder for actual transaction deletion logic
        }
    }
}