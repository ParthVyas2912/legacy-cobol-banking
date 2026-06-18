using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Mock<ILogger<AcctmgrService>> _mockLogger;
        private readonly AcctmgrService _acctmgrService;

        public AcctmgrParityTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockLogger = new Mock<ILogger<AcctmgrService>>();
            _acctmgrService = new AcctmgrService(_mockAccountRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInitializeProcessAndFinalize()
        {
            // Arrange
            var mockAccounts = new List<AccountManager.CustomerRecord.AccountRecord>
            {
                new AccountManager.CustomerRecord.AccountRecord
                {
                    AccountNumber = "123456789012",
                    Balance = 1000.00m,
                    AccountType = AccountManager.CustomerRecord.AccountRecord.AccountTypeEnum.Checking
                },
                new AccountManager.CustomerRecord.AccountRecord
                {
                    AccountNumber = "987654321098",
                    Balance = 2000.00m,
                    AccountType = AccountManager.CustomerRecord.AccountRecord.AccountTypeEnum.Savings
                }
            };

            _mockAccountRepository.Setup(repo => repo.ReadAccountsAsync(It.IsAny<string>()))
                .ReturnsAsync(mockAccounts.ToArray());

            _mockAccountRepository.Setup(repo => repo.WriteAccountsAsync(It.IsAny<string>(), It.IsAny<AccountManager.CustomerRecord.AccountRecord[]>()))
                .Returns(Task.CompletedTask);

            // Act
            await _acctmgrService.ExecuteAsync();

            // Assert
            _mockAccountRepository.Verify(repo => repo.ReadAccountsAsync(It.IsAny<string>()), Times.Once);
            _mockAccountRepository.Verify(repo => repo.WriteAccountsAsync(It.IsAny<string>(), It.IsAny<AccountManager.CustomerRecord.AccountRecord[]>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldLogErrorsDuringProcessing()
        {
            // Arrange
            var mockAccounts = new List<AccountManager.CustomerRecord.AccountRecord>
            {
                new AccountManager.CustomerRecord.AccountRecord
                {
                    AccountNumber = "123456789012",
                    Balance = 1000.00m,
                    AccountType = AccountManager.CustomerRecord.AccountRecord.AccountTypeEnum.Checking
                },
                new AccountManager.CustomerRecord.AccountRecord
                {
                    AccountNumber = "987654321098",
                    Balance = 2000.00m,
                    AccountType = AccountManager.CustomerRecord.AccountRecord.AccountTypeEnum.Savings
                }
            };

            _mockAccountRepository.Setup(repo => repo.ReadAccountsAsync(It.IsAny<string>()))
                .ReturnsAsync(mockAccounts.ToArray());

            _mockAccountRepository.Setup(repo => repo.WriteAccountsAsync(It.IsAny<string>(), It.Is<AccountManager.CustomerRecord.AccountRecord[]>(a => a.Any(r => r.AccountNumber == "123456789012"))))
                .ThrowsAsync(new Exception("Test exception"));

            _mockAccountRepository.Setup(repo => repo.WriteAccountsAsync(It.IsAny<string>(), It.Is<AccountManager.CustomerRecord.AccountRecord[]>(a => a.Any(r => r.AccountNumber == "987654321098"))))
                .Returns(Task.CompletedTask);

            // Act
            await _acctmgrService.ExecuteAsync();

            // Assert
            _mockLogger.Verify(logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error processing account ID: 123456789012")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);

            _mockAccountRepository.Verify(repo => repo.ReadAccountsAsync(It.IsAny<string>()), Times.Once);
            _mockAccountRepository.Verify(repo => repo.WriteAccountsAsync(It.IsAny<string>(), It.IsAny<AccountManager.CustomerRecord.AccountRecord[]>()), Times.Exactly(mockAccounts.Count));
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyAccountsGracefully()
        {
            // Arrange
            _mockAccountRepository.Setup(repo => repo.ReadAccountsAsync(It.IsAny<string>()))
                .ReturnsAsync(Array.Empty<AccountManager.CustomerRecord.AccountRecord>());

            _mockAccountRepository.Setup(repo => repo.WriteAccountsAsync(It.IsAny<string>(), It.IsAny<AccountManager.CustomerRecord.AccountRecord[]>()))
                .Returns(Task.CompletedTask);

            // Act
            await _acctmgrService.ExecuteAsync();

            // Assert
            _mockAccountRepository.Verify(repo => repo.ReadAccountsAsync(It.IsAny<string>()), Times.Once);
            _mockAccountRepository.Verify(repo => repo.WriteAccountsAsync(It.IsAny<string>(), It.IsAny<AccountManager.CustomerRecord.AccountRecord[]>()), Times.Never);
        }
    }
}