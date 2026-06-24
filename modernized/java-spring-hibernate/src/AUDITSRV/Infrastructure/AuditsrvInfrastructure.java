package src.AUDITSRV.Infrastructure;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.jpa.repository.support.JpaRepositoryFactoryBean;
import org.springframework.stereotype.Repository;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.persistence.EntityManager;
import java.util.List;

import src.AUDITSRV.Domain.AuditsrvDomainModels.AuditRecord;

@Entity
@Table(name = "auditvsam", schema = "public")
class Auditvsam {

    @Id
    @Column(name = "audit_key", length = 20, nullable = false)
    // COBOL: AUDIT-KEY PIC X(20)
    private String auditKey;

    @Column(name = "audit_acct_number", length = 10, nullable = false)
    // COBOL: AUDIT-ACCT-NUMBER PIC 9(10)
    private String auditAcctNumber;

    @Column(name = "audit_action", length = 16, nullable = false)
    // COBOL: AUDIT-ACTION PIC X(16)
    private String auditAction;

    @Column(name = "audit_user", length = 8, nullable = false)
    // COBOL: AUDIT-USER PIC X(08)
    private String auditUser;

    @Column(name = "audit_ts", columnDefinition = "TIMESTAMP", nullable = false)
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

    public String getAuditAcctNumber() {
        return auditAcctNumber;
    }

    public void setAuditAcctNumber(String auditAcctNumber) {
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

@Repository
interface AuditvsamRepository extends JpaRepository<Auditvsam, String> {

    @Query("SELECT a FROM Auditvsam a WHERE a.auditAcctNumber = :batchId")
    List<Auditvsam> findByBatchId(String batchId);
}

@Configuration
public class AuditsrvInfrastructure {

    @Bean
    public JpaRepositoryFactoryBean<AuditvsamRepository, Auditvsam, String> auditvsamRepository(EntityManager entityManager) {
        JpaRepositoryFactoryBean<AuditvsamRepository, Auditvsam, String> factoryBean = new JpaRepositoryFactoryBean<>(AuditvsamRepository.class);
        factoryBean.setEntityManager(entityManager);
        return factoryBean;
    }
}