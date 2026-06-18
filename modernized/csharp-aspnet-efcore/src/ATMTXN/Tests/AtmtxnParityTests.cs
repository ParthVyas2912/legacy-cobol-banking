using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ATMTXN.Application;
using ATMTXN.Application.Interfaces;
using ATMTXN.Domain;

namespace ATMTXN.Tests
{
    public class AtmtxnParityTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<ICicsService> _cicsServiceMock;
        private readonly AtmtxnService _atmtxnService;

        public AtmtxnParityTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _cicsServiceMock = new Mock<ICicsService>();
            _atmtxnService = new AtmtxnService(_transactionRepositoryMock.Object, _cicsServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldCompleteSuccessfully()
        {
            // Arrange
            var transaction = new AtmtxnDomainModels.Transaction
            {
                TransactionId = "TXN123456789",
                AccountNumber = "1234567890",
                TransactionAmount = 100.50m,
                TransactionDate = "230101",
                TransactionTime = "120000",
                TransactionType = AtmtxnDomainModels.TransactionTypeEnum.Withdrawal,
                TransactionStatus = AtmtxnDomainModels.TransactionStatusEnum.Pending,
                Details = new AtmtxnDomainModels.Transaction.TransactionDetails
                {
                    MerchantName = "Test Merchant",
                    MerchantId = "MERCHANT12345",
                    MerchantAmount = 100.50m
                }
            };

            _transactionRepositoryMock.Setup(repo => repo.GetTransactionByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(transaction);

            _cicsServiceMock.Setup(service => service.ProcessTransactionAsync(It.IsAny<AtmtxnDomainModels.Transaction>()))
                .ReturnsAsync(AtmtxnDomainModels.TransactionStatusEnum.Completed);

            _transactionRepositoryMock.Setup(repo => repo.UpdateTransactionStatusAsync(It.IsAny<string>(), It.IsAny<AtmtxnDomainModels.TransactionStatusEnum>()))
                .Returns(Task.CompletedTask);

            // Act
            await _atmtxnService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.GetTransactionByIdAsync(It.IsAny<string>()), Times.Once);
            _cicsServiceMock.Verify(service => service.ProcessTransactionAsync(It.IsAny<AtmtxnDomainModels.Transaction>()), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.UpdateTransactionStatusAsync(It.IsAny<string>(), AtmtxnDomainModels.TransactionStatusEnum.Completed), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleTransactionNotFound()
        {
            // Arrange
            _transactionRepositoryMock.Setup(repo => repo.GetTransactionByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((AtmtxnDomainModels.Transaction)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _atmtxnService.ExecuteAsync());
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleCicsServiceFailure()
        {
            // Arrange
            var transaction = new AtmtxnDomainModels.Transaction
            {
                TransactionId = "TXN123456789",
                AccountNumber = "1234567890",
                TransactionAmount = 100.50m,
                TransactionDate = "230101",
                TransactionTime = "120000",
                TransactionType = AtmtxnDomainModels.TransactionTypeEnum.Withdrawal,
                TransactionStatus = AtmtxnDomainModels.TransactionStatusEnum.Pending,
                Details = new AtmtxnDomainModels.Transaction.TransactionDetails
                {
                    MerchantName = "Test Merchant",
                    MerchantId = "MERCHANT12345",
                    MerchantAmount = 100.50m
                }
            };

            _transactionRepositoryMock.Setup(repo => repo.GetTransactionByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(transaction);

            _cicsServiceMock.Setup(service => service.ProcessTransactionAsync(It.IsAny<AtmtxnDomainModels.Transaction>()))
                .ReturnsAsync(AtmtxnDomainModels.TransactionStatusEnum.Failed);

            _transactionRepositoryMock.Setup(repo => repo.UpdateTransactionStatusAsync(It.IsAny<string>(), It.IsAny<AtmtxnDomainModels.TransactionStatusEnum>()))
                .Returns(Task.CompletedTask);

            // Act
            await _atmtxnService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.GetTransactionByIdAsync(It.IsAny<string>()), Times.Once);
            _cicsServiceMock.Verify(service => service.ProcessTransactionAsync(It.IsAny<AtmtxnDomainModels.Transaction>()), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.UpdateTransactionStatusAsync(It.IsAny<string>(), AtmtxnDomainModels.TransactionStatusEnum.Failed), Times.Once);
        }
    }
}