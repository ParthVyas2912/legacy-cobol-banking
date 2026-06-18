using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using KYCSYNC.Application;
using KYCSYNC.Application.Interfaces;
using KYCSYNC.Domain;

namespace KYCSYNC.Tests
{
    public class KycsyncParityTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<ICicsService> _cicsServiceMock;
        private readonly IKycsyncService _kycsyncService;

        public KycsyncParityTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _cicsServiceMock = new Mock<ICicsService>();

            _kycsyncService = new KycsyncService(
                _customerRepositoryMock.Object,
                _transactionRepositoryMock.Object,
                _cicsServiceMock.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessAllCustomers()
        {
            // Arrange
            var customers = new[]
            {
                new KycsyncDomainModels.CustomerRecord
                {
                    CustomerId = "CUST001",
                    CustomerName = "John Doe",
                    CustomerAddress = new KycsyncDomainModels.CustomerRecord.Address
                    {
                        Street = "123 Main St",
                        City = "Anytown",
                        ZipCode = "12345"
                    },
                    AccountBalance = 1000.50m,
                    AccountStatus = KycsyncDomainModels.CustomerRecord.AccountStatusEnum.Active,
                    Transactions = new[]
                    {
                        new KycsyncDomainModels.CustomerRecord.Transaction
                        {
                            TransactionId = "TXN001",
                            TransactionAmount = 100.00m,
                            TransactionDate = new DateTime(2023, 1, 1),
                            TransactionType = KycsyncDomainModels.CustomerRecord.TransactionTypeEnum.Credit
                        }
                    }
                },
                new KycsyncDomainModels.CustomerRecord
                {
                    CustomerId = "CUST002",
                    CustomerName = "Jane Smith",
                    CustomerAddress = new KycsyncDomainModels.CustomerRecord.Address
                    {
                        Street = "456 Elm St",
                        City = "Othertown",
                        ZipCode = "67890"
                    },
                    AccountBalance = 2000.75m,
                    AccountStatus = KycsyncDomainModels.CustomerRecord.AccountStatusEnum.Inactive,
                    Transactions = new[]
                    {
                        new KycsyncDomainModels.CustomerRecord.Transaction
                        {
                            TransactionId = "TXN002",
                            TransactionAmount = 200.00m,
                            TransactionDate = new DateTime(2023, 2, 1),
                            TransactionType = KycsyncDomainModels.CustomerRecord.TransactionTypeEnum.Debit
                        }
                    }
                }
            };

            _customerRepositoryMock
                .Setup(repo => repo.ReadAllCustomersAsync())
                .ReturnsAsync(customers);

            _transactionRepositoryMock
                .Setup(repo => repo.ReadTransactionsByCustomerIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string customerId) =>
                {
                    return customers.First(c => c.CustomerId == customerId).Transactions;
                });

            _cicsServiceMock
                .Setup(service => service.UpdateCustomerStatusAsync(It.IsAny<string>(), It.IsAny<KycsyncDomainModels.CustomerRecord.AccountStatusEnum>()))
                .ReturnsAsync(true);

            _cicsServiceMock
                .Setup(service => service.LogTransactionAsync(It.IsAny<string>(), It.IsAny<KycsyncDomainModels.CustomerRecord.TransactionTypeEnum>(), It.IsAny<decimal>()))
                .ReturnsAsync(true);

            // Act
            await _kycsyncService.ExecuteAsync();

            // Assert
            _customerRepositoryMock.Verify(repo => repo.ReadAllCustomersAsync(), Times.Once);
            foreach (var customer in customers)
            {
                _transactionRepositoryMock.Verify(repo => repo.ReadTransactionsByCustomerIdAsync(customer.CustomerId), Times.Once);
                _cicsServiceMock.Verify(service => service.UpdateCustomerStatusAsync(customer.CustomerId, customer.AccountStatus), Times.Once);

                foreach (var transaction in customer.Transactions)
                {
                    _cicsServiceMock.Verify(service => service.LogTransactionAsync(transaction.TransactionId, transaction.TransactionType, transaction.TransactionAmount), Times.Once);
                }
            }
        }
    }
}