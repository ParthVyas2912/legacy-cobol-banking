using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Mock<ITransactionInputRepository> _transactionInputRepositoryMock;
        private readonly Mock<IAuditOutputRepository> _auditOutputRepositoryMock;
        private readonly Mock<ITransactionLedgerService> _transactionLedgerServiceMock;
        private readonly Mock<IWorkingStorageService> _workingStorageServiceMock;
        private readonly TxnpostService _txnpostService;

        public TxnpostParityTests()
        {
            _transactionInputRepositoryMock = new Mock<ITransactionInputRepository>();
            _auditOutputRepositoryMock = new Mock<IAuditOutputRepository>();
            _transactionLedgerServiceMock = new Mock<ITransactionLedgerService>();
            _workingStorageServiceMock = new Mock<IWorkingStorageService>();

            _txnpostService = new TxnpostService(
                _transactionInputRepositoryMock.Object,
                _auditOutputRepositoryMock.Object,
                _transactionLedgerServiceMock.Object,
                _workingStorageServiceMock.Object
            );
        }

        [Fact]
        public async Task Txnpost_HappyPath_ShouldProcessTransactionsCorrectly()
        {
            // Arrange
            var sampleTransaction = new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = "000000000001",
                AccountNumber = "0000000001",
                TransactionAmount = 12222222222.34m,
                TransactionDate = "20230101",
                Description = "Sample Transaction"
            };

            _transactionInputRepositoryMock
                .Setup(repo => repo.ReadTransactions())
                .Returns(new[] { sampleTransaction });

            // Act
            await _txnpostService.ExecuteAsync();

            // Assert
            _transactionLedgerServiceMock.Verify(service =>
                service.UpdateLedger(It.Is<TxnpostDomainModels.TransactionRecord>(txn =>
                    txn.TransactionId == sampleTransaction.TransactionId &&
                    txn.AccountNumber == sampleTransaction.AccountNumber &&
                    txn.TransactionAmount == sampleTransaction.TransactionAmount &&
                    txn.TransactionDate == sampleTransaction.TransactionDate &&
                    txn.Description == sampleTransaction.Description
                )), Times.Once);

            _auditOutputRepositoryMock.Verify(repo =>
                repo.WriteAuditRecord(It.Is<TxnpostDomainModels.AuditOutput>(audit =>
                    audit.AccountNumber == sampleTransaction.AccountNumber &&
                    audit.AuditAmount == sampleTransaction.TransactionAmount &&
                    audit.AuditDate == sampleTransaction.TransactionDate &&
                    audit.AuditDescription == sampleTransaction.Description
                )), Times.Once);

            _workingStorageServiceMock.Verify(service =>
                service.UpdateRecordCount(It.Is<decimal>(count => count == 1)), Times.Once);
        }

        [Fact]
        public async Task Txnpost_EmptyInputFile_ShouldNotProcessTransactions()
        {
            // Arrange
            _transactionInputRepositoryMock
                .Setup(repo => repo.ReadTransactions())
                .Returns(Array.Empty<TxnpostDomainModels.TransactionRecord>());

            // Act
            await _txnpostService.ExecuteAsync();

            // Assert
            _transactionLedgerServiceMock.Verify(service =>
                service.UpdateLedger(It.IsAny<TxnpostDomainModels.TransactionRecord>()), Times.Never);

            _auditOutputRepositoryMock.Verify(repo =>
                repo.WriteAuditRecord(It.IsAny<TxnpostDomainModels.AuditOutput>()), Times.Never);

            _workingStorageServiceMock.Verify(service =>
                service.UpdateRecordCount(It.IsAny<decimal>()), Times.Never);
        }
    }
}