using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using KYCSYNC.Application;
using KYCSYNC.Application.Interfaces;
using KYCSYNC.Infrastructure;

namespace KYCSYNC.Tests
{
    public class KycsyncParityTests
    {
        private readonly Mock<IKycAuditRepository> _mockKycAuditRepository;
        private readonly Mock<IKycsyncBridgeClient> _mockKycsyncBridgeClient;
        private readonly KycsyncService _kycsyncService;

        public KycsyncParityTests()
        {
            _mockKycAuditRepository = new Mock<IKycAuditRepository>();
            _mockKycsyncBridgeClient = new Mock<IKycsyncBridgeClient>();
            _kycsyncService = new KycsyncService(_mockKycAuditRepository.Object, _mockKycsyncBridgeClient.Object);
        }

        [Fact]
        public async Task Kycsync_HappyPath_ExecutesSuccessfully()
        {
            // Arrange
            _mockKycAuditRepository.Setup(repo => repo.AddKycAudit(It.IsAny<KycAudit>())).Verifiable();
            _mockKycAuditRepository.Setup(repo => repo.UpdateKycAudit(It.IsAny<KycAudit>())).Verifiable();
            _mockKycsyncBridgeClient.Setup(client => client.CallCbltdli(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();

            // Act
            await _kycsyncService.ExecuteAsync();

            // Assert
            _mockKycAuditRepository.Verify(repo => repo.AddKycAudit(It.IsAny<KycAudit>()), Times.Never); // Adjust based on actual logic
            _mockKycAuditRepository.Verify(repo => repo.UpdateKycAudit(It.IsAny<KycAudit>()), Times.Never); // Adjust based on actual logic
            _mockKycsyncBridgeClient.Verify(client => client.CallCbltdli(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once); // Adjust based on actual logic
        }

        [Fact]
        public async Task Kycsync_FetchLoop_ProcessesRecords()
        {
            // Arrange
            _mockKycAuditRepository.Setup(repo => repo.AddKycAudit(It.IsAny<KycAudit>())).Verifiable();
            _mockKycAuditRepository.Setup(repo => repo.UpdateKycAudit(It.IsAny<KycAudit>())).Verifiable();

            // Act
            await _kycsyncService.FetchLoopAsync();

            // Assert
            _mockKycAuditRepository.Verify(repo => repo.AddKycAudit(It.IsAny<KycAudit>()), Times.Never); // Adjust based on actual logic
            _mockKycAuditRepository.Verify(repo => repo.UpdateKycAudit(It.IsAny<KycAudit>()), Times.Never); // Adjust based on actual logic
        }

        [Fact]
        public async Task Kycsync_UpsertKyc_PerformsDatabaseOperations()
        {
            // Arrange
            var testKycAudit = new KycAudit
            {
                CustId = "123456",
                CustKycStatus = DateTime.UtcNow,
                CustRiskScore = "High"
            };

            _mockKycAuditRepository.Setup(repo => repo.AddKycAudit(It.IsAny<KycAudit>())).Verifiable();
            _mockKycAuditRepository.Setup(repo => repo.UpdateKycAudit(It.IsAny<KycAudit>())).Verifiable();

            // Act
            await _kycsyncService.UpsertKycAsync();

            // Assert
            _mockKycAuditRepository.Verify(repo => repo.AddKycAudit(It.IsAny<KycAudit>()), Times.Once); // Adjust based on actual logic
            _mockKycAuditRepository.Verify(repo => repo.UpdateKycAudit(It.IsAny<KycAudit>()), Times.Never); // Adjust based on actual logic
        }
    }
}