using System;
using System.Threading.Tasks;
using TXNPOST.Application.Interfaces;
using TXNPOST.Domain;

namespace TXNPOST.Application
{
    public class TxnpostService
    {
        private readonly ITransactionInputRepository _transactionInputRepository;
        private readonly IAuditOutputRepository _auditOutputRepository;
        private readonly ITransactionLedgerService _transactionLedgerService;
        private readonly IWorkingStorageService _workingStorageService;

        public TxnpostService(
            ITransactionInputRepository transactionInputRepository,
            IAuditOutputRepository auditOutputRepository,
            ITransactionLedgerService transactionLedgerService,
            IWorkingStorageService workingStorageService)
        {
            _transactionInputRepository = transactionInputRepository;
            _auditOutputRepository = auditOutputRepository;
            _transactionLedgerService = transactionLedgerService;
            _workingStorageService = workingStorageService;
        }

        public async Task ExecuteAsync()
        {
            await MainAsync();
        }

        private async Task MainAsync()
        {
            await OpenFilesAsync();

            while (await ProcessTransactionsAsync())
            {
                // Loop until the condition in ProcessTransactionsAsync is met
            }

            await CloseFilesAsync();
        }

        private async Task OpenFilesAsync()
        {
            // COBOL equivalent: OPEN INPUT TXN-IN OUTPUT AUDIT-OUT
            // Logic to open files
        }

        private async Task<bool> ProcessTransactionsAsync()
        {
            var transactions = _transactionInputRepository.ReadTransactions();

            foreach (var transaction in transactions)
            {
                await ApplyTransactionAsync(transaction);
            }

            // Placeholder for loop condition
            // Return false to exit the loop
            return false;
        }

        private async Task ApplyTransactionAsync(TxnpostDomainModels.TransactionRecord transaction)
        {
            // COBOL equivalent:
            // MOVE transaction data to working storage
            // CALL 'ACCTMGR' USING transaction data
            // MOVE results to working storage
            // ADD to record count
            // WRITE audit record

            // Move transaction data to working storage
            _workingStorageService.UpdateRecordCount(1); // Example logic for updating record count

            // Call external service (ACCTMGR equivalent)
            _transactionLedgerService.UpdateLedger(transaction);

            // Write audit record
            var auditOutput = new TxnpostDomainModels.AuditOutput
            {
                AuditId = Guid.NewGuid().ToString(),
                AccountNumber = transaction.AccountNumber,
                AuditAmount = transaction.TransactionAmount,
                AuditDate = transaction.TransactionDate,
                AuditTime = DateTime.Now.ToString("HHmmss"),
                AuditDescription = transaction.Description
            };
            _auditOutputRepository.WriteAuditRecord(auditOutput);
        }

        private async Task CloseFilesAsync()
        {
            // COBOL equivalent: CLOSE TXN-IN AUDIT-OUT
            // Logic to close files
        }
    }
}