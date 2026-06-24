using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCHEOD.Domain;
using BCHEOD.Application.Interfaces;

namespace BCHEOD.Infrastructure
{
    public class BcheodDbContext : DbContext
    {
        public BcheodDbContext(DbContextOptions<BcheodDbContext> options) : base(options) { }

        public DbSet<BcheodDomainModels.RootRecord> RootRecords { get; set; }
        public DbSet<BcheodDomainModels.Header> Headers { get; set; }
        public DbSet<BcheodDomainModels.Detail> Details { get; set; }
        public DbSet<BcheodDomainModels.Trailer> Trailers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BcheodDomainModels.RootRecord>(entity =>
            {
                entity.ToTable("root_records");
                entity.HasKey(e => e.RecordId);
                entity.Property(e => e.RecordId).HasMaxLength(10).IsRequired().HasColumnName("record_id"); // COBOL: RECORD-ID PIC X(10)
            });

            modelBuilder.Entity<BcheodDomainModels.Header>(entity =>
            {
                entity.ToTable("headers");
                entity.HasKey(e => e.FileDate);
                entity.Property(e => e.FileDate).HasMaxLength(8).IsRequired().HasColumnName("file_date"); // COBOL: FILE-DATE PIC X(8)
                entity.Property(e => e.FileTime).HasMaxLength(6).IsRequired().HasColumnName("file_time"); // COBOL: FILE-TIME PIC X(6)
                entity.Property(e => e.FileName).HasMaxLength(20).IsRequired().HasColumnName("file_name"); // COBOL: FILE-NAME PIC X(20)
            });

            modelBuilder.Entity<BcheodDomainModels.Detail>(entity =>
            {
                entity.ToTable("details");
                entity.HasKey(e => e.AccountNumber);
                entity.Property(e => e.AccountNumber).HasMaxLength(15).IsRequired().HasColumnName("account_number"); // COBOL: ACCOUNT-NUMBER PIC X(15)
                entity.Property(e => e.TransactionAmount).HasPrecision(15, 2).IsRequired().HasColumnName("transaction_amount"); // COBOL: TRANSACTION-AMOUNT PIC S9(13)V99 COMP-3
                entity.Property(e => e.CurrencyCode).HasMaxLength(3).IsRequired().HasColumnName("currency_code"); // COBOL: CURRENCY-CODE PIC X(3)
                entity.Property(e => e.TransactionType).HasMaxLength(1).IsRequired().HasColumnName("transaction_type"); // COBOL: TRANSACTION-TYPE PIC X(1)
                entity.Property(e => e.Status).HasColumnName("status").IsRequired();
            });

            modelBuilder.Entity<BcheodDomainModels.Trailer>(entity =>
            {
                entity.ToTable("trailers");
                entity.HasKey(e => e.RecordCount);
                entity.Property(e => e.RecordCount).HasMaxLength(8).IsRequired().HasColumnName("record_count"); // COBOL: RECORD-COUNT PIC X(8)
                entity.Property(e => e.TotalAmount).HasPrecision(17, 2).IsRequired().HasColumnName("total_amount"); // COBOL: TOTAL-AMOUNT PIC S9(15)V99 COMP-3
            });
        }
    }

    public class RootRecordRepository : IRootRecordRepository
    {
        private readonly BcheodDbContext _context;

        public RootRecordRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public async Task<BcheodDomainModels.RootRecord?> GetByIdAsync(string recordId)
        {
            return await _context.RootRecords.SingleOrDefaultAsync(r => r.RecordId == recordId);
        }

        public async Task<IEnumerable<BcheodDomainModels.RootRecord>> GetAllAsync()
        {
            return await _context.RootRecords.ToListAsync();
        }

        public async Task AddAsync(BcheodDomainModels.RootRecord rootRecord)
        {
            await _context.RootRecords.AddAsync(rootRecord);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BcheodDomainModels.RootRecord rootRecord)
        {
            _context.RootRecords.Update(rootRecord);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string recordId)
        {
            var record = await GetByIdAsync(recordId);
            if (record != null)
            {
                _context.RootRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class HeaderRepository : IHeaderRepository
    {
        private readonly BcheodDbContext _context;

        public HeaderRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public async Task<BcheodDomainModels.Header?> GetByFileDateAsync(string fileDate)
        {
            return await _context.Headers.SingleOrDefaultAsync(h => h.FileDate == fileDate);
        }

        public async Task AddAsync(BcheodDomainModels.Header header)
        {
            await _context.Headers.AddAsync(header);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BcheodDomainModels.Header header)
        {
            _context.Headers.Update(header);
            await _context.SaveChangesAsync();
        }
    }

    public class DetailRepository : IDetailRepository
    {
        private readonly BcheodDbContext _context;

        public DetailRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BcheodDomainModels.Detail>> GetByAccountNumberAsync(string accountNumber)
        {
            return await _context.Details.Where(d => d.AccountNumber == accountNumber).ToListAsync();
        }

        public async Task AddAsync(BcheodDomainModels.Detail detail)
        {
            await _context.Details.AddAsync(detail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BcheodDomainModels.Detail detail)
        {
            _context.Details.Update(detail);
            await _context.SaveChangesAsync();
        }
    }

    public class TrailerRepository : ITrailerRepository
    {
        private readonly BcheodDbContext _context;

        public TrailerRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public async Task<BcheodDomainModels.Trailer?> GetByRecordCountAsync(string recordCount)
        {
            return await _context.Trailers.SingleOrDefaultAsync(t => t.RecordCount == recordCount);
        }

        public async Task AddAsync(BcheodDomainModels.Trailer trailer)
        {
            await _context.Trailers.AddAsync(trailer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BcheodDomainModels.Trailer trailer)
        {
            _context.Trailers.Update(trailer);
            await _context.SaveChangesAsync();
        }
    }
}