using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using ACCTMGR.Domain;
using ACCTMGR.Application;
using ACCTMGR.Infrastructure;

namespace ACCTMGR.Tests
{
    public class AcctmgrParityTests
    {
        private DbContextOptions<AcctmgrDbContext> CreateInMemoryDatabaseOptions()
        {
            return new DbContextOptionsBuilder<AcctmgrDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private AcctmgrService CreateService(AcctmgrDbContext context)
        {
            var accountRepository = new AccountRepository(context);
            var customerRepository = new CustomerRepository(context);
            var transactionRepository = new TransactionRepository(context);
            var accountSummaryRepository = new AccountSummaryRepository(context);

            // Assuming AcctmgrService has dependencies injected via constructor
            return new AcctmgrService();
        }

        [Fact]
        public async Task ExecuteAsync_HappyPath_ShouldProcessAllRecordsCorrectly()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            using var context = new AcctmgrDbContext(options);

            // Seed data
            var account = new AccountManager.Account
            {
                AccountNumber = "1234567890",
                AccountName = "Test Account",
                Balance = 1000.00m,
                AccountStatus = "A"
            };
            var customer = new AccountManager.Customer
            {
                CustomerId = "CUST12345",
                CustomerName = "John Doe",
                CustomerType = "I"
            };
            var transaction = new AccountManager.Transaction
            {
                TransactionId = "TXN123456789",
                TransactionAmount = 500.00m,
                TransactionDate = "20231010",
                TransactionType = "D"
            };
            var accountSummary = new AccountManager.AccountSummary
            {
                AccountNumber = "1234567890",
                TotalCredits = 500.00m,
                TotalDebits = 0.00m,
                NetBalance = 1500.00m
            };

            await context.Accounts.AddAsync(account);
            await context.Customers.AddAsync(customer);
            await context.Transactions.AddAsync(transaction);
            await context.AccountSummaries.AddAsync(accountSummary);
            await context.SaveChangesAsync();

            var service = CreateService(context);

            // Act
            await service.ExecuteAsync();

            // Assert
            var accounts = await context.Accounts.ToListAsync();
            accounts.Should().HaveCount(1);
            accounts[0].AccountNumber.Should().Be("1234567890");
            accounts[0].Balance.Should().Be(1500.00m);

            var customers = await context.Customers.ToListAsync();
            customers.Should().HaveCount(1);
            customers[0].CustomerId.Should().Be("CUST12345");
            customers[0].CustomerName.Should().Be("John Doe");

            var transactions = await context.Transactions.ToListAsync();
            transactions.Should().HaveCount(1);
            transactions[0].TransactionId.Should().Be("TXN123456789");
            transactions[0].TransactionAmount.Should().Be(500.00m);

            var summaries = await context.AccountSummaries.ToListAsync();
            summaries.Should().HaveCount(1);
            summaries[0].AccountNumber.Should().Be("1234567890");
            summaries[0].NetBalance.Should().Be(1500.00m);
        }

        [Fact]
        public async Task ExecuteAsync_EmptyInput_ShouldHandleGracefully()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            using var context = new AcctmgrDbContext(options);
            var service = CreateService(context);

            // Act
            await service.ExecuteAsync();

            // Assert
            var accounts = await context.Accounts.ToListAsync();
            accounts.Should().BeEmpty();

            var customers = await context.Customers.ToListAsync();
            customers.Should().BeEmpty();

            var transactions = await context.Transactions.ToListAsync();
            transactions.Should().BeEmpty();

            var summaries = await context.AccountSummaries.ToListAsync();
            summaries.Should().BeEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_BoundaryValues_ShouldHandleMaxValuesCorrectly()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            using var context = new AcctmgrDbContext(options);

            // Seed data with boundary values
            var account = new AccountManager.Account
            {
                AccountNumber = "9999999999",
                AccountName = "Max Account",
                Balance = 9999999999999.99m,
                AccountStatus = "C"
            };
            var customer = new AccountManager.Customer
            {
                CustomerId = "CUST99999999999",
                CustomerName = new string('X', 50),
                CustomerType = "C"
            };
            var transaction = new AccountManager.Transaction
            {
                TransactionId = "TXN999999999999",
                TransactionAmount = 99999999999.99m,
                TransactionDate = "99991231",
                TransactionType = "T"
            };
            var accountSummary = new AccountManager.AccountSummary
            {
                AccountNumber = "9999999999",
                TotalCredits = 9999999999999.99m,
                TotalDebits = 9999999999999.99m,
                NetBalance = 0.00m
            };

            await context.Accounts.AddAsync(account);
            await context.Customers.AddAsync(customer);
            await context.Transactions.AddAsync(transaction);
            await context.AccountSummaries.AddAsync(accountSummary);
            await context.SaveChangesAsync();

            var service = CreateService(context);

            // Act
            await service.ExecuteAsync();

            // Assert
            var accounts = await context.Accounts.ToListAsync();
            accounts.Should().HaveCount(1);
            accounts[0].AccountNumber.Should().Be("9999999999");
            accounts[0].Balance.Should().Be(9999999999999.99m);

            var customers = await context.Customers.ToListAsync();
            customers.Should().HaveCount(1);
            customers[0].CustomerId.Should().Be("CUST99999999999");
            customers[0].CustomerName.Should().Be(new string('X', 50));

            var transactions = await context.Transactions.ToListAsync();
            transactions.Should().HaveCount(1);
            transactions[0].TransactionId.Should().Be("TXN999999999999");
            transactions[0].TransactionAmount.Should().Be(99999999999.99m);

            var summaries = await context.AccountSummaries.ToListAsync();
            summaries.Should().HaveCount(1);
            summaries[0].AccountNumber.Should().Be("9999999999");
            summaries[0].NetBalance.Should().Be(0.00m);
        }
    }
}