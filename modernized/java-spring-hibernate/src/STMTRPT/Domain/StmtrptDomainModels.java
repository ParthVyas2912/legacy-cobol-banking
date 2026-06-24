package src.STMTRPT.Domain;

import java.math.BigDecimal;
import java.util.List;

/**
 * Domain models for STMTRPT program.
 */
public class StmtrptDomainModels {

    /**
     * Represents the main record structure.
     */
    public static class MainRecord {

        private String accountNumber; // PIC X(10)

        private BigDecimal balance; // PIC S9(13)V99 COMP-3

        private int transactionCount; // PIC 9(5)

        private List<Transaction> transactions; // OCCURS 10 TIMES

        // Getters and setters
        public String getAccountNumber() {
            return accountNumber;
        }

        public void setAccountNumber(String accountNumber) {
            this.accountNumber = accountNumber;
        }

        public BigDecimal getBalance() {
            return balance;
        }

        public void setBalance(BigDecimal balance) {
            this.balance = balance;
        }

        public int getTransactionCount() {
            return transactionCount;
        }

        public void setTransactionCount(int transactionCount) {
            this.transactionCount = transactionCount;
        }

        public List<Transaction> getTransactions() {
            return transactions;
        }

        public void setTransactions(List<Transaction> transactions) {
            this.transactions = transactions;
        }
    }

    /**
     * Represents a transaction record.
     */
    public static class Transaction {

        private String transactionCode; // PIC X(6)

        private BigDecimal transactionAmount; // PIC S9(8)V99 COMP-3

        private String transactionDate; // PIC X(8)

        // Getters and setters
        public String getTransactionCode() {
            return transactionCode;
        }

        public void setTransactionCode(String transactionCode) {
            this.transactionCode = transactionCode;
        }

        public BigDecimal getTransactionAmount() {
            return transactionAmount;
        }

        public void setTransactionAmount(BigDecimal transactionAmount) {
            this.transactionAmount = transactionAmount;
        }

        public String getTransactionDate() {
            return transactionDate;
        }

        public void setTransactionDate(String transactionDate) {
            this.transactionDate = transactionDate;
        }
    }

    /**
     * Represents the account status as an enum.
     */
    public enum AccountStatus {
        ACTIVE("A"),
        INACTIVE("I"),
        CLOSED("C");

        private final String value;

        AccountStatus(String value) {
            this.value = value;
        }

        public String getValue() {
            return value;
        }

        public static AccountStatus fromValue(String value) {
            for (AccountStatus status : AccountStatus.values()) {
                if (status.value.equals(value)) {
                    return status;
                }
            }
            throw new IllegalArgumentException("Unknown value: " + value);
        }
    }
}