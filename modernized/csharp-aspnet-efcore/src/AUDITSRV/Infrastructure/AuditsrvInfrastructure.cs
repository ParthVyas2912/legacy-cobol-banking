using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AUDITSRV.Application.Interfaces;
using AUDITSRV.Domain;

namespace AUDITSRV.Infrastructure
{
    public class AuditsrvDbContext : DbContext
    {
        public DbSet<Auditvsam> Auditvsam { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection string here
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auditvsam>(entity =>
            {
                entity.ToTable("auditvsam", "public");

                entity.HasKey(e => e.AuditKey);

                entity.Property(e => e.AuditKey)
                    .HasColumnName("audit_key")
                    .HasMaxLength(20) // COBOL: AUDIT-KEY PIC X(20)
                    .IsRequired();

                entity.Property(e => e.AuditAcctNumber)
                    .HasColumnName("audit_acct_number")
                    .HasColumnType("numeric(10,0)") // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.AuditAction)
                    .HasColumnName("audit_action")
                    .HasMaxLength(16) // COBOL: AUDIT-ACTION PIC X(16)
                    .IsRequired();

                entity.Property(e => e.AuditUser)
                    .HasColumnName("audit_user")
                    .HasMaxLength(8) // COBOL: AUDIT-USER PIC X(08)
                    .IsRequired();

                entity.Property(e => e.AuditTs)
                    .HasColumnName("audit_ts")
                    .HasColumnType("timestamptz") // COBOL: AUDIT-TS PIC X(26)
                    .IsRequired();

                entity.Property(e => e.AuditDetail)
                    .HasColumnName("audit_detail")
                    .HasMaxLength(120) // COBOL: AUDIT-DETAIL PIC X(120)
                    .IsRequired();
            });
        }
    }

    public class Auditvsam
    {
        public string AuditKey { get; set; } // COBOL: AUDIT-KEY PIC X(20)
        public decimal AuditAcctNumber { get; set; } // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)
        public string AuditAction { get; set; } // COBOL: AUDIT-ACTION PIC X(16)
        public string AuditUser { get; set; } // COBOL: AUDIT-USER PIC X(08)
        public DateTime AuditTs { get; set; } // COBOL: AUDIT-TS PIC X(26)
        public string AuditDetail { get; set; } // COBOL: AUDIT-DETAIL PIC X(120)
    }

    public class AuditRepository : IAuditRepository
    {
        private readonly AuditsrvDbContext _context;

        public AuditRepository(AuditsrvDbContext context)
        {
            _context = context;
        }

        public AuditsrvDomainModels.AuditRecord ReadAuditById(string auditId)
        {
            var entity = _context.Auditvsam.FirstOrDefault(a => a.AuditKey == auditId);
            if (entity == null) return null;

            return new AuditsrvDomainModels.AuditRecord
            {
                AuditId = entity.AuditKey,
                AuditNotes = entity.AuditDetail,
                AuditDate = entity.AuditTs,
                // Map other fields as necessary
            };
        }

        public void WriteAudit(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            var entity = new Auditvsam
            {
                AuditKey = auditRecord.AuditId,
                AuditDetail = auditRecord.AuditNotes,
                AuditTs = auditRecord.AuditDate,
                // Map other fields as necessary
            };

            _context.Auditvsam.Add(entity);
            _context.SaveChanges();
        }

        public void DeleteAudit(string auditId)
        {
            var entity = _context.Auditvsam.FirstOrDefault(a => a.AuditKey == auditId);
            if (entity != null)
            {
                _context.Auditvsam.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void UpdateAudit(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            var entity = _context.Auditvsam.FirstOrDefault(a => a.AuditKey == auditRecord.AuditId);
            if (entity != null)
            {
                entity.AuditDetail = auditRecord.AuditNotes;
                entity.AuditTs = auditRecord.AuditDate;
                // Update other fields as necessary

                _context.SaveChanges();
            }
        }

        public List<AuditsrvDomainModels.AuditRecord> GetAllAudits()
        {
            return _context.Auditvsam.Select(entity => new AuditsrvDomainModels.AuditRecord
            {
                AuditId = entity.AuditKey,
                AuditNotes = entity.AuditDetail,
                AuditDate = entity.AuditTs,
                // Map other fields as necessary
            }).ToList();
        }
    }
}