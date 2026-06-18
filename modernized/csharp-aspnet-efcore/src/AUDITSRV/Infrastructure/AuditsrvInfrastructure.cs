using System;
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

        public AuditsrvDbContext(DbContextOptions<AuditsrvDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auditvsam>(entity =>
            {
                entity.ToTable("auditvsam", "public");

                entity.HasKey(e => e.AuditKey)
                      .HasName("pk_audit_key");

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

        public async Task<IEnumerable<AuditsrvDomainModels.AuditRecord>> ReadAuditRecordsAsync()
        {
            var records = await _context.Auditvsam.ToListAsync();
            return records.Select(r => new AuditsrvDomainModels.AuditRecord
            {
                AuditId = r.AuditKey,
                UserId = r.AuditUser,
                ActionDate = r.AuditTs.ToString("yyyyMMdd"),
                ActionTime = r.AuditTs.ToString("HHmmss"),
                ActionDescription = r.AuditAction,
                IpAddress = null, // No mapping in COBOL
                HostName = null, // No mapping in COBOL
                Amount = 0, // No mapping in COBOL
                ActionType = Enum.TryParse<AuditsrvDomainModels.AuditActionType>(r.AuditAction, out var actionType) ? actionType : default,
                Status = AuditsrvDomainModels.AuditStatus.Completed // Default value
            });
        }

        public async Task WriteAuditRecordAsync(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            var entity = new Auditvsam
            {
                AuditKey = auditRecord.AuditId,
                AuditUser = auditRecord.UserId,
                AuditTs = DateTime.ParseExact($"{auditRecord.ActionDate}{auditRecord.ActionTime}", "yyyyMMddHHmmss", null),
                AuditAction = auditRecord.ActionDescription,
                AuditDetail = auditRecord.ActionDescription // Assuming ActionDescription maps to AuditDetail
            };

            _context.Auditvsam.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuditRecordAsync(AuditsrvDomainModels.AuditRecord auditRecord)
        {
            var entity = await _context.Auditvsam.FindAsync(auditRecord.AuditId);
            if (entity != null)
            {
                entity.AuditUser = auditRecord.UserId;
                entity.AuditTs = DateTime.ParseExact($"{auditRecord.ActionDate}{auditRecord.ActionTime}", "yyyyMMddHHmmss", null);
                entity.AuditAction = auditRecord.ActionDescription;
                entity.AuditDetail = auditRecord.ActionDescription; // Assuming ActionDescription maps to AuditDetail

                _context.Auditvsam.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAuditRecordAsync(string auditId)
        {
            var entity = await _context.Auditvsam.FindAsync(auditId);
            if (entity != null)
            {
                _context.Auditvsam.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class CicsService : ICicsService
    {
        public async Task CallCicsTransactionAsync(string transactionId, object requestPayload)
        {
            // Implementation for calling CICS transaction
            // This is a placeholder for actual CICS interaction logic
            await Task.CompletedTask;
        }
    }
}