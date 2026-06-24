using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ATMTXN.Domain;
using ATMTXN.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ATMTXN.Infrastructure
{
    public class AtmtxnDbContext : DbContext
    {
        public DbSet<AtmtxnDomainModels.TransactionRecord> Transactions { get; set; }
        public DbSet<AtmtxnDomainModels.TransactionDetail> TransactionDetails { get; set; }

        public AtmtxnDbContext(DbContextOptions<AtmtxnDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AtmtxnDomainModels.TransactionRecord>(entity =>
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
                      .HasPrecision(15, 2); // COBOL: TRANSACTION-AMOUNT PIC S9(13)V99 COMP-3

                entity.Property(e => e.TransactionType)
                      .HasColumnName("transaction_type")
                      .HasMaxLength(1); // COBOL: TRANSACTION-TYPE PIC X(1)

                entity.Property(e => e.Status)
                      .HasColumnName("status")
                      .HasConversion<string>(); // COBOL: STATUS (88-level equivalent)

                entity.Property(e => e.TransactionDate)
                      .HasColumnName("transaction_date")
                      .HasMaxLength(8); // COBOL: TRANSACTION-DATE PIC 9(8)

                entity.OwnsOne(e => e.Location, location =>
                {
                    location.Property(l => l.City)
                            .HasColumnName("location_city")
                            .HasMaxLength(20); // COBOL: CITY PIC X(20)

                    location.Property(l => l.State)
                            .HasColumnName("location_state")
                            .HasMaxLength(2); // COBOL: STATE PIC X(2)

                    location.Property(l => l.ZipCode)
                            .HasColumnName("location_zip_code")
                            .HasMaxLength(10); // COBOL: ZIP-CODE PIC X(10)
                });
            });

            modelBuilder.Entity<AtmtxnDomainModels.TransactionDetail>(entity =>
            {
                entity.ToTable("transaction_details");

                entity.HasKey(e => new { e.ItemCode, e.Description });

                entity.Property(e => e.ItemCode)
                      .HasColumnName("item_code")
                      .HasMaxLength(10); // COBOL: ITEM-CODE PIC X(10)

                entity.Property(e => e.ItemAmount)
                      .HasColumnName("item_amount")
                      .HasPrecision(9, 2); // COBOL: ITEM-AMOUNT PIC S9(7)V99 COMP-3

                entity.Property(e => e.Description)
                      .HasColumnName("description")
                      .HasMaxLength(30); // COBOL: DESCRIPTION PIC X(30)
            });
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AtmtxnDbContext _context;

        public TransactionRepository(AtmtxnDbContext context)
        {
            _context = context;
        }

        public async Task<AtmtxnDomainModels.TransactionRecord?> GetTransactionByIdAsync(string transactionId)
        {
            return await _context.Transactions
                                 .AsNoTracking()
                                 .SingleOrDefaultAsync(t => t.TransactionId == transactionId);
        }

        public async Task<IEnumerable<AtmtxnDomainModels.TransactionRecord>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task AddTransactionAsync(AtmtxnDomainModels.TransactionRecord transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransactionAsync(AtmtxnDomainModels.TransactionRecord transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(string transactionId)
        {
            var transaction = await _context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == transactionId);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class TransactionDetailRepository : ITransactionDetailRepository
    {
        private readonly AtmtxnDbContext _context;

        public TransactionDetailRepository(AtmtxnDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AtmtxnDomainModels.TransactionDetail>> GetTransactionDetailsByTransactionIdAsync(string transactionId)
        {
            return await _context.TransactionDetails
                                 .AsNoTracking()
                                 .Where(d => d.ItemCode.StartsWith(transactionId))
                                 .ToListAsync();
        }

        public async Task AddTransactionDetailAsync(AtmtxnDomainModels.TransactionDetail detail)
        {
            await _context.TransactionDetails.AddAsync(detail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransactionDetailAsync(AtmtxnDomainModels.TransactionDetail detail)
        {
            _context.TransactionDetails.Update(detail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransactionDetailAsync(string transactionId, string itemCode)
        {
            var detail = await _context.TransactionDetails
                                       .SingleOrDefaultAsync(d => d.ItemCode == itemCode && d.Description.StartsWith(transactionId));
            if (detail != null)
            {
                _context.TransactionDetails.Remove(detail);
                await _context.SaveChangesAsync();
            }
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAtmtxnInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AtmtxnDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionDetailRepository, TransactionDetailRepository>();
            return services;
        }
    }
}