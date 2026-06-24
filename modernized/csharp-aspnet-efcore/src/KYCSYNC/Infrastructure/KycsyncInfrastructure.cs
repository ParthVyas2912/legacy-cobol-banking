using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KYCSYNC.Domain;
using KYCSYNC.Application.Interfaces;

namespace KYCSYNC.Infrastructure
{
    public class KycsyncDbContext : DbContext
    {
        public KycsyncDbContext(DbContextOptions<KycsyncDbContext> options) : base(options) { }

        public DbSet<KycsyncDomainModels.Customer> Customers { get; set; }
        public DbSet<KycsyncDomainModels.Document> Documents { get; set; }
        public DbSet<KycAudit> KycAudits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KycsyncDomainModels.Customer>(entity =>
            {
                entity.ToTable("customers");
                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasMaxLength(10); // COBOL: CUSTOMER-ID PIC X(10)
                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(30); // COBOL: FIRST-NAME PIC X(30)
                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(30); // COBOL: LAST-NAME PIC X(30)
                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(1); // COBOL: GENDER PIC X(1)
                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("date_of_birth")
                    .HasColumnType("DATE"); // COBOL: DATE-OF-BIRTH PIC X(10)
                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(15); // COBOL: PHONE-NUMBER PIC X(15)
                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50); // COBOL: EMAIL PIC X(50)
                entity.OwnsOne(e => e.CustomerAddress, address =>
                {
                    address.Property(a => a.Street)
                        .HasColumnName("street")
                        .HasMaxLength(50); // COBOL: STREET PIC X(50)
                    address.Property(a => a.City)
                        .HasColumnName("city")
                        .HasMaxLength(30); // COBOL: CITY PIC X(30)
                    address.Property(a => a.State)
                        .HasColumnName("state")
                        .HasMaxLength(20); // COBOL: STATE PIC X(20)
                    address.Property(a => a.PostalCode)
                        .HasColumnName("postal_code")
                        .HasMaxLength(10); // COBOL: POSTAL-CODE PIC X(10)
                    address.Property(a => a.Country)
                        .HasColumnName("country")
                        .HasMaxLength(30); // COBOL: COUNTRY PIC X(30)
                });
            });

            modelBuilder.Entity<KycsyncDomainModels.Document>(entity =>
            {
                entity.ToTable("documents");
                entity.HasKey(e => e.DocumentId);
                entity.Property(e => e.DocumentId)
                    .HasColumnName("document_id")
                    .HasMaxLength(15); // COBOL: DOCUMENT-ID PIC X(15)
                entity.Property(e => e.DocumentType)
                    .HasColumnName("document_type")
                    .HasMaxLength(20); // COBOL: DOCUMENT-TYPE PIC X(20)
                entity.Property(e => e.DocumentName)
                    .HasColumnName("document_name")
                    .HasMaxLength(50); // COBOL: DOCUMENT-NAME PIC X(50)
                entity.Property(e => e.IssueDate)
                    .HasColumnName("issue_date")
                    .HasColumnType("DATE"); // COBOL: ISSUE-DATE PIC X(10)
                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("expiry_date")
                    .HasColumnType("DATE"); // COBOL: EXPIRY-DATE PIC X(10)
                entity.Property(e => e.VerificationStatus)
                    .HasColumnName("verification_status")
                    .HasMaxLength(1); // COBOL: VERIFICATION-STATUS PIC X(1)
            });

            modelBuilder.Entity<KycAudit>(entity =>
            {
                entity.ToTable("kyc_audit");
                entity.HasKey(e => e.CustId);
                entity.Property(e => e.CustId)
                    .HasColumnName("cust_id")
                    .HasMaxLength(10); // COBOL: CUST-ID PIC X(10)
                entity.Property(e => e.CustKycStatus)
                    .HasColumnName("cust_kyc_status")
                    .HasMaxLength(30); // COBOL: CUST-KYC-STATUS PIC X(30)
                entity.Property(e => e.CustRiskScore)
                    .HasColumnName("cust_risk_score")
                    .HasMaxLength(30); // COBOL: CUST-RISK-SCORE PIC X(30)
            });
        }
    }

    public class KycAudit
    {
        public string CustId { get; set; } = string.Empty; // COBOL: CUST-ID PIC X(10)
        public string CustKycStatus { get; set; } = string.Empty; // COBOL: CUST-KYC-STATUS PIC X(30)
        public string CustRiskScore { get; set; } = string.Empty; // COBOL: CUST-RISK-SCORE PIC X(30)
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly KycsyncDbContext _context;

        public CustomerRepository(KycsyncDbContext context)
        {
            _context = context;
        }

        public async Task<List<KycsyncDomainModels.Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<KycsyncDomainModels.Customer?> GetCustomerByIdAsync(string customerId)
        {
            return await _context.Customers.AsNoTracking().SingleOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task AddCustomerAsync(KycsyncDomainModels.Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(KycsyncDomainModels.Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(string customerId)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class DocumentRepository : IDocumentRepository
    {
        private readonly KycsyncDbContext _context;

        public DocumentRepository(KycsyncDbContext context)
        {
            _context = context;
        }

        public async Task<List<KycsyncDomainModels.Document>> GetDocumentsByCustomerIdAsync(string customerId)
        {
            return await _context.Documents.AsNoTracking()
                .Where(d => d.DocumentId.StartsWith(customerId))
                .ToListAsync();
        }

        public async Task<KycsyncDomainModels.Document?> GetDocumentByIdAsync(string documentId)
        {
            return await _context.Documents.AsNoTracking().SingleOrDefaultAsync(d => d.DocumentId == documentId);
        }

        public async Task AddDocumentAsync(KycsyncDomainModels.Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDocumentAsync(KycsyncDomainModels.Document document)
        {
            _context.Documents.Update(document);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDocumentAsync(string documentId)
        {
            var document = await _context.Documents.SingleOrDefaultAsync(d => d.DocumentId == documentId);
            if (document != null)
            {
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
            }
        }
    }

    public static class KycsyncInfrastructureExtensions
    {
        public static IServiceCollection AddKycsyncInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<KycsyncDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();

            return services;
        }
    }
}