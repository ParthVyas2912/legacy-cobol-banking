package src.TXNPOST.Infrastructure;

import jakarta.persistence.*;
import java.math.BigDecimal;

/**
 * Infrastructure layer for TXNPOST program.
 * This includes ORM mappings and repository interfaces.
 */
public class TxnpostInfrastructure {

    /**
     * Entity class for the TXNIN table.
     */
    @Entity
    @Table(name = "txnin", schema = "public")
    public static class Txnin {

        @Id
        @Column(name = "txn_id", length = 12, nullable = false)
        // COBOL: TXN-ID PIC 9(12)
        private String txnId;

        @Column(name = "txn_acct_number", length = 10, nullable = false)
        // COBOL: TXN-ACCT-NUMBER PIC 9(10)
        private String txnAcctNumber;

        @Column(name = "txn_post_date", nullable = false)
        // COBOL: TXN-POST-DATE PIC 9(8)
        private java.sql.Date txnPostDate;

        @Column(name = "txn_effective_date", nullable = false)
        // COBOL: TXN-EFFECTIVE-DATE PIC 9(8)
        private java.sql.Date txnEffectiveDate;

        @Column(name = "txn_amount", precision = 13, scale = 2, nullable = false)
        // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3
        private BigDecimal txnAmount;

        @Column(name = "txn_code", length = 3, nullable = false)
        // COBOL: TXN-CODE PIC X(03)
        private String txnCode;

        @Column(name = "txn_channel", length = 4, nullable = false)
        // COBOL: TXN-CHANNEL PIC X(04)
        private String txnChannel;

        @Column(name = "txn_ref_number", length = 20, nullable = false)
        // COBOL: TXN-REF-NUMBER PIC X(20)
        private String txnRefNumber;

        @Column(name = "txn_description", length = 40)
        // COBOL: TXN-DESCRIPTION PIC X(40)
        private String txnDescription;

        @Column(name = "txn_status", length = 1, nullable = false)
        // COBOL: TXN-STATUS PIC X(01)
        private String txnStatus;

        @Column(name = "txn_balance_after", precision = 15, scale = 2)
        // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3
        private BigDecimal txnBalanceAfter;

        @Column(name = "txn_user_id", length = 8, nullable = false)
        // COBOL: TXN-USER-ID PIC X(08)
        private String txnUserId;

        // Getters and Setters
        public String getTxnId() {
            return txnId;
        }

        public void setTxnId(String txnId) {
            this.txnId = txnId;
        }

        public String getTxnAcctNumber() {
            return txnAcctNumber;
        }

        public void setTxnAcctNumber(String txnAcctNumber) {
            this.txnAcctNumber = txnAcctNumber;
        }

        public java.sql.Date getTxnPostDate() {
            return txnPostDate;
        }

        public void setTxnPostDate(java.sql.Date txnPostDate) {
            this.txnPostDate = txnPostDate;
        }

        public java.sql.Date getTxnEffectiveDate() {
            return txnEffectiveDate;
        }

        public void setTxnEffectiveDate(java.sql.Date txnEffectiveDate) {
            this.txnEffectiveDate = txnEffectiveDate;
        }

        public BigDecimal getTxnAmount() {
            return txnAmount;
        }

        public void setTxnAmount(BigDecimal txnAmount) {
            this.txnAmount = txnAmount;
        }

        public String getTxnCode() {
            return txnCode;
        }

        public void setTxnCode(String txnCode) {
            this.txnCode = txnCode;
        }

        public String getTxnChannel() {
            return txnChannel;
        }

        public void setTxnChannel(String txnChannel) {
            this.txnChannel = txnChannel;
        }

        public String getTxnRefNumber() {
            return txnRefNumber;
        }

        public void setTxnRefNumber(String txnRefNumber) {
            this.txnRefNumber = txnRefNumber;
        }

        public String getTxnDescription() {
            return txnDescription;
        }

        public void setTxnDescription(String txnDescription) {
            this.txnDescription = txnDescription;
        }

        public String getTxnStatus() {
            return txnStatus;
        }

        public void setTxnStatus(String txnStatus) {
            this.txnStatus = txnStatus;
        }

        public BigDecimal getTxnBalanceAfter() {
            return txnBalanceAfter;
        }

        public void setTxnBalanceAfter(BigDecimal txnBalanceAfter) {
            this.txnBalanceAfter = txnBalanceAfter;
        }

        public String getTxnUserId() {
            return txnUserId;
        }

        public void setTxnUserId(String txnUserId) {
            this.txnUserId = txnUserId;
        }
    }

    /**
     * Repository interface for Txnin entity.
     */
    public interface TxninRepository extends JpaRepository<Txnin, String> {
        // Custom query methods can be added here if needed
    }

    /**
     * Entity class for the AUDITOUT table.
     */
    @Entity
    @Table(name = "auditout", schema = "public")
    public static class Auditout {
        @Id
        @GeneratedValue(strategy = GenerationType.IDENTITY)
        @Column(name = "audit_id", nullable = false)
        private Long auditId;

        @Column(name = "txn_id", length = 12, nullable = false)
        private String txnId;

        @Column(name = "audit_timestamp", nullable = false)
        private java.sql.Timestamp auditTimestamp;

        @Column(name = "audit_action", length = 20, nullable = false)
        private String auditAction;

        @Column(name = "audit_user_id", length = 8, nullable = false)
        private String auditUserId;

        // Getters and Setters
        public Long getAuditId() {
            return auditId;
        }

        public void setAuditId(Long auditId) {
            this.auditId = auditId;
        }

        public String getTxnId() {
            return txnId;
        }

        public void setTxnId(String txnId) {
            this.txnId = txnId;
        }

        public java.sql.Timestamp getAuditTimestamp() {
            return auditTimestamp;
        }

        public void setAuditTimestamp(java.sql.Timestamp auditTimestamp) {
            this.auditTimestamp = auditTimestamp;
        }

        public String getAuditAction() {
            return auditAction;
        }

        public void setAuditAction(String auditAction) {
            this.auditAction = auditAction;
        }

        public String getAuditUserId() {
            return auditUserId;
        }

        public void setAuditUserId(String auditUserId) {
            this.auditUserId = auditUserId;
        }
    }

    /**
     * Repository interface for Auditout entity.
     */
    public interface AuditoutRepository extends JpaRepository<Auditout, Long> {
        // Custom query methods can be added here if needed
    }

    /**
     * Entity class for the TXN_LEDGER table.
     */
    @Entity
    @Table(name = "txn_ledger", schema = "public")
    public static class TxnLedger {
        @Id
        @Column(name = "txn_id", length = 12, nullable = false)
        private String txnId;

        @Column(name = "txn_acct_number", length = 10, nullable = false)
        private String txnAcctNumber;

        @Column(name = "txn_amount", precision = 13, scale = 2, nullable = false)
        private BigDecimal txnAmount;

        @Column(name = "txn_code", length = 3, nullable = false)
        private String txnCode;

        @Column(name = "posted_at", nullable = false)
        private java.sql.Timestamp postedAt;

        // Getters and Setters
        public String getTxnId() {
            return txnId;
        }

        public void setTxnId(String txnId) {
            this.txnId = txnId;
        }

        public String getTxnAcctNumber() {
            return txnAcctNumber;
        }

        public void setTxnAcctNumber(String txnAcctNumber) {
            this.txnAcctNumber = txnAcctNumber;
        }

        public BigDecimal getTxnAmount() {
            return txnAmount;
        }

        public void setTxnAmount(BigDecimal txnAmount) {
            this.txnAmount = txnAmount;
        }

        public String getTxnCode() {
            return txnCode;
        }

        public void setTxnCode(String txnCode) {
            this.txnCode = txnCode;
        }

        public java.sql.Timestamp getPostedAt() {
            return postedAt;
        }

        public void setPostedAt(java.sql.Timestamp postedAt) {
            this.postedAt = postedAt;
        }
    }

    /**
     * Repository interface for TxnLedger entity.
     */
    public interface TxnLedgerRepository extends JpaRepository<TxnLedger, String> {
        // Custom query methods can be added here if needed
    }
}