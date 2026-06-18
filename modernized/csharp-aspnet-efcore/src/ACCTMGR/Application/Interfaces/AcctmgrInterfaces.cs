using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ACCTMGR.Domain;

namespace ACCTMGR.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<AcctmgrDomainModels.Account> GetAccountByNumberAsync(string accountNumber);
        Task<IEnumerable<AcctmgrDomainModels.Account>> GetAllAccountsAsync();
        Task SaveAccountAsync(AcctmgrDomainModels.Account account);
    }

    public interface ITransactionRepository
    {
        Task<IEnumerable<AcctmgrDomainModels.Transaction>> GetTransactionsByAccountNumberAsync(string accountNumber);
        Task SaveTransactionAsync(AcctmgrDomainModels.Transaction transaction);
    }

    public interface IAcctmgrService
    {
        Task ExecuteAsync();
    }
}