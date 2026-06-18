package src.INTCALC.Domain;

import java.math.BigDecimal;

/**
 * Domain model classes generated from the COBOL program INTCALC.
 */
public class IntcalcDomainModels {

    /**
     * Represents the main record structure of the INTCALC program.
     */
    public static class MainRecord {
        // PIC S9(13)V99 COMP-3
        private BigDecimal accountBalance;

        // PIC X(20)
        private String accountHolderName;

        // PIC 9(5)
        private int branchCode;

        // PIC 9(3)V9(2) COMP-3
        private BigDecimal interestRate;

        // PIC 9(8)
        private long accountNumber;

        // PIC X(1)
        private String accountType;

        // PIC 9(4)
        private int transactionCount;

        // OCCURS 12 TIMES
        private MonthlyTransaction[] monthlyTransactions;

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

        public int getBranchCode() {
            return branchCode;
        }

        public void setBranchCode(int branchCode) {
            this.branchCode = branchCode;
        }

        public BigDecimal getInterestRate() {
            return interestRate;
        }

        public void setInterestRate(BigDecimal interestRate) {
            this.interestRate = interestRate;
        }

        public long getAccountNumber() {
            return accountNumber;
        }

        public void setAccountNumber(long accountNumber) {
            this.accountNumber = accountNumber;
        }

        public String getAccountType() {
            return accountType;
        }

        public void setAccountType(String accountType) {
            this.accountType = accountType;
        }

        public int getTransactionCount() {
            return transactionCount;
        }

        public void setTransactionCount(int transactionCount) {
            this.transactionCount = transactionCount;
        }

        public MonthlyTransaction[] getMonthlyTransactions() {
            return monthlyTransactions;
        }

        public void setMonthlyTransactions(MonthlyTransaction[] monthlyTransactions) {
            this.monthlyTransactions = monthlyTransactions;
        }
    }

    /**
     * Represents a monthly transaction record.
     */
    public static class MonthlyTransaction {
        // PIC 9(4)
        private int month;

        // PIC S9(13)V99 COMP-3
        private BigDecimal transactionAmount;

        // Getters and setters
        public int getMonth() {
            return month;
        }

        public void setMonth(int month) {
            this.month = month;
        }

        public BigDecimal getTransactionAmount() {
            return transactionAmount;
        }

        public void setTransactionAmount(BigDecimal transactionAmount) {
            this.transactionAmount = transactionAmount;
        }
    }

    /**
     * Represents the account type as an enumerated value.
     */
    public enum AccountType {
        SAVINGS, // 88-level value for savings account
        CHECKING, // 88-level value for checking account
        BUSINESS // 88-level value for business account
    }
}