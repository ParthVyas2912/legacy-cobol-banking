using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace INTCALC.Application
{
    public class IntcalcService
    {
        // Entry point for the service
        public async Task ExecuteAsync()
        {
            await MainProcessingAsync();
        }

        // Main processing control flow
        private async Task MainProcessingAsync()
        {
            await InitializeProcessingAsync();
            await CalculateInterestAsync();
            await FinalizeProcessingAsync();
        }

        // Initialization logic
        private async Task InitializeProcessingAsync()
        {
            // Initialize working storage variables
            InitializeWorkingStorage();

            // Read input file and load records
            bool endOfFile = false;
            while (!endOfFile)
            {
                endOfFile = await ReadInputFileAsync();
            }
        }

        // Main interest calculation logic
        private async Task CalculateInterestAsync()
        {
            foreach (var record in _workingStorage.Records)
            {
                await CalculateInterestForRecordAsync(record);
            }
        }

        // Finalization logic
        private async Task FinalizeProcessingAsync()
        {
            foreach (var record in _workingStorage.Records)
            {
                await WriteOutputFileAsync(record);
            }

            await CloseFilesAsync();
        }

        // Reads a single record from the input file
        private async Task<bool> ReadInputFileAsync()
        {
            var record = await _inputFileRepository.ReadNextRecordAsync();
            if (record == null)
            {
                return true; // End of file
            }

            _workingStorage.Records.Add(record);
            return false;
        }

        // Calculates interest for a single record
        private async Task CalculateInterestForRecordAsync(WorkingStorage.Record record)
        {
            // Perform interest calculation
            record.InterestAmount = record.Principal * record.Rate / 100;

            // Update total interest in working storage
            _workingStorage.TotalInterest += record.InterestAmount;

            await Task.CompletedTask;
        }

        // Writes a single record to the output file
        private async Task WriteOutputFileAsync(WorkingStorage.Record record)
        {
            await _outputFileRepository.WriteRecordAsync(record);
        }

        // Closes all files
        private async Task CloseFilesAsync()
        {
            await _inputFileRepository.CloseAsync();
            await _outputFileRepository.CloseAsync();
        }

        // Initializes working storage variables
        private void InitializeWorkingStorage()
        {
            _workingStorage = new WorkingStorage
            {
                Records = new List<WorkingStorage.Record>(),
                TotalInterest = 0
            };
        }

        // Dependencies
        private readonly IInputFileRepository _inputFileRepository;
        private readonly IOutputFileRepository _outputFileRepository;

        // Working storage
        private WorkingStorage _workingStorage;

        // Constructor with dependency injection
        public IntcalcService(IInputFileRepository inputFileRepository, IOutputFileRepository outputFileRepository)
        {
            _inputFileRepository = inputFileRepository ?? throw new ArgumentNullException(nameof(inputFileRepository));
            _outputFileRepository = outputFileRepository ?? throw new ArgumentNullException(nameof(outputFileRepository));
        }

        // Working storage definition
        public class WorkingStorage
        {
            public List<Record> Records { get; set; }
            public decimal TotalInterest { get; set; }

            // Record definition
            public class Record
            {
                public decimal Principal { get; set; }
                public decimal Rate { get; set; }
                public decimal InterestAmount { get; set; }
            }
        }
    }

    // Input file repository interface
    public interface IInputFileRepository
    {
        Task<IntcalcService.WorkingStorage.Record> ReadNextRecordAsync();
        Task CloseAsync();
    }

    // Output file repository interface
    public interface IOutputFileRepository
    {
        Task WriteRecordAsync(IntcalcService.WorkingStorage.Record record);
        Task CloseAsync();
    }
}