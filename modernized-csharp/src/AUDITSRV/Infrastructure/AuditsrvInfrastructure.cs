using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AUDITSRV.Domain;
using AUDITSRV.Application.Interfaces;

namespace AUDITSRV.Infrastructure
{
    /// <summary>
    /// Entity Framework DbContext for the AUDITSRV program.
    /// </summary>
    public class AuditsrvDbContext : DbContext
    {
        public DbSet<AuditFile> AuditFiles { get; set; }

        public AuditsrvDbContext(DbContextOptions<AuditsrvDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuditFile>(entity =>
            {
                entity.ToTable("auditvsam", "public");

                entity.HasKey(e => e.AuditKey);
                entity.Property(e => e.AuditKey)
                    .HasColumnName("audit_key")
                    .HasMaxLength(20); // COBOL: AUDIT-KEY PIC X(20)

                entity.Property(e => e.AuditAcctNumber)
                    .HasColumnName("audit_acct_number")
                    .HasColumnType("bigint"); // COBOL: AUDIT-ACCT-NUMBER PIC 9(18)

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

                entity.HasIndex(e => e.AuditAcctNumber)
                    .HasDatabaseName("idx_auditvsam_audit_acct_number");
            });
        }
    }

    /// <summary>
    /// Repository implementation for the AUDIT-FILE.
    /// </summary>
    public class AuditFileRepository : IAuditFileRepository
    {
        private readonly AuditsrvDbContext _dbContext;

        public AuditFileRepository(AuditsrvDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Reads an audit record from the AUDIT-FILE.
        /// </summary>
        /// <param name="auditKey">The primary key of the audit record to read.</param>
        /// <returns>The audit record if found, otherwise null.</returns>
        public async Task<AuditFile> ReadAuditRecordAsync(string auditKey)
        {
            if (string.IsNullOrWhiteSpace(auditKey))
            {
                throw new ArgumentException("Audit key cannot be null or empty.", nameof(auditKey));
            }

            return await _dbContext.AuditFiles
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AuditKey == auditKey);
        }
    }

    /// <summary>
    /// Implementation of CICS service operations used in the AUDITSRV program.
    /// </summary>
    public class CicsService : ICicsService
    {
        /// <summary>
        /// Writes a response back to the CICS terminal.
        /// </summary>
        /// <param name="response">The response message to write.</param>
        public Task WriteResponseAsync(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                throw new ArgumentException("Response cannot be null or empty.", nameof(response));
            }

            // Simulate writing response to CICS terminal.
            Console.WriteLine($"CICS Response: {response}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Returns control to the CICS environment.
        /// </summary>
        public Task ReturnControlAsync()
        {
            // Simulate returning control to CICS.
            Console.WriteLine("Returning control to CICS environment.");
            return Task.CompletedTask;
        }
    }
}