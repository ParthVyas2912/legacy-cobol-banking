using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FRDCHK.Application;
using FRDCHK.Application.Interfaces;
using FRDCHK.Domain;
using FRDCHK.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FRDCHK.Tests
{
    public class FrdchkParityTests
    {
        private readonly DbContextOptions<FrdchkDbContext> _dbContextOptions;

        public FrdchkParityTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<FrdchkDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private FrdchkService CreateService(FrdchkDbContext context, out MockFraudDetectionRepository fraudDetectionRepo, out MockTransactionRepository transactionRepo, out MockReportGenerator reportGenerator)
        {
            fraudDetectionRepo = new MockFraudDetectionRepository();
            transactionRepo = new MockTransactionRepository();
            reportGenerator = new MockReportGenerator();

            return new FrdchkService(fraudDetectionRepo, transactionRepo, reportGenerator);
        }

        [Fact]
        public async Task ExecuteAsync_HappyPath_ShouldProcessFraudCheckSuccessfully()
        {
            // Arrange
            using var context = new FrdchkDbContext(_dbContextOptions);
            var service = CreateService(context, out var fraudDetectionRepo, out var transactionRepo, out var reportGenerator);

            var transaction1 = new Transaction
            {
                Id = 1,
                Amount = 1000.00m,
                AccountNumber = "1234567890",
                TransactionDate = DateTime.UtcNow
            };

            var transaction2 = new Transaction
            {
                Id = 2,
                Amount = 500.00m,
                AccountNumber = "0987654321",
                TransactionDate = DateTime.UtcNow
            };

            transactionRepo.SeedTransactions(new List<Transaction> { transaction1, transaction2 });
            fraudDetectionRepo.SetFraudulentTransactions(new List<int> { 1 });

            // Act
            await service.ExecuteAsync();

            // Assert
            transactionRepo.FlaggedTransactions.Should().ContainSingle(t => t.Id == 1);
            transactionRepo.FlaggedTransactions.Should().NotContain(t => t.Id == 2);

            fraudDetectionRepo.LoadRulesCallCount.Should().Be(1);
            fraudDetectionRepo.ResetCountersCallCount.Should().Be(1);
            fraudDetectionRepo.SaveResultsCallCount.Should().Be(1);

            reportGenerator.GenerateFraudReportCallCount.Should().Be(1);
        }

        [Fact]
        public async Task ExecuteAsync_EmptyInput_ShouldHandleGracefully()
        {
            // Arrange
            using var context = new FrdchkDbContext(_dbContextOptions);
            var service = CreateService(context, out var fraudDetectionRepo, out var transactionRepo, out var reportGenerator);

            // Act
            await service.ExecuteAsync();

            // Assert
            transactionRepo.FlaggedTransactions.Should().BeEmpty();

            fraudDetectionRepo.LoadRulesCallCount.Should().Be(1);
            fraudDetectionRepo.ResetCountersCallCount.Should().Be(1);
            fraudDetectionRepo.SaveResultsCallCount.Should().Be(1);

            reportGenerator.GenerateFraudReportCallCount.Should().Be(1);
        }

        [Fact]
        public async Task ExecuteAsync_BoundaryValues_ShouldProcessCorrectly()
        {
            // Arrange
            using var context = new FrdchkDbContext(_dbContextOptions);
            var service = CreateService(context, out var fraudDetectionRepo, out var transactionRepo, out var reportGenerator);

            var maxTransaction = new Transaction
            {
                Id = 1,
                Amount = 9999999999999.99m, // Max value for PIC S9(13)V99 COMP-3
                AccountNumber = "1234567890",
                TransactionDate = DateTime.UtcNow
            };

            transactionRepo.SeedTransactions(new List<Transaction> { maxTransaction });
            fraudDetectionRepo.SetFraudulentTransactions(new List<int> { 1 });

            // Act
            await service.ExecuteAsync();

            // Assert
            transactionRepo.FlaggedTransactions.Should().ContainSingle(t => t.Id == 1);

            fraudDetectionRepo.LoadRulesCallCount.Should().Be(1);
            fraudDetectionRepo.ResetCountersCallCount.Should().Be(1);
            fraudDetectionRepo.SaveResultsCallCount.Should().Be(1);

            reportGenerator.GenerateFraudReportCallCount.Should().Be(1);
        }

        private class MockFraudDetectionRepository : IFraudDetectionRepository
        {
            public int LoadRulesCallCount { get; private set; }
            public int ResetCountersCallCount { get; private set; }
            public int SaveResultsCallCount { get; private set; }
            private List<int> _fraudulentTransactions = new();

            public Task LoadRulesAsync()
            {
                LoadRulesCallCount++;
                return Task.CompletedTask;
            }

            public void ResetCounters()
            {
                ResetCountersCallCount++;
            }

            public bool CheckTransactionForFraud(Transaction transaction)
            {
                return _fraudulentTransactions.Contains(transaction.Id);
            }

            public Task SaveResultsAsync()
            {
                SaveResultsCallCount++;
                return Task.CompletedTask;
            }

            public void SetFraudulentTransactions(List<int> fraudulentTransactionIds)
            {
                _fraudulentTransactions = fraudulentTransactionIds;
            }
        }

        private class MockTransactionRepository : ITransactionRepository
        {
            private readonly List<Transaction> _transactions = new();
            public List<Transaction> FlaggedTransactions { get; } = new();

            public Task<IEnumerable<Transaction>> GetPendingTransactionsAsync()
            {
                return Task.FromResult(_transactions.AsEnumerable());
            }

            public Task FlagTransactionAsFraudulentAsync(Transaction transaction)
            {
                FlaggedTransactions.Add(transaction);
                return Task.CompletedTask;
            }

            public void SeedTransactions(List<Transaction> transactions)
            {
                _transactions.AddRange(transactions);
            }
        }

        private class MockReportGenerator : IReportGenerator
        {
            public int GenerateFraudReportCallCount { get; private set; }

            public Task GenerateFraudReportAsync()
            {
                GenerateFraudReportCallCount++;
                return Task.CompletedTask;
            }
        }
    }
}