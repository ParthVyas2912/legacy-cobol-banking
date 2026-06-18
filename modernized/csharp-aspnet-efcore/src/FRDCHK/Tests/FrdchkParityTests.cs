using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FRDCHK.Application;
using FRDCHK.Application.Interfaces;
using FRDCHK.Domain;

namespace FRDCHK.Tests
{
    public class FrdchkParityTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IItemDetailRepository> _itemDetailRepositoryMock;
        private readonly Mock<ICicsFraudCheckService> _fraudCheckServiceMock;
        private readonly FrdchkService _frdchkService;

        public FrdchkParityTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _itemDetailRepositoryMock = new Mock<IItemDetailRepository>();
            _fraudCheckServiceMock = new Mock<ICicsFraudCheckService>();

            _frdchkService = new FrdchkService(
                _transactionRepositoryMock.Object,
                _itemDetailRepositoryMock.Object,
                _fraudCheckServiceMock.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInvokeMainProcessing()
        {
            // Arrange
            var pendingTransactions = new List<FrdchkDomainModels.TransactionRecord>
            {
                new FrdchkDomainModels.TransactionRecord
                {
                    TransactionId = "TXN001",
                    AccountNumber = "1234567890",
                    TransactionAmount = 1000.00m,
                    Status = FrdchkDomainModels.TransactionStatus.Pending,
                    TransactionDate = DateTime.Parse("2023-10-01"),
                    ItemDetails = new List<FrdchkDomainModels.ItemDetail>
                    {
                        new FrdchkDomainModels.ItemDetail
                        {
                            ItemCode = "ITEM001",
                            ItemAmount = 500.00m,
                            Quantity = 2
                        }
                    }
                }
            };

            _transactionRepositoryMock
                .Setup(repo => repo.GetPendingTransactionsAsync())
                .ReturnsAsync(pendingTransactions);

            _fraudCheckServiceMock
                .Setup(service => service.PerformFraudCheckAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()))
                .ReturnsAsync(true);

            // Act
            await _frdchkService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.GetPendingTransactionsAsync(), Times.Once);
            _fraudCheckServiceMock.Verify(service => service.PerformFraudCheckAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleFraudulentTransaction()
        {
            // Arrange
            var fraudulentTransaction = new FrdchkDomainModels.TransactionRecord
            {
                TransactionId = "TXN002",
                AccountNumber = "9876543210",
                TransactionAmount = 5000.00m,
                Status = FrdchkDomainModels.TransactionStatus.Pending,
                TransactionDate = DateTime.Parse("2023-10-02"),
                ItemDetails = new List<FrdchkDomainModels.ItemDetail>
                {
                    new FrdchkDomainModels.ItemDetail
                    {
                        ItemCode = "ITEM002",
                        ItemAmount = 2500.00m,
                        Quantity = 2
                    }
                }
            };

            var pendingTransactions = new List<FrdchkDomainModels.TransactionRecord> { fraudulentTransaction };

            _transactionRepositoryMock
                .Setup(repo => repo.GetPendingTransactionsAsync())
                .ReturnsAsync(pendingTransactions);

            _fraudCheckServiceMock
                .Setup(service => service.PerformFraudCheckAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()))
                .ReturnsAsync(false);

            _transactionRepositoryMock
                .Setup(repo => repo.UpdateTransactionStatusAsync(fraudulentTransaction.TransactionId, FrdchkDomainModels.TransactionStatus.Rejected))
                .Returns(Task.CompletedTask);

            // Act
            await _frdchkService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.GetPendingTransactionsAsync(), Times.Once);
            _fraudCheckServiceMock.Verify(service => service.PerformFraudCheckAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.UpdateTransactionStatusAsync(fraudulentTransaction.TransactionId, FrdchkDomainModels.TransactionStatus.Rejected), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessValidTransaction()
        {
            // Arrange
            var validTransaction = new FrdchkDomainModels.TransactionRecord
            {
                TransactionId = "TXN003",
                AccountNumber = "1122334455",
                TransactionAmount = 2000.00m,
                Status = FrdchkDomainModels.TransactionStatus.Pending,
                TransactionDate = DateTime.Parse("2023-10-03"),
                ItemDetails = new List<FrdchkDomainModels.ItemDetail>
                {
                    new FrdchkDomainModels.ItemDetail
                    {
                        ItemCode = "ITEM003",
                        ItemAmount = 1000.00m,
                        Quantity = 2
                    }
                }
            };

            var pendingTransactions = new List<FrdchkDomainModels.TransactionRecord> { validTransaction };

            _transactionRepositoryMock
                .Setup(repo => repo.GetPendingTransactionsAsync())
                .ReturnsAsync(pendingTransactions);

            _fraudCheckServiceMock
                .Setup(service => service.PerformFraudCheckAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()))
                .ReturnsAsync(true);

            _transactionRepositoryMock
                .Setup(repo => repo.UpdateTransactionStatusAsync(validTransaction.TransactionId, FrdchkDomainModels.TransactionStatus.Completed))
                .Returns(Task.CompletedTask);

            // Act
            await _frdchkService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.GetPendingTransactionsAsync(), Times.Once);
            _fraudCheckServiceMock.Verify(service => service.PerformFraudCheckAsync(It.IsAny<FrdchkDomainModels.TransactionRecord>()), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.UpdateTransactionStatusAsync(validTransaction.TransactionId, FrdchkDomainModels.TransactionStatus.Completed), Times.Once);
        }
    }
}