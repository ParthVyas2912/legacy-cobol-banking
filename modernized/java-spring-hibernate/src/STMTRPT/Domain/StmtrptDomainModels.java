package src.STMTRPT.Domain;

import java.math.BigDecimal;
import java.util.List;

/**
 * Domain models generated from COBOL program STMTRPT.
 */
public class StmtrptDomainModels {

    /**
     * Represents the main record structure of STMTRPT.
     */
    public static class MainRecord {
        // PIC X(10)
        private String accountNumber; // MaxLength(10)

        // PIC S9(13)V99 COMP-3
        private BigDecimal balance; // Precision (15, 2)

        // PIC 9(8)
        private int transactionDate; // YYYYMMDD format

        // PIC X(30)
        private String transactionDescription; // MaxLength(30)

        // OCCURS 10 TIMES
        private List<TransactionDetail> transactionDetails;

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

        public int getTransactionDate() {
            return transactionDate;
        }

        public void setTransactionDate(int transactionDate) {
            this.transactionDate = transactionDate;
        }

        public String getTransactionDescription() {
            return transactionDescription;
        }

        public void setTransactionDescription(String transactionDescription) {
            this.transactionDescription = transactionDescription;
        }

        public List<TransactionDetail> getTransactionDetails() {
            return transactionDetails;
        }

        public void setTransactionDetails(List<TransactionDetail> transactionDetails) {
            this.transactionDetails = transactionDetails;
        }
    }

    /**
     * Represents a transaction detail record.
     */
    public static class TransactionDetail {
        // PIC X(5)
        private String transactionCode; // MaxLength(5)

        // PIC S9(9)V99 COMP-3
        private BigDecimal transactionAmount; // Precision (11, 2)

        // PIC 9(6)
        private int transactionTime; // HHMMSS format

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

        public int getTransactionTime() {
            return transactionTime;
        }

        public void setTransactionTime(int transactionTime) {
            this.transactionTime = transactionTime;
        }
    }
}