package src.ACCTMGR.Tests;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.annotation.DirtiesContext;
import org.springframework.transaction.annotation.Transactional;
import src.ACCTMGR.Application.AcctmgrService;
import src.ACCTMGR.Application.Interfaces.AcctmgrInterfaces.AccountRepository;
import src.ACCTMGR.Application.Interfaces.AcctmgrInterfaces.CustomerRepository;
import src.ACCTMGR.Application.Interfaces.AcctmgrInterfaces.TransactionRepository;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Account;
import src.ACCTMGR.Domain.AcctmgrDomainModels.AccountStatus;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Customer;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Transaction;
import src.ACCTMGR.Domain.AcctmgrDomainModels.TransactionType;

import java.math.BigDecimal;
import java.util.Collections;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest
@Transactional
@DirtiesContext(classMode = DirtiesContext.ClassMode.BEFORE_EACH_TEST_METHOD)
public class AcctmgrParityTests {

    @Autowired
    private AcctmgrService acctmgrService;

    @Autowired
    private AccountRepository accountRepository;

    @Autowired
    private CustomerRepository customerRepository;

    @Autowired
    private TransactionRepository transactionRepository;

    @Test
    public void testExecute_HappyPath() {
        // Seed data
        Account account = new Account();
        account.setAccountNumber("1234567890");
        account.setAccountName("John Doe");
        account.setAccountBalance(new BigDecimal("1000.00"));
        account.setAccountStatus(AccountStatus.ACTIVE);
        accountRepository.saveAccount(account);

        Customer customer = new Customer();
        customer.setCustomerId("CUST123456789");
        customer.setCustomerName("John Doe");
        customer.setCustomerAddress("123 Main Street");
        customer.setAccounts(Collections.singletonList(account));
        customerRepository.saveCustomer(customer);

        Transaction transaction = new Transaction();
        transaction.setTransactionId("TXN123456789");
        transaction.setTransactionAmount(new BigDecimal("200.00"));
        transaction.setTransactionDate("20231001");
        transaction.setTransactionType(TransactionType.DEPOSIT);
        transactionRepository.saveTransaction(transaction);

        // Execute service
        acctmgrService.execute();

        // Assertions
        assertThat(accountRepository.findAllAccounts()).hasSize(1);
        Account savedAccount = accountRepository.findByAccountNumber("1234567890");
        assertThat(savedAccount).isNotNull();
        assertThat(savedAccount.getAccountNumber()).isEqualTo("1234567890");
        assertThat(savedAccount.getAccountName()).isEqualTo("John Doe");
        assertThat(savedAccount.getAccountBalance()).isEqualByComparingTo(new BigDecimal("1000.00"));
        assertThat(savedAccount.getAccountStatus()).isEqualTo(AccountStatus.ACTIVE);

        assertThat(customerRepository.findAllCustomers()).hasSize(1);
        Customer savedCustomer = customerRepository.findByCustomerId("CUST123456789");
        assertThat(savedCustomer).isNotNull();
        assertThat(savedCustomer.getCustomerId()).isEqualTo("CUST123456789");
        assertThat(savedCustomer.getCustomerName()).isEqualTo("John Doe");
        assertThat(savedCustomer.getCustomerAddress()).isEqualTo("123 Main Street");
        assertThat(savedCustomer.getAccounts()).hasSize(1);
        assertThat(savedCustomer.getAccounts().get(0).getAccountNumber()).isEqualTo("1234567890");

        assertThat(transactionRepository.findAllTransactions()).hasSize(1);
        Transaction savedTransaction = transactionRepository.findByTransactionId("TXN123456789");
        assertThat(savedTransaction).isNotNull();
        assertThat(savedTransaction.getTransactionId()).isEqualTo("TXN123456789");
        assertThat(savedTransaction.getTransactionAmount()).isEqualByComparingTo(new BigDecimal("200.00"));
        assertThat(savedTransaction.getTransactionDate()).isEqualTo("20231001");
        assertThat(savedTransaction.getTransactionType()).isEqualTo(TransactionType.DEPOSIT);
    }

    @Test
    public void testExecute_EmptyInput() {
        // Execute service with no seed data
        acctmgrService.execute();

        // Assertions
        assertThat(accountRepository.findAllAccounts()).isEmpty();
        assertThat(customerRepository.findAllCustomers()).isEmpty();
        assertThat(transactionRepository.findAllTransactions()).isEmpty();
    }

    @Test
    public void testExecute_BoundaryValues() {
        // Seed data with boundary values
        Account account = new Account();
        account.setAccountNumber("9999999999");
        account.setAccountName("Max Boundary");
        account.setAccountBalance(new BigDecimal("9999999999999.99"));
        account.setAccountStatus(AccountStatus.CLOSED);
        accountRepository.saveAccount(account);

        Customer customer = new Customer();
        customer.setCustomerId("CUST999999999");
        customer.setCustomerName("Max Customer");
        customer.setCustomerAddress("999 Boundary Street");
        customer.setAccounts(Collections.singletonList(account));
        customerRepository.saveCustomer(customer);

        Transaction transaction = new Transaction();
        transaction.setTransactionId("TXN999999999");
        transaction.setTransactionAmount(new BigDecimal("99999999999.99"));
        transaction.setTransactionDate("99991231");
        transaction.setTransactionType(TransactionType.TRANSFER);
        transactionRepository.saveTransaction(transaction);

        // Execute service
        acctmgrService.execute();

        // Assertions
        assertThat(accountRepository.findAllAccounts()).hasSize(1);
        Account savedAccount = accountRepository.findByAccountNumber("9999999999");
        assertThat(savedAccount).isNotNull();
        assertThat(savedAccount.getAccountNumber()).isEqualTo("9999999999");
        assertThat(savedAccount.getAccountName()).isEqualTo("Max Boundary");
        assertThat(savedAccount.getAccountBalance()).isEqualByComparingTo(new BigDecimal("9999999999999.99"));
        assertThat(savedAccount.getAccountStatus()).isEqualTo(AccountStatus.CLOSED);

        assertThat(customerRepository.findAllCustomers()).hasSize(1);
        Customer savedCustomer = customerRepository.findByCustomerId("CUST999999999");
        assertThat(savedCustomer).isNotNull();
        assertThat(savedCustomer.getCustomerId()).isEqualTo("CUST999999999");
        assertThat(savedCustomer.getCustomerName()).isEqualTo("Max Customer");
        assertThat(savedCustomer.getCustomerAddress()).isEqualTo("999 Boundary Street");
        assertThat(savedCustomer.getAccounts()).hasSize(1);
        assertThat(savedCustomer.getAccounts().get(0).getAccountNumber()).isEqualTo("9999999999");

        assertThat(transactionRepository.findAllTransactions()).hasSize(1);
        Transaction savedTransaction = transactionRepository.findByTransactionId("TXN999999999");
        assertThat(savedTransaction).isNotNull();
        assertThat(savedTransaction.getTransactionId()).isEqualTo("TXN999999999");
        assertThat(savedTransaction.getTransactionAmount()).isEqualByComparingTo(new BigDecimal("99999999999.99"));
        assertThat(savedTransaction.getTransactionDate()).isEqualTo("99991231");
        assertThat(savedTransaction.getTransactionType()).isEqualTo(TransactionType.TRANSFER);
    }
}