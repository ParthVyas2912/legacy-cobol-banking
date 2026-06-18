using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using STMTRPT.Application;
using STMTRPT.Application.Interfaces;
using STMTRPT.Domain;

namespace STMTRPT.Tests
{
    public class StmtrptParityTests
    {
        private readonly Mock<IFileHandler> _mockFileHandler;
        private readonly Mock<IStatementProcessor> _mockStatementProcessor;
        private readonly Mock<IReportGenerator> _mockReportGenerator;
        private readonly StmtrptService _stmtrptService;

        public StmtrptParityTests()
        {
            _mockFileHandler = new Mock<IFileHandler>();
            _mockStatementProcessor = new Mock<IStatementProcessor>();
            _mockReportGenerator = new Mock<IReportGenerator>();

            _stmtrptService = new StmtrptService(
                _mockFileHandler.Object,
                _mockStatementProcessor.Object,
                _mockReportGenerator.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInitializeProcessAndFinalize()
        {
            // Arrange
            _mockFileHandler.Setup(f => f.OpenInputFileAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _mockFileHandler.Setup(f => f.OpenOutputFileAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _mockFileHandler.Setup(f => f.InitializeTemporaryStorageAsync()).Returns(Task.CompletedTask);
            _mockFileHandler.SetupSequence(f => f.ReadNextRecordAsync())
                .ReturnsAsync(new StatementRecord { AccountNumber = 12345, Amount = 100.50m, Description = "Test Transaction" })
                .ReturnsAsync((StatementRecord)null);
            _mockStatementProcessor.Setup(s => s.ProcessRecordAsync(It.IsAny<StatementRecord>()))
                .ReturnsAsync(new StatementProcessingResult
                {
                    IsSuccess = true,
                    ProcessedRecord = new ProcessedStatementRecord { AccountNumber = 12345, Amount = 100.50m, Description = "Processed Transaction" }
                });
            _mockStatementProcessor.Setup(s => s.GenerateSummaryAsync())
                .ReturnsAsync(new StatementSummary { TotalAccounts = 1, TotalBalance = 100.50m });
            _mockReportGenerator.Setup(r => r.GenerateReportAsync(It.IsAny<StatementSummary>())).Returns(Task.CompletedTask);
            _mockFileHandler.Setup(f => f.CloseFilesAsync()).Returns(Task.CompletedTask);

            // Act
            await _stmtrptService.ExecuteAsync();

            // Assert
            _mockFileHandler.Verify(f => f.OpenInputFileAsync(It.IsAny<string>()), Times.Once);
            _mockFileHandler.Verify(f => f.OpenOutputFileAsync(It.IsAny<string>()), Times.Once);
            _mockFileHandler.Verify(f => f.InitializeTemporaryStorageAsync(), Times.Once);
            _mockFileHandler.Verify(f => f.ReadNextRecordAsync(), Times.Exactly(2));
            _mockStatementProcessor.Verify(s => s.ProcessRecordAsync(It.IsAny<StatementRecord>()), Times.Once);
            _mockFileHandler.Verify(f => f.WriteToOutputFileAsync(It.IsAny<ProcessedStatementRecord>()), Times.Once);
            _mockStatementProcessor.Verify(s => s.GenerateSummaryAsync(), Times.Once);
            _mockReportGenerator.Verify(r => r.GenerateReportAsync(It.IsAny<StatementSummary>()), Times.Once);
            _mockFileHandler.Verify(f => f.CloseFilesAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleProcessingErrors()
        {
            // Arrange
            _mockFileHandler.Setup(f => f.OpenInputFileAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _mockFileHandler.Setup(f => f.OpenOutputFileAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _mockFileHandler.Setup(f => f.InitializeTemporaryStorageAsync()).Returns(Task.CompletedTask);
            _mockFileHandler.SetupSequence(f => f.ReadNextRecordAsync())
                .ReturnsAsync(new StatementRecord { AccountNumber = 12345, Amount = 100.50m, Description = "Test Transaction" })
                .ReturnsAsync((StatementRecord)null);
            _mockStatementProcessor.Setup(s => s.ProcessRecordAsync(It.IsAny<StatementRecord>()))
                .ReturnsAsync(new StatementProcessingResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid record format"
                });
            _mockStatementProcessor.Setup(s => s.GenerateSummaryAsync())
                .ReturnsAsync(new StatementSummary { TotalAccounts = 0, TotalBalance = 0 });
            _mockReportGenerator.Setup(r => r.GenerateReportAsync(It.IsAny<StatementSummary>())).Returns(Task.CompletedTask);
            _mockFileHandler.Setup(f => f.CloseFilesAsync()).Returns(Task.CompletedTask);

            // Act
            await _stmtrptService.ExecuteAsync();

            // Assert
            _mockFileHandler.Verify(f => f.OpenInputFileAsync(It.IsAny<string>()), Times.Once);
            _mockFileHandler.Verify(f => f.OpenOutputFileAsync(It.IsAny<string>()), Times.Once);
            _mockFileHandler.Verify(f => f.InitializeTemporaryStorageAsync(), Times.Once);
            _mockFileHandler.Verify(f => f.ReadNextRecordAsync(), Times.Exactly(2));
            _mockStatementProcessor.Verify(s => s.ProcessRecordAsync(It.IsAny<StatementRecord>()), Times.Once);
            _mockFileHandler.Verify(f => f.WriteToOutputFileAsync(It.IsAny<ProcessedStatementRecord>()), Times.Never);
            _mockStatementProcessor.Verify(s => s.GenerateSummaryAsync(), Times.Once);
            _mockReportGenerator.Verify(r => r.GenerateReportAsync(It.IsAny<StatementSummary>()), Times.Once);
            _mockFileHandler.Verify(f => f.CloseFilesAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowExceptionOnInitializationFailure()
        {
            // Arrange
            _mockFileHandler.Setup(f => f.OpenInputFileAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Failed to open input file"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _stmtrptService.ExecuteAsync());

            _mockFileHandler.Verify(f => f.OpenInputFileAsync(It.IsAny<string>()), Times.Once);
            _mockFileHandler.Verify(f => f.OpenOutputFileAsync(It.IsAny<string>()), Times.Never);
            _mockFileHandler.Verify(f => f.InitializeTemporaryStorageAsync(), Times.Never);
            _mockFileHandler.Verify(f => f.ReadNextRecordAsync(), Times.Never);
            _mockStatementProcessor.Verify(s => s.ProcessRecordAsync(It.IsAny<StatementRecord>()), Times.Never);
            _mockFileHandler.Verify(f => f.WriteToOutputFileAsync(It.IsAny<ProcessedStatementRecord>()), Times.Never);
            _mockStatementProcessor.Verify(s => s.GenerateSummaryAsync(), Times.Never);
            _mockReportGenerator.Verify(r => r.GenerateReportAsync(It.IsAny<StatementSummary>()), Times.Never);
            _mockFileHandler.Verify(f => f.CloseFilesAsync(), Times.Never);
        }
    }
}