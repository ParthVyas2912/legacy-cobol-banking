using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BCHEOD.Domain;
using BCHEOD.Application.Interfaces;

namespace BCHEOD.Infrastructure
{
    public class BcheodDbContext : DbContext
    {
        public DbSet<BcheodDomainModels.EodRun> EodRuns { get; set; }

        public BcheodDbContext(DbContextOptions<BcheodDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BcheodDomainModels.EodRun>(entity =>
            {
                entity.ToTable("eod_runs", "public");

                entity.Property(e => e.RunId)
                    .HasColumnName("run_id")
                    .HasMaxLength(10)
                    .IsRequired(); // COBOL: RUN-ID PIC X(10)

                entity.Property(e => e.RunDate)
                    .HasColumnName("run_date")
                    .HasColumnType("date")
                    .IsRequired(); // COBOL: RUN-DATE PIC 9(8)

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(1)
                    .IsRequired(); // COBOL: STATUS PIC X(1)

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(15, 2)
                    .IsRequired(); // COBOL: AMOUNT PIC S9(13)V99 COMP-3
            });
        }
    }

    public class EodRunsRepository : IEodRunsRepository
    {
        private readonly BcheodDbContext _dbContext;

        public EodRunsRepository(BcheodDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public BcheodDomainModels.EodRun GetEodRunById(string runId)
        {
            if (string.IsNullOrWhiteSpace(runId))
                throw new ArgumentException("Run ID cannot be null or empty.", nameof(runId));

            return _dbContext.EodRuns.SingleOrDefault(e => e.RunId == runId);
        }

        public void UpdateEodRun(BcheodDomainModels.EodRun eodRun)
        {
            if (eodRun == null)
                throw new ArgumentNullException(nameof(eodRun));

            var existingRun = _dbContext.EodRuns.SingleOrDefault(e => e.RunId == eodRun.RunId);
            if (existingRun == null)
                throw new InvalidOperationException($"EOD run with ID {eodRun.RunId} does not exist.");

            existingRun.RunDate = eodRun.RunDate;
            existingRun.Status = eodRun.Status;
            existingRun.Amount = eodRun.Amount;

            _dbContext.SaveChanges();
        }

        public void InsertEodRun(BcheodDomainModels.EodRun eodRun)
        {
            if (eodRun == null)
                throw new ArgumentNullException(nameof(eodRun));

            _dbContext.EodRuns.Add(eodRun);
            _dbContext.SaveChanges();
        }
    }

    public class BcheodExternalService : IBcheodExternalService
    {
        public void CallIntCalcService(object parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            // Simulate external service call to INTCALC
            Console.WriteLine("Calling INTCALC service with parameters: " + parameters);
        }

        public void CallTxnPostService(object parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            // Simulate external service call to TXNPOST
            Console.WriteLine("Calling TXNPOST service with parameters: " + parameters);
        }
    }
}