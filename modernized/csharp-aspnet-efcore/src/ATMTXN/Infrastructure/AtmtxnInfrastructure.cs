using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ATMTXN.Application.Interfaces;
using ATMTXN.Domain;

namespace ATMTXN.Infrastructure
{
    public class AtmtxnDbContext : DbContext
    {
        public DbSet<AtmtxnDomainModels.Transaction> Transactions { get; set; }

        public AtmtxnDbContext(DbContextOptions<AtmtxnDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AtmtxnDomainModels.Transaction>(entity =>
            {
                entity.ToTable("transactions");

                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(12); // COBOL: TRANSACTION-ID PIC X(12)

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(16); // COBOL: ACCOUNT-NUMBER PIC X(16)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(15, 2); // COBOL: TRANSACTION-AMOUNT PIC 9(13)V99 COMP-3

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .HasConversion<int>(); // COBOL: TRANSACTION-TYPE (88-level equivalent)

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasPrecision(8, 0); // COBOL: TRANSACTION-DATE PIC 9(8)

                entity.Property(e => e.TransactionTime)
                    .HasColumnName("transaction_time")
                    .HasMaxLength(6); // COBOL: TRANSACTION-TIME PIC X(6)

                entity.Property(e => e.CurrencyCode)
                    .HasColumnName("currency_code")
                    .HasMaxLength(3); // COBOL: CURRENCY-CODE PIC X(3)

                entity.Property(e => e.BalanceAfterTransaction)
                    .HasColumnName("balance_after_transaction")
                    .HasPrecision(15, 2); // COBOL: BALANCE-AFTER-TRANSACTION PIC 9(13)V99 COMP-3

                entity.Property(e => e.TransactionStatus)
                    .HasColumnName("transaction_status")
                    .HasConversion<int>(); // COBOL: TRANSACTION-STATUS (88-level equivalent)
            });
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AtmtxnDbContext _dbContext;

        public TransactionRepository(AtmtxnDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AtmtxnDomainModels.Transaction> GetTransactionByIdAsync(string transactionId)
        {
            return await _dbContext.Transactions
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }

        public async Task SaveTransactionAsync(AtmtxnDomainModels.Transaction transaction)
        {
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTransactionAsync(AtmtxnDomainModels.Transaction transaction)
        {
            _dbContext.Transactions.Update(transaction);
            await _dbContext.SaveChangesAsync();
        }
    }

    public class AccountService : IAccountService
    {
        private readonly AtmtxnDbContext _dbContext;

        public AccountService(AtmtxnDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<decimal> GetAccountBalanceAsync(string accountNumber)
        {
            var transaction = await _dbContext.Transactions
                .OrderByDescending(t => t.TransactionDate)
                .ThenByDescending(t => t.TransactionTime)
                .FirstOrDefaultAsync(t => t.AccountNumber == accountNumber);

            return transaction?.BalanceAfterTransaction ?? 0m;
        }

        public async Task UpdateAccountBalanceAsync(string accountNumber, decimal newBalance)
        {
            var transaction = await _dbContext.Transactions
                .OrderByDescending(t => t.TransactionDate)
                .ThenByDescending(t => t.TransactionTime)
                .FirstOrDefaultAsync(t => t.AccountNumber == accountNumber);

            if (transaction != null)
            {
                transaction.BalanceAfterTransaction = newBalance;
                _dbContext.Transactions.Update(transaction);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

    public class CicsService : ICicsService
    {
        public async Task<bool> PerformCicsOperationAsync(AtmtxnDomainModels.Transaction transaction)
        {
            // Simulate a CICS operation
            await Task.Delay(100); // Simulating some processing delay
            return true; // Assume the operation is always successful
        }
    }
}