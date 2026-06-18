package src.INTCALC.Infrastructure;

import jakarta.persistence.*;
import java.math.BigDecimal;
import java.util.Date;

/**
 * Infrastructure classes for the INTCALC program.
 * This includes ORM mappings and repository interfaces.
 */
public class IntcalcInfrastructure {

    /**
     * Entity class for the ACCTFILE table.
     */
    @Entity
    @Table(name = "acctfile", schema = "public")
    public static class Acctfile {

        @Id
        @Column(name = "acct_number", length = 10, nullable = false)
        // COBOL: ACCT-NUMBER PIC 9(10)
        private String acctNumber;

        @Column(name = "acct_customer_id", length = 8, nullable = false)
        // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
        private String acctCustomerId;

        @Column(name = "acct_opened_date", nullable = false)
        // COBOL: ACCT-OPENED-DATE PIC 9(8)
        private Date acctOpenedDate;

        @Column(name = "acct_type", length = 2, nullable = false)
        // COBOL: ACCT-TYPE PIC X(02)
        private String acctType;

        @Column(name = "acct_status", length = 1, nullable = false)
        // COBOL: ACCT-STATUS PIC X(01)
        private String acctStatus;

        @Column(name = "acct_balance", precision = 15, scale = 2, nullable = false)
        // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3
        private BigDecimal acctBalance;

        @Column(name = "acct_avail_bal", precision = 15, scale = 2, nullable = false)
        // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3
        private BigDecimal acctAvailBal;

        @Column(name = "acct_int_rate", precision = 6, scale = 4, nullable = false)
        // COBOL: ACCT-INT-RATE PIC 9(2)V9(4) COMP-3
        private BigDecimal acctIntRate;

        @Column(name = "acct_last_txn_dt", nullable = false)
        // COBOL: ACCT-LAST-TXN-DT PIC 9(8)
        private Date acctLastTxnDt;

        @Column(name = "acct_currency", length = 3, nullable = false)
        // COBOL: ACCT-CURRENCY PIC X(03)
        private String acctCurrency;

        @Column(name = "acct_branch_code", length = 4, nullable = false)
        // COBOL: ACCT-BRANCH-CODE PIC 9(4)
        private String acctBranchCode;

        @Column(name = "acct_flags", length = 4, nullable = true)
        // COBOL: ACCT-FLAGS PIC X(04)
        private String acctFlags;

        @Column(name = "acct_filler", length = 50, nullable = true)
        // COBOL: ACCT-FILLER PIC X(50)
        private String acctFiller;

        // Getters and setters
        public String getAcctNumber() {
            return acctNumber;
        }

        public void setAcctNumber(String acctNumber) {
            this.acctNumber = acctNumber;
        }

        public String getAcctCustomerId() {
            return acctCustomerId;
        }

        public void setAcctCustomerId(String acctCustomerId) {
            this.acctCustomerId = acctCustomerId;
        }

        public Date getAcctOpenedDate() {
            return acctOpenedDate;
        }

        public void setAcctOpenedDate(Date acctOpenedDate) {
            this.acctOpenedDate = acctOpenedDate;
        }

        public String getAcctType() {
            return acctType;
        }

        public void setAcctType(String acctType) {
            this.acctType = acctType;
        }

        public String getAcctStatus() {
            return acctStatus;
        }

        public void setAcctStatus(String acctStatus) {
            this.acctStatus = acctStatus;
        }

        public BigDecimal getAcctBalance() {
            return acctBalance;
        }

        public void setAcctBalance(BigDecimal acctBalance) {
            this.acctBalance = acctBalance;
        }

        public BigDecimal getAcctAvailBal() {
            return acctAvailBal;
        }

        public void setAcctAvailBal(BigDecimal acctAvailBal) {
            this.acctAvailBal = acctAvailBal;
        }

        public BigDecimal getAcctIntRate() {
            return acctIntRate;
        }

        public void setAcctIntRate(BigDecimal acctIntRate) {
            this.acctIntRate = acctIntRate;
        }

        public Date getAcctLastTxnDt() {
            return acctLastTxnDt;
        }

        public void setAcctLastTxnDt(Date acctLastTxnDt) {
            this.acctLastTxnDt = acctLastTxnDt;
        }

        public String getAcctCurrency() {
            return acctCurrency;
        }

        public void setAcctCurrency(String acctCurrency) {
            this.acctCurrency = acctCurrency;
        }

        public String getAcctBranchCode() {
            return acctBranchCode;
        }

        public void setAcctBranchCode(String acctBranchCode) {
            this.acctBranchCode = acctBranchCode;
        }

        public String getAcctFlags() {
            return acctFlags;
        }

        public void setAcctFlags(String acctFlags) {
            this.acctFlags = acctFlags;
        }

        public String getAcctFiller() {
            return acctFiller;
        }

        public void setAcctFiller(String acctFiller) {
            this.acctFiller = acctFiller;
        }
    }

    /**
     * Entity class for the INTJRNL table.
     */
    @Entity
    @Table(name = "intjrnl", schema = "public")
    public static class Intjrnl {

        @Id
        @Column(name = "txn_id", length = 12, nullable = false)
        // COBOL: TXN-ID PIC 9(12)
        private String txnId;

        @Column(name = "txn_acct_number", length = 10, nullable = false)
        // COBOL: TXN-ACCT-NUMBER PIC 9(10)
        private String txnAcctNumber;

        @Column(name = "txn_post_date", nullable = false)
        // COBOL: TXN-POST-DATE PIC 9(8)
        private Date txnPostDate;

        @Column(name = "txn_effective_date", nullable = false)
        // COBOL: TXN-EFFECTIVE-DATE PIC 9(8)
        private Date txnEffectiveDate;

        @Column(name = "txn_amount", precision = 13, scale = 2, nullable = false)
        // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3
        private BigDecimal txnAmount;

        @Column(name = "txn_code", length = 3, nullable = false)
        // COBOL: TXN-CODE PIC X(03)
        private String txnCode;

        @Column(name = "txn_channel", length = 4, nullable = false)
        // COBOL: TXN-CHANNEL PIC X(04)
        private String txnChannel;

        @Column(name = "txn_ref_number", length = 20, nullable = true)
        // COBOL: TXN-REF-NUMBER PIC X(20)
        private String txnRefNumber;

        @Column(name = "txn_description", length = 40, nullable = true)
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

        // Getters and setters
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

        public Date getTxnPostDate() {
            return txnPostDate;
        }

        public void setTxnPostDate(Date txnPostDate) {
            this.txnPostDate = txnPostDate;
        }

        public Date getTxnEffectiveDate() {
            return txnEffectiveDate;
        }

        public void setTxnEffectiveDate(Date txnEffectiveDate) {
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
}