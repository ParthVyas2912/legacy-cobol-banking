package src.ATMTXN.Domain;

import javax.validation.constraints.Size; // Corrected import statement
import java.math.BigDecimal;
import java.util.List;

public class AtmtxnDomainModels {

    // Main transaction record
    public static class TransactionRecord {
        @Size(max = 12)
        private String transactionId; // PIC X(12)

        @Size(max = 10)
        private String accountNumber; // PIC X(10)

        private BigDecimal transactionAmount; // PIC S9(13)V99 COMP-3

        @Size(max = 1)
        private String transactionType; // PIC X(1)

        private TransactionStatus transactionStatus; // 88-level values for transaction status

        private List<TransactionDetail> transactionDetails; // OCCURS 10 TIMES

        // Getters and Setters
        public String getTransactionId() {
            return transactionId;
        }

        public void setTransactionId(String transactionId) {
            this.transactionId = transactionId;
        }

        public String getAccountNumber() {
            return accountNumber;
        }

        public void setAccountNumber(String accountNumber) {
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

        public TransactionStatus getTransactionStatus() {
            return transactionStatus;
        }

        public void setTransactionStatus(TransactionStatus transactionStatus) {
            this.transactionStatus = transactionStatus;
        }

        public List<TransactionDetail> getTransactionDetails() {
            return transactionDetails;
        }

        public void setTransactionDetails(List<TransactionDetail> transactionDetails) {
            this.transactionDetails = transactionDetails;
        }
    }

    // Nested class for transaction details
    public static class TransactionDetail {
        @Size(max = 6)
        private String detailCode; // PIC X(6)

        @Size(max = 20)
        private String description; // PIC X(20)

        private BigDecimal amount; // PIC S9(9)V99 COMP-3

        // Getters and Setters
        public String getDetailCode() {
            return detailCode;
        }

        public void setDetailCode(String detailCode) {
            this.detailCode = detailCode;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }

        public BigDecimal getAmount() {
            return amount;
        }

        public void setAmount(BigDecimal amount) {
            this.amount = amount;
        }
    }

    // Enum for 88-level values for transaction status
    public enum TransactionStatus {
        SUCCESS("S"),
        FAILURE("F"),
        PENDING("P");

        private final String value;

        TransactionStatus(String value) {
            this.value = value;
        }

        public String getValue() {
            return value;
        }

        public static TransactionStatus fromValue(String value) {
            for (TransactionStatus status : TransactionStatus.values()) {
                if (status.value.equals(value)) {
                    return status;
                }
            }
            throw new IllegalArgumentException("Unknown value: " + value);
        }
    }
}