using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using INTCALC.Application;
using INTCALC.Application.Interfaces;
using INTCALC.Domain;
using INTCALC.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace INTCALC.Tests
{
    public class IntcalcParityTests
    {
        private readonly DbContextOptions<IntcalcDbContext> _dbContextOptions;

        public IntcalcParityTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<IntcalcDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessInterestCalculations_HappyPath()
        {
            // Arrange
            await using var context = new IntcalcDbContext(_dbContextOptions);
            var repository = new InterestCalculationRepository(context);
            var service = new IntcalcService();

            var seedCalculation = new IntcalcDomainModels.InterestCalculation
            {
                AccountNumber = "1234567890",
                PrincipalAmount = 1000.00m,
                InterestRate = 5.00m,
                CalculationDate = "20231010",
                Status = IntcalcDomainModels.CalculationStatus.Pending
            };
            await repository.AddAsync(seedCalculation);
            await context.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var calculations = await repository.GetAllAsync();
            calculations.Should().HaveCount(1);

            var calculation = calculations.First();
            calculation.AccountNumber.Should().Be("1234567890");
            calculation.PrincipalAmount.Should().Be(1000.00m);
            calculation.InterestRate.Should().Be(5.00m);
            calculation.InterestAmount.Should().Be(50.00m); // Assuming interest calculation logic
            calculation.Status.Should().Be(IntcalcDomainModels.CalculationStatus.Completed);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyInputGracefully()
        {
            // Arrange
            await using var context = new IntcalcDbContext(_dbContextOptions);
            var repository = new InterestCalculationRepository(context);
            var service = new IntcalcService();

            // Act
            await service.ExecuteAsync();

            // Assert
            var calculations = await repository.GetAllAsync();
            calculations.Should().BeEmpty();
        }

        [Theory]
        [InlineData(9999999999999.99, 99.99, 999999999999.00)]
        [InlineData(0.01, 0.01, 0.00)]
        public async Task ExecuteAsync_ShouldProcessBoundaryValues(decimal principal, decimal rate, decimal expectedInterest)
        {
            // Arrange
            await using var context = new IntcalcDbContext(_dbContextOptions);
            var repository = new InterestCalculationRepository(context);
            var service = new IntcalcService();

            var seedCalculation = new IntcalcDomainModels.InterestCalculation
            {
                AccountNumber = "9999999999",
                PrincipalAmount = principal,
                InterestRate = rate,
                CalculationDate = "20231010",
                Status = IntcalcDomainModels.CalculationStatus.Pending
            };
            await repository.AddAsync(seedCalculation);
            await context.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var calculations = await repository.GetAllAsync();
            calculations.Should().HaveCount(1);

            var calculation = calculations.First();
            calculation.AccountNumber.Should().Be("9999999999");
            calculation.PrincipalAmount.Should().Be(principal);
            calculation.InterestRate.Should().Be(rate);
            calculation.InterestAmount.Should().Be(expectedInterest);
            calculation.Status.Should().Be(IntcalcDomainModels.CalculationStatus.Completed);
        }
    }
}