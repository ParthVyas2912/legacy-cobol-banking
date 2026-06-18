package src.TXNPOST.Tests;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import src.TXNPOST.Application.Interfaces.TxnpostInterfaces;
import src.TXNPOST.Application.TxnpostService;
import src.TXNPOST.Domain.TxnpostDomainModels.TransactionRecord;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

public class TxnpostParityTests {

    private TxnpostService txnpostService;
    private TxnpostInterfaces txnpostInterfaces;

    @BeforeEach
    public void setUp() {
        txnpostInterfaces = mock(TxnpostInterfaces.class);
        txnpostService = new TxnpostService();
    }

    @Test
    public void testInitializeTransactionPosting() {
        doNothing().when(txnpostInterfaces).initializeTransactionPosting();

        txnpostInterfaces.initializeTransactionPosting();

        verify(txnpostInterfaces, times(1)).initializeTransactionPosting();
    }

    @Test
    public void testReadTransactionRecord() {
        TransactionRecord mockRecord = new TransactionRecord();
        mockRecord.setTransactionId("TXN1234567");
        mockRecord.setAccountNumber(12345678);
        mockRecord.setTransactionAmount(new BigDecimal("1000.50"));
        mockRecord.setTransactionDescription("Test Transaction");
        mockRecord.setTransactionDate(20231010);
        mockRecord.setTransactionTime(1234);
        mockRecord.setTransactionStatus("P");
        mockRecord.setTransactionType("D");
        mockRecord.setCustomerName("John Doe");
        mockRecord.setCustomerPhone("1234567890");
        mockRecord.setCustomerAddress("123 Main St");
        mockRecord.setBranchCode(123);
        mockRecord.setCurrencyCode("USD");
        mockRecord.setExchangeRate(new BigDecimal("1.00"));
        mockRecord.setTellerId(456);
        mockRecord.setReversalFlag("N");
        mockRecord.setAuthorizationFlag("Y");
        mockRecord.setFraudCheckFlag("N");
        mockRecord.setSettlementFlag("Y");
        mockRecord.setPostingFlag("Y");
        mockRecord.setAdjustmentFlag("N");
        mockRecord.setErrorFlag("N");
        mockRecord.setOverrideFlag("N");
        mockRecord.setBatchFlag("N");
        mockRecord.setArchiveFlag("N");
        mockRecord.setPrintFlag("N");
        mockRecord.setEmailFlag("N");
        mockRecord.setSmsFlag("N");
        mockRecord.setNotificationFlag("N");
        mockRecord.setAuditFlag("N");
        mockRecord.setComplianceFlag("N");
        mockRecord.setRegulatoryFlag("N");
        mockRecord.setReconciliationFlag("N");
        mockRecord.setReportingFlag("N");
        mockRecord.setReservedFlag("N");
        mockRecord.setReservedField1("RES1");
        mockRecord.setReservedField2("RES2");
        mockRecord.setReservedField3("RES3");
        mockRecord.setReservedField4("RES4");
        mockRecord.setReservedField5("RES5");
        mockRecord.setReservedField6("RES6");
        mockRecord.setReservedField7("RES7");
        mockRecord.setReservedField8("RES8");
        mockRecord.setReservedField9("RES9");
        mockRecord.setReservedField10("RES10");

        when(txnpostInterfaces.readTransactionRecord()).thenReturn(mockRecord);

        TransactionRecord result = txnpostInterfaces.readTransactionRecord();

        assertNotNull(result);
        assertEquals("TXN1234567", result.getTransactionId());
        assertEquals(12345678, result.getAccountNumber());
        assertEquals(new BigDecimal("1000.50"), result.getTransactionAmount());
        assertEquals("Test Transaction", result.getTransactionDescription());
        assertEquals(20231010, result.getTransactionDate());
        assertEquals(1234, result.getTransactionTime());
        assertEquals("P", result.getTransactionStatus());
        assertEquals("D", result.getTransactionType());
        assertEquals("John Doe", result.getCustomerName());
        assertEquals("1234567890", result.getCustomerPhone());
        assertEquals("123 Main St", result.getCustomerAddress());
        assertEquals(123, result.getBranchCode());
        assertEquals("USD", result.getCurrencyCode());
        assertEquals(new BigDecimal("1.00"), result.getExchangeRate());
        assertEquals(456, result.getTellerId());
        assertEquals("N", result.getReversalFlag());
        assertEquals("Y", result.getAuthorizationFlag());
        assertEquals("N", result.getFraudCheckFlag());
        assertEquals("Y", result.getSettlementFlag());
        assertEquals("Y", result.getPostingFlag());
        assertEquals("N", result.getAdjustmentFlag());
        assertEquals("N", result.getErrorFlag());
        assertEquals("N", result.getOverrideFlag());
        assertEquals("N", result.getBatchFlag());
        assertEquals("N", result.getArchiveFlag());
        assertEquals("N", result.getPrintFlag());
        assertEquals("N", result.getEmailFlag());
        assertEquals("N", result.getSmsFlag());
        assertEquals("N", result.getNotificationFlag());
        assertEquals("N", result.getAuditFlag());
        assertEquals("N", result.getComplianceFlag());
        assertEquals("N", result.getRegulatoryFlag());
        assertEquals("N", result.getReconciliationFlag());
        assertEquals("N", result.getReportingFlag());
        assertEquals("N", result.getReservedFlag());
        assertEquals("RES1", result.getReservedField1());
        assertEquals("RES2", result.getReservedField2());
        assertEquals("RES3", result.getReservedField3());
        assertEquals("RES4", result.getReservedField4());
        assertEquals("RES5", result.getReservedField5());
        assertEquals("RES6", result.getReservedField6());
        assertEquals("RES7", result.getReservedField7());
        assertEquals("RES8", result.getReservedField8());
        assertEquals("RES9", result.getReservedField9());
        assertEquals("RES10", result.getReservedField10());

        verify(txnpostInterfaces, times(1)).readTransactionRecord();
    }

    @Test
    public void testValidateTransactionRecord() {
        TransactionRecord mockRecord = new TransactionRecord();
        mockRecord.setTransactionId("TXN1234567");

        when(txnpostInterfaces.validateTransactionRecord(mockRecord)).thenReturn(true);

        boolean isValid = txnpostInterfaces.validateTransactionRecord(mockRecord);

        assertTrue(isValid);
        verify(txnpostInterfaces, times(1)).validateTransactionRecord(mockRecord);
    }

    @Test
    public void testPostTransaction() {
        TransactionRecord mockRecord = new TransactionRecord();
        mockRecord.setTransactionId("TXN1234567");

        doNothing().when(txnpostInterfaces).postTransaction(mockRecord);

        txnpostInterfaces.postTransaction(mockRecord);

        verify(txnpostInterfaces, times(1)).postTransaction(mockRecord);
    }

    @Test
    public void testHandleTransactionError() {
        TransactionRecord mockRecord = new TransactionRecord();
        mockRecord.setTransactionId("TXN1234567");

        doNothing().when(txnpostInterfaces).handleTransactionError(mockRecord);

        txnpostInterfaces.handleTransactionError(mockRecord);

        verify(txnpostInterfaces, times(1)).handleTransactionError(mockRecord);
    }

    @Test
    public void testWriteTransactionRecord() {
        TransactionRecord mockRecord = new TransactionRecord();
        mockRecord.setTransactionId("TXN1234567");

        doNothing().when(txnpostInterfaces).writeTransactionRecord(mockRecord);

        txnpostInterfaces.writeTransactionRecord(mockRecord);

        verify(txnpostInterfaces, times(1)).writeTransactionRecord(mockRecord);
    }

    @Test
    public void testFinalizeTransactionPosting() {
        doNothing().when(txnpostInterfaces).finalizeTransactionPosting();

        txnpostInterfaces.finalizeTransactionPosting();

        verify(txnpostInterfaces, times(1)).finalizeTransactionPosting();
    }
}