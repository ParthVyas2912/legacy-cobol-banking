package src.ATMTXN.Infrastructure;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import org.springframework.stereotype.Service;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.transaction.annotation.Transactional;
import src.ATMTXN.Domain.AtmtxnDomainModels.TransactionRecord;
import src.ATMTXN.Domain.AtmtxnDomainModels.TransactionDetail;
import src.ATMTXN.Domain.AtmtxnDomainModels.TransactionStatus;
import src.ATMTXN.Application.Interfaces.AtmtxnInterfaces;

import jakarta.persistence.Entity;
import jakarta.persistence.Table;
import jakarta.persistence.Id;
import jakarta.persistence.Column;
import jakarta.persistence.OneToMany;
import jakarta.persistence.CascadeType;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import java.math.BigDecimal;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

@Entity
@Table(name = "transaction_record")
class TransactionRecordEntity {

    @Id
    @Column(name = "transaction_id", length = 12, nullable = false, unique = true)
    // COBOL: TRANSACTION-ID PIC X(12)
    private String transactionId;

    @Column(name = "account_number", length = 10, nullable = false)
    // COBOL: ACCOUNT-NUMBER PIC X(10)
    private String accountNumber;

    @Column(name = "transaction_amount", precision = 15, scale = 2, nullable = false)
    // COBOL: TRANSACTION-AMOUNT PIC S9(13)V99 COMP-3
    private BigDecimal transactionAmount;

    @Column(name = "transaction_type", length = 1, nullable = false)
    // COBOL: TRANSACTION-TYPE PIC X(1)
    private String transactionType;

    @Column(name = "transaction_status", length = 1, nullable = false)
    // COBOL: TRANSACTION-STATUS PIC X(1)
    private String transactionStatus;

    @OneToMany(mappedBy = "transactionRecord", cascade = CascadeType.ALL, orphanRemoval = true)
    private List<TransactionDetailEntity> transactionDetails;

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

    public String getTransactionStatus() {
        return transactionStatus;
    }

    public void setTransactionStatus(String transactionStatus) {
        this.transactionStatus = transactionStatus;
    }

    public List<TransactionDetailEntity> getTransactionDetails() {
        return transactionDetails;
    }

    public void setTransactionDetails(List<TransactionDetailEntity> transactionDetails) {
        this.transactionDetails = transactionDetails;
    }
}

@Entity
@Table(name = "transaction_detail")
class TransactionDetailEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "transaction_id", nullable = false)
    private TransactionRecordEntity transactionRecord;

    @Column(name = "detail_code", length = 6, nullable = false)
    // COBOL: DETAIL-CODE PIC X(6)
    private String detailCode;

    @Column(name = "description", length = 20, nullable = false)
    // COBOL: DESCRIPTION PIC X(20)
    private String description;

    @Column(name = "amount", precision = 11, scale = 2, nullable = false)
    // COBOL: AMOUNT PIC S9(9)V99 COMP-3
    private BigDecimal amount;

    // Getters and Setters
    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public TransactionRecordEntity getTransactionRecord() {
        return transactionRecord;
    }

    public void setTransactionRecord(TransactionRecordEntity transactionRecord) {
        this.transactionRecord = transactionRecord;
    }

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

@Repository
interface TransactionRecordRepository extends JpaRepository<TransactionRecordEntity, String> {
    List<TransactionRecordEntity> findAll();
}

@Repository
interface TransactionDetailRepository extends JpaRepository<TransactionDetailEntity, Long> {
    List<TransactionDetailEntity> findByTransactionRecord_TransactionId(String transactionId);
}

@Service
@Transactional
public class AtmtxnInfrastructure implements AtmtxnInterfaces {

    private final TransactionRecordRepository transactionRecordRepository;
    private final TransactionDetailRepository transactionDetailRepository;

    @Autowired
    public AtmtxnInfrastructure(TransactionRecordRepository transactionRecordRepository,
                                 TransactionDetailRepository transactionDetailRepository) {
        this.transactionRecordRepository = transactionRecordRepository;
        this.transactionDetailRepository = transactionDetailRepository;
    }

    @Override
    public void initializeTransactionEnvironment() {
        // Initialization logic if required
    }

    @Override
    public Optional<TransactionRecord> readTransactionRecord(String transactionId) {
        return transactionRecordRepository.findById(transactionId)
                .map(this::mapToDomain);
    }

    @Override
    public void writeTransactionRecord(TransactionRecord transactionRecord) {
        TransactionRecordEntity entity = mapToEntity(transactionRecord);
        transactionRecordRepository.save(entity);
    }

    @Override
    public void updateTransactionRecord(TransactionRecord transactionRecord) {
        TransactionRecordEntity entity = mapToEntity(transactionRecord);
        transactionRecordRepository.save(entity);
    }

    @Override
    public void deleteTransactionRecord(String transactionId) {
        transactionRecordRepository.deleteById(transactionId);
    }

    @Override
    public List<TransactionRecord> getAllTransactionRecords() {
        return transactionRecordRepository.findAll().stream()
                .map(this::mapToDomain)
                .collect(Collectors.toList());
    }

    @Override
    public TransactionStatus processTransaction(TransactionRecord transactionRecord) {
        // Business logic for processing a transaction
        // Placeholder for actual implementation
        return TransactionStatus.SUCCESS;
    }

    @Override
    public List<TransactionDetail> getTransactionDetails(String transactionId) {
        return transactionDetailRepository.findByTransactionRecord_TransactionId(transactionId).stream()
                .map(this::mapToDomain)
                .collect(Collectors.toList());
    }

    @Override
    public boolean validateTransaction(TransactionRecord transactionRecord) {
        // Validation logic for transaction
        // Placeholder for actual implementation
        return true;
    }

    @Override
    public void finalizeTransactionEnvironment() {
        // Finalization logic if required
    }

    private TransactionRecord mapToDomain(TransactionRecordEntity entity) {
        TransactionRecord domain = new TransactionRecord();
        domain.setTransactionId(entity.getTransactionId());
        domain.setAccountNumber(entity.getAccountNumber());
        domain.setTransactionAmount(entity.getTransactionAmount());
        domain.setTransactionType(entity.getTransactionType());
        domain.setTransactionStatus(TransactionStatus.fromValue(entity.getTransactionStatus()));
        domain.setTransactionDetails(entity.getTransactionDetails().stream()
                .map(this::mapToDomain)
                .collect(Collectors.toList()));
        return domain;
    }

    private TransactionDetail mapToDomain(TransactionDetailEntity entity) {
        TransactionDetail detail = new TransactionDetail();
        detail.setDetailCode(entity.getDetailCode());
        detail.setDescription(entity.getDescription());
        detail.setAmount(entity.getAmount());
        return detail;
    }

    private TransactionRecordEntity mapToEntity(TransactionRecord domain) {
        TransactionRecordEntity entity = new TransactionRecordEntity();
        entity.setTransactionId(domain.getTransactionId());
        entity.setAccountNumber(domain.getAccountNumber());
        entity.setTransactionAmount(domain.getTransactionAmount());
        entity.setTransactionType(domain.getTransactionType());
        entity.setTransactionStatus(domain.getTransactionStatus().getValue());
        entity.setTransactionDetails(domain.getTransactionDetails().stream()
                .map(detail -> {
                    TransactionDetailEntity detailEntity = new TransactionDetailEntity();
                    detailEntity.setDetailCode(detail.getDetailCode());
                    detailEntity.setDescription(detail.getDescription());
                    detailEntity.setAmount(detail.getAmount());
                    detailEntity.setTransactionRecord(entity);
                    return detailEntity;
                })
                .collect(Collectors.toList()));
        return entity;
    }
}