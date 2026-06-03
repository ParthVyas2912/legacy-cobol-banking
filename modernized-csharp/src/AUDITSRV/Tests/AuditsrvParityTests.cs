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
        private readonly Mock<IAuditFileRepository> _mockAuditFileRepository;
        private readonly Mock<ICicsService> _mockCicsService;
        private readonly AuditsrvService _auditsrvService;

        public AuditsrvParityTests()
        {
            _mockAuditFileRepository = new Mock<IAuditFileRepository>();
            _mockCicsService = new Mock<ICicsService>();
            _auditsrvService = new AuditsrvService(_mockAuditFileRepository.Object, _mockCicsService.Object);
        }

        [Fact]
        public async Task Auditsrv_HappyPath_ShouldWriteAuditRecordFoundResponse()
        {
            // Arrange
            var sampleAuditRecord = new AuditFile
            {
                AuditKey = "AAAAAAAAAAAAAAAAAAAA",
                AuditAcctNumber = 1,
                AuditAction = "AAAAAAAAAAAAAAAA",
                AuditUser = "AAAAAAAA",
                AuditTs = "AAAAAAAAAAAAAAAAAAAAAAAAAA",
                AuditDetail = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
            };

            _mockAuditFileRepository
                .Setup(repo => repo.ReadAuditRecordAsync(It.IsAny<string>()))
                .ReturnsAsync(sampleAuditRecord);

            // Act
            await _auditsrvService.StartAsync();

            // Assert
            _mockAuditFileRepository.Verify(repo => repo.ReadAuditRecordAsync(It.IsAny<string>()), Times.Once);
            _mockCicsService.Verify(cics => cics.WriteResponseAsync($"Audit record found: {sampleAuditRecord.AuditKey}"), Times.Once);
            _mockCicsService.Verify(cics => cics.ReturnControlAsync(), Times.Once);
        }

        [Fact]
        public async Task Auditsrv_EmptyFile_ShouldWriteAuditRecordNotFoundResponse()
        {
            // Arrange
            _mockAuditFileRepository
                .Setup(repo => repo.ReadAuditRecordAsync(It.IsAny<string>()))
                .ReturnsAsync((AuditFile)null);

            // Act
            await _auditsrvService.StartAsync();

            // Assert
            _mockAuditFileRepository.Verify(repo => repo.ReadAuditRecordAsync(It.IsAny<string>()), Times.Once);
            _mockCicsService.Verify(cics => cics.WriteResponseAsync("Audit record not found."), Times.Once);
            _mockCicsService.Verify(cics => cics.ReturnControlAsync(), Times.Once);
        }
    }
}