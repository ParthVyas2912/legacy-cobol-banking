package src.ATMTXN.Tests;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.annotation.DirtiesContext;
import src.ATMTXN.Application.Interfaces.AtmtxnInterfaces;
import src.ATMTXN.Domain.AtmtxnDomainModels.TransactionDetail;
import src.ATMTXN.Domain.AtmtxnDomainModels.TransactionRecord;
import src.ATMTXN.Domain.AtmtxnDomainModels.TransactionStatus;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest
@DirtiesContext(classMode = DirtiesContext.ClassMode.BEFORE_EACH_TEST_METHOD)
public class AtmtxnParityTests {

    @Autowired
    private AtmtxnInterfaces atmtxnService;

    @BeforeEach
    public void setup() {
        atmtxnService.initializeTransactionEnvironment();
    }

    @Test
    public void testHappyPathTransactionProcessing() {
        // Seed data
        TransactionRecord transactionRecord = new TransactionRecord();
        transactionRecord.setTransactionId("TRX123456789");
        transactionRecord.setAccountNumber("1234567890");
        transactionRecord.setTransactionAmount(new BigDecimal("1234567890123.45"));
        transactionRecord.setTransactionType("D");
        transactionRecord.setTransactionStatus(TransactionStatus.PENDING);

        List<TransactionDetail> details = new ArrayList<>();
        for (int i = 1; i <= 10; i++) {
            TransactionDetail detail = new TransactionDetail();
            detail.setDetailCode(String.format("DC%04d", i));
            detail.setDescription("Detail " + i);
            detail.setAmount(new BigDecimal("12345.67"));
            details.add(detail);
        }
        transactionRecord.setTransactionDetails(details);

        // Write transaction record
        atmtxnService.writeTransactionRecord(transactionRecord);

        // Process transaction
        TransactionStatus status = atmtxnService.processTransaction(transactionRecord);

        // Assert transaction status
        assertThat(status).isEqualTo(TransactionStatus.SUCCESS);

        // Assert database state
        Optional<TransactionRecord> savedRecord = atmtxnService.readTransactionRecord("TRX123456789");
        assertThat(savedRecord).isPresent();
        assertThat(savedRecord.get().getTransactionId()).isEqualTo("TRX123456789");
        assertThat(savedRecord.get().getAccountNumber()).isEqualTo("1234567890");
        assertThat(savedRecord.get().getTransactionAmount()).isEqualTo(new BigDecimal("1234567890123.45"));
        assertThat(savedRecord.get().getTransactionType()).isEqualTo("D");
        assertThat(savedRecord.get().getTransactionStatus()).isEqualTo(TransactionStatus.SUCCESS);
        assertThat(savedRecord.get().getTransactionDetails()).hasSize(10);
    }

    @Test
    public void testEmptyInputTransactionProcessing() {
        // Execute with no data
        List<TransactionRecord> records = atmtxnService.getAllTransactionRecords();

        // Assert no records exist
        assertThat(records).isEmpty();
    }

    @Test
    public void testBoundaryValueTransactionProcessing() {
        // Seed data with boundary values
        TransactionRecord transactionRecord = new TransactionRecord();
        transactionRecord.setTransactionId("TRX999999999");
        transactionRecord.setAccountNumber("9999999999");
        transactionRecord.setTransactionAmount(new BigDecimal("9999999999999.99"));
        transactionRecord.setTransactionType("W");
        transactionRecord.setTransactionStatus(TransactionStatus.PENDING);

        List<TransactionDetail> details = new ArrayList<>();
        for (int i = 1; i <= 10; i++) {
            TransactionDetail detail = new TransactionDetail();
            detail.setDetailCode(String.format("DC%04d", i));
            detail.setDescription("Detail " + i);
            detail.setAmount(new BigDecimal("99999999.99"));
            details.add(detail);
        }
        transactionRecord.setTransactionDetails(details);

        // Write transaction record
        atmtxnService.writeTransactionRecord(transactionRecord);

        // Process transaction
        TransactionStatus status = atmtxnService.processTransaction(transactionRecord);

        // Assert transaction status
        assertThat(status).isEqualTo(TransactionStatus.SUCCESS);

        // Assert database state
        Optional<TransactionRecord> savedRecord = atmtxnService.readTransactionRecord("TRX999999999");
        assertThat(savedRecord).isPresent();
        assertThat(savedRecord.get().getTransactionId()).isEqualTo("TRX999999999");
        assertThat(savedRecord.get().getAccountNumber()).isEqualTo("9999999999");
        assertThat(savedRecord.get().getTransactionAmount()).isEqualTo(new BigDecimal("9999999999999.99"));
        assertThat(savedRecord.get().getTransactionType()).isEqualTo("W");
        assertThat(savedRecord.get().getTransactionStatus()).isEqualTo(TransactionStatus.SUCCESS);
        assertThat(savedRecord.get().getTransactionDetails()).hasSize(10);

        // Assert transaction details
        List<TransactionDetail> savedDetails = savedRecord.get().getTransactionDetails();
        for (int i = 0; i < savedDetails.size(); i++) {
            TransactionDetail detail = savedDetails.get(i);
            assertThat(detail.getDetailCode()).isEqualTo(String.format("DC%04d", i + 1));
            assertThat(detail.getDescription()).isEqualTo("Detail " + (i + 1));
            assertThat(detail.getAmount()).isEqualTo(new BigDecimal("99999999.99"));
        }
    }
}