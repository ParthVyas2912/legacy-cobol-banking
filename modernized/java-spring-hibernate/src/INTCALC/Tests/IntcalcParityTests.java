package src.INTCALC.Tests;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.jdbc.Sql;
import src.INTCALC.Application.IntcalcService;
import src.INTCALC.Infrastructure.Acctfile;
import src.INTCALC.Infrastructure.AcctfileRepository;
import src.INTCALC.Infrastructure.Intjrnl;
import src.INTCALC.Infrastructure.IntjrnlRepository;

import java.math.BigDecimal;
import java.util.Optional;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest
@DataJpaTest
public class IntcalcParityTests {

    @Autowired
    private IntcalcService intcalcService;

    @Autowired
    private AcctfileRepository acctfileRepository;

    @Autowired
    private IntjrnlRepository intjrnlRepository;

    @Test
    @Sql(scripts = "/testdata/intcalc_happy_path.sql")
    void testHappyPath() {
        // Execute the program
        intcalcService.execute();

        // Assert database state after execution
        Optional<Acctfile> account = acctfileRepository.findByAcctNumber("1234567890");
        assertThat(account).isPresent();
        assertThat(account.get().getAcctBalance()).isEqualByComparingTo(new BigDecimal("10500.00"));
        assertThat(account.get().getAcctStatus()).isEqualTo("A");

        Optional<Intjrnl> transaction = intjrnlRepository.findByTxnId("000000000001");
        assertThat(transaction).isPresent();
        assertThat(transaction.get().getTxnAmount()).isEqualByComparingTo(new BigDecimal("500.00"));
        assertThat(transaction.get().getTxnStatus()).isEqualTo("C");
    }

    @Test
    void testEmptyInput() {
        // Execute the program with no data
        intcalcService.execute();

        // Assert database state is empty
        assertThat(acctfileRepository.count()).isEqualTo(0);
        assertThat(intjrnlRepository.count()).isEqualTo(0);
    }

    @Test
    @Sql(scripts = "/testdata/intcalc_boundary_values.sql")
    void testBoundaryValues() {
        // Execute the program
        intcalcService.execute();

        // Assert database state after execution
        Optional<Acctfile> account = acctfileRepository.findByAcctNumber("9999999999");
        assertThat(account).isPresent();
        assertThat(account.get().getAcctBalance()).isEqualByComparingTo(new BigDecimal("9999999999999.99"));
        assertThat(account.get().getAcctStatus()).isEqualTo("A");

        Optional<Intjrnl> transaction = intjrnlRepository.findByTxnId("999999999999");
        assertThat(transaction).isPresent();
        assertThat(transaction.get().getTxnAmount()).isEqualByComparingTo(new BigDecimal("999999999999.99"));
        assertThat(transaction.get().getTxnStatus()).isEqualTo("C");
    }
}