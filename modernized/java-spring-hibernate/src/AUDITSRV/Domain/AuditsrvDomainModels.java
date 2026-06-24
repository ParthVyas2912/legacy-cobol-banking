package src.AUDITSRV.Domain;

import javax.validation.constraints.Size;
import javax.persistence.Column;
import java.math.BigDecimal;
import java.util.List;

public class AuditsrvDomainModels {

    public static class AuditRecord {
        @Size(max = 10)
        @Column(length = 10)
        private String recordId; // PIC X(10)

        @Size(max = 20)
        @Column(length = 20)
        private String userId; // PIC X(20)

        @Size(max = 8)
        @Column(length = 8)
        private String transactionDate; // PIC X(8)

        @Size(max = 6)
        @Column(length = 6)
        private String transactionTime; // PIC X(6)

        @Column(precision = 15, scale = 2)
        private BigDecimal transactionAmount; // PIC S9(13)V99 COMP-3

        @Size(max = 50)
        @Column(length = 50)
        private String description; // PIC X(50)

        private TransactionStatus transactionStatus; // 88-level values

        // Getters and Setters
        public String getRecordId() {
            return recordId;
        }

        public void setRecordId(String recordId) {
            this.recordId = recordId;
        }

        public String getUserId() {
            return userId;
        }

        public void setUserId(String userId) {
            this.userId = userId;
        }

        public String getTransactionDate() {
            return transactionDate;
        }

        public void setTransactionDate(String transactionDate) {
            this.transactionDate = transactionDate;
        }

        public String getTransactionTime() {
            return transactionTime;
        }

        public void setTransactionTime(String transactionTime) {
            this.transactionTime = transactionTime;
        }

        public BigDecimal getTransactionAmount() {
            return transactionAmount;
        }

        public void setTransactionAmount(BigDecimal transactionAmount) {
            this.transactionAmount = transactionAmount;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }

        public TransactionStatus getTransactionStatus() {
            return transactionStatus;
        }

        public void setTransactionStatus(TransactionStatus transactionStatus) {
            this.transactionStatus = transactionStatus;
        }
    }

    public enum TransactionStatus {
        SUCCESS("S"), // 88 TRANSACTION-SUCCESS VALUE 'S'
        FAILURE("F"); // 88 TRANSACTION-FAILURE VALUE 'F'

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

    public static class AuditHeader {
        @Size(max = 5)
        @Column(length = 5)
        private String headerId; // PIC X(5)

        @Size(max = 10)
        @Column(length = 10)
        private String batchId; // PIC X(10)

        private List<AuditRecord> auditRecords; // OCCURS n TIMES

        // Getters and Setters
        public String getHeaderId() {
            return headerId;
        }

        public void setHeaderId(String headerId) {
            this.headerId = headerId;
        }

        public String getBatchId() {
            return batchId;
        }

        public void setBatchId(String batchId) {
            this.batchId = batchId;
        }

        public List<AuditRecord> getAuditRecords() {
            return auditRecords;
        }

        public void setAuditRecords(List<AuditRecord> auditRecords) {
            this.auditRecords = auditRecords;
        }
    }
}