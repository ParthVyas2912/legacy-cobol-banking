package src.KYCSYNC.Tests;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import src.KYCSYNC.Application.Interfaces.KycsyncInterfaces;
import src.KYCSYNC.Application.KycsyncService;
import src.KYCSYNC.Domain.KycsyncDomainModels.KycRecord;
import src.KYCSYNC.Infrastructure.KycsyncInfrastructure.KycAudit;
import src.KYCSYNC.Infrastructure.KycsyncInfrastructure.KycAuditRepository;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.Mockito.*;

public class KycsyncParityTests {

    private KycsyncService kycsyncService;
    private KycsyncInterfaces kycsyncInterfaces;
    private KycAuditRepository kycAuditRepository;

    @BeforeEach
    public void setUp() {
        kycsyncInterfaces = mock(KycsyncInterfaces.class);
        kycAuditRepository = mock(KycAuditRepository.class);
        kycsyncService = new KycsyncService();
    }

    @Test
    public void testInitializeKycsync() {
        doNothing().when(kycsyncInterfaces).initializeKycsync();

        kycsyncInterfaces.initializeKycsync();

        verify(kycsyncInterfaces, times(1)).initializeKycsync();
    }

    @Test
    public void testReadKycRecords() {
        List<KycRecord> mockRecords = new ArrayList<>();
        KycRecord record = new KycRecord();
        record.setCustomerId("CUST12345");
        record.setCustomerName("John Doe");
        record.setAccountNumber(1234567890L);
        record.setAccountBalance(new BigDecimal("1000.50"));
        record.setAccountStatus("A");
        record.setBranchCode(1234);
        record.setLastUpdatedBy("Admin");
        record.setLastUpdatedDate(20231010);
        record.setLastUpdatedTime(123456);
        mockRecords.add(record);

        when(kycsyncInterfaces.readKycRecords()).thenReturn(mockRecords);

        List<KycRecord> records = kycsyncInterfaces.readKycRecords();

        assertEquals(1, records.size());
        assertEquals("CUST12345", records.get(0).getCustomerId());
        verify(kycsyncInterfaces, times(1)).readKycRecords();
    }

    @Test
    public void testValidateKycRecords() {
        List<KycRecord> inputRecords = new ArrayList<>();
        KycRecord record = new KycRecord();
        record.setCustomerId("CUST12345");
        record.setCustomerName("John Doe");
        record.setAccountNumber(1234567890L);
        record.setAccountBalance(new BigDecimal("1000.50"));
        record.setAccountStatus("A");
        record.setBranchCode(1234);
        record.setLastUpdatedBy("Admin");
        record.setLastUpdatedDate(20231010);
        record.setLastUpdatedTime(123456);
        inputRecords.add(record);

        when(kycsyncInterfaces.validateKycRecords(inputRecords)).thenReturn(inputRecords);

        List<KycRecord> validatedRecords = kycsyncInterfaces.validateKycRecords(inputRecords);

        assertEquals(1, validatedRecords.size());
        assertEquals("CUST12345", validatedRecords.get(0).getCustomerId());
        verify(kycsyncInterfaces, times(1)).validateKycRecords(inputRecords);
    }

    @Test
    public void testProcessKycRecords() {
        List<KycRecord> inputRecords = new ArrayList<>();
        KycRecord record = new KycRecord();
        record.setCustomerId("CUST12345");
        record.setCustomerName("John Doe");
        record.setAccountNumber(1234567890L);
        record.setAccountBalance(new BigDecimal("1000.50"));
        record.setAccountStatus("A");
        record.setBranchCode(1234);
        record.setLastUpdatedBy("Admin");
        record.setLastUpdatedDate(20231010);
        record.setLastUpdatedTime(123456);
        inputRecords.add(record);

        doNothing().when(kycsyncInterfaces).processKycRecords(inputRecords);

        kycsyncInterfaces.processKycRecords(inputRecords);

        verify(kycsyncInterfaces, times(1)).processKycRecords(inputRecords);
    }

    @Test
    public void testWriteKycRecords() {
        List<KycRecord> processedRecords = new ArrayList<>();
        KycRecord record = new KycRecord();
        record.setCustomerId("CUST12345");
        record.setCustomerName("John Doe");
        record.setAccountNumber(1234567890L);
        record.setAccountBalance(new BigDecimal("1000.50"));
        record.setAccountStatus("A");
        record.setBranchCode(1234);
        record.setLastUpdatedBy("Admin");
        record.setLastUpdatedDate(20231010);
        record.setLastUpdatedTime(123456);
        processedRecords.add(record);

        doNothing().when(kycsyncInterfaces).writeKycRecords(processedRecords);

        kycsyncInterfaces.writeKycRecords(processedRecords);

        verify(kycsyncInterfaces, times(1)).writeKycRecords(processedRecords);
    }

    @Test
    public void testFinalizeKycsync() {
        doNothing().when(kycsyncInterfaces).finalizeKycsync();

        kycsyncInterfaces.finalizeKycsync();

        verify(kycsyncInterfaces, times(1)).finalizeKycsync();
    }

    @Test
    public void testKycAuditRepositorySave() {
        KycAudit audit = new KycAudit();
        audit.setCustId("CUST12345");
        audit.setCustKycStatus("VALID");
        audit.setCustRiskScore("LOW");

        when(kycAuditRepository.save(audit)).thenReturn(audit);

        KycAudit savedAudit = kycAuditRepository.save(audit);

        assertEquals("CUST12345", savedAudit.getCustId());
        assertEquals("VALID", savedAudit.getCustKycStatus());
        assertEquals("LOW", savedAudit.getCustRiskScore());
        verify(kycAuditRepository, times(1)).save(audit);
    }
}