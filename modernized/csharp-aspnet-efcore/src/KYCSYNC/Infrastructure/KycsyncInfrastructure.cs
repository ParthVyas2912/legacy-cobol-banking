using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KYCSYNC.Domain;
using KYCSYNC.Application.Interfaces;

namespace KYCSYNC.Infrastructure
{
    public class KycsyncDbContext : DbContext
    {
        public KycsyncDbContext(DbContextOptions<KycsyncDbContext> options) : base(options) { }

        public DbSet<KycAudit> KycAudits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KycAudit>(entity =>
            {
                entity.ToTable("kyc_audit", "public");

                entity.Property(e => e.CustId)
                    .HasColumnName("cust_id")
                    .HasColumnType("TEXT")
                    .HasMaxLength(10); // COBOL: CUST_ID PIC X(10)

                entity.Property(e => e.CustKycStatus)
                    .HasColumnName("cust_kyc_status")
                    .HasColumnType("TEXT")
                    .HasMaxLength(30); // COBOL: CUST_KYC_STATUS PIC X(30)

                entity.Property(e => e.CustRiskScore)
                    .HasColumnName("cust_risk_score")
                    .HasColumnType("TEXT")
                    .HasMaxLength(30); // COBOL: CUST_RISK_SCORE PIC X(30)
            });
        }
    }

    public class KycAudit
    {
        public string CustId { get; set; } = string.Empty; // COBOL: CUST_ID PIC X(10)
        public string CustKycStatus { get; set; } = string.Empty; // COBOL: CUST_KYC_STATUS PIC X(30)
        public string CustRiskScore { get; set; } = string.Empty; // COBOL: CUST_RISK_SCORE PIC X(30)
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly KycsyncDbContext _dbContext;

        public CustomerRepository(KycsyncDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<KycsyncDomainModels.CustomerRecord?> GetCustomerByIdAsync(string customerId)
        {
            var customer = await _dbContext.KycAudits
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustId == customerId);

            if (customer == null) return null;

            return new KycsyncDomainModels.CustomerRecord
            {
                CustomerId = customer.CustId,
                Status = MapStatus(customer.CustRiskScore)
            };
        }

        public async Task<List<KycsyncDomainModels.CustomerRecord>> GetAllCustomersAsync()
        {
            var customers = await _dbContext.KycAudits
                .AsNoTracking()
                .ToListAsync();

            return customers.Select(c => new KycsyncDomainModels.CustomerRecord
            {
                CustomerId = c.CustId,
                Status = MapStatus(c.CustRiskScore)
            }).ToList();
        }

        public async Task AddCustomerAsync(KycsyncDomainModels.CustomerRecord customer)
        {
            var entity = new KycAudit
            {
                CustId = customer.CustomerId,
                CustRiskScore = customer.Status.ToString()
            };

            _dbContext.KycAudits.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(KycsyncDomainModels.CustomerRecord customer)
        {
            var entity = await _dbContext.KycAudits
                .FirstOrDefaultAsync(c => c.CustId == customer.CustomerId);

            if (entity == null) throw new KeyNotFoundException("Customer not found");

            entity.CustRiskScore = customer.Status.ToString();

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(string customerId)
        {
            var entity = await _dbContext.KycAudits
                .FirstOrDefaultAsync(c => c.CustId == customerId);

            if (entity == null) throw new KeyNotFoundException("Customer not found");

            _dbContext.KycAudits.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        private KycsyncDomainModels.CustomerRecord.StatusEnum MapStatus(string riskScore)
        {
            return riskScore switch
            {
                "A" => KycsyncDomainModels.CustomerRecord.StatusEnum.Active,
                "I" => KycsyncDomainModels.CustomerRecord.StatusEnum.Inactive,
                "S" => KycsyncDomainModels.CustomerRecord.StatusEnum.Suspended,
                _ => throw new ArgumentException("Invalid status code")
            };
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly KycsyncDbContext _dbContext;

        public TransactionRepository(KycsyncDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<KycsyncDomainModels.CustomerRecord.Transaction>> GetTransactionsByCustomerIdAsync(string customerId)
        {
            var transactions = await _dbContext.KycAudits
                .Where(c => c.CustId == customerId)
                .Select(c => new KycsyncDomainModels.CustomerRecord.Transaction
                {
                    TransactionId = Guid.NewGuid().ToString(), // Placeholder for transaction ID
                    Amount = 0, // Placeholder for transaction amount
                    Date = DateTime.UtcNow.ToString("o") // Fixed: Convert DateTime to ISO 8601 string format
                })
                .ToListAsync();

            return transactions;
        }

        public async Task AddTransactionAsync(string customerId, KycsyncDomainModels.CustomerRecord.Transaction transaction)
        {
            var customer = await _dbContext.KycAudits
                .FirstOrDefaultAsync(c => c.CustId == customerId);

            if (customer == null) throw new KeyNotFoundException("Customer not found");

            // Placeholder logic for adding a transaction
            // In a real implementation, this would involve adding a transaction record to a related table
            await Task.CompletedTask;
        }

        public async Task UpdateTransactionAsync(string customerId, KycsyncDomainModels.CustomerRecord.Transaction transaction)
        {
            var customer = await _dbContext.KycAudits
                .FirstOrDefaultAsync(c => c.CustId == customerId);

            if (customer == null) throw new KeyNotFoundException("Customer not found");

            // Placeholder logic for updating a transaction
            // In a real implementation, this would involve updating a transaction record in a related table
            await Task.CompletedTask;
        }

        public async Task DeleteTransactionAsync(string customerId, string transactionId)
        {
            var customer = await _dbContext.KycAudits
                .FirstOrDefaultAsync(c => c.CustId == customerId);

            if (customer == null) throw new KeyNotFoundException("Customer not found");

            // Placeholder logic for deleting a transaction
            // In a real implementation, this would involve removing a transaction record from a related table
            await Task.CompletedTask;
        }
    }
}