using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCHEOD.Domain;
using BCHEOD.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BCHEOD.Infrastructure
{
    public class BcheodDbContext : DbContext
    {
        public BcheodDbContext(DbContextOptions<BcheodDbContext> options) : base(options) { }

        public DbSet<BcheodDomainModels.BcheodRecord> BcheodRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BcheodDomainModels.BcheodRecord>(entity =>
            {
                entity.ToTable("bcheod_records");

                entity.Property(e => e.RecordId)
                    .HasColumnName("record_id")
                    .HasMaxLength(10) // COBOL: RECORD-ID PIC X(10)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(20) // COBOL: DESCRIPTION PIC X(20)
                    .IsRequired();

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(9, 2) // COBOL: AMOUNT PIC 9(7)V99 COMP-3
                    .IsRequired();

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasColumnType("DATE") // COBOL: TRANSACTION-DATE PIC 9(8)
                    .IsRequired();

                entity.HasMany(e => e.TransactionDetails)
                    .WithOne()
                    .HasForeignKey("BcheodRecordId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BcheodDomainModels.BcheodRecord.TransactionDetail>(entity =>
            {
                entity.ToTable("transaction_details");

                entity.Property(e => e.DetailCode)
                    .HasColumnName("detail_code")
                    .HasMaxLength(5) // COBOL: DETAIL-CODE PIC X(5)
                    .IsRequired();

                entity.Property(e => e.DetailAmount)
                    .HasColumnName("detail_amount")
                    .HasPrecision(7, 2) // COBOL: DETAIL-AMOUNT PIC 9(5)V99 COMP-3
                    .IsRequired();

                entity.Property(e => e.DetailDescription)
                    .HasColumnName("detail_description")
                    .HasMaxLength(15) // COBOL: DETAIL-DESCRIPTION PIC X(15)
                    .IsRequired();
            });
        }
    }

    public class BcheodRepository : IBcheodRepository
    {
        private readonly BcheodDbContext _context;

        public BcheodRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public async Task<BcheodDomainModels.BcheodRecord?> GetByRecordIdAsync(string recordId)
        {
            return await _context.BcheodRecords
                .Include(r => r.TransactionDetails)
                .SingleOrDefaultAsync(r => r.RecordId == recordId);
        }

        public async Task AddAsync(BcheodDomainModels.BcheodRecord record)
        {
            await _context.BcheodRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BcheodDomainModels.BcheodRecord record)
        {
            _context.BcheodRecords.Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string recordId)
        {
            var record = await _context.BcheodRecords.SingleOrDefaultAsync(r => r.RecordId == recordId);
            if (record != null)
            {
                _context.BcheodRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<BcheodDomainModels.BcheodRecord>> GetAllAsync()
        {
            return await _context.BcheodRecords
                .Include(r => r.TransactionDetails)
                .ToListAsync();
        }
    }

    public static class BcheodInfrastructureExtensions
    {
        public static IServiceCollection AddBcheodInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<BcheodDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IBcheodRepository, BcheodRepository>();

            return services;
        }
    }
}