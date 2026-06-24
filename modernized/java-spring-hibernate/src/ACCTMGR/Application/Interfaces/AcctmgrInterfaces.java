package src.ACCTMGR.Application.Interfaces;

import src.ACCTMGR.Domain.AcctmgrDomainModels.Account;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Customer;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Transaction;

import java.math.BigDecimal;
import java.util.List;

public interface AcctmgrInterfaces {

    // Public entry point
    void execute();

    // Repository interfaces for file I/O operations
    interface AccountRepository {
        Account findByAccountNumber(String accountNumber);

        List<Account> findAllAccounts();

        void saveAccount(Account account);

        void updateAccount(Account account);

        void deleteAccount(String accountNumber);
    }

    interface CustomerRepository {
        Customer findByCustomerId(String customerId);

        List<Customer> findAllCustomers();

        void saveCustomer(Customer customer);

        void updateCustomer(Customer customer);

        void deleteCustomer(String customerId);
    }

    interface TransactionRepository {
        Transaction findByTransactionId(String transactionId);

        List<Transaction> findAllTransactions();

        void saveTransaction(Transaction transaction);

        void updateTransaction(Transaction transaction);

        void deleteTransaction(String transactionId);
    }

    // CICS service interfaces
    interface AccountService {
        Account getAccountDetails(String accountNumber);

        void updateAccountBalance(String accountNumber, BigDecimal newBalance);

        void closeAccount(String accountNumber);
    }

    interface CustomerService {
        Customer getCustomerDetails(String customerId);

        void updateCustomerInfo(Customer customer);

        void addCustomer(Customer customer);
    }

    interface TransactionService {
        void processTransaction(Transaction transaction);

        List<Transaction> getTransactionsByAccount(String accountNumber);
    }
}