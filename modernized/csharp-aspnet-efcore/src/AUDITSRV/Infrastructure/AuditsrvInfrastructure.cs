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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auditvsam>(entity =>
            {
                entity.ToTable("auditvsam", "public");

                entity.HasKey(e => e.AuditKey);

                entity.Property(e => e.AuditKey)
                    .HasColumnName("audit_key")
                    .HasMaxLength(20); // COBOL: AUDIT-KEY PIC X(20)

                entity.Property(e => e.AuditAcctNumber)
                    .HasColumnName("audit_acct_number")
                    .HasColumnType("numeric(10, 0)"); // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.AuditAction)
                    .HasColumnName("audit_action")
                    .HasMaxLength(16); // COBOL: AUDIT-ACTION PIC X(16)

                entity.Property(e => e.AuditUser)
                    .HasColumnName("audit_user")
                    .HasMaxLength(8); // COBOL: AUDIT-USER PIC X(08)

                entity.Property(e => e.AuditTs)
                    .HasColumnName("audit_ts")
                    .HasColumnType("timestamptz"); // COBOL: AUDIT-TS PIC X(26)

                entity.Property(e => e.AuditDetail)
                    .HasColumnName("audit_detail")
                    .HasMaxLength(120); // COBOL: AUDIT-DETAIL PIC X(120)
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("YourConnectionStringHere");
            }
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
                UserId = entity.AuditUser,
                AuditTimestamp = entity.AuditTs,
                Action = entity.AuditAction,
                Description = entity.AuditDetail,
                Amount = 0, // Placeholder, as no mapping for Amount is provided
                Status = AuditsrvDomainModels.AuditStatus.Pending, // Placeholder
                Details = null // Placeholder
            };
        }

        public void WriteAudit(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            var entity = new Auditvsam
            {
                AuditKey = auditRecord.AuditId,
                AuditAcctNumber = decimal.Parse(auditRecord.UserId), // Assuming UserId is numeric
                AuditAction = auditRecord.Action,
                AuditUser = auditRecord.UserId,
                AuditTs = auditRecord.AuditTimestamp,
                AuditDetail = auditRecord.Description
            };

            _context.Auditvsam.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateAudit(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            var entity = _context.Auditvsam.FirstOrDefault(a => a.AuditKey == auditRecord.AuditId);
            if (entity == null) throw new KeyNotFoundException("Audit record not found.");

            entity.AuditAcctNumber = decimal.Parse(auditRecord.UserId); // Assuming UserId is numeric
            entity.AuditAction = auditRecord.Action;
            entity.AuditUser = auditRecord.UserId;
            entity.AuditTs = auditRecord.AuditTimestamp;
            entity.AuditDetail = auditRecord.Description;

            _context.SaveChanges();
        }

        public void DeleteAudit(string auditId)
        {
            var entity = _context.Auditvsam.FirstOrDefault(a => a.AuditKey == auditId);
            if (entity == null) throw new KeyNotFoundException("Audit record not found.");

            _context.Auditvsam.Remove(entity);
            _context.SaveChanges();
        }
    }

    public class AuditCicsService : IAuditCicsService
    {
        public AuditsrvDomainModels.AuditStatus ProcessAuditTransaction(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            // Placeholder for CICS transaction logic
            return AuditsrvDomainModels.AuditStatus.Completed;
        }

        public AuditsrvDomainModels.AuditStatus GetTransactionStatus(string transactionId)
        {
            // Placeholder for CICS transaction status retrieval logic
            return AuditsrvDomainModels.AuditStatus.Completed;
        }
    }
}