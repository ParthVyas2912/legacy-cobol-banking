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
                    .HasPrecision(6, 4); // COBOL: ACCT-INT-RATE PIC 9(2)V9(4)

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
                    .HasMaxLength(50); // COBOL: ACTION PIC X(50)

                entity.Property(e => e.Actor)
                    .HasColumnName("actor")
                    .HasMaxLength(50); // COBOL: ACTOR PIC X(50)

                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("TIMESTAMPTZ"); // COBOL: TS PIC 9(14)

                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10)
                    .IsFixedLength(); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.CommFunction)
                    .HasColumnName("comm_function")
                    .HasMaxLength(20); // COBOL: COMM-FUNCTION PIC X(20)
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer", "public");

                entity.Property(e => e.ActiveAcctCount)
                    .HasColumnName("active_acct_count")
                    .HasPrecision(5, 0); // COBOL: ACTIVE-ACCT-COUNT PIC 9(5)

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
        public decimal AcctIntRate { get; set; } // COBOL: ACCT-INT-RATE PIC 9(2)V9(4)
        public DateTime AcctLastTxnDt { get; set; } // COBOL: ACCT-LAST-TXN-DT PIC 9(8)
        public string AcctCurrency { get; set; } // COBOL: ACCT-CURRENCY PIC X(03)
        public string AcctBranchCode { get; set; } // COBOL: ACCT-BRANCH-CODE PIC 9(4)
        public string AcctFlags { get; set; } // COBOL: ACCT-FLAGS PIC X(04)
        public string AcctFiller { get; set; } // COBOL: ACCT-FILLER PIC X(50)
    }

    public class AcctAudit
    {
        public string AcctNum { get; set; } // COBOL: ACCT-NUM PIC 9(10)
        public string Action { get; set; } // COBOL: ACTION PIC X(50)
        public string Actor { get; set; } // COBOL: ACTOR PIC X(50)
        public DateTime Ts { get; set; } // COBOL: TS PIC 9(14)
        public string AcctNumber { get; set; } // COBOL: ACCT-NUMBER PIC 9(10)
        public string CommFunction { get; set; } // COBOL: COMM-FUNCTION PIC X(20)
    }

    public class Customer
    {
        public int ActiveAcctCount { get; set; } // COBOL: ACTIVE-ACCT-COUNT PIC 9(5)
        public string AcctCustomerId { get; set; } // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
    }

    public class AcctmgrRepository : IAcctmgrRepository
    {
        private readonly AcctmgrDbContext _context;

        public AcctmgrRepository(AcctmgrDbContext context)
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
                AccountStatus = Enum.Parse<AcctmgrDomainModels.Account.AccountStatusEnum>(acctfile.AcctStatus)
            };
        }

        public void WriteAccount(AcctmgrDomainModels.Account account)
        {
            var acctfile = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == account.AccountNumber);
            if (acctfile == null)
            {
                acctfile = new Acctfile { AcctNumber = account.AccountNumber };
                _context.Acctfiles.Add(acctfile);
            }

            acctfile.AcctBalance = account.Balance;
            acctfile.AcctStatus = account.AccountStatus.ToString();
            _context.SaveChanges();
        }

        public void DeleteAccount(string accountNumber)
        {
            var acctfile = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == accountNumber);
            if (acctfile != null)
            {
                _context.Acctfiles.Remove(acctfile);
                _context.SaveChanges();
            }
        }

        public List<AcctmgrDomainModels.Account> ReadAllAccounts()
        {
            return _context.Acctfiles.Select(a => new AcctmgrDomainModels.Account
            {
                AccountNumber = a.AcctNumber,
                Balance = a.AcctBalance,
                AccountStatus = Enum.Parse<AcctmgrDomainModels.Account.AccountStatusEnum>(a.AcctStatus)
            }).ToList();
        }
    }
}