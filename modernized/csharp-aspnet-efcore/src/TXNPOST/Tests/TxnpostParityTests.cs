using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TXNPOST.Application.Interfaces;
using TXNPOST.Domain;

namespace TXNPOST.Tests
{
    public class TxnpostParityTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<ICicsService> _cicsServiceMock;
        private readonly ITxnpostService _txnpostService;

        public TxnpostParityTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _cicsServiceMock = new Mock<ICicsService>();
            _txnpostService = new TxnpostService(_transactionRepositoryMock.Object, _cicsServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Process_Valid_Transaction()
        {
            // Arrange
            var transactionId = "TXN1234567";
            var transactionRecord = new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = transactionId,
                AccountNumber = "1234567890",
                TransactionAmount = 1000.00m,
                TransactionDate = DateTime.Parse("2023-10-01"),
                TransactionType = TxnpostDomainModels.TransactionTypeEnum.Credit,
                Status = TxnpostDomainModels.StatusEnum.Pending,
                Details = new TxnpostDomainModels.TransactionDetails
                {
                    Description = "Test Transaction",
                    CurrencyCode = "USD",
                    ExchangeRate = 1.0000m
                }
            };

            _transactionRepositoryMock
                .Setup(repo => repo.ReadTransactionAsync(transactionId))
                .ReturnsAsync(transactionRecord);

            _cicsServiceMock
                .Setup(service => service.ValidateAccountAsync(transactionRecord.AccountNumber))
                .ReturnsAsync(true);

            _cicsServiceMock
                .Setup(service => service.PostToLedgerAsync(transactionRecord))
                .ReturnsAsync(true);

            _cicsServiceMock
                .Setup(service => service.NotifyExternalSystemAsync(transactionRecord))
                .ReturnsAsync(true);

            _transactionRepositoryMock
                .Setup(repo => repo.UpdateTransactionStatusAsync(transactionId, TxnpostDomainModels.StatusEnum.Completed))
                .Returns(Task.CompletedTask);

            // Act
            await _txnpostService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.ReadTransactionAsync(transactionId), Times.Once);
            _cicsServiceMock.Verify(service => service.ValidateAccountAsync(transactionRecord.AccountNumber), Times.Once);
            _cicsServiceMock.Verify(service => service.PostToLedgerAsync(transactionRecord), Times.Once);
            _cicsServiceMock.Verify(service => service.NotifyExternalSystemAsync(transactionRecord), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.UpdateTransactionStatusAsync(transactionId, TxnpostDomainModels.StatusEnum.Completed), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Handle_Invalid_Account()
        {
            // Arrange
            var transactionId = "TXN1234567";
            var transactionRecord = new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = transactionId,
                AccountNumber = "INVALID123",
                TransactionAmount = 1000.00m,
                TransactionDate = DateTime.Parse("2023-10-01"),
                TransactionType = TxnpostDomainModels.TransactionTypeEnum.Credit,
                Status = TxnpostDomainModels.StatusEnum.Pending,
                Details = new TxnpostDomainModels.TransactionDetails
                {
                    Description = "Test Transaction",
                    CurrencyCode = "USD",
                    ExchangeRate = 1.0000m
                }
            };

            _transactionRepositoryMock
                .Setup(repo => repo.ReadTransactionAsync(transactionId))
                .ReturnsAsync(transactionRecord);

            _cicsServiceMock
                .Setup(service => service.ValidateAccountAsync(transactionRecord.AccountNumber))
                .ReturnsAsync(false);

            _transactionRepositoryMock
                .Setup(repo => repo.UpdateTransactionStatusAsync(transactionId, TxnpostDomainModels.StatusEnum.Failed))
                .Returns(Task.CompletedTask);

            // Act
            await _txnpostService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.ReadTransactionAsync(transactionId), Times.Once);
            _cicsServiceMock.Verify(service => service.ValidateAccountAsync(transactionRecord.AccountNumber), Times.Once);
            _cicsServiceMock.Verify(service => service.PostToLedgerAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.Never);
            _cicsServiceMock.Verify(service => service.NotifyExternalSystemAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.Never);
            _transactionRepositoryMock.Verify(repo => repo.UpdateTransactionStatusAsync(transactionId, TxnpostDomainModels.StatusEnum.Failed), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Handle_Ledger_Post_Failure()
        {
            // Arrange
            var transactionId = "TXN1234567";
            var transactionRecord = new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = transactionId,
                AccountNumber = "1234567890",
                TransactionAmount = 1000.00m,
                TransactionDate = DateTime.Parse("2023-10-01"),
                TransactionType = TxnpostDomainModels.TransactionTypeEnum.Credit,
                Status = TxnpostDomainModels.StatusEnum.Pending,
                Details = new TxnpostDomainModels.TransactionDetails
                {
                    Description = "Test Transaction",
                    CurrencyCode = "USD",
                    ExchangeRate = 1.0000m
                }
            };

            _transactionRepositoryMock
                .Setup(repo => repo.ReadTransactionAsync(transactionId))
                .ReturnsAsync(transactionRecord);

            _cicsServiceMock
                .Setup(service => service.ValidateAccountAsync(transactionRecord.AccountNumber))
                .ReturnsAsync(true);

            _cicsServiceMock
                .Setup(service => service.PostToLedgerAsync(transactionRecord))
                .ReturnsAsync(false);

            _transactionRepositoryMock
                .Setup(repo => repo.UpdateTransactionStatusAsync(transactionId, TxnpostDomainModels.StatusEnum.Failed))
                .Returns(Task.CompletedTask);

            // Act
            await _txnpostService.ExecuteAsync();

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.ReadTransactionAsync(transactionId), Times.Once);
            _cicsServiceMock.Verify(service => service.ValidateAccountAsync(transactionRecord.AccountNumber), Times.Once);
            _cicsServiceMock.Verify(service => service.PostToLedgerAsync(transactionRecord), Times.Once);
            _cicsServiceMock.Verify(service => service.NotifyExternalSystemAsync(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.Never);
            _transactionRepositoryMock.Verify(repo => repo.UpdateTransactionStatusAsync(transactionId, TxnpostDomainModels.StatusEnum.Failed), Times.Once);
        }
    }
}