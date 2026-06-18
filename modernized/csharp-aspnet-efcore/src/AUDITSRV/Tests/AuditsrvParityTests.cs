using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AUDITSRV.Application;
using AUDITSRV.Application.Interfaces;
using AUDITSRV.Domain;

namespace AUDITSRV.Tests
{
    public class AuditsrvParityTests
    {
        private readonly Mock<IAuditRepository> _mockAuditRepository;
        private readonly Mock<ICicsService> _mockCicsService;
        private readonly AuditsrvService _service;

        public AuditsrvParityTests()
        {
            _mockAuditRepository = new Mock<IAuditRepository>();
            _mockCicsService = new Mock<ICicsService>();
            _service = new AuditsrvService(_mockAuditRepository.Object, _mockCicsService.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessRecordsCorrectly()
        {
            // Arrange
            var testRecord = new AuditsrvDomainModels.AuditRecord
            {
                RecordId = "REC1234567",
                AuditType = "TYPE123",
                Amount = 12345.67m,
                Status = AuditsrvDomainModels.AuditStatus.Pending,
                Details = new AuditsrvDomainModels.AuditDetails
                {
                    Description = "Test Description",
                    CreatedBy = "USER01",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "USER02",
                    UpdatedDate = DateTime.UtcNow
                }
            };

            _mockAuditRepository.Setup(repo => repo.ReadNextRecordAsync())
                .ReturnsAsync(testRecord);

            _mockCicsService.Setup(cics => cics.SendAuditNotificationAsync(It.IsAny<AuditsrvDomainModels.AuditRecord>()))
                .ReturnsAsync(true);

            // Act
            await _service.ExecuteAsync();

            // Assert
            _mockAuditRepository.Verify(repo => repo.ReadNextRecordAsync(), Times.Once);
            _mockCicsService.Verify(cics => cics.SendAuditNotificationAsync(It.Is<AuditsrvDomainModels.AuditRecord>(r => r.RecordId == testRecord.RecordId)), Times.Once);
            _mockAuditRepository.Verify(repo => repo.WriteRecordAsync(It.Is<AuditsrvDomainModels.AuditRecord>(r => r.RecordId == testRecord.RecordId)), Times.Once);
            _mockAuditRepository.Verify(repo => repo.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleNoRecordsGracefully()
        {
            // Arrange
            _mockAuditRepository.Setup(repo => repo.ReadNextRecordAsync())
                .ReturnsAsync((AuditsrvDomainModels.AuditRecord)null);

            // Act
            await _service.ExecuteAsync();

            // Assert
            _mockAuditRepository.Verify(repo => repo.ReadNextRecordAsync(), Times.Once);
            _mockCicsService.Verify(cics => cics.SendAuditNotificationAsync(It.IsAny<AuditsrvDomainModels.AuditRecord>()), Times.Never);
            _mockAuditRepository.Verify(repo => repo.WriteRecordAsync(It.IsAny<AuditsrvDomainModels.AuditRecord>()), Times.Never);
            _mockAuditRepository.Verify(repo => repo.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleCicsFailureGracefully()
        {
            // Arrange
            var testRecord = new AuditsrvDomainModels.AuditRecord
            {
                RecordId = "REC1234567",
                AuditType = "TYPE123",
                Amount = 12345.67m,
                Status = AuditsrvDomainModels.AuditStatus.Pending,
                Details = new AuditsrvDomainModels.AuditDetails
                {
                    Description = "Test Description",
                    CreatedBy = "USER01",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "USER02",
                    UpdatedDate = DateTime.UtcNow
                }
            };

            _mockAuditRepository.Setup(repo => repo.ReadNextRecordAsync())
                .ReturnsAsync(testRecord);

            _mockCicsService.Setup(cics => cics.SendAuditNotificationAsync(It.IsAny<AuditsrvDomainModels.AuditRecord>()))
                .ReturnsAsync(false);

            // Act
            await _service.ExecuteAsync();

            // Assert
            _mockAuditRepository.Verify(repo => repo.ReadNextRecordAsync(), Times.Once);
            _mockCicsService.Verify(cics => cics.SendAuditNotificationAsync(It.Is<AuditsrvDomainModels.AuditRecord>(r => r.RecordId == testRecord.RecordId)), Times.Once);
            _mockAuditRepository.Verify(repo => repo.WriteRecordAsync(It.IsAny<AuditsrvDomainModels.AuditRecord>()), Times.Never);
            _mockAuditRepository.Verify(repo => repo.CommitAsync(), Times.Never);
        }
    }
}