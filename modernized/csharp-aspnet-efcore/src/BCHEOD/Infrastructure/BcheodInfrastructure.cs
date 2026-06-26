using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BCHEOD.Domain;
using BCHEOD.Application.Interfaces;

namespace BCHEOD.Infrastructure
{
    public class BcheodDbContext : DbContext
    {
        public DbSet<EodRuns> EodRuns { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection string here
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EodRuns>(entity =>
            {
                entity.ToTable("eod_runs", "public");

                entity.Property(e => e.StartedAt)
                    .HasColumnName("started_at")
                    .HasColumnType("timestamptz")
                    .IsRequired();
                // COBOL: STARTED_AT TIMESTAMPTZ

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("varchar")
                    .HasMaxLength(50)
                    .IsRequired();
                // COBOL: STATUS PIC X(50)

                entity.Property(e => e.EndedAt)
                    .HasColumnName("ended_at")
                    .HasColumnType("timestamptz")
                    .IsRequired();
                // COBOL: ENDED_AT TIMESTAMPTZ

                entity.Property(e => e.WsStatusCode)
                    .HasColumnName("ws_status_code")
                    .HasColumnType("integer")
                    .IsRequired();
                // COBOL: WS_STATUS_CODE PIC 9(5) COMP
            });
        }
    }

    public class EodRuns
    {
        public DateTime StartedAt { get; set; } // COBOL: STARTED_AT TIMESTAMPTZ
        public string Status { get; set; } // COBOL: STATUS PIC X(50)
        public DateTime EndedAt { get; set; } // COBOL: ENDED_AT TIMESTAMPTZ
        public int WsStatusCode { get; set; } // COBOL: WS_STATUS_CODE PIC 9(5) COMP
    }

    public class BcheodRepository : IBcheodRepository
    {
        private readonly BcheodDbContext _context;

        public BcheodRepository(BcheodDbContext context)
        {
            _context = context;
        }

        public BcheodDomainModels.BcheodFile ReadBcheodFile()
        {
            // Implementation for reading BCHEOD file
            var bcheodFile = new BcheodDomainModels.BcheodFile
            {
                // Assuming the BCHEOD file is stored in the database as a single record
                Details = _context.EodRuns.Select(e => new BcheodDomainModels.Detail
                {
                    StartedAt = e.StartedAt,
                    Status = e.Status,
                    EndedAt = e.EndedAt,
                    WsStatusCode = e.WsStatusCode
                }).ToList()
            };

            return bcheodFile;
        }

        public void WriteBcheodFile(BcheodDomainModels.BcheodFile bcheodFile)
        {
            // Implementation for writing BCHEOD file
            foreach (var detail in bcheodFile.Details)
            {
                var eodRun = new EodRuns
                {
                    StartedAt = detail.StartedAt,
                    Status = detail.Status,
                    EndedAt = detail.EndedAt,
                    WsStatusCode = detail.WsStatusCode
                };

                _context.EodRuns.Add(eodRun);
            }

            _context.SaveChanges();
        }

        public void AppendDetail(BcheodDomainModels.Detail detail)
        {
            // Implementation for appending a detail record
            var eodRun = new EodRuns
            {
                StartedAt = detail.StartedAt,
                Status = detail.Status,
                EndedAt = detail.EndedAt,
                WsStatusCode = detail.WsStatusCode
            };

            _context.EodRuns.Add(eodRun);
            _context.SaveChanges();
        }

        public void DeleteDetail(int detailId)
        {
            // Implementation for deleting a detail record
            var eodRun = _context.EodRuns.Find(detailId);
            if (eodRun != null)
            {
                _context.EodRuns.Remove(eodRun);
                _context.SaveChanges();
            }
        }
    }
}