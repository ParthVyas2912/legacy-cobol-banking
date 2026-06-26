using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AUDITSRV.Application.Interfaces;
using AUDITSRV.Domain;

namespace AUDITSRV.Infrastructure
{
    public class AuditsrvDbContext : DbContext
    {
        public DbSet<Auditvsam> Auditvsam { get; set; }

        public AuditsrvDbContext(DbContextOptions<AuditsrvDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Auditvsam>(entity =>
            {
                entity.ToTable("auditvsam", "public");

                entity.HasKey(e => e.AuditKey);

                entity.Property(e => e.AuditKey)
                    .HasColumnName("audit_key")
                    .HasMaxLength(20)
                    .IsRequired(); // COBOL: AUDIT-KEY PIC X(20)

                entity.Property(e => e.AuditAcctNumber)
                    .HasColumnName("audit_acct_number")
                    .HasColumnType("decimal(10, 0)") // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.AuditAction)
                    .HasColumnName("audit_action")
                    .HasMaxLength(16)
                    .IsRequired(); // COBOL: AUDIT-ACTION PIC X(16)

                entity.Property(e => e.AuditUser)
                    .HasColumnName("audit_user")
                    .HasMaxLength(8)
                    .IsRequired(); // COBOL: AUDIT-USER PIC X(08)

                entity.Property(e => e.AuditTs)
                    .HasColumnName("audit_ts")
                    .HasColumnType("timestamptz")
                    .IsRequired(); // COBOL: AUDIT-TS PIC X(26)

                entity.Property(e => e.AuditDetail)
                    .HasColumnName("audit_detail")
                    .HasMaxLength(120)
                    .IsRequired(); // COBOL: AUDIT-DETAIL PIC X(120)
            });
        }
    }

    public class Auditvsam
    {
        [Key]
        [MaxLength(20)] // COBOL: AUDIT-KEY PIC X(20)
        public string AuditKey { get; set; }

        [Required]
        [Precision(10, 0)] // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)
        public string AuditAcctNumber { get; set; }

        [Required]
        [MaxLength(16)] // COBOL: AUDIT-ACTION PIC X(16)
        public string AuditAction { get; set; }

        [Required]
        [MaxLength(8)] // COBOL: AUDIT-USER PIC X(08)
        public string AuditUser { get; set; }

        [Required]
        public DateTime AuditTs { get; set; } // COBOL: AUDIT-TS PIC X(26)

        [Required]
        [MaxLength(120)] // COBOL: AUDIT-DETAIL PIC X(120)
        public string AuditDetail { get; set; }
    }

    public class AuditRepository : IAuditRepository
    {
        private readonly AuditsrvDbContext _context;

        public AuditRepository(AuditsrvDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public AuditsrvDomainModels.AuditRecord ReadAuditRecord(string recordId)
        {
            var entity = _context.Auditvsam.Find(recordId);
            if (entity == null)
                return null;

            return MapToDomainModel(entity);
        }

        public void WriteAuditRecord(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            if (auditRecord == null) throw new ArgumentNullException(nameof(auditRecord));

            var entity = MapToEntity(auditRecord);
            _context.Auditvsam.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateAuditRecord(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            if (auditRecord == null) throw new ArgumentNullException(nameof(auditRecord));

            var entity = _context.Auditvsam.Find(auditRecord.RecordId);
            if (entity == null) throw new KeyNotFoundException("Audit record not found.");

            entity.AuditAcctNumber = auditRecord.UserId;
            entity.AuditAction = auditRecord.Action;
            entity.AuditUser = auditRecord.UserId;
            entity.AuditTs = DateTime.ParseExact(auditRecord.AuditDate.ToString("D8") + auditRecord.AuditTime.ToString("D6"), "yyyyMMddHHmmss", null);
            entity.AuditDetail = auditRecord.Description;

            _context.Auditvsam.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteAuditRecord(string recordId)
        {
            var entity = _context.Auditvsam.Find(recordId);
            if (entity == null) throw new KeyNotFoundException("Audit record not found.");

            _context.Auditvsam.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<AuditsrvDomainModels.AuditRecord> GetAllAuditRecords()
        {
            var entities = _context.Auditvsam;
            var domainModels = new List<AuditsrvDomainModels.AuditRecord>();

            foreach (var entity in entities)
            {
                domainModels.Add(MapToDomainModel(entity));
            }

            return domainModels;
        }

        private AuditsrvDomainModels.AuditRecord MapToDomainModel(Auditvsam entity)
        {
            return new AuditsrvDomainModels.AuditRecord
            {
                RecordId = entity.AuditKey,
                UserId = entity.AuditUser,
                Action = entity.AuditAction,
                Description = entity.AuditDetail,
                AuditDate = int.Parse(entity.AuditTs.ToString("yyyyMMdd")),
                AuditTime = int.Parse(entity.AuditTs.ToString("HHmmss"))
            };
        }

        private Auditvsam MapToEntity(AuditsrvDomainModels.AuditRecord domainModel)
        {
            return new Auditvsam
            {
                AuditKey = domainModel.RecordId,
                AuditAcctNumber = domainModel.UserId,
                AuditAction = domainModel.Action,
                AuditUser = domainModel.UserId,
                AuditTs = DateTime.ParseExact(domainModel.AuditDate.ToString("D8") + domainModel.AuditTime.ToString("D6"), "yyyyMMddHHmmss", null),
                AuditDetail = domainModel.Description
            };
        }
    }
}