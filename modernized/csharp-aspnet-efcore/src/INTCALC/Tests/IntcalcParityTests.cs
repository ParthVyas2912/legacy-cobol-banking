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
        private readonly Mock<IInputFileReader> _mockInputFileReader;
        private readonly Mock<IOutputFileWriter> _mockOutputFileWriter;
        private readonly Mock<IInterestCalculator> _mockInterestCalculator;
        private readonly IntcalcService _service;

        public IntcalcParityTests()
        {
            _mockInputFileReader = new Mock<IInputFileReader>();
            _mockOutputFileWriter = new Mock<IOutputFileWriter>();
            _mockInterestCalculator = new Mock<IInterestCalculator>();

            _service = new IntcalcService(
                _mockInputFileReader.Object,
                _mockOutputFileWriter.Object,
                _mockInterestCalculator.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInitializeCalculateAndFinalize()
        {
            // Arrange
            var records = new List<Record>
            {
                new Record { Id = 1, Principal = 1000m, InterestRate = 5m, TermInDays = 30 },
                new Record { Id = 2, Principal = 2000m, InterestRate = 3m, TermInDays = 60 }
            };

            _mockInputFileReader.Setup(x => x.OpenAsync()).Returns(Task.CompletedTask);
            _mockInputFileReader.Setup(x => x.ReadRecordsAsync()).ReturnsAsync(records);
            _mockInputFileReader.Setup(x => x.CloseAsync()).Returns(Task.CompletedTask);

            _mockOutputFileWriter.Setup(x => x.OpenAsync()).Returns(Task.CompletedTask);
            _mockOutputFileWriter.Setup(x => x.WriteRecordAsync(It.IsAny<Record>())).Returns(Task.CompletedTask);
            _mockOutputFileWriter.Setup(x => x.CloseAsync()).Returns(Task.CompletedTask);

            _mockInterestCalculator.Setup(x => x.Calculate(It.IsAny<Record>()))
                .Returns((Record record) =>
                {
                    record.InterestAmount = record.Principal * record.InterestRate / 100;
                    return record;
                });

            // Act
            await _service.ExecuteAsync();

            // Assert
            _mockInputFileReader.Verify(x => x.OpenAsync(), Times.Once);
            _mockInputFileReader.Verify(x => x.ReadRecordsAsync(), Times.Once);
            _mockInputFileReader.Verify(x => x.CloseAsync(), Times.Once);

            _mockOutputFileWriter.Verify(x => x.OpenAsync(), Times.Once);
            _mockOutputFileWriter.Verify(x => x.WriteRecordAsync(It.IsAny<Record>()), Times.Exactly(records.Count));
            _mockOutputFileWriter.Verify(x => x.CloseAsync(), Times.Once);

            _mockInterestCalculator.Verify(x => x.Calculate(It.IsAny<Record>()), Times.Exactly(records.Count));
        }

        [Fact]
        public async Task CalculateInterestAsync_ShouldHandleErrorsGracefully()
        {
            // Arrange
            var records = new List<Record>
            {
                new Record { Id = 1, Principal = 1000m, InterestRate = 5m, TermInDays = 30 },
                new Record { Id = 2, Principal = 2000m, InterestRate = 3m, TermInDays = 60 }
            };

            _mockInputFileReader.Setup(x => x.ReadRecordsAsync()).ReturnsAsync(records);

            _mockInterestCalculator.Setup(x => x.Calculate(It.Is<Record>(r => r.Id == 1)))
                .Throws(new Exception("Calculation error"));

            _mockInterestCalculator.Setup(x => x.Calculate(It.Is<Record>(r => r.Id == 2)))
                .Returns((Record record) =>
                {
                    record.InterestAmount = record.Principal * record.InterestRate / 100;
                    return record;
                });

            // Act
            await _service.ExecuteAsync();

            // Assert
            _mockOutputFileWriter.Verify(x => x.WriteRecordAsync(It.Is<Record>(r => r.Id == 1)), Times.Never);
            _mockOutputFileWriter.Verify(x => x.WriteRecordAsync(It.Is<Record>(r => r.Id == 2)), Times.Once);
        }

        [Fact]
        public async Task InitializeAsync_ShouldOpenInputAndOutputFiles()
        {
            // Arrange
            _mockInputFileReader.Setup(x => x.OpenAsync()).Returns(Task.CompletedTask);
            _mockOutputFileWriter.Setup(x => x.OpenAsync()).Returns(Task.CompletedTask);

            // Act
            await _service.ExecuteAsync();

            // Assert
            _mockInputFileReader.Verify(x => x.OpenAsync(), Times.Once);
            _mockOutputFileWriter.Verify(x => x.OpenAsync(), Times.Once);
        }

        [Fact]
        public async Task FinalizeAsync_ShouldCloseInputAndOutputFiles()
        {
            // Arrange
            _mockInputFileReader.Setup(x => x.CloseAsync()).Returns(Task.CompletedTask);
            _mockOutputFileWriter.Setup(x => x.CloseAsync()).Returns(Task.CompletedTask);

            // Act
            await _service.ExecuteAsync();

            // Assert
            _mockInputFileReader.Verify(x => x.CloseAsync(), Times.Once);
            _mockOutputFileWriter.Verify(x => x.CloseAsync(), Times.Once);
        }
    }
}