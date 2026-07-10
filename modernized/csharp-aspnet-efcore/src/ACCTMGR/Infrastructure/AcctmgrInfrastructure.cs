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
                    .HasMaxLength(10)
                    .IsFixedLength(); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasMaxLength(8)
                    .IsFixedLength(); // COBOL: ACCT-CUSTOMER-ID PIC 9(8)

                entity.Property(e => e.AcctOpenedDate)
                    .HasColumnName("acct_opened_date")
                    .HasColumnType("date"); // COBOL: ACCT-OPENED-DATE PIC 9(8)

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
                    .HasColumnType("decimal(15,2)"); // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3

                entity.Property(e => e.AcctAvailBal)
                    .HasColumnName("acct_avail_bal")
                    .HasColumnType("decimal(15,2)"); // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3

                entity.Property(e => e.AcctIntRate)
                    .HasColumnName("acct_int_rate")
                    .HasColumnType("decimal(6,4)"); // COBOL: ACCT-INT-RATE PIC 9(2)V9(4)

                entity.Property(e => e.AcctLastTxnDt)
                    .HasColumnName("acct_last_txn_dt")
                    .HasColumnType("date"); // COBOL: ACCT-LAST-TXN-DT PIC 9(8)

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

            // Mapping for AcctAudit
            modelBuilder.Entity<AcctAudit>(entity =>
            {
                entity.ToTable("acct_audit", "public");

                entity.Property(e => e.AcctNum)
                    .HasColumnName("acct_num")
                    .HasMaxLength(10)
                    .IsFixedLength(); // COBOL: ACCT-NUM PIC 9(10)

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasMaxLength(20); // COBOL: ACTION PIC X(20)

                entity.Property(e => e.Actor)
                    .HasColumnName("actor")
                    .HasMaxLength(30); // COBOL: ACTOR PIC X(30)

                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("timestamptz"); // COBOL: TS PIC 9(14)

                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10)
                    .IsFixedLength(); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.CommFunction)
                    .HasColumnName("comm_function")
                    .HasMaxLength(50); // COBOL: COMM-FUNCTION PIC X(50)
            });

            // Mapping for Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer", "public");

                entity.Property(e => e.ActiveAcctCount)
                    .HasColumnName("active_acct_count")
                    .HasColumnType("int"); // COBOL: ACTIVE-ACCT-COUNT PIC 9(5)

                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasMaxLength(8)
                    .IsFixedLength(); // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
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
            var acctfile = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == accountNumber);
            if (acctfile == null) return null;

            return new AcctmgrDomainModels.Account
            {
                AccountNumber = acctfile.AcctNumber,
                Balance = acctfile.AcctBalance,
                Status = Enum.Parse<AcctmgrDomainModels.AccountStatus>(acctfile.AcctStatus)
            };
        }

        public void WriteAccount(AcctmgrDomainModels.Account account)
        {
            var acctfile = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == account.AccountNumber);
            if (acctfile == null)
            {
                acctfile = new Acctfile
                {
                    AcctNumber = account.AccountNumber
                };
                _context.Acctfiles.Add(acctfile);
            }

            acctfile.AcctBalance = account.Balance;
            acctfile.AcctStatus = account.Status.ToString();

            _context.SaveChanges();
        }

        public IEnumerable<AcctmgrDomainModels.Account> ReadAllAccounts()
        {
            return _context.Acctfiles.Select(acctfile => new AcctmgrDomainModels.Account
            {
                AccountNumber = acctfile.AcctNumber,
                Balance = acctfile.AcctBalance,
                Status = Enum.Parse<AcctmgrDomainModels.AccountStatus>(acctfile.AcctStatus)
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

        public IEnumerable<AcctmgrDomainModels.Account.Transaction> ReadTransactions(string accountNumber)
        {
            // Placeholder for transaction logic
            return new List<AcctmgrDomainModels.Account.Transaction>();
        }

        public void WriteTransaction(string accountNumber, AcctmgrDomainModels.Account.Transaction transaction)
        {
            // Placeholder for transaction logic
        }
    }

    public class Acctfile
    {
        public string AcctNumber { get; set; }
        public string AcctCustomerId { get; set; }
        public DateTime? AcctOpenedDate { get; set; }
        public string AcctType { get; set; }
        public string AcctStatus { get; set; }
        public decimal AcctBalance { get; set; }
        public decimal AcctAvailBal { get; set; }
        public decimal AcctIntRate { get; set; }
        public DateTime? AcctLastTxnDt { get; set; }
        public string AcctCurrency { get; set; }
        public string AcctBranchCode { get; set; }
        public string AcctFlags { get; set; }
        public string AcctFiller { get; set; }
    }

    public class AcctAudit
    {
        public string AcctNum { get; set; }
        public string Action { get; set; }
        public string Actor { get; set; }
        public DateTime Ts { get; set; }
        public string AcctNumber { get; set; }
        public string CommFunction { get; set; }
    }

    public class Customer
    {
        public int ActiveAcctCount { get; set; }
        public string AcctCustomerId { get; set; }
    }
}