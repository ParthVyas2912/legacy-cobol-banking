using System;
using System.Threading.Tasks;
using KYCSYNC.Infrastructure.Interfaces;

namespace KYCSYNC.Application
{
    public class KycsyncService
    {
        private readonly IRecordRepository _recordRepository;
        private readonly ILoggingService _loggingService;
        private readonly IEnvironmentService _environmentService;

        public KycsyncService(IRecordRepository recordRepository, ILoggingService loggingService, IEnvironmentService environmentService)
        {
            _recordRepository = recordRepository ?? throw new ArgumentNullException(nameof(recordRepository));
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _environmentService = environmentService ?? throw new ArgumentNullException(nameof(environmentService));
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await InitializeAsync();
                await ProcessRecordsAsync();
                await FinalizeAsync();
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync($"An error occurred during execution: {ex.Message}", ex);
                throw;
            }
        }

        private async Task InitializeAsync()
        {
            try
            {
                await _loggingService.LogInfoAsync("Initializing KYCSYNC service...");
                await _environmentService.SetupEnvironmentAsync();
                await _loggingService.LogInfoAsync("Environment setup completed.");
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync($"Initialization failed: {ex.Message}", ex);
                throw;
            }
        }

        private async Task ProcessRecordsAsync()
        {
            await _loggingService.LogInfoAsync("Starting record processing...");

            bool hasMoreRecords = true;

            while (hasMoreRecords)
            {
                try
                {
                    var record = await _recordRepository.FetchNextRecordAsync();

                    if (record == null)
                    {
                        hasMoreRecords = false;
                        continue;
                    }

                    await _loggingService.LogInfoAsync($"Processing record ID: {record.Id}");

                    await _recordRepository.ProcessRecordAsync(record);

                    await _loggingService.LogInfoAsync($"Successfully processed record ID: {record.Id}");
                }
                catch (Exception ex)
                {
                    await _loggingService.LogErrorAsync($"Error processing record: {ex.Message}", ex);
                }
            }

            await _loggingService.LogInfoAsync("Record processing completed.");
        }

        private async Task FinalizeAsync()
        {
            try
            {
                await _loggingService.LogInfoAsync("Finalizing KYCSYNC service...");
                await _environmentService.CleanupEnvironmentAsync();
                await _loggingService.LogInfoAsync("Finalization completed.");
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync($"Finalization failed: {ex.Message}", ex);
                throw;
            }
        }
    }
}