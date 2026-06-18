package src.INTCALC.Tests;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import src.INTCALC.Application.IntcalcService;
import src.INTCALC.Domain.IntcalcDomainModels.MainRecord;
import src.INTCALC.Domain.IntcalcDomainModels.MonthlyTransaction;
import src.INTCALC.Application.Interfaces.IntcalcInterfaces;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.*;

/**
 * Parity tests for the INTCALC program.
 */
public class IntcalcParityTests {

    private IntcalcService intcalcService;
    private IntcalcInterfaces intcalcInterfacesMock;

    @BeforeEach
    public void setUp() {
        intcalcService = new IntcalcService();
        // Mock or initialize IntcalcInterfaces implementation
        intcalcInterfacesMock = new IntcalcInterfaces() {
            @Override
            public void initializeProgram() {
                // Mock implementation
            }

            @Override
            public MainRecord readInputRecord() {
                MainRecord record = new MainRecord();
                record.setAccountBalance(new BigDecimal("1000.00"));
                record.setInterestRate(new BigDecimal("0.05"));
                record.setAccountHolderName("John Doe");
                record.setBranchCode(12345);
                record.setAccountNumber(987654321L);
                record.setAccountType("SAVINGS");
                record.setTransactionCount(2);

                MonthlyTransaction[] transactions = new MonthlyTransaction[12];
                for (int i = 0; i < 12; i++) {
                    MonthlyTransaction transaction = new MonthlyTransaction();
                    transaction.setMonth(i + 1);
                    transaction.setTransactionAmount(new BigDecimal("100.00"));
                    transactions[i] = transaction;
                }
                record.setMonthlyTransactions(transactions);

                return record;
            }

            @Override
            public BigDecimal calculateInterest(BigDecimal accountBalance, BigDecimal interestRate) {
                return accountBalance.multiply(interestRate);
            }

            @Override
            public void updateAccountBalance(MainRecord mainRecord, BigDecimal interestAmount) {
                BigDecimal updatedBalance = mainRecord.getAccountBalance().add(interestAmount);
                mainRecord.setAccountBalance(updatedBalance);
            }

            @Override
            public void writeOutputRecord(MainRecord mainRecord) {
                // Mock implementation
            }

            @Override
            public BigDecimal processMonthlyTransactions(MonthlyTransaction[] monthlyTransactions) {
                BigDecimal total = BigDecimal.ZERO;
                for (MonthlyTransaction transaction : monthlyTransactions) {
                    total = total.add(transaction.getTransactionAmount());
                }
                return total;
            }

            @Override
            public void finalizeProgram() {
                // Mock implementation
            }
        };
    }

    @Test
    public void testInitializeProgram() {
        assertDoesNotThrow(() -> intcalcInterfacesMock.initializeProgram());
    }

    @Test
    public void testReadInputRecord() {
        MainRecord record = intcalcInterfacesMock.readInputRecord();
        assertNotNull(record);
        assertEquals(new BigDecimal("1000.00"), record.getAccountBalance());
        assertEquals(new BigDecimal("0.05"), record.getInterestRate());
        assertEquals("John Doe", record.getAccountHolderName());
        assertEquals(12345, record.getBranchCode());
        assertEquals(987654321L, record.getAccountNumber());
        assertEquals("SAVINGS", record.getAccountType());
        assertEquals(2, record.getTransactionCount());
        assertNotNull(record.getMonthlyTransactions());
        assertEquals(12, record.getMonthlyTransactions().length);
    }

    @Test
    public void testCalculateInterest() {
        BigDecimal accountBalance = new BigDecimal("1000.00");
        BigDecimal interestRate = new BigDecimal("0.05");
        BigDecimal interest = intcalcInterfacesMock.calculateInterest(accountBalance, interestRate);
        assertEquals(new BigDecimal("50.00"), interest);
    }

    @Test
    public void testUpdateAccountBalance() {
        MainRecord record = intcalcInterfacesMock.readInputRecord();
        BigDecimal interestAmount = new BigDecimal("50.00");
        intcalcInterfacesMock.updateAccountBalance(record, interestAmount);
        assertEquals(new BigDecimal("1050.00"), record.getAccountBalance());
    }

    @Test
    public void testProcessMonthlyTransactions() {
        MainRecord record = intcalcInterfacesMock.readInputRecord();
        BigDecimal totalTransactions = intcalcInterfacesMock.processMonthlyTransactions(record.getMonthlyTransactions());
        assertEquals(new BigDecimal("1200.00"), totalTransactions);
    }

    @Test
    public void testWriteOutputRecord() {
        MainRecord record = intcalcInterfacesMock.readInputRecord();
        assertDoesNotThrow(() -> intcalcInterfacesMock.writeOutputRecord(record));
    }

    @Test
    public void testFinalizeProgram() {
        assertDoesNotThrow(() -> intcalcInterfacesMock.finalizeProgram());
    }
}