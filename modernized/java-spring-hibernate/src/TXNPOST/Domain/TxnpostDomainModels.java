package src.TXNPOST.Domain;

import java.math.BigDecimal;

/**
 * Domain models generated from COBOL data structures for the TXNPOST program.
 */
public class TxnpostDomainModels {

    /**
     * Represents the primary transaction record.
     */
    public static class TransactionRecord {
        // PIC X(10)
        private String transactionId; // MaxLength(10)

        // PIC 9(8)
        private int accountNumber;

        // PIC S9(13)V99 COMP-3
        private BigDecimal transactionAmount; // Precision (15, 2)

        // PIC X(20)
        private String transactionDescription; // MaxLength(20)

        // PIC 9(6)
        private int transactionDate;

        // PIC 9(4)
        private int transactionTime;

        // PIC X(1)
        private String transactionStatus; // MaxLength(1)

        // PIC X(1)
        private String transactionType; // MaxLength(1)

        // PIC X(30)
        private String customerName; // MaxLength(30)

        // PIC X(15)
        private String customerPhone; // MaxLength(15)

        // PIC X(50)
        private String customerAddress; // MaxLength(50)

        // PIC 9(5)
        private int branchCode;

        // PIC X(3)
        private String currencyCode; // MaxLength(3)

        // PIC S9(7)V99 COMP-3
        private BigDecimal exchangeRate; // Precision (9, 2)

        // PIC 9(3)
        private int tellerId;

        // PIC X(1)
        private String reversalFlag; // MaxLength(1)

        // PIC X(1)
        private String authorizationFlag; // MaxLength(1)

        // PIC X(1)
        private String fraudCheckFlag; // MaxLength(1)

        // PIC X(1)
        private String settlementFlag; // MaxLength(1)

        // PIC X(1)
        private String postingFlag; // MaxLength(1)

        // PIC X(1)
        private String adjustmentFlag; // MaxLength(1)

        // PIC X(1)
        private String errorFlag; // MaxLength(1)

        // PIC X(1)
        private String overrideFlag; // MaxLength(1)

        // PIC X(1)
        private String batchFlag; // MaxLength(1)

        // PIC X(1)
        private String archiveFlag; // MaxLength(1)

        // PIC X(1)
        private String printFlag; // MaxLength(1)

        // PIC X(1)
        private String emailFlag; // MaxLength(1)

        // PIC X(1)
        private String smsFlag; // MaxLength(1)

        // PIC X(1)
        private String notificationFlag; // MaxLength(1)

        // PIC X(1)
        private String auditFlag; // MaxLength(1)

        // PIC X(1)
        private String complianceFlag; // MaxLength(1)

        // PIC X(1)
        private String regulatoryFlag; // MaxLength(1)

        // PIC X(1)
        private String reconciliationFlag; // MaxLength(1)

        // PIC X(1)
        private String reportingFlag; // MaxLength(1)

        // PIC X(1)
        private String reservedFlag; // MaxLength(1)

        // PIC X(10)
        private String reservedField1; // MaxLength(10)

        // PIC X(10)
        private String reservedField2; // MaxLength(10)

        // PIC X(10)
        private String reservedField3; // MaxLength(10)

        // PIC X(10)
        private String reservedField4; // MaxLength(10)

        // PIC X(10)
        private String reservedField5; // MaxLength(10)

        // PIC X(10)
        private String reservedField6; // MaxLength(10)

        // PIC X(10)
        private String reservedField7; // MaxLength(10)

        // PIC X(10)
        private String reservedField8; // MaxLength(10)

        // PIC X(10)
        private String reservedField9; // MaxLength(10)

        // PIC X(10)
        private String reservedField10; // MaxLength(10)

        // Getters and Setters
        public String getTransactionId() {
            return transactionId;
        }

        public void setTransactionId(String transactionId) {
            this.transactionId = transactionId;
        }

        public int getAccountNumber() {
            return accountNumber;
        }

        public void setAccountNumber(int accountNumber) {
            this.accountNumber = accountNumber;
        }

        public BigDecimal getTransactionAmount() {
            return transactionAmount;
        }

        public void setTransactionAmount(BigDecimal transactionAmount) {
            this.transactionAmount = transactionAmount;
        }

        public String getTransactionDescription() {
            return transactionDescription;
        }

        public void setTransactionDescription(String transactionDescription) {
            this.transactionDescription = transactionDescription;
        }

        public int getTransactionDate() {
            return transactionDate;
        }

        public void setTransactionDate(int transactionDate) {
            this.transactionDate = transactionDate;
        }

        public int getTransactionTime() {
            return transactionTime;
        }

        public void setTransactionTime(int transactionTime) {
            this.transactionTime = transactionTime;
        }

        public String getTransactionStatus() {
            return transactionStatus;
        }

        public void setTransactionStatus(String transactionStatus) {
            this.transactionStatus = transactionStatus;
        }

        public String getTransactionType() {
            return transactionType;
        }

        public void setTransactionType(String transactionType) {
            this.transactionType = transactionType;
        }

        public String getCustomerName() {
            return customerName;
        }

        public void setCustomerName(String customerName) {
            this.customerName = customerName;
        }

        public String getCustomerPhone() {
            return customerPhone;
        }

        public void setCustomerPhone(String customerPhone) {
            this.customerPhone = customerPhone;
        }

        public String getCustomerAddress() {
            return customerAddress;
        }

        public void setCustomerAddress(String customerAddress) {
            this.customerAddress = customerAddress;
        }

        public int getBranchCode() {
            return branchCode;
        }

        public void setBranchCode(int branchCode) {
            this.branchCode = branchCode;
        }

        public String getCurrencyCode() {
            return currencyCode;
        }

        public void setCurrencyCode(String currencyCode) {
            this.currencyCode = currencyCode;
        }

        public BigDecimal getExchangeRate() {
            return exchangeRate;
        }

        public void setExchangeRate(BigDecimal exchangeRate) {
            this.exchangeRate = exchangeRate;
        }

        public int getTellerId() {
            return tellerId;
        }

        public void setTellerId(int tellerId) {
            this.tellerId = tellerId;
        }

        public String getReversalFlag() {
            return reversalFlag;
        }

        public void setReversalFlag(String reversalFlag) {
            this.reversalFlag = reversalFlag;
        }

        public String getAuthorizationFlag() {
            return authorizationFlag;
        }

        public void setAuthorizationFlag(String authorizationFlag) {
            this.authorizationFlag = authorizationFlag;
        }

        public String getFraudCheckFlag() {
            return fraudCheckFlag;
        }

        public void setFraudCheckFlag(String fraudCheckFlag) {
            this.fraudCheckFlag = fraudCheckFlag;
        }

        public String getSettlementFlag() {
            return settlementFlag;
        }

        public void setSettlementFlag(String settlementFlag) {
            this.settlementFlag = settlementFlag;
        }

        public String getPostingFlag() {
            return postingFlag;
        }

        public void setPostingFlag(String postingFlag) {
            this.postingFlag = postingFlag;
        }

        public String getAdjustmentFlag() {
            return adjustmentFlag;
        }

        public void setAdjustmentFlag(String adjustmentFlag) {
            this.adjustmentFlag = adjustmentFlag;
        }

        public String getErrorFlag() {
            return errorFlag;
        }

        public void setErrorFlag(String errorFlag) {
            this.errorFlag = errorFlag;
        }

        public String getOverrideFlag() {
            return overrideFlag;
        }

        public void setOverrideFlag(String overrideFlag) {
            this.overrideFlag = overrideFlag;
        }

        public String getBatchFlag() {
            return batchFlag;
        }

        public void setBatchFlag(String batchFlag) {
            this.batchFlag = batchFlag;
        }

        public String getArchiveFlag() {
            return archiveFlag;
        }

        public void setArchiveFlag(String archiveFlag) {
            this.archiveFlag = archiveFlag;
        }

        public String getPrintFlag() {
            return printFlag;
        }

        public void setPrintFlag(String printFlag) {
            this.printFlag = printFlag;
        }

        public String getEmailFlag() {
            return emailFlag;
        }

        public void setEmailFlag(String emailFlag) {
            this.emailFlag = emailFlag;
        }

        public String getSmsFlag() {
            return smsFlag;
        }

        public void setSmsFlag(String smsFlag) {
            this.smsFlag = smsFlag;
        }

        public String getNotificationFlag() {
            return notificationFlag;
        }

        public void setNotificationFlag(String notificationFlag) {
            this.notificationFlag = notificationFlag;
        }

        public String getAuditFlag() {
            return auditFlag;
        }

        public void setAuditFlag(String auditFlag) {
            this.auditFlag = auditFlag;
        }

        public String getComplianceFlag() {
            return complianceFlag;
        }

        public void setComplianceFlag(String complianceFlag) {
            this.complianceFlag = complianceFlag;
        }

        public String getRegulatoryFlag() {
            return regulatoryFlag;
        }

        public void setRegulatoryFlag(String regulatoryFlag) {
            this.regulatoryFlag = regulatoryFlag;
        }

        public String getReconciliationFlag() {
            return reconciliationFlag;
        }

        public void setReconciliationFlag(String reconciliationFlag) {
            this.reconciliationFlag = reconciliationFlag;
        }

        public String getReportingFlag() {
            return reportingFlag;
        }

        public void setReportingFlag(String reportingFlag) {
            this.reportingFlag = reportingFlag;
        }

        public String getReservedFlag() {
            return reservedFlag;
        }

        public void setReservedFlag(String reservedFlag) {
            this.reservedFlag = reservedFlag;
        }

        public String getReservedField1() {
            return reservedField1;
        }

        public void setReservedField1(String reservedField1) {
            this.reservedField1 = reservedField1;
        }

        public String getReservedField2() {
            return reservedField2;
        }

        public void setReservedField2(String reservedField2) {
            this.reservedField2 = reservedField2;
        }

        public String getReservedField3() {
            return reservedField3;
        }

        public void setReservedField3(String reservedField3) {
            this.reservedField3 = reservedField3;
        }

        public String getReservedField4() {
            return reservedField4;
        }

        public void setReservedField4(String reservedField4) {
            this.reservedField4 = reservedField4;
        }

        public String getReservedField5() {
            return reservedField5;
        }

        public void setReservedField5(String reservedField5) {
            this.reservedField5 = reservedField5;
        }

        public String getReservedField6() {
            return reservedField6;
        }

        public void setReservedField6(String reservedField6) {
            this.reservedField6 = reservedField6;
        }

        public String getReservedField7() {
            return reservedField7;
        }

        public void setReservedField7(String reservedField7) {
            this.reservedField7 = reservedField7;
        }

        public String getReservedField8() {
            return reservedField8;
        }

        public void setReservedField8(String reservedField8) {
            this.reservedField8 = reservedField8;
        }

        public String getReservedField9() {
            return reservedField9;
        }

        public void setReservedField9(String reservedField9) {
            this.reservedField9 = reservedField9;
        }

        public String getReservedField10() {
            return reservedField10;
        }

        public void setReservedField10(String reservedField10) {
            this.reservedField10 = reservedField10;
        }
    }
}