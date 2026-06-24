package src.FRDCHK.Infrastructure;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.time.OffsetDateTime;

// Entity for TXN_LEDGER table
@Entity
@Table(name = "txn_ledger", schema = "public")
class TxnLedger {

    @Id
    @Column(name = "txn_id", length = 10) // COBOL: TXN_ID PIC X(10)
    private String txnId;

    @Column(name = "txn_acct_number", length = 20) // COBOL: TXN_ACCT_NUMBER PIC X(20)
    private String txnAcctNumber;

    @Column(name = "acct", length = 20) // COBOL: ACCT PIC X(20)
    private String acct;

    @Column(name = "amount", precision = 15, scale = 2) // COBOL: AMOUNT PIC 9(13)V99 COMP-3
    private BigDecimal amount;

    @Column(name = "code", length = 3) // COBOL: CODE PIC X(3)
    private String code;

    @Column(name = "posted_at", columnDefinition = "TIMESTAMPTZ") // COBOL: POSTED_AT TIMESTAMP
    private OffsetDateTime postedAt;

    @Column(name = "txn_amount", precision = 15, scale = 2) // COBOL: TXN_AMOUNT PIC 9(13)V99 COMP-3
    private BigDecimal txnAmount;

    @Column(name = "txn_code", length = 3) // COBOL: TXN_CODE PIC X(3)
    private String txnCode;

    @Column(name = "count", precision = 5, scale = 0) // COBOL: COUNT PIC 9(5)
    private Integer count;

    @Column(name = "coalesce", length = 255) // COBOL: COALESCE PIC X(255) (Assumed correct as no COBOL mapping provided)
    private String coalesce;

    @Column(name = "ws_velocity_count", precision = 5, scale = 0) // COBOL: WS_VELOCITY_COUNT PIC 9(5)
    private Integer wsVelocityCount;

    @Column(name = "ws_velocity_amt", precision = 15, scale = 2) // COBOL: WS_VELOCITY_AMT PIC 9(13)V99 COMP-3
    private BigDecimal wsVelocityAmt;

    @Column(name = "ws_current_acct", length = 20) // COBOL: WS_CURRENT_ACCT PIC X(20)
    private String wsCurrentAcct;

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

    public OffsetDateTime getPostedAt() {
        return postedAt;
    }

    public void setPostedAt(OffsetDateTime postedAt) {
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

    public Integer getCount() {
        return count;
    }

    public void setCount(Integer count) {
        this.count = count;
    }

    public String getCoalesce() {
        return coalesce;
    }

    public void setCoalesce(String coalesce) {
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

    public String getWsCurrentAcct() {
        return wsCurrentAcct;
    }

    public void setWsCurrentAcct(String wsCurrentAcct) {
        this.wsCurrentAcct = wsCurrentAcct;
    }
}

// Repository for TXN_LEDGER
@Repository
interface TxnLedgerRepository extends JpaRepository<TxnLedger, String> {
}

// Entity for FRAUD_FLAGS table
@Entity
@Table(name = "fraud_flags", schema = "public")
class FraudFlags {

    @Id
    @Column(name = "acct", length = 20) // COBOL: ACCT PIC X(20)
    private String acct;

    @Column(name = "score", precision = 5, scale = 2) // COBOL: SCORE PIC 9(5)V99 COMP-3
    private BigDecimal score;

    @Column(name = "reason", length = 50) // COBOL: REASON PIC X(50)
    private String reason;

    @Column(name = "ts", columnDefinition = "TIMESTAMPTZ") // COBOL: TS TIMESTAMP
    private OffsetDateTime ts;

    @Column(name = "txn_acct_number", length = 20) // COBOL: TXN_ACCT_NUMBER PIC X(20)
    private String txnAcctNumber;

    @Column(name = "ws_risk_score", precision = 5, scale = 2) // COBOL: WS_RISK_SCORE PIC 9(5)V99 COMP-3
    private BigDecimal wsRiskScore;

    // Getters and Setters
    public String getAcct() {
        return acct;
    }

    public void setAcct(String acct) {
        this.acct = acct;
    }

    public BigDecimal getScore() {
        return score;
    }

    public void setScore(BigDecimal score) {
        this.score = score;
    }

    public String getReason() {
        return reason;
    }

    public void setReason(String reason) {
        this.reason = reason;
    }

    public OffsetDateTime getTs() {
        return ts;
    }

    public void setTs(OffsetDateTime ts) {
        this.ts = ts;
    }

    public String getTxnAcctNumber() {
        return txnAcctNumber;
    }

    public void setTxnAcctNumber(String txnAcctNumber) {
        this.txnAcctNumber = txnAcctNumber;
    }

    public BigDecimal getWsRiskScore() {
        return wsRiskScore;
    }

    public void setWsRiskScore(BigDecimal wsRiskScore) {
        this.wsRiskScore = wsRiskScore;
    }
}

// Repository for FRAUD_FLAGS
@Repository
interface FraudFlagsRepository extends JpaRepository<FraudFlags, String> {
}