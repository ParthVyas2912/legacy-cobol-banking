using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AUDITSRV.Domain;
using AUDITSRV.Application.Interfaces;

namespace AUDITSRV.Infrastructure
{
    public class AuditsrvDbContext : DbContext
    {
        public DbSet<Auditvsam> Auditvsam { get; set; }

        public AuditsrvDbContext(DbContextOptions<AuditsrvDbContext> options) : base(options) { }

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
                    .HasColumnType("decimal(10,0)") // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)
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
                AuditDate = entity.AuditTs,
                AuditAmount = 0 // Placeholder as no equivalent field exists in the COBOL mapping
            };
        }

        public void WriteAudit(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            var entity = new Auditvsam
            {
                AuditKey = auditRecord.AuditId,
                AuditTs = auditRecord.AuditDate,
                AuditAcctNumber = 0, // Placeholder as no equivalent field exists in the domain model
                AuditAction = string.Empty, // Placeholder as no equivalent field exists in the domain model
                AuditUser = string.Empty, // Placeholder as no equivalent field exists in the domain model
                AuditDetail = string.Empty // Placeholder as no equivalent field exists in the domain model
            };

            _context.Auditvsam.Add(entity);
            _context.SaveChanges();
        }

        public void DeleteAuditById(string auditId)
        {
            var entity = _context.Auditvsam.FirstOrDefault(a => a.AuditKey == auditId);
            if (entity != null)
            {
                _context.Auditvsam.Remove(entity);
                _context.SaveChanges();
            }
        }
    }

    public class AuditCicsService : IAuditCicsService
    {
        public void SendAuditToCics(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            // Implementation for sending audit record to external CICS system
            // Simulating a CICS call by logging the audit record
            Console.WriteLine($"Sending Audit Record to CICS: {auditRecord.AuditId}, Date: {auditRecord.AuditDate}, Amount: {auditRecord.AuditAmount}");
        }

        public AuditsrvDomainModels.AuditRecord RetrieveAuditFromCics(string auditId)
        {
            // Implementation for retrieving audit record from external CICS system
            // Simulating a CICS call by returning a mock record
            return new AuditsrvDomainModels.AuditRecord
            {
                AuditId = auditId,
                AuditDate = DateTime.UtcNow,
                AuditAmount = 100.00M // Mock amount
            };
        }
    }
}