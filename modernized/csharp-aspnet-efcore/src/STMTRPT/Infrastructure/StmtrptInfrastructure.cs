using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using STMTRPT.Application.Interfaces;
using STMTRPT.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace STMTRPT.Infrastructure
{
    public class StmtrptDbContext : DbContext
    {
        public DbSet<Stmtout> Stmtout { get; set; }

        public StmtrptDbContext(DbContextOptions<StmtrptDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stmtout>(entity =>
            {
                entity.ToTable("stmtout", "public");

                entity.Property(e => e.StmtAcctNum)
                    .HasColumnName("stmt_acct_num")
                    .HasMaxLength(10) // Corrected: Matches PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.StmtDescription)
                    .HasColumnName("stmt_description")
                    .HasMaxLength(40) // Corrected: Matches PIC X(40)
                    .IsRequired();

                entity.Property(e => e.StmtAmount)
                    .HasColumnName("stmt_amount")
                    .HasPrecision(13, 2) // Corrected: Matches PIC -Z(11)9.99
                    .IsRequired();

                entity.Property(e => e.StmtBalance)
                    .HasColumnName("stmt_balance")
                    .HasPrecision(15, 2) // Corrected: Matches PIC -Z(13)9.99
                    .IsRequired();
            });
        }
    }

    public class Stmtout
    {
        public string StmtAcctNum { get; set; } = string.Empty; // Corrected: Matches PIC 9(10)
        public string StmtDescription { get; set; } = string.Empty; // Corrected: Matches PIC X(40)
        public decimal StmtAmount { get; set; } // Corrected: Matches PIC -Z(11)9.99
        public decimal StmtBalance { get; set; } // Corrected: Matches PIC -Z(13)9.99
    }

    public class StatementReportRepository : IStatementReportRepository
    {
        private readonly StmtrptDbContext _context;

        public StatementReportRepository(StmtrptDbContext context)
        {
            _context = context;
        }

        public async Task<StmtrptDomainModels.StatementReport?> GetByAccountNumberAsync(string accountNumber)
        {
            var stmtout = await _context.Stmtout
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StmtAcctNum == accountNumber);

            if (stmtout == null)
            {
                return null;
            }

            return new StmtrptDomainModels.StatementReport
            {
                AccountNumber = stmtout.StmtAcctNum,
                AccountHolderName = stmtout.StmtDescription,
                CurrentBalance = stmtout.StmtBalance,
                AvailableBalance = stmtout.StmtAmount
            };
        }

        public async Task AddAsync(StmtrptDomainModels.StatementReport report)
        {
            var stmtout = new Stmtout
            {
                StmtAcctNum = report.AccountNumber,
                StmtDescription = report.AccountHolderName,
                StmtBalance = report.CurrentBalance,
                StmtAmount = report.AvailableBalance
            };

            await _context.Stmtout.AddAsync(stmtout);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StmtrptDomainModels.StatementReport report)
        {
            var stmtout = await _context.Stmtout
                .FirstOrDefaultAsync(s => s.StmtAcctNum == report.AccountNumber);

            if (stmtout == null)
            {
                return;
            }

            stmtout.StmtDescription = report.AccountHolderName;
            stmtout.StmtBalance = report.CurrentBalance;
            stmtout.StmtAmount = report.AvailableBalance;

            _context.Stmtout.Update(stmtout);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StmtrptDomainModels.StatementReport>> GetAllAsync()
        {
            var stmtouts = await _context.Stmtout.AsNoTracking().ToListAsync();

            return stmtouts.Select(stmtout => new StmtrptDomainModels.StatementReport
            {
                AccountNumber = stmtout.StmtAcctNum,
                AccountHolderName = stmtout.StmtDescription,
                CurrentBalance = stmtout.StmtBalance,
                AvailableBalance = stmtout.StmtAmount
            }).ToList();
        }
    }

    public static class StmtrptInfrastructureExtensions
    {
        public static void AddStmtrptInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<StmtrptDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IStatementReportRepository, StatementReportRepository>();
        }
    }
}