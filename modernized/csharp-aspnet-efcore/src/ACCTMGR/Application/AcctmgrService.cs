using System;
using System.Threading.Tasks;
using ACCTMGR.Infrastructure;

namespace ACCTMGR.Application
{
    public class AcctmgrService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionProcessor _transactionProcessor;
        private readonly ILogger<AcctmgrService> _logger;

        public AcctmgrService(IAccountRepository accountRepository, ITransactionProcessor transactionProcessor, ILogger<AcctmgrService> logger)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _transactionProcessor = transactionProcessor ?? throw new ArgumentNullException(nameof(transactionProcessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await InitializeAsync();
                await ProcessAccountsAsync();
                await FinalizeProcessingAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during execution.");
                throw;
            }
        }

        private async Task InitializeAsync()
        {
            _logger.LogInformation("Initialization started.");

            // Load configuration or perform any necessary setup
            await _accountRepository.LoadConfigurationAsync();

            _logger.LogInformation("Initialization completed.");
        }

        private async Task ProcessAccountsAsync()
        {
            _logger.LogInformation("Processing accounts started.");

            var accounts = await _accountRepository.GetAllAccountsAsync();

            foreach (var account in accounts)
            {
                try
                {
                    _logger.LogInformation($"Processing account {account.AccountId}.");

                    // Process transactions for the account
                    var transactions = await _accountRepository.GetTransactionsForAccountAsync(account.AccountId);

                    foreach (var transaction in transactions)
                    {
                        await _transactionProcessor.ProcessTransactionAsync(transaction);
                    }

                    // Update account status after processing
                    await _accountRepository.UpdateAccountStatusAsync(account.AccountId, "Processed");

                    _logger.LogInformation($"Account {account.AccountId} processed successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing account {account.AccountId}. Skipping to next account.");
                }
            }

            _logger.LogInformation("Processing accounts completed.");
        }

        private async Task FinalizeProcessingAsync()
        {
            _logger.LogInformation("Finalization started.");

            // Perform any finalization tasks, such as saving logs or releasing resources
            await _accountRepository.SaveChangesAsync();

            _logger.LogInformation("Finalization completed.");
        }
    }
}