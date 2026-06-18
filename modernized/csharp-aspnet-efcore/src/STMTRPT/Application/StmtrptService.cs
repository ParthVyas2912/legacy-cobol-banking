using System;
using System.Threading.Tasks;
using STMTRPT.Infrastructure;

namespace STMTRPT.Application
{
    public class StmtrptService
    {
        private readonly IFileHandler _fileHandler;
        private readonly IStatementProcessor _statementProcessor;
        private readonly IReportGenerator _reportGenerator;

        public StmtrptService(IFileHandler fileHandler, IStatementProcessor statementProcessor, IReportGenerator reportGenerator)
        {
            _fileHandler = fileHandler ?? throw new ArgumentNullException(nameof(fileHandler));
            _statementProcessor = statementProcessor ?? throw new ArgumentNullException(nameof(statementProcessor));
            _reportGenerator = reportGenerator ?? throw new ArgumentNullException(nameof(reportGenerator));
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await InitializeAsync();
                await ProcessStatementsAsync();
                await FinalizeReportAsync();
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        private async Task InitializeAsync()
        {
            // Open necessary files and initialize variables
            Console.WriteLine("Initializing the STMTRPT service...");
            await _fileHandler.OpenInputFileAsync("inputFile.dat");
            await _fileHandler.OpenOutputFileAsync("outputFile.dat");
            await _fileHandler.InitializeTemporaryStorageAsync();
        }

        private async Task ProcessStatementsAsync()
        {
            Console.WriteLine("Processing statements...");
            bool hasMoreRecords = true;

            while (hasMoreRecords)
            {
                var record = await _fileHandler.ReadNextRecordAsync();
                if (record == null)
                {
                    hasMoreRecords = false;
                    continue;
                }

                var processingResult = await _statementProcessor.ProcessRecordAsync(record);

                if (processingResult.IsSuccess)
                {
                    await _fileHandler.WriteToOutputFileAsync(processingResult.ProcessedRecord);
                }
                else
                {
                    Console.WriteLine($"Error processing record: {processingResult.ErrorMessage}");
                }
            }
        }

        private async Task FinalizeReportAsync()
        {
            Console.WriteLine("Finalizing the report...");
            var summary = await _statementProcessor.GenerateSummaryAsync();
            await _reportGenerator.GenerateReportAsync(summary);
            await _fileHandler.CloseFilesAsync();
        }
    }
}