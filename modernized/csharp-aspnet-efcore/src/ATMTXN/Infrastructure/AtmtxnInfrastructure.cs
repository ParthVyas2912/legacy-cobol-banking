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
                    .HasMaxLength(12)
                    .IsRequired(); // COBOL: TransactionId PIC X(12)

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(10)
                    .IsRequired(); // COBOL: AccountNumber PIC X(10)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(15, 2)
                    .IsRequired(); // COBOL: TransactionAmount PIC S9(13)V99 COMP-3

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasMaxLength(6)
                    .IsRequired(); // COBOL: TransactionDate PIC X(6)

                entity.Property(e => e.TransactionTime)
                    .HasColumnName("transaction_time")
                    .HasMaxLength(6)
                    .IsRequired(); // COBOL: TransactionTime PIC X(6)

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .IsRequired(); // COBOL: TransactionTypeEnum (88-level equivalent)

                entity.Property(e => e.TransactionStatus)
                    .HasColumnName("transaction_status")
                    .IsRequired(); // COBOL: TransactionStatusEnum (88-level equivalent)

                entity.OwnsOne(e => e.Details, details =>
                {
                    details.Property(d => d.MerchantName)
                        .HasColumnName("merchant_name")
                        .HasMaxLength(20)
                        .IsRequired(); // COBOL: MerchantName PIC X(20)

                    details.Property(d => d.MerchantId)
                        .HasColumnName("merchant_id")
                        .HasMaxLength(15)
                        .IsRequired(); // COBOL: MerchantId PIC X(15)

                    details.Property(d => d.MerchantAmount)
                        .HasColumnName("merchant_amount")
                        .HasPrecision(15, 2)
                        .IsRequired(); // COBOL: MerchantAmount PIC S9(13)V99 COMP-3
                });
            });
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AtmtxnDbContext _dbContext;

        public TransactionRepository(AtmtxnDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<AtmtxnDomainModels.Transaction> GetTransactionByIdAsync(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new ArgumentException("Transaction ID cannot be null or empty.", nameof(transactionId));

            return await _dbContext.Transactions
                .Include(t => t.Details)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }

        public async Task SaveTransactionAsync(AtmtxnDomainModels.Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTransactionStatusAsync(string transactionId, AtmtxnDomainModels.TransactionStatusEnum status)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new ArgumentException("Transaction ID cannot be null or empty.", nameof(transactionId));

            var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.TransactionId == transactionId);
            if (transaction == null)
                throw new InvalidOperationException($"Transaction with ID {transactionId} not found.");

            transaction.TransactionStatus = status;
            _dbContext.Transactions.Update(transaction);
            await _dbContext.SaveChangesAsync();
        }
    }

    public class CicsService : ICicsService
    {
        public async Task<AtmtxnDomainModels.TransactionStatusEnum> ProcessTransactionAsync(AtmtxnDomainModels.Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            // Simulate a call to an external CICS service
            await Task.Delay(500); // Simulate network latency

            // For demonstration purposes, assume all transactions are successful
            return AtmtxnDomainModels.TransactionStatusEnum.Completed;
        }
    }
}