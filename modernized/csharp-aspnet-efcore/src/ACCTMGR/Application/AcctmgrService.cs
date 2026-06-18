using System;
using System.Threading.Tasks;
using ACCTMGR.Infrastructure;

namespace ACCTMGR.Application
{
    public class AcctmgrService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AcctmgrService> _logger;

        public AcctmgrService(IAccountRepository accountRepository, ILogger<AcctmgrService> logger)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await InitializeAsync();
                await ProcessAccountsAsync();
                await FinalizeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the ACCTMGR service.");
                throw;
            }
        }

        private async Task InitializeAsync()
        {
            _logger.LogInformation("Initialization started.");

            // Load configurations or perform any setup required
            await _accountRepository.LoadConfigurationsAsync();

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
                    _logger.LogInformation($"Processing account ID: {account.AccountId}");

                    // Perform necessary calculations or updates on the account
                    var updatedAccount = await ProcessAccountAsync(account);

                    // Save the updated account back to the repository
                    await _accountRepository.UpdateAccountAsync(updatedAccount);

                    _logger.LogInformation($"Successfully processed account ID: {account.AccountId}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing account ID: {account.AccountId}");
                    // Handle specific account processing errors if needed
                }
            }

            _logger.LogInformation("Processing accounts completed.");
        }

        private async Task<Account> ProcessAccountAsync(Account account)
        {
            // Example of processing logic
            // Perform calculations, update balances, etc.
            account.Balance += account.PendingTransactions.Sum(t => t.Amount);
            account.LastProcessedDate = DateTime.UtcNow;

            return await Task.FromResult(account);
        }

        private async Task FinalizeAsync()
        {
            _logger.LogInformation("Finalization started.");

            // Perform any cleanup or finalization tasks
            await _accountRepository.SaveChangesAsync();

            _logger.LogInformation("Finalization completed.");
        }
    }
}