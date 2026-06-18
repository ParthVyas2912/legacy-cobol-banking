using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Auditvsam>(entity =>
            {
                entity.ToTable("auditvsam", "public");

                entity.HasKey(e => e.AuditKey);

                entity.Property(e => e.AuditKey)
                    .HasColumnName("audit_key")
                    .HasMaxLength(20); // COBOL: AUDIT-KEY PIC X(20)

                entity.Property(e => e.AuditAcctNumber)
                    .HasColumnName("audit_acct_number")
                    .HasColumnType("NUMERIC(10, 0)"); // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.AuditAction)
                    .HasColumnName("audit_action")
                    .HasMaxLength(16); // COBOL: AUDIT-ACTION PIC X(16)

                entity.Property(e => e.AuditUser)
                    .HasColumnName("audit_user")
                    .HasMaxLength(8); // COBOL: AUDIT-USER PIC X(08)

                entity.Property(e => e.AuditTs)
                    .HasColumnName("audit_ts")
                    .HasColumnType("TIMESTAMPTZ"); // COBOL: AUDIT-TS PIC X(26)

                entity.Property(e => e.AuditDetail)
                    .HasColumnName("audit_detail")
                    .HasMaxLength(120); // COBOL: AUDIT-DETAIL PIC X(120)
            });
        }
    }

    [Table("auditvsam", Schema = "public")]
    public class Auditvsam
    {
        [Key]
        [Column("audit_key")]
        [MaxLength(20)] // COBOL: AUDIT-KEY PIC X(20)
        public string AuditKey { get; set; }

        [Column("audit_acct_number", TypeName = "NUMERIC(10, 0)")] // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)
        public string AuditAcctNumber { get; set; }

        [Column("audit_action")]
        [MaxLength(16)] // COBOL: AUDIT-ACTION PIC X(16)
        public string AuditAction { get; set; }

        [Column("audit_user")]
        [MaxLength(8)] // COBOL: AUDIT-USER PIC X(08)
        public string AuditUser { get; set; }

        [Column("audit_ts", TypeName = "TIMESTAMPTZ")] // COBOL: AUDIT-TS PIC X(26)
        public DateTime AuditTs { get; set; }

        [Column("audit_detail")]
        [MaxLength(120)] // COBOL: AUDIT-DETAIL PIC X(120)
        public string AuditDetail { get; set; }
    }

    public class AuditRepository : IAuditRepository
    {
        private readonly AuditsrvDbContext _dbContext;

        public AuditRepository(AuditsrvDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AuditsrvDomainModels.AuditRecord> ReadNextRecordAsync()
        {
            var entity = await _dbContext.Auditvsam.FirstOrDefaultAsync();
            if (entity == null) return null;

            return new AuditsrvDomainModels.AuditRecord
            {
                RecordId = entity.AuditKey,
                AuditType = entity.AuditAcctNumber,
                Amount = 0, // Placeholder as no mapping exists for Amount
                Status = AuditsrvDomainModels.AuditStatus.Pending, // Placeholder as no mapping exists for Status
                Details = new AuditsrvDomainModels.AuditDetails
                {
                    Description = entity.AuditDetail,
                    CreatedBy = entity.AuditUser,
                    CreatedDate = entity.AuditTs.Date,
                    UpdatedBy = entity.AuditUser,
                    UpdatedDate = entity.AuditTs.Date
                }
            };
        }

        public async Task WriteRecordAsync(AuditsrvDomainModels.AuditRecord record)
        {
            var entity = new Auditvsam
            {
                AuditKey = record.RecordId,
                AuditAcctNumber = record.AuditType,
                AuditAction = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), // Placeholder as no mapping exists for AuditAction
                AuditUser = record.Details.CreatedBy,
                AuditTs = record.Details.CreatedDate,
                AuditDetail = record.Details.Description
            };

            _dbContext.Auditvsam.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }

    public class CicsService : ICicsService
    {
        public async Task<bool> SendAuditNotificationAsync(AuditsrvDomainModels.AuditRecord record)
        {
            // Placeholder for actual CICS communication logic
            await Task.Delay(100); // Simulate async operation
            return true; // Assume success for now
        }
    }
}