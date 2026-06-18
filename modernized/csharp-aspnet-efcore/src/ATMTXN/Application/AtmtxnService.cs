using System;
using System.Threading.Tasks;
using ATMTXN.Application.Interfaces;
using ATMTXN.Domain.Models;
using ATMTXN.Infrastructure.Repositories;

namespace ATMTXN.Application
{
    public class AtmtxnService : IAtmtxnService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILoggingService _loggingService;

        public AtmtxnService(ITransactionRepository transactionRepository, ILoggingService loggingService)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await InitializeTransactionAsync();
                await ProcessTransactionAsync();
                await FinalizeTransactionAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task InitializeTransactionAsync()
        {
            // Initialize transaction state
            _loggingService.LogInfo("Initializing transaction...");
            var initializationResult = await _transactionRepository.InitializeTransactionAsync();

            if (!initializationResult.IsSuccess)
            {
                throw new InvalidOperationException("Failed to initialize transaction.");
            }

            _loggingService.LogInfo("Transaction initialized successfully.");
        }

        private async Task ProcessTransactionAsync()
        {
            // Process the transaction
            _loggingService.LogInfo("Processing transaction...");
            var transactionDetails = await _transactionRepository.GetTransactionDetailsAsync();

            if (transactionDetails == null)
            {
                throw new InvalidOperationException("Transaction details not found.");
            }

            // Perform business logic
            if (transactionDetails.Amount <= 0)
            {
                throw new InvalidOperationException("Invalid transaction amount.");
            }

            var processingResult = await _transactionRepository.ProcessTransactionAsync(transactionDetails);

            if (!processingResult.IsSuccess)
            {
                throw new InvalidOperationException("Transaction processing failed.");
            }

            _loggingService.LogInfo("Transaction processed successfully.");
        }

        private async Task FinalizeTransactionAsync()
        {
            // Finalize the transaction
            _loggingService.LogInfo("Finalizing transaction...");
            var finalizationResult = await _transactionRepository.FinalizeTransactionAsync();

            if (!finalizationResult.IsSuccess)
            {
                throw new InvalidOperationException("Failed to finalize transaction.");
            }

            _loggingService.LogInfo("Transaction finalized successfully.");
        }

        private async Task HandleErrorAsync(Exception ex)
        {
            // Handle errors
            _loggingService.LogError("An error occurred during transaction processing.", ex);

            var rollbackResult = await _transactionRepository.RollbackTransactionAsync();

            if (!rollbackResult.IsSuccess)
            {
                _loggingService.LogError("Failed to rollback transaction.");
            }
            else
            {
                _loggingService.LogInfo("Transaction rolled back successfully.");
            }
        }
    }
}