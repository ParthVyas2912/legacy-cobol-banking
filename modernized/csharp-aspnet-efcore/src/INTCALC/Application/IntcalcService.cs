using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace INTCALC.Application
{
    public class IntcalcService
    {
        private readonly IInputFileReader _inputFileReader;
        private readonly IOutputFileWriter _outputFileWriter;
        private readonly IInterestCalculator _interestCalculator;

        public IntcalcService(IInputFileReader inputFileReader, IOutputFileWriter outputFileWriter, IInterestCalculator interestCalculator)
        {
            _inputFileReader = inputFileReader ?? throw new ArgumentNullException(nameof(inputFileReader));
            _outputFileWriter = outputFileWriter ?? throw new ArgumentNullException(nameof(outputFileWriter));
            _interestCalculator = interestCalculator ?? throw new ArgumentNullException(nameof(interestCalculator));
        }

        public async Task ExecuteAsync()
        {
            await InitializeAsync();
            await CalculateInterestAsync();
            await FinalizeAsync();
        }

        private async Task InitializeAsync()
        {
            // Initialize resources, read input files, and prepare for processing
            await _inputFileReader.OpenAsync();
            await _outputFileWriter.OpenAsync();
        }

        private async Task CalculateInterestAsync()
        {
            // Main logic for processing records and calculating interest
            List<Record> records = await _inputFileReader.ReadRecordsAsync();

            foreach (var record in records)
            {
                try
                {
                    // Perform interest calculation
                    var updatedRecord = _interestCalculator.Calculate(record);

                    // Write the updated record to the output
                    await _outputFileWriter.WriteRecordAsync(updatedRecord);
                }
                catch (Exception ex)
                {
                    // Log the error and continue processing the next record
                    Console.Error.WriteLine($"Error processing record ID {record.Id}: {ex.Message}");
                }
            }
        }

        private async Task FinalizeAsync()
        {
            // Finalize processing, close resources, and handle any cleanup
            await _inputFileReader.CloseAsync();
            await _outputFileWriter.CloseAsync();
        }
    }

    public interface IInputFileReader
    {
        Task OpenAsync();
        Task<List<Record>> ReadRecordsAsync();
        Task CloseAsync();
    }

    public interface IOutputFileWriter
    {
        Task OpenAsync();
        Task WriteRecordAsync(Record record);
        Task CloseAsync();
    }

    public interface IInterestCalculator
    {
        Record Calculate(Record record);
    }

    public class Record
    {
        public int Id { get; set; }
        public decimal Principal { get; set; }
        public decimal InterestRate { get; set; }
        public int TermInDays { get; set; }
        public decimal InterestAmount { get; set; }
    }
}