using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ATMTXN.Application;
using ATMTXN.Application.Interfaces;
using ATMTXN.Domain;

namespace ATMTXN.Tests
{
    public class AtmtxnParityTests
    {
        private readonly Mock<IAtmtxnCicsService> _mockCicsService;
        private readonly AtmtxnService _atmtxnService;

        public AtmtxnParityTests()
        {
            _mockCicsService = new Mock<IAtmtxnCicsService>();
            _atmtxnService = new AtmtxnService();
        }

        [Fact]
        public async Task Atmtxn_HappyPath_ExecutesSuccessfully()
        {
            // Arrange: Mock CICS RECEIVE input for ATMMAIN
            _mockCicsService.Setup(c => c.Receive()).Verifiable();

            // Act: Execute the ATMTXN service
            await _atmtxnService.ExecuteAsync();

            // Assert: Validate the expected CICS interactions
            _mockCicsService.Verify(c => c.Receive(), Times.Once, "CICS RECEIVE for ATMMAIN was not called.");
            _mockCicsService.Verify(c => c.Link(), Times.Exactly(2), "CICS LINK was not called twice.");
            _mockCicsService.Verify(c => c.Send(), Times.Once, "CICS SEND for ATMNEXT was not called.");
        }

        [Fact]
        public async Task Atmtxn_Validate_CallsPerformValidateAsync()
        {
            // Arrange: Mock the validation logic
            _mockCicsService.Setup(c => c.Handle()).Verifiable();

            // Act: Call ValidateAsync
            await _atmtxnService.ValidateAsync();

            // Assert: Ensure validation logic was executed
            _mockCicsService.Verify(c => c.Handle(), Times.Once, "CICS HANDLE was not called during validation.");
        }

        [Fact]
        public async Task Atmtxn_PostTransaction_CallsPerformPostTransactionAsync()
        {
            // Arrange: Mock the post transaction logic
            _mockCicsService.Setup(c => c.Link()).Verifiable();

            // Act: Call PostTransactionAsync
            await _atmtxnService.PostTransactionAsync();

            // Assert: Ensure post transaction logic was executed
            _mockCicsService.Verify(c => c.Link(), Times.Once, "CICS LINK was not called during post transaction.");
        }

        [Fact]
        public async Task Atmtxn_SendNext_CallsPerformSendNextAsync()
        {
            // Arrange: Mock the send next logic
            _mockCicsService.Setup(c => c.Send()).Verifiable();

            // Act: Call SendNextAsync
            await _atmtxnService.SendNextAsync();

            // Assert: Ensure send next logic was executed
            _mockCicsService.Verify(c => c.Send(), Times.Once, "CICS SEND was not called during send next.");
        }

        [Fact]
        public async Task Atmtxn_MapError_CallsPerformMapErrorAsync()
        {
            // Arrange: Mock the map error logic
            _mockCicsService.Setup(c => c.Handle()).Verifiable();

            // Act: Call MapErrorAsync
            await _atmtxnService.MapErrorAsync();

            // Assert: Ensure map error logic was executed
            _mockCicsService.Verify(c => c.Handle(), Times.Once, "CICS HANDLE was not called during map error.");
        }

        [Fact]
        public async Task Atmtxn_Fatal_CallsPerformFatalAsync()
        {
            // Arrange: Mock the fatal logic
            _mockCicsService.Setup(c => c.Abend()).Verifiable();

            // Act: Call FatalAsync
            await _atmtxnService.FatalAsync();

            // Assert: Ensure fatal logic was executed
            _mockCicsService.Verify(c => c.Abend(), Times.Once, "CICS ABEND was not called during fatal.");
        }
    }
}