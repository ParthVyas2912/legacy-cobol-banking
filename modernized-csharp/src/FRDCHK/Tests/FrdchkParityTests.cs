using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using FRDCHK.Application;
using FRDCHK.Application.Interfaces;
using FRDCHK.Domain;
using FRDCHK.Infrastructure;

namespace FRDCHK.Tests
{
    public class FrdchkParityTests
    {
        private readonly FrdchkDbContext _dbContext;
        private readonly TxnLedgerRepository _txnLedgerRepository;
        private readonly FraudFlagsRepository _fraudFlagsRepository;
        private readonly IFrdchkService _frdchkService;

        public FrdchkParityTests()
        {
            var options = new DbContextOptionsBuilder<FrdchkDbContext>()
                .UseInMemoryDatabase(databaseName: "FrdchkTestDb")
                .Options;

            _dbContext = new FrdchkDbContext(options);
            _txnLedgerRepository = new TxnLedgerRepository(_dbContext);
            _fraudFlagsRepository = new FraudFlagsRepository(_dbContext);
            _frdchkService = new FrdchkService();
        }

        [Fact]
        public async Task Frdchk_HappyPath_Test()
        {
            // Arrange
            var testTxnLedger = new TxnLedger
            {
                Count = "1",
                Coalesce = "N/A",
                WsVelocityCount = 0,
                WsVelocityAmt = 0.00m,
                TxnAcctNumber = "1234567890",
                WsCurrentAcct = "1234567890",
                TxnId = "TXN001",
                Acct = "1234567890",
                Amount = 100.00m,
                Code = "DEP",
                PostedAt = DateTime.UtcNow,
                TxnAmount = 100.00m,
                TxnCode = "DEP"
            };

            await _txnLedgerRepository.AddAsync(testTxnLedger);

            // Act
            await _frdchkService.StartAsync();

            // Assert
            var fraudFlag = await _fraudFlagsRepository.GetByAcctAsync("1234567890");
            Assert.NotNull(fraudFlag);
            Assert.Equal("1234567890", fraudFlag.Acct);

            var updatedTxnLedger = await _txnLedgerRepository.GetByIdAsync("TXN001");
            Assert.NotNull(updatedTxnLedger);
            Assert.Equal(100.00m, updatedTxnLedger.TxnAmount);
        }

        [Fact]
        public async Task Frdchk_NoFraud_Test()
        {
            // Arrange
            var testTxnLedger = new TxnLedger
            {
                Count = "1",
                Coalesce = "N/A",
                WsVelocityCount = 0,
                WsVelocityAmt = 0.00m,
                TxnAcctNumber = "9876543210",
                WsCurrentAcct = "9876543210",
                TxnId = "TXN002",
                Acct = "9876543210",
                Amount = 50.00m,
                Code = "WITHDRAWAL",
                PostedAt = DateTime.UtcNow,
                TxnAmount = 50.00m,
                TxnCode = "WITHDRAWAL"
            };

            await _txnLedgerRepository.AddAsync(testTxnLedger);

            // Act
            await _frdchkService.StartAsync();

            // Assert
            var fraudFlag = await _fraudFlagsRepository.GetByAcctAsync("9876543210");
            Assert.Null(fraudFlag);

            var updatedTxnLedger = await _txnLedgerRepository.GetByIdAsync("TXN002");
            Assert.NotNull(updatedTxnLedger);
            Assert.Equal(50.00m, updatedTxnLedger.TxnAmount);
        }

        [Fact]
        public async Task Frdchk_MultipleTransactions_Test()
        {
            // Arrange
            var testTxnLedger1 = new TxnLedger
            {
                Count = "1",
                Coalesce = "N/A",
                WsVelocityCount = 0,
                WsVelocityAmt = 0.00m,
                TxnAcctNumber = "5555555555",
                WsCurrentAcct = "5555555555",
                TxnId = "TXN003",
                Acct = "5555555555",
                Amount = 200.00m,
                Code = "DEP",
                PostedAt = DateTime.UtcNow,
                TxnAmount = 200.00m,
                TxnCode = "DEP"
            };

            var testTxnLedger2 = new TxnLedger
            {
                Count = "1",
                Coalesce = "N/A",
                WsVelocityCount = 0,
                WsVelocityAmt = 0.00m,
                TxnAcctNumber = "5555555555",
                WsCurrentAcct = "5555555555",
                TxnId = "TXN004",
                Acct = "5555555555",
                Amount = 300.00m,
                Code = "DEP",
                PostedAt = DateTime.UtcNow,
                TxnAmount = 300.00m,
                TxnCode = "DEP"
            };

            await _txnLedgerRepository.AddAsync(testTxnLedger1);
            await _txnLedgerRepository.AddAsync(testTxnLedger2);

            // Act
            await _frdchkService.StartAsync();

            // Assert
            var fraudFlag = await _fraudFlagsRepository.GetByAcctAsync("5555555555");
            Assert.NotNull(fraudFlag);
            Assert.Equal("5555555555", fraudFlag.Acct);

            var updatedTxnLedger1 = await _txnLedgerRepository.GetByIdAsync("TXN003");
            var updatedTxnLedger2 = await _txnLedgerRepository.GetByIdAsync("TXN004");
            Assert.NotNull(updatedTxnLedger1);
            Assert.NotNull(updatedTxnLedger2);
            Assert.Equal(200.00m, updatedTxnLedger1.TxnAmount);
            Assert.Equal(300.00m, updatedTxnLedger2.TxnAmount);
        }
    }
}