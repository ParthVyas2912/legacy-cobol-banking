using System.Threading.Tasks;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    public interface IInterestCalculationService
    {
        /// <summary>
        /// Performs interest calculation based on the provided input.
        /// </summary>
        /// <param name="input">The input parameters for the calculation.</param>
        /// <returns>The result of the interest calculation.</returns>
        Task<IntcalcDomainModels.CalculationResult> PerformCalculationAsync(IntcalcDomainModels.CalculationInput input);
    }

    public interface IAccountRepository
    {
        /// <summary>
        /// Retrieves account details based on the account number.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve details for.</param>
        /// <returns>The account details.</returns>
        Task<IntcalcDomainModels.CalculationInput> GetAccountDetailsAsync(string accountNumber);

        /// <summary>
        /// Saves the result of an interest calculation.
        /// </summary>
        /// <param name="accountNumber">The account number associated with the calculation.</param>
        /// <param name="result">The result of the calculation to save.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SaveCalculationResultAsync(string accountNumber, IntcalcDomainModels.CalculationResult result);
    }

    public interface ICicsTransactionService
    {
        /// <summary>
        /// Executes a CICS transaction for interest calculation.
        /// </summary>
        /// <param name="input">The input parameters for the transaction.</param>
        /// <returns>The result of the CICS transaction.</returns>
        Task<IntcalcDomainModels.CalculationResult> ExecuteCicsTransactionAsync(IntcalcDomainModels.CalculationInput input);
    }
}