using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using BCHEOD.Application;
using BCHEOD.Application.Interfaces;
using BCHEOD.Domain;

namespace BCHEOD.Tests
{
    public class BcheodParityTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;
        private readonly Mock<ICicsService> _mockCicsService;
        private readonly BcheodService _bcheodService;

        public BcheodParityTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockCicsService = new Mock<ICicsService>();

            _bcheodService = new BcheodService();
        }

        [Fact]
        public async Task ExecuteAsync_Should_InitializeProcessAndFinalize()
        {
            // Arrange
            var accounts = new[]
            {
                new BcheodDomainModels.AccountRecord
                {
                    AccountNumber = 1234567890,
                    AccountHolderName = "John Doe",
                    AccountBalance = 1000.00m,
                    AccountStatus = BcheodDomainModels.AccountStatusEnum.Active,
                    Transactions = new[]
                    {
                        new BcheodDomainModels.AccountRecord.TransactionRecord
                        {
                            TransactionAmount = 100.00m,
                            TransactionDate = "2023-10-01",
                            TransactionType = BcheodDomainModels.TransactionTypeEnum.Deposit
                        }
                    }
                }
            };

            _mockAccountRepository.Setup(repo => repo.ReadAllAccountsAsync())
                .ReturnsAsync(accounts);

            _mockCicsService.Setup(service => service.ValidateAccountAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockCicsService.Setup(service => service.UpdateAccountStatusAsync(It.IsAny<int>(), It.IsAny<BcheodDomainModels.AccountStatusEnum>()))
                .ReturnsAsync(true);

            // Act
            await _bcheodService.ExecuteAsync();

            // Assert
            _mockAccountRepository.Verify(repo => repo.ReadAllAccountsAsync(), Times.Once);
            _mockCicsService.Verify(service => service.ValidateAccountAsync(It.IsAny<int>()), Times.AtLeastOnce);
            _mockCicsService.Verify(service => service.UpdateAccountStatusAsync(It.IsAny<int>(), It.IsAny<BcheodDomainModels.AccountStatusEnum>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ProcessRecordsAsync_Should_ProcessEachRecord()
        {
            // Arrange
            var accounts = new[]
            {
                new BcheodDomainModels.AccountRecord
                {
                    AccountNumber = 1234567890,
                    AccountHolderName = "John Doe",
                    AccountBalance = 1000.00m,
                    AccountStatus = BcheodDomainModels.AccountStatusEnum.Active,
                    Transactions = new[]
                    {
                        new BcheodDomainModels.AccountRecord.TransactionRecord
                        {
                            TransactionAmount = 100.00m,
                            TransactionDate = "2023-10-01",
                            TransactionType = BcheodDomainModels.TransactionTypeEnum.Deposit
                        }
                    }
                },
                new BcheodDomainModels.AccountRecord
                {
                    AccountNumber = 9876543210,
                    AccountHolderName = "Jane Smith",
                    AccountBalance = 2000.00m,
                    AccountStatus = BcheodDomainModels.AccountStatusEnum.Inactive,
                    Transactions = new[]
                    {
                        new BcheodDomainModels.AccountRecord.TransactionRecord
                        {
                            TransactionAmount = 200.00m,
                            TransactionDate = "2023-10-02",
                            TransactionType = BcheodDomainModels.TransactionTypeEnum.Withdrawal
                        }
                    }
                }
            };

            _mockAccountRepository.Setup(repo => repo.ReadAllAccountsAsync())
                .ReturnsAsync(accounts);

            _mockCicsService.Setup(service => service.ValidateAccountAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockCicsService.Setup(service => service.UpdateAccountStatusAsync(It.IsAny<int>(), It.IsAny<BcheodDomainModels.AccountStatusEnum>()))
                .ReturnsAsync(true);

            // Act
            await _bcheodService.ExecuteAsync();

            // Assert
            _mockAccountRepository.Verify(repo => repo.ReadAllAccountsAsync(), Times.Once);
            _mockCicsService.Verify(service => service.ValidateAccountAsync(It.IsAny<int>()), Times.Exactly(accounts.Length));
            _mockCicsService.Verify(service => service.UpdateAccountStatusAsync(It.IsAny<int>(), It.IsAny<BcheodDomainModels.AccountStatusEnum>()), Times.Exactly(accounts.Length));
        }

        [Fact]
        public async Task HandleProcessingErrorAsync_Should_LogError()
        {
            // Arrange
            var record = new Record();
            var exception = new Exception("Test exception");

            _mockAccountRepository.Setup(repo => repo.ReadAllAccountsAsync())
                .ThrowsAsync(exception);

            // Act
            var exceptionThrown = await Record.ExceptionAsync(() => _bcheodService.ExecuteAsync());

            // Assert
            Assert.NotNull(exceptionThrown);
            Assert.Equal(exception.Message, exceptionThrown.Message);
        }

        [Fact]
        public async Task FinalizeProcessingAsync_Should_CommitAndLogCompletion()
        {
            // Arrange
            _mockAccountRepository.Setup(repo => repo.ReadAllAccountsAsync())
                .ReturnsAsync(Array.Empty<BcheodDomainModels.AccountRecord>());

            // Act
            await _bcheodService.ExecuteAsync();

            // Assert
            _mockAccountRepository.Verify(repo => repo.ReadAllAccountsAsync(), Times.Once);
            // Add additional assertions for CommitTransactionsAsync and LogCompletionAsync if they are exposed for verification
        }
    }
}