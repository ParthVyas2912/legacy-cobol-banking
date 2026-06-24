using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using INTCALC.Application;
using INTCALC.Application.Interfaces;
using INTCALC.Domain;
using INTCALC.Infrastructure;

namespace INTCALC.Tests
{
    public class IntcalcParityTests
    {
        private readonly ServiceProvider _serviceProvider;

        public IntcalcParityTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<IntcalcDbContext>(options =>
                options.UseInMemoryDatabase("IntcalcTestDb"));
            services.AddScoped<IInterestCalculationRepository, InterestCalculationRepository>();
            services.AddScoped<ICicsService, MockCicsService>();
            services.AddScoped<IInputFileRepository, MockInputFileRepository>();
            services.AddScoped<IOutputFileRepository, MockOutputFileRepository>();
            services.AddScoped<IIntcalcService, IntcalcService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessInterestCalculationsCorrectly()
        {
            // Arrange
            var dbContext = _serviceProvider.GetRequiredService<IntcalcDbContext>();
            var repository = _serviceProvider.GetRequiredService<IInterestCalculationRepository>();
            var service = _serviceProvider.GetRequiredService<IIntcalcService>();

            SeedDatabase(dbContext);

            // Act
            await service.ExecuteAsync();

            // Assert
            var calculations = await repository.GetAllAsync();
            calculations.Should().HaveCount(2);

            var calculation1 = calculations.First(c => c.AccountNumber == "1234567890");
            calculation1.InterestAmount.Should().Be(50.00m);

            var calculation2 = calculations.First(c => c.AccountNumber == "0987654321");
            calculation2.InterestAmount.Should().Be(100.00m);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyInputGracefully()
        {
            // Arrange
            var service = _serviceProvider.GetRequiredService<IIntcalcService>();

            // Act
            Func<Task> act = async () => await service.ExecuteAsync();

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleBoundaryValuesCorrectly()
        {
            // Arrange
            var dbContext = _serviceProvider.GetRequiredService<IntcalcDbContext>();
            var repository = _serviceProvider.GetRequiredService<IInterestCalculationRepository>();
            var service = _serviceProvider.GetRequiredService<IIntcalcService>();

            dbContext.Acctfiles.Add(new Acctfile
            {
                AcctNumber = "9999999999",
                AcctBalance = 999999999999.99m,
                AcctIntRate = 99.99m,
                AcctLastTxnDt = DateTime.ParseExact("20231231", "yyyyMMdd", null)
            });
            await dbContext.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var calculation = await repository.GetByAccountAndDateAsync("9999999999", "20231231");
            calculation.Should().NotBeNull();
            calculation!.InterestAmount.Should().Be(999999999999.99m * 99.99m / 100);
        }

        private void SeedDatabase(IntcalcDbContext dbContext)
        {
            dbContext.Acctfiles.AddRange(
                new Acctfile
                {
                    AcctNumber = "1234567890",
                    AcctBalance = 1000.00m,
                    AcctIntRate = 5.00m,
                    AcctLastTxnDt = DateTime.ParseExact("20230101", "yyyyMMdd", null)
                },
                new Acctfile
                {
                    AcctNumber = "0987654321",
                    AcctBalance = 2000.00m,
                    AcctIntRate = 5.00m,
                    AcctLastTxnDt = DateTime.ParseExact("20230101", "yyyyMMdd", null)
                }
            );
            dbContext.SaveChanges();
        }

        private class MockCicsService : ICicsService
        {
            public Task LinkToExternalServiceAsync(IntcalcDomainModels.InterestCalculation calculation)
            {
                return Task.CompletedTask;
            }
        }

        private class MockInputFileRepository : IInputFileRepository
        {
            private readonly Queue<IntcalcService.WorkingStorage.Record> _records = new();

            public MockInputFileRepository()
            {
                _records.Enqueue(new IntcalcService.WorkingStorage.Record
                {
                    Principal = 1000.00m,
                    Rate = 5.00m
                });
                _records.Enqueue(new IntcalcService.WorkingStorage.Record
                {
                    Principal = 2000.00m,
                    Rate = 5.00m
                });
            }

            public Task<IntcalcService.WorkingStorage.Record> ReadNextRecordAsync()
            {
                return Task.FromResult(_records.Count > 0 ? _records.Dequeue() : null);
            }

            public Task CloseAsync()
            {
                return Task.CompletedTask;
            }
        }

        private class MockOutputFileRepository : IOutputFileRepository
        {
            public Task WriteRecordAsync(IntcalcService.WorkingStorage.Record record)
            {
                return Task.CompletedTask;
            }

            public Task CloseAsync()
            {
                return Task.CompletedTask;
            }
        }
    }
}