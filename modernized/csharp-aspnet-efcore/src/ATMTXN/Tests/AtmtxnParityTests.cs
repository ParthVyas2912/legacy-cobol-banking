using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Mock<ILoggingService> _loggingServiceMock;
        private readonly AtmtxnService _atmtxnService;

        public AtmtxnParityTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _loggingServiceMock = new Mock<ILoggingService>();
            _atmtxnService = new AtmtxnService(_transactionRepositoryMock.Object, _loggingServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInitializeProcessAndFinalizeTransaction()
        {
            // Arrange
            _transactionRepositoryMock.Setup(repo => repo.ReadAllTransactionsAsync())
                .ReturnsAsync(new List<AtmtxnDomainModels.TransactionRecord>
                {
                    new AtmtxnDomainModels.TransactionRecord
                    {
                        TransactionId = "TXN123456789",
                        AccountNumber = "1234567890123456",
                        TransactionType = "DEP",
                        TransactionAmount = 1000.00m,
                        TransactionDate = "20231010",
                        TransactionTime = "123456",
                        Status = AtmtxnDomainModels.TransactionStatus.Pending,
                        Location = new AtmtxnDomainModels.TransactionRecord.LocationDetails
                        {
                            City = "New York",
                            State = "NY",
                            CountryCode = "US"
                        }
                    }
                });

            _transactionRepositoryMock.Setup(repo => repo.WriteTransactionAsync(It.IsAny<AtmtxnDomainModels.TransactionRecord>()))
                .Returns(Task.CompletedTask);

            // Act
            await _atmtxnService.ExecuteAsync();

            // Assert
            _loggingServiceMock.Verify(log => log.LogInfo("Initializing transaction..."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogInfo("Transaction initialized successfully."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogInfo("Processing transaction..."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogInfo("Transaction processed successfully."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogInfo("Finalizing transaction..."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogInfo("Transaction finalized successfully."), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleErrorDuringInitialization()
        {
            // Arrange
            _transactionRepositoryMock.Setup(repo => repo.ReadAllTransactionsAsync())
                .ThrowsAsync(new InvalidOperationException("Initialization failed"));

            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(() => _atmtxnService.ExecuteAsync());

            // Assert
            _loggingServiceMock.Verify(log => log.LogInfo("Initializing transaction..."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogError("An error occurred during transaction processing.", It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleErrorDuringProcessing()
        {
            // Arrange
            _transactionRepositoryMock.Setup(repo => repo.ReadAllTransactionsAsync())
                .ReturnsAsync(new List<AtmtxnDomainModels.TransactionRecord>
                {
                    new AtmtxnDomainModels.TransactionRecord
                    {
                        TransactionId = "TXN123456789",
                        AccountNumber = "1234567890123456",
                        TransactionType = "DEP",
                        TransactionAmount = -1000.00m, // Invalid amount
                        TransactionDate = "20231010",
                        TransactionTime = "123456",
                        Status = AtmtxnDomainModels.TransactionStatus.Pending,
                        Location = new AtmtxnDomainModels.TransactionRecord.LocationDetails
                        {
                            City = "New York",
                            State = "NY",
                            CountryCode = "US"
                        }
                    }
                });

            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(() => _atmtxnService.ExecuteAsync());

            // Assert
            _loggingServiceMock.Verify(log => log.LogInfo("Initializing transaction..."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogInfo("Transaction initialized successfully."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogInfo("Processing transaction..."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogError("An error occurred during transaction processing.", It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleErrorDuringFinalization()
        {
            // Arrange
            _transactionRepositoryMock.Setup(repo => repo.ReadAllTransactionsAsync())
                .ReturnsAsync(new List<AtmtxnDomainModels.TransactionRecord>
                {
                    new AtmtxnDomainModels.TransactionRecord
                    {
                        TransactionId = "TXN123456789",
                        AccountNumber = "1234567890123456",
                        TransactionType = "DEP",
                        TransactionAmount = 1000.00m,
                        TransactionDate = "20231010",
                        TransactionTime = "123456",
                        Status = AtmtxnDomainModels.TransactionStatus.Pending,
                        Location = new AtmtxnDomainModels.TransactionRecord.LocationDetails
                        {
                            City = "New York",
                            State = "NY",
                            CountryCode = "US"
                        }
                    }
                });

            _transactionRepositoryMock.Setup(repo => repo.WriteTransactionAsync(It.IsAny<AtmtxnDomainModels.TransactionRecord>()))
                .ThrowsAsync(new InvalidOperationException("Finalization failed"));

            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(() => _atmtxnService.ExecuteAsync());

            // Assert
            _loggingServiceMock.Verify(log => log.LogInfo("Initializing transaction..."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogInfo("Transaction initialized successfully."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogInfo("Processing transaction..."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogInfo("Transaction processed successfully."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogInfo("Finalizing transaction..."), Times.Once);
            _loggingServiceMock.Verify(log => log.LogError("An error occurred during transaction processing.", It.IsAny<Exception>()), Times.Once);
        }
    }

    public class OperationResult
    {
        public bool IsSuccess { get; set; }
    }

    public interface ILoggingService
    {
        void LogInfo(string message);
        void LogError(string message, Exception exception = null);
    }
}