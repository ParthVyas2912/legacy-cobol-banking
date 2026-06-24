using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TXNPOST.Application;
using TXNPOST.Application.Interfaces;
using TXNPOST.Domain;
using TXNPOST.Infrastructure;
using Xunit;

namespace TXNPOST.Tests
{
    public class TxnpostParityTests
    {
        private readonly ServiceProvider _serviceProvider;

        public TxnpostParityTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<TxnpostDbContext>(options =>
                options.UseInMemoryDatabase("TxnpostTestDb"));
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionDetailRepository, TransactionDetailRepository>();
            services.AddScoped<ITxnpostService, TxnpostService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessTransactionsCorrectly()
        {
            // Arrange
            var transactionRepository = _serviceProvider.GetRequiredService<ITransactionRepository>();
            var txnpostService = _serviceProvider.GetRequiredService<ITxnpostService>();

            var transaction = new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = "1234567890",
                AccountNumber = "1234567890",
                TransactionAmount = 1000.50m,
                TransactionType = "D",
                TransactionDate = "20231010",
                TransactionTime = "123456",
                TransactionStatus = TxnpostDomainModels.TransactionStatusEnum.Pending
            };

            await transactionRepository.AddTransactionAsync(transaction);

            // Act
            await txnpostService.ExecuteAsync();

            // Assert
            var updatedTransaction = await transactionRepository.GetTransactionByIdAsync("1234567890");
            updatedTransaction.Should().NotBeNull();
            updatedTransaction!.TransactionStatus.Should().Be(TxnpostDomainModels.TransactionStatusEnum.Completed);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyInputGracefully()
        {
            // Arrange
            var txnpostService = _serviceProvider.GetRequiredService<ITxnpostService>();

            // Act
            Func<Task> act = async () => await txnpostService.ExecuteAsync();

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleBoundaryValuesCorrectly()
        {
            // Arrange
            var transactionRepository = _serviceProvider.GetRequiredService<ITransactionRepository>();
            var txnpostService = _serviceProvider.GetRequiredService<ITxnpostService>();

            var maxTransaction = new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = "9999999999",
                AccountNumber = "9999999999",
                TransactionAmount = 9999999999999.99m,
                TransactionType = "W",
                TransactionDate = "99991231",
                TransactionTime = "235959",
                TransactionStatus = TxnpostDomainModels.TransactionStatusEnum.Pending
            };

            await transactionRepository.AddTransactionAsync(maxTransaction);

            // Act
            await txnpostService.ExecuteAsync();

            // Assert
            var updatedTransaction = await transactionRepository.GetTransactionByIdAsync("9999999999");
            updatedTransaction.Should().NotBeNull();
            updatedTransaction!.TransactionStatus.Should().Be(TxnpostDomainModels.TransactionStatusEnum.Completed);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldUpdateTransactionDetailsCorrectly()
        {
            // Arrange
            var transactionRepository = _serviceProvider.GetRequiredService<ITransactionRepository>();
            var transactionDetailRepository = _serviceProvider.GetRequiredService<ITransactionDetailRepository>();
            var txnpostService = _serviceProvider.GetRequiredService<ITxnpostService>();

            var transaction = new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = "1234567890",
                AccountNumber = "1234567890",
                TransactionAmount = 500.00m,
                TransactionType = "D",
                TransactionDate = "20231010",
                TransactionTime = "123456",
                TransactionStatus = TxnpostDomainModels.TransactionStatusEnum.Pending,
                TransactionDetails = new List<TxnpostDomainModels.TransactionDetail>
                {
                    new TxnpostDomainModels.TransactionDetail
                    {
                        DetailId = "D1234567890",
                        Description = "Initial Deposit",
                        DetailAmount = 500.00m
                    }
                }
            };

            await transactionRepository.AddTransactionAsync(transaction);

            // Act
            await txnpostService.ExecuteAsync();

            // Assert
            var transactionDetails = await transactionDetailRepository.GetTransactionDetailsByTransactionIdAsync("1234567890");
            transactionDetails.Should().HaveCount(1);
            transactionDetails[0].Description.Should().Be("Initial Deposit");
            transactionDetails[0].DetailAmount.Should().Be(500.00m);
        }
    }
}