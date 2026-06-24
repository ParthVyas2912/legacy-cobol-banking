using System.Collections.Generic;
using System.Threading.Tasks;
using KYCSYNC.Domain;

namespace KYCSYNC.Application.Interfaces
{
    public interface IKycsyncService
    {
        /// <summary>
        /// Main entry point for executing the KYCSYNC program.
        /// </summary>
        Task ExecuteAsync();
    }

    public interface ICustomerRepository
    {
        /// <summary>
        /// Retrieves a customer record by its unique identifier.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>The customer record if found; otherwise, null.</returns>
        Task<KycsyncDomainModels.CustomerRecord?> GetCustomerByIdAsync(string customerId);

        /// <summary>
        /// Retrieves all customer records.
        /// </summary>
        /// <returns>A list of all customer records.</returns>
        Task<List<KycsyncDomainModels.CustomerRecord>> GetAllCustomersAsync();

        /// <summary>
        /// Adds a new customer record to the repository.
        /// </summary>
        /// <param name="customer">The customer record to add.</param>
        Task AddCustomerAsync(KycsyncDomainModels.CustomerRecord customer);

        /// <summary>
        /// Updates an existing customer record in the repository.
        /// </summary>
        /// <param name="customer">The customer record to update.</param>
        Task UpdateCustomerAsync(KycsyncDomainModels.CustomerRecord customer);

        /// <summary>
        /// Deletes a customer record by its unique identifier.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer to delete.</param>
        Task DeleteCustomerAsync(string customerId);
    }

    public interface ITransactionRepository
    {
        /// <summary>
        /// Retrieves all transactions for a specific customer.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>A list of transactions for the specified customer.</returns>
        Task<List<KycsyncDomainModels.CustomerRecord.Transaction>> GetTransactionsByCustomerIdAsync(string customerId);

        /// <summary>
        /// Adds a new transaction for a specific customer.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <param name="transaction">The transaction to add.</param>
        Task AddTransactionAsync(string customerId, KycsyncDomainModels.CustomerRecord.Transaction transaction);

        /// <summary>
        /// Updates an existing transaction for a specific customer.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <param name="transaction">The transaction to update.</param>
        Task UpdateTransactionAsync(string customerId, KycsyncDomainModels.CustomerRecord.Transaction transaction);

        /// <summary>
        /// Deletes a transaction by its unique identifier for a specific customer.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <param name="transactionId">The unique identifier of the transaction to delete.</param>
        Task DeleteTransactionAsync(string customerId, string transactionId);
    }
}