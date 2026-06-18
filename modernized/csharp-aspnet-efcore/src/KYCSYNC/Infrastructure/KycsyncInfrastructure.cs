using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KYCSYNC.Application.Interfaces;
using KYCSYNC.Domain;

namespace KYCSYNC.Infrastructure
{
    public class KycsyncDbContext : DbContext
    {
        public KycsyncDbContext(DbContextOptions<KycsyncDbContext> options) : base(options)
        {
        }

        public DbSet<KycAudit> KycAudits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new KycAuditEntityConfiguration());
        }
    }

    public class KycAudit
    {
        public string CustId { get; set; } // COBOL: CUST_ID PIC X(10)
        public DateTime? CustKycStatus { get; set; } // COBOL: CUST_KYC_STATUS PIC X(26) (TIMESTAMPTZ in DB)
        public string CustRiskScore { get; set; } // COBOL: CUST_RISK_SCORE PIC X(10)
    }

    public class KycAuditEntityConfiguration : IEntityTypeConfiguration<KycAudit>
    {
        public void Configure(EntityTypeBuilder<KycAudit> builder)
        {
            builder.ToTable("kyc_audit", "public");

            builder.Property(e => e.CustId)
                .HasColumnName("cust_id")
                .HasColumnType("VARCHAR")
                .HasMaxLength(10); // COBOL: CUST_ID PIC X(10)

            builder.Property(e => e.CustKycStatus)
                .HasColumnName("cust_kyc_status")
                .HasColumnType("TIMESTAMPTZ"); // COBOL: CUST_KYC_STATUS PIC X(26)

            builder.Property(e => e.CustRiskScore)
                .HasColumnName("cust_risk_score")
                .HasColumnType("VARCHAR")
                .HasMaxLength(10); // COBOL: CUST_RISK_SCORE PIC X(10)
        }
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly KycsyncDbContext _context;

        public CustomerRepository(KycsyncDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KycsyncDomainModels.CustomerRecord>> ReadAllCustomersAsync()
        {
            // This method should be implemented to map from the database entity to the domain model.
            // Since the database schema and domain model are not fully aligned, this requires a mapping layer.
            // For now, this is a placeholder for the actual implementation.
            return await Task.FromResult(new List<KycsyncDomainModels.CustomerRecord>());
        }

        public async Task WriteCustomerAsync(KycsyncDomainModels.CustomerRecord customerRecord)
        {
            // This method should be implemented to map from the domain model to the database entity.
            // Since the database schema and domain model are not fully aligned, this requires a mapping layer.
            // For now, this is a placeholder for the actual implementation.
            await Task.CompletedTask;
        }
    }

    public class KycStatusService : IKycStatusService
    {
        private readonly KycsyncDbContext _context;

        public KycStatusService(KycsyncDbContext context)
        {
            _context = context;
        }

        public async Task<KycsyncDomainModels.KycStatus.KycStatusEnum> GetKycStatusAsync(string customerId)
        {
            var kycAudit = await _context.KycAudits
                .FirstOrDefaultAsync(a => a.CustId == customerId);

            if (kycAudit == null || string.IsNullOrEmpty(kycAudit.CustRiskScore))
            {
                return KycsyncDomainModels.KycStatus.KycStatusEnum.NotVerified;
            }

            return kycAudit.CustRiskScore switch
            {
                "Y" => KycsyncDomainModels.KycStatus.KycStatusEnum.Verified,
                "P" => KycsyncDomainModels.KycStatus.KycStatusEnum.Pending,
                _ => KycsyncDomainModels.KycStatus.KycStatusEnum.NotVerified
            };
        }

        public async Task UpdateKycStatusAsync(string customerId, KycsyncDomainModels.KycStatus.KycStatusEnum status)
        {
            var kycAudit = await _context.KycAudits
                .FirstOrDefaultAsync(a => a.CustId == customerId);

            if (kycAudit != null)
            {
                kycAudit.CustRiskScore = status switch
                {
                    KycsyncDomainModels.KycStatus.KycStatusEnum.Verified => "Y",
                    KycsyncDomainModels.KycStatus.KycStatusEnum.Pending => "P",
                    _ => "N"
                };

                await _context.SaveChangesAsync();
            }
            else
            {
                kycAudit = new KycAudit
                {
                    CustId = customerId,
                    CustRiskScore = status switch
                    {
                        KycsyncDomainModels.KycStatus.KycStatusEnum.Verified => "Y",
                        KycsyncDomainModels.KycStatus.KycStatusEnum.Pending => "P",
                        _ => "N"
                    }
                };

                _context.KycAudits.Add(kycAudit);
                await _context.SaveChangesAsync();
            }
        }
    }
}