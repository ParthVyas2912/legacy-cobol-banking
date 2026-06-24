package src.ACCTMGR.Domain;

import java.math.BigDecimal;
import java.util.List;

public class AcctmgrDomainModels {

    public static class Account {
        private String accountNumber; // PIC X(10)
        private String accountName; // PIC X(30)
        private BigDecimal accountBalance; // PIC S9(13)V99 COMP-3
        private AccountStatus accountStatus;

        // Getters and Setters
        public String getAccountNumber() {
            return accountNumber;
        }

        public void setAccountNumber(String accountNumber) {
            this.accountNumber = accountNumber;
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

        public AccountStatus getAccountStatus() {
            return accountStatus;
        }

        public void setAccountStatus(AccountStatus accountStatus) {
            this.accountStatus = accountStatus;
        }
    }

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

    public static class Customer {
        private String customerId; // PIC X(15)
        private String customerName; // PIC X(50)
        private String customerAddress; // PIC X(100)
        private List<Account> accounts; // OCCURS n TIMES

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

        public String getCustomerAddress() {
            return customerAddress;
        }

        public void setCustomerAddress(String customerAddress) {
            this.customerAddress = customerAddress;
        }

        public List<Account> getAccounts() {
            return accounts;
        }

        public void setAccounts(List<Account> accounts) {
            this.accounts = accounts;
        }
    }

    public static class Transaction {
        private String transactionId; // PIC X(12)
        private BigDecimal transactionAmount; // PIC S9(11)V99 COMP-3
        private String transactionDate; // PIC X(8)
        private TransactionType transactionType;

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

        public String getTransactionDate() {
            return transactionDate;
        }

        public void setTransactionDate(String transactionDate) {
            this.transactionDate = transactionDate;
        }

        public TransactionType getTransactionType() {
            return transactionType;
        }

        public void setTransactionType(TransactionType transactionType) {
            this.transactionType = transactionType;
        }
    }

    public enum TransactionType {
        DEPOSIT("D"),
        WITHDRAWAL("W"),
        TRANSFER("T");

        private final String value;

        TransactionType(String value) {
            this.value = value;
        }

        public String getValue() {
            return value;
        }

        public static TransactionType fromValue(String value) {
            for (TransactionType type : TransactionType.values()) {
                if (type.value.equals(value)) {
                    return type;
                }
            }
            throw new IllegalArgumentException("Unknown value: " + value);
        }
    }
}