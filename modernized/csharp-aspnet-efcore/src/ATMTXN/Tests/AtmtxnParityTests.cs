using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATMTXN.Application;
using ATMTXN.Application.Interfaces;
using ATMTXN.Domain;
using ATMTXN.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

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
            var repository = new TransactionRepository(context);
            return new AtmtxnService(repository);
        }

        private AtmtxnDbContext CreateDbContext()
        {
            return new AtmtxnDbContext(_dbContextOptions);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessAllTransactions()
        {
            // Arrange
            using var context = CreateDbContext();
            var service = CreateService(context);

            var transaction1 = new AtmtxnDomainModels.Transaction
            {
                TransactionId = "TXN000001",
                CardNumber = "1234567890123456",
                TransactionAmount = 100.50m,
                TransactionDate = "20231001",
                TransactionTime = "120000",
                TransactionStatus = "Pending"
            };

            var transaction2 = new AtmtxnDomainModels.Transaction
            {
                TransactionId = "TXN000002",
                CardNumber = "6543210987654321",
                TransactionAmount = 200.75m,
                TransactionDate = "20231002",
                TransactionTime = "130000",
                TransactionStatus = "Pending"
            };

            await context.Transactions.AddRangeAsync(transaction1, transaction2);
            await context.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var processedTransactions = await context.Transactions.ToListAsync();
            processedTransactions.Should().HaveCount(2);
            processedTransactions.All(t => t.TransactionStatus == "Processed").Should().BeTrue();
            processedTransactions.All(t => t.ProcessedDate.HasValue).Should().BeTrue();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyDatabaseGracefully()
        {
            // Arrange
            using var context = CreateDbContext();
            var service = CreateService(context);

            // Act
            await service.ExecuteAsync();

            // Assert
            var transactions = await context.Transactions.ToListAsync();
            transactions.Should().BeEmpty();
        }

        [Fact]
        public async Task ProcessTransactionAsync_ShouldProcessSingleTransaction()
        {
            // Arrange
            using var context = CreateDbContext();
            var service = CreateService(context);

            var transaction = new AtmtxnDomainModels.Transaction
            {
                TransactionId = "TXN000003",
                CardNumber = "1234567890123456",
                TransactionAmount = 300.00m,
                TransactionDate = "20231003",
                TransactionTime = "140000",
                TransactionStatus = "Pending"
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            // Act
            await service.ProcessTransactionAsync(transaction.TransactionId);

            // Assert
            var processedTransaction = await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == transaction.TransactionId);
            processedTransaction.Should().NotBeNull();
            processedTransaction!.TransactionStatus.Should().Be("Processed");
            processedTransaction.ProcessedDate.Should().NotBeNull();
        }

        [Fact]
        public async Task ReverseTransactionAsync_ShouldReverseTransaction()
        {
            // Arrange
            using var context = CreateDbContext();
            var service = CreateService(context);

            var transaction = new AtmtxnDomainModels.Transaction
            {
                TransactionId = "TXN000004",
                CardNumber = "1234567890123456",
                TransactionAmount = 400.00m,
                TransactionDate = "20231004",
                TransactionTime = "150000",
                TransactionStatus = "Processed"
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            // Act
            await service.ReverseTransactionAsync(transaction.TransactionId);

            // Assert
            var reversedTransaction = await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == transaction.TransactionId);
            reversedTransaction.Should().NotBeNull();
            reversedTransaction!.TransactionStatus.Should().Be("Reversed");
            reversedTransaction.ReversedDate.Should().NotBeNull();
        }

        [Fact]
        public async Task AdjustTransactionAsync_ShouldAdjustTransaction()
        {
            // Arrange
            using var context = CreateDbContext();
            var service = CreateService(context);

            var transaction = new AtmtxnDomainModels.Transaction
            {
                TransactionId = "TXN000005",
                CardNumber = "1234567890123456",
                TransactionAmount = 500.00m,
                TransactionDate = "20231005",
                TransactionTime = "160000",
                TransactionStatus = "Processed"
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            // Act
            await service.AdjustTransactionAsync(transaction.TransactionId, 50.00m, "ADJ001");

            // Assert
            var adjustedTransaction = await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == transaction.TransactionId);
            adjustedTransaction.Should().NotBeNull();
            adjustedTransaction!.TransactionStatus.Should().Be("Adjusted");
            adjustedTransaction.AdjustmentAmount.Should().Be(50.00m);
            adjustedTransaction.AdjustmentReasonCode.Should().Be("ADJ001");
            adjustedTransaction.AdjustedDate.Should().NotBeNull();
        }

        [Fact]
        public async Task RefundTransactionAsync_ShouldRefundTransaction()
        {
            // Arrange
            using var context = CreateDbContext();
            var service = CreateService(context);

            var transaction = new AtmtxnDomainModels.Transaction
            {
                TransactionId = "TXN000006",
                CardNumber = "1234567890123456",
                TransactionAmount = 600.00m,
                TransactionDate = "20231006",
                TransactionTime = "170000",
                TransactionStatus = "Processed"
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            // Act
            await service.RefundTransactionAsync(transaction.TransactionId, 100.00m, "REF001");

            // Assert
            var refundedTransaction = await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == transaction.TransactionId);
            refundedTransaction.Should().NotBeNull();
            refundedTransaction!.TransactionStatus.Should().Be("Refunded");
            refundedTransaction.RefundAmount.Should().Be(100.00m);
            refundedTransaction.RefundReasonCode.Should().Be("REF001");
            refundedTransaction.RefundedDate.Should().NotBeNull();
        }

        [Fact]
        public async Task ReconcileTransactionAsync_ShouldReconcileTransaction()
        {
            // Arrange
            using var context = CreateDbContext();
            var service = CreateService(context);

            var transaction = new AtmtxnDomainModels.Transaction
            {
                TransactionId = "TXN000007",
                CardNumber = "1234567890123456",
                TransactionAmount = 700.00m,
                TransactionDate = "20231007",
                TransactionTime = "180000",
                TransactionStatus = "Processed"
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            // Act
            await service.ReconcileTransactionAsync(transaction.TransactionId, "REC001");

            // Assert
            var reconciledTransaction = await context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == transaction.TransactionId);
            reconciledTransaction.Should().NotBeNull();
            reconciledTransaction!.TransactionStatus.Should().Be("Reconciled");
            reconciledTransaction.ReconciliationReasonCode.Should().Be("REC001");
            reconciledTransaction.ReconciledDate.Should().NotBeNull();
        }
    }
}