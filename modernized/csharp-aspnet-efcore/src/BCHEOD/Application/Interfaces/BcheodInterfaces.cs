using System.Collections.Generic;
using System.Threading.Tasks;
using BCHEOD.Domain;

namespace BCHEOD.Application.Interfaces
{
    public interface IBcheodApplicationService
    {
        Task ExecuteAsync();
    }

    public interface IAccountRepository
    {
        Task<IEnumerable<BcheodDomainModels.Account>> GetAllAccountsAsync();
        Task UpdateAccountAsync(BcheodDomainModels.Account account);
    }

    public interface ITransactionRepository
    {
        Task<IEnumerable<BcheodDomainModels.Transaction>> GetTransactionsByAccountAsync(string accountNumber);
        Task AddTransactionAsync(BcheodDomainModels.Transaction transaction);
    }

    public interface ICicsService
    {
        Task CallExternalServiceAsync(string serviceName, object requestPayload);
    }
}