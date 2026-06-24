using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCHEOD.Application;
using BCHEOD.Application.Interfaces;
using BCHEOD.Domain;
using BCHEOD.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;

namespace BCHEOD.Tests
{
    public class BcheodParityTests
    {
        private DbContextOptions<BcheodDbContext> CreateInMemoryDatabaseOptions()
        {
            return new DbContextOptionsBuilder<BcheodDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private BcheodDbContext CreateDbContext(DbContextOptions<BcheodDbContext> options)
        {
            return new BcheodDbContext(options);
        }

        private BcheodService CreateService(BcheodDbContext context)
        {
            var rootRecordRepository = new RootRecordRepository(context);
            var headerRepository = new HeaderRepository(context);
            var detailRepository = new DetailRepository(context);
            var trailerRepository = new TrailerRepository(context);

            // Assuming BcheodService has dependencies injected
            return new BcheodService(rootRecordRepository, headerRepository, detailRepository, trailerRepository);
        }

        [Fact]
        public async Task ExecuteAsync_HappyPath_ShouldProcessRecordsCorrectly()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            await using var context = CreateDbContext(options);

            var service = CreateService(context);

            // Seed data
            var rootRecord = new BcheodDomainModels.RootRecord
            {
                RecordId = "REC1234567",
                Header = new BcheodDomainModels.Header
                {
                    FileDate = "20231010",
                    FileTime = "120000",
                    FileName = "TestFile"
                },
                Details = new List<BcheodDomainModels.Detail>
                {
                    new BcheodDomainModels.Detail
                    {
                        AccountNumber = "ACC123456789012",
                        TransactionAmount = 12345.67m,
                        CurrencyCode = "USD",
                        TransactionType = "D",
                        Status = BcheodDomainModels.TransactionStatus.Pending
                    }
                },
                Trailer = new BcheodDomainModels.Trailer
                {
                    RecordCount = "00000001",
                    TotalAmount = 12345.67m
                }
            };

            await context.RootRecords.AddAsync(rootRecord);
            await context.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var processedRecord = await context.RootRecords.Include(r => r.Details).Include(r => r.Trailer).FirstOrDefaultAsync(r => r.RecordId == "REC1234567");
            processedRecord.Should().NotBeNull();
            processedRecord!.Details.Should().HaveCount(1);
            processedRecord.Details.First().Status.Should().Be(BcheodDomainModels.TransactionStatus.Completed);
            processedRecord.Trailer.TotalAmount.Should().Be(12345.67m);
        }

        [Fact]
        public async Task ExecuteAsync_EmptyInput_ShouldHandleGracefully()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            await using var context = CreateDbContext(options);

            var service = CreateService(context);

            // Act
            await service.ExecuteAsync();

            // Assert
            var records = await context.RootRecords.ToListAsync();
            records.Should().BeEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_BoundaryValues_ShouldProcessCorrectly()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            await using var context = CreateDbContext(options);

            var service = CreateService(context);

            // Seed data with boundary values
            var rootRecord = new BcheodDomainModels.RootRecord
            {
                RecordId = "REC9999999",
                Header = new BcheodDomainModels.Header
                {
                    FileDate = "99999999",
                    FileTime = "235959",
                    FileName = new string('X', 20)
                },
                Details = new List<BcheodDomainModels.Detail>
                {
                    new BcheodDomainModels.Detail
                    {
                        AccountNumber = new string('9', 15),
                        TransactionAmount = 9999999999999.99m,
                        CurrencyCode = "ZZZ",
                        TransactionType = "X",
                        Status = BcheodDomainModels.TransactionStatus.Pending
                    }
                },
                Trailer = new BcheodDomainModels.Trailer
                {
                    RecordCount = "99999999",
                    TotalAmount = 9999999999999.99m
                }
            };

            await context.RootRecords.AddAsync(rootRecord);
            await context.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var processedRecord = await context.RootRecords.Include(r => r.Details).Include(r => r.Trailer).FirstOrDefaultAsync(r => r.RecordId == "REC9999999");
            processedRecord.Should().NotBeNull();
            processedRecord!.Details.Should().HaveCount(1);
            processedRecord.Details.First().Status.Should().Be(BcheodDomainModels.TransactionStatus.Completed);
            processedRecord.Trailer.TotalAmount.Should().Be(9999999999999.99m);
        }
    }
}