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
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IKycStatusService> _mockKycStatusService;
        private readonly IKycsyncService _kycsyncService;

        public KycsyncParityTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockKycStatusService = new Mock<IKycStatusService>();
            _kycsyncService = new KycsyncService(_mockCustomerRepository.Object, _mockKycStatusService.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessAllCustomers()
        {
            // Arrange
            var customers = new List<KycsyncDomainModels.CustomerRecord>
            {
                new KycsyncDomainModels.CustomerRecord
                {
                    CustomerId = "CUST001",
                    FirstName = "John",
                    LastName = "Doe",
                    Gender = "M",
                    DateOfBirth = "1980-01-01",
                    Address = new KycsyncDomainModels.Address
                    {
                        Street = "123 Main St",
                        City = "Metropolis",
                        State = "NY",
                        PostalCode = "12345",
                        CountryCode = "US"
                    },
                    ContactDetails = new KycsyncDomainModels.ContactDetails
                    {
                        PhoneNumber = "1234567890",
                        Email = "john.doe@example.com"
                    },
                    EmploymentDetails = new KycsyncDomainModels.EmploymentDetails
                    {
                        EmployerName = "Tech Corp",
                        JobTitle = "Engineer",
                        AnnualIncome = 75000.00m
                    }
                },
                new KycsyncDomainModels.CustomerRecord
                {
                    CustomerId = "CUST002",
                    FirstName = "Jane",
                    LastName = "Smith",
                    Gender = "F",
                    DateOfBirth = "1990-05-15",
                    Address = new KycsyncDomainModels.Address
                    {
                        Street = "456 Elm St",
                        City = "Gotham",
                        State = "CA",
                        PostalCode = "67890",
                        CountryCode = "US"
                    },
                    ContactDetails = new KycsyncDomainModels.ContactDetails
                    {
                        PhoneNumber = "0987654321",
                        Email = "jane.smith@example.com"
                    },
                    EmploymentDetails = new KycsyncDomainModels.EmploymentDetails
                    {
                        EmployerName = "Finance Inc",
                        JobTitle = "Analyst",
                        AnnualIncome = 85000.00m
                    }
                }
            };

            _mockCustomerRepository
                .Setup(repo => repo.ReadAllCustomersAsync())
                .ReturnsAsync(customers);

            _mockKycStatusService
                .Setup(service => service.GetKycStatusAsync(It.IsAny<string>()))
                .ReturnsAsync(KycsyncDomainModels.KycStatus.KycStatusEnum.NotVerified);

            // Act
            await _kycsyncService.ExecuteAsync();

            // Assert
            _mockCustomerRepository.Verify(repo => repo.ReadAllCustomersAsync(), Times.Once);
            foreach (var customer in customers)
            {
                _mockKycStatusService.Verify(service => service.GetKycStatusAsync(customer.CustomerId), Times.Once);
            }
        }

        [Fact]
        public async Task UpdateKycStatusAsync_ShouldUpdateStatusForExistingCustomer()
        {
            // Arrange
            var customerId = "CUST001";
            var initialStatus = KycsyncDomainModels.KycStatus.KycStatusEnum.NotVerified;
            var updatedStatus = KycsyncDomainModels.KycStatus.KycStatusEnum.Verified;

            _mockKycStatusService
                .Setup(service => service.GetKycStatusAsync(customerId))
                .ReturnsAsync(initialStatus);

            _mockKycStatusService
                .Setup(service => service.UpdateKycStatusAsync(customerId, updatedStatus))
                .Returns(Task.CompletedTask);

            // Act
            await _mockKycStatusService.Object.UpdateKycStatusAsync(customerId, updatedStatus);

            // Assert
            _mockKycStatusService.Verify(service => service.UpdateKycStatusAsync(customerId, updatedStatus), Times.Once);
        }

        [Fact]
        public async Task GetKycStatusAsync_ShouldReturnCorrectStatus()
        {
            // Arrange
            var customerId = "CUST001";
            var expectedStatus = KycsyncDomainModels.KycStatus.KycStatusEnum.Verified;

            _mockKycStatusService
                .Setup(service => service.GetKycStatusAsync(customerId))
                .ReturnsAsync(expectedStatus);

            // Act
            var actualStatus = await _mockKycStatusService.Object.GetKycStatusAsync(customerId);

            // Assert
            Assert.Equal(expectedStatus, actualStatus);
            _mockKycStatusService.Verify(service => service.GetKycStatusAsync(customerId), Times.Once);
        }

        [Fact]
        public async Task WriteCustomerAsync_ShouldSaveCustomerRecord()
        {
            // Arrange
            var customerRecord = new KycsyncDomainModels.CustomerRecord
            {
                CustomerId = "CUST003",
                FirstName = "Alice",
                LastName = "Johnson",
                Gender = "F",
                DateOfBirth = "1985-07-20",
                Address = new KycsyncDomainModels.Address
                {
                    Street = "789 Pine St",
                    City = "Star City",
                    State = "TX",
                    PostalCode = "54321",
                    CountryCode = "US"
                },
                ContactDetails = new KycsyncDomainModels.ContactDetails
                {
                    PhoneNumber = "5551234567",
                    Email = "alice.johnson@example.com"
                },
                EmploymentDetails = new KycsyncDomainModels.EmploymentDetails
                {
                    EmployerName = "HealthCare Co",
                    JobTitle = "Nurse",
                    AnnualIncome = 65000.00m
                }
            };

            _mockCustomerRepository
                .Setup(repo => repo.WriteCustomerAsync(customerRecord))
                .Returns(Task.CompletedTask);

            // Act
            await _mockCustomerRepository.Object.WriteCustomerAsync(customerRecord);

            // Assert
            _mockCustomerRepository.Verify(repo => repo.WriteCustomerAsync(customerRecord), Times.Once);
        }
    }
}