using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FRDCHK.Application.Interfaces;
using FRDCHK.Domain;

namespace FRDCHK.Infrastructure
{
    public class FrdchkDbContext : DbContext
    {
        public FrdchkDbContext(DbContextOptions<FrdchkDbContext> options) : base(options) { }

        public DbSet<FraudCheckRecord> FraudCheckRecords { get; set; }
        public DbSet<FraudCheckDetail> FraudCheckDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FraudCheckRecord>(entity =>
            {
                entity.ToTable("fraud_check_records");

                entity.Property(e => e.RecordId)
                    .HasColumnName("record_id")
                    .HasMaxLength(10); // COBOL: RECORD-ID PIC X(10)

                entity.Property(e => e.CustomerName)
                    .HasColumnName("customer_name")
                    .HasMaxLength(20); // COBOL: CUSTOMER-NAME PIC X(20)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(15, 2); // COBOL: TRANSACTION-AMOUNT PIC S9(13)V99 COMP-3

                entity.Property(e => e.CurrencyCode)
                    .HasColumnName("currency_code")
                    .HasMaxLength(3); // COBOL: CURRENCY-CODE PIC X(3)

                entity.Property(e => e.FraudIndicator)
                    .HasColumnName("fraud_indicator")
                    .HasMaxLength(1); // COBOL: FRAUD-INDICATOR PIC X(1)

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasMaxLength(8); // COBOL: TRANSACTION-DATE PIC X(8)

                entity.Property(e => e.TransactionTime)
                    .HasColumnName("transaction_time")
                    .HasMaxLength(6); // COBOL: TRANSACTION-TIME PIC X(6)

                entity.Property(e => e.MerchantName)
                    .HasColumnName("merchant_name")
                    .HasMaxLength(50); // COBOL: MERCHANT-NAME PIC X(50)

                entity.Property(e => e.MerchantCategory)
                    .HasColumnName("merchant_category")
                    .HasMaxLength(30); // COBOL: MERCHANT-CATEGORY PIC X(30)

                entity.Property(e => e.MerchantRating)
                    .HasColumnName("merchant_rating")
                    .HasPrecision(10, 2); // COBOL: MERCHANT-RATING PIC S9(8)V99 COMP-3
            });

            modelBuilder.Entity<FraudCheckDetail>(entity =>
            {
                entity.ToTable("fraud_check_details");

                entity.Property(e => e.DetailId)
                    .HasColumnName("detail_id")
                    .HasMaxLength(10); // COBOL: DETAIL-ID PIC X(10)

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(50); // COBOL: DESCRIPTION PIC X(50)

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(10, 2); // COBOL: AMOUNT PIC S9(8)V99 COMP-3

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(1); // COBOL: STATUS PIC X(1)
            });
        }
    }

    public class FraudCheckRepository : IFraudCheckRepository
    {
        private readonly FrdchkDbContext _context;

        public FraudCheckRepository(FrdchkDbContext context)
        {
            _context = context;
        }

        public async Task<FraudCheckRecord?> GetFraudCheckRecordByIdAsync(string recordId)
        {
            return await _context.FraudCheckRecords
                .Include(r => r.FraudCheckDetails)
                .SingleOrDefaultAsync(r => r.RecordId == recordId);
        }

        public async Task<List<FraudCheckRecord>> GetAllFraudCheckRecordsAsync()
        {
            return await _context.FraudCheckRecords
                .Include(r => r.FraudCheckDetails)
                .ToListAsync();
        }

        public async Task AddFraudCheckRecordAsync(FraudCheckRecord record)
        {
            await _context.FraudCheckRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFraudCheckRecordAsync(FraudCheckRecord record)
        {
            _context.FraudCheckRecords.Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFraudCheckRecordAsync(string recordId)
        {
            var record = await GetFraudCheckRecordByIdAsync(recordId);
            if (record != null)
            {
                _context.FraudCheckRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFrdchkInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<FrdchkDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IFraudCheckRepository, FraudCheckRepository>();

            return services;
        }
    }
}