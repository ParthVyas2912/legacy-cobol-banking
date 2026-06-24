package src.AUDITSRV.Tests;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.jdbc.Sql;
import src.AUDITSRV.Application.AuditsrvService;
import src.AUDITSRV.Infrastructure.Auditvsam;
import src.AUDITSRV.Infrastructure.AuditvsamRepository;

import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest
@DataJpaTest
public class AuditsrvParityTests {

    @Autowired
    private AuditvsamRepository auditvsamRepository;

    private AuditsrvService auditsrvService;

    @BeforeEach
    public void setUp() {
        auditsrvService = new AuditsrvService();
    }

    @Test
    public void testHappyPath() {
        // Seed data
        Auditvsam auditvsam = new Auditvsam();
        auditvsam.setAuditKey("12345678901234567890");
        auditvsam.setAuditAcctNumber("1234567890");
        auditvsam.setAuditAction("CREATE");
        auditvsam.setAuditUser("USER1234");
        auditvsam.setAuditTs("20231010120000");
        auditvsam.setAuditDetail("Test audit detail");
        auditvsamRepository.save(auditvsam);

        // Execute the service
        auditsrvService.execute();

        // Assert database state
        List<Auditvsam> auditRecords = auditvsamRepository.findAll();
        assertThat(auditRecords).hasSize(1);
        Auditvsam persistedRecord = auditRecords.get(0);
        assertThat(persistedRecord.getAuditKey()).isEqualTo("12345678901234567890");
        assertThat(persistedRecord.getAuditAcctNumber()).isEqualTo("1234567890");
        assertThat(persistedRecord.getAuditAction()).isEqualTo("CREATE");
        assertThat(persistedRecord.getAuditUser()).isEqualTo("USER1234");
        assertThat(persistedRecord.getAuditTs()).isEqualTo("20231010120000");
        assertThat(persistedRecord.getAuditDetail()).isEqualTo("Test audit detail");
    }

    @Test
    public void testEmptyInput() {
        // No seed data

        // Execute the service
        auditsrvService.execute();

        // Assert database state
        List<Auditvsam> auditRecords = auditvsamRepository.findAll();
        assertThat(auditRecords).isEmpty();
    }

    @Test
    public void testBoundaryValues() {
        // Seed data with boundary values
        Auditvsam auditvsam = new Auditvsam();
        auditvsam.setAuditKey("12345678901234567890"); // Max length for PIC X(20)
        auditvsam.setAuditAcctNumber("9999999999"); // Max value for PIC 9(10)
        auditvsam.setAuditAction("ACTIONBOUNDARY"); // Max length for PIC X(16)
        auditvsam.setAuditUser("USER9999"); // Max length for PIC X(8)
        auditvsam.setAuditTs("99991231235959"); // Max timestamp for PIC X(26)
        auditvsam.setAuditDetail("Boundary test detail data for audit record."); // Max length for PIC X(120)
        auditvsamRepository.save(auditvsam);

        // Execute the service
        auditsrvService.execute();

        // Assert database state
        List<Auditvsam> auditRecords = auditvsamRepository.findAll();
        assertThat(auditRecords).hasSize(1);
        Auditvsam persistedRecord = auditRecords.get(0);
        assertThat(persistedRecord.getAuditKey()).isEqualTo("12345678901234567890");
        assertThat(persistedRecord.getAuditAcctNumber()).isEqualTo("9999999999");
        assertThat(persistedRecord.getAuditAction()).isEqualTo("ACTIONBOUNDARY");
        assertThat(persistedRecord.getAuditUser()).isEqualTo("USER9999");
        assertThat(persistedRecord.getAuditTs()).isEqualTo("99991231235959");
        assertThat(persistedRecord.getAuditDetail()).isEqualTo("Boundary test detail data for audit record.");
    }
}