using System;
using System.Threading.Tasks;
using AUDITSRV.Infrastructure;

namespace AUDITSRV.Application
{
    public class AuditsrvService
    {
        private readonly IAuditRepository _auditRepository;
        private readonly ILogger<AuditsrvService> _logger;

        public AuditsrvService(IAuditRepository auditRepository, ILogger<AuditsrvService> logger)
        {
            _auditRepository = auditRepository ?? throw new ArgumentNullException(nameof(auditRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await MainProcessingAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the execution of the audit service.");
                throw;
            }
        }

        private async Task MainProcessingAsync()
        {
            _logger.LogInformation("Starting main processing for audit service.");

            await InitializeAuditAsync();
            await ProcessAuditRecordsAsync();
            await FinalizeAuditAsync();

            _logger.LogInformation("Completed main processing for audit service.");
        }

        private async Task InitializeAuditAsync()
        {
            _logger.LogInformation("Initializing audit process.");

            // Load configuration settings
            var config = await _auditRepository.LoadConfigurationAsync();
            if (config == null)
            {
                _logger.LogError("Failed to load configuration settings.");
                throw new InvalidOperationException("Configuration settings could not be loaded.");
            }

            // Initialize audit parameters
            var initialized = await _auditRepository.InitializeAuditParametersAsync(config);
            if (!initialized)
            {
                _logger.LogError("Failed to initialize audit parameters.");
                throw new InvalidOperationException("Audit parameters could not be initialized.");
            }

            _logger.LogInformation("Audit process initialized successfully.");
        }

        private async Task ProcessAuditRecordsAsync()
        {
            _logger.LogInformation("Processing audit records.");

            var records = await _auditRepository.FetchAuditRecordsAsync();
            if (records == null || records.Count == 0)
            {
                _logger.LogWarning("No audit records found to process.");
                return;
            }

            foreach (var record in records)
            {
                try
                {
                    // Apply business rules
                    var processedRecord = await _auditRepository.ApplyBusinessRulesAsync(record);
                    if (processedRecord == null)
                    {
                        _logger.LogWarning($"Record with ID {record.Id} could not be processed.");
                        continue;
                    }

                    // Update records
                    var updated = await _auditRepository.UpdateAuditRecordAsync(processedRecord);
                    if (!updated)
                    {
                        _logger.LogWarning($"Failed to update record with ID {record.Id}.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing record with ID {record.Id}.");
                }
            }

            _logger.LogInformation("Completed processing audit records.");
        }

        private async Task FinalizeAuditAsync()
        {
            _logger.LogInformation("Finalizing audit process.");

            // Write audit summary
            var summaryGenerated = await _auditRepository.GenerateAuditSummaryAsync();
            if (!summaryGenerated)
            {
                _logger.LogError("Failed to generate audit summary.");
                throw new InvalidOperationException("Audit summary could not be generated.");
            }

            // Release resources
            var resourcesReleased = await _auditRepository.ReleaseResourcesAsync();
            if (!resourcesReleased)
            {
                _logger.LogError("Failed to release resources.");
                throw new InvalidOperationException("Resources could not be released.");
            }

            _logger.LogInformation("Audit process finalized successfully.");
        }
    }
}