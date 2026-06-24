package TXNPOST.Domain;

import javax.validation.constraints.Size; // Corrected import statement for Javax validation API
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

public class TxnpostDomainModels {

    public static class TransactionRecord {
        @Size(max = 10)
        private String transactionId; // PIC X(10)

        @Size(max = 20)
        private String accountNumber; // PIC X(20)

        private BigDecimal transactionAmount; // PIC S9(13)V99 COMP-3

        @Size(max = 8)
        private String transactionDate; // PIC X(8)

        @Size(max = 1)
        private String transactionType; // PIC X(1)

        private TransactionStatus transactionStatus; // 88-level values

        public TransactionRecord() {
            this.transactionId = "";
            this.accountNumber = "";
            this.transactionAmount = BigDecimal.ZERO;
            this.transactionDate = "";
            this.transactionType = "";
            this.transactionStatus = TransactionStatus.UNKNOWN;
        }

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

        public TransactionStatus getTransactionStatus() {
            return transactionStatus;
        }

        public void setTransactionStatus(TransactionStatus transactionStatus) {
            this.transactionStatus = transactionStatus;
        }

        public enum TransactionStatus {
            APPROVED("A"), // 88 TRANSACTION-APPROVED VALUE 'A'
            REJECTED("R"), // 88 TRANSACTION-REJECTED VALUE 'R'
            PENDING("P"),  // 88 TRANSACTION-PENDING VALUE 'P'
            UNKNOWN("");   // Default value

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
                return UNKNOWN;
            }
        }
    }

    public static class BatchHeader {
        @Size(max = 6)
        private String batchId; // PIC X(6)

        @Size(max = 8)
        private String processingDate; // PIC X(8)

        private int recordCount; // PIC 9(5)

        public BatchHeader() {
            this.batchId = "";
            this.processingDate = "";
            this.recordCount = 0;
        }

        public String getBatchId() {
            return batchId;
        }

        public void setBatchId(String batchId) {
            this.batchId = batchId;
        }

        public String getProcessingDate() {
            return processingDate;
        }

        public void setProcessingDate(String processingDate) {
            this.processingDate = processingDate;
        }

        public int getRecordCount() {
            return recordCount;
        }

        public void setRecordCount(int recordCount) {
            this.recordCount = recordCount;
        }
    }

    public static class BatchTrailer {
        @Size(max = 6)
        private String batchId; // PIC X(6)

        private int totalRecords; // PIC 9(5)

        private BigDecimal totalAmount; // PIC S9(15)V99 COMP-3

        public BatchTrailer() {
            this.batchId = "";
            this.totalRecords = 0;
            this.totalAmount = BigDecimal.ZERO;
        }

        public String getBatchId() {
            return batchId;
        }

        public void setBatchId(String batchId) {
            this.batchId = batchId;
        }

        public int getTotalRecords() {
            return totalRecords;
        }

        public void setTotalRecords(int totalRecords) {
            this.totalRecords = totalRecords;
        }

        public BigDecimal getTotalAmount() {
            return totalAmount;
        }

        public void setTotalAmount(BigDecimal totalAmount) {
            this.totalAmount = totalAmount;
        }
    }

    public static class TransactionBatch {
        private BatchHeader batchHeader;
        private List<TransactionRecord> transactionRecords; // OCCURS n TIMES
        private BatchTrailer batchTrailer;

        public TransactionBatch() {
            this.batchHeader = new BatchHeader();
            this.transactionRecords = new ArrayList<>();
            this.batchTrailer = new BatchTrailer();
        }

        public BatchHeader getBatchHeader() {
            return batchHeader;
        }

        public void setBatchHeader(BatchHeader batchHeader) {
            this.batchHeader = batchHeader;
        }

        public List<TransactionRecord> getTransactionRecords() {
            return transactionRecords;
        }

        public void setTransactionRecords(List<TransactionRecord> transactionRecords) {
            this.transactionRecords = transactionRecords;
        }

        public BatchTrailer getBatchTrailer() {
            return batchTrailer;
        }

        public void setBatchTrailer(BatchTrailer batchTrailer) {
            this.batchTrailer = batchTrailer;
        }
    }
}