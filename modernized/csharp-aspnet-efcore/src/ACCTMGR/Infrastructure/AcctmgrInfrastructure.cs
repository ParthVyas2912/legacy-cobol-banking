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
        public DbSet<Transaction> Transactions { get; set; }

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
                    .HasPrecision(15, 2); // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3

                entity.Property(e => e.AcctAvailBal)
                    .HasColumnName("acct_avail_bal")
                    .HasPrecision(15, 2); // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3

                entity.Property(e => e.AcctIntRate)
                    .HasColumnName("acct_int_rate")
                    .HasPrecision(6, 4); // COBOL: ACCT-INT-RATE PIC 9(2)V9(4) COMP-3

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

            modelBuilder.Entity<AcctAudit>(entity =>
            {
                entity.ToTable("acct_audit", "public");

                entity.Property(e => e.AcctNum)
                    .HasColumnName("acct_num")
                    .HasMaxLength(10)
                    .IsFixedLength(); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasMaxLength(20); // Assuming action is a string of length 20

                entity.Property(e => e.Actor)
                    .HasColumnName("actor")
                    .HasMaxLength(30); // Assuming actor is a string of length 30

                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("timestamptz");

                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10)
                    .IsFixedLength(); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.CommFunction)
                    .HasColumnName("comm_function")
                    .HasMaxLength(50); // Assuming comm_function is a string of length 50
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer", "public");

                entity.Property(e => e.ActiveAcctCount)
                    .HasColumnName("active_acct_count")
                    .HasMaxLength(5)
                    .IsFixedLength(); // Assuming ACTIVE-ACCT-COUNT PIC 9(5)

                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasMaxLength(8)
                    .IsFixedLength(); // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transaction", "public");

                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(20); // Assuming Transaction ID is a string of length 20

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(10)
                    .IsFixedLength(); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(15, 2); // COBOL: TRANSACTION-AMOUNT PIC S9(13)V99 COMP-3

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasColumnType("date"); // COBOL: TRANSACTION-DATE PIC 9(8)

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(100); // Assuming description is a string of length 100
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
        public string AcctNum { get; set; } // COBOL: ACCT-NUMBER PIC 9(10)
        public string Action { get; set; } // Assuming ACTION PIC X(20)
        public string Actor { get; set; } // Assuming ACTOR PIC X(30)
        public DateTime Ts { get; set; }
        public string AcctNumber { get; set; } // COBOL: ACCT-NUMBER PIC 9(10)
        public string CommFunction { get; set; } // Assuming COMM-FUNCTION PIC X(50)
    }

    public class Customer
    {
        public string ActiveAcctCount { get; set; } // Assuming ACTIVE-ACCT-COUNT PIC 9(5)
        public string AcctCustomerId { get; set; } // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
    }

    public class Transaction
    {
        public string TransactionId { get; set; } // Assuming TRANSACTION-ID PIC X(20)
        public string AccountNumber { get; set; } // COBOL: ACCT-NUMBER PIC 9(10)
        public decimal Amount { get; set; } // COBOL: TRANSACTION-AMOUNT PIC S9(13)V99 COMP-3
        public DateTime TransactionDate { get; set; } // COBOL: TRANSACTION-DATE PIC 9(8)
        public string Description { get; set; } // Assuming DESCRIPTION PIC X(100)
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
            var entity = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == accountNumber);
            if (entity == null) return null;

            return new AcctmgrDomainModels.Account
            {
                AccountNumber = entity.AcctNumber,
                Balance = entity.AcctBalance,
                Status = Enum.Parse<AcctmgrDomainModels.AccountStatus>(entity.AcctStatus)
            };
        }

        public void UpdateAccount(AcctmgrDomainModels.Account account)
        {
            var entity = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == account.AccountNumber);
            if (entity == null) throw new Exception("Account not found");

            entity.AcctBalance = account.Balance;
            entity.AcctStatus = account.Status.ToString();

            _context.SaveChanges();
        }

        public void AddAccount(AcctmgrDomainModels.Account account)
        {
            var entity = new Acctfile
            {
                AcctNumber = account.AccountNumber,
                AcctBalance = account.Balance,
                AcctStatus = account.Status.ToString()
            };

            _context.Acctfiles.Add(entity);
            _context.SaveChanges();
        }

        public void DeleteAccount(string accountNumber)
        {
            var entity = _context.Acctfiles.SingleOrDefault(a => a.AcctNumber == accountNumber);
            if (entity == null) throw new Exception("Account not found");

            _context.Acctfiles.Remove(entity);
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

        public List<AcctmgrDomainModels.Transaction> GetTransactionsByAccountNumber(string accountNumber)
        {
            var transactions = _context.Transactions
                .Where(t => t.AccountNumber == accountNumber)
                .Select(t => new AcctmgrDomainModels.Transaction
                {
                    TransactionId = t.TransactionId,
                    AccountNumber = t.AccountNumber,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    Description = t.Description
                })
                .ToList();

            return transactions;
        }

        public void AddTransaction(AcctmgrDomainModels.Transaction transaction)
        {
            var entity = new Transaction
            {
                TransactionId = transaction.TransactionId,
                AccountNumber = transaction.AccountNumber,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                Description = transaction.Description
            };

            _context.Transactions.Add(entity);
            _context.SaveChanges();
        }
    }
}