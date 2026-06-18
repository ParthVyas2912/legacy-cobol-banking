package src.ACCTMGR.Infrastructure;

import jakarta.persistence.*;
import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Infrastructure classes for the ACCTMGR application, including ORM mappings and repository implementations.
 */
public class AcctmgrInfrastructure {

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
        private LocalDateTime acctOpenedDate;

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
        // COBOL: ACCT-INT-RATE PIC 9(2)V9(4)
        private BigDecimal acctIntRate;

        @Column(name = "acct_last_txn_dt", nullable = false)
        // COBOL: ACCT-LAST-TXN-DT PIC 9(8)
        private LocalDateTime acctLastTxnDt;

        @Column(name = "acct_currency", length = 3, nullable = false)
        // COBOL: ACCT-CURRENCY PIC X(03)
        private String acctCurrency;

        @Column(name = "acct_branch_code", length = 4, nullable = false)
        // COBOL: ACCT-BRANCH-CODE PIC 9(4)
        private String acctBranchCode;

        @Column(name = "acct_flags", length = 4, nullable = false)
        // COBOL: ACCT-FLAGS PIC X(04)
        private String acctFlags;

        @Column(name = "acct_filler", length = 50, nullable = true)
        // COBOL: ACCT-FILLER PIC X(50)
        private String acctFiller;

        // Getters and Setters
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

        public LocalDateTime getAcctOpenedDate() {
            return acctOpenedDate;
        }

        public void setAcctOpenedDate(LocalDateTime acctOpenedDate) {
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

        public LocalDateTime getAcctLastTxnDt() {
            return acctLastTxnDt;
        }

        public void setAcctLastTxnDt(LocalDateTime acctLastTxnDt) {
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
     * Entity class for the ACCT_AUDIT table.
     */
    @Entity
    @Table(name = "acct_audit", schema = "public")
    public static class AcctAudit {

        @Id
        @GeneratedValue(strategy = GenerationType.IDENTITY)
        @Column(name = "id")
        private Long id;

        @Column(name = "acct_num", length = 10, nullable = false)
        // COBOL: ACCT-NUM PIC X(10)
        private String acctNum;

        @Column(name = "action", length = 20, nullable = false)
        // COBOL: ACTION PIC X(20)
        private String action;

        @Column(name = "actor", length = 30, nullable = false)
        // COBOL: ACTOR PIC X(30)
        private String actor;

        @Column(name = "ts", nullable = false)
        // COBOL: TS PIC 9(8)
        private LocalDateTime ts;

        @Column(name = "acct_number", length = 10, nullable = false)
        // COBOL: ACCT-NUMBER PIC 9(10)
        private String acctNumber;

        @Column(name = "comm_function", length = 15, nullable = false)
        // COBOL: COMM-FUNCTION PIC X(15)
        private String commFunction;

        // Getters and Setters
        public Long getId() {
            return id;
        }

        public void setId(Long id) {
            this.id = id;
        }

        public String getAcctNum() {
            return acctNum;
        }

        public void setAcctNum(String acctNum) {
            this.acctNum = acctNum;
        }

        public String getAction() {
            return action;
        }

        public void setAction(String action) {
            this.action = action;
        }

        public String getActor() {
            return actor;
        }

        public void setActor(String actor) {
            this.actor = actor;
        }

        public LocalDateTime getTs() {
            return ts;
        }

        public void setTs(LocalDateTime ts) {
            this.ts = ts;
        }

        public String getAcctNumber() {
            return acctNumber;
        }

        public void setAcctNumber(String acctNumber) {
            this.acctNumber = acctNumber;
        }

        public String getCommFunction() {
            return commFunction;
        }

        public void setCommFunction(String commFunction) {
            this.commFunction = commFunction;
        }
    }

    /**
     * Entity class for the CUSTOMER table.
     */
    @Entity
    @Table(name = "customer", schema = "public")
    public static class Customer {

        @Id
        @GeneratedValue(strategy = GenerationType.IDENTITY)
        @Column(name = "id")
        private Long id;

        @Column(name = "active_acct_count", precision = 5, scale = 0, nullable = false)
        // COBOL: ACTIVE-ACCT-COUNT PIC 9(5)
        private Integer activeAcctCount;

        @Column(name = "acct_customer_id", length = 8, nullable = false)
        // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
        private String acctCustomerId;

        // Getters and Setters
        public Long getId() {
            return id;
        }

        public void setId(Long id) {
            this.id = id;
        }

        public Integer getActiveAcctCount() {
            return activeAcctCount;
        }

        public void setActiveAcctCount(Integer activeAcctCount) {
            this.activeAcctCount = activeAcctCount;
        }

        public String getAcctCustomerId() {
            return acctCustomerId;
        }

        public void setAcctCustomerId(String acctCustomerId) {
            this.acctCustomerId = acctCustomerId;
        }
    }
}