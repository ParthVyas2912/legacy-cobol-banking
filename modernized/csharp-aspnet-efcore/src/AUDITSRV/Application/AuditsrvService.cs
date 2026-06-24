using System;
using System.IO;
using System.Threading.Tasks;

namespace AUDITSRV.Application
{
    public class AuditsrvService
    {
        private StreamReader _auditFileReader;
        private int TotalErrorCount;

        public async Task ExecuteAsync()
        {
            try
            {
                await MainControlFlowAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred during the execution of AUDITSRV.", ex);
            }
        }

        private async Task MainControlFlowAsync()
        {
            await InitializeAuditAsync();
            await ProcessRecordsAsync();
            await FinalizeAuditAsync();
        }

        private async Task InitializeAuditAsync()
        {
            TotalErrorCount = 0;
            await OpenAuditFileAsync();
        }

        private async Task ProcessRecordsAsync()
        {
            bool endOfFile = false;

            while (!endOfFile)
            {
                var record = await ReadAuditFileAsync();
                if (record == null)
                {
                    endOfFile = true;
                }
                else if (IsRecordValid(record))
                {
                    await ProcessValidRecordAsync(record);
                }
                else
                {
                    await HandleInvalidRecordAsync(record);
                }
            }
        }

        private async Task FinalizeAuditAsync()
        {
            await CloseAuditFileAsync();
            Console.WriteLine("AUDIT COMPLETED");
        }

        private async Task OpenAuditFileAsync()
        {
            try
            {
                _auditFileReader = new StreamReader("audit-file.txt");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to open the audit file.", ex);
            }
        }

        private async Task CloseAuditFileAsync()
        {
            try
            {
                if (_auditFileReader != null)
                {
                    _auditFileReader.Close();
                    _auditFileReader = null;
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to close the audit file.", ex);
            }
        }

        private async Task<string> ReadAuditFileAsync()
        {
            try
            {
                if (_auditFileReader == null || _auditFileReader.EndOfStream)
                {
                    return null;
                }

                var line = await _auditFileReader.ReadLineAsync();
                return line;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to read from the audit file.", ex);
            }
        }

        private bool IsRecordValid(string record)
        {
            // Example validation logic: Check if the record is not empty and has a specific format
            return !string.IsNullOrWhiteSpace(record) && record.Length > 5;
        }

        private async Task ProcessValidRecordAsync(string record)
        {
            try
            {
                // Example processing logic: Log the valid record
                Console.WriteLine($"Processing valid record: {record}");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to process valid record: {record}", ex);
            }
        }

        private async Task HandleInvalidRecordAsync(string record)
        {
            try
            {
                TotalErrorCount++;
                Console.WriteLine($"Invalid record encountered: {record}");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to handle invalid record: {record}", ex);
            }
        }
    }
}