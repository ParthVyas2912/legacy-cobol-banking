using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FRDCHK.Application;
using FRDCHK.Application.Interfaces;
using FRDCHK.Domain;
using Moq;
using Xunit;

namespace FRDCHK.Tests
{
    public class FrdchkParityTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<ICicsService> _cicsServiceMock;
        private readonly FrdchkService _frdchkService;

        public FrdchkParityTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _cicsServiceMock = new Mock<ICicsService>();
            _frdchkService = new FrdchkService(_transactionRepositoryMock.Object, _cicsServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInitializePerformFraudCheckAndFinalize()
        {
            // Arrange
            var transactions = new List<FrdchkDomainModels.TransactionRecord>
            {
                new FrdchkDomainModels.TransactionRecord
                {
                    TransactionId = "TXN001",
                    TransactionDate = DateTime.UtcNow,
                    TransactionAmount = 5000,
                    TransactionStatus = FrdchkDomainModels.TransactionStatusEnum.Pending,
                    Details = new FrdchkDomainModels.TransactionRecord.TransactionDetails
                    {
                        Description = "Test Transaction",
                        MerchantCode = "M123",
                        ItemCount = 1
                    }
                }
            };

            _transactionRepositoryMock.Setup(repo => repo.GetPendingTransactionsAsync())
                .ReturnsAsync(transactions);

            _cicsServiceMock.Setup(service => service.ValidateTransactionAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()))
                .ReturnsAsync(true);

            _transactionRepositoryMock.Setup(repo => repo.GetRecentTransactionsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<FrdchkDomainModels.TransactionRecord>());

            _cicsServiceMock.Setup(service => service.UpdateTransactionStatusAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()))
                .Returns(Task.CompletedTask);

            // Act
            await _frdchkService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.GetPendingTransactionsAsync(), Times.Once);
            _cicsServiceMock.Verify(service => service.ValidateTransactionAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.GetRecentTransactionsAsync(It.IsAny<string>()), Times.Once);
            _cicsServiceMock.Verify(service => service.UpdateTransactionStatusAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()), Times.Once);
        }

        [Fact]
        public async Task PerformFraudCheck_ShouldFlagFraudulentTransaction_WhenValidationFails()
        {
            // Arrange
            var transaction = new FrdchkDomainModels.TransactionRecord
            {
                TransactionId = "TXN002",
                TransactionDate = DateTime.UtcNow,
                TransactionAmount = 2000,
                TransactionStatus = FrdchkDomainModels.TransactionStatusEnum.Pending,
                Details = new FrdchkDomainModels.TransactionRecord.TransactionDetails
                {
                    Description = "Invalid Transaction",
                    MerchantCode = "M456",
                    ItemCount = 2
                }
            };

            _transactionRepositoryMock.Setup(repo => repo.GetPendingTransactionsAsync())
                .ReturnsAsync(new List<FrdchkDomainModels.TransactionRecord> { transaction });

            _cicsServiceMock.Setup(service => service.ValidateTransactionAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()))
                .ReturnsAsync(false);

            _transactionRepositoryMock.Setup(repo => repo.FlagFraudulentTransactionAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _frdchkService.ExecuteAsync();

            // Assert
            _cicsServiceMock.Verify(service => service.ValidateTransactionAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.FlagFraudulentTransactionAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PerformVelocityCheck_ShouldFlagFraudulentTransaction_WhenVelocityCheckFails()
        {
            // Arrange
            var transaction = new FrdchkDomainModels.TransactionRecord
            {
                TransactionId = "TXN003",
                TransactionDate = DateTime.UtcNow,
                TransactionAmount = 8000,
                TransactionStatus = FrdchkDomainModels.TransactionStatusEnum.Pending,
                Details = new FrdchkDomainModels.TransactionRecord.TransactionDetails
                {
                    Description = "High Velocity Transaction",
                    MerchantCode = "M789",
                    ItemCount = 5
                }
            };

            var recentTransactions = new List<FrdchkDomainModels.TransactionRecord>
            {
                new FrdchkDomainModels.TransactionRecord { TransactionAmount = 3000 },
                new FrdchkDomainModels.TransactionRecord { TransactionAmount = 4000 }
            };

            _transactionRepositoryMock.Setup(repo => repo.GetPendingTransactionsAsync())
                .ReturnsAsync(new List<FrdchkDomainModels.TransactionRecord> { transaction });

            _cicsServiceMock.Setup(service => service.ValidateTransactionAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()))
                .ReturnsAsync(true);

            _transactionRepositoryMock.Setup(repo => repo.GetRecentTransactionsAsync(It.IsAny<string>()))
                .ReturnsAsync(recentTransactions);

            _transactionRepositoryMock.Setup(repo => repo.FlagFraudulentTransactionAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _frdchkService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.GetRecentTransactionsAsync(It.IsAny<string>()), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.FlagFraudulentTransactionAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task PerformFraudCheck_ShouldUpdateTransactionStatus_WhenTransactionIsValid()
        {
            // Arrange
            var transaction = new FrdchkDomainModels.TransactionRecord
            {
                TransactionId = "TXN004",
                TransactionDate = DateTime.UtcNow,
                TransactionAmount = 1000,
                TransactionStatus = FrdchkDomainModels.TransactionStatusEnum.Pending,
                Details = new FrdchkDomainModels.TransactionRecord.TransactionDetails
                {
                    Description = "Valid Transaction",
                    MerchantCode = "M123",
                    ItemCount = 1
                }
            };

            _transactionRepositoryMock.Setup(repo => repo.GetPendingTransactionsAsync())
                .ReturnsAsync(new List<FrdchkDomainModels.TransactionRecord> { transaction });

            _cicsServiceMock.Setup(service => service.ValidateTransactionAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()))
                .ReturnsAsync(true);

            _transactionRepositoryMock.Setup(repo => repo.GetRecentTransactionsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<FrdchkDomainModels.TransactionRecord>());

            _cicsServiceMock.Setup(service => service.UpdateTransactionStatusAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()))
                .Returns(Task.CompletedTask);

            // Act
            await _frdchkService.ExecuteAsync();

            // Assert
            _cicsServiceMock.Verify(service => service.ValidateTransactionAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.GetRecentTransactionsAsync(It.IsAny<string>()), Times.Once);
            _cicsServiceMock.Verify(service => service.UpdateTransactionStatusAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()), Times.Once);
        }
    }
}