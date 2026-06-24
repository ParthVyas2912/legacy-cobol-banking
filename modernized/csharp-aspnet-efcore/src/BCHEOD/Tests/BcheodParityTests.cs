using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BCHEOD.Domain;
using BCHEOD.Application;
using BCHEOD.Application.Interfaces;
using BCHEOD.Infrastructure;

namespace BCHEOD.Tests
{
    public class BcheodParityTests
    {
        private readonly DbContextOptions<BcheodDbContext> _dbContextOptions;

        public BcheodParityTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<BcheodDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private IBcheodService CreateService(out BcheodDbContext dbContext, out BcheodRepository repository)
        {
            dbContext = new BcheodDbContext(_dbContextOptions);
            repository = new BcheodRepository(dbContext);
            return new BcheodService();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessRecordsCorrectly()
        {
            // Arrange
            var service = CreateService(out var dbContext, out var repository);

            var record = new BcheodDomainModels.BcheodRecord
            {
                RecordId = "REC001",
                Description = "Test Record",
                Amount = 12345.67m,
                TransactionDate = new DateTime(2023, 1, 1),
                TransactionDetails = new List<BcheodDomainModels.BcheodRecord.TransactionDetail>
                {
                    new BcheodDomainModels.BcheodRecord.TransactionDetail
                    {
                        DetailCode = "D001",
                        DetailAmount = 100.50m,
                        DetailDescription = "Detail 1"
                    },
                    new BcheodDomainModels.BcheodRecord.TransactionDetail
                    {
                        DetailCode = "D002",
                        DetailAmount = 200.75m,
                        DetailDescription = "Detail 2"
                    }
                }
            };

            await repository.AddAsync(record);

            // Act
            await service.ExecuteAsync();

            // Assert
            var records = await repository.GetAllAsync();
            records.Should().HaveCount(1);

            var processedRecord = records[0];
            processedRecord.RecordId.Should().Be("REC001");
            processedRecord.Description.Should().Be("Test Record");
            processedRecord.Amount.Should().Be(12345.67m);
            processedRecord.TransactionDate.Should().Be(new DateTime(2023, 1, 1));
            processedRecord.TransactionDetails.Should().HaveCount(2);

            var detail1 = processedRecord.TransactionDetails[0];
            detail1.DetailCode.Should().Be("D001");
            detail1.DetailAmount.Should().Be(100.50m);
            detail1.DetailDescription.Should().Be("Detail 1");

            var detail2 = processedRecord.TransactionDetails[1];
            detail2.DetailCode.Should().Be("D002");
            detail2.DetailAmount.Should().Be(200.75m);
            detail2.DetailDescription.Should().Be("Detail 2");
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyInputGracefully()
        {
            // Arrange
            var service = CreateService(out var dbContext, out var repository);

            // Act
            await service.ExecuteAsync();

            // Assert
            var records = await repository.GetAllAsync();
            records.Should().BeEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleBoundaryValuesCorrectly()
        {
            // Arrange
            var service = CreateService(out var dbContext, out var repository);

            var record = new BcheodDomainModels.BcheodRecord
            {
                RecordId = "1234567890", // Max length for PIC X(10)
                Description = "12345678901234567890", // Max length for PIC X(20)
                Amount = 9999999.99m, // Max value for PIC 9(7)V99 COMP-3
                TransactionDate = new DateTime(9999, 12, 31), // Max value for PIC 9(8)
                TransactionDetails = new List<BcheodDomainModels.BcheodRecord.TransactionDetail>
                {
                    new BcheodDomainModels.BcheodRecord.TransactionDetail
                    {
                        DetailCode = "ABCDE", // Max length for PIC X(5)
                        DetailAmount = 99999.99m, // Max value for PIC 9(5)V99 COMP-3
                        DetailDescription = "123456789012345" // Max length for PIC X(15)
                    }
                }
            };

            await repository.AddAsync(record);

            // Act
            await service.ExecuteAsync();

            // Assert
            var records = await repository.GetAllAsync();
            records.Should().HaveCount(1);

            var processedRecord = records[0];
            processedRecord.RecordId.Should().Be("1234567890");
            processedRecord.Description.Should().Be("12345678901234567890");
            processedRecord.Amount.Should().Be(9999999.99m);
            processedRecord.TransactionDate.Should().Be(new DateTime(9999, 12, 31));
            processedRecord.TransactionDetails.Should().HaveCount(1);

            var detail = processedRecord.TransactionDetails[0];
            detail.DetailCode.Should().Be("ABCDE");
            detail.DetailAmount.Should().Be(99999.99m);
            detail.DetailDescription.Should().Be("123456789012345");
        }
    }
}