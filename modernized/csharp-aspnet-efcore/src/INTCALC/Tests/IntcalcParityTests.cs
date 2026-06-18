using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using INTCALC.Application;
using INTCALC.Application.Interfaces;
using INTCALC.Domain;

namespace INTCALC.Tests
{
    public class IntcalcParityTests
    {
        private readonly Mock<IRecordRepository> _mockRecordRepository;
        private readonly Mock<IOutputRepository> _mockOutputRepository;
        private readonly IntcalcService _service;

        public IntcalcParityTests()
        {
            _mockRecordRepository = new Mock<IRecordRepository>();
            _mockOutputRepository = new Mock<IOutputRepository>();
            _service = new IntcalcService(_mockRecordRepository.Object, _mockOutputRepository.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessRecordsCorrectly()
        {
            // Arrange
            var records = new List<Record>
            {
                new Record { Amount = 100.50m },
                new Record { Amount = 200.75m },
                new Record { Amount = 300.25m }
            };

            _mockRecordRepository
                .Setup(repo => repo.GetRecordsAsync())
                .ReturnsAsync(records);

            decimal expectedTotalAmount = 100.50m + 200.75m + 300.25m;
            int expectedRecordCount = records.Count;

            // Act
            await _service.ExecuteAsync();

            // Assert
            _mockOutputRepository.Verify(repo => repo.SaveResultsAsync(expectedTotalAmount, expectedRecordCount), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyRecords()
        {
            // Arrange
            var records = new List<Record>();

            _mockRecordRepository
                .Setup(repo => repo.GetRecordsAsync())
                .ReturnsAsync(records);

            decimal expectedTotalAmount = 0m;
            int expectedRecordCount = 0;

            // Act
            await _service.ExecuteAsync();

            // Assert
            _mockOutputRepository.Verify(repo => repo.SaveResultsAsync(expectedTotalAmount, expectedRecordCount), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowException_WhenRecordIsNull()
        {
            // Arrange
            var records = new List<Record> { null };

            _mockRecordRepository
                .Setup(repo => repo.GetRecordsAsync())
                .ReturnsAsync(records);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.ExecuteAsync());
        }
    }
}