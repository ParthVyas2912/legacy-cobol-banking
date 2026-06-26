using System.Collections.Generic;
using ATMTXN.Domain;

namespace ATMTXN.Application.Interfaces
{
    public interface ITransactionRepository
    {
        AtmtxnDomainModels.TransactionRecord GetTransactionById(int transactionId);
        void SaveTransaction(AtmtxnDomainModels.TransactionRecord transaction);
        List<AtmtxnDomainModels.TransactionRecord> GetAllTransactions();
        void DeleteTransaction(int transactionId); // Added for completeness
    }

    public interface ICustomerRepository
    {
        AtmtxnDomainModels.CustomerRecord GetCustomerById(string customerId);
        void SaveCustomer(AtmtxnDomainModels.CustomerRecord customer);
        List<AtmtxnDomainModels.CustomerRecord> GetAllCustomers();
        void DeleteCustomer(string customerId); // Added for completeness
    }

    public interface IAtmMachineRepository
    {
        AtmtxnDomainModels.AtmMachineRecord GetAtmById(string atmId);
        void SaveAtm(AtmtxnDomainModels.AtmMachineRecord atmMachine);
        List<AtmtxnDomainModels.AtmMachineRecord> GetAllAtms();
        void DeleteAtm(string atmId); // Added for completeness
    }

    public interface ITransactionService
    {
        void ProcessTransaction(AtmtxnDomainModels.TransactionRecord transaction);
        void ReverseTransaction(int transactionId);
        decimal GetAvailableBalance(string accountNumber);
        void LogTransaction(AtmtxnDomainModels.TransactionLog transactionLog); // Added for transaction logging
    }

    public interface ICustomerService
    {
        void UpdateCustomerStatus(string customerId, string status);
        void NotifyCustomer(string customerId, string message);
        AtmtxnDomainModels.CustomerRecord GetCustomerDetails(string customerId); // Added for retrieving customer details
    }

    public interface IAtmService
    {
        void UpdateAtmStatus(string atmId, string status);
        void ReplenishCash(string atmId, decimal amount);
        decimal GetAtmBalance(string atmId); // Added for retrieving ATM balance
    }
}