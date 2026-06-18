package src.ACCTMGR.Domain;

import java.math.BigDecimal;

/**
 * Domain model classes generated from the COBOL program ACCTMGR.
 * Each field includes a comment with the original COBOL PIC clause.
 */
public class AcctmgrDomainModels {

    /**
     * Account class representing the main account structure.
     */
    public static class Account {
        // PIC X(10)
        private String accountId; // MaxLength(10)

        // PIC X(30)
        private String accountName; // MaxLength(30)

        // PIC S9(13)V99 COMP-3
        private BigDecimal accountBalance; // Precision (15, 2)

        // PIC 9(4)
        private int branchCode;

        // PIC 9(8)
        private long creationDate;

        // PIC X(1)
        private AccountStatus accountStatus; // Enum for 88-level values

        // Getters and Setters
        public String getAccountId() {
            return accountId;
        }

        public void setAccountId(String accountId) {
            this.accountId = accountId;
        }

        public String getAccountName() {
            return accountName;
        }

        public void setAccountName(String accountName) {
            this.accountName = accountName;
        }

        public BigDecimal getAccountBalance() {
            return accountBalance;
        }

        public void setAccountBalance(BigDecimal accountBalance) {
            this.accountBalance = accountBalance;
        }

        public int getBranchCode() {
            return branchCode;
        }

        public void setBranchCode(int branchCode) {
            this.branchCode = branchCode;
        }

        public long getCreationDate() {
            return creationDate;
        }

        public void setCreationDate(long creationDate) {
            this.creationDate = creationDate;
        }

        public AccountStatus getAccountStatus() {
            return accountStatus;
        }

        public void setAccountStatus(AccountStatus accountStatus) {
            this.accountStatus = accountStatus;
        }
    }

    /**
     * Enum representing account status (88-level values).
     */
    public enum AccountStatus {
        ACTIVE, // 88 ACTIVE VALUE 'A'
        INACTIVE, // 88 INACTIVE VALUE 'I'
        CLOSED // 88 CLOSED VALUE 'C'
    }

    /**
     * Transaction class representing a transaction record.
     */
    public static class Transaction {
        // PIC X(12)
        private String transactionId; // MaxLength(12)

        // PIC S9(13)V99 COMP-3
        private BigDecimal transactionAmount; // Precision (15, 2)

        // PIC 9(8)
        private long transactionDate;

        // PIC X(1)
        private TransactionType transactionType; // Enum for 88-level values

        // Getters and Setters
        public String getTransactionId() {
            return transactionId;
        }

        public void setTransactionId(String transactionId) {
            this.transactionId = transactionId;
        }

        public BigDecimal getTransactionAmount() {
            return transactionAmount;
        }

        public void setTransactionAmount(BigDecimal transactionAmount) {
            this.transactionAmount = transactionAmount;
        }

        public long getTransactionDate() {
            return transactionDate;
        }

        public void setTransactionDate(long transactionDate) {
            this.transactionDate = transactionDate;
        }

        public TransactionType getTransactionType() {
            return transactionType;
        }

        public void setTransactionType(TransactionType transactionType) {
            this.transactionType = transactionType;
        }
    }

    /**
     * Enum representing transaction type (88-level values).
     */
    public enum TransactionType {
        DEPOSIT, // 88 DEPOSIT VALUE 'D'
        WITHDRAWAL, // 88 WITHDRAWAL VALUE 'W'
        TRANSFER // 88 TRANSFER VALUE 'T'
    }

    /**
     * Customer class representing a customer record.
     */
    public static class Customer {
        // PIC X(10)
        private String customerId; // MaxLength(10)

        // PIC X(30)
        private String customerName; // MaxLength(30)

        // PIC 9(8)
        private long dateOfBirth;

        // PIC X(1)
        private Gender gender; // Enum for 88-level values

        // Getters and Setters
        public String getCustomerId() {
            return customerId;
        }

        public void setCustomerId(String customerId) {
            this.customerId = customerId;
        }

        public String getCustomerName() {
            return customerName;
        }

        public void setCustomerName(String customerName) {
            this.customerName = customerName;
        }

        public long getDateOfBirth() {
            return dateOfBirth;
        }

        public void setDateOfBirth(long dateOfBirth) {
            this.dateOfBirth = dateOfBirth;
        }

        public Gender getGender() {
            return gender;
        }

        public void setGender(Gender gender) {
            this.gender = gender;
        }
    }

    /**
     * Enum representing gender (88-level values).
     */
    public enum Gender {
        MALE, // 88 MALE VALUE 'M'
        FEMALE // 88 FEMALE VALUE 'F'
    }
}