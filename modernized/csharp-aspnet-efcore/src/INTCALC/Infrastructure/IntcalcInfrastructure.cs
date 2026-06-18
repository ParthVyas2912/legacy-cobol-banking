using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using INTCALC.Application.Interfaces;
using INTCALC.Domain;

namespace INTCALC.Infrastructure
{
    public class IntcalcDbContext : DbContext
    {
        public DbSet<IntcalcDomainModels.Account> Accounts { get; set; }
        public DbSet<IntcalcDomainModels.Transaction> Transactions { get; set; }
        public DbSet<IntcalcDomainModels.Customer> Customers { get; set; } // Added Customers DbSet

        public IntcalcDbContext(DbContextOptions<IntcalcDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntcalcDomainModels.Account>(ConfigureAccount);
            modelBuilder.Entity<IntcalcDomainModels.Transaction>(ConfigureTransaction);
            modelBuilder.Entity<IntcalcDomainModels.Customer>(ConfigureCustomer); // Added Customer configuration
        }

        private void ConfigureAccount(EntityTypeBuilder<IntcalcDomainModels.Account> builder)
        {
            builder.ToTable("acctfile", "public");

            builder.HasKey(a => a.AccountNumber);
            builder.Property(a => a.AccountNumber)
                .HasColumnName("acct_number")
                .HasMaxLength(10); // COBOL: ACCT-NUMBER PIC 9(10)

            builder.Property(a => a.InterestRate)
                .HasColumnName("acct_int_rate")
                .HasPrecision(6, 4); // COBOL: ACCT-INT-RATE PIC 9(2)V9(4) COMP-3

            builder.Property(a => a.Balance)
                .HasColumnName("acct_balance")
                .HasPrecision(15, 2); // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3

            builder.Property(a => a.Status)
                .HasColumnName("acct_status")
                .HasConversion<string>()
                .HasMaxLength(1); // COBOL: ACCT-STATUS PIC X(01)
        }

        private void ConfigureTransaction(EntityTypeBuilder<IntcalcDomainModels.Transaction> builder)
        {
            builder.ToTable("intjrnl", "public");

            builder.HasKey(t => t.TransactionId);
            builder.Property(t => t.TransactionId)
                .HasColumnName("txn_id")
                .HasMaxLength(12); // COBOL: TXN-ID PIC 9(12)

            builder.Property(t => t.TransactionDate)
                .HasColumnName("txn_post_date")
                .HasColumnType("DATE"); // COBOL: TXN-POST-DATE PIC 9(8)

            builder.Property(t => t.Amount)
                .HasColumnName("txn_amount")
                .HasPrecision(13, 2); // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3

            builder.Property(t => t.CurrencyCode)
                .HasColumnName("txn_currency")
                .HasMaxLength(3); // COBOL: TXN-CURRENCY PIC X(03)

            builder.Property(t => t.Type)
                .HasColumnName("txn_code")
                .HasConversion<string>()
                .HasMaxLength(3); // COBOL: TXN-CODE PIC X(03)
        }

        private void ConfigureCustomer(EntityTypeBuilder<IntcalcDomainModels.Customer> builder)
        {
            builder.ToTable("custfile", "public");

            builder.HasKey(c => c.CustomerId);
            builder.Property(c => c.CustomerId)
                .HasColumnName("cust_id")
                .HasMaxLength(10); // COBOL: CUST-ID PIC X(10)

            builder.Property(c => c.Name)
                .HasColumnName("cust_name")
                .HasMaxLength(50); // COBOL: CUST-NAME PIC X(50)

            builder.Property(c => c.Address)
                .HasColumnName("cust_address")
                .HasMaxLength(100); // COBOL: CUST-ADDRESS PIC X(100)

            builder.Property(c => c.PhoneNumber)
                .HasColumnName("cust_phone")
                .HasMaxLength(15); // COBOL: CUST-PHONE PIC X(15)
        }
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly IntcalcDbContext _context;

        public AccountRepository(IntcalcDbContext context)
        {
            _context = context;
        }

        public async Task<IntcalcDomainModels.Account> GetAccountAsync(string accountNumber)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task UpdateAccountAsync(IntcalcDomainModels.Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly IntcalcDbContext _context;

        public TransactionRepository(IntcalcDbContext context)
        {
            _context = context;
        }

        public async Task RecordTransactionAsync(IntcalcDomainModels.Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly IntcalcDbContext _context;

        public CustomerRepository(IntcalcDbContext context)
        {
            _context = context;
        }

        public async Task<IntcalcDomainModels.Customer> GetCustomerAsync(string customerId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }
    }

    public class InterestCalculator : IInterestCalculator
    {
        public Task<decimal> CalculateInterestAsync(decimal principal, decimal rate)
        {
            var interest = principal * rate / 100;
            return Task.FromResult(interest);
        }
    }
}