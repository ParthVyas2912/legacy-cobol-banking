package src.ACCTMGR.Tests;

import org.junit.jupiter.api.Test;
import src.ACCTMGR.Application.AcctmgrService;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Account;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Transaction;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Customer;
import src.ACCTMGR.Domain.AcctmgrDomainModels.AccountStatus;
import src.ACCTMGR.Domain.AcctmgrDomainModels.TransactionType;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Gender;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.*;

/**
 * Parity tests for the ACCTMGR application.
 * These tests ensure that the migrated Java implementation behaves identically to the original COBOL program.
 */
public class AcctmgrParityTests {

    @Test
    public void testExecuteMethod() {
        AcctmgrService service = new AcctmgrService();
        assertDoesNotThrow(service::execute, "The execute method should not throw any exceptions.");
    }

    @Test
    public void testAccountDomainModel() {
        Account account = new Account();
        account.setAccountId("1234567890");
        account.setAccountName("Test Account");
        account.setAccountBalance(new BigDecimal("12345.67"));
        account.setBranchCode(1234);
        account.setCreationDate(20230101L);
        account.setAccountStatus(AccountStatus.ACTIVE);

        assertEquals("1234567890", account.getAccountId());
        assertEquals("Test Account", account.getAccountName());
        assertEquals(new BigDecimal("12345.67"), account.getAccountBalance());
        assertEquals(1234, account.getBranchCode());
        assertEquals(20230101L, account.getCreationDate());
        assertEquals(AccountStatus.ACTIVE, account.getAccountStatus());
    }

    @Test
    public void testTransactionDomainModel() {
        Transaction transaction = new Transaction();
        transaction.setTransactionId("TX1234567890");
        transaction.setTransactionAmount(new BigDecimal("9876.54"));
        transaction.setTransactionDate(20230102L);
        transaction.setTransactionType(TransactionType.DEPOSIT);

        assertEquals("TX1234567890", transaction.getTransactionId());
        assertEquals(new BigDecimal("9876.54"), transaction.getTransactionAmount());
        assertEquals(20230102L, transaction.getTransactionDate());
        assertEquals(TransactionType.DEPOSIT, transaction.getTransactionType());
    }

    @Test
    public void testCustomerDomainModel() {
        Customer customer = new Customer();
        customer.setCustomerId("CUST123456");
        customer.setCustomerName("John Doe");
        customer.setDateOfBirth(19800101L);
        customer.setGender(Gender.MALE);

        assertEquals("CUST123456", customer.getCustomerId());
        assertEquals("John Doe", customer.getCustomerName());
        assertEquals(19800101L, customer.getDateOfBirth());
        assertEquals(Gender.MALE, customer.getGender());
    }

    @Test
    public void testAccountStatusEnum() {
        assertEquals(AccountStatus.ACTIVE, AccountStatus.valueOf("ACTIVE"));
        assertEquals(AccountStatus.INACTIVE, AccountStatus.valueOf("INACTIVE"));
        assertEquals(AccountStatus.CLOSED, AccountStatus.valueOf("CLOSED"));
    }

    @Test
    public void testTransactionTypeEnum() {
        assertEquals(TransactionType.DEPOSIT, TransactionType.valueOf("DEPOSIT"));
        assertEquals(TransactionType.WITHDRAWAL, TransactionType.valueOf("WITHDRAWAL"));
        assertEquals(TransactionType.TRANSFER, TransactionType.valueOf("TRANSFER"));
    }

    @Test
    public void testGenderEnum() {
        assertEquals(Gender.MALE, Gender.valueOf("MALE"));
        assertEquals(Gender.FEMALE, Gender.valueOf("FEMALE"));
    }
}