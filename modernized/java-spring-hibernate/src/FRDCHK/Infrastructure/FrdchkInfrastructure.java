package src.FRDCHK.Infrastructure;

import jakarta.persistence.*;
import java.math.BigDecimal;
import java.time.OffsetDateTime;

/**
 * Infrastructure classes for the FRDCHK program, including ORM mappings and repository interfaces.
 */
public class FrdchkInfrastructure {

    /**
     * Entity class for the "public.txn_ledger" table.
     */
    @Entity
    @Table(name = "txn_ledger", schema = "public")
    public static class TxnLedger {

        @Id
        @GeneratedValue(strategy = GenerationType.IDENTITY)
        @Column(name = "txn_id", nullable = false, length = 30)
        private String txnId; // COBOL: TXN_ID PIC X(30)

        @Column(name = "count", length = 30)
        private String count; // COBOL: COUNT PIC X(30)

        @Column(name = "coalesce", length = 30)
        private String coalesce; // COBOL: COALESCE PIC X(30)

        @Column(name = "ws_velocity_count", length = 30)
        private String wsVelocityCount; // COBOL: WS_VELOCITY_COUNT PIC X(30)

        @Column(name = "ws_velocity_amt", precision = 15, scale = 2)
        private BigDecimal wsVelocityAmt; // COBOL: WS_VELOCITY_AMT PIC S9(13)V99 COMP-3

        @Column(name = "txn_acct_number", length = 30)
        private String txnAcctNumber; // COBOL: TXN_ACCT_NUMBER PIC X(30)

        @Column(name = "ws_current_acct", length = 30)
        private String wsCurrentAcct; // COBOL: WS_CURRENT_ACCT PIC X(30)

        @Column(name = "acct", length = 30)
        private String acct; // COBOL: ACCT PIC X(30)

        @Column(name = "amount", precision = 15, scale = 2)
        private BigDecimal amount; // COBOL: AMOUNT PIC S9(13)V99 COMP-3

        @Column(name = "code", length = 30)
        private String code; // COBOL: CODE PIC X(30)

        @Column(name = "posted_at")
        private OffsetDateTime postedAt; // COBOL: POSTED_AT TIMESTAMPTZ

        @Column(name = "txn_amount", precision = 15, scale = 2)
        private BigDecimal txnAmount; // COBOL: TXN_AMOUNT PIC S9(13)V99 COMP-3

        @Column(name = "txn_code", length = 30)
        private String txnCode; // COBOL: TXN_CODE PIC X(30)

        // Getters and Setters
        public String getTxnId() {
            return txnId;
        }

        public void setTxnId(String txnId) {
            this.txnId = txnId;
        }

        public String getCount() {
            return count;
        }

        public void setCount(String count) {
            this.count = count;
        }

        public String getCoalesce() {
            return coalesce;
        }

        public void setCoalesce(String coalesce) {
            this.coalesce = coalesce;
        }

        public String getWsVelocityCount() {
            return wsVelocityCount;
        }

        public void setWsVelocityCount(String wsVelocityCount) {
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
    }

    /**
     * Entity class for the "public.fraud_flags" table.
     */
    @Entity
    @Table(name = "fraud_flags", schema = "public")
    public static class FraudFlags {

        @Id
        @GeneratedValue(strategy = GenerationType.IDENTITY)
        @Column(name = "acct", nullable = false, length = 30)
        private String acct; // COBOL: ACCT PIC X(30)

        @Column(name = "score", precision = 15, scale = 2)
        private BigDecimal score; // COBOL: SCORE PIC S9(13)V99 COMP-3

        @Column(name = "reason", length = 30)
        private String reason; // COBOL: REASON PIC X(30)

        @Column(name = "ts")
        private OffsetDateTime ts; // COBOL: TS TIMESTAMPTZ

        @Column(name = "txn_acct_number", length = 30)
        private String txnAcctNumber; // COBOL: TXN_ACCT_NUMBER PIC X(30)

        @Column(name = "ws_risk_score", precision = 15, scale = 2)
        private BigDecimal wsRiskScore; // COBOL: WS_RISK_SCORE PIC S9(13)V99 COMP-3

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
}