package src.ATMTXN.Infrastructure;

import jakarta.persistence.*;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;
import src.ATMTXN.Domain.AtmtxnDomainModels.AtmTransaction;

import java.math.BigDecimal;
import java.util.Optional;

/**
 * Infrastructure layer for the ATMTXN program.
 */
public class AtmtxnInfrastructure {

    @Entity
    @Table(name = "atm_transaction")
    public static class AtmTransactionEntity {

        @Id
        @Column(name = "transaction_id", length = 16, nullable = false)
        // COBOL: transactionId PIC X(16)
        private String transactionId;

        @Column(name = "account_number", precision = 10, scale = 0, nullable = false)
        // COBOL: accountNumber PIC 9(10)
        private long accountNumber;

        @Column(name = "transaction_amount", precision = 15, scale = 2, nullable = false)
        // COBOL: transactionAmount PIC S9(13)V99 COMP-3
        private BigDecimal transactionAmount;

        @Column(name = "transaction_type", length = 10, nullable = false)
        // COBOL: transactionType PIC X(10)
        private String transactionType;

        @Column(name = "transaction_date", length = 8, nullable = false)
        // COBOL: transactionDate PIC X(8)
        private String transactionDate;

        @Column(name = "transaction_time", length = 6, nullable = false)
        // COBOL: transactionTime PIC X(6)
        private String transactionTime;

        @Column(name = "atm_location_code", length = 3, nullable = false)
        // COBOL: atmLocationCode PIC X(3)
        private String atmLocationCode;

        @Column(name = "transaction_status", length = 1, nullable = false)
        // COBOL: transactionStatus PIC X(1)
        private String transactionStatus;

        @Column(name = "customer_name", length = 20, nullable = false)
        // COBOL: customerName PIC X(20)
        private String customerName;

        @Column(name = "remarks", length = 30)
        // COBOL: remarks PIC X(30)
        private String remarks;

        // Getters and Setters
        public String getTransactionId() {
            return transactionId;
        }

        public void setTransactionId(String transactionId) {
            this.transactionId = transactionId;
        }

        public long getAccountNumber() {
            return accountNumber;
        }

        public void setAccountNumber(long accountNumber) {
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

        public String getAtmLocationCode() {
            return atmLocationCode;
        }

        public void setAtmLocationCode(String atmLocationCode) {
            this.atmLocationCode = atmLocationCode;
        }

        public String getTransactionStatus() {
            return transactionStatus;
        }

        public void setTransactionStatus(String transactionStatus) {
            this.transactionStatus = transactionStatus;
        }

        public String getCustomerName() {
            return customerName;
        }

        public void setCustomerName(String customerName) {
            this.customerName = customerName;
        }

        public String getRemarks() {
            return remarks;
        }

        public void setRemarks(String remarks) {
            this.remarks = remarks;
        }
    }

    @Repository
    public interface AtmTransactionRepository extends JpaRepository<AtmTransactionEntity, String> {
        Optional<AtmTransactionEntity> findByTransactionId(String transactionId);
    }

    @PersistenceContext
    private EntityManager entityManager;

    @Transactional
    public AtmTransactionEntity saveTransaction(AtmTransactionEntity transactionEntity) {
        return entityManager.merge(transactionEntity);
    }

    public Optional<AtmTransactionEntity> findTransactionById(String transactionId) {
        return Optional.ofNullable(entityManager.find(AtmTransactionEntity.class, transactionId));
    }
}