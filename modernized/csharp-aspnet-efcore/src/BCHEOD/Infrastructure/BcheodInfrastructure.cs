using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BCHEOD.Domain;
using BCHEOD.Application.Interfaces;

namespace BCHEOD.Infrastructure
{
    public class BcheodDbContext : DbContext
    {
        public DbSet<EodRuns> EodRuns { get; set; }
        public DbSet<AccountDetails> AccountDetails { get; set; }
        public DbSet<CustomerDetails> CustomerDetails { get; set; }
        public DbSet<TransactionDetails> TransactionDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection string here
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EodRuns>(entity =>
            {
                entity.ToTable("eod_runs", "public");

                entity.Property(e => e.StartedAt)
                    .HasColumnName("started_at")
                    .HasColumnType("timestamptz");
                // COBOL: STARTED_AT

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("timestamptz");
                // COBOL: STATUS

                entity.Property(e => e.EndedAt)
                    .HasColumnName("ended_at")
                    .HasColumnType("timestamptz");
                // COBOL: ENDED_AT

                entity.Property(e => e.WsStatusCode)
                    .HasColumnName("ws_status_code")
                    .HasColumnType("timestamptz");
                // COBOL: WS_STATUS_CODE
            });

            modelBuilder.Entity<AccountDetails>(entity =>
            {
                entity.ToTable("account_details", "public");

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(10);
                // COBOL: ACCOUNT_NUMBER PIC X(10)

                entity.Property(e => e.AccountName)
                    .HasColumnName("account_name")
                    .HasMaxLength(30);
                // COBOL: ACCOUNT_NAME PIC X(30)

                entity.Property(e => e.AccountBalance)
                    .HasColumnName("account_balance")
                    .HasPrecision(15, 2);
                // COBOL: ACCOUNT_BALANCE PIC S9(13)V99 COMP-3

                entity.Property(e => e.AccountStatus)
                    .HasColumnName("account_status")
                    .HasConversion<string>();
                // COBOL: ACCOUNT_STATUS 88-level equivalent
            });

            modelBuilder.Entity<CustomerDetails>(entity =>
            {
                entity.ToTable("customer_details", "public");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasMaxLength(15);
                // COBOL: CUSTOMER_ID PIC X(15)

                entity.Property(e => e.CustomerName)
                    .HasColumnName("customer_name")
                    .HasMaxLength(50);
                // COBOL: CUSTOMER_NAME PIC X(50)

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(100);
                // COBOL: ADDRESS PIC X(100)
            });

            modelBuilder.Entity<TransactionDetails>(entity =>
            {
                entity.ToTable("transaction_details", "public");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(12);
                // COBOL: TRANSACTION_ID PIC X(12)

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasColumnType("date");
                // COBOL: TRANSACTION_DATE PIC 9(8) (YYYYMMDD)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(13, 2);
                // COBOL: TRANSACTION_AMOUNT PIC S9(11)V99 COMP-3

                entity.Property(e => e.CurrencyCode)
                    .HasColumnName("currency_code")
                    .HasMaxLength(3);
                // COBOL: CURRENCY_CODE PIC X(3)

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .HasConversion<string>();
                // COBOL: TRANSACTION_TYPE 88-level equivalent
            });
        }
    }

    public class EodRuns
    {
        public DateTime StartedAt { get; set; }
        public DateTime Status { get; set; }
        public DateTime EndedAt { get; set; }
        public DateTime WsStatusCode { get; set; }
    }

    public class AccountDetails
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public decimal AccountBalance { get; set; }
        public BcheodDomainModels.AccountStatusEnum AccountStatus { get; set; }
    }

    public class CustomerDetails
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
    }

    public class TransactionDetails
    {
        public string TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionAmount { get; set; }
        public string CurrencyCode { get; set; }
        public BcheodDomainModels.TransactionTypeEnum TransactionType { get; set; }
    }

    public class BcheodRepository : IBcheodRepository
    {
        private readonly BcheodDbContext _context;

        public BcheodRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public BcheodDomainModels.AccountDetails GetAccountDetails(string accountNumber)
        {
            var account = _context.AccountDetails
                .FirstOrDefault(a => a.AccountNumber == accountNumber);

            if (account == null)
                throw new KeyNotFoundException($"Account with number {accountNumber} not found.");

            return new BcheodDomainModels.AccountDetails
            {
                AccountNumber = account.AccountNumber,
                AccountName = account.AccountName,
                AccountBalance = account.AccountBalance,
                AccountStatus = account.AccountStatus
            };
        }

        public void UpdateAccountDetails(BcheodDomainModels.AccountDetails accountDetails)
        {
            var account = _context.AccountDetails
                .FirstOrDefault(a => a.AccountNumber == accountDetails.AccountNumber);

            if (account == null)
                throw new KeyNotFoundException($"Account with number {accountDetails.AccountNumber} not found.");

            account.AccountBalance = accountDetails.AccountBalance;
            account.AccountStatus = accountDetails.AccountStatus;
            _context.SaveChanges();
        }

        public BcheodDomainModels.CustomerDetails GetCustomerDetails(string customerId)
        {
            var customer = _context.CustomerDetails
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

            return new BcheodDomainModels.CustomerDetails
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                Address = customer.Address
            };
        }

        public void WriteTransactionDetails(BcheodDomainModels.TransactionDetails transactionDetails)
        {
            var transaction = new TransactionDetails
            {
                TransactionId = transactionDetails.TransactionId,
                TransactionDate = transactionDetails.TransactionDate,
                TransactionAmount = transactionDetails.TransactionAmount,
                CurrencyCode = transactionDetails.CurrencyCode,
                TransactionType = transactionDetails.TransactionType
            };

            _context.TransactionDetails.Add(transaction);
            _context.SaveChanges();
        }

        public List<BcheodDomainModels.TransactionDetails> GetTransactionsByAccount(string accountNumber)
        {
            var transactions = _context.TransactionDetails
                .Where(t => t.AccountNumber == accountNumber)
                .ToList();

            return transactions.Select(t => new BcheodDomainModels.TransactionDetails
            {
                TransactionId = t.TransactionId,
                TransactionDate = t.TransactionDate,
                TransactionAmount = t.TransactionAmount,
                CurrencyCode = t.CurrencyCode,
                TransactionType = t.TransactionType
            }).ToList();
        }
    }
}