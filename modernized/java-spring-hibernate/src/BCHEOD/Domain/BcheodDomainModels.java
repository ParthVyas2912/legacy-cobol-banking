package src.BCHEOD.Domain;

import javax.validation.constraints.Size;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.OneToMany;
import java.math.BigDecimal;
import java.util.List;

public class BcheodDomainModels {

    @Entity
    public static class Account {
        @Id
        @Size(max = 10)
        @Column(length = 10)
        private String accountNumber; // PIC X(10)

        @Size(max = 30)
        @Column(length = 30)
        private String accountName; // PIC X(30)

        @Column(precision = 15, scale = 2)
        private BigDecimal accountBalance; // PIC S9(13)V99 COMP-3

        @Column(length = 1)
        private String accountStatus; // 88-level values

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
                for (AccountStatus status : values()) {
                    if (status.value.equals(value)) {
                        return status;
                    }
                }
                throw new IllegalArgumentException("Unknown value: " + value);
            }
        }

        // Getters and setters
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

        public String getAccountStatus() {
            return accountStatus;
        }

        public void setAccountStatus(String accountStatus) {
            this.accountStatus = accountStatus;
        }
    }

    @Entity
    public static class Transaction {
        @Id
        @Size(max = 12)
        @Column(length = 12)
        private String transactionId; // PIC X(12)

        @Column(precision = 13, scale = 2)
        private BigDecimal transactionAmount; // PIC S9(11)V99 COMP-3

        @Size(max = 8)
        @Column(length = 8)
        private String transactionDate; // PIC X(8)

        @Size(max = 1)
        @Column(length = 1)
        private String transactionType; // PIC X(1)

        @Column(length = 1)
        private String transactionStatus; // 88-level values

        public enum TransactionStatus {
            SUCCESS("S"),
            PENDING("P"),
            FAILED("F");

            private final String value;

            TransactionStatus(String value) {
                this.value = value;
            }

            public String getValue() {
                return value;
            }

            public static TransactionStatus fromValue(String value) {
                for (TransactionStatus status : values()) {
                    if (status.value.equals(value)) {
                        return status;
                    }
                }
                throw new IllegalArgumentException("Unknown value: " + value);
            }
        }

        // Getters and setters
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

        public String getTransactionType() {
            return transactionType;
        }

        public void setTransactionType(String transactionType) {
            this.transactionType = transactionType;
        }

        public String getTransactionStatus() {
            return transactionStatus;
        }

        public void setTransactionStatus(String transactionStatus) {
            this.transactionStatus = transactionStatus;
        }
    }

    @Entity
    public static class Customer {
        @Id
        @Size(max = 15)
        @Column(length = 15)
        private String customerId; // PIC X(15)

        @Size(max = 50)
        @Column(length = 50)
        private String customerName; // PIC X(50)

        @Size(max = 100)
        @Column(length = 100)
        private String customerAddress; // PIC X(100)

        @OneToMany
        private List<Account> accounts; // OCCURS n TIMES

        // Getters and setters
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
}