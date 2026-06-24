package src.ACCTMGR.Infrastructure;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Account;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Customer;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Transaction;
import src.ACCTMGR.Application.Interfaces.AcctmgrInterfaces.AccountRepository as AccountRepositoryInterface;
import src.ACCTMGR.Application.Interfaces.AcctmgrInterfaces.CustomerRepository as CustomerRepositoryInterface;
import src.ACCTMGR.Application.Interfaces.AcctmgrInterfaces.TransactionRepository as TransactionRepositoryInterface;

import javax.persistence.*;
import java.math.BigDecimal;
import java.util.List;

@Configuration
public class AcctmgrInfrastructure {

    @Bean
    public AccountRepository accountRepository() {
        return new AccountRepositoryImpl();
    }

    @Bean
    public CustomerRepository customerRepository() {
        return new CustomerRepositoryImpl();
    }

    @Bean
    public TransactionRepository transactionRepository() {
        return new TransactionRepositoryImpl();
    }

    @Entity
    @Table(name = "acctfile", schema = "public")
    public static class Acctfile {

        @Id
        @Column(name = "acct_number", length = 10, nullable = false)
        // COBOL: ACCT-NUMBER PIC 9(10)
        private String acctNumber;

        @Column(name = "acct_customer_id", length = 8, nullable = false)
        // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
        private String acctCustomerId;

        @Column(name = "acct_opened_date", columnDefinition = "DATE", nullable = false)
        // COBOL: ACCT-OPENED-DATE PIC 9(8)
        private String acctOpenedDate;

        @Column(name = "acct_type", length = 2, nullable = false)
        // COBOL: ACCT-TYPE PIC X(02)
        private String acctType;

        @Column(name = "acct_status", length = 1, nullable = false)
        // COBOL: ACCT-STATUS PIC X(01)
        private String acctStatus;

        @Column(name = "acct_balance", precision = 15, scale = 2, nullable = false)
        // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3
        private BigDecimal acctBalance;

        @Column(name = "acct_avail_bal", precision = 15, scale = 2, nullable = false)
        // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3
        private BigDecimal acctAvailBal;

        @Column(name = "acct_int_rate", precision = 6, scale = 4, nullable = false)
        // COBOL: ACCT-INT-RATE PIC 9(2)V9(4) COMP-3
        private BigDecimal acctIntRate;

        @Column(name = "acct_last_txn_dt", columnDefinition = "DATE")
        // COBOL: ACCT-LAST-TXN-DT PIC 9(8)
        private String acctLastTxnDt;

        @Column(name = "acct_currency", length = 3, nullable = false)
        // COBOL: ACCT-CURRENCY PIC X(03)
        private String acctCurrency;

        @Column(name = "acct_branch_code", length = 4, nullable = false)
        // COBOL: ACCT-BRANCH-CODE PIC 9(4)
        private String acctBranchCode;

        @Column(name = "acct_flags", length = 4)
        // COBOL: ACCT-FLAGS PIC X(04)
        private String acctFlags;

        @Column(name = "acct_filler", length = 50)
        // COBOL: ACCT-FILLER PIC X(50)
        private String acctFiller;

        // Getters and Setters
        // ...
    }

    @Entity
    @Table(name = "acct_audit", schema = "public")
    public static class AcctAudit {

        @Id
        @GeneratedValue(strategy = GenerationType.IDENTITY)
        private Long id;

        @Column(name = "acct_num", length = 10, nullable = false)
        // COBOL: ACCT-NUM PIC 9(10)
        private String acctNum;

        @Column(name = "action", length = 50, nullable = false)
        // COBOL: ACTION PIC X(50)
        private String action;

        @Column(name = "actor", length = 50, nullable = false)
        // COBOL: ACTOR PIC X(50)
        private String actor;

        @Column(name = "ts", columnDefinition = "TIMESTAMPTZ", nullable = false)
        // COBOL: TS PIC 9(14)
        private String ts;

        @Column(name = "acct_number", length = 10, nullable = false)
        // COBOL: ACCT-NUMBER PIC 9(10)
        private String acctNumber;

        @Column(name = "comm_function", length = 20, nullable = false)
        // COBOL: COMM-FUNCTION PIC X(20)
        private String commFunction;

        // Getters and Setters
        // ...
    }

    @Entity
    @Table(name = "customer", schema = "public")
    public static class Customer {

        @Id
        @Column(name = "acct_customer_id", length = 15, nullable = false)
        // COBOL: ACCT-CUSTOMER-ID PIC X(15)
        private String acctCustomerId;

        @Column(name = "customer_name", length = 50, nullable = false)
        // COBOL: CUSTOMER-NAME PIC X(50)
        private String customerName;

        @Column(name = "customer_address", length = 100, nullable = false)
        // COBOL: CUSTOMER-ADDRESS PIC X(100)
        private String customerAddress;

        @Column(name = "active_acct_count", precision = 5, scale = 0, nullable = false)
        // COBOL: ACTIVE-ACCT-COUNT PIC 9(5)
        private Integer activeAcctCount;

        // Getters and Setters
        // ...
    }

    @Repository
    public interface AccountRepository extends JpaRepository<Acctfile, String>, AccountRepositoryInterface {

        @Override
        @Query("SELECT a FROM Acctfile a WHERE a.acctNumber = ?1")
        Account findByAccountNumber(String accountNumber);

        @Override
        @Query("SELECT a FROM Acctfile a")
        List<Account> findAllAccounts();

        @Override
        default void saveAccount(Account account) {
            save(account);
        }

        @Override
        default void updateAccount(Account account) {
            save(account);
        }

        @Override
        default void deleteAccount(String accountNumber) {
            deleteById(accountNumber);
        }
    }

    @Repository
    public interface CustomerRepository extends JpaRepository<Customer, String>, CustomerRepositoryInterface {

        @Override
        @Query("SELECT c FROM Customer c WHERE c.acctCustomerId = ?1")
        Customer findByCustomerId(String customerId);

        @Override
        @Query("SELECT c FROM Customer c")
        List<Customer> findAllCustomers();

        @Override
        default void saveCustomer(Customer customer) {
            save(customer);
        }

        @Override
        default void updateCustomer(Customer customer) {
            save(customer);
        }

        @Override
        default void deleteCustomer(String customerId) {
            deleteById(customerId);
        }
    }

    @Repository
    public interface TransactionRepository extends JpaRepository<Transaction, String>, TransactionRepositoryInterface {

        @Override
        @Query("SELECT t FROM Transaction t WHERE t.transactionId = ?1")
        Transaction findByTransactionId(String transactionId);

        @Override
        @Query("SELECT t FROM Transaction t")
        List<Transaction> findAllTransactions();

        @Override
        default void saveTransaction(Transaction transaction) {
            save(transaction);
        }

        @Override
        default void updateTransaction(Transaction transaction) {
            save(transaction);
        }

        @Override
        default void deleteTransaction(String transactionId) {
            deleteById(transactionId);
        }
    }
}