using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TXNPOST.Application.Interfaces;
using TXNPOST.Domain;

namespace TXNPOST.Infrastructure
{
    public class TxnpostDbContext : DbContext
    {
        public DbSet<TxnpostDomainModels.Transaction> Transactions { get; set; }
        public DbSet<TxnpostDomainModels.TransactionDetail> TransactionDetails { get; set; }

        public TxnpostDbContext(DbContextOptions<TxnpostDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TxnpostDomainModels.Transaction>(entity =>
            {
                entity.ToTable("txnin", "public");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("txn_id")
                    .HasMaxLength(12)
                    .IsFixedLength(); // COBOL: TXN-ID PIC 9(12)

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("txn_acct_number")
                    .HasMaxLength(10)
                    .IsFixedLength(); // COBOL: TXN-ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("txn_amount")
                    .HasPrecision(13, 2); // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("txn_post_date")
                    .HasMaxLength(8)
                    .IsFixedLength(); // COBOL: TXN-POST-DATE PIC 9(8)

                entity.Property(e => e.TransactionType)
                    .HasColumnName("txn_code")
                    .HasMaxLength(3)
                    .IsFixedLength(); // COBOL: TXN-CODE PIC X(03)

                entity.Property(e => e.Status)
                    .HasColumnName("txn_status")
                    .HasMaxLength(1)
                    .IsFixedLength(); // COBOL: TXN-STATUS PIC X(01)
            });

            modelBuilder.Entity<TxnpostDomainModels.TransactionDetail>(entity =>
            {
                entity.ToTable("auditout", "public");

                entity.Property(e => e.DetailId)
                    .HasColumnName("txn_ref_number")
                    .HasMaxLength(20)
                    .IsFixedLength(); // COBOL: TXN-REF-NUMBER PIC X(20)

                entity.Property(e => e.DetailAmount)
                    .HasColumnName("txn_balance_after")
                    .HasPrecision(15, 2); // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3

                entity.Property(e => e.Description)
                    .HasColumnName("txn_description")
                    .HasMaxLength(40)
                    .IsFixedLength(); // COBOL: TXN-DESCRIPTION PIC X(40)
            });
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly TxnpostDbContext _context;

        public TransactionRepository(TxnpostDbContext context)
        {
            _context = context;
        }

        public async Task<TxnpostDomainModels.Transaction?> GetTransactionByIdAsync(string transactionId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.TransactionId == transactionId);
        }

        public async Task<List<TxnpostDomainModels.Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddTransactionAsync(TxnpostDomainModels.Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransactionAsync(TxnpostDomainModels.Transaction transaction)
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
        private readonly TxnpostDbContext _context;

        public TransactionDetailRepository(TxnpostDbContext context)
        {
            _context = context;
        }

        public async Task<TxnpostDomainModels.TransactionDetail?> GetTransactionDetailByIdAsync(string detailId)
        {
            return await _context.TransactionDetails
                .AsNoTracking()
                .SingleOrDefaultAsync(td => td.DetailId == detailId);
        }

        public async Task<List<TxnpostDomainModels.TransactionDetail>> GetTransactionDetailsByTransactionIdAsync(string transactionId)
        {
            return await _context.TransactionDetails
                .AsNoTracking()
                .Where(td => td.DetailId == transactionId)
                .ToListAsync();
        }

        public async Task AddTransactionDetailAsync(TxnpostDomainModels.TransactionDetail transactionDetail)
        {
            await _context.TransactionDetails.AddAsync(transactionDetail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransactionDetailAsync(TxnpostDomainModels.TransactionDetail transactionDetail)
        {
            _context.TransactionDetails.Update(transactionDetail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransactionDetailAsync(string detailId)
        {
            var transactionDetail = await _context.TransactionDetails.SingleOrDefaultAsync(td => td.DetailId == detailId);
            if (transactionDetail != null)
            {
                _context.TransactionDetails.Remove(transactionDetail);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class CicsService : ICicsService
    {
        public async Task LinkAsync(string programName, object requestData, object responseData)
        {
            // Simulate a CICS LINK operation
            await Task.CompletedTask;
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddTxnpostInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<TxnpostDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionDetailRepository, TransactionDetailRepository>();
            services.AddScoped<ICicsService, CicsService>();
        }
    }
}