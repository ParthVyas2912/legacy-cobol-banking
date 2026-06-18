package src.AUDITSRV.Tests;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import src.AUDITSRV.Application.AuditsrvService;
import src.AUDITSRV.Application.Interfaces.AuditsrvInterfaces;
import src.AUDITSRV.Domain.AuditsrvDomainModels.AuditRecord;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class AuditsrvParityTests {

    private AuditsrvService auditsrvService;
    private AuditsrvInterfaces auditsrvInterfacesMock;

    @BeforeEach
    void setUp() {
        auditsrvInterfacesMock = Mockito.mock(AuditsrvInterfaces.class);
        auditsrvService = new AuditsrvService();
    }

    @Test
    void testReadAuditRecords() {
        // Arrange
        List<AuditRecord> mockAuditRecords = new ArrayList<>();
        AuditRecord record = new AuditRecord();
        record.setRecordId("REC001");
        record.setTransactionDate(20231001);
        record.setTransactionTime(123456);
        record.setTransactionAmount(new BigDecimal("100.50"));
        record.setTransactionDescription("Test Transaction");
        record.setTransactionType("D");
        record.setUserId("USER001");
        record.setComments("Test Comment");
        mockAuditRecords.add(record);

        when(auditsrvInterfacesMock.readAuditRecords()).thenReturn(mockAuditRecords);

        // Act
        List<AuditRecord> result = auditsrvInterfacesMock.readAuditRecords();

        // Assert
        assertNotNull(result);
        assertEquals(1, result.size());
        assertEquals("REC001", result.get(0).getRecordId());
        assertEquals(20231001, result.get(0).getTransactionDate());
        assertEquals(123456, result.get(0).getTransactionTime());
        assertEquals(new BigDecimal("100.50"), result.get(0).getTransactionAmount());
        assertEquals("Test Transaction", result.get(0).getTransactionDescription());
        assertEquals("D", result.get(0).getTransactionType());
        assertEquals("USER001", result.get(0).getUserId());
        assertEquals("Test Comment", result.get(0).getComments());
    }

    @Test
    void testWriteAuditRecords() {
        // Arrange
        List<AuditRecord> auditRecords = new ArrayList<>();
        AuditRecord record = new AuditRecord();
        record.setRecordId("REC002");
        record.setTransactionDate(20231002);
        record.setTransactionTime(123457);
        record.setTransactionAmount(new BigDecimal("200.75"));
        record.setTransactionDescription("Another Transaction");
        record.setTransactionType("W");
        record.setUserId("USER002");
        record.setComments("Another Comment");
        auditRecords.add(record);

        doNothing().when(auditsrvInterfacesMock).writeAuditRecords(auditRecords);

        // Act
        auditsrvInterfacesMock.writeAuditRecords(auditRecords);

        // Assert
        verify(auditsrvInterfacesMock, times(1)).writeAuditRecords(auditRecords);
    }

    @Test
    void testProcessAuditRecord() {
        // Arrange
        AuditRecord record = new AuditRecord();
        record.setRecordId("REC003");
        record.setTransactionDate(20231003);
        record.setTransactionTime(123458);
        record.setTransactionAmount(new BigDecimal("300.00"));
        record.setTransactionDescription("Processing Transaction");
        record.setTransactionType("T");
        record.setUserId("USER003");
        record.setComments("Processing Comment");

        doNothing().when(auditsrvInterfacesMock).processAuditRecord(record);

        // Act
        auditsrvInterfacesMock.processAuditRecord(record);

        // Assert
        verify(auditsrvInterfacesMock, times(1)).processAuditRecord(record);
    }

    @Test
    void testValidateAuditRecord() {
        // Arrange
        AuditRecord record = new AuditRecord();
        record.setRecordId("REC004");
        record.setTransactionDate(20231004);
        record.setTransactionTime(123459);
        record.setTransactionAmount(new BigDecimal("400.25"));
        record.setTransactionDescription("Validation Transaction");
        record.setTransactionType("P");
        record.setUserId("USER004");
        record.setComments("Validation Comment");

        when(auditsrvInterfacesMock.validateAuditRecord(record)).thenReturn(true);

        // Act
        boolean isValid = auditsrvInterfacesMock.validateAuditRecord(record);

        // Assert
        assertTrue(isValid);
        verify(auditsrvInterfacesMock, times(1)).validateAuditRecord(record);
    }

    @Test
    void testLogError() {
        // Arrange
        AuditRecord record = new AuditRecord();
        record.setRecordId("REC005");
        record.setTransactionDate(20231005);
        record.setTransactionTime(123460);
        record.setTransactionAmount(new BigDecimal("500.50"));
        record.setTransactionDescription("Error Transaction");
        record.setTransactionType("D");
        record.setUserId("USER005");
        record.setComments("Error Comment");

        String errorMessage = "Invalid transaction type";

        doNothing().when(auditsrvInterfacesMock).logError(record, errorMessage);

        // Act
        auditsrvInterfacesMock.logError(record, errorMessage);

        // Assert
        verify(auditsrvInterfacesMock, times(1)).logError(record, errorMessage);
    }

    @Test
    void testFinalizeAuditProcessing() {
        // Arrange
        doNothing().when(auditsrvInterfacesMock).finalizeAuditProcessing();

        // Act
        auditsrvInterfacesMock.finalizeAuditProcessing();

        // Assert
        verify(auditsrvInterfacesMock, times(1)).finalizeAuditProcessing();
    }
}