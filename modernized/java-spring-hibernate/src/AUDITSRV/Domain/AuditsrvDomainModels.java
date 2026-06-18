package src.AUDITSRV.Domain;

import java.math.BigDecimal;

/**
 * Domain model classes generated from COBOL data structures for AUDITSRV.
 */
public class AuditsrvDomainModels {

    /**
     * Represents the primary audit record structure.
     */
    public static class AuditRecord {
        // PIC X(10)
        private String recordId; // MaxLength(10)

        // PIC 9(8)
        private int transactionDate;

        // PIC 9(6)
        private int transactionTime;

        // PIC S9(13)V99 COMP-3
        private BigDecimal transactionAmount; // Precision (15, 2)

        // PIC X(20)
        private String transactionDescription; // MaxLength(20)

        // PIC X(1)
        private String transactionType; // MaxLength(1)

        // PIC X(30)
        private String userId; // MaxLength(30)

        // PIC X(50)
        private String comments; // MaxLength(50)

        // Getters and Setters
        public String getRecordId() {
            return recordId;
        }

        public void setRecordId(String recordId) {
            this.recordId = recordId;
        }

        public int getTransactionDate() {
            return transactionDate;
        }

        public void setTransactionDate(int transactionDate) {
            this.transactionDate = transactionDate;
        }

        public int getTransactionTime() {
            return transactionTime;
        }

        public void setTransactionTime(int transactionTime) {
            this.transactionTime = transactionTime;
        }

        public BigDecimal getTransactionAmount() {
            return transactionAmount;
        }

        public void setTransactionAmount(BigDecimal transactionAmount) {
            this.transactionAmount = transactionAmount;
        }

        public String getTransactionDescription() {
            return transactionDescription;
        }

        public void setTransactionDescription(String transactionDescription) {
            this.transactionDescription = transactionDescription;
        }

        public String getTransactionType() {
            return transactionType;
        }

        public void setTransactionType(String transactionType) {
            this.transactionType = transactionType;
        }

        public String getUserId() {
            return userId;
        }

        public void setUserId(String userId) {
            this.userId = userId;
        }

        public String getComments() {
            return comments;
        }

        public void setComments(String comments) {
            this.comments = comments;
        }
    }

    /**
     * Represents the transaction type as an enumeration.
     */
    public enum TransactionType {
        // 88-level values
        DEPOSIT("D"),
        WITHDRAWAL("W"),
        TRANSFER("T"),
        PAYMENT("P");

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
            throw new IllegalArgumentException("Unknown TransactionType value: " + value);
        }
    }
}