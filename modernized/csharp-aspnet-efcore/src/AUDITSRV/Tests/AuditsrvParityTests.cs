using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUDITSRV.Application;
using AUDITSRV.Application.Interfaces;
using AUDITSRV.Domain;
using Moq;
using Xunit;

namespace AUDITSRV.Tests
{
    public class AuditsrvParityTests
    {
        private readonly Mock<IAuditRecordRepository> _mockAuditRecordRepository;
        private readonly Mock<IAuditDetailRepository> _mockAuditDetailRepository;
        private readonly Mock<ICicsService> _mockCicsService;
        private readonly AuditsrvService _service;

        public AuditsrvParityTests()
        {
            _mockAuditRecordRepository = new Mock<IAuditRecordRepository>();
            _mockAuditDetailRepository = new Mock<IAuditDetailRepository>();
            _mockCicsService = new Mock<ICicsService>();
            _service = new AuditsrvService();
        }

        [Fact]
        public async Task ExecuteAsync_HappyPath_ShouldProcessAuditRecordsCorrectly()
        {
            // Arrange
            var auditRecord = new AuditsrvDomainModels.AuditRecord
            {
                RecordId = "REC001",
                UserName = "TestUser",
                ActionCode = "CREATE",
                Amount = 12345.67m,
                Timestamp = "20231010",
                Status = AuditsrvDomainModels.AuditStatus.Pending,
                Details = new List<AuditsrvDomainModels.AuditDetail>
                {
                    new AuditsrvDomainModels.AuditDetail
                    {
                        DetailCode = "D001",
                        Description = "Detail 1",
                        DetailAmount = 100.00m
                    },
                    new AuditsrvDomainModels.AuditDetail
                    {
                        DetailCode = "D002",
                        Description = "Detail 2",
                        DetailAmount = 200.00m
                    }
                }
            };

            _mockAuditRecordRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<AuditsrvDomainModels.AuditRecord> { auditRecord });

            // Act
            await _service.ExecuteAsync();

            // Assert
            _mockAuditRecordRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockAuditRecordRepository.Verify(repo => repo.UpdateAsync(It.Is<AuditsrvDomainModels.AuditRecord>(r => r.Status == AuditsrvDomainModels.AuditStatus.Approved)), Times.Once);
            _mockAuditRecordRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_EmptyInput_ShouldHandleGracefully()
        {
            // Arrange
            _mockAuditRecordRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<AuditsrvDomainModels.AuditRecord>());

            // Act
            await _service.ExecuteAsync();

            // Assert
            _mockAuditRecordRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockAuditRecordRepository.Verify(repo => repo.UpdateAsync(It.IsAny<AuditsrvDomainModels.AuditRecord>()), Times.Never);
            _mockAuditRecordRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData("REC001", "TestUser", "CREATE", 9999999999999.99, "20231010", AuditsrvDomainModels.AuditStatus.Pending)]
        [InlineData("REC002", "AnotherUser", "UPDATE", 0.01, "20231011", AuditsrvDomainModels.AuditStatus.Pending)]
        public async Task ExecuteAsync_BoundaryValues_ShouldProcessCorrectly(
            string recordId,
            string userName,
            string actionCode,
            decimal amount,
            string timestamp,
            AuditsrvDomainModels.AuditStatus status)
        {
            // Arrange
            var auditRecord = new AuditsrvDomainModels.AuditRecord
            {
                RecordId = recordId,
                UserName = userName,
                ActionCode = actionCode,
                Amount = amount,
                Timestamp = timestamp,
                Status = status,
                Details = new List<AuditsrvDomainModels.AuditDetail>
                {
                    new AuditsrvDomainModels.AuditDetail
                    {
                        DetailCode = "D001",
                        Description = "Boundary Detail",
                        DetailAmount = 0.01m
                    }
                }
            };

            _mockAuditRecordRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<AuditsrvDomainModels.AuditRecord> { auditRecord });

            // Act
            await _service.ExecuteAsync();

            // Assert
            _mockAuditRecordRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockAuditRecordRepository.Verify(repo => repo.UpdateAsync(It.Is<AuditsrvDomainModels.AuditRecord>(r => r.Status == AuditsrvDomainModels.AuditStatus.Approved)), Times.Once);
            _mockAuditRecordRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
    }
}