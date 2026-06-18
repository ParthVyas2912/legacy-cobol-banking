package src.BCHEOD.Domain;

import java.math.BigDecimal;

/**
 * Domain model classes generated from COBOL data structures for BCHEOD.
 */
public class BcheodDomainModels {

    /**
     * Represents the main record structure.
     */
    public static class MainRecord {
        // PIC S9(13)V99 COMP-3
        private BigDecimal accountBalance;

        // PIC X(20)
        private String accountHolderName;

        // PIC 9(8)
        private int transactionDate;

        // PIC 9(5)V99 COMP-3
        private BigDecimal transactionAmount;

        // PIC X(1)
        private String transactionType;

        // PIC 9(4)
        private int branchCode;

        // PIC 9(3)
        private int currencyCode;

        // Nested group item
        private TransactionDetails transactionDetails;

        // OCCURS 10 TIMES
        private TransactionHistory[] transactionHistory = new TransactionHistory[10];

        // Getters and setters
        public BigDecimal getAccountBalance() {
            return accountBalance;
        }

        public void setAccountBalance(BigDecimal accountBalance) {
            this.accountBalance = accountBalance;
        }

        public String getAccountHolderName() {
            return accountHolderName;
        }

        public void setAccountHolderName(String accountHolderName) {
            this.accountHolderName = accountHolderName;
        }

        public int getTransactionDate() {
            return transactionDate;
        }

        public void setTransactionDate(int transactionDate) {
            this.transactionDate = transactionDate;
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

        public int getBranchCode() {
            return branchCode;
        }

        public void setBranchCode(int branchCode) {
            this.branchCode = branchCode;
        }

        public int getCurrencyCode() {
            return currencyCode;
        }

        public void setCurrencyCode(int currencyCode) {
            this.currencyCode = currencyCode;
        }

        public TransactionDetails getTransactionDetails() {
            return transactionDetails;
        }

        public void setTransactionDetails(TransactionDetails transactionDetails) {
            this.transactionDetails = transactionDetails;
        }

        public TransactionHistory[] getTransactionHistory() {
            return transactionHistory;
        }

        public void setTransactionHistory(TransactionHistory[] transactionHistory) {
            this.transactionHistory = transactionHistory;
        }
    }

    /**
     * Represents the transaction details group.
     */
    public static class TransactionDetails {
        // PIC 9(6)
        private int transactionId;

        // PIC X(15)
        private String transactionDescription;

        // Getters and setters
        public int getTransactionId() {
            return transactionId;
        }

        public void setTransactionId(int transactionId) {
            this.transactionId = transactionId;
        }

        public String getTransactionDescription() {
            return transactionDescription;
        }

        public void setTransactionDescription(String transactionDescription) {
            this.transactionDescription = transactionDescription;
        }
    }

    /**
     * Represents a single transaction history entry.
     */
    public static class TransactionHistory {
        // PIC 9(8)
        private int historyDate;

        // PIC 9(5)V99 COMP-3
        private BigDecimal historyAmount;

        // PIC X(1)
        private String historyType;

        // Getters and setters
        public int getHistoryDate() {
            return historyDate;
        }

        public void setHistoryDate(int historyDate) {
            this.historyDate = historyDate;
        }

        public BigDecimal getHistoryAmount() {
            return historyAmount;
        }

        public void setHistoryAmount(BigDecimal historyAmount) {
            this.historyAmount = historyAmount;
        }

        public String getHistoryType() {
            return historyType;
        }

        public void setHistoryType(String historyType) {
            this.historyType = historyType;
        }
    }
}