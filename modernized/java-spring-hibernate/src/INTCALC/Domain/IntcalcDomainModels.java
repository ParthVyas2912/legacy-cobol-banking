package src.INTCALC.Domain;

import javax.validation.constraints.Size; // Corrected import for validation
import java.math.BigDecimal;
import java.util.List;

public class IntcalcDomainModels {

    public static class Account {
        @Size(max = 10)
        private String accountNumber; // PIC X(10)

        @Size(max = 30)
        private String accountName; // PIC X(30)

        private BigDecimal accountBalance; // PIC S9(13)V99 COMP-3

        private AccountStatus accountStatus;

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

    public static class InterestCalculation {
        private BigDecimal interestRate; // PIC S9(5)V99 COMP-3

        private BigDecimal principalAmount; // PIC S9(13)V99 COMP-3

        private BigDecimal calculatedInterest; // PIC S9(13)V99 COMP-3

        public BigDecimal getInterestRate() {
            return interestRate;
        }

        public void setInterestRate(BigDecimal interestRate) {
            this.interestRate = interestRate;
        }

        public BigDecimal getPrincipalAmount() {
            return principalAmount;
        }

        public void setPrincipalAmount(BigDecimal principalAmount) {
            this.principalAmount = principalAmount;
        }

        public BigDecimal getCalculatedInterest() {
            return calculatedInterest;
        }

        public void setCalculatedInterest(BigDecimal calculatedInterest) {
            this.calculatedInterest = calculatedInterest;
        }
    }

    public static class Transaction {
        @Size(max = 15)
        private String transactionId; // PIC X(15)

        private BigDecimal transactionAmount; // PIC S9(13)V99 COMP-3

        private TransactionType transactionType;

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

        public TransactionType getTransactionType() {
            return transactionType;
        }

        public void setTransactionType(TransactionType transactionType) {
            this.transactionType = transactionType;
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

    public static class Customer {
        @Size(max = 10)
        private String customerId; // PIC X(10)

        @Size(max = 50)
        private String customerName; // PIC X(50)

        private List<Account> accounts; // OCCURS n TIMES

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

        public List<Account> getAccounts() {
            return accounts;
        }

        public void setAccounts(List<Account> accounts) {
            this.accounts = accounts;
        }
    }
}