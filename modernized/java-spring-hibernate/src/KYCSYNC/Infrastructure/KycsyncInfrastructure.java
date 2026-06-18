package src.KYCSYNC.Infrastructure;

import jakarta.persistence.*;
import java.time.OffsetDateTime;

/**
 * Infrastructure layer for KYCSYNC program.
 * This includes ORM entity mappings and repository interfaces.
 */
public class KycsyncInfrastructure {

    /**
     * ORM Entity for KYC Audit table.
     */
    @Entity
    @Table(name = "kyc_audit", schema = "public")
    public static class KycAudit {

        @Id
        @GeneratedValue(strategy = GenerationType.IDENTITY)
        @Column(name = "id")
        private Long id;

        @Column(name = "cust_id", length = 20, nullable = false)
        // COBOL: CUST_ID PIC X(20)
        private String custId;

        @Column(name = "cust_kyc_status", length = 30, nullable = false)
        // COBOL: CUST_KYC_STATUS PIC X(30)
        private String custKycStatus;

        @Column(name = "cust_risk_score", length = 30, nullable = false)
        // COBOL: CUST_RISK_SCORE PIC X(30)
        private String custRiskScore;

        // Getters and Setters
        public Long getId() {
            return id;
        }

        public void setId(Long id) {
            this.id = id;
        }

        public String getCustId() {
            return custId;
        }

        public void setCustId(String custId) {
            this.custId = custId;
        }

        public String getCustKycStatus() {
            return custKycStatus;
        }

        public void setCustKycStatus(String custKycStatus) {
            this.custKycStatus = custKycStatus;
        }

        public String getCustRiskScore() {
            return custRiskScore;
        }

        public void setCustRiskScore(String custRiskScore) {
            this.custRiskScore = custRiskScore;
        }
    }

    /**
     * Repository interface for KYC Audit table.
     */
    public interface KycAuditRepository extends JpaRepository<KycAudit, Long> {
        // Custom query methods can be added here if needed
    }
}