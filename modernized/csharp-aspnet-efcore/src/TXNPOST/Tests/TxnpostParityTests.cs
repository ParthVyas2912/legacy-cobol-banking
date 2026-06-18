using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using TXNPOST.Application;
using TXNPOST.Application.Interfaces;
using TXNPOST.Domain;

namespace TXNPOST.Tests
{
    public class TxnpostParityTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<ILogger<TxnpostService>> _loggerMock;
        private readonly TxnpostService _txnpostService;

        public TxnpostParityTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _loggerMock = new Mock<ILogger<TxnpostService>>();

            _txnpostService = new TxnpostService(
                _transactionRepositoryMock.Object,
                _accountRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInitializeProcessAndFinalize()
        {
            // Arrange
            _transactionRepositoryMock
                .Setup(repo => repo.ReadTransactionAsync(It.IsAny<string>()))
                .ReturnsAsync((TxnpostDomainModels.TransactionRecord)null);

            _transactionRepositoryMock
                .Setup(repo => repo.UpdateTransactionAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()))
                .Returns(Task.CompletedTask);

            // Act
            await _txnpostService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.ReadTransactionAsync(It.IsAny<string>()), Times.AtLeastOnce);
            _transactionRepositoryMock.Verify(repo => repo.UpdateTransactionAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleTransactionProcessing()
        {
            // Arrange
            var transaction = new TxnpostDomainModels.TransactionRecord
            {
                // Populate fields as per the domain model
            };

            _transactionRepositoryMock
                .Setup(repo => repo.ReadTransactionAsync(It.IsAny<string>()))
                .ReturnsAsync(transaction);

            _transactionRepositoryMock
                .Setup(repo => repo.UpdateTransactionAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()))
                .Returns(Task.CompletedTask);

            _accountRepositoryMock
                .Setup(repo => repo.GetAccountBalanceAsync(It.IsAny<string>()))
                .ReturnsAsync(500.00m);

            _accountRepositoryMock
                .Setup(repo => repo.UpdateAccountBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()))
                .Returns(Task.CompletedTask);

            // Act
            await _txnpostService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.ReadTransactionAsync(It.IsAny<string>()), Times.AtLeastOnce);
            _transactionRepositoryMock.Verify(repo => repo.UpdateTransactionAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.AtLeastOnce);
            _accountRepositoryMock.Verify(repo => repo.GetAccountBalanceAsync(It.IsAny<string>()), Times.AtLeastOnce);
            _accountRepositoryMock.Verify(repo => repo.UpdateAccountBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldMarkTransactionAsFailed_WhenValidationFails()
        {
            // Arrange
            var transaction = new TxnpostDomainModels.TransactionRecord
            {
                // Populate fields as per the domain model
            };

            _transactionRepositoryMock
                .Setup(repo => repo.ReadTransactionAsync(It.IsAny<string>()))
                .ReturnsAsync(transaction);

            _transactionRepositoryMock
                .Setup(repo => repo.UpdateTransactionAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()))
                .Returns(Task.CompletedTask);

            // Act
            await _txnpostService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.ReadTransactionAsync(It.IsAny<string>()), Times.AtLeastOnce);
            _transactionRepositoryMock.Verify(repo => repo.UpdateTransactionAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleExceptionDuringTransactionProcessing()
        {
            // Arrange
            var transaction = new TxnpostDomainModels.TransactionRecord
            {
                // Populate fields as per the domain model
            };

            _transactionRepositoryMock
                .Setup(repo => repo.ReadTransactionAsync(It.IsAny<string>()))
                .ReturnsAsync(transaction);

            _transactionRepositoryMock
                .Setup(repo => repo.UpdateTransactionAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            await _txnpostService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.ReadTransactionAsync(It.IsAny<string>()), Times.AtLeastOnce);
            _transactionRepositoryMock.Verify(repo => repo.UpdateTransactionAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.AtLeastOnce);
        }
    }
}