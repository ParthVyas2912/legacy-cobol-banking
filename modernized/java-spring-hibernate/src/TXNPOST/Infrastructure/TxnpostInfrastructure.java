package TXNPOST.Infrastructure;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;
import org.springframework.stereotype.Repository;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;
import java.math.BigDecimal;
import java.util.Optional;

@Configuration
@EnableJpaRepositories(basePackages = "TXNPOST.Infrastructure")
public class TxnpostInfrastructure {

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

        @Column(name = "txn_post_date", length = 8, nullable = false)
        // COBOL: TXN-POST-DATE PIC 9(8)
        private String txnPostDate;

        @Column(name = "txn_effective_date", length = 8, nullable = false)
        // COBOL: TXN-EFFECTIVE-DATE PIC 9(8)
        private String txnEffectiveDate;

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

        @Column(name = "txn_description", length = 40, nullable = false)
        // COBOL: TXN-DESCRIPTION PIC X(40)
        private String txnDescription;

        @Column(name = "txn_status", length = 1, nullable = false)
        // COBOL: TXN-STATUS PIC X(01)
        private String txnStatus;

        @Column(name = "txn_balance_after", precision = 15, scale = 2, nullable = false)
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

        public String getTxnPostDate() {
            return txnPostDate;
        }

        public void setTxnPostDate(String txnPostDate) {
            this.txnPostDate = txnPostDate;
        }

        public String getTxnEffectiveDate() {
            return txnEffectiveDate;
        }

        public void setTxnEffectiveDate(String txnEffectiveDate) {
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

    @Repository
    public interface TxninRepository extends JpaRepository<Txnin, String> {

        @Query("SELECT t FROM Txnin t WHERE t.txnId = :txnId")
        Optional<Txnin> findByTxnId(String txnId);
    }

    @Entity
    @Table(name = "auditout", schema = "public")
    public static class Auditout {
        // Placeholder for future implementation as columnMap is empty in the provided data-source mapping
    }

    @Repository
    public interface AuditoutRepository extends JpaRepository<Auditout, Long> {
        // Placeholder for future implementation as columnMap is empty in the provided data-source mapping
    }

    @Entity
    @Table(name = "txn_ledger", schema = "public")
    public static class TxnLedger {

        @Id
        @Column(name = "txn_id", length = 12, nullable = false)
        // COBOL: TXN-ID PIC 9(12)
        private String txnId;

        @Column(name = "count", length = 5)
        // COBOL: COUNT PIC 9(5)
        private Integer count;

        @Column(name = "coalesce", length = 5)
        // COBOL: COALESCE PIC 9(5)
        private Integer coalesce;

        @Column(name = "ws_velocity_count", length = 5)
        // COBOL: WS-VELOCITY-COUNT PIC 9(5)
        private Integer wsVelocityCount;

        @Column(name = "ws_velocity_amt", precision = 15, scale = 2)
        // COBOL: WS-VELOCITY-AMT PIC S9(13)V99 COMP-3
        private BigDecimal wsVelocityAmt;

        @Column(name = "txn_acct_number", length = 10)
        // COBOL: TXN-ACCT-NUMBER PIC 9(10)
        private String txnAcctNumber;

        @Column(name = "ws_current_acct", length = 10)
        // COBOL: WS-CURRENT-ACCT PIC 9(10)
        private String wsCurrentAcct;

        @Column(name = "acct", length = 10)
        // COBOL: ACCT PIC 9(10)
        private String acct;

        @Column(name = "amount", precision = 15, scale = 2)
        // COBOL: AMOUNT PIC S9(13)V99 COMP-3
        private BigDecimal amount;

        @Column(name = "code", length = 3)
        // COBOL: CODE PIC X(03)
        private String code;

        @Column(name = "posted_at", columnDefinition = "TIMESTAMPTZ")
        private String postedAt;

        @Column(name = "txn_amount", precision = 13, scale = 2)
        // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3
        private BigDecimal txnAmount;

        @Column(name = "txn_code", length = 3)
        // COBOL: TXN-CODE PIC X(03)
        private String txnCode;

        // Getters and Setters
        public String getTxnId() {
            return txnId;
        }

        public void setTxnId(String txnId) {
            this.txnId = txnId;
        }

        public Integer getCount() {
            return count;
        }

        public void setCount(Integer count) {
            this.count = count;
        }

        public Integer getCoalesce() {
            return coalesce;
        }

        public void setCoalesce(Integer coalesce) {
            this.coalesce = coalesce;
        }

        public Integer getWsVelocityCount() {
            return wsVelocityCount;
        }

        public void setWsVelocityCount(Integer wsVelocityCount) {
            this.wsVelocityCount = wsVelocityCount;
        }

        public BigDecimal getWsVelocityAmt() {
            return wsVelocityAmt;
        }

        public void setWsVelocityAmt(BigDecimal wsVelocityAmt) {
            this.wsVelocityAmt = wsVelocityAmt;
        }

        public String getTxnAcctNumber() {
            return txnAcctNumber;
        }

        public void setTxnAcctNumber(String txnAcctNumber) {
            this.txnAcctNumber = txnAcctNumber;
        }

        public String getWsCurrentAcct() {
            return wsCurrentAcct;
        }

        public void setWsCurrentAcct(String wsCurrentAcct) {
            this.wsCurrentAcct = wsCurrentAcct;
        }

        public String getAcct() {
            return acct;
        }

        public void setAcct(String acct) {
            this.acct = acct;
        }

        public BigDecimal getAmount() {
            return amount;
        }

        public void setAmount(BigDecimal amount) {
            this.amount = amount;
        }

        public String getCode() {
            return code;
        }

        public void setCode(String code) {
            this.code = code;
        }

        public String getPostedAt() {
            return postedAt;
        }

        public void setPostedAt(String postedAt) {
            this.postedAt = postedAt;
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
    }

    @Repository
    public interface TxnLedgerRepository extends JpaRepository<TxnLedger, String> {

        @Query("SELECT t FROM TxnLedger t WHERE t.txnId = :txnId")
        Optional<TxnLedger> findByTxnId(String txnId);
    }
}