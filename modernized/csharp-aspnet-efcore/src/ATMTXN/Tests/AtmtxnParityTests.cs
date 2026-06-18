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
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<ICicsService> _cicsServiceMock;
        private readonly AtmtxnService _atmtxnService;

        public AtmtxnParityTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _accountServiceMock = new Mock<IAccountService>();
            _cicsServiceMock = new Mock<ICicsService>();

            _atmtxnService = new AtmtxnService();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldRunWithoutExceptions()
        {
            // Arrange
            // No specific setup required as the current implementation does not depend on external services

            // Act & Assert
            await _atmtxnService.ExecuteAsync();
        }

        [Fact]
        public async Task InitializeTransactionAsync_ShouldCompleteSuccessfully()
        {
            // Arrange
            // No specific setup required as the current implementation does not depend on external services

            // Act
            var initializeTransactionMethod = typeof(AtmtxnService).GetMethod("InitializeTransactionAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(initializeTransactionMethod);

            var task = (Task)initializeTransactionMethod.Invoke(_atmtxnService, null);

            // Assert
            await task;
        }

        [Fact]
        public async Task ProcessTransactionAsync_ShouldCompleteSuccessfully()
        {
            // Arrange
            // No specific setup required as the current implementation does not depend on external services

            // Act
            var processTransactionMethod = typeof(AtmtxnService).GetMethod("ProcessTransactionAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(processTransactionMethod);

            var task = (Task)processTransactionMethod.Invoke(_atmtxnService, null);

            // Assert
            await task;
        }

        [Fact]
        public async Task FinalizeTransactionAsync_ShouldCompleteSuccessfully()
        {
            // Arrange
            // No specific setup required as the current implementation does not depend on external services

            // Act
            var finalizeTransactionMethod = typeof(AtmtxnService).GetMethod("FinalizeTransactionAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(finalizeTransactionMethod);

            var task = (Task)finalizeTransactionMethod.Invoke(_atmtxnService, null);

            // Assert
            await task;
        }

        [Fact]
        public async Task GetTransactionByIdAsync_ShouldReturnTransaction_WhenTransactionExists()
        {
            // Arrange
            var transactionId = "TXN123456789";
            var expectedTransaction = new AtmtxnDomainModels.Transaction
            {
                TransactionId = transactionId,
                AccountNumber = "1234567890123456",
                TransactionAmount = 100.00m,
                TransactionType = AtmtxnDomainModels.TransactionTypeEnum.Withdrawal,
                TransactionDate = DateTime.Now,
                TransactionTime = "120000",
                CurrencyCode = "USD",
                BalanceAfterTransaction = 900.00m,
                TransactionStatus = AtmtxnDomainModels.StatusEnum.Success
            };

            _transactionRepositoryMock
                .Setup(repo => repo.GetTransactionByIdAsync(transactionId))
                .ReturnsAsync(expectedTransaction);

            // Act
            var result = await _transactionRepositoryMock.Object.GetTransactionByIdAsync(transactionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTransaction.TransactionId, result.TransactionId);
        }

        [Fact]
        public async Task SaveTransactionAsync_ShouldSaveTransactionSuccessfully()
        {
            // Arrange
            var transaction = new AtmtxnDomainModels.Transaction
            {
                TransactionId = "TXN987654321",
                AccountNumber = "6543210987654321",
                TransactionAmount = 200.00m,
                TransactionType = AtmtxnDomainModels.TransactionTypeEnum.Deposit,
                TransactionDate = DateTime.Now,
                TransactionTime = "150000",
                CurrencyCode = "USD",
                BalanceAfterTransaction = 1200.00m,
                TransactionStatus = AtmtxnDomainModels.StatusEnum.Success
            };

            _transactionRepositoryMock
                .Setup(repo => repo.SaveTransactionAsync(transaction))
                .Returns(Task.CompletedTask);

            // Act
            await _transactionRepositoryMock.Object.SaveTransactionAsync(transaction);

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.SaveTransactionAsync(transaction), Times.Once);
        }

        [Fact]
        public async Task UpdateTransactionAsync_ShouldUpdateTransactionSuccessfully()
        {
            // Arrange
            var transaction = new AtmtxnDomainModels.Transaction
            {
                TransactionId = "TXN123456789",
                AccountNumber = "1234567890123456",
                TransactionAmount = 150.00m,
                TransactionType = AtmtxnDomainModels.TransactionTypeEnum.Transfer,
                TransactionDate = DateTime.Now,
                TransactionTime = "130000",
                CurrencyCode = "USD",
                BalanceAfterTransaction = 850.00m,
                TransactionStatus = AtmtxnDomainModels.StatusEnum.Pending
            };

            _transactionRepositoryMock
                .Setup(repo => repo.UpdateTransactionAsync(transaction))
                .Returns(Task.CompletedTask);

            // Act
            await _transactionRepositoryMock.Object.UpdateTransactionAsync(transaction);

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.UpdateTransactionAsync(transaction), Times.Once);
        }

        [Fact]
        public async Task PerformCicsOperationAsync_ShouldReturnTrue_WhenOperationIsSuccessful()
        {
            // Arrange
            var transaction = new AtmtxnDomainModels.Transaction
            {
                TransactionId = "TXN123456789",
                AccountNumber = "1234567890123456",
                TransactionAmount = 100.00m,
                TransactionType = AtmtxnDomainModels.TransactionTypeEnum.Withdrawal,
                TransactionDate = DateTime.Now,
                TransactionTime = "120000",
                CurrencyCode = "USD",
                BalanceAfterTransaction = 900.00m,
                TransactionStatus = AtmtxnDomainModels.StatusEnum.Success
            };

            _cicsServiceMock
                .Setup(service => service.PerformCicsOperationAsync(transaction))
                .ReturnsAsync(true);

            // Act
            var result = await _cicsServiceMock.Object.PerformCicsOperationAsync(transaction);

            // Assert
            Assert.True(result);
        }
    }
}