package src.FRDCHK.Tests;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.annotation.DirtiesContext;
import src.FRDCHK.Application.FrdchkService;
import src.FRDCHK.Infrastructure.TxnLedger;
import src.FRDCHK.Infrastructure.TxnLedgerRepository;
import src.FRDCHK.Infrastructure.FraudFlags;
import src.FRDCHK.Infrastructure.FraudFlagsRepository;

import java.math.BigDecimal;
import java.time.OffsetDateTime;
import java.time.ZoneOffset;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest
@DataJpaTest
@DirtiesContext(classMode = DirtiesContext.ClassMode.AFTER_EACH_TEST_METHOD)
public class FrdchkParityTests {

    @Autowired
    private TxnLedgerRepository txnLedgerRepository;

    @Autowired
    private FraudFlagsRepository fraudFlagsRepository;

    @Autowired
    private FrdchkService frdchkService;

    @Test
    public void testHappyPath() {
        // Seed data for TXN_LEDGER
        TxnLedger txnLedger = new TxnLedger();
        txnLedger.setTxnId("TXN1234567");
        txnLedger.setTxnAcctNumber("ACCT12345678901234");
        txnLedger.setAcct("ACCT12345678901234");
        txnLedger.setAmount(new BigDecimal("12345.67"));
        txnLedger.setCode("USD");
        txnLedger.setPostedAt(OffsetDateTime.of(2023, 1, 1, 12, 0, 0, 0, ZoneOffset.UTC));
        txnLedger.setTxnAmount(new BigDecimal("12345.67"));
        txnLedger.setTxnCode("D");
        txnLedger.setCount(1);
        txnLedger.setCoalesce("Test Coalesce");
        txnLedger.setWsVelocityCount(5);
        txnLedger.setWsVelocityAmt(new BigDecimal("500.00"));
        txnLedger.setWsCurrentAcct("ACCT12345678901234");
        txnLedgerRepository.save(txnLedger);

        // Seed data for FRAUD_FLAGS
        FraudFlags fraudFlags = new FraudFlags();
        fraudFlags.setAcct("ACCT12345678901234");
        fraudFlags.setScore(new BigDecimal("85.50"));
        fraudFlags.setReason("High Risk");
        fraudFlags.setTs(OffsetDateTime.of(2023, 1, 1, 12, 0, 0, 0, ZoneOffset.UTC));
        fraudFlags.setTxnAcctNumber("ACCT12345678901234");
        fraudFlags.setWsRiskScore(new BigDecimal("85.50"));
        fraudFlagsRepository.save(fraudFlags);

        // Execute the service
        frdchkService.execute();

        // Assertions
        assertThat(txnLedgerRepository.count()).isEqualTo(1);
        assertThat(fraudFlagsRepository.count()).isEqualTo(1);

        TxnLedger persistedTxnLedger = txnLedgerRepository.findById("TXN1234567").orElse(null);
        assertThat(persistedTxnLedger).isNotNull();
        assertThat(persistedTxnLedger.getTxnAcctNumber()).isEqualTo("ACCT12345678901234");
        assertThat(persistedTxnLedger.getAmount()).isEqualByComparingTo(new BigDecimal("12345.67"));
        assertThat(persistedTxnLedger.getCode()).isEqualTo("USD");

        FraudFlags persistedFraudFlags = fraudFlagsRepository.findById("ACCT12345678901234").orElse(null);
        assertThat(persistedFraudFlags).isNotNull();
        assertThat(persistedFraudFlags.getScore()).isEqualByComparingTo(new BigDecimal("85.50"));
        assertThat(persistedFraudFlags.getReason()).isEqualTo("High Risk");
    }

    @Test
    public void testEmptyInput() {
        // Execute the service with no seed data
        frdchkService.execute();

        // Assertions
        assertThat(txnLedgerRepository.count()).isEqualTo(0);
        assertThat(fraudFlagsRepository.count()).isEqualTo(0);
    }

    @Test
    public void testBoundaryValues() {
        // Seed data for TXN_LEDGER with boundary values
        TxnLedger txnLedger = new TxnLedger();
        txnLedger.setTxnId("TXN9999999");
        txnLedger.setTxnAcctNumber("ACCT99999999999999");
        txnLedger.setAcct("ACCT99999999999999");
        txnLedger.setAmount(new BigDecimal("9999999999999.99"));
        txnLedger.setCode("JPY");
        txnLedger.setPostedAt(OffsetDateTime.of(9999, 12, 31, 23, 59, 59, 0, ZoneOffset.UTC));
        txnLedger.setTxnAmount(new BigDecimal("9999999999999.99"));
        txnLedger.setTxnCode("C");
        txnLedger.setCount(99999);
        txnLedger.setCoalesce("Boundary Test");
        txnLedger.setWsVelocityCount(99999);
        txnLedger.setWsVelocityAmt(new BigDecimal("9999999999999.99"));
        txnLedger.setWsCurrentAcct("ACCT99999999999999");
        txnLedgerRepository.save(txnLedger);

        // Seed data for FRAUD_FLAGS with boundary values
        FraudFlags fraudFlags = new FraudFlags();
        fraudFlags.setAcct("ACCT99999999999999");
        fraudFlags.setScore(new BigDecimal("99.99"));
        fraudFlags.setReason("Boundary Risk");
        fraudFlags.setTs(OffsetDateTime.of(9999, 12, 31, 23, 59, 59, 0, ZoneOffset.UTC));
        fraudFlags.setTxnAcctNumber("ACCT99999999999999");
        fraudFlags.setWsRiskScore(new BigDecimal("99.99"));
        fraudFlagsRepository.save(fraudFlags);

        // Execute the service
        frdchkService.execute();

        // Assertions
        assertThat(txnLedgerRepository.count()).isEqualTo(1);
        assertThat(fraudFlagsRepository.count()).isEqualTo(1);

        TxnLedger persistedTxnLedger = txnLedgerRepository.findById("TXN9999999").orElse(null);
        assertThat(persistedTxnLedger).isNotNull();
        assertThat(persistedTxnLedger.getTxnAcctNumber()).isEqualTo("ACCT99999999999999");
        assertThat(persistedTxnLedger.getAmount()).isEqualByComparingTo(new BigDecimal("9999999999999.99"));
        assertThat(persistedTxnLedger.getCode()).isEqualTo("JPY");

        FraudFlags persistedFraudFlags = fraudFlagsRepository.findById("ACCT99999999999999").orElse(null);
        assertThat(persistedFraudFlags).isNotNull();
        assertThat(persistedFraudFlags.getScore()).isEqualByComparingTo(new BigDecimal("99.99"));
        assertThat(persistedFraudFlags.getReason()).isEqualTo("Boundary Risk");
    }
}