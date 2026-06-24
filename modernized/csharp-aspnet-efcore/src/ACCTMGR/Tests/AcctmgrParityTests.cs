using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using ACCTMGR.Domain;
using ACCTMGR.Application;
using ACCTMGR.Application.Interfaces;
using ACCTMGR.Infrastructure;

namespace ACCTMGR.Tests
{
    public class AcctmgrParityTests
    {
        private readonly DbContextOptions<AcctmgrDbContext> _dbContextOptions;

        public AcctmgrParityTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AcctmgrDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessAllRecords()
        {
            // Arrange
            using var context = new AcctmgrDbContext(_dbContextOptions);
            var accountRepository = new AccountRepository(context);
            var transactionRepository = new TransactionRepository(context);
            var service = new AcctmgrService();

            SeedDatabase(context);

            // Act
            await service.ExecuteAsync();

            // Assert
            context.Accounts.Count().Should().Be(3); // Ensure all accounts are still present
            context.Transactions.Count().Should().Be(0); // No transactions should have been added
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyDatabaseGracefully()
        {
            // Arrange
            using var context = new AcctmgrDbContext(_dbContextOptions);
            var accountRepository = new AccountRepository(context);
            var transactionRepository = new TransactionRepository(context);
            var service = new AcctmgrService();

            // Act
            await service.ExecuteAsync();

            // Assert
            context.Accounts.Count().Should().Be(0); // No accounts should exist
            context.Transactions.Count().Should().Be(0); // No transactions should exist
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleBoundaryValues()
        {
            // Arrange
            using var context = new AcctmgrDbContext(_dbContextOptions);
            var accountRepository = new AccountRepository(context);
            var transactionRepository = new TransactionRepository(context);
            var service = new AcctmgrService();

            SeedDatabaseWithBoundaryValues(context);

            // Act
            await service.ExecuteAsync();

            // Assert
            var account = await context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == "1234567890");
            account.Should().NotBeNull();
            account!.AccountBalance.Should().Be(9999999999999.99m); // Max COMP-3 value
        }

        private void SeedDatabase(AcctmgrDbContext context)
        {
            var accounts = new List<AccountManager>
            {
                new AccountManager
                {
                    AccountNumber = "0000000001",
                    AccountName = "John Doe",
                    AccountBalance = 1000.00m,
                    AccountType = "CHK",
                    AccountStatus = "A",
                    Status = AccountManager.AccountStatusEnum.Active
                },
                new AccountManager
                {
                    AccountNumber = "0000000002",
                    AccountName = "Jane Smith",
                    AccountBalance = 2000.00m,
                    AccountType = "SAV",
                    AccountStatus = "A",
                    Status = AccountManager.AccountStatusEnum.Active
                },
                new AccountManager
                {
                    AccountNumber = "0000000003",
                    AccountName = "Alice Johnson",
                    AccountBalance = 3000.00m,
                    AccountType = "BUS",
                    AccountStatus = "I",
                    Status = AccountManager.AccountStatusEnum.Inactive
                }
            };

            context.Accounts.AddRange(accounts);
            context.SaveChanges();
        }

        private void SeedDatabaseWithBoundaryValues(AcctmgrDbContext context)
        {
            var accounts = new List<AccountManager>
            {
                new AccountManager
                {
                    AccountNumber = "1234567890", // Max PIC X(10)
                    AccountName = new string('A', 30), // Max PIC X(30)
                    AccountBalance = 9999999999999.99m, // Max PIC S9(13)V99 COMP-3
                    AccountType = "ABC", // Max PIC X(3)
                    AccountStatus = "A", // PIC X(1)
                    Status = AccountManager.AccountStatusEnum.Active
                }
            };

            context.Accounts.AddRange(accounts);
            context.SaveChanges();
        }
    }
}