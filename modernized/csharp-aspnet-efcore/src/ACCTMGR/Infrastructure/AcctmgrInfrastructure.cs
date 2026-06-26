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
                    .HasMaxLength(10)
                    .IsRequired(); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasMaxLength(8)
                    .IsRequired(); // COBOL: ACCT-CUSTOMER-ID PIC 9(8)

                entity.Property(e => e.AcctOpenedDate)
                    .HasColumnName("acct_opened_date")
                    .HasColumnType("DATE")
                    .IsRequired(); // COBOL: ACCT-OPENED-DATE PIC 9(8)

                entity.Property(e => e.AcctType)
                    .HasColumnName("acct_type")
                    .HasMaxLength(2)
                    .IsRequired(); // COBOL: ACCT-TYPE PIC X(02)

                entity.Property(e => e.AcctStatus)
                    .HasColumnName("acct_status")
                    .HasMaxLength(1)
                    .IsRequired(); // COBOL: ACCT-STATUS PIC X(01)

                entity.Property(e => e.AcctBalance)
                    .HasColumnName("acct_balance")
                    .HasPrecision(15, 2)
                    .IsRequired(); // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3

                entity.Property(e => e.AcctAvailBal)
                    .HasColumnName("acct_avail_bal")
                    .HasPrecision(15, 2)
                    .IsRequired(); // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3

                entity.Property(e => e.AcctIntRate)
                    .HasColumnName("acct_int_rate")
                    .HasPrecision(6, 4)
                    .IsRequired(); // COBOL: ACCT-INT-RATE PIC 9(2)V9(4)

                entity.Property(e => e.AcctLastTxnDt)
                    .HasColumnName("acct_last_txn_dt")
                    .HasColumnType("DATE")
                    .IsRequired(); // COBOL: ACCT-LAST-TXN-DT PIC 9(8)

                entity.Property(e => e.AcctCurrency)
                    .HasColumnName("acct_currency")
                    .HasMaxLength(3)
                    .IsRequired(); // COBOL: ACCT-CURRENCY PIC X(03)

                entity.Property(e => e.AcctBranchCode)
                    .HasColumnName("acct_branch_code")
                    .HasMaxLength(4)
                    .IsRequired(); // COBOL: ACCT-BRANCH-CODE PIC 9(4)

                entity.Property(e => e.AcctFlags)
                    .HasColumnName("acct_flags")
                    .HasMaxLength(4)
                    .IsRequired(); // COBOL: ACCT-FLAGS PIC X(04)

                entity.Property(e => e.AcctFiller)
                    .HasColumnName("acct_filler")
                    .HasMaxLength(50)
                    .IsRequired(); // COBOL: ACCT-FILLER PIC X(50)
            });

            modelBuilder.Entity<AcctAudit>(entity =>
            {
                entity.ToTable("acct_audit", "public");

                entity.Property(e => e.AcctNum)
                    .HasColumnName("acct_num")
                    .HasMaxLength(10)
                    .IsRequired(); // COBOL: ACCT_NUM PIC 9(10)

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasMaxLength(20)
                    .IsRequired(); // COBOL: ACTION PIC X(20)

                entity.Property(e => e.Actor)
                    .HasColumnName("actor")
                    .HasMaxLength(30)
                    .IsRequired(); // COBOL: ACTOR PIC X(30)

                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("TIMESTAMPTZ")
                    .IsRequired(); // COBOL: TS PIC 9(14)

                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10)
                    .IsRequired(); // COBOL: ACCT_NUMBER PIC 9(10)

                entity.Property(e => e.CommFunction)
                    .HasColumnName("comm_function")
                    .HasMaxLength(50)
                    .IsRequired(); // COBOL: COMM_FUNCTION PIC X(50)
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer", "public");

                entity.Property(e => e.ActiveAcctCount)
                    .HasColumnName("active_acct_count")
                    .HasPrecision(5, 0)
                    .IsRequired(); // COBOL: ACTIVE_ACCT_COUNT PIC 9(5)

                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasMaxLength(8)
                    .IsRequired(); // COBOL: ACCT_CUSTOMER_ID PIC 9(8)
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

        public AcctmgrDomainModels.Account ReadAccount(string accountNumber)
        {
            var acctfile = _context.Acctfiles.Find(accountNumber);
            if (acctfile == null) return null;

            return new AcctmgrDomainModels.Account
            {
                AccountNumber = acctfile.AcctNumber,
                Balance = acctfile.AcctBalance,
                Status = (AcctmgrDomainModels.AccountStatus)Enum.Parse(typeof(AcctmgrDomainModels.AccountStatus), acctfile.AcctStatus),
                Type = (AcctmgrDomainModels.AccountType)Enum.Parse(typeof(AcctmgrDomainModels.AccountType), acctfile.AcctType)
            };
        }

        public void WriteAccount(AcctmgrDomainModels.Account account)
        {
            var acctfile = new Acctfile
            {
                AcctNumber = account.AccountNumber,
                AcctBalance = account.Balance,
                AcctStatus = account.Status.ToString(),
                AcctType = account.Type.ToString()
            };

            _context.Acctfiles.Update(acctfile);
            _context.SaveChanges();
        }

        public void DeleteAccount(string accountNumber)
        {
            var acctfile = _context.Acctfiles.Find(accountNumber);
            if (acctfile != null)
            {
                _context.Acctfiles.Remove(acctfile);
                _context.SaveChanges();
            }
        }

        public bool AccountExists(string accountNumber)
        {
            return _context.Acctfiles.Any(a => a.AcctNumber == accountNumber);
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AcctmgrDbContext _context;

        public TransactionRepository(AcctmgrDbContext context)
        {
            _context = context;
        }

        public List<AcctmgrDomainModels.Transaction> ReadTransactions(string accountNumber)
        {
            // Placeholder for actual transaction retrieval logic
            return new List<AcctmgrDomainModels.Transaction>();
        }

        public void WriteTransactions(string accountNumber, List<AcctmgrDomainModels.Transaction> transactions)
        {
            // Placeholder for actual transaction write logic
        }

        public void DeleteTransactions(string accountNumber)
        {
            // Placeholder for actual transaction delete logic
        }
    }
}