using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using KYCSYNC.Application;
using KYCSYNC.Infrastructure.Interfaces;

namespace KYCSYNC.Tests
{
    public class KycsyncParityTests
    {
        private readonly Mock<IRecordRepository> _mockRecordRepository;
        private readonly Mock<ILoggingService> _mockLoggingService;
        private readonly Mock<IEnvironmentService> _mockEnvironmentService;
        private readonly KycsyncService _kycsyncService;

        public KycsyncParityTests()
        {
            _mockRecordRepository = new Mock<IRecordRepository>();
            _mockLoggingService = new Mock<ILoggingService>();
            _mockEnvironmentService = new Mock<IEnvironmentService>();

            _kycsyncService = new KycsyncService(
                _mockRecordRepository.Object,
                _mockLoggingService.Object,
                _mockEnvironmentService.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldInitializeProcessAndFinalize()
        {
            // Arrange
            _mockEnvironmentService.Setup(x => x.SetupEnvironmentAsync()).Returns(Task.CompletedTask);
            _mockEnvironmentService.Setup(x => x.CleanupEnvironmentAsync()).Returns(Task.CompletedTask);
            _mockLoggingService.Setup(x => x.LogInfoAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _mockLoggingService.Setup(x => x.LogErrorAsync(It.IsAny<string>(), It.IsAny<Exception>())).Returns(Task.CompletedTask);
            _mockRecordRepository.SetupSequence(x => x.FetchNextRecordAsync())
                .ReturnsAsync(new Record { Id = "1" })
                .ReturnsAsync((Record)null);
            _mockRecordRepository.Setup(x => x.ProcessRecordAsync(It.IsAny<Record>())).Returns(Task.CompletedTask);

            // Act
            await _kycsyncService.ExecuteAsync();

            // Assert
            _mockLoggingService.Verify(x => x.LogInfoAsync("Initializing KYCSYNC service..."), Times.Once);
            _mockEnvironmentService.Verify(x => x.SetupEnvironmentAsync(), Times.Once);
            _mockLoggingService.Verify(x => x.LogInfoAsync("Environment setup completed."), Times.Once);
            _mockLoggingService.Verify(x => x.LogInfoAsync("Starting record processing..."), Times.Once);
            _mockLoggingService.Verify(x => x.LogInfoAsync("Record processing completed."), Times.Once);
            _mockLoggingService.Verify(x => x.LogInfoAsync("Finalizing KYCSYNC service..."), Times.Once);
            _mockEnvironmentService.Verify(x => x.CleanupEnvironmentAsync(), Times.Once);
            _mockLoggingService.Verify(x => x.LogInfoAsync("Finalization completed."), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldLogErrorOnInitializationFailure()
        {
            // Arrange
            var exception = new Exception("Initialization error");
            _mockEnvironmentService.Setup(x => x.SetupEnvironmentAsync()).ThrowsAsync(exception);
            _mockLoggingService.Setup(x => x.LogErrorAsync(It.IsAny<string>(), It.IsAny<Exception>())).Returns(Task.CompletedTask);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _kycsyncService.ExecuteAsync());
            _mockLoggingService.Verify(x => x.LogErrorAsync("Initialization failed: Initialization error", exception), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldLogErrorOnRecordProcessingFailure()
        {
            // Arrange
            var exception = new Exception("Processing error");
            _mockEnvironmentService.Setup(x => x.SetupEnvironmentAsync()).Returns(Task.CompletedTask);
            _mockEnvironmentService.Setup(x => x.CleanupEnvironmentAsync()).Returns(Task.CompletedTask);
            _mockLoggingService.Setup(x => x.LogInfoAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _mockLoggingService.Setup(x => x.LogErrorAsync(It.IsAny<string>(), It.IsAny<Exception>())).Returns(Task.CompletedTask);
            _mockRecordRepository.SetupSequence(x => x.FetchNextRecordAsync())
                .ReturnsAsync(new Record { Id = "1" })
                .ReturnsAsync((Record)null);
            _mockRecordRepository.Setup(x => x.ProcessRecordAsync(It.IsAny<Record>())).ThrowsAsync(exception);

            // Act
            await _kycsyncService.ExecuteAsync();

            // Assert
            _mockLoggingService.Verify(x => x.LogErrorAsync("Error processing record: Processing error", exception), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldLogErrorOnFinalizationFailure()
        {
            // Arrange
            var exception = new Exception("Finalization error");
            _mockEnvironmentService.Setup(x => x.SetupEnvironmentAsync()).Returns(Task.CompletedTask);
            _mockEnvironmentService.Setup(x => x.CleanupEnvironmentAsync()).ThrowsAsync(exception);
            _mockLoggingService.Setup(x => x.LogInfoAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _mockLoggingService.Setup(x => x.LogErrorAsync(It.IsAny<string>(), It.IsAny<Exception>())).Returns(Task.CompletedTask);
            _mockRecordRepository.SetupSequence(x => x.FetchNextRecordAsync())
                .ReturnsAsync(new Record { Id = "1" })
                .ReturnsAsync((Record)null);
            _mockRecordRepository.Setup(x => x.ProcessRecordAsync(It.IsAny<Record>())).Returns(Task.CompletedTask);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _kycsyncService.ExecuteAsync());
            _mockLoggingService.Verify(x => x.LogErrorAsync("Finalization failed: Finalization error", exception), Times.Once);
        }
    }

    // Mocked Record class for testing purposes
    public class Record
    {
        public string Id { get; set; }
    }
}