package src.FRDCHK.Domain;

import javax.validation.constraints.Size; // Corrected import statement
import java.math.BigDecimal;
import java.util.List;

public class FrdchkDomainModels {

    public static class FraudCheckRequest {
        @Size(max = 10)
        private String transactionId; // PIC X(10)

        @Size(max = 20)
        private String accountNumber; // PIC X(20)

        private BigDecimal transactionAmount; // PIC 9(13)V99 COMP-3

        @Size(max = 8)
        private String transactionDate; // PIC X(8)

        @Size(max = 3)
        private String currencyCode; // PIC X(3)

        private TransactionType transactionType; // 88-level values for transaction type

        private List<FraudCheckDetail> fraudCheckDetails; // OCCURS 10 TIMES

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

        public String getTransactionDate() {
            return transactionDate;
        }

        public void setTransactionDate(String transactionDate) {
            this.transactionDate = transactionDate;
        }

        public String getCurrencyCode() {
            return currencyCode;
        }

        public void setCurrencyCode(String currencyCode) {
            this.currencyCode = currencyCode;
        }

        public TransactionType getTransactionType() {
            return transactionType;
        }

        public void setTransactionType(TransactionType transactionType) {
            this.transactionType = transactionType;
        }

        public List<FraudCheckDetail> getFraudCheckDetails() {
            return fraudCheckDetails;
        }

        public void setFraudCheckDetails(List<FraudCheckDetail> fraudCheckDetails) {
            this.fraudCheckDetails = fraudCheckDetails;
        }
    }

    public static class FraudCheckDetail {
        @Size(max = 15)
        private String ruleId; // PIC X(15)

        @Size(max = 50)
        private String ruleDescription; // PIC X(50)

        private BigDecimal ruleScore; // PIC 9(5)V99 COMP-3

        // Getters and Setters
        public String getRuleId() {
            return ruleId;
        }

        public void setRuleId(String ruleId) {
            this.ruleId = ruleId;
        }

        public String getRuleDescription() {
            return ruleDescription;
        }

        public void setRuleDescription(String ruleDescription) {
            this.ruleDescription = ruleDescription;
        }

        public BigDecimal getRuleScore() {
            return ruleScore;
        }

        public void setRuleScore(BigDecimal ruleScore) {
            this.ruleScore = ruleScore;
        }
    }

    public enum TransactionType {
        DEBIT("D"), // 88 DEBIT VALUE 'D'
        CREDIT("C"); // 88 CREDIT VALUE 'C'

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