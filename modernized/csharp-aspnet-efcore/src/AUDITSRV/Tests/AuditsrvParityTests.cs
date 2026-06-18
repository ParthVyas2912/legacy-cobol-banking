using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AUDITSRV.Application;
using AUDITSRV.Application.Interfaces;
using AUDITSRV.Domain;

namespace AUDITSRV.Tests
{
    public class AuditsrvParityTests
    {
        private readonly Mock<IAuditRepository> _mockAuditRepository;
        private readonly Mock<ILogger<AuditsrvService>> _mockLogger;
        private readonly AuditsrvService _auditService;

        public AuditsrvParityTests()
        {
            _mockAuditRepository = new Mock<IAuditRepository>();
            _mockLogger = new Mock<ILogger<AuditsrvService>>();
            _auditService = new AuditsrvService(_mockAuditRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Call_MainProcessing()
        {
            // Arrange
            _mockAuditRepository.Setup(repo => repo.ReadAuditRecordsAsync()).ReturnsAsync(new List<AuditsrvDomainModels.AuditRecord>());
            _mockAuditRepository.Setup(repo => repo.UpdateAuditRecordAsync(It.IsAny<AuditsrvDomainModels.AuditRecord>())).Returns(Task.CompletedTask);

            // Act
            await _auditService.ExecuteAsync();

            // Assert
            _mockAuditRepository.Verify(repo => repo.ReadAuditRecordsAsync(), Times.Once);
            _mockAuditRepository.Verify(repo => repo.UpdateAuditRecordAsync(It.IsAny<AuditsrvDomainModels.AuditRecord>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task InitializeAuditAsync_Should_Throw_When_Configuration_Is_Null()
        {
            // Arrange
            _mockAuditRepository.Setup(repo => repo.ReadAuditRecordsAsync()).ReturnsAsync((IEnumerable<AuditsrvDomainModels.AuditRecord>)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _auditService.ExecuteAsync());
        }

        [Fact]
        public async Task ProcessAuditRecordsAsync_Should_Log_Warning_When_No_Records()
        {
            // Arrange
            _mockAuditRepository.Setup(repo => repo.ReadAuditRecordsAsync()).ReturnsAsync(new List<AuditsrvDomainModels.AuditRecord>());

            // Act
            await _auditService.ExecuteAsync();

            // Assert
            _mockLogger.Verify(logger => logger.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No audit records found to process.")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public async Task ProcessAuditRecordsAsync_Should_Process_Records()
        {
            // Arrange
            var auditRecords = new List<AuditsrvDomainModels.AuditRecord>
            {
                new AuditsrvDomainModels.AuditRecord { AuditId = "A001", UserId = "User1", ActionDate = "20231010", ActionTime = "123456", ActionDescription = "Test Action 1", Status = AuditsrvDomainModels.AuditStatus.Pending },
                new AuditsrvDomainModels.AuditRecord { AuditId = "A002", UserId = "User2", ActionDate = "20231011", ActionTime = "123457", ActionDescription = "Test Action 2", Status = AuditsrvDomainModels.AuditStatus.Pending }
            };

            _mockAuditRepository.Setup(repo => repo.ReadAuditRecordsAsync()).ReturnsAsync(auditRecords);
            _mockAuditRepository.Setup(repo => repo.UpdateAuditRecordAsync(It.IsAny<AuditsrvDomainModels.AuditRecord>())).Returns(Task.CompletedTask);

            // Act
            await _auditService.ExecuteAsync();

            // Assert
            _mockAuditRepository.Verify(repo => repo.UpdateAuditRecordAsync(It.IsAny<AuditsrvDomainModels.AuditRecord>()), Times.Exactly(auditRecords.Count));
        }

        [Fact]
        public async Task FinalizeAuditAsync_Should_Throw_When_Summary_Generation_Fails()
        {
            // Arrange
            _mockAuditRepository.Setup(repo => repo.ReadAuditRecordsAsync()).ReturnsAsync(new List<AuditsrvDomainModels.AuditRecord>());
            _mockAuditRepository.Setup(repo => repo.UpdateAuditRecordAsync(It.IsAny<AuditsrvDomainModels.AuditRecord>())).ThrowsAsync(new InvalidOperationException());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _auditService.ExecuteAsync());
        }
    }
}