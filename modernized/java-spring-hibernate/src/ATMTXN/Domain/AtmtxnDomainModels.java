package src.ATMTXN.Domain;

import java.math.BigDecimal;

/**
 * Domain models for the ATMTXN program.
 */
public class AtmtxnDomainModels {

    /**
     * Represents the ATM Transaction record.
     */
    public static class AtmTransaction {
        // PIC X(16)
        private String transactionId; // MaxLength(16)

        // PIC 9(10)
        private long accountNumber;

        // PIC S9(13)V99 COMP-3
        private BigDecimal transactionAmount; // Precision (15, 2)

        // PIC X(10)
        private String transactionType; // MaxLength(10)

        // PIC X(8)
        private String transactionDate; // MaxLength(8)

        // PIC X(6)
        private String transactionTime; // MaxLength(6)

        // PIC X(3)
        private String atmLocationCode; // MaxLength(3)

        // PIC X(1)
        private String transactionStatus; // MaxLength(1)

        // PIC X(20)
        private String customerName; // MaxLength(20)

        // PIC X(30)
        private String remarks; // MaxLength(30)

        // Getters and Setters
        public String getTransactionId() {
            return transactionId;
        }

        public void setTransactionId(String transactionId) {
            this.transactionId = transactionId;
        }

        public long getAccountNumber() {
            return accountNumber;
        }

        public void setAccountNumber(long accountNumber) {
            this.accountNumber = accountNumber;
        }

        public BigDecimal getTransactionAmount() {
            return transactionAmount;
        }

        public void setTransactionAmount(BigDecimal transactionAmount) {
            this.transactionAmount = transactionAmount;
        }

        public String getTransactionType() {
            return transactionType;
        }

        public void setTransactionType(String transactionType) {
            this.transactionType = transactionType;
        }

        public String getTransactionDate() {
            return transactionDate;
        }

        public void setTransactionDate(String transactionDate) {
            this.transactionDate = transactionDate;
        }

        public String getTransactionTime() {
            return transactionTime;
        }

        public void setTransactionTime(String transactionTime) {
            this.transactionTime = transactionTime;
        }

        public String getAtmLocationCode() {
            return atmLocationCode;
        }

        public void setAtmLocationCode(String atmLocationCode) {
            this.atmLocationCode = atmLocationCode;
        }

        public String getTransactionStatus() {
            return transactionStatus;
        }

        public void setTransactionStatus(String transactionStatus) {
            this.transactionStatus = transactionStatus;
        }

        public String getCustomerName() {
            return customerName;
        }

        public void setCustomerName(String customerName) {
            this.customerName = customerName;
        }

        public String getRemarks() {
            return remarks;
        }

        public void setRemarks(String remarks) {
            this.remarks = remarks;
        }
    }
}