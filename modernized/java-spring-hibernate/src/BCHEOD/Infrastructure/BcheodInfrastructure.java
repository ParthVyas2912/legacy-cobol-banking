package src.BCHEOD.Infrastructure;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import org.springframework.stereotype.Service;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.transaction.annotation.Transactional;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.time.OffsetDateTime;
import java.util.Optional;

// Entity class for EodRuns
@Entity
@Table(name = "eod_runs", schema = "public")
class EodRuns {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    private Long id;

    @Column(name = "started_at", columnDefinition = "TIMESTAMPTZ")
    private OffsetDateTime startedAt; // COBOL: STARTED_AT TIMESTAMPTZ

    @Column(name = "status", length = 20)
    private String status; // COBOL: STATUS X(20)

    @Column(name = "ended_at", columnDefinition = "TIMESTAMPTZ")
    private OffsetDateTime endedAt; // COBOL: ENDED_AT TIMESTAMPTZ

    @Column(name = "ws_status_code", length = 10)
    private String wsStatusCode; // COBOL: WS_STATUS_CODE X(10)

    // Getters and Setters
    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public OffsetDateTime getStartedAt() {
        return startedAt;
    }

    public void setStartedAt(OffsetDateTime startedAt) {
        this.startedAt = startedAt;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public OffsetDateTime getEndedAt() {
        return endedAt;
    }

    public void setEndedAt(OffsetDateTime endedAt) {
        this.endedAt = endedAt;
    }

    public String getWsStatusCode() {
        return wsStatusCode;
    }

    public void setWsStatusCode(String wsStatusCode) {
        this.wsStatusCode = wsStatusCode;
    }
}

// Repository interface for EodRuns
@Repository
interface EodRunsRepository extends JpaRepository<EodRuns, Long> {

    @Query("SELECT e FROM EodRuns e WHERE e.id = :id")
    Optional<EodRuns> findById(Long id);

    @Query("SELECT e FROM EodRuns e WHERE e.startedAt = :startedAt")
    Optional<EodRuns> findByStartedAt(OffsetDateTime startedAt);
}

// Service for EodRuns operations
@Service
class EodRunsService {

    private final EodRunsRepository eodRunsRepository;

    @Autowired
    public EodRunsService(EodRunsRepository eodRunsRepository) {
        this.eodRunsRepository = eodRunsRepository;
    }

    @Transactional
    public EodRuns saveEodRun(EodRuns eodRun) {
        return eodRunsRepository.save(eodRun);
    }

    @Transactional(readOnly = true)
    public Optional<EodRuns> getEodRunById(Long id) {
        return eodRunsRepository.findById(id);
    }

    @Transactional(readOnly = true)
    public Optional<EodRuns> getEodRunByStartedAt(OffsetDateTime startedAt) {
        return eodRunsRepository.findByStartedAt(startedAt);
    }

    @Transactional
    public void deleteEodRun(Long id) {
        eodRunsRepository.deleteById(id);
    }
}

// Configuration class for BCHEOD infrastructure
@Configuration
class BcheodInfrastructureConfig {

    @Bean
    public EodRunsService eodRunsService(EodRunsRepository eodRunsRepository) {
        return new EodRunsService(eodRunsRepository);
    }
}