package src.STMTRPT.Tests;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.ContextConfiguration;
import src.STMTRPT.Application.StmtrptService;
import src.STMTRPT.Infrastructure.StmtrptInfrastructure.Stmtout;
import src.STMTRPT.Infrastructure.StmtrptInfrastructure.StmtoutRepository;

import java.math.BigDecimal;
import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest
@ContextConfiguration(classes = {StmtrptService.class, StmtoutRepository.class})
@DataJpaTest
public class StmtrptParityTests {

    @Autowired
    private StmtoutRepository stmtoutRepository;

    @Autowired
    private StmtrptService stmtrptService;

    @Test
    public void testHappyPath() {
        // Seed data
        Stmtout stmtout = new Stmtout();
        stmtout.setStmtAcctNum("1234567890");
        stmtout.setStmtDescription("Test Description");
        stmtout.setStmtAmount(new BigDecimal("12345.67"));
        stmtout.setStmtBalance(new BigDecimal("98765.43"));
        stmtoutRepository.save(stmtout);

        // Execute the service
        stmtrptService.execute();

        // Assertions
        List<Stmtout> stmtoutList = stmtoutRepository.findAll();
        assertThat(stmtoutList).hasSize(1);

        Stmtout result = stmtoutList.get(0);
        assertThat(result.getStmtAcctNum()).isEqualTo("1234567890");
        assertThat(result.getStmtDescription()).isEqualTo("Test Description");
        assertThat(result.getStmtAmount()).isEqualTo(new BigDecimal("12345.67"));
        assertThat(result.getStmtBalance()).isEqualTo(new BigDecimal("98765.43"));
    }

    @Test
    public void testEmptyInput() {
        // No seed data

        // Execute the service
        stmtrptService.execute();

        // Assertions
        List<Stmtout> stmtoutList = stmtoutRepository.findAll();
        assertThat(stmtoutList).isEmpty();
    }

    @Test
    public void testBoundaryValues() {
        // Seed data with boundary values
        Stmtout stmtout = new Stmtout();
        stmtout.setStmtAcctNum("9999999999"); // Max PIC 9(10)
        stmtout.setStmtDescription("Boundary Test Description");
        stmtout.setStmtAmount(new BigDecimal("99999999999.99")); // Max PIC -Z(11)9.99
        stmtout.setStmtBalance(new BigDecimal("9999999999999.99")); // Max PIC -Z(13)9.99
        stmtoutRepository.save(stmtout);

        // Execute the service
        stmtrptService.execute();

        // Assertions
        List<Stmtout> stmtoutList = stmtoutRepository.findAll();
        assertThat(stmtoutList).hasSize(1);

        Stmtout result = stmtoutList.get(0);
        assertThat(result.getStmtAcctNum()).isEqualTo("9999999999");
        assertThat(result.getStmtDescription()).isEqualTo("Boundary Test Description");
        assertThat(result.getStmtAmount()).isEqualTo(new BigDecimal("99999999999.99"));
        assertThat(result.getStmtBalance()).isEqualTo(new BigDecimal("9999999999999.99"));
    }
}