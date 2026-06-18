using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ATMTXN.Application.Interfaces;
using ATMTXN.Domain;

namespace ATMTXN.Infrastructure
{
    public class AtmtxnDbContext : DbContext
    {
        public DbSet<AtmtxnDomainModels.TransactionRecord> Transactions { get; set; }

        public AtmtxnDbContext(DbContextOptions<AtmtxnDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AtmtxnDomainModels.TransactionRecord>(entity =>
            {
                entity.ToTable("transaction_records");

                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(12); // COBOL: TransactionId PIC X(12)

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(16); // COBOL: AccountNumber PIC X(16)

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .HasMaxLength(3); // COBOL: TransactionType PIC X(3)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(15, 2); // COBOL: TransactionAmount PIC 9(13)V99 COMP-3

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasMaxLength(8); // COBOL: TransactionDate PIC X(8)

                entity.Property(e => e.TransactionTime)
                    .HasColumnName("transaction_time")
                    .HasMaxLength(6); // COBOL: TransactionTime PIC X(6)

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(1); // COBOL: Status PIC X(1)

                entity.OwnsOne(e => e.Location, location =>
                {
                    location.Property(l => l.City)
                        .HasColumnName("location_city")
                        .HasMaxLength(20); // COBOL: City PIC X(20)

                    location.Property(l => l.State)
                        .HasColumnName("location_state")
                        .HasMaxLength(2); // COBOL: State PIC X(2)

                    location.Property(l => l.CountryCode)
                        .HasColumnName("location_country_code")
                        .HasMaxLength(10); // COBOL: CountryCode PIC X(10)
                });
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

        public async Task<IEnumerable<AtmtxnDomainModels.TransactionRecord>> ReadAllTransactionsAsync()
        {
            return await _dbContext.Transactions
                .Include(t => t.Location)
                .ToListAsync();
        }

        public async Task WriteTransactionAsync(AtmtxnDomainModels.TransactionRecord transactionRecord)
        {
            _dbContext.Transactions.Add(transactionRecord);
            await _dbContext.SaveChangesAsync();
        }
    }

    public class CicsService : ICicsService
    {
        public async Task<AtmtxnDomainModels.TransactionStatus> ProcessTransactionAsync(AtmtxnDomainModels.TransactionRecord transactionRecord)
        {
            // Simulate a CICS transaction processing call
            // In a real implementation, this would involve calling the actual CICS system
            await Task.Delay(100); // Simulate network latency
            return AtmtxnDomainModels.TransactionStatus.Approved; // Simulated response
        }
    }
}