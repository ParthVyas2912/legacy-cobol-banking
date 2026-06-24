using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AUDITSRV.Domain;
using AUDITSRV.Application.Interfaces;

namespace AUDITSRV.Infrastructure
{
    public class AuditsrvDbContext : DbContext
    {
        public DbSet<AuditsrvDomainModels.AuditRecord> AuditRecords { get; set; }
        public DbSet<AuditsrvDomainModels.AuditDetail> AuditDetails { get; set; }

        public AuditsrvDbContext(DbContextOptions<AuditsrvDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditsrvDomainModels.AuditRecord>(entity =>
            {
                entity.ToTable("auditvsam", "public");

                entity.HasKey(e => e.RecordId);
                entity.Property(e => e.RecordId)
                    .HasColumnName("audit_key")
                    .HasMaxLength(20); // COBOL: AUDIT-KEY PIC X(20)

                entity.Property(e => e.UserName)
                    .HasColumnName("audit_user")
                    .HasMaxLength(8); // COBOL: AUDIT-USER PIC X(08)

                entity.Property(e => e.ActionCode)
                    .HasColumnName("audit_action")
                    .HasMaxLength(16); // COBOL: AUDIT-ACTION PIC X(16)

                entity.Property(e => e.Amount)
                    .HasColumnName("audit_acct_number")
                    .HasPrecision(10, 0); // Corrected precision to match COBOL: AUDIT-ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.Timestamp)
                    .HasColumnName("audit_ts")
                    .HasMaxLength(26); // COBOL: AUDIT-TS PIC X(26)

                entity.Property(e => e.Status)
                    .HasColumnName("audit_status")
                    .HasConversion<int>(); // Assuming AuditStatus is an enum, added conversion to int

                entity.Property(e => e.Details)
                    .HasColumnName("audit_detail")
                    .HasMaxLength(120); // COBOL: AUDIT-DETAIL PIC X(120)
            });

            modelBuilder.Entity<AuditsrvDomainModels.AuditDetail>(entity =>
            {
                entity.ToTable("auditdetails", "public");

                // Assuming AuditDetail has properties that need to be mapped
                // Add mappings here as per the domain model and COBOL contract
            });
        }
    }
}