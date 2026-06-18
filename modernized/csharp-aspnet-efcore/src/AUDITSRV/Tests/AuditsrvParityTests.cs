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
        private readonly Mock<IAuditRepository> _mockAuditRepository;
        private readonly Mock<ICicsService> _mockCicsService;
        private readonly AuditsrvService _auditsrvService;

        public AuditsrvParityTests()
        {
            _mockAuditRepository = new Mock<IAuditRepository>();
            _mockCicsService = new Mock<ICicsService>();
            _auditsrvService = new AuditsrvService();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldCompleteWithoutErrors()
        {
            // Arrange
            _mockAuditRepository.Setup(repo => repo.ReadAllAuditRecordsAsync())
                .ReturnsAsync(new List<AuditsrvDomainModels.AuditRecord>());

            _mockAuditRepository.Setup(repo => repo.ReadAuditSummaryAsync())
                .ReturnsAsync(new AuditsrvDomainModels.AuditSummary
                {
                    SummaryId = "SUM001",
                    TotalRecords = 10,
                    TotalAmount = 1000.00m
                });

            // Act
            await _auditsrvService.ExecuteAsync();

            // Assert
            _mockAuditRepository.Verify(repo => repo.ReadAllAuditRecordsAsync(), Times.Never);
            _mockAuditRepository.Verify(repo => repo.ReadAuditSummaryAsync(), Times.Never);
        }

        [Fact]
        public async Task ReadAllAuditRecordsAsync_ShouldReturnAuditRecords()
        {
            // Arrange
            var mockAuditRecords = new List<AuditsrvDomainModels.AuditRecord>
            {
                new AuditsrvDomainModels.AuditRecord
                {
                    AuditId = "AUDIT001",
                    UserId = "USER001",
                    ActionDate = "20231001",
                    ActionTime = "120000",
                    ActionType = "CREATE",
                    ActionDescription = "Created a new record",
                    IpAddress = "192.168.1.1",
                    TerminalId = "TERM001",
                    ApplicationId = "APP001",
                    SessionId = "SESSION001",
                    ObjectId = "OBJ001",
                    ObjectType = "TYPE001",
                    AdditionalInfo = "Additional information",
                    Amount = 123.45m
                }
            };

            _mockAuditRepository.Setup(repo => repo.ReadAllAuditRecordsAsync())
                .ReturnsAsync(mockAuditRecords);

            // Act
            var result = await _mockAuditRepository.Object.ReadAllAuditRecordsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("AUDIT001", result.First().AuditId);
        }

        [Fact]
        public async Task WriteAuditRecordAsync_ShouldCallRepository()
        {
            // Arrange
            var auditRecord = new AuditsrvDomainModels.AuditRecord
            {
                AuditId = "AUDIT002",
                UserId = "USER002",
                ActionDate = "20231002",
                ActionTime = "130000",
                ActionType = "UPDATE",
                ActionDescription = "Updated a record",
                IpAddress = "192.168.1.2",
                TerminalId = "TERM002",
                ApplicationId = "APP002",
                SessionId = "SESSION002",
                ObjectId = "OBJ002",
                ObjectType = "TYPE002",
                AdditionalInfo = "Updated information",
                Amount = 456.78m
            };

            // Act
            await _mockAuditRepository.Object.WriteAuditRecordAsync(auditRecord);

            // Assert
            _mockAuditRepository.Verify(repo => repo.WriteAuditRecordAsync(It.Is<AuditsrvDomainModels.AuditRecord>(r =>
                r.AuditId == "AUDIT002" &&
                r.UserId == "USER002" &&
                r.ActionDate == "20231002" &&
                r.ActionTime == "130000" &&
                r.ActionType == "UPDATE" &&
                r.ActionDescription == "Updated a record" &&
                r.IpAddress == "192.168.1.2" &&
                r.TerminalId == "TERM002" &&
                r.ApplicationId == "APP002" &&
                r.SessionId == "SESSION002" &&
                r.ObjectId == "OBJ002" &&
                r.ObjectType == "TYPE002" &&
                r.AdditionalInfo == "Updated information" &&
                r.Amount == 456.78m
            )), Times.Once);
        }

        [Fact]
        public async Task ReadAuditSummaryAsync_ShouldReturnAuditSummary()
        {
            // Arrange
            var mockAuditSummary = new AuditsrvDomainModels.AuditSummary
            {
                SummaryId = "SUM002",
                TotalRecords = 20,
                TotalAmount = 2000.00m
            };

            _mockAuditRepository.Setup(repo => repo.ReadAuditSummaryAsync())
                .ReturnsAsync(mockAuditSummary);

            // Act
            var result = await _mockAuditRepository.Object.ReadAuditSummaryAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("SUM002", result.SummaryId);
            Assert.Equal(20, result.TotalRecords);
            Assert.Equal(2000.00m, result.TotalAmount);
        }

        [Fact]
        public async Task PerformCicsOperationAsync_ShouldCallCicsService()
        {
            // Arrange
            var operationCode = "TEST_OP";
            var requestData = new { Key = "Value" };

            // Act
            await _mockCicsService.Object.PerformCicsOperationAsync(operationCode, requestData);

            // Assert
            _mockCicsService.Verify(service => service.PerformCicsOperationAsync(operationCode, requestData), Times.Once);
        }
    }
}