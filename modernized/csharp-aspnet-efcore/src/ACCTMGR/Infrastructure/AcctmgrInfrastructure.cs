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
                    .HasColumnType("date"); // COBOL: ACCT-OPENED-DATE PIC 9(8)

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
                    .HasColumnType("date"); // COBOL: ACCT-LAST-TXN-DT PIC 9(8)

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

            modelBuilder.Entity<AcctAudit>(entity =>
            {
                entity.ToTable("acct_audit", "public");

                entity.Property(e => e.AcctNum)
                    .HasColumnName("acct_num")
                    .HasMaxLength(10); // COBOL: ACCT-NUM PIC 9(10)

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasMaxLength(20); // COBOL: ACTION PIC X(20)

                entity.Property(e => e.Actor)
                    .HasColumnName("actor")
                    .HasMaxLength(20); // COBOL: ACTOR PIC X(20)

                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("timestamptz"); // COBOL: TS PIC 9(14)

                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.CommFunction)
                    .HasColumnName("comm_function")
                    .HasMaxLength(30); // COBOL: COMM-FUNCTION PIC X(30)
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer", "public");

                entity.Property(e => e.ActiveAcctCount)
                    .HasColumnName("active_acct_count")
                    .HasPrecision(5, 0); // COBOL: ACTIVE-ACCT-COUNT PIC 9(5)

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
            var acctfile = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == accountNumber);
            if (acctfile == null) return null;

            return new AcctmgrDomainModels.Account
            {
                AccountNumber = acctfile.AcctNumber,
                AccountBalance = acctfile.AcctBalance,
                AccountType = acctfile.AcctType,
                AccountStatus = (AcctmgrDomainModels.AccountStatusEnum)Enum.Parse(typeof(AcctmgrDomainModels.AccountStatusEnum), acctfile.AcctStatus),
                TransactionHistory = new List<AcctmgrDomainModels.Transaction>() // Placeholder for transaction mapping
            };
        }

        public void UpdateAccount(AcctmgrDomainModels.Account account)
        {
            var acctfile = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == account.AccountNumber);
            if (acctfile == null) return;

            acctfile.AcctBalance = account.AccountBalance;
            acctfile.AcctType = account.AccountType;
            acctfile.AcctStatus = account.AccountStatus.ToString();

            _context.SaveChanges();
        }

        public List<AcctmgrDomainModels.Account> GetAllAccounts()
        {
            return _context.Acctfiles.Select(acctfile => new AcctmgrDomainModels.Account
            {
                AccountNumber = acctfile.AcctNumber,
                AccountBalance = acctfile.AcctBalance,
                AccountType = acctfile.AcctType,
                AccountStatus = (AcctmgrDomainModels.AccountStatusEnum)Enum.Parse(typeof(AcctmgrDomainModels.AccountStatusEnum), acctfile.AcctStatus),
                TransactionHistory = new List<AcctmgrDomainModels.Transaction>() // Placeholder for transaction mapping
            }).ToList();
        }

        public void DeleteAccount(string accountNumber)
        {
            var acctfile = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == accountNumber);
            if (acctfile == null) return;

            _context.Acctfiles.Remove(acctfile);
            _context.SaveChanges();
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AcctmgrDbContext _context;

        public TransactionRepository(AcctmgrDbContext context)
        {
            _context = context;
        }

        public void AddTransaction(AcctmgrDomainModels.Transaction transaction)
        {
            // Placeholder for adding transaction logic
        }

        public List<AcctmgrDomainModels.Transaction> GetTransactionsByAccountNumber(string accountNumber)
        {
            // Placeholder for retrieving transactions by account number
            return new List<AcctmgrDomainModels.Transaction>();
        }

        public List<AcctmgrDomainModels.Transaction> GetAllTransactions()
        {
            // Placeholder for retrieving all transactions
            return new List<AcctmgrDomainModels.Transaction>();
        }
    }
}