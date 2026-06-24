using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using STMTRPT.Application;
using STMTRPT.Application.Interfaces;
using STMTRPT.Domain;
using STMTRPT.Infrastructure;
using Xunit;
using FluentAssertions;

namespace STMTRPT.Tests
{
    public class StmtrptParityTests
    {
        private DbContextOptions<StmtrptDbContext> CreateInMemoryDatabaseOptions()
        {
            return new DbContextOptionsBuilder<StmtrptDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task ExecuteAsync_HappyPath_ShouldProcessRecordsCorrectly()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            await using var context = new StmtrptDbContext(options);

            var statementReportRepository = new StatementReportRepository(context);
            var transactionRepository = new TransactionRepository(context);
            var service = new StmtrptService();

            // Seed data
            var initialStatementReport = new StmtrptDomainModels.StatementReport
            {
                AccountNumber = "1234567890",
                AccountHolderName = "John Doe",
                AccountBalance = 1000.00M,
                AccountStatus = StmtrptDomainModels.AccountStatusEnum.Active,
                Transactions = new List<StmtrptDomainModels.StatementReport.Transaction>
                {
                    new StmtrptDomainModels.StatementReport.Transaction
                    {
                        TransactionId = "TXN001",
                        Description = "Deposit",
                        Amount = 500.00M,
                        TransactionDate = "20231010"
                    },
                    new StmtrptDomainModels.StatementReport.Transaction
                    {
                        TransactionId = "TXN002",
                        Description = "Withdrawal",
                        Amount = -200.00M,
                        TransactionDate = "20231011"
                    }
                }
            };

            await statementReportRepository.AddAsync(initialStatementReport);

            // Act
            await service.ExecuteAsync();

            // Assert
            var processedReport = await statementReportRepository.GetByAccountNumberAsync("1234567890");
            processedReport.Should().NotBeNull();
            processedReport!.AccountBalance.Should().Be(1000.00M);
            processedReport.Transactions.Should().HaveCount(2);
            processedReport.Transactions[0].Description.Should().Be("Deposit");
            processedReport.Transactions[0].Amount.Should().Be(500.00M);
            processedReport.Transactions[1].Description.Should().Be("Withdrawal");
            processedReport.Transactions[1].Amount.Should().Be(-200.00M);
        }

        [Fact]
        public async Task ExecuteAsync_EmptyInput_ShouldHandleGracefully()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            await using var context = new StmtrptDbContext(options);

            var statementReportRepository = new StatementReportRepository(context);
            var transactionRepository = new TransactionRepository(context);
            var service = new StmtrptService();

            // Act
            await service.ExecuteAsync();

            // Assert
            var allReports = await context.Stmtouts.ToListAsync();
            allReports.Should().BeEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_BoundaryValues_ShouldHandleMaxValuesCorrectly()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            await using var context = new StmtrptDbContext(options);

            var statementReportRepository = new StatementReportRepository(context);
            var transactionRepository = new TransactionRepository(context);
            var service = new StmtrptService();

            // Seed data with boundary values
            var maxStatementReport = new StmtrptDomainModels.StatementReport
            {
                AccountNumber = "9999999999", // Max PIC X(10)
                AccountHolderName = new string('X', 30), // Max PIC X(30)
                AccountBalance = 9999999999999.99M, // Max PIC 9(13)V99 COMP-3
                AccountStatus = StmtrptDomainModels.AccountStatusEnum.Active,
                Transactions = new List<StmtrptDomainModels.StatementReport.Transaction>
                {
                    new StmtrptDomainModels.StatementReport.Transaction
                    {
                        TransactionId = "TXN999999", // Max PIC X(10)
                        Description = new string('Y', 20), // Max PIC X(20)
                        Amount = 99999999999.99M, // Max PIC 9(11)V99 COMP-3
                        TransactionDate = "99991231" // Max PIC X(8)
                    }
                }
            };

            await statementReportRepository.AddAsync(maxStatementReport);

            // Act
            await service.ExecuteAsync();

            // Assert
            var processedReport = await statementReportRepository.GetByAccountNumberAsync("9999999999");
            processedReport.Should().NotBeNull();
            processedReport!.AccountBalance.Should().Be(9999999999999.99M);
            processedReport.AccountHolderName.Should().Be(new string('X', 30));
            processedReport.Transactions.Should().HaveCount(1);
            processedReport.Transactions[0].TransactionId.Should().Be("TXN999999");
            processedReport.Transactions[0].Description.Should().Be(new string('Y', 20));
            processedReport.Transactions[0].Amount.Should().Be(99999999999.99M);
            processedReport.Transactions[0].TransactionDate.Should().Be("99991231");
        }
    }
}