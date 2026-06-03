using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using BCHEOD.Application;
using BCHEOD.Application.Interfaces;
using BCHEOD.Domain;

namespace BCHEOD.Tests
{
    public class BcheodParityTests
    {
        private readonly Mock<IBcheodExternalService> _mockExternalService;
        private readonly BcheodService _bcheodService;

        public BcheodParityTests()
        {
            _mockExternalService = new Mock<IBcheodExternalService>();
            _bcheodService = new BcheodService(_mockExternalService.Object);
        }

        [Fact]
        public async Task Bcheod_HappyPath_ShouldExecuteWithoutErrors()
        {
            // Arrange
            _mockExternalService
                .Setup(service => service.CallIntCalcService(It.IsAny<object>()))
                .Verifiable();

            _mockExternalService
                .Setup(service => service.CallTxnPostService(It.IsAny<object>()))
                .Verifiable();

            // Act
            await _bcheodService.ExecuteAsync();

            // Assert
            _mockExternalService.Verify(service => service.CallIntCalcService(It.IsAny<object>()), Times.AtLeastOnce);
            _mockExternalService.Verify(service => service.CallTxnPostService(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public async Task Bcheod_Abend_ShouldThrowException()
        {
            // Arrange
            _mockExternalService
                .Setup(service => service.CallIntCalcService(It.IsAny<object>()))
                .Throws(new InvalidOperationException("Critical modernization finding present"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _bcheodService.ExecuteAsync());
        }
    }
}