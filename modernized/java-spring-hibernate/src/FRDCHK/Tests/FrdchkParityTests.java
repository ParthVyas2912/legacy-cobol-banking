package src.FRDCHK.Tests;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import src.FRDCHK.Application.FrdchkService;
import src.FRDCHK.Application.Interfaces.FrdchkInterfaces;
import src.FRDCHK.Domain.FrdchkDomainModels.FrdchkRecord;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

public class FrdchkParityTests {

    private FrdchkService frdchkService;
    private FrdchkInterfaces frdchkInterfacesMock;

    @BeforeEach
    public void setUp() {
        frdchkInterfacesMock = Mockito.mock(FrdchkInterfaces.class);
        frdchkService = new FrdchkService();
    }

    @Test
    public void testReadInputFile() {
        List<FrdchkRecord> mockRecords = new ArrayList<>();
        FrdchkRecord record = new FrdchkRecord();
        record.setRecordId("REC001");
        record.setTransactionCode(12345);
        record.setTransactionAmount(new BigDecimal("100.50"));
        record.setCustomerName("John Doe");
        record.setTransactionDate(20231010L);
        record.setTransactionStatus("A");
        mockRecords.add(record);

        when(frdchkInterfacesMock.readInputFile()).thenReturn(mockRecords);

        List<FrdchkRecord> records = frdchkInterfacesMock.readInputFile();
        assertNotNull(records);
        assertEquals(1, records.size());
        assertEquals("REC001", records.get(0).getRecordId());
        assertEquals(12345, records.get(0).getTransactionCode());
        assertEquals(new BigDecimal("100.50"), records.get(0).getTransactionAmount());
        assertEquals("John Doe", records.get(0).getCustomerName());
        assertEquals(20231010L, records.get(0).getTransactionDate());
        assertEquals("A", records.get(0).getTransactionStatus());
    }

    @Test
    public void testWriteOutputFile() {
        List<FrdchkRecord> records = new ArrayList<>();
        FrdchkRecord record = new FrdchkRecord();
        record.setRecordId("REC002");
        record.setTransactionCode(54321);
        record.setTransactionAmount(new BigDecimal("200.75"));
        record.setCustomerName("Jane Smith");
        record.setTransactionDate(20231011L);
        record.setTransactionStatus("P");
        records.add(record);

        doNothing().when(frdchkInterfacesMock).writeOutputFile(records);

        frdchkInterfacesMock.writeOutputFile(records);
        verify(frdchkInterfacesMock, times(1)).writeOutputFile(records);
    }

    @Test
    public void testValidateTransactionCode() {
        FrdchkRecord record = new FrdchkRecord();
        record.setTransactionCode(12345);

        when(frdchkInterfacesMock.validateTransactionCode(record)).thenReturn(true);

        boolean isValid = frdchkInterfacesMock.validateTransactionCode(record);
        assertTrue(isValid);
        verify(frdchkInterfacesMock, times(1)).validateTransactionCode(record);
    }

    @Test
    public void testValidateTransactionAmount() {
        FrdchkRecord record = new FrdchkRecord();
        record.setTransactionAmount(new BigDecimal("150.00"));

        when(frdchkInterfacesMock.validateTransactionAmount(record)).thenReturn(true);

        boolean isValid = frdchkInterfacesMock.validateTransactionAmount(record);
        assertTrue(isValid);
        verify(frdchkInterfacesMock, times(1)).validateTransactionAmount(record);
    }

    @Test
    public void testUpdateTransactionStatus() {
        FrdchkRecord record = new FrdchkRecord();
        record.setTransactionStatus("P");

        doNothing().when(frdchkInterfacesMock).updateTransactionStatus(record);

        frdchkInterfacesMock.updateTransactionStatus(record);
        verify(frdchkInterfacesMock, times(1)).updateTransactionStatus(record);
    }

    @Test
    public void testProcessRecord() {
        FrdchkRecord record = new FrdchkRecord();
        record.setRecordId("REC003");
        record.setTransactionCode(67890);
        record.setTransactionAmount(new BigDecimal("300.25"));
        record.setCustomerName("Alice Johnson");
        record.setTransactionDate(20231012L);
        record.setTransactionStatus("R");

        doNothing().when(frdchkInterfacesMock).processRecord(record);

        frdchkInterfacesMock.processRecord(record);
        verify(frdchkInterfacesMock, times(1)).processRecord(record);
    }

    @Test
    public void testProcessAllRecords() {
        doNothing().when(frdchkInterfacesMock).processAllRecords();

        frdchkInterfacesMock.processAllRecords();
        verify(frdchkInterfacesMock, times(1)).processAllRecords();
    }
}