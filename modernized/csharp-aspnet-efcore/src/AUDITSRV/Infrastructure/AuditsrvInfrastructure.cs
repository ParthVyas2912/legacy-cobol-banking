using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AUDITSRV.Application.Interfaces;
using AUDITSRV.Domain;

namespace AUDITSRV.Infrastructure
{
    public class AuditsrvDbContext : DbContext
    {
        public AuditsrvDbContext(DbContextOptions<AuditsrvDbContext> options) : base(options) { }

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
                    .HasColumnType("decimal(10,0)"); // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)

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
        public string AuditKey { get; set; } = string.Empty; // COBOL: AUDIT-KEY PIC X(20)
        public decimal AuditAcctNumber { get; set; } // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)
        public string AuditAction { get; set; } = string.Empty; // COBOL: AUDIT-ACTION PIC X(16)
        public string AuditUser { get; set; } = string.Empty; // COBOL: AUDIT-USER PIC X(08)
        public string AuditTs { get; set; } = string.Empty; // COBOL: AUDIT-TS PIC X(26)
        public string AuditDetail { get; set; } = string.Empty; // COBOL: AUDIT-DETAIL PIC X(120)
    }

    public class AuditRepository : IAuditRepository
    {
        private readonly AuditsrvDbContext _context;

        public AuditRepository(AuditsrvDbContext context)
        {
            _context = context;
        }

        public async Task<AuditsrvDomainModels.AuditRecord?> GetAuditRecordByIdAsync(string auditId)
        {
            var entity = await _context.Auditvsam.AsNoTracking()
                .SingleOrDefaultAsync(a => a.AuditKey == auditId);

            if (entity == null) return null;

            return new AuditsrvDomainModels.AuditRecord
            {
                AuditId = entity.AuditKey,
                UserId = entity.AuditUser,
                ActionDate = entity.AuditTs.Substring(0, 10), // Assuming date is in the first 10 characters
                ActionTime = entity.AuditTs.Substring(11, 8), // Assuming time is in the next 8 characters
                ActionDescription = entity.AuditAction,
                Comments = entity.AuditDetail
            };
        }

        public async Task<List<AuditsrvDomainModels.AuditRecord>> GetAllAuditRecordsAsync()
        {
            var entities = await _context.Auditvsam.AsNoTracking().ToListAsync();

            return entities.Select(entity => new AuditsrvDomainModels.AuditRecord
            {
                AuditId = entity.AuditKey,
                UserId = entity.AuditUser,
                ActionDate = entity.AuditTs.Substring(0, 10), // Assuming date is in the first 10 characters
                ActionTime = entity.AuditTs.Substring(11, 8), // Assuming time is in the next 8 characters
                ActionDescription = entity.AuditAction,
                Comments = entity.AuditDetail
            }).ToList();
        }

        public async Task AddAuditRecordAsync(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            var entity = new Auditvsam
            {
                AuditKey = auditRecord.AuditId,
                AuditUser = auditRecord.UserId,
                AuditTs = $"{auditRecord.ActionDate} {auditRecord.ActionTime}",
                AuditAction = auditRecord.ActionDescription,
                AuditDetail = auditRecord.Comments
            };

            _context.Auditvsam.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuditRecordAsync(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            var entity = await _context.Auditvsam.SingleOrDefaultAsync(a => a.AuditKey == auditRecord.AuditId);

            if (entity == null) return;

            entity.AuditUser = auditRecord.UserId;
            entity.AuditTs = $"{auditRecord.ActionDate} {auditRecord.ActionTime}";
            entity.AuditAction = auditRecord.ActionDescription;
            entity.AuditDetail = auditRecord.Comments;

            _context.Auditvsam.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuditRecordAsync(string auditId)
        {
            var entity = await _context.Auditvsam.SingleOrDefaultAsync(a => a.AuditKey == auditId);

            if (entity == null) return;

            _context.Auditvsam.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public class AuditSummaryRepository : IAuditSummaryRepository
    {
        private readonly AuditsrvDbContext _context;

        public AuditSummaryRepository(AuditsrvDbContext context)
        {
            _context = context;
        }

        public async Task<AuditsrvDomainModels.AuditSummary?> GetAuditSummaryByIdAsync(string summaryId)
        {
            // Implementation for summaries (if applicable)
            return null;
        }

        public async Task<List<AuditsrvDomainModels.AuditSummary>> GetAllAuditSummariesAsync()
        {
            // Implementation for summaries (if applicable)
            return new List<AuditsrvDomainModels.AuditSummary>();
        }

        public async Task AddAuditSummaryAsync(AuditsrvDomainModels.AuditSummary auditSummary)
        {
            // Implementation for summaries (if applicable)
        }

        public async Task UpdateAuditSummaryAsync(AuditsrvDomainModels.AuditSummary auditSummary)
        {
            // Implementation for summaries (if applicable)
        }

        public async Task DeleteAuditSummaryAsync(string summaryId)
        {
            // Implementation for summaries (if applicable)
        }
    }
}