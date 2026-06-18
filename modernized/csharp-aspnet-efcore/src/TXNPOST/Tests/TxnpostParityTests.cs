using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TXNPOST.Application;
using TXNPOST.Application.Interfaces;
using TXNPOST.Domain;
using Xunit;

namespace TXNPOST.Tests
{
    public class TxnpostParityTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<ITransactionLogService> _transactionLogServiceMock;
        private readonly Mock<ICicsService> _cicsServiceMock;
        private readonly TxnpostService _txnpostService;

        public TxnpostParityTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _transactionLogServiceMock = new Mock<ITransactionLogService>();
            _cicsServiceMock = new Mock<ICicsService>();

            _txnpostService = new TxnpostService();
        }

        [Fact]
        public async Task ExecuteAsync_Should_ProcessTransactions()
        {
            // Arrange
            var pendingTransactions = new List<TxnpostDomainModels.TransactionRecord>
            {
                new TxnpostDomainModels.TransactionRecord
                {
                    TransactionId = "TXN001",
                    AccountNumber = "1234567890",
                    TransactionAmount = 100.00m,
                    TransactionType = TxnpostDomainModels.TransactionTypeEnum.Credit,
                    TransactionDate = "20231010",
                    Status = TxnpostDomainModels.StatusEnum.Pending
                },
                new TxnpostDomainModels.TransactionRecord
                {
                    TransactionId = "TXN002",
                    AccountNumber = "0987654321",
                    TransactionAmount = 50.00m,
                    TransactionType = TxnpostDomainModels.TransactionTypeEnum.Debit,
                    TransactionDate = "20231011",
                    Status = TxnpostDomainModels.StatusEnum.Pending
                }
            };

            _transactionRepositoryMock
                .Setup(repo => repo.GetPendingTransactionsAsync())
                .ReturnsAsync(pendingTransactions);

            _accountRepositoryMock
                .Setup(repo => repo.GetAccountBalanceAsync(It.IsAny<string>()))
                .ReturnsAsync(1000.00m);

            _cicsServiceMock
                .Setup(service => service.PostTransactionToCicsAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()))
                .ReturnsAsync(true);

            // Act
            await _txnpostService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.GetPendingTransactionsAsync(), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.SaveTransactionAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.Exactly(pendingTransactions.Count));
            _accountRepositoryMock.Verify(repo => repo.GetAccountBalanceAsync(It.IsAny<string>()), Times.Exactly(pendingTransactions.Count));
            _accountRepositoryMock.Verify(repo => repo.UpdateAccountBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()), Times.Exactly(pendingTransactions.Count));
            _transactionLogServiceMock.Verify(service => service.LogTransactionAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.Exactly(pendingTransactions.Count));
            _cicsServiceMock.Verify(service => service.PostTransactionToCicsAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.Exactly(pendingTransactions.Count));
        }

        [Fact]
        public async Task ExecuteAsync_Should_Handle_NoPendingTransactions()
        {
            // Arrange
            _transactionRepositoryMock
                .Setup(repo => repo.GetPendingTransactionsAsync())
                .ReturnsAsync(new List<TxnpostDomainModels.TransactionRecord>());

            // Act
            await _txnpostService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.GetPendingTransactionsAsync(), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.SaveTransactionAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.Never);
            _accountRepositoryMock.Verify(repo => repo.GetAccountBalanceAsync(It.IsAny<string>()), Times.Never);
            _accountRepositoryMock.Verify(repo => repo.UpdateAccountBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
            _transactionLogServiceMock.Verify(service => service.LogTransactionAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.Never);
            _cicsServiceMock.Verify(service => service.PostTransactionToCicsAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Handle_FailedCicsTransaction()
        {
            // Arrange
            var transaction = new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = "TXN003",
                AccountNumber = "1234567890",
                TransactionAmount = 200.00m,
                TransactionType = TxnpostDomainModels.TransactionTypeEnum.Debit,
                TransactionDate = "20231012",
                Status = TxnpostDomainModels.StatusEnum.Pending
            };

            _transactionRepositoryMock
                .Setup(repo => repo.GetPendingTransactionsAsync())
                .ReturnsAsync(new List<TxnpostDomainModels.TransactionRecord> { transaction });

            _accountRepositoryMock
                .Setup(repo => repo.GetAccountBalanceAsync(It.IsAny<string>()))
                .ReturnsAsync(500.00m);

            _cicsServiceMock
                .Setup(service => service.PostTransactionToCicsAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()))
                .ReturnsAsync(false);

            // Act
            await _txnpostService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.GetPendingTransactionsAsync(), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.SaveTransactionAsync(It.Is<TxnpostDomainModels.TransactionRecord>(t => t.Status == TxnpostDomainModels.StatusEnum.Failed)), Times.Once);
            _accountRepositoryMock.Verify(repo => repo.GetAccountBalanceAsync(It.IsAny<string>()), Times.Once);
            _accountRepositoryMock.Verify(repo => repo.UpdateAccountBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
            _transactionLogServiceMock.Verify(service => service.LogTransactionAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.Once);
            _cicsServiceMock.Verify(service => service.PostTransactionToCicsAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.Once);
        }
    }
}