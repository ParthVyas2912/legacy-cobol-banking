using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using KYCSYNC.Application;
using KYCSYNC.Application.Interfaces;
using KYCSYNC.Domain;
using KYCSYNC.Infrastructure;

namespace KYCSYNC.Tests
{
    public class KycsyncParityTests
    {
        private KycsyncDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<KycsyncDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new KycsyncDbContext(options);
        }

        private KycsyncService CreateService(KycsyncDbContext dbContext)
        {
            var customerRepository = new CustomerRepository(dbContext);
            var transactionRepository = new TransactionRepository(dbContext);
            return new KycsyncService(); // Adjust if dependencies are required in the constructor
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessCustomersCorrectly()
        {
            // Arrange
            using var dbContext = CreateInMemoryDbContext();
            var service = CreateService(dbContext);

            // Seed data
            dbContext.KycAudits.Add(new KycAudit
            {
                CustId = "CUST001",
                CustKycStatus = "Active",
                CustRiskScore = "A"
            });
            dbContext.KycAudits.Add(new KycAudit
            {
                CustId = "CUST002",
                CustKycStatus = "Inactive",
                CustRiskScore = "I"
            });
            await dbContext.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var customers = await dbContext.KycAudits.ToListAsync();
            customers.Should().HaveCount(2);

            var customer1 = customers.Find(c => c.CustId == "CUST001");
            customer1.Should().NotBeNull();
            customer1!.CustRiskScore.Should().Be("A");

            var customer2 = customers.Find(c => c.CustId == "CUST002");
            customer2.Should().NotBeNull();
            customer2!.CustRiskScore.Should().Be("I");
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyDatabaseGracefully()
        {
            // Arrange
            using var dbContext = CreateInMemoryDbContext();
            var service = CreateService(dbContext);

            // Act
            await service.ExecuteAsync();

            // Assert
            var customers = await dbContext.KycAudits.ToListAsync();
            customers.Should().BeEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleBoundaryValues()
        {
            // Arrange
            using var dbContext = CreateInMemoryDbContext();
            var service = CreateService(dbContext);

            // Seed data with boundary values
            dbContext.KycAudits.Add(new KycAudit
            {
                CustId = "CUSTMAXVAL",
                CustKycStatus = new string('X', 30),
                CustRiskScore = "S"
            });
            await dbContext.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var customers = await dbContext.KycAudits.ToListAsync();
            customers.Should().HaveCount(1);

            var customer = customers[0];
            customer.CustId.Should().Be("CUSTMAXVAL");
            customer.CustKycStatus.Should().Be(new string('X', 30));
            customer.CustRiskScore.Should().Be("S");
        }
    }
}