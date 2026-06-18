using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TXNPOST.Application.Interfaces;
using TXNPOST.Domain.Entities;

namespace TXNPOST.Application
{
    public class TxnpostService : ITxnpostService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<TxnpostService> _logger;

        public TxnpostService(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            ILogger<TxnpostService> logger)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await InitializeAsync();
                await ProcessTransactionsAsync();
                await FinalizeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the execution of TxnpostService.");
                throw;
            }
        }

        private async Task InitializeAsync()
        {
            _logger.LogInformation("Initialization started.");

            // Load initial configurations
            await _transactionRepository.LoadConfigurationsAsync();

            // Perform any necessary setup
            _logger.LogInformation("Initialization completed.");
        }

        private async Task ProcessTransactionsAsync()
        {
            _logger.LogInformation("Processing transactions started.");

            // Fetch all pending transactions
            IEnumerable<Transaction> transactions = await _transactionRepository.GetPendingTransactionsAsync();

            foreach (var transaction in transactions)
            {
                try
                {
                    _logger.LogInformation($"Processing transaction ID: {transaction.Id}");

                    // Validate transaction
                    if (!await ValidateTransactionAsync(transaction))
                    {
                        _logger.LogWarning($"Transaction ID {transaction.Id} failed validation.");
                        await _transactionRepository.MarkTransactionAsFailedAsync(transaction.Id);
                        continue;
                    }

                    // Post transaction to account
                    var account = await _accountRepository.GetAccountByIdAsync(transaction.AccountId);
                    if (account == null)
                    {
                        _logger.LogWarning($"Account ID {transaction.AccountId} not found for transaction ID {transaction.Id}.");
                        await _transactionRepository.MarkTransactionAsFailedAsync(transaction.Id);
                        continue;
                    }

                    account.Balance += transaction.Amount;
                    await _accountRepository.UpdateAccountAsync(account);

                    // Mark transaction as processed
                    await _transactionRepository.MarkTransactionAsProcessedAsync(transaction.Id);

                    _logger.LogInformation($"Transaction ID {transaction.Id} processed successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing transaction ID {transaction.Id}. Marking as failed.");
                    await _transactionRepository.MarkTransactionAsFailedAsync(transaction.Id);
                }
            }

            _logger.LogInformation("Processing transactions completed.");
        }

        private async Task<bool> ValidateTransactionAsync(Transaction transaction)
        {
            // Example validation logic
            if (transaction.Amount <= 0)
            {
                _logger.LogWarning($"Transaction ID {transaction.Id} has invalid amount: {transaction.Amount}");
                return false;
            }

            if (string.IsNullOrEmpty(transaction.AccountId))
            {
                _logger.LogWarning($"Transaction ID {transaction.Id} has no associated account.");
                return false;
            }

            // Additional validation logic can be added here

            return true;
        }

        private async Task FinalizeAsync()
        {
            _logger.LogInformation("Finalization started.");

            // Perform any cleanup or finalization tasks
            await _transactionRepository.CleanupProcessedTransactionsAsync();

            _logger.LogInformation("Finalization completed.");
        }
    }
}