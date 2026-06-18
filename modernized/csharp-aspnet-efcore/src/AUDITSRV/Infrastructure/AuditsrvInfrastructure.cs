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
                    .HasColumnType("timestamptz"); // COBOL: AUDIT-TS PIC X(26)

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

        public async Task<IEnumerable<AuditsrvDomainModels.AuditRecord>> ReadAllAuditRecordsAsync()
        {
            return await _context.Auditvsam.Select(a => new AuditsrvDomainModels.AuditRecord
            {
                AuditId = a.AuditKey,
                UserId = a.AuditUser,
                ActionType = a.AuditAction,
                ActionDescription = a.AuditDetail,
                ActionDate = a.AuditTs.Substring(0, 10),
                ActionTime = a.AuditTs.Substring(11, 8),
                AdditionalInfo = a.AuditDetail
            }).ToListAsync();
        }

        public async Task WriteAuditRecordAsync(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            var entity = new Auditvsam
            {
                AuditKey = auditRecord.AuditId,
                AuditAcctNumber = decimal.Parse(auditRecord.ObjectId), // Ensure proper conversion to decimal
                AuditAction = auditRecord.ActionType,
                AuditUser = auditRecord.UserId,
                AuditTs = $"{auditRecord.ActionDate}T{auditRecord.ActionTime}",
                AuditDetail = auditRecord.AdditionalInfo
            };

            _context.Auditvsam.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<AuditsrvDomainModels.AuditSummary> ReadAuditSummaryAsync()
        {
            var totalRecords = await _context.Auditvsam.CountAsync();
            var totalAmount = await _context.Auditvsam.SumAsync(a => (decimal?)a.AuditAcctNumber) ?? 0;

            return new AuditsrvDomainModels.AuditSummary
            {
                TotalRecords = totalRecords,
                TotalAmount = totalAmount
            };
        }

        public async Task WriteAuditSummaryAsync(AuditsrvDomainModels.AuditSummary auditSummary)
        {
            // No implementation provided in the original specification
            await Task.CompletedTask;
        }
    }

    public class CicsService : ICicsService
    {
        public async Task PerformCicsOperationAsync(string operationCode, object requestData)
        {
            // No implementation provided in the original specification
            await Task.CompletedTask;
        }
    }
}