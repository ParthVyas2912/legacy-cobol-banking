using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BCHEOD.Domain;
using BCHEOD.Application.Interfaces;

namespace BCHEOD.Infrastructure
{
    public class BcheodDbContext : DbContext
    {
        public BcheodDbContext(DbContextOptions<BcheodDbContext> options) : base(options)
        {
        }

        public DbSet<EodRuns> EodRuns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EodRunsConfiguration());
        }
    }

    public class EodRuns
    {
        public DateTime StartedAt { get; set; } // COBOL: STARTED_AT TIMESTAMPTZ
        public DateTime Status { get; set; } // COBOL: STATUS TIMESTAMPTZ
        public DateTime EndedAt { get; set; } // COBOL: ENDED_AT TIMESTAMPTZ
        public string WsStatusCode { get; set; } // COBOL: WS_STATUS_CODE PIC X(10)
    }

    public class EodRunsConfiguration : IEntityTypeConfiguration<EodRuns>
    {
        public void Configure(EntityTypeBuilder<EodRuns> builder)
        {
            builder.ToTable("eod_runs", "public");

            builder.Property(e => e.StartedAt)
                .HasColumnName("started_at")
                .HasColumnType("timestamptz")
                .IsRequired(); // COBOL: STARTED_AT TIMESTAMPTZ

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasColumnType("timestamptz")
                .IsRequired(); // COBOL: STATUS TIMESTAMPTZ

            builder.Property(e => e.EndedAt)
                .HasColumnName("ended_at")
                .HasColumnType("timestamptz")
                .IsRequired(); // COBOL: ENDED_AT TIMESTAMPTZ

            builder.Property(e => e.WsStatusCode)
                .HasColumnName("ws_status_code")
                .HasMaxLength(10)
                .IsRequired(); // COBOL: WS_STATUS_CODE PIC X(10)
        }
    }

    public class InputFileRepository : IInputFileRepository
    {
        private readonly string _filePath;
        private StreamReader _reader;

        public InputFileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<BcheodDomainModels.ExampleRecord> ReadNextRecordAsync()
        {
            if (_reader == null)
            {
                _reader = new StreamReader(new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.UTF8);
            }

            string line = await _reader.ReadLineAsync();
            if (line == null)
            {
                return null; // End of file
            }

            // Assuming the file is CSV and fields are separated by commas
            var fields = line.Split(',');

            // Map fields to ExampleRecord properties
            var record = new BcheodDomainModels.ExampleRecord
            {
                Field1 = fields[0],
                Field2 = fields[1],
                Field3 = int.Parse(fields[2]),
                Field4 = DateTime.Parse(fields[3])
            };

            return record;
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }
    }

    public class OutputFileRepository : IOutputFileRepository
    {
        private readonly string _filePath;
        private StreamWriter _writer;

        public OutputFileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task WriteRecordAsync(BcheodDomainModels.ExampleRecord record)
        {
            if (_writer == null)
            {
                _writer = new StreamWriter(new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.None), Encoding.UTF8);
            }

            // Assuming the file is CSV and fields are separated by commas
            var line = $"{record.Field1},{record.Field2},{record.Field3},{record.Field4:O}";
            await _writer.WriteLineAsync(line);
        }

        public void Dispose()
        {
            _writer?.Dispose();
        }
    }

    public class CicsService : ICicsService
    {
        public async Task<string> PerformTransactionAsync(string transactionData)
        {
            // Simulate a CICS transaction by calling an external service or performing some operation
            // For example, this could be a call to an HTTP API or a database stored procedure
            // Here, we simulate a delay and return a mock response
            await Task.Delay(100); // Simulate network latency or processing time

            // Mock response for the transaction
            string response = $"Transaction successful for data: {transactionData}";
            return response;
        }
    }
}