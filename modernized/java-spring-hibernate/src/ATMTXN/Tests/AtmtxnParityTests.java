package src.ATMTXN.Tests;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import src.ATMTXN.Application.AtmtxnService;
import src.ATMTXN.Application.Interfaces.AtmtxnInterfaces;
import src.ATMTXN.Domain.AtmtxnDomainModels.AtmTransaction;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

public class AtmtxnParityTests {

    private AtmtxnService atmtxnService;
    private AtmtxnInterfaces atmtxnInterfaces;

    @BeforeEach
    public void setUp() {
        atmtxnInterfaces = Mockito.mock(AtmtxnInterfaces.class);
        atmtxnService = new AtmtxnService();
    }

    @Test
    public void testReadAtmTransactionRecords() {
        List<AtmTransaction> mockTransactions = new ArrayList<>();
        AtmTransaction transaction = new AtmTransaction();
        transaction.setTransactionId("TXN1234567890123");
        transaction.setAccountNumber(1234567890L);
        transaction.setTransactionAmount(new BigDecimal("100.50"));
        transaction.setTransactionType("WITHDRAWAL");
        transaction.setTransactionDate("20231010");
        transaction.setTransactionTime("123456");
        transaction.setAtmLocationCode("001");
        transaction.setTransactionStatus("S");
        transaction.setCustomerName("John Doe");
        transaction.setRemarks("Test transaction");
        mockTransactions.add(transaction);

        when(atmtxnInterfaces.readAtmTransactionRecords()).thenReturn(mockTransactions);

        List<AtmTransaction> transactions = atmtxnInterfaces.readAtmTransactionRecords();
        assertNotNull(transactions);
        assertEquals(1, transactions.size());
        assertEquals("TXN1234567890123", transactions.get(0).getTransactionId());
    }

    @Test
    public void testProcessAtmTransaction() {
        AtmTransaction transaction = new AtmTransaction();
        transaction.setTransactionId("TXN1234567890123");
        transaction.setAccountNumber(1234567890L);
        transaction.setTransactionAmount(new BigDecimal("100.50"));
        transaction.setTransactionType("WITHDRAWAL");
        transaction.setTransactionDate("20231010");
        transaction.setTransactionTime("123456");
        transaction.setAtmLocationCode("001");
        transaction.setTransactionStatus("P");
        transaction.setCustomerName("John Doe");
        transaction.setRemarks("Processing transaction");

        when(atmtxnInterfaces.processAtmTransaction(transaction)).thenReturn(transaction);

        AtmTransaction processedTransaction = atmtxnInterfaces.processAtmTransaction(transaction);
        assertNotNull(processedTransaction);
        assertEquals("TXN1234567890123", processedTransaction.getTransactionId());
        assertEquals("P", processedTransaction.getTransactionStatus());
    }

    @Test
    public void testWriteAtmTransactionRecords() {
        List<AtmTransaction> transactions = new ArrayList<>();
        AtmTransaction transaction = new AtmTransaction();
        transaction.setTransactionId("TXN1234567890123");
        transaction.setAccountNumber(1234567890L);
        transaction.setTransactionAmount(new BigDecimal("100.50"));
        transaction.setTransactionType("WITHDRAWAL");
        transaction.setTransactionDate("20231010");
        transaction.setTransactionTime("123456");
        transaction.setAtmLocationCode("001");
        transaction.setTransactionStatus("S");
        transaction.setCustomerName("John Doe");
        transaction.setRemarks("Test transaction");
        transactions.add(transaction);

        doNothing().when(atmtxnInterfaces).writeAtmTransactionRecords(transactions);

        atmtxnInterfaces.writeAtmTransactionRecords(transactions);
        verify(atmtxnInterfaces, times(1)).writeAtmTransactionRecords(transactions);
    }

    @Test
    public void testLogTransactionDetails() {
        AtmTransaction transaction = new AtmTransaction();
        transaction.setTransactionId("TXN1234567890123");
        transaction.setAccountNumber(1234567890L);
        transaction.setTransactionAmount(new BigDecimal("100.50"));
        transaction.setTransactionType("WITHDRAWAL");
        transaction.setTransactionDate("20231010");
        transaction.setTransactionTime("123456");
        transaction.setAtmLocationCode("001");
        transaction.setTransactionStatus("S");
        transaction.setCustomerName("John Doe");
        transaction.setRemarks("Logging transaction");

        doNothing().when(atmtxnInterfaces).logTransactionDetails(transaction);

        atmtxnInterfaces.logTransactionDetails(transaction);
        verify(atmtxnInterfaces, times(1)).logTransactionDetails(transaction);
    }

    @Test
    public void testUpdateTransactionStatus() {
        String transactionId = "TXN1234567890123";
        String status = "C";

        doNothing().when(atmtxnInterfaces).updateTransactionStatus(transactionId, status);

        atmtxnInterfaces.updateTransactionStatus(transactionId, status);
        verify(atmtxnInterfaces, times(1)).updateTransactionStatus(transactionId, status);
    }

    @Test
    public void testGetTransactionById() {
        String transactionId = "TXN1234567890123";
        AtmTransaction transaction = new AtmTransaction();
        transaction.setTransactionId(transactionId);
        transaction.setAccountNumber(1234567890L);
        transaction.setTransactionAmount(new BigDecimal("100.50"));
        transaction.setTransactionType("WITHDRAWAL");
        transaction.setTransactionDate("20231010");
        transaction.setTransactionTime("123456");
        transaction.setAtmLocationCode("001");
        transaction.setTransactionStatus("S");
        transaction.setCustomerName("John Doe");
        transaction.setRemarks("Retrieve transaction");

        when(atmtxnInterfaces.getTransactionById(transactionId)).thenReturn(transaction);

        AtmTransaction retrievedTransaction = atmtxnInterfaces.getTransactionById(transactionId);
        assertNotNull(retrievedTransaction);
        assertEquals(transactionId, retrievedTransaction.getTransactionId());
    }

    @Test
    public void testValidateTransaction() {
        AtmTransaction transaction = new AtmTransaction();
        transaction.setTransactionId("TXN1234567890123");
        transaction.setAccountNumber(1234567890L);
        transaction.setTransactionAmount(new BigDecimal("100.50"));
        transaction.setTransactionType("WITHDRAWAL");
        transaction.setTransactionDate("20231010");
        transaction.setTransactionTime("123456");
        transaction.setAtmLocationCode("001");
        transaction.setTransactionStatus("S");
        transaction.setCustomerName("John Doe");
        transaction.setRemarks("Validate transaction");

        when(atmtxnInterfaces.validateTransaction(transaction)).thenReturn(true);

        boolean isValid = atmtxnInterfaces.validateTransaction(transaction);
        assertTrue(isValid);
    }

    @Test
    public void testCalculateTotalTransactionAmount() {
        List<AtmTransaction> transactions = new ArrayList<>();
        AtmTransaction transaction1 = new AtmTransaction();
        transaction1.setTransactionAmount(new BigDecimal("100.50"));
        transactions.add(transaction1);

        AtmTransaction transaction2 = new AtmTransaction();
        transaction2.setTransactionAmount(new BigDecimal("200.75"));
        transactions.add(transaction2);

        when(atmtxnInterfaces.calculateTotalTransactionAmount(transactions)).thenReturn(301.25);

        double totalAmount = atmtxnInterfaces.calculateTotalTransactionAmount(transactions);
        assertEquals(301.25, totalAmount, 0.001);
    }

    @Test
    public void testGenerateTransactionSummaryReport() {
        List<AtmTransaction> transactions = new ArrayList<>();
        AtmTransaction transaction = new AtmTransaction();
        transaction.setTransactionId("TXN1234567890123");
        transaction.setAccountNumber(1234567890L);
        transaction.setTransactionAmount(new BigDecimal("100.50"));
        transaction.setTransactionType("WITHDRAWAL");
        transaction.setTransactionDate("20231010");
        transaction.setTransactionTime("123456");
        transaction.setAtmLocationCode("001");
        transaction.setTransactionStatus("S");
        transaction.setCustomerName("John Doe");
        transaction.setRemarks("Summary transaction");
        transactions.add(transaction);

        String expectedSummary = "Summary Report: 1 transactions processed.";
        when(atmtxnInterfaces.generateTransactionSummaryReport(transactions)).thenReturn(expectedSummary);

        String summaryReport = atmtxnInterfaces.generateTransactionSummaryReport(transactions);
        assertNotNull(summaryReport);
        assertEquals(expectedSummary, summaryReport);
    }

    @Test
    public void testArchiveProcessedTransactions() {
        List<AtmTransaction> transactions = new ArrayList<>();
        AtmTransaction transaction = new AtmTransaction();
        transaction.setTransactionId("TXN1234567890123");
        transaction.setAccountNumber(1234567890L);
        transaction.setTransactionAmount(new BigDecimal("100.50"));
        transaction.setTransactionType("WITHDRAWAL");
        transaction.setTransactionDate("20231010");
        transaction.setTransactionTime("123456");
        transaction.setAtmLocationCode("001");
        transaction.setTransactionStatus("S");
        transaction.setCustomerName("John Doe");
        transaction.setRemarks("Archive transaction");
        transactions.add(transaction);

        doNothing().when(atmtxnInterfaces).archiveProcessedTransactions(transactions);

        atmtxnInterfaces.archiveProcessedTransactions(transactions);
        verify(atmtxnInterfaces, times(1)).archiveProcessedTransactions(transactions);
    }
}