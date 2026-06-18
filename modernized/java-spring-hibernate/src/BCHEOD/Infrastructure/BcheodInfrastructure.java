package src.BCHEOD.Infrastructure;

import jakarta.persistence.*;
import java.time.OffsetDateTime;

/**
 * Infrastructure layer for BCHEOD program.
 * Contains ORM mappings, repositories, and bridge clients.
 */
public class BcheodInfrastructure {

    /**
     * ORM Entity for the EOD_RUNS table.
     */
    @Entity
    @Table(name = "eod_runs", schema = "public")
    public static class EodRuns {

        @Id
        @GeneratedValue(strategy = GenerationType.IDENTITY)
        @Column(name = "id")
        private Long id;

        @Column(name = "started_at", columnDefinition = "TIMESTAMPTZ", nullable = false)
        private OffsetDateTime startedAt; // COBOL: STARTED_AT

        @Column(name = "status", length = 20, nullable = false)
        private String status; // COBOL: STATUS (PIC X(20))

        @Column(name = "ended_at", columnDefinition = "TIMESTAMPTZ", nullable = true)
        private OffsetDateTime endedAt; // COBOL: ENDED_AT

        @Column(name = "ws_status_code", length = 10, nullable = true)
        private String wsStatusCode; // COBOL: WS_STATUS_CODE (PIC X(10))

        // Getters and setters
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

    /**
     * Repository interface for EodRuns entity.
     */
    public interface EodRunsRepository extends JpaRepository<EodRuns, Long> {
        // Additional query methods can be defined here if needed
    }

    /**
     * Bridge client for external systems (if applicable).
     */
    public static class ExternalSystemBridgeClient {
        // Placeholder for external system integration logic
        // Implement methods to interact with external systems as needed
    }
}