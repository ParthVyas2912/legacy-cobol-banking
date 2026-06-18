using System;
using System.Linq;
using System.Threading.Tasks;
using FRDCHK.Application.Interfaces;
using FRDCHK.Domain;
using FRDCHK.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FRDCHK.Application
{
    public class FrdchkService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICicsService _cicsService;

        public FrdchkService(ITransactionRepository transactionRepository, ICicsService cicsService)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _cicsService = cicsService ?? throw new ArgumentNullException(nameof(cicsService));
        }

        public async Task ExecuteAsync()
        {
            await InitializeProgramAsync();
            await PerformFraudCheckAsync();
            await FinalizeProgramAsync();
        }

        private async Task InitializeProgramAsync()
        {
            // Initialization logic
            Console.WriteLine("Initializing program...");
            // Any additional initialization logic can be added here
            await Task.CompletedTask;
        }

        private async Task PerformFraudCheckAsync()
        {
            Console.WriteLine("Performing fraud check...");

            // Fetch transactions from the repository
            var transactions = await _transactionRepository.GetPendingTransactionsAsync();

            foreach (var transaction in transactions)
            {
                // Validate the transaction using CICS service
                var isValid = await _cicsService.ValidateTransactionAsync(transaction);

                if (!isValid)
                {
                    // If transaction is invalid, flag it as fraudulent
                    await FlagFraudulentTransactionAsync(transaction, "Validation failed");
                    continue;
                }

                // Perform velocity checks
                var velocityCheckResult = await PerformVelocityCheckAsync(transaction);

                if (!velocityCheckResult.IsValid)
                {
                    // If velocity check fails, flag it as fraudulent
                    await FlagFraudulentTransactionAsync(transaction, velocityCheckResult.Reason);
                    continue;
                }

                // Update transaction status to processed
                await _cicsService.UpdateTransactionStatusAsync(transaction);
            }
        }

        private async Task FinalizeProgramAsync()
        {
            // Finalization logic
            Console.WriteLine("Finalizing program...");
            // Any additional finalization logic can be added here
            await Task.CompletedTask;
        }

        private async Task<VelocityCheckResult> PerformVelocityCheckAsync(FrdchkDomainModels.TransactionRecord transaction)
        {
            // Perform velocity checks based on transaction history
            var recentTransactions = await _transactionRepository.GetRecentTransactionsAsync(transaction.TxnAcctNumber);

            var totalAmount = recentTransactions.Sum(t => t.TxnAmount);
            var transactionCount = recentTransactions.Count();

            if (transactionCount > 10 || totalAmount > 10000) // Example thresholds
            {
                return new VelocityCheckResult
                {
                    IsValid = false,
                    Reason = "Velocity check failed: Too many transactions or high total amount"
                };
            }

            return new VelocityCheckResult
            {
                IsValid = true
            };
        }

        private async Task FlagFraudulentTransactionAsync(FrdchkDomainModels.TransactionRecord transaction, string reason)
        {
            // Create a fraud flag record
            var fraudFlag = new FraudFlags
            {
                Acct = transaction.TxnAcctNumber,
                Score = 100, // Example score for fraudulent transactions
                Reason = reason,
                Ts = DateTime.UtcNow,
                TxnAcctNumber = transaction.TxnAcctNumber,
                WsRiskScore = 100 // Example risk score
            };

            // Save the fraud flag to the database
            await _transactionRepository.FlagFraudulentTransactionAsync(fraudFlag);
        }
    }

    public class VelocityCheckResult
    {
        public bool IsValid { get; set; }
        public string Reason { get; set; }
    }
}