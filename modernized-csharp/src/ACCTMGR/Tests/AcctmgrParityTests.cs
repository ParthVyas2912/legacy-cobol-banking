using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ACCTMGR.Application;
using ACCTMGR.Application.Interfaces;
using ACCTMGR.Domain;

namespace ACCTMGR.Tests
{
    public class AcctmgrParityTests
    {
        private readonly Mock<IAccountFileRepository> _mockAccountFileRepository;
        private readonly Mock<ICicsService> _mockCicsService;
        private readonly AcctmgrService _acctmgrService;

        public AcctmgrParityTests()
        {
            _mockAccountFileRepository = new Mock<IAccountFileRepository>();
            _mockCicsService = new Mock<ICicsService>();
            _acctmgrService = new AcctmgrService(_mockAccountFileRepository.Object, _mockCicsService.Object);
        }

        [Fact]
        public async Task ExecuteAsync_HappyPath_ShouldProcessCorrectly()
        {
            // Arrange
            var sampleAccount = new AcctmgrDomainModels.AccountRecord
            {
                AccountNumber = "0000000001",
                CustomerId = "00000001",
                OpenedDate = new DateTime(2023, 01, 01),
                AccountType = "CK",
                AccountStatus = "A",
                AccountBalance = 1222222222222.34m,
                AvailableBalance = 1222222222222.34m,
                InterestRate = 12.3444m,
                LastTransactionDate = new DateTime(2023, 01, 01),
                Currency = "USD",
                BranchCode = 1,
                Flags = "YYYY",
                Filler = "FILLERDATA"
            };

            _mockAccountFileRepository
                .Setup(repo => repo.ReadAccount(It.IsAny<string>()))
                .Returns(sampleAccount);

            // Act
            await _acctmgrService.ExecuteAsync();

            // Assert
            _mockAccountFileRepository.Verify(repo => repo.ReadAccount(It.IsAny<string>()), Times.Once);
            _mockAccountFileRepository.Verify(repo => repo.RewriteAccount(It.IsAny<AcctmgrDomainModels.AccountRecord>()), Times.Once);
            _mockCicsService.Verify(service => service.Return(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_EmptyFile_ShouldHandleGracefully()
        {
            // Arrange
            _mockAccountFileRepository
                .Setup(repo => repo.ReadAccount(It.IsAny<string>()))
                .Returns((AcctmgrDomainModels.AccountRecord)null);

            // Act
            await _acctmgrService.ExecuteAsync();

            // Assert
            _mockAccountFileRepository.Verify(repo => repo.ReadAccount(It.IsAny<string>()), Times.Once);
            _mockAccountFileRepository.Verify(repo => repo.RewriteAccount(It.IsAny<AcctmgrDomainModels.AccountRecord>()), Times.Never);
            _mockCicsService.Verify(service => service.Return(), Times.Once);
        }

        [Fact]
        public void CreateAccount_ShouldInvokeRepositoryWrite()
        {
            // Arrange
            var newAccount = new AcctmgrDomainModels.AccountRecord
            {
                AccountNumber = "0000000002",
                CustomerId = "00000002",
                OpenedDate = new DateTime(2023, 01, 02),
                AccountType = "SV",
                AccountStatus = "A",
                AccountBalance = 1000.00m,
                AvailableBalance = 1000.00m,
                InterestRate = 1.2345m,
                LastTransactionDate = new DateTime(2023, 01, 02),
                Currency = "USD",
                BranchCode = 2,
                Flags = "NNNN",
                Filler = "FILLERDATA"
            };

            // Act
            _acctmgrService.CreateAccount(newAccount);

            // Assert
            _mockAccountFileRepository.Verify(repo => repo.WriteAccount(newAccount), Times.Once);
        }

        [Fact]
        public void ReadAccount_ShouldReturnCorrectAccount()
        {
            // Arrange
            var accountNumber = "0000000003";
            var expectedAccount = new AcctmgrDomainModels.AccountRecord
            {
                AccountNumber = accountNumber,
                CustomerId = "00000003",
                OpenedDate = new DateTime(2023, 01, 03),
                AccountType = "LN",
                AccountStatus = "F",
                AccountBalance = 5000.00m,
                AvailableBalance = 4000.00m,
                InterestRate = 5.6789m,
                LastTransactionDate = new DateTime(2023, 01, 03),
                Currency = "USD",
                BranchCode = 3,
                Flags = "YYYY",
                Filler = "FILLERDATA"
            };

            _mockAccountFileRepository
                .Setup(repo => repo.ReadAccount(accountNumber))
                .Returns(expectedAccount);

            // Act
            var result = _acctmgrService.ReadAccount(accountNumber);

            // Assert
            Assert.Equal(expectedAccount, result);
            _mockAccountFileRepository.Verify(repo => repo.ReadAccount(accountNumber), Times.Once);
        }

        [Fact]
        public void UpdateAccount_ShouldInvokeRepositoryRewrite()
        {
            // Arrange
            var updatedAccount = new AcctmgrDomainModels.AccountRecord
            {
                AccountNumber = "0000000004",
                CustomerId = "00000004",
                OpenedDate = new DateTime(2023, 01, 04),
                AccountType = "CK",
                AccountStatus = "A",
                AccountBalance = 2000.00m,
                AvailableBalance = 2000.00m,
                InterestRate = 2.3456m,
                LastTransactionDate = new DateTime(2023, 01, 04),
                Currency = "USD",
                BranchCode = 4,
                Flags = "NNNN",
                Filler = "FILLERDATA"
            };

            // Act
            _acctmgrService.UpdateAccount(updatedAccount);

            // Assert
            _mockAccountFileRepository.Verify(repo => repo.RewriteAccount(updatedAccount), Times.Once);
        }

        [Fact]
        public void CloseAccount_ShouldInvokeRepositoryRewriteWithClosedStatus()
        {
            // Arrange
            var accountNumber = "0000000005";
            var existingAccount = new AcctmgrDomainModels.AccountRecord
            {
                AccountNumber = accountNumber,
                CustomerId = "00000005",
                OpenedDate = new DateTime(2023, 01, 05),
                AccountType = "SV",
                AccountStatus = "A",
                AccountBalance = 3000.00m,
                AvailableBalance = 3000.00m,
                InterestRate = 3.4567m,
                LastTransactionDate = new DateTime(2023, 01, 05),
                Currency = "USD",
                BranchCode = 5,
                Flags = "YYYY",
                Filler = "FILLERDATA"
            };

            _mockAccountFileRepository
                .Setup(repo => repo.ReadAccount(accountNumber))
                .Returns(existingAccount);

            // Act
            _acctmgrService.CloseAccount(accountNumber);

            // Assert
            _mockAccountFileRepository.Verify(repo => repo.ReadAccount(accountNumber), Times.Once);
            _mockAccountFileRepository.Verify(repo => repo.RewriteAccount(It.Is<AcctmgrDomainModels.AccountRecord>(a => a.AccountStatus == "C")), Times.Once);
        }
    }
}