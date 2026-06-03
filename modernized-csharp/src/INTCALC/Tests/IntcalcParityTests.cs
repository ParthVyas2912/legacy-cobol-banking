using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using INTCALC.Application.Interfaces;
using INTCALC.Application;
using INTCALC.Domain;

namespace INTCALC.Tests
{
    public class IntcalcParityTests
    {
        private readonly Mock<IAccountFileRepository> _mockAccountFileRepository;
        private readonly IntcalcService _intcalcService;

        public IntcalcParityTests()
        {
            _mockAccountFileRepository = new Mock<IAccountFileRepository>();
            _intcalcService = new IntcalcService(_mockAccountFileRepository.Object);
        }

        [Fact]
        public async Task Intcalc_HappyPath_Test()
        {
            // Arrange
            _mockAccountFileRepository.Setup(repo => repo.OpenAccountFileAsync()).Returns(Task.CompletedTask);
            _mockAccountFileRepository.Setup(repo => repo.CloseAccountFileAsync()).Returns(Task.CompletedTask);
            _mockAccountFileRepository.SetupSequence(repo => repo.ReadAccountFileAsync())
                .Returns(Task.FromResult(true)) // Simulate reading the first record
                .Returns(Task.FromResult(false)); // Simulate end of file
            _mockAccountFileRepository.Setup(repo => repo.WriteInterestJournalAsync()).Returns(Task.CompletedTask);

            // Act
            await _intcalcService.ExecuteMainAsync();

            // Assert
            _mockAccountFileRepository.Verify(repo => repo.OpenAccountFileAsync(), Times.Once);
            _mockAccountFileRepository.Verify(repo => repo.CloseAccountFileAsync(), Times.Once);
            _mockAccountFileRepository.Verify(repo => repo.ReadAccountFileAsync(), Times.AtLeastOnce);
            _mockAccountFileRepository.Verify(repo => repo.WriteInterestJournalAsync(), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Intcalc_EmptyFile_Test()
        {
            // Arrange
            _mockAccountFileRepository.Setup(repo => repo.OpenAccountFileAsync()).Returns(Task.CompletedTask);
            _mockAccountFileRepository.Setup(repo => repo.CloseAccountFileAsync()).Returns(Task.CompletedTask);
            _mockAccountFileRepository.SetupSequence(repo => repo.ReadAccountFileAsync())
                .Returns(Task.FromResult(false)); // Simulate no records in the file

            // Act
            await _intcalcService.ExecuteMainAsync();

            // Assert
            _mockAccountFileRepository.Verify(repo => repo.OpenAccountFileAsync(), Times.Once);
            _mockAccountFileRepository.Verify(repo => repo.CloseAccountFileAsync(), Times.Once);
            _mockAccountFileRepository.Verify(repo => repo.ReadAccountFileAsync(), Times.AtLeastOnce);
            _mockAccountFileRepository.Verify(repo => repo.WriteInterestJournalAsync(), Times.Never);
        }
    }
}