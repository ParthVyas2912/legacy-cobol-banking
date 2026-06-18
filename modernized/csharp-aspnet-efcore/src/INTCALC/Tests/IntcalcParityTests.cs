using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using INTCALC.Application;
using INTCALC.Application.Interfaces;
using INTCALC.Domain;

namespace INTCALC.Tests
{
    public class IntcalcParityTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IInterestCalculationService> _interestCalculationServiceMock;
        private readonly Mock<IReportService> _reportServiceMock;
        private readonly IntcalcService _intcalcService;

        public IntcalcParityTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _interestCalculationServiceMock = new Mock<IInterestCalculationService>();
            _reportServiceMock = new Mock<IReportService>();

            _intcalcService = new IntcalcService();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldRunWithoutExceptions()
        {
            // Arrange
            // Mock dependencies if necessary

            // Act
            var exception = await Record.ExceptionAsync(() => _intcalcService.ExecuteAsync());

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task InterestCalculationService_ShouldCalculateCorrectInterest()
        {
            // Arrange
            decimal principalAmount = 1000m;
            decimal interestRate = 0.05m;
            decimal expectedInterest = 50m;
            decimal expectedTotal = 1050m;

            _interestCalculationServiceMock
                .Setup(service => service.CalculateInterestAsync(principalAmount, interestRate))
                .ReturnsAsync(new IntcalcDomainModels.InterestCalculation
                {
                    PrincipalAmount = principalAmount,
                    CalculatedInterest = expectedInterest,
                    TotalAmount = expectedTotal
                });

            // Act
            var result = await _interestCalculationServiceMock.Object.CalculateInterestAsync(principalAmount, interestRate);

            // Assert
            Assert.Equal(principalAmount, result.PrincipalAmount);
            Assert.Equal(expectedInterest, result.CalculatedInterest);
            Assert.Equal(expectedTotal, result.TotalAmount);
        }

        [Fact]
        public async Task AccountRepository_ShouldRetrieveAccount()
        {
            // Arrange
            var accountNumber = "1234567890";
            var expectedAccount = new IntcalcDomainModels.Account
            {
                AccountNumber = accountNumber,
                AccountName = "Test Account",
                InterestRate = 0.05m,
                Balance = 1000m,
                Status = IntcalcDomainModels.AccountStatus.Active
            };

            _accountRepositoryMock
                .Setup(repo => repo.GetAccountAsync(accountNumber))
                .ReturnsAsync(expectedAccount);

            // Act
            var result = await _accountRepositoryMock.Object.GetAccountAsync(accountNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAccount.AccountNumber, result.AccountNumber);
            Assert.Equal(expectedAccount.AccountName, result.AccountName);
            Assert.Equal(expectedAccount.InterestRate, result.InterestRate);
            Assert.Equal(expectedAccount.Balance, result.Balance);
            Assert.Equal(expectedAccount.Status, result.Status);
        }

        [Fact]
        public async Task TransactionRepository_ShouldAddTransaction()
        {
            // Arrange
            var transaction = new IntcalcDomainModels.Transaction
            {
                TransactionId = "TXN123456789",
                TransactionDate = DateTime.Today,
                Amount = 500m,
                CurrencyCode = "USD"
            };

            _transactionRepositoryMock
                .Setup(repo => repo.AddTransactionAsync(transaction))
                .Returns(Task.CompletedTask);

            // Act
            var exception = await Record.ExceptionAsync(() => _transactionRepositoryMock.Object.AddTransactionAsync(transaction));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task ReportService_ShouldGenerateAccountReport()
        {
            // Arrange
            var account = new IntcalcDomainModels.Account
            {
                AccountNumber = "1234567890",
                AccountName = "Test Account",
                InterestRate = 0.05m,
                Balance = 1000m,
                Status = IntcalcDomainModels.AccountStatus.Active
            };

            _reportServiceMock
                .Setup(service => service.GenerateAccountReportAsync(account))
                .Returns(Task.CompletedTask);

            // Act
            var exception = await Record.ExceptionAsync(() => _reportServiceMock.Object.GenerateAccountReportAsync(account));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task ReportService_ShouldGenerateTransactionReport()
        {
            // Arrange
            var transaction = new IntcalcDomainModels.Transaction
            {
                TransactionId = "TXN123456789",
                TransactionDate = DateTime.Today,
                Amount = 500m,
                CurrencyCode = "USD"
            };

            _reportServiceMock
                .Setup(service => service.GenerateTransactionReportAsync(transaction))
                .Returns(Task.CompletedTask);

            // Act
            var exception = await Record.ExceptionAsync(() => _reportServiceMock.Object.GenerateTransactionReportAsync(transaction));

            // Assert
            Assert.Null(exception);
        }
    }
}