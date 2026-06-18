using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ACCTMGR.Application.Interfaces;
using ACCTMGR.Domain;

namespace ACCTMGR.Tests
{
    public class AcctmgrParityTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IReportService> _mockReportService;
        private readonly AcctmgrService _acctmgrService;

        public AcctmgrParityTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockReportService = new Mock<IReportService>();

            _acctmgrService = new AcctmgrService(
                _mockCustomerRepository.Object,
                _mockAccountRepository.Object,
                _mockReportService.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessCustomerRecordsAndGenerateReports()
        {
            // Arrange
            var customerRecords = new[]
            {
                new AccountManager.CustomerRecord
                {
                    CustomerId = "CUST001",
                    CustomerName = "John Doe",
                    Address = new AccountManager.AddressRecord
                    {
                        Street = "123 Main St",
                        City = "Springfield",
                        State = "IL",
                        ZipCode = "62701"
                    },
                    Accounts = new[]
                    {
                        new AccountManager.AccountRecord
                        {
                            AccountNumber = "ACC001",
                            Balance = 1000.00m,
                            Status = AccountManager.AccountStatus.Active
                        },
                        new AccountManager.AccountRecord
                        {
                            AccountNumber = "ACC002",
                            Balance = 500.00m,
                            Status = AccountManager.AccountStatus.Inactive
                        }
                    }
                },
                new AccountManager.CustomerRecord
                {
                    CustomerId = "CUST002",
                    CustomerName = "Jane Smith",
                    Address = new AccountManager.AddressRecord
                    {
                        Street = "456 Elm St",
                        City = "Chicago",
                        State = "IL",
                        ZipCode = "60601"
                    },
                    Accounts = new[]
                    {
                        new AccountManager.AccountRecord
                        {
                            AccountNumber = "ACC003",
                            Balance = 2000.00m,
                            Status = AccountManager.AccountStatus.Active
                        }
                    }
                }
            };

            _mockCustomerRepository
                .Setup(repo => repo.ReadCustomerRecordsAsync())
                .ReturnsAsync(customerRecords);

            _mockAccountRepository
                .Setup(repo => repo.ReadAccountRecordsAsync(It.IsAny<string>()))
                .ReturnsAsync((string customerId) =>
                {
                    return customerRecords
                        .FirstOrDefault(c => c.CustomerId == customerId)?
                        .Accounts ?? Array.Empty<AccountManager.AccountRecord>();
                });

            // Act
            await _acctmgrService.ExecuteAsync();

            // Assert
            _mockCustomerRepository.Verify(repo => repo.ReadCustomerRecordsAsync(), Times.Once);
            _mockAccountRepository.Verify(repo => repo.ReadAccountRecordsAsync(It.IsAny<string>()), Times.Exactly(customerRecords.Length));
            _mockReportService.Verify(service => service.GenerateCustomerReportAsync(customerRecords), Times.Once);

            foreach (var customer in customerRecords)
            {
                _mockReportService.Verify(service => service.GenerateAccountSummaryReportAsync(customer.Accounts), Times.Once);
            }
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyCustomerRecords()
        {
            // Arrange
            _mockCustomerRepository
                .Setup(repo => repo.ReadCustomerRecordsAsync())
                .ReturnsAsync(Array.Empty<AccountManager.CustomerRecord>());

            // Act
            await _acctmgrService.ExecuteAsync();

            // Assert
            _mockCustomerRepository.Verify(repo => repo.ReadCustomerRecordsAsync(), Times.Once);
            _mockAccountRepository.Verify(repo => repo.ReadAccountRecordsAsync(It.IsAny<string>()), Times.Never);
            _mockReportService.Verify(service => service.GenerateCustomerReportAsync(It.IsAny<AccountManager.CustomerRecord[]>()), Times.Once);
            _mockReportService.Verify(service => service.GenerateAccountSummaryReportAsync(It.IsAny<AccountManager.AccountRecord[]>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldWriteUpdatedCustomerRecords()
        {
            // Arrange
            var customerRecords = new[]
            {
                new AccountManager.CustomerRecord
                {
                    CustomerId = "CUST001",
                    CustomerName = "John Doe",
                    Address = new AccountManager.AddressRecord
                    {
                        Street = "123 Main St",
                        City = "Springfield",
                        State = "IL",
                        ZipCode = "62701"
                    },
                    Accounts = new[]
                    {
                        new AccountManager.AccountRecord
                        {
                            AccountNumber = "ACC001",
                            Balance = 1000.00m,
                            Status = AccountManager.AccountStatus.Active
                        }
                    }
                }
            };

            _mockCustomerRepository
                .Setup(repo => repo.ReadCustomerRecordsAsync())
                .ReturnsAsync(customerRecords);

            // Act
            await _acctmgrService.ExecuteAsync();

            // Assert
            _mockCustomerRepository.Verify(repo => repo.WriteCustomerRecordsAsync(It.IsAny<AccountManager.CustomerRecord[]>()), Times.Once);
        }
    }
}