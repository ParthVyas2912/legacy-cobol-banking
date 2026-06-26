using System.Collections.Generic;
using FRDCHK.Domain;

namespace FRDCHK.Application.Interfaces
{
    // Repository interface for file I/O operations
    public interface ICustomerRepository
    {
        FrdchkDomainModels.Customer GetCustomerById(string customerId);
        void SaveCustomer(FrdchkDomainModels.Customer customer);
        IEnumerable<FrdchkDomainModels.Customer> GetAllCustomers();
        void DeleteCustomer(string customerId); // Added method for deleting a customer
        void UpdateCustomer(FrdchkDomainModels.Customer customer); // Added method for updating a customer
    }

    // Service interface for CICS operations
    public interface ICicsService
    {
        void StartTransaction(string transactionId, Dictionary<string, string> parameters);
        string GetTransactionStatus(string transactionId);
        void CommitTransaction(string transactionId);
        void RollbackTransaction(string transactionId);
        void SendDataToCics(string transactionId, string data); // Added method for sending data to CICS
        string ReceiveDataFromCics(string transactionId); // Added method for receiving data from CICS
    }

    // Application interface for FRDCHK program
    public interface IFrdchkApplication
    {
        void Initialize();
        void ValidateCustomerData(FrdchkDomainModels.Customer customer);
        void ProcessCustomerAccounts(FrdchkDomainModels.Customer customer);
        void GenerateFraudCheckReport(FrdchkDomainModels.Customer customer);
        void Finalize();
        void LogFraudCheckActivity(string activityDetails); // Added method for logging fraud check activities
        bool IsCustomerFraudulent(FrdchkDomainModels.Customer customer); // Added method to check if a customer is fraudulent
    }
}