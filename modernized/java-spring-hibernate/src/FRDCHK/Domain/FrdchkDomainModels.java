package src.FRDCHK.Domain;

import java.math.BigDecimal;

/**
 * Domain model classes generated from COBOL data structures for the FRDCHK program.
 */
public class FrdchkDomainModels {

    /**
     * Represents the main record structure for FRDCHK.
     */
    public static class FrdchkRecord {
        // PIC X(10)
        private String recordId; // MaxLength(10)

        // PIC 9(5)
        private int transactionCode;

        // PIC S9(13)V99 COMP-3
        private BigDecimal transactionAmount; // Precision (15, 2)

        // PIC X(20)
        private String customerName; // MaxLength(20)

        // PIC 9(8)
        private long transactionDate;

        // PIC X(1)
        private String transactionStatus; // MaxLength(1)

        // 88-level for transactionStatus
        public enum TransactionStatus {
            APPROVED("A"),
            PENDING("P"),
            REJECTED("R");

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

        // Getters and Setters
        public String getRecordId() {
            return recordId;
        }

        public void setRecordId(String recordId) {
            this.recordId = recordId;
        }

        public int getTransactionCode() {
            return transactionCode;
        }

        public void setTransactionCode(int transactionCode) {
            this.transactionCode = transactionCode;
        }

        public BigDecimal getTransactionAmount() {
            return transactionAmount;
        }

        public void setTransactionAmount(BigDecimal transactionAmount) {
            this.transactionAmount = transactionAmount;
        }

        public String getCustomerName() {
            return customerName;
        }

        public void setCustomerName(String customerName) {
            this.customerName = customerName;
        }

        public long getTransactionDate() {
            return transactionDate;
        }

        public void setTransactionDate(long transactionDate) {
            this.transactionDate = transactionDate;
        }

        public String getTransactionStatus() {
            return transactionStatus;
        }

        public void setTransactionStatus(String transactionStatus) {
            this.transactionStatus = transactionStatus;
        }
    }
}