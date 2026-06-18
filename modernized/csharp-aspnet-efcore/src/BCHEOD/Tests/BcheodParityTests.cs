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
        private readonly Mock<IInputFileRepository> _mockInputFileRepository;
        private readonly Mock<IOutputFileRepository> _mockOutputFileRepository;
        private readonly Mock<ICicsService> _mockCicsService;
        private readonly BcheodService _bcheodService;

        public BcheodParityTests()
        {
            _mockInputFileRepository = new Mock<IInputFileRepository>();
            _mockOutputFileRepository = new Mock<IOutputFileRepository>();
            _mockCicsService = new Mock<ICicsService>();

            _bcheodService = new BcheodService();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldCompleteSuccessfully()
        {
            // Arrange
            var exampleRecord = new BcheodDomainModels.ExampleRecord
            {
                ExampleDecimalField = 12345.67m,
                ExampleStringField = "TestString",
                ExampleIntegerField = 12345,
                ExampleLongField = 1234567890L,
                ExampleEnumField = BcheodDomainModels.ExampleEnum.Value1,
                ExampleGroupField = new BcheodDomainModels.ExampleGroup
                {
                    GroupStringField = "GroupString",
                    GroupDecimalField = 123.45m
                },
                ExampleArrayField = new[]
                {
                    new BcheodDomainModels.ExampleNestedRecord
                    {
                        NestedStringField = "NestedString1",
                        NestedIntegerField = 1234
                    },
                    new BcheodDomainModels.ExampleNestedRecord
                    {
                        NestedStringField = "NestedString2",
                        NestedIntegerField = 5678
                    }
                },
                ExampleRedefineField = new BcheodDomainModels.ExampleRedefine
                {
                    RedefineStringField = "Redefine",
                    RedefineDecimalField = 12.34m
                }
            };

            _mockInputFileRepository.Setup(repo => repo.ReadNextRecordAsync())
                .ReturnsAsync(exampleRecord);

            _mockOutputFileRepository.Setup(repo => repo.WriteRecordAsync(It.IsAny<BcheodDomainModels.ExampleRecord>()))
                .Returns(Task.CompletedTask);

            _mockCicsService.Setup(service => service.PerformTransactionAsync(It.IsAny<string>()))
                .ReturnsAsync("Transaction successful");

            // Act
            await _bcheodService.ExecuteAsync();

            // Assert
            _mockInputFileRepository.Verify(repo => repo.ReadNextRecordAsync(), Times.AtLeastOnce);
            _mockOutputFileRepository.Verify(repo => repo.WriteRecordAsync(It.IsAny<BcheodDomainModels.ExampleRecord>()), Times.AtLeastOnce);
            _mockCicsService.Verify(service => service.PerformTransactionAsync(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyInputFile()
        {
            // Arrange
            _mockInputFileRepository.SetupSequence(repo => repo.ReadNextRecordAsync())
                .ReturnsAsync((BcheodDomainModels.ExampleRecord)null);

            // Act
            await _bcheodService.ExecuteAsync();

            // Assert
            _mockInputFileRepository.Verify(repo => repo.ReadNextRecordAsync(), Times.AtLeastOnce);
            _mockOutputFileRepository.Verify(repo => repo.WriteRecordAsync(It.IsAny<BcheodDomainModels.ExampleRecord>()), Times.Never);
            _mockCicsService.Verify(service => service.PerformTransactionAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleCicsTransactionFailure()
        {
            // Arrange
            var exampleRecord = new BcheodDomainModels.ExampleRecord
            {
                ExampleDecimalField = 12345.67m,
                ExampleStringField = "TestString",
                ExampleIntegerField = 12345,
                ExampleLongField = 1234567890L,
                ExampleEnumField = BcheodDomainModels.ExampleEnum.Value1,
                ExampleGroupField = new BcheodDomainModels.ExampleGroup
                {
                    GroupStringField = "GroupString",
                    GroupDecimalField = 123.45m
                },
                ExampleArrayField = new[]
                {
                    new BcheodDomainModels.ExampleNestedRecord
                    {
                        NestedStringField = "NestedString1",
                        NestedIntegerField = 1234
                    },
                    new BcheodDomainModels.ExampleNestedRecord
                    {
                        NestedStringField = "NestedString2",
                        NestedIntegerField = 5678
                    }
                },
                ExampleRedefineField = new BcheodDomainModels.ExampleRedefine
                {
                    RedefineStringField = "Redefine",
                    RedefineDecimalField = 12.34m
                }
            };

            _mockInputFileRepository.Setup(repo => repo.ReadNextRecordAsync())
                .ReturnsAsync(exampleRecord);

            _mockCicsService.Setup(service => service.PerformTransactionAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("CICS transaction failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _bcheodService.ExecuteAsync());

            _mockInputFileRepository.Verify(repo => repo.ReadNextRecordAsync(), Times.AtLeastOnce);
            _mockCicsService.Verify(service => service.PerformTransactionAsync(It.IsAny<string>()), Times.AtLeastOnce);
            _mockOutputFileRepository.Verify(repo => repo.WriteRecordAsync(It.IsAny<BcheodDomainModels.ExampleRecord>()), Times.Never);
        }
    }
}