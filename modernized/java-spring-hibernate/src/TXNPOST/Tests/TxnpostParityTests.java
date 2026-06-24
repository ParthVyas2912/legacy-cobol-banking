package TXNPOST.Tests;

import TXNPOST.Application.TxnpostService;
import TXNPOST.Infrastructure.TxnpostInfrastructure.Txnin;
import TXNPOST.Infrastructure.TxnpostInfrastructure.TxnLedger;
import TXNPOST.Infrastructure.TxnpostInfrastructure.TxninRepository;
import TXNPOST.Infrastructure.TxnpostInfrastructure.TxnLedgerRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.annotation.DirtiesContext;

import java.math.BigDecimal;
import java.util.Optional;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest
@DataJpaTest
@DirtiesContext(classMode = DirtiesContext.ClassMode.BEFORE_EACH_TEST_METHOD)
public class TxnpostParityTests {

    @Autowired
    private TxninRepository txninRepository;

    @Autowired
    private TxnLedgerRepository txnLedgerRepository;

    private TxnpostService txnpostService;

    @BeforeEach
    public void setUp() {
        txnpostService = new TxnpostService();
    }

    @Test
    public void testHappyPath() {
        // Seed data
        Txnin txnin = new Txnin();
        txnin.setTxnId("123456789012");
        txnin.setTxnAcctNumber("1234567890");
        txnin.setTxnPostDate("20231010");
        txnin.setTxnEffectiveDate("20231011");
        txnin.setTxnAmount(new BigDecimal("12345.67"));
        txnin.setTxnCode("ABC");
        txnin.setTxnChannel("WEB");
        txnin.setTxnRefNumber("REF123456789012345678");
        txnin.setTxnDescription("Test Transaction");
        txnin.setTxnStatus("P");
        txnin.setTxnBalanceAfter(new BigDecimal("98765.43"));
        txnin.setTxnUserId("USER1234");
        txninRepository.save(txnin);

        // Execute the service
        txnpostService.execute();

        // Assertions
        Optional<Txnin> retrievedTxnin = txninRepository.findByTxnId("123456789012");
        assertThat(retrievedTxnin).isPresent();
        assertThat(retrievedTxnin.get().getTxnStatus()).isEqualTo("P");

        Optional<TxnLedger> retrievedTxnLedger = txnLedgerRepository.findByTxnId("123456789012");
        assertThat(retrievedTxnLedger).isPresent();
        assertThat(retrievedTxnLedger.get().getTxnId()).isEqualTo("123456789012");
        assertThat(retrievedTxnLedger.get().getTxnAcctNumber()).isEqualTo("1234567890");
        assertThat(retrievedTxnLedger.get().getTxnAmount()).isEqualTo(new BigDecimal("12345.67"));
    }

    @Test
    public void testEmptyInput() {
        // Execute the service with no seed data
        txnpostService.execute();

        // Assertions
        assertThat(txninRepository.findAll()).isEmpty();
        assertThat(txnLedgerRepository.findAll()).isEmpty();
    }

    @Test
    public void testBoundaryValues() {
        // Seed data with boundary values
        Txnin txnin = new Txnin();
        txnin.setTxnId("999999999999");
        txnin.setTxnAcctNumber("9999999999");
        txnin.setTxnPostDate("99999999");
        txnin.setTxnEffectiveDate("99999999");
        txnin.setTxnAmount(new BigDecimal("99999999999.99"));
        txnin.setTxnCode("ZZZ");
        txnin.setTxnChannel("ZZZZ");
        txnin.setTxnRefNumber("ZZZZZZZZZZZZZZZZZZZZ");
        txnin.setTxnDescription("Boundary Test Transaction");
        txnin.setTxnStatus("P");
        txnin.setTxnBalanceAfter(new BigDecimal("999999999999.99"));
        txnin.setTxnUserId("ZZZZZZZZ");
        txninRepository.save(txnin);

        // Execute the service
        txnpostService.execute();

        // Assertions
        Optional<Txnin> retrievedTxnin = txninRepository.findByTxnId("999999999999");
        assertThat(retrievedTxnin).isPresent();
        assertThat(retrievedTxnin.get().getTxnStatus()).isEqualTo("P");

        Optional<TxnLedger> retrievedTxnLedger = txnLedgerRepository.findByTxnId("999999999999");
        assertThat(retrievedTxnLedger).isPresent();
        assertThat(retrievedTxnLedger.get().getTxnId()).isEqualTo("999999999999");
        assertThat(retrievedTxnLedger.get().getTxnAcctNumber()).isEqualTo("9999999999");
        assertThat(retrievedTxnLedger.get().getTxnAmount()).isEqualTo(new BigDecimal("99999999999.99"));
    }
}