using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using STMTRPT.Application;
using STMTRPT.Application.Interfaces;
using STMTRPT.Domain;

namespace STMTRPT.Tests
{
    public class StmtrptParityTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IReportRepository> _mockReportRepository;
        private readonly IStmtrptService _stmtrptService;

        public StmtrptParityTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockReportRepository = new Mock<IReportRepository>();
            _stmtrptService = new StmtrptService(_mockAccountRepository.Object, _mockReportRepository.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldCompleteWithoutExceptions()
        {
            // Arrange
            _mockAccountRepository.Setup(repo => repo.GetAllAccountsAsync())
                .ReturnsAsync(new StmtrptDomainModels.AccountSummary[0]);

            _mockReportRepository.Setup(repo => repo.SaveReportHeaderAsync(It.IsAny<StmtrptDomainModels.ReportHeader>()))
                .Returns(Task.CompletedTask);

            _mockReportRepository.Setup(repo => repo.SaveAccountSummaryAsync(It.IsAny<StmtrptDomainModels.AccountSummary>()))
                .Returns(Task.CompletedTask);

            _mockReportRepository.Setup(repo => repo.SaveReportFooterAsync(It.IsAny<StmtrptDomainModels.ReportFooter>()))
                .Returns(Task.CompletedTask);

            // Act
            var exception = await Record.ExceptionAsync(() => _stmtrptService.ExecuteAsync());

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldCallRepositories()
        {
            // Arrange
            var mockAccountSummaries = new[]
            {
                new StmtrptDomainModels.AccountSummary
                {
                    AccountNumber = "1234567890",
                    AccountHolderName = "John Doe",
                    AccountBalance = 1000.50m,
                    Transactions = new[]
                    {
                        new StmtrptDomainModels.Transaction
                        {
                            TransactionDate = "230101",
                            TransactionCode = "DEP",
                            TransactionAmount = 500.00m,
                            Description = "Deposit"
                        },
                        new StmtrptDomainModels.Transaction
                        {
                            TransactionDate = "230102",
                            TransactionCode = "WDR",
                            TransactionAmount = -200.00m,
                            Description = "Withdrawal"
                        }
                    }
                }
            };

            _mockAccountRepository.Setup(repo => repo.GetAllAccountsAsync())
                .ReturnsAsync(mockAccountSummaries);

            _mockReportRepository.Setup(repo => repo.SaveReportHeaderAsync(It.IsAny<StmtrptDomainModels.ReportHeader>()))
                .Returns(Task.CompletedTask);

            _mockReportRepository.Setup(repo => repo.SaveAccountSummaryAsync(It.IsAny<StmtrptDomainModels.AccountSummary>()))
                .Returns(Task.CompletedTask);

            _mockReportRepository.Setup(repo => repo.SaveReportFooterAsync(It.IsAny<StmtrptDomainModels.ReportFooter>()))
                .Returns(Task.CompletedTask);

            // Act
            await _stmtrptService.ExecuteAsync();

            // Assert
            _mockAccountRepository.Verify(repo => repo.GetAllAccountsAsync(), Times.Once);
            _mockReportRepository.Verify(repo => repo.SaveReportHeaderAsync(It.IsAny<StmtrptDomainModels.ReportHeader>()), Times.Once);
            _mockReportRepository.Verify(repo => repo.SaveAccountSummaryAsync(It.IsAny<StmtrptDomainModels.AccountSummary>()), Times.Exactly(mockAccountSummaries.Length));
            _mockReportRepository.Verify(repo => repo.SaveReportFooterAsync(It.IsAny<StmtrptDomainModels.ReportFooter>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyAccounts()
        {
            // Arrange
            _mockAccountRepository.Setup(repo => repo.GetAllAccountsAsync())
                .ReturnsAsync(Array.Empty<StmtrptDomainModels.AccountSummary>());

            _mockReportRepository.Setup(repo => repo.SaveReportHeaderAsync(It.IsAny<StmtrptDomainModels.ReportHeader>()))
                .Returns(Task.CompletedTask);

            _mockReportRepository.Setup(repo => repo.SaveReportFooterAsync(It.IsAny<StmtrptDomainModels.ReportFooter>()))
                .Returns(Task.CompletedTask);

            // Act
            await _stmtrptService.ExecuteAsync();

            // Assert
            _mockAccountRepository.Verify(repo => repo.GetAllAccountsAsync(), Times.Once);
            _mockReportRepository.Verify(repo => repo.SaveReportHeaderAsync(It.IsAny<StmtrptDomainModels.ReportHeader>()), Times.Once);
            _mockReportRepository.Verify(repo => repo.SaveAccountSummaryAsync(It.IsAny<StmtrptDomainModels.AccountSummary>()), Times.Never);
            _mockReportRepository.Verify(repo => repo.SaveReportFooterAsync(It.IsAny<StmtrptDomainModels.ReportFooter>()), Times.Once);
        }
    }
}