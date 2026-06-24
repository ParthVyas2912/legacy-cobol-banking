using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ACCTMGR.Application
{
    public class AcctmgrService
    {
        private int totalRecordsProcessed;
        private bool endOfFile;
        private List<CustomerRecord> customerFile;
        private int currentRecordIndex;

        public AcctmgrService()
        {
            customerFile = new List<CustomerRecord>();
            currentRecordIndex = 0;
        }

        public async Task ExecuteAsync()
        {
            await InitializeAsync();
            await ProcessRecordsAsync();
            await TerminateAsync();
        }

        private async Task InitializeAsync()
        {
            Console.WriteLine("Initialization started...");

            // Initialize variables
            totalRecordsProcessed = 0;
            endOfFile = false;

            // Load customer file (simulating file read)
            await LoadCustomerFileAsync();

            Console.WriteLine("Initialization completed.");
        }

        private async Task ProcessRecordsAsync()
        {
            Console.WriteLine("Processing records...");

            while (!endOfFile)
            {
                var record = await ReadCustomerRecordAsync();
                if (record == null)
                {
                    endOfFile = true;
                }
                else
                {
                    await ProcessRecordAsync(record);
                }
            }

            Console.WriteLine("Record processing completed.");
        }

        private async Task ProcessRecordAsync(CustomerRecord record)
        {
            Console.WriteLine($"Processing record for Customer ID: {record.CustomerId}");

            // Increment total records processed
            totalRecordsProcessed++;

            // Perform processing on the record
            record.Processed = true;

            // Write the processed record to output
            await WriteOutputRecordAsync(record);
        }

        private async Task TerminateAsync()
        {
            Console.WriteLine("Termination started...");

            // Close customer file
            await CloseCustomerFileAsync();

            // Output total records processed
            Console.WriteLine($"Total records processed: {totalRecordsProcessed}");

            Console.WriteLine("Termination completed.");
        }

        private async Task LoadCustomerFileAsync()
        {

            await Task.Delay(100);
            customerFile.Add(new CustomerRecord { CustomerId = 1, Name = "John Doe", Processed = false });
            customerFile.Add(new CustomerRecord { CustomerId = 2, Name = "Jane Smith", Processed = false });
            customerFile.Add(new CustomerRecord { CustomerId = 3, Name = "Alice Johnson", Processed = false });
        }

        private async Task<CustomerRecord> ReadCustomerRecordAsync()
        {

            await Task.Delay(50);

            if (currentRecordIndex < customerFile.Count)
            {
                var record = customerFile[currentRecordIndex];
                currentRecordIndex++;
                return record;
            }
            else
            {
                return null;
            }
        }

        private async Task WriteOutputRecordAsync(CustomerRecord record)
        {

            await Task.Delay(50);
            Console.WriteLine($"Record written for Customer ID: {record.CustomerId}");
        }

        private async Task CloseCustomerFileAsync()
        {

            await Task.Delay(50);
            Console.WriteLine("Customer file closed.");
        }

        private class CustomerRecord
        {
            public int CustomerId { get; set; }
            public string Name { get; set; }
            public bool Processed { get; set; }
        }
    }
}