using System;
using System.Collections.Generic;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    public interface ICustomerRepository
    {
        FrdchkDomainModels.CustomerRecord GetCustomerById(string customerId);
        void UpdateCustomer(FrdchkDomainModels.CustomerRecord customerRecord);
        IEnumerable<FrdchkDomainModels.CustomerRecord> GetCustomersByStatus(FrdchkDomainModels.AccountStatusEnum status);
    }

    public interface ITransactionRepository
    {
        IEnumerable<FrdchkDomainModels.TransactionRecord> GetTransactionsByCustomerId(string customerId);
        void AddTransaction(FrdchkDomainModels.TransactionRecord transactionRecord);
        IEnumerable<FrdchkDomainModels.TransactionRecord> GetTransactionsByType(FrdchkDomainModels.TransactionTypeEnum transactionType);
    }

    public interface IFraudCheckService
    {
        FrdchkDomainModels.FraudCheckResult PerformFraudCheck(
            FrdchkDomainModels.CustomerRecord customerRecord,
            IEnumerable<FrdchkDomainModels.TransactionRecord> transactionRecords);
        bool ValidateTransaction(FrdchkDomainModels.TransactionRecord transactionRecord);
    }

    public interface IFrdchkApplication
    {
        void InitializeFraudCheck(string customerId);
        FrdchkDomainModels.FraudCheckResult ExecuteFraudCheck(string customerId);
        void FinalizeFraudCheck(string customerId, FrdchkDomainModels.FraudCheckResult fraudCheckResult);
        void LogFraudCheckResult(string customerId, FrdchkDomainModels.FraudCheckResult fraudCheckResult);
    }
}