using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AUDITSRV.Application;
using AUDITSRV.Application.Interfaces;
using AUDITSRV.Domain;
using AUDITSRV.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AUDITSRV.Tests
{
    public class AuditsrvParityTests
    {
        private DbContextOptions<AuditsrvDbContext> CreateInMemoryDbOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AuditsrvDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessValidRecordsAndHandleInvalidRecords()
        {
            // Arrange
            var dbOptions = CreateInMemoryDbOptions(nameof(ExecuteAsync_ShouldProcessValidRecordsAndHandleInvalidRecords));
            await using var context = new AuditsrvDbContext(dbOptions);
            var repository = new AuditRepository(context);
            var service = new AuditsrvService();

            var validRecord = "VALID_RECORD_12345";
            var invalidRecord = "INV";

            var filePath = "audit-file.txt";
            await File.WriteAllLinesAsync(filePath, new[] { validRecord, invalidRecord });

            // Act
            await service.ExecuteAsync();

            // Assert
            var allRecords = await repository.GetAllAuditRecordsAsync();
            allRecords.Should().HaveCount(1);
            allRecords.First().ActionDescription.Should().Be("Processing valid record: VALID_RECORD_12345");

            File.Delete(filePath);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyInputGracefully()
        {
            // Arrange
            var dbOptions = CreateInMemoryDbOptions(nameof(ExecuteAsync_ShouldHandleEmptyInputGracefully));
            await using var context = new AuditsrvDbContext(dbOptions);
            var service = new AuditsrvService();

            var filePath = "audit-file.txt";
            await File.WriteAllTextAsync(filePath, string.Empty);

            // Act
            await service.ExecuteAsync();

            // Assert
            var fileExists = File.Exists(filePath);
            fileExists.Should().BeTrue();

            File.Delete(filePath);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessBoundaryValueRecords()
        {
            // Arrange
            var dbOptions = CreateInMemoryDbOptions(nameof(ExecuteAsync_ShouldProcessBoundaryValueRecords));
            await using var context = new AuditsrvDbContext(dbOptions);
            var repository = new AuditRepository(context);
            var service = new AuditsrvService();

            var boundaryRecord = new string('X', 100); // Simulating a boundary value record
            var filePath = "audit-file.txt";
            await File.WriteAllTextAsync(filePath, boundaryRecord);

            // Act
            await service.ExecuteAsync();

            // Assert
            var allRecords = await repository.GetAllAuditRecordsAsync();
            allRecords.Should().HaveCount(1);
            allRecords.First().ActionDescription.Should().Be($"Processing valid record: {boundaryRecord}");

            File.Delete(filePath);
        }

        [Fact]
        public async Task AddAuditRecordAsync_ShouldPersistRecord()
        {
            // Arrange
            var dbOptions = CreateInMemoryDbOptions(nameof(AddAuditRecordAsync_ShouldPersistRecord));
            await using var context = new AuditsrvDbContext(dbOptions);
            var repository = new AuditRepository(context);

            var auditRecord = new AuditsrvDomainModels.AuditRecord
            {
                AuditId = "AUDIT001",
                UserId = "USER123",
                ActionDate = "20231010",
                ActionTime = "123456",
                ActionDescription = "Test Action",
                Comments = "Test Comments"
            };

            // Act
            await repository.AddAuditRecordAsync(auditRecord);

            // Assert
            var persistedRecord = await repository.GetAuditRecordByIdAsync("AUDIT001");
            persistedRecord.Should().NotBeNull();
            persistedRecord!.AuditId.Should().Be("AUDIT001");
            persistedRecord.UserId.Should().Be("USER123");
            persistedRecord.ActionDate.Should().Be("20231010");
            persistedRecord.ActionTime.Should().Be("123456");
            persistedRecord.ActionDescription.Should().Be("Test Action");
            persistedRecord.Comments.Should().Be("Test Comments");
        }

        [Fact]
        public async Task UpdateAuditRecordAsync_ShouldModifyExistingRecord()
        {
            // Arrange
            var dbOptions = CreateInMemoryDbOptions(nameof(UpdateAuditRecordAsync_ShouldModifyExistingRecord));
            await using var context = new AuditsrvDbContext(dbOptions);
            var repository = new AuditRepository(context);

            var auditRecord = new AuditsrvDomainModels.AuditRecord
            {
                AuditId = "AUDIT002",
                UserId = "USER456",
                ActionDate = "20231011",
                ActionTime = "123457",
                ActionDescription = "Initial Action",
                Comments = "Initial Comments"
            };

            await repository.AddAuditRecordAsync(auditRecord);

            // Act
            auditRecord.ActionDescription = "Updated Action";
            auditRecord.Comments = "Updated Comments";
            await repository.UpdateAuditRecordAsync(auditRecord);

            // Assert
            var updatedRecord = await repository.GetAuditRecordByIdAsync("AUDIT002");
            updatedRecord.Should().NotBeNull();
            updatedRecord!.ActionDescription.Should().Be("Updated Action");
            updatedRecord.Comments.Should().Be("Updated Comments");
        }

        [Fact]
        public async Task DeleteAuditRecordAsync_ShouldRemoveRecord()
        {
            // Arrange
            var dbOptions = CreateInMemoryDbOptions(nameof(DeleteAuditRecordAsync_ShouldRemoveRecord));
            await using var context = new AuditsrvDbContext(dbOptions);
            var repository = new AuditRepository(context);

            var auditRecord = new AuditsrvDomainModels.AuditRecord
            {
                AuditId = "AUDIT003",
                UserId = "USER789",
                ActionDate = "20231012",
                ActionTime = "123458",
                ActionDescription = "Delete Action",
                Comments = "Delete Comments"
            };

            await repository.AddAuditRecordAsync(auditRecord);

            // Act
            await repository.DeleteAuditRecordAsync("AUDIT003");

            // Assert
            var deletedRecord = await repository.GetAuditRecordByIdAsync("AUDIT003");
            deletedRecord.Should().BeNull();
        }
    }
}