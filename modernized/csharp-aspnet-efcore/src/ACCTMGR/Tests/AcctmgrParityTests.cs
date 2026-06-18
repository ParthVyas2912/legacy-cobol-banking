using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ACCTMGR.Application;
using ACCTMGR.Application.Interfaces;
using ACCTMGR.Domain;

namespace ACCTMGR.Tests
{
    public class AcctmgrParityTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;
        private readonly Mock<ITransactionProcessor> _mockTransactionProcessor;
        private readonly Mock<ILogger<AcctmgrService>> _mockLogger;
        private readonly AcctmgrService _acctmgrService;

        public AcctmgrParityTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockTransactionProcessor = new Mock<ITransactionProcessor>();
            _mockLogger = new Mock<ILogger<AcctmgrService>>();

            _acctmgrService = new AcctmgrService(
                _mockAccountRepository.Object,
                _mockTransactionRepository.Object,
                _mockTransactionProcessor.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInitializeProcessAndFinalize()
        {
            // Arrange
            _mockAccountRepository.Setup(repo => repo.GetAllAccountsAsync()).ReturnsAsync(new List<AcctmgrDomainModels.Account>());
            _mockAccountRepository.Setup(repo => repo.SaveAccountAsync(It.IsAny<AcctmgrDomainModels.Account>())).Returns(Task.CompletedTask);

            // Act
            await _acctmgrService.ExecuteAsync();

            // Assert
            _mockAccountRepository.Verify(repo => repo.GetAllAccountsAsync(), Times.Once);
            _mockAccountRepository.Verify(repo => repo.SaveAccountAsync(It.IsAny<AcctmgrDomainModels.Account>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ProcessAccountsAsync_ShouldProcessEachAccount()
        {
            // Arrange
            var accounts = new List<AcctmgrDomainModels.Account>
            {
                new AcctmgrDomainModels.Account { AccountNumber = "1234567890", AccountName = "Test Account 1", Balance = 1000.00m, Status = AcctmgrDomainModels.AccountStatus.Active },
                new AcctmgrDomainModels.Account { AccountNumber = "0987654321", AccountName = "Test Account 2", Balance = 2000.00m, Status = AcctmgrDomainModels.AccountStatus.Active }
            };

            var transactions = new List<AcctmgrDomainModels.Transaction>
            {
                new AcctmgrDomainModels.Transaction { TransactionCode = "TXN001", TransactionDate = DateTime.Now, Amount = 100.00m, Description = "Test Transaction 1" },
                new AcctmgrDomainModels.Transaction { TransactionCode = "TXN002", TransactionDate = DateTime.Now, Amount = 200.00m, Description = "Test Transaction 2" }
            };

            _mockAccountRepository.Setup(repo => repo.GetAllAccountsAsync()).ReturnsAsync(accounts);
            _mockTransactionRepository.Setup(repo => repo.GetTransactionsByAccountNumberAsync(It.IsAny<string>())).ReturnsAsync(transactions);
            _mockTransactionProcessor.Setup(processor => processor.ProcessTransactionAsync(It.IsAny<AcctmgrDomainModels.Transaction>())).Returns(Task.CompletedTask);
            _mockAccountRepository.Setup(repo => repo.SaveAccountAsync(It.IsAny<AcctmgrDomainModels.Account>())).Returns(Task.CompletedTask);

            // Act
            await _acctmgrService.ExecuteAsync();

            // Assert
            _mockAccountRepository.Verify(repo => repo.GetAllAccountsAsync(), Times.Once);
            _mockTransactionRepository.Verify(repo => repo.GetTransactionsByAccountNumberAsync(It.IsAny<string>()), Times.Exactly(accounts.Count));
            _mockTransactionProcessor.Verify(processor => processor.ProcessTransactionAsync(It.IsAny<AcctmgrDomainModels.Transaction>()), Times.Exactly(transactions.Count * accounts.Count));
            _mockAccountRepository.Verify(repo => repo.SaveAccountAsync(It.IsAny<AcctmgrDomainModels.Account>()), Times.AtLeast(accounts.Count));
        }

        [Fact]
        public async Task ExecuteAsync_ShouldLogErrorOnException()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _mockAccountRepository.Setup(repo => repo.GetAllAccountsAsync()).ThrowsAsync(exception);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _acctmgrService.ExecuteAsync());
            _mockLogger.Verify(logger => logger.LogError(exception, "An error occurred during execution."), Times.Once);
        }
    }
}