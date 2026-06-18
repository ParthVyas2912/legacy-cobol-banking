using System;
using System.Threading.Tasks;
using INTCALC.Domain;

namespace INTCALC.Application.Interfaces
{
    public interface IIntcalcService
    {
        Task ExecuteAsync();
    }

    public interface IAccountRepository
    {
        Task<IntcalcDomainModels.Account> GetAccountAsync(string accountNumber);
        Task UpdateAccountAsync(IntcalcDomainModels.Account account);
    }

    public interface ITransactionRepository
    {
        Task AddTransactionAsync(IntcalcDomainModels.Transaction transaction);
    }

    public interface ICustomerRepository
    {
        Task<IntcalcDomainModels.Customer> GetCustomerAsync(string customerId);
    }

    public interface IInterestCalculationService
    {
        Task<IntcalcDomainModels.InterestCalculation> CalculateInterestAsync(decimal principalAmount, decimal interestRate);
    }

    public interface IReportService
    {
        Task GenerateAccountReportAsync(IntcalcDomainModels.Account account);
        Task GenerateTransactionReportAsync(IntcalcDomainModels.Transaction transaction);
    }
}