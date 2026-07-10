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
                    .HasMaxLength(20); // COBOL: AUDIT-KEY PIC X(20)

                entity.Property(e => e.AuditAcctNumber)
                    .HasColumnName("audit_acct_number")
                    .HasColumnType("numeric(10,0)"); // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.AuditAction)
                    .HasColumnName("audit_action")
                    .HasMaxLength(16); // COBOL: AUDIT-ACTION PIC X(16)

                entity.Property(e => e.AuditUser)
                    .HasColumnName("audit_user")
                    .HasMaxLength(8); // COBOL: AUDIT-USER PIC X(08)

                entity.Property(e => e.AuditTs)
                    .HasColumnName("audit_ts")
                    .HasMaxLength(26); // COBOL: AUDIT-TS PIC X(26)

                entity.Property(e => e.AuditDetail)
                    .HasColumnName("audit_detail")
                    .HasMaxLength(120); // COBOL: AUDIT-DETAIL PIC X(120)
            });
        }
    }

    public class Auditvsam
    {
        public string AuditKey { get; set; } // COBOL: AUDIT-KEY PIC X(20)
        public decimal AuditAcctNumber { get; set; } // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)
        public string AuditAction { get; set; } // COBOL: AUDIT-ACTION PIC X(16)
        public string AuditUser { get; set; } // COBOL: AUDIT-USER PIC X(08)
        public string AuditTs { get; set; } // COBOL: AUDIT-TS PIC X(26)
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

            return MapToDomainModel(entity);
        }

        public void WriteAudit(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            var entity = MapToEntity(auditRecord);
            _context.Auditvsam.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateAudit(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            var entity = _context.Auditvsam.FirstOrDefault(a => a.AuditKey == auditRecord.AuditId);
            if (entity == null) throw new InvalidOperationException("Audit record not found.");

            entity.AuditAcctNumber = decimal.Parse(auditRecord.AuditId);
            entity.AuditAction = auditRecord.Status.ToString();
            entity.AuditUser = auditRecord.AuditorName;
            entity.AuditTs = auditRecord.AuditDate.ToString("o");
            entity.AuditDetail = auditRecord.Details.Description;

            _context.SaveChanges();
        }

        public void DeleteAudit(string auditId)
        {
            var entity = _context.Auditvsam.FirstOrDefault(a => a.AuditKey == auditId);
            if (entity == null) throw new InvalidOperationException("Audit record not found.");

            _context.Auditvsam.Remove(entity);
            _context.SaveChanges();
        }

        public List<AuditsrvDomainModels.AuditRecord> GetAllAudits()
        {
            return _context.Auditvsam.Select(MapToDomainModel).ToList();
        }

        private static AuditsrvDomainModels.AuditRecord MapToDomainModel(Auditvsam entity)
        {
            return new AuditsrvDomainModels.AuditRecord
            {
                AuditId = entity.AuditKey,
                AuditDate = DateTime.Parse(entity.AuditTs),
                AuditorName = entity.AuditUser,
                AuditAmount = 0, // Placeholder as no mapping exists for this field
                Status = Enum.TryParse(entity.AuditAction, out AuditsrvDomainModels.AuditStatus status) ? status : AuditsrvDomainModels.AuditStatus.Pending,
                Details = new AuditsrvDomainModels.AuditDetails
                {
                    Department = "", // Placeholder as no mapping exists for this field
                    Description = entity.AuditDetail,
                    TotalAmount = 0 // Placeholder as no mapping exists for this field
                }
            };
        }

        private static Auditvsam MapToEntity(AuditsrvDomainModels.AuditRecord domainModel)
        {
            return new Auditvsam
            {
                AuditKey = domainModel.AuditId,
                AuditAcctNumber = decimal.Parse(domainModel.AuditId),
                AuditAction = domainModel.Status.ToString(),
                AuditUser = domainModel.AuditorName,
                AuditTs = domainModel.AuditDate.ToString("o"),
                AuditDetail = domainModel.Details.Description
            };
        }
    }
}