using System.Collections.Generic;
using System.Threading.Tasks;
using KYCSYNC.Domain;

namespace KYCSYNC.Application.Interfaces
{
    public interface IKycsyncService
    {
        Task ExecuteAsync();
    }

    public interface ICustomerRepository
    {
        Task<List<KycsyncDomainModels.Customer>> GetAllCustomersAsync();
        Task<KycsyncDomainModels.Customer?> GetCustomerByIdAsync(string customerId);
        Task AddCustomerAsync(KycsyncDomainModels.Customer customer);
        Task UpdateCustomerAsync(KycsyncDomainModels.Customer customer);
        Task DeleteCustomerAsync(string customerId);
    }

    public interface IDocumentRepository
    {
        Task<List<KycsyncDomainModels.Document>> GetDocumentsByCustomerIdAsync(string customerId);
        Task<KycsyncDomainModels.Document?> GetDocumentByIdAsync(string documentId);
        Task AddDocumentAsync(KycsyncDomainModels.Document document);
        Task UpdateDocumentAsync(KycsyncDomainModels.Document document);
        Task DeleteDocumentAsync(string documentId);
    }
}