using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using ATMTXN.Application;
using ATMTXN.Application.Interfaces;
using ATMTXN.Domain;
using ATMTXN.Infrastructure;

namespace ATMTXN.Tests
{
    public class AtmtxnParityTests
    {
        private readonly DbContextOptions<AtmtxnDbContext> _dbContextOptions;

        public AtmtxnParityTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AtmtxnDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private AtmtxnService CreateService(AtmtxnDbContext context)
        {
            var transactionRepository = new TransactionRepository(context);
            var transactionDetailRepository = new TransactionDetailRepository(context);
            return new AtmtxnService();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessTransactionSuccessfully()
        {
            // Arrange
            using var context = new AtmtxnDbContext(_dbContextOptions);
            var service = CreateService(context);

            var transaction = new AtmtxnDomainModels.TransactionRecord
            {
                TransactionId = "TXN123456789",
                AccountNumber = "1234567890123456",
                TransactionAmount = 1000.50m,
                TransactionType = "D",
                Status = AtmtxnDomainModels.TransactionStatus.Pending,
                TransactionDate = new DateTime(2023, 10, 1),
                Location = new AtmtxnDomainModels.TransactionLocation
                {
                    City = "New York",
                    State = "NY",
                    ZipCode = "10001"
                },
                Details = new List<AtmtxnDomainModels.TransactionDetail>
                {
                    new AtmtxnDomainModels.TransactionDetail
                    {
                        ItemCode = "ITEM001",
                        ItemAmount = 500.25m,
                        Description = "Test Item 1"
                    },
                    new AtmtxnDomainModels.TransactionDetail
                    {
                        ItemCode = "ITEM002",
                        ItemAmount = 500.25m,
                        Description = "Test Item 2"
                    }
                }
            };

            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var processedTransaction = await context.Transactions
                .SingleOrDefaultAsync(t => t.TransactionId == "TXN123456789");

            processedTransaction.Should().NotBeNull();
            processedTransaction!.Status.Should().Be(AtmtxnDomainModels.TransactionStatus.Completed);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyInputGracefully()
        {
            // Arrange
            using var context = new AtmtxnDbContext(_dbContextOptions);
            var service = CreateService(context);

            // Act
            await service.ExecuteAsync();

            // Assert
            var transactions = await context.Transactions.ToListAsync();
            transactions.Should().BeEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleBoundaryValues()
        {
            // Arrange
            using var context = new AtmtxnDbContext(_dbContextOptions);
            var service = CreateService(context);

            var transaction = new AtmtxnDomainModels.TransactionRecord
            {
                TransactionId = "TXN999999999",
                AccountNumber = "9999999999999999",
                TransactionAmount = 9999999999999.99m,
                TransactionType = "W",
                Status = AtmtxnDomainModels.TransactionStatus.Pending,
                TransactionDate = new DateTime(9999, 12, 31),
                Location = new AtmtxnDomainModels.TransactionLocation
                {
                    City = "BoundaryCity",
                    State = "BC",
                    ZipCode = "99999"
                },
                Details = new List<AtmtxnDomainModels.TransactionDetail>
                {
                    new AtmtxnDomainModels.TransactionDetail
                    {
                        ItemCode = "ITEM999999",
                        ItemAmount = 9999999.99m,
                        Description = "Boundary Item"
                    }
                }
            };

            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var processedTransaction = await context.Transactions
                .SingleOrDefaultAsync(t => t.TransactionId == "TXN999999999");

            processedTransaction.Should().NotBeNull();
            processedTransaction!.Status.Should().Be(AtmtxnDomainModels.TransactionStatus.Completed);
        }
    }
}