using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using KYCSYNC.Application;
using KYCSYNC.Application.Interfaces;
using KYCSYNC.Domain;
using KYCSYNC.Infrastructure;

namespace KYCSYNC.Tests
{
    public class KycsyncParityTests
    {
        private readonly DbContextOptions<KycsyncDbContext> _dbContextOptions;

        public KycsyncParityTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<KycsyncDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private KycsyncDbContext CreateDbContext()
        {
            return new KycsyncDbContext(_dbContextOptions);
        }

        private KycsyncService CreateKycsyncService(KycsyncDbContext context)
        {
            var customerRepository = new CustomerRepository(context);
            var documentRepository = new DocumentRepository(context);
            return new KycsyncService();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyDatabaseGracefully()
        {
            // Arrange
            using var context = CreateDbContext();
            var service = CreateKycsyncService(context);

            // Act
            await service.ExecuteAsync();

            // Assert
            var customers = await context.Customers.ToListAsync();
            customers.Should().BeEmpty();

            var documents = await context.Documents.ToListAsync();
            documents.Should().BeEmpty();

            var audits = await context.KycAudits.ToListAsync();
            audits.Should().BeEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessCustomersAndDocumentsCorrectly()
        {
            // Arrange
            using var context = CreateDbContext();
            var service = CreateKycsyncService(context);

            var customer = new KycsyncDomainModels.Customer
            {
                CustomerId = "CUST000001",
                FirstName = "John",
                LastName = "Doe",
                Gender = "M",
                DateOfBirth = "19800101",
                CustomerAddress = new KycsyncDomainModels.Address
                {
                    Street = "123 Main St",
                    City = "Metropolis",
                    State = "NY",
                    PostalCode = "12345",
                    Country = "USA"
                },
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                KycStatus = KycsyncDomainModels.KycStatusEnum.NotVerified
            };

            var document = new KycsyncDomainModels.Document
            {
                DocumentId = "DOC0000001",
                DocumentType = "ID",
                DocumentName = "Passport",
                IssueDate = "20200101",
                ExpiryDate = "20300101",
                VerificationStatus = "N"
            };

            await context.Customers.AddAsync(customer);
            await context.Documents.AddAsync(document);
            await context.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var updatedCustomer = await context.Customers.SingleOrDefaultAsync(c => c.CustomerId == "CUST000001");
            updatedCustomer.Should().NotBeNull();
            updatedCustomer!.KycStatus.Should().Be(KycsyncDomainModels.KycStatusEnum.Verified);

            var updatedDocument = await context.Documents.SingleOrDefaultAsync(d => d.DocumentId == "DOC0000001");
            updatedDocument.Should().NotBeNull();
            updatedDocument!.VerificationStatus.Should().Be("Y");

            var audit = await context.KycAudits.SingleOrDefaultAsync(a => a.CustId == "CUST000001");
            audit.Should().NotBeNull();
            audit!.CustKycStatus.Should().Be("Verified");
            audit.CustRiskScore.Should().Be("Low");
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleBoundaryValuesCorrectly()
        {
            // Arrange
            using var context = CreateDbContext();
            var service = CreateKycsyncService(context);

            var customer = new KycsyncDomainModels.Customer
            {
                CustomerId = "CUST999999",
                FirstName = new string('A', 30),
                LastName = new string('B', 30),
                Gender = "F",
                DateOfBirth = "19000101",
                CustomerAddress = new KycsyncDomainModels.Address
                {
                    Street = new string('C', 50),
                    City = new string('D', 30),
                    State = new string('E', 20),
                    PostalCode = new string('9', 10),
                    Country = new string('F', 30)
                },
                PhoneNumber = new string('9', 15),
                Email = new string('G', 50),
                KycStatus = KycsyncDomainModels.KycStatusEnum.NotVerified
            };

            var document = new KycsyncDomainModels.Document
            {
                DocumentId = "DOC9999999",
                DocumentType = new string('H', 20),
                DocumentName = new string('I', 50),
                IssueDate = "19000101",
                ExpiryDate = "20991231",
                VerificationStatus = "N"
            };

            await context.Customers.AddAsync(customer);
            await context.Documents.AddAsync(document);
            await context.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var updatedCustomer = await context.Customers.SingleOrDefaultAsync(c => c.CustomerId == "CUST999999");
            updatedCustomer.Should().NotBeNull();
            updatedCustomer!.KycStatus.Should().Be(KycsyncDomainModels.KycStatusEnum.Verified);

            var updatedDocument = await context.Documents.SingleOrDefaultAsync(d => d.DocumentId == "DOC9999999");
            updatedDocument.Should().NotBeNull();
            updatedDocument!.VerificationStatus.Should().Be("Y");

            var audit = await context.KycAudits.SingleOrDefaultAsync(a => a.CustId == "CUST999999");
            audit.Should().NotBeNull();
            audit!.CustKycStatus.Should().Be("Verified");
            audit.CustRiskScore.Should().Be("Low");
        }
    }
}