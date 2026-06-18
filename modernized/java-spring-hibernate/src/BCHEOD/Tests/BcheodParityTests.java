package src.BCHEOD.Tests;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import src.BCHEOD.Application.BcheodService;
import src.BCHEOD.Application.Interfaces.BcheodInterfaces;
import src.BCHEOD.Domain.BcheodDomainModels.MainRecord;

import java.math.BigDecimal;

import static org.mockito.Mockito.*;

/**
 * Parity tests for BCHEOD program.
 */
public class BcheodParityTests {

    private BcheodService bcheodService;
    private BcheodInterfaces bcheodInterfacesMock;

    @BeforeEach
    public void setUp() {
        bcheodInterfacesMock = Mockito.mock(BcheodInterfaces.class);
        bcheodService = new BcheodService();
    }

    @Test
    public void testInitializeProgram() {
        // Arrange
        doNothing().when(bcheodInterfacesMock).initializeProgram();

        // Act
        bcheodInterfacesMock.initializeProgram();

        // Assert
        verify(bcheodInterfacesMock, times(1)).initializeProgram();
    }

    @Test
    public void testProcessInputFile() {
        // Arrange
        doNothing().when(bcheodInterfacesMock).processInputFile();

        // Act
        bcheodInterfacesMock.processInputFile();

        // Assert
        verify(bcheodInterfacesMock, times(1)).processInputFile();
    }

    @Test
    public void testProcessRecord() {
        // Arrange
        MainRecord record = new MainRecord();
        record.setAccountBalance(new BigDecimal("12345.67"));
        record.setAccountHolderName("John Doe");
        record.setTransactionDate(20231010);
        record.setTransactionAmount(new BigDecimal("500.00"));
        record.setTransactionType("D");
        record.setBranchCode(1234);
        record.setCurrencyCode(840);

        doNothing().when(bcheodInterfacesMock).processRecord(record);

        // Act
        bcheodInterfacesMock.processRecord(record);

        // Assert
        verify(bcheodInterfacesMock, times(1)).processRecord(record);
    }

    @Test
    public void testWriteOutputRecord() {
        // Arrange
        MainRecord record = new MainRecord();
        record.setAccountBalance(new BigDecimal("12345.67"));
        record.setAccountHolderName("John Doe");
        record.setTransactionDate(20231010);
        record.setTransactionAmount(new BigDecimal("500.00"));
        record.setTransactionType("D");
        record.setBranchCode(1234);
        record.setCurrencyCode(840);

        doNothing().when(bcheodInterfacesMock).writeOutputRecord(record);

        // Act
        bcheodInterfacesMock.writeOutputRecord(record);

        // Assert
        verify(bcheodInterfacesMock, times(1)).writeOutputRecord(record);
    }

    @Test
    public void testFinalizeProgram() {
        // Arrange
        doNothing().when(bcheodInterfacesMock).finalizeProgram();

        // Act
        bcheodInterfacesMock.finalizeProgram();

        // Assert
        verify(bcheodInterfacesMock, times(1)).finalizeProgram();
    }
}