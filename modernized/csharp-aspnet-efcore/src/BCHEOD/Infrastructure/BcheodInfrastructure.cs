using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BCHEOD.Domain;

namespace BCHEOD.Infrastructure
{
    /// <summary>
    /// Entity Framework DbContext for BCHEOD program.
    /// </summary>
    public class BcheodDbContext : DbContext
    {
        public DbSet<EodRuns> EodRuns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EodRuns>(entity =>
            {
                entity.ToTable("eod_runs", "public");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .IsRequired();

                entity.Property(e => e.StartedAt)
                    .HasColumnName("started_at")
                    .HasColumnType("timestamptz")
                    .IsRequired();
                // COBOL: STARTED_AT TIMESTAMPTZ

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(20)
                    .IsRequired();
                // COBOL: STATUS PIC X(20)

                entity.Property(e => e.EndedAt)
                    .HasColumnName("ended_at")
                    .HasColumnType("timestamptz")
                    .IsRequired();
                // COBOL: ENDED_AT TIMESTAMPTZ

                entity.Property(e => e.WsStatusCode)
                    .HasColumnName("ws_status_code")
                    .HasMaxLength(10)
                    .IsRequired();
                // COBOL: WS_STATUS_CODE PIC X(10)
            });
        }

        public BcheodDbContext(DbContextOptions<BcheodDbContext> options) : base(options)
        {
        }
    }

    /// <summary>
    /// Entity class for EOD_RUNS table.
    /// </summary>
    [Table("eod_runs", Schema = "public")]
    public class EodRuns
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("started_at", TypeName = "timestamptz")]
        public DateTime StartedAt { get; set; }
        // COBOL: STARTED_AT TIMESTAMPTZ

        [Column("status")]
        [MaxLength(20)]
        public string Status { get; set; }
        // COBOL: STATUS PIC X(20)

        [Column("ended_at", TypeName = "timestamptz")]
        public DateTime EndedAt { get; set; }
        // COBOL: ENDED_AT TIMESTAMPTZ

        [Column("ws_status_code")]
        [MaxLength(10)]
        public string WsStatusCode { get; set; }
        // COBOL: WS_STATUS_CODE PIC X(10)
    }

    /// <summary>
    /// Repository for BCHEOD file operations.
    /// </summary>
    public class BcheodFileRepository : BCHEOD.Application.Interfaces.IBcheodFileRepository
    {
        private const string InputFilePath = "input_file_path.txt";
        private const string OutputFilePath = "output_file_path.txt";

        public BcheodDomainModels.Header ReadHeader()
        {
            using (var reader = new StreamReader(InputFilePath))
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    throw new InvalidOperationException("Header record is missing in the input file.");
                }

                var fields = line.Split('|');
                return new BcheodDomainModels.Header
                {
                    RecordType = fields[0],
                    RunDate = DateTime.Parse(fields[1]),
                    SequenceNumber = int.Parse(fields[2])
                };
            }
        }

        public BcheodDomainModels.Detail[] ReadDetails()
        {
            var details = new System.Collections.Generic.List<BcheodDomainModels.Detail>();

            using (var reader = new StreamReader(InputFilePath))
            {
                // Skip the header line
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split('|');
                    details.Add(new BcheodDomainModels.Detail
                    {
                        RecordType = fields[0],
                        AccountNumber = fields[1],
                        Amount = decimal.Parse(fields[2]),
                        Description = fields[3]
                    });
                }
            }

            return details.ToArray();
        }

        public void WriteHeader(BcheodDomainModels.Header header)
        {
            using (var writer = new StreamWriter(OutputFilePath, append: false))
            {
                writer.WriteLine($"{header.RecordType}|{header.RunDate:yyyy-MM-dd}|{header.SequenceNumber}");
            }
        }

        public void WriteDetails(BcheodDomainModels.Detail[] details)
        {
            using (var writer = new StreamWriter(OutputFilePath, append: true))
            {
                foreach (var detail in details)
                {
                    writer.WriteLine($"{detail.RecordType}|{detail.AccountNumber}|{detail.Amount:F2}|{detail.Description}");
                }
            }
        }
    }

    /// <summary>
    /// Service for BCHEOD CICS operations.
    /// </summary>
    public class BcheodCicsService : BCHEOD.Application.Interfaces.IBcheodCicsService
    {
        public void CommitTransaction()
        {
            using (var dbContext = new BcheodDbContext(new DbContextOptions<BcheodDbContext>()))
            {
                dbContext.SaveChanges();
            }
        }

        public void RollbackTransaction()
        {
            // In EF Core, transactions are rolled back automatically if SaveChanges is not called.
            // This method is a placeholder for explicit rollback logic if needed.
        }

        public void LogError(string errorMessage)
        {
            var logFilePath = "cics_error_log.txt";
            using (var writer = new StreamWriter(logFilePath, append: true))
            {
                writer.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - ERROR: {errorMessage}");
            }
        }
    }
}