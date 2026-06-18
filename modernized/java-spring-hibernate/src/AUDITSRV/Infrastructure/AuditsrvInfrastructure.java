package src.AUDITSRV.Infrastructure;

import jakarta.persistence.*;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

/**
 * Infrastructure layer for AUDITSRV program.
 * Includes ORM entity mapping, repository interfaces, and bridge clients.
 */
public class AuditsrvInfrastructure {

    /**
     * ORM Entity for the AUDITVSAM table.
     */
    @Entity
    @Table(name = "auditvsam", schema = "public")
    public static class Auditvsam {

        @Id
        @Column(name = "audit_key", length = 20, nullable = false)
        // COBOL: AUDIT-KEY PIC X(20)
        private String auditKey;

        @Column(name = "audit_acct_number", precision = 10, scale = 0, nullable = false)
        // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)
        private Long auditAcctNumber;

        @Column(name = "audit_action", length = 16, nullable = false)
        // COBOL: AUDIT-ACTION PIC X(16)
        private String auditAction;

        @Column(name = "audit_user", length = 8, nullable = false)
        // COBOL: AUDIT-USER PIC X(08)
        private String auditUser;

        @Column(name = "audit_ts", columnDefinition = "TIMESTAMPTZ", nullable = false)
        // COBOL: AUDIT-TS PIC X(26)
        private String auditTs;

        @Column(name = "audit_detail", length = 120, nullable = true)
        // COBOL: AUDIT-DETAIL PIC X(120)
        private String auditDetail;

        // Getters and Setters
        public String getAuditKey() {
            return auditKey;
        }

        public void setAuditKey(String auditKey) {
            this.auditKey = auditKey;
        }

        public Long getAuditAcctNumber() {
            return auditAcctNumber;
        }

        public void setAuditAcctNumber(Long auditAcctNumber) {
            this.auditAcctNumber = auditAcctNumber;
        }

        public String getAuditAction() {
            return auditAction;
        }

        public void setAuditAction(String auditAction) {
            this.auditAction = auditAction;
        }

        public String getAuditUser() {
            return auditUser;
        }

        public void setAuditUser(String auditUser) {
            this.auditUser = auditUser;
        }

        public String getAuditTs() {
            return auditTs;
        }

        public void setAuditTs(String auditTs) {
            this.auditTs = auditTs;
        }

        public String getAuditDetail() {
            return auditDetail;
        }

        public void setAuditDetail(String auditDetail) {
            this.auditDetail = auditDetail;
        }
    }

    /**
     * Repository interface for accessing the AUDITVSAM table.
     */
    public interface AuditvsamRepository extends JpaRepository<Auditvsam, String> {

        /**
         * Finds all audit records by a specific user.
         *
         * @param auditUser The user ID to filter by.
         * @return List of Auditvsam entities.
         */
        List<Auditvsam> findByAuditUser(String auditUser);

        /**
         * Finds all audit records by a specific action.
         *
         * @param auditAction The action to filter by.
         * @return List of Auditvsam entities.
         */
        List<Auditvsam> findByAuditAction(String auditAction);
    }
}