using ATMTXN.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATMTXN.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<AtmtxnDomainModels.Transaction?> GetTransactionByIdAsync(string transactionId);
        Task<IEnumerable<AtmtxnDomainModels.Transaction>> GetAllTransactionsAsync();
        Task AddTransactionAsync(AtmtxnDomainModels.Transaction transaction);
        Task UpdateTransactionAsync(AtmtxnDomainModels.Transaction transaction);
        Task DeleteTransactionAsync(string transactionId);
    }

    public interface IAtmtxnService
    {
        Task ProcessTransactionAsync(string transactionId);
        Task ReverseTransactionAsync(string transactionId);
        Task AdjustTransactionAsync(string transactionId, decimal adjustmentAmount, string adjustmentReasonCode);
        Task RefundTransactionAsync(string transactionId, decimal refundAmount, string refundReasonCode);
        Task ReconcileTransactionAsync(string transactionId, string reconciliationReasonCode);
        Task HandleChargebackAsync(string transactionId, string chargebackReasonCode);
        Task RetryTransactionAsync(string transactionId, string retryReasonCode);
        Task MarkTransactionAsOfflineAsync(string transactionId, string offlineReasonCode);
        Task SettleTransactionAsync(string transactionId, string settlementDate);
    }
}