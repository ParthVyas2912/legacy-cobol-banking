using System;
using System.Collections.Generic;
using System.Linq;
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
            services.AddScoped<ICicsService, CicsService>();
            services.AddScoped<ITxnpostService, TxnpostService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessTransactionsCorrectly()
        {
            // Arrange
            var transactionRepository = _serviceProvider.GetRequiredService<ITransactionRepository>();
            var txnpostService = _serviceProvider.GetRequiredService<ITxnpostService>();

            var transaction = new TxnpostDomainModels.Transaction
            {
                TransactionId = "TXN123456789",
                AccountNumber = "ACC1234567",
                TransactionAmount = 1000.50m,
                TransactionDate = "20231010",
                TransactionType = "D",
                Status = TxnpostDomainModels.TransactionStatus.Pending
            };

            await transactionRepository.AddTransactionAsync(transaction);

            // Act
            await txnpostService.ExecuteAsync();

            // Assert
            var processedTransaction = await transactionRepository.GetTransactionByIdAsync("TXN123456789");
            processedTransaction.Should().NotBeNull();
            processedTransaction!.Status.Should().Be(TxnpostDomainModels.TransactionStatus.Completed);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyInputGracefully()
        {
            // Arrange
            var txnpostService = _serviceProvider.GetRequiredService<ITxnpostService>();
            var transactionRepository = _serviceProvider.GetRequiredService<ITransactionRepository>();

            // Act
            await txnpostService.ExecuteAsync();

            // Assert
            var transactions = await transactionRepository.GetAllTransactionsAsync();
            transactions.Should().BeEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleBoundaryValues()
        {
            // Arrange
            var transactionRepository = _serviceProvider.GetRequiredService<ITransactionRepository>();
            var txnpostService = _serviceProvider.GetRequiredService<ITxnpostService>();

            var transaction = new TxnpostDomainModels.Transaction
            {
                TransactionId = "TXN999999999",
                AccountNumber = "ACC9999999",
                TransactionAmount = 99999999999.99m,
                TransactionDate = "99991231",
                TransactionType = "C",
                Status = TxnpostDomainModels.TransactionStatus.Pending
            };

            await transactionRepository.AddTransactionAsync(transaction);

            // Act
            await txnpostService.ExecuteAsync();

            // Assert
            var processedTransaction = await transactionRepository.GetTransactionByIdAsync("TXN999999999");
            processedTransaction.Should().NotBeNull();
            processedTransaction!.TransactionAmount.Should().Be(99999999999.99m);
            processedTransaction.Status.Should().Be(TxnpostDomainModels.TransactionStatus.Completed);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessTransactionDetails()
        {
            // Arrange
            var transactionRepository = _serviceProvider.GetRequiredService<ITransactionRepository>();
            var transactionDetailRepository = _serviceProvider.GetRequiredService<ITransactionDetailRepository>();
            var txnpostService = _serviceProvider.GetRequiredService<ITxnpostService>();

            var transaction = new TxnpostDomainModels.Transaction
            {
                TransactionId = "TXN123456789",
                AccountNumber = "ACC1234567",
                TransactionAmount = 500.00m,
                TransactionDate = "20231010",
                TransactionType = "D",
                Status = TxnpostDomainModels.TransactionStatus.Pending,
                Details = new List<TxnpostDomainModels.TransactionDetail>
                {
                    new TxnpostDomainModels.TransactionDetail
                    {
                        DetailId = "DET123456789",
                        DetailAmount = 500.00m,
                        Description = "Test Transaction Detail"
                    }
                }
            };

            await transactionRepository.AddTransactionAsync(transaction);

            // Act
            await txnpostService.ExecuteAsync();

            // Assert
            var processedTransaction = await transactionRepository.GetTransactionByIdAsync("TXN123456789");
            processedTransaction.Should().NotBeNull();
            processedTransaction!.Status.Should().Be(TxnpostDomainModels.TransactionStatus.Completed);

            var transactionDetails = await transactionDetailRepository.GetTransactionDetailsByTransactionIdAsync("TXN123456789");
            transactionDetails.Should().HaveCount(1);
            transactionDetails[0].DetailId.Should().Be("DET123456789");
            transactionDetails[0].DetailAmount.Should().Be(500.00m);
            transactionDetails[0].Description.Should().Be("Test Transaction Detail");
        }
    }
}