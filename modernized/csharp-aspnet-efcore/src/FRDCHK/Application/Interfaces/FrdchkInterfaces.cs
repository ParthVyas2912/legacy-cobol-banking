using System.Collections.Generic;
using System.Threading.Tasks;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    public interface IFrdchkService
    {
        Task ExecuteAsync();
    }

    public interface ITransactionRepository
    {
        Task<IEnumerable<FrdchkDomainModels.TransactionRecord>> GetPendingTransactionsAsync();
        Task SaveTransactionAsync(FrdchkDomainModels.TransactionRecord transaction);
        Task UpdateTransactionStatusAsync(string transactionId, FrdchkDomainModels.TransactionStatus status);
    }

    public interface IItemDetailRepository
    {
        Task<IEnumerable<FrdchkDomainModels.ItemDetail>> GetItemDetailsAsync(string transactionId);
        Task SaveItemDetailsAsync(string transactionId, IEnumerable<FrdchkDomainModels.ItemDetail> itemDetails);
    }

    public interface ICicsFraudCheckService
    {
        Task<bool> PerformFraudCheckAsync(FrdchkDomainModels.TransactionRecord transaction);
    }
}