using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FRDCHK.Application;
using FRDCHK.Application.Interfaces;
using FRDCHK.Domain;
using FRDCHK.Infrastructure;

namespace FRDCHK.Tests
{
    public class FrdchkParityTests
    {
        private readonly IServiceProvider _serviceProvider;

        public FrdchkParityTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<FrdchkDbContext>(options =>
                options.UseInMemoryDatabase("FrdchkTestDb"));
            services.AddScoped<IFraudCheckRepository, FraudCheckRepository>();
            services.AddScoped<ICicsFraudCheckService, CicsFraudCheckService>();
            services.AddScoped<IFrdchkService, FrdchkService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessRecordsCorrectly()
        {
            // Arrange
            var dbContext = _serviceProvider.GetRequiredService<FrdchkDbContext>();
            var repository = _serviceProvider.GetRequiredService<IFraudCheckRepository>();
            var service = _serviceProvider.GetRequiredService<IFrdchkService>();

            // Seed data
            var fraudCheckRecord = new FrdchkDomainModels.FraudCheckRecord
            {
                RecordId = "REC1234567",
                CustomerName = "John Doe",
                TransactionAmount = 12345.67m,
                TransactionDate = new DateTime(2023, 10, 1),
                FraudStatus = FrdchkDomainModels.FraudStatusEnum.Unknown,
                TransactionDetails = new List<FrdchkDomainModels.FraudCheckRecord.TransactionDetail>
                {
                    new FrdchkDomainModels.FraudCheckRecord.TransactionDetail
                    {
                        DetailId = "DET123456789012",
                        Amount = 1234.56m,
                        Description = "Test Transaction 1"
                    },
                    new FrdchkDomainModels.FraudCheckRecord.TransactionDetail
                    {
                        DetailId = "DET987654321098",
                        Amount = 5678.90m,
                        Description = "Test Transaction 2"
                    }
                }
            };

            await repository.AddAsync(fraudCheckRecord);
            await dbContext.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var records = await repository.GetAllAsync();
            records.Should().HaveCount(1);

            var processedRecord = records[0];
            processedRecord.RecordId.Should().Be("REC1234567");
            processedRecord.CustomerName.Should().Be("John Doe");
            processedRecord.TransactionAmount.Should().Be(12345.67m);
            processedRecord.TransactionDate.Should().Be(new DateTime(2023, 10, 1));
            processedRecord.FraudStatus.Should().NotBe(FrdchkDomainModels.FraudStatusEnum.Unknown);

            processedRecord.TransactionDetails.Should().HaveCount(2);
            processedRecord.TransactionDetails[0].DetailId.Should().Be("DET123456789012");
            processedRecord.TransactionDetails[0].Amount.Should().Be(1234.56m);
            processedRecord.TransactionDetails[0].Description.Should().Be("Test Transaction 1");
            processedRecord.TransactionDetails[1].DetailId.Should().Be("DET987654321098");
            processedRecord.TransactionDetails[1].Amount.Should().Be(5678.90m);
            processedRecord.TransactionDetails[1].Description.Should().Be("Test Transaction 2");
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyInputGracefully()
        {
            // Arrange
            var service = _serviceProvider.GetRequiredService<IFrdchkService>();
            var repository = _serviceProvider.GetRequiredService<IFraudCheckRepository>();

            // Act
            await service.ExecuteAsync();

            // Assert
            var records = await repository.GetAllAsync();
            records.Should().BeEmpty();
        }

        [Theory]
        [InlineData("REC9999999", "Max Customer Name", 9999999999999.99, "20231001", FrdchkDomainModels.FraudStatusEnum.Unknown)]
        public async Task ExecuteAsync_ShouldHandleBoundaryValues(string recordId, string customerName, decimal transactionAmount, string transactionDate, FrdchkDomainModels.FraudStatusEnum fraudStatus)
        {
            // Arrange
            var dbContext = _serviceProvider.GetRequiredService<FrdchkDbContext>();
            var repository = _serviceProvider.GetRequiredService<IFraudCheckRepository>();
            var service = _serviceProvider.GetRequiredService<IFrdchkService>();

            var fraudCheckRecord = new FrdchkDomainModels.FraudCheckRecord
            {
                RecordId = recordId,
                CustomerName = customerName,
                TransactionAmount = transactionAmount,
                TransactionDate = DateTime.ParseExact(transactionDate, "yyyyMMdd", null),
                FraudStatus = fraudStatus,
                TransactionDetails = new List<FrdchkDomainModels.FraudCheckRecord.TransactionDetail>
                {
                    new FrdchkDomainModels.FraudCheckRecord.TransactionDetail
                    {
                        DetailId = "MAXDETAIL123456",
                        Amount = 9999999999.99m,
                        Description = "Max Boundary Transaction"
                    }
                }
            };

            await repository.AddAsync(fraudCheckRecord);
            await dbContext.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var records = await repository.GetAllAsync();
            records.Should().HaveCount(1);

            var processedRecord = records[0];
            processedRecord.RecordId.Should().Be(recordId);
            processedRecord.CustomerName.Should().Be(customerName);
            processedRecord.TransactionAmount.Should().Be(transactionAmount);
            processedRecord.TransactionDate.Should().Be(DateTime.ParseExact(transactionDate, "yyyyMMdd", null));
            processedRecord.FraudStatus.Should().NotBe(fraudStatus);

            processedRecord.TransactionDetails.Should().HaveCount(1);
            processedRecord.TransactionDetails[0].DetailId.Should().Be("MAXDETAIL123456");
            processedRecord.TransactionDetails[0].Amount.Should().Be(9999999999.99m);
            processedRecord.TransactionDetails[0].Description.Should().Be("Max Boundary Transaction");
        }
    }
}