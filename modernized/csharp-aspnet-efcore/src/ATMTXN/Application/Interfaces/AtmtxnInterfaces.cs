using System;
using ATMTXN.Domain;

namespace ATMTXN.Application.Interfaces
{
    public interface ITransactionRepository
    {
        AtmtxnDomainModels.Transaction GetTransactionById(string transactionId);
        void SaveTransaction(AtmtxnDomainModels.Transaction transaction);
        void UpdateTransaction(AtmtxnDomainModels.Transaction transaction);
        void DeleteTransaction(string transactionId);
    }

    public interface IAccountRepository
    {
        AtmtxnDomainModels.AccountDetails GetAccountByNumber(string accountNumber);
        void UpdateAccountBalance(string accountNumber, decimal newBalance);
    }

    public interface ITransactionService
    {
        void ProcessTransaction(AtmtxnDomainModels.Transaction transaction);
        bool ValidateTransaction(AtmtxnDomainModels.Transaction transaction);
        decimal CalculateTransactionFees(AtmtxnDomainModels.Transaction transaction);
    }

    public interface IAccountService
    {
        bool ValidateAccount(string accountNumber);
        decimal GetAccountBalance(string accountNumber);
    }
}