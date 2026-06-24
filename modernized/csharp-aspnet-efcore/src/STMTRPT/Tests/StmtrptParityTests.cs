using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using STMTRPT.Application;
using STMTRPT.Application.Interfaces;
using STMTRPT.Domain;
using STMTRPT.Infrastructure;
using Xunit;

namespace STMTRPT.Tests
{
    public class StmtrptParityTests
    {
        private readonly IServiceProvider _serviceProvider;

        public StmtrptParityTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<StmtrptDbContext>(options =>
                options.UseInMemoryDatabase("StmtrptTestDb"));
            services.AddScoped<IStatementReportRepository, StatementReportRepository>();
            services.AddScoped<IStmtrptService, StmtrptService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessRecordsAndGenerateReport_HappyPath()
        {
            // Arrange
            var dbContext = _serviceProvider.GetRequiredService<StmtrptDbContext>();
            var repository = _serviceProvider.GetRequiredService<IStatementReportRepository>();
            var service = _serviceProvider.GetRequiredService<IStmtrptService>();

            // Seed data
            var stmtout = new Stmtout
            {
                StmtAcctNum = "1234567890",
                StmtDescription = "John Doe",
                StmtAmount = 1000.50m,
                StmtBalance = 5000.75m
            };
            dbContext.Stmtout.Add(stmtout);
            await dbContext.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var reports = await repository.GetAllAsync();
            reports.Should().HaveCount(1);

            var report = reports.First();
            report.AccountNumber.Should().Be("1234567890");
            report.AccountHolderName.Should().Be("John Doe");
            report.AvailableBalance.Should().Be(1000.50m);
            report.CurrentBalance.Should().Be(5000.75m);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyInputGracefully()
        {
            // Arrange
            var service = _serviceProvider.GetRequiredService<IStmtrptService>();

            // Act
            Func<Task> act = async () => await service.ExecuteAsync();

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessBoundaryValuesCorrectly()
        {
            // Arrange
            var dbContext = _serviceProvider.GetRequiredService<StmtrptDbContext>();
            var repository = _serviceProvider.GetRequiredService<IStatementReportRepository>();
            var service = _serviceProvider.GetRequiredService<IStmtrptService>();

            // Seed data with boundary values
            var stmtout = new Stmtout
            {
                StmtAcctNum = "9999999999", // Max PIC X(10)
                StmtDescription = "Max Boundary Test", // Max PIC X(40)
                StmtAmount = 99999999999.99m, // Max PIC -Z(11)9.99
                StmtBalance = 9999999999999.99m // Max PIC -Z(13)9.99
            };
            dbContext.Stmtout.Add(stmtout);
            await dbContext.SaveChangesAsync();

            // Act
            await service.ExecuteAsync();

            // Assert
            var reports = await repository.GetAllAsync();
            reports.Should().HaveCount(1);

            var report = reports.First();
            report.AccountNumber.Should().Be("9999999999");
            report.AccountHolderName.Should().Be("Max Boundary Test");
            report.AvailableBalance.Should().Be(99999999999.99m);
            report.CurrentBalance.Should().Be(9999999999999.99m);
        }
    }
}