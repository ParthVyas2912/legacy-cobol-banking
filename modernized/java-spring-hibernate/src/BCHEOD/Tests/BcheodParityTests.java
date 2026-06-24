package src.BCHEOD.Tests;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.jdbc.Sql;
import src.BCHEOD.Application.BcheodService;
import src.BCHEOD.Infrastructure.EodRuns;
import src.BCHEOD.Infrastructure.EodRunsRepository;

import java.time.OffsetDateTime;
import java.util.Optional;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest
public class BcheodParityTests {

    @Autowired
    private BcheodService bcheodService;

    @Autowired
    private EodRunsRepository eodRunsRepository;

    @Test
    @Sql(scripts = "/testdata/seed-data.sql")
    public void testExecute_HappyPath() {
        // Arrange: Seed data is already loaded via @Sql annotation

        // Act
        bcheodService.execute();

        // Assert
        assertThat(eodRunsRepository.count()).isEqualTo(1);
        Optional<EodRuns> eodRun = eodRunsRepository.findById(1L);
        assertThat(eodRun).isPresent();
        assertThat(eodRun.get().getStatus()).isEqualTo("COMPLETED");
        assertThat(eodRun.get().getWsStatusCode()).isEqualTo("SUCCESS");
    }

    @Test
    public void testExecute_EmptyInput() {
        // Arrange: No seed data

        // Act
        bcheodService.execute();

        // Assert
        assertThat(eodRunsRepository.count()).isEqualTo(0);
    }

    @Test
    public void testExecute_BoundaryValues() {
        // Arrange
        EodRuns eodRun = new EodRuns();
        eodRun.setId(9999999999L);
        eodRun.setStartedAt(OffsetDateTime.parse("2023-01-01T00:00:00Z"));
        eodRun.setStatus("PENDING");
        eodRun.setEndedAt(null);
        eodRun.setWsStatusCode("PENDING");
        eodRunsRepository.save(eodRun);

        // Act
        bcheodService.execute();

        // Assert
        assertThat(eodRunsRepository.count()).isEqualTo(1);
        Optional<EodRuns> updatedEodRun = eodRunsRepository.findById(9999999999L);
        assertThat(updatedEodRun).isPresent();
        assertThat(updatedEodRun.get().getStatus()).isEqualTo("COMPLETED");
        assertThat(updatedEodRun.get().getWsStatusCode()).isEqualTo("SUCCESS");
        assertThat(updatedEodRun.get().getEndedAt()).isNotNull();
    }
}