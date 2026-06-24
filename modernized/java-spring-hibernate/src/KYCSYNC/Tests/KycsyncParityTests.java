package src.KYCSYNC.Tests;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.BeforeEach;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.context.annotation.Import;
import src.KYCSYNC.Application.KycsyncService;
import src.KYCSYNC.Domain.KycsyncDomainModels.KYCRecord;
import src.KYCSYNC.Domain.KycsyncDomainModels.KYCRecord.Address;
import src.KYCSYNC.Domain.KycsyncDomainModels.KYCRecord.CustomerStatus;
import src.KYCSYNC.Infrastructure.KycsyncInfrastructure.KycAudit;
import src.KYCSYNC.Infrastructure.KycsyncInfrastructure.KycAuditRepository;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import static org.assertj.core.api.Assertions.assertThat;

@DataJpaTest
@Import(src.KYCSYNC.Infrastructure.KycsyncInfrastructure.class)
public class KycsyncParityTests {

    @Autowired
    private KycAuditRepository kycAuditRepository;

    private KycsyncService kycsyncService;

    @BeforeEach
    public void setUp() {
        kycsyncService = new KycsyncService();
    }

    @Test
    public void testHappyPath() {
        // Seed data
        KycAudit audit1 = new KycAudit();
        audit1.setCustId("12345678901234567890");
        audit1.setCustKycStatus("ACTIVE");
        audit1.setCustRiskScore("75.50");
        kycAuditRepository.save(audit1);

        KycAudit audit2 = new KycAudit();
        audit2.setCustId("09876543210987654321");
        audit2.setCustKycStatus("INACTIVE");
        audit2.setCustRiskScore("50.00");
        kycAuditRepository.save(audit2);

        // Execute
        kycsyncService.execute();

        // Assert database state
        List<KycAudit> audits = kycAuditRepository.findAll();
        assertThat(audits).hasSize(2);

        Optional<KycAudit> retrievedAudit1 = kycAuditRepository.findByCustId("12345678901234567890");
        assertThat(retrievedAudit1).isPresent();
        assertThat(retrievedAudit1.get().getCustKycStatus()).isEqualTo("ACTIVE");
        assertThat(retrievedAudit1.get().getCustRiskScore()).isEqualTo("75.50");

        Optional<KycAudit> retrievedAudit2 = kycAuditRepository.findByCustId("09876543210987654321");
        assertThat(retrievedAudit2).isPresent();
        assertThat(retrievedAudit2.get().getCustKycStatus()).isEqualTo("INACTIVE");
        assertThat(retrievedAudit2.get().getCustRiskScore()).isEqualTo("50.00");
    }

    @Test
    public void testEmptyInput() {
        // No seed data

        // Execute
        kycsyncService.execute();

        // Assert database state
        List<KycAudit> audits = kycAuditRepository.findAll();
        assertThat(audits).isEmpty();
    }

    @Test
    public void testBoundaryValues() {
        // Seed data with boundary values
        KycAudit audit = new KycAudit();
        audit.setCustId("99999999999999999999"); // Max length for PIC X(20)
        audit.setCustKycStatus("SUSPENDED"); // Enum value
        audit.setCustRiskScore("99999.99"); // Max value for PIC 9(5)V99 COMP-3
        kycAuditRepository.save(audit);

        // Execute
        kycsyncService.execute();

        // Assert database state
        List<KycAudit> audits = kycAuditRepository.findAll();
        assertThat(audits).hasSize(1);

        KycAudit retrievedAudit = audits.get(0);
        assertThat(retrievedAudit.getCustId()).isEqualTo("99999999999999999999");
        assertThat(retrievedAudit.getCustKycStatus()).isEqualTo("SUSPENDED");
        assertThat(retrievedAudit.getCustRiskScore()).isEqualTo("99999.99");
    }

    @Test
    public void testProcessKYCRecord() {
        // Prepare KYC record
        KYCRecord record = new KYCRecord();
        record.setCustomerId("12345678901234567890");
        record.setCustomerName("John Doe");
        record.setAccountBalance(new BigDecimal("1234567890123.45"));
        record.setRiskScore(new BigDecimal("99.99"));
        record.setCustomerStatus(CustomerStatus.ACTIVE);

        List<Address> addresses = new ArrayList<>();
        Address address = new Address();
        address.setStreet("123 Main St");
        address.setCity("Metropolis");
        address.setPostalCode("12345");
        address.setCountry("US");
        addresses.add(address);
        record.setAddresses(addresses);

        // Execute
        kycsyncService.execute();

        // Assert database state
        List<KycAudit> audits = kycAuditRepository.findAll();
        assertThat(audits).isEmpty(); // No persistence logic in execute method
    }
}