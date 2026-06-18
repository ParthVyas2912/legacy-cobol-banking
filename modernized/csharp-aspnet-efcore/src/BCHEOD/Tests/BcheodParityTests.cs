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
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;
        private readonly Mock<ICicsService> _mockCicsService;
        private readonly BcheodService _bcheodService;

        public BcheodParityTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockCicsService = new Mock<ICicsService>();

            _bcheodService = new BcheodService();
        }

        [Fact]
        public async Task ExecuteAsync_Should_Initialize_Process_And_Finalize()
        {
            // Arrange
            var initializeCalled = false;
            var processRecordsCalled = false;
            var finalizeProcessingCalled = false;

            var service = new Mock<BcheodService>();
            service
                .Setup(s => s.GetType().GetMethod("InitializeAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(s, null))
                .Callback(() => initializeCalled = true)
                .Returns(Task.CompletedTask);
            service
                .Setup(s => s.GetType().GetMethod("ProcessRecordsAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(s, null))
                .Callback(() => processRecordsCalled = true)
                .Returns(Task.CompletedTask);
            service
                .Setup(s => s.GetType().GetMethod("FinalizeProcessingAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(s, null))
                .Callback(() => finalizeProcessingCalled = true)
                .Returns(Task.CompletedTask);

            // Act
            await service.Object.ExecuteAsync();

            // Assert
            Assert.True(initializeCalled, "InitializeAsync was not called.");
            Assert.True(processRecordsCalled, "ProcessRecordsAsync was not called.");
            Assert.True(finalizeProcessingCalled, "FinalizeProcessingAsync was not called.");
        }

        [Fact]
        public async Task InitializeAsync_Should_Log_Initialization_Message()
        {
            // Arrange
            using var consoleOutput = new ConsoleOutput();
            var service = new BcheodService();

            // Act
            var initializeMethod = service.GetType().GetMethod("InitializeAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (initializeMethod != null)
            {
                await (Task)initializeMethod.Invoke(service, null);
            }

            // Assert
            Assert.Contains("Initializing service...", consoleOutput.GetOutput());
        }

        [Fact]
        public async Task ProcessRecordsAsync_Should_Log_Processing_Message()
        {
            // Arrange
            using var consoleOutput = new ConsoleOutput();
            var service = new BcheodService();

            // Act
            var processMethod = service.GetType().GetMethod("ProcessRecordsAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (processMethod != null)
            {
                await (Task)processMethod.Invoke(service, null);
            }

            // Assert
            Assert.Contains("Processing records...", consoleOutput.GetOutput());
        }

        [Fact]
        public async Task FinalizeProcessingAsync_Should_Log_Finalization_Message()
        {
            // Arrange
            using var consoleOutput = new ConsoleOutput();
            var service = new BcheodService();

            // Act
            var finalizeMethod = service.GetType().GetMethod("FinalizeProcessingAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (finalizeMethod != null)
            {
                await (Task)finalizeMethod.Invoke(service, null);
            }

            // Assert
            Assert.Contains("Finalizing processing...", consoleOutput.GetOutput());
        }

        [Fact]
        public async Task ExecuteAsync_Should_Handle_Exception_And_Rethrow()
        {
            // Arrange
            var service = new Mock<BcheodService>();
            service
                .Setup(s => s.GetType().GetMethod("InitializeAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(s, null))
                .Throws(new Exception("Initialization failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => service.Object.ExecuteAsync());
            Assert.Equal("Initialization failed", exception.Message);
        }
    }

    public class ConsoleOutput : IDisposable
    {
        private readonly System.IO.StringWriter _stringWriter;
        private readonly System.IO.TextWriter _originalOutput;

        public ConsoleOutput()
        {
            _stringWriter = new System.IO.StringWriter();
            _originalOutput = Console.Out;
            Console.SetOut(_stringWriter);
        }

        public string GetOutput()
        {
            return _stringWriter.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(_originalOutput);
            _stringWriter.Dispose();
        }
    }
}