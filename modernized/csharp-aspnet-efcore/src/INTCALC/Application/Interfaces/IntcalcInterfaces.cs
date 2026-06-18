using System.Threading.Tasks;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    public interface IIntcalcService
    {
        /// <summary>
        /// Main entry point for the program logic.
        /// </summary>
        Task ExecuteAsync();
    }

    public interface IAccountRepository
    {
        /// <summary>
        /// Reads account data from the data source.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve.</param>
        /// <returns>The account data.</returns>
        Task<IntcalcDomainModels.Account> GetAccountAsync(string accountNumber);

        /// <summary>
        /// Updates account data in the data source.
        /// </summary>
        /// <param name="account">The account data to update.</param>
        Task UpdateAccountAsync(IntcalcDomainModels.Account account);
    }

    public interface ITransactionRepository
    {
        /// <summary>
        /// Records a transaction in the data source.
        /// </summary>
        /// <param name="transaction">The transaction to record.</param>
        Task RecordTransactionAsync(IntcalcDomainModels.Transaction transaction);
    }

    public interface ICustomerRepository
    {
        /// <summary>
        /// Retrieves customer data from the data source.
        /// </summary>
        /// <param name="customerId">The customer ID to retrieve.</param>
        /// <returns>The customer data.</returns>
        Task<IntcalcDomainModels.Customer> GetCustomerAsync(string customerId);
    }

    public interface IInterestCalculator
    {
        /// <summary>
        /// Calculates interest based on the provided data.
        /// </summary>
        /// <param name="principal">The principal amount.</param>
        /// <param name="rate">The interest rate.</param>
        /// <returns>The calculated interest.</returns>
        Task<decimal> CalculateInterestAsync(decimal principal, decimal rate);
    }
}