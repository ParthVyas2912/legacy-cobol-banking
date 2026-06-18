using System;
using System.IO;
using System.Text;
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
        public DbSet<IntcalcDomainModels.Customer> Customers { get; set; }

        public IntcalcDbContext(DbContextOptions<IntcalcDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerEntityConfiguration());
        }
    }

    public class AccountEntityConfiguration : IEntityTypeConfiguration<IntcalcDomainModels.Account>
    {
        public void Configure(EntityTypeBuilder<IntcalcDomainModels.Account> builder)
        {
            builder.ToTable("acctfile", "public");

            builder.HasKey(a => a.AccountNumber);
            builder.Property(a => a.AccountNumber)
                .HasColumnName("acct_number")
                .HasMaxLength(10) // COBOL: ACCT-NUMBER PIC 9(10)
                .IsRequired();

            builder.Property(a => a.AccountName)
                .HasColumnName("acct_name")
                .HasMaxLength(30); // COBOL: ACCT-NAME PIC X(30)

            builder.Property(a => a.InterestRate)
                .HasColumnName("acct_int_rate")
                .HasPrecision(6, 4); // COBOL: ACCT-INT-RATE PIC 9(2)V9(4) COMP-3

            builder.Property(a => a.Balance)
                .HasColumnName("acct_balance")
                .HasPrecision(15, 2); // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3

            builder.Property(a => a.Status)
                .HasColumnName("acct_status")
                .HasMaxLength(1); // COBOL: ACCT-STATUS PIC X(01)
        }
    }

    public class TransactionEntityConfiguration : IEntityTypeConfiguration<IntcalcDomainModels.Transaction>
    {
        public void Configure(EntityTypeBuilder<IntcalcDomainModels.Transaction> builder)
        {
            builder.ToTable("intjrnl", "public");

            builder.HasKey(t => t.TransactionId);
            builder.Property(t => t.TransactionId)
                .HasColumnName("txn_id")
                .HasMaxLength(12) // COBOL: TXN-ID PIC 9(12)
                .IsRequired();

            builder.Property(t => t.TransactionDate)
                .HasColumnName("txn_post_date")
                .HasColumnType("date"); // COBOL: TXN-POST-DATE PIC 9(8)

            builder.Property(t => t.Amount)
                .HasColumnName("txn_amount")
                .HasPrecision(13, 2); // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3

            builder.Property(t => t.CurrencyCode)
                .HasColumnName("txn_currency")
                .HasMaxLength(3); // COBOL: TXN-CURRENCY PIC X(3)
        }
    }

    public class CustomerEntityConfiguration : IEntityTypeConfiguration<IntcalcDomainModels.Customer>
    {
        public void Configure(EntityTypeBuilder<IntcalcDomainModels.Customer> builder)
        {
            builder.ToTable("custfile", "public");

            builder.HasKey(c => c.CustomerId);
            builder.Property(c => c.CustomerId)
                .HasColumnName("cust_id")
                .HasMaxLength(15) // COBOL: CUST-ID PIC X(15)
                .IsRequired();

            builder.Property(c => c.CustomerName)
                .HasColumnName("cust_name")
                .HasMaxLength(50); // COBOL: CUST-NAME PIC X(50)

            builder.Property(c => c.AddressLine1)
                .HasColumnName("cust_addr1")
                .HasMaxLength(50); // COBOL: CUST-ADDR1 PIC X(50)

            builder.Property(c => c.AddressLine2)
                .HasColumnName("cust_addr2")
                .HasMaxLength(50); // COBOL: CUST-ADDR2 PIC X(50)

            builder.Property(c => c.City)
                .HasColumnName("cust_city")
                .HasMaxLength(30); // COBOL: CUST-CITY PIC X(30)

            builder.Property(c => c.State)
                .HasColumnName("cust_state")
                .HasMaxLength(2); // COBOL: CUST-STATE PIC X(2)

            builder.Property(c => c.ZipCode)
                .HasColumnName("cust_zip")
                .HasMaxLength(10); // COBOL: CUST-ZIP PIC X(10)
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

        public async Task AddTransactionAsync(IntcalcDomainModels.Transaction transaction)
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

    public class InterestCalculationService : IInterestCalculationService
    {
        public Task<IntcalcDomainModels.InterestCalculation> CalculateInterestAsync(decimal principalAmount, decimal interestRate)
        {
            var calculatedInterest = principalAmount * interestRate;
            var totalAmount = principalAmount + calculatedInterest;

            var result = new IntcalcDomainModels.InterestCalculation
            {
                PrincipalAmount = principalAmount,
                CalculatedInterest = calculatedInterest,
                TotalAmount = totalAmount
            };

            return Task.FromResult(result);
        }
    }

    public class ReportService : IReportService
    {
        public async Task GenerateAccountReportAsync(IntcalcDomainModels.Account account)
        {
            var reportBuilder = new StringBuilder();
            reportBuilder.AppendLine("Account Report");
            reportBuilder.AppendLine("----------------------");
            reportBuilder.AppendLine($"Account Number: {account.AccountNumber}");
            reportBuilder.AppendLine($"Account Name: {account.AccountName}");
            reportBuilder.AppendLine($"Interest Rate: {account.InterestRate}");
            reportBuilder.AppendLine($"Balance: {account.Balance}");
            reportBuilder.AppendLine($"Status: {account.Status}");
            reportBuilder.AppendLine("----------------------");

            var reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"AccountReport_{account.AccountNumber}.txt");
            await File.WriteAllTextAsync(reportPath, reportBuilder.ToString());
        }

        public async Task GenerateTransactionReportAsync(IntcalcDomainModels.Transaction transaction)
        {
            var reportBuilder = new StringBuilder();
            reportBuilder.AppendLine("Transaction Report");
            reportBuilder.AppendLine("----------------------");
            reportBuilder.AppendLine($"Transaction ID: {transaction.TransactionId}");
            reportBuilder.AppendLine($"Transaction Date: {transaction.TransactionDate:yyyy-MM-dd}");
            reportBuilder.AppendLine($"Amount: {transaction.Amount}");
            reportBuilder.AppendLine($"Currency: {transaction.CurrencyCode}");
            reportBuilder.AppendLine("----------------------");

            var reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"TransactionReport_{transaction.TransactionId}.txt");
            await File.WriteAllTextAsync(reportPath, reportBuilder.ToString());
        }
    }
}