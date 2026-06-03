using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using STMTRPT.Application;
using STMTRPT.Application.Interfaces;
using STMTRPT.Domain;
using STMTRPT.Infrastructure;
using Xunit;

namespace STMTRPT.Tests
{
    public class StmtrptParityTests
    {
        private readonly Mock<ICustomerRecordRepository> _mockCustomerRecordRepository;
        private readonly Mock<IImsService> _mockImsService;
        private readonly Mock<ITransactionLedgerRepository> _mockTransactionLedgerRepository;
        private readonly Mock<IStatementFileRepository> _mockStatementFileRepository;
        private readonly StmtrptService _service;

        public StmtrptParityTests()
        {
            _mockCustomerRecordRepository = new Mock<ICustomerRecordRepository>();
            _mockImsService = new Mock<IImsService>();
            _mockTransactionLedgerRepository = new Mock<ITransactionLedgerRepository>();
            _mockStatementFileRepository = new Mock<IStatementFileRepository>();

            _service = new StmtrptService(
                _mockCustomerRecordRepository.Object,
                _mockImsService.Object,
                _mockTransactionLedgerRepository.Object,
                _mockStatementFileRepository.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_HappyPath_ShouldProcessCorrectly()
        {
            // Arrange
            var customerRecords = new List<StmtrptDomainModels.CustomerRecord>
            {
                new StmtrptDomainModels.CustomerRecord
                {
                    CustomerId = "0000000001",
                    CustomerName = "John Doe",
                    RiskScore = 85,
                    AccountCount = 3,
                    CustomerStatus = "Active",
                    CustomerAddress = new StmtrptDomainModels.Address
                    {
                        Street = "123 Main St",
                        City = "Springfield",
                        State = "IL",
                        ZipCode = "62704"
                    }
                }
            };

            var imsRecord = new StmtrptDomainModels.CustomerRecord
            {
                CustomerId = "0000000001",
                CustomerName = "John Doe",
                RiskScore = 85,
                AccountCount = 3,
                CustomerStatus = "Active",
                CustomerAddress = new StmtrptDomainModels.Address
                {
                    Street = "123 Main St",
                    City = "Springfield",
                    State = "IL",
                    ZipCode = "62704"
                }
            };

            var transactionLedgers = new List<StmtrptDomainModels.TransactionLedger>
            {
                new StmtrptDomainModels.TransactionLedger
                {
                    TransactionId = 1,
                    AccountNumber = "0000000001",
                    TransactionAmount = 100.00m,
                    TransactionDate = "20231001",
                    Description = "Deposit"
                }
            };

            var statementFile = new StmtrptDomainModels.StatementFile
            {
                StatementId = "STMT0001",
                CustomerId = "0000000001",
                TotalBalance = 100.00m,
                StatementDate = "20231001",
                Transactions = transactionLedgers
            };

            _mockCustomerRecordRepository
                .Setup(repo => repo.FetchCustomerRecords())
                .Returns(customerRecords);

            _mockImsService
                .Setup(service => service.ExecuteImsGn(It.IsAny<string>()))
                .Returns(imsRecord);

            _mockTransactionLedgerRepository
                .Setup(repo => repo.FetchTransactionLedgers())
                .Returns(transactionLedgers);

            _mockStatementFileRepository
                .Setup(repo => repo.WriteStatementFile(It.IsAny<StmtrptDomainModels.StatementFile>()))
                .Callback<StmtrptDomainModels.StatementFile>(file =>
                {
                    Assert.Equal(statementFile.CustomerId, file.CustomerId);
                    Assert.Equal(statementFile.TotalBalance, file.TotalBalance);
                    Assert.Equal(statementFile.StatementDate, file.StatementDate);
                    Assert.Equal(statementFile.Transactions.Count, file.Transactions.Count);
                });

            // Act
            await _service.ExecuteAsync();

            // Assert
            _mockCustomerRecordRepository.Verify(repo => repo.FetchCustomerRecords(), Times.Once);
            _mockImsService.Verify(service => service.ExecuteImsGn("GET_CUSTOMER"), Times.AtLeastOnce);
            _mockTransactionLedgerRepository.Verify(repo => repo.FetchTransactionLedgers(), Times.Once);
            _mockStatementFileRepository.Verify(repo => repo.WriteStatementFile(It.IsAny<StmtrptDomainModels.StatementFile>()), Times.Once);
        }
    }
}