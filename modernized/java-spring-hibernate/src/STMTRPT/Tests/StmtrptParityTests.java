package src.STMTRPT.Tests;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import src.STMTRPT.Application.Interfaces.StmtrptInterfaces;
import src.STMTRPT.Application.StmtrptService;
import src.STMTRPT.Domain.StmtrptDomainModels.MainRecord;
import src.STMTRPT.Domain.StmtrptDomainModels.TransactionDetail;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.Mockito.*;

public class StmtrptParityTests {

    private StmtrptInterfaces stmtrptInterfacesMock;
    private StmtrptService stmtrptService;

    @BeforeEach
    public void setUp() {
        stmtrptInterfacesMock = Mockito.mock(StmtrptInterfaces.class);
        stmtrptService = new StmtrptService();
    }

    @Test
    public void testInitialize() {
        doNothing().when(stmtrptInterfacesMock).initialize();

        stmtrptInterfacesMock.initialize();

        verify(stmtrptInterfacesMock, times(1)).initialize();
    }

    @Test
    public void testReadInputFile() {
        List<MainRecord> mockRecords = createMockMainRecords();
        when(stmtrptInterfacesMock.readInputFile()).thenReturn(mockRecords);

        List<MainRecord> result = stmtrptInterfacesMock.readInputFile();

        verify(stmtrptInterfacesMock, times(1)).readInputFile();
        assertEquals(mockRecords.size(), result.size());
        assertEquals(mockRecords.get(0).getAccountNumber(), result.get(0).getAccountNumber());
    }

    @Test
    public void testProcessRecord() {
        MainRecord mockRecord = createMockMainRecord();
        doNothing().when(stmtrptInterfacesMock).processRecord(mockRecord);

        stmtrptInterfacesMock.processRecord(mockRecord);

        verify(stmtrptInterfacesMock, times(1)).processRecord(mockRecord);
    }

    @Test
    public void testWriteOutputFile() {
        List<MainRecord> mockRecords = createMockMainRecords();
        doNothing().when(stmtrptInterfacesMock).writeOutputFile(mockRecords);

        stmtrptInterfacesMock.writeOutputFile(mockRecords);

        verify(stmtrptInterfacesMock, times(1)).writeOutputFile(mockRecords);
    }

    @Test
    public void testGenerateSummaryReport() {
        List<MainRecord> mockRecords = createMockMainRecords();
        doNothing().when(stmtrptInterfacesMock).generateSummaryReport(mockRecords);

        stmtrptInterfacesMock.generateSummaryReport(mockRecords);

        verify(stmtrptInterfacesMock, times(1)).generateSummaryReport(mockRecords);
    }

    @Test
    public void testCleanup() {
        doNothing().when(stmtrptInterfacesMock).cleanup();

        stmtrptInterfacesMock.cleanup();

        verify(stmtrptInterfacesMock, times(1)).cleanup();
    }

    private List<MainRecord> createMockMainRecords() {
        List<MainRecord> records = new ArrayList<>();
        records.add(createMockMainRecord());
        return records;
    }

    private MainRecord createMockMainRecord() {
        MainRecord record = new MainRecord();
        record.setAccountNumber("1234567890");
        record.setBalance(new BigDecimal("1000.00"));
        record.setTransactionDate(20231010);
        record.setTransactionDescription("Mock Transaction");
        record.setTransactionDetails(createMockTransactionDetails());
        return record;
    }

    private List<TransactionDetail> createMockTransactionDetails() {
        List<TransactionDetail> details = new ArrayList<>();
        TransactionDetail detail = new TransactionDetail();
        detail.setTransactionCode("T001");
        detail.setTransactionAmount(new BigDecimal("500.00"));
        detail.setTransactionTime(123456);
        details.add(detail);
        return details;
    }
}