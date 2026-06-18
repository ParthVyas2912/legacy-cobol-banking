package src.STMTRPT.Infrastructure;

import jakarta.persistence.*;
import java.math.BigDecimal;

/**
 * Infrastructure for STMTRPT program.
 * Includes ORM mappings and repository interfaces.
 */
public class StmtrptInfrastructure {

    /**
     * Entity class for the `stmtout` table.
     */
    @Entity
    @Table(name = "stmtout", schema = "public")
    public static class Stmtout {

        @Id
        @GeneratedValue(strategy = GenerationType.IDENTITY)
        @Column(name = "id")
        private Long id;

        @Column(name = "stmt_acct_num", length = 10, nullable = false)
        // COBOL: STMT-ACCT-NUM PIC 9(10)
        private String stmtAcctNum;

        @Column(name = "stmt_description", length = 40)
        // COBOL: STMT-DESCRIPTION PIC X(40)
        private String stmtDescription;

        @Column(name = "stmt_amount", precision = 14, scale = 2)
        // COBOL: STMT-AMOUNT PIC -Z(11)9.99
        private BigDecimal stmtAmount;

        @Column(name = "stmt_balance", precision = 16, scale = 2)
        // COBOL: STMT-BALANCE PIC -Z(13)9.99
        private BigDecimal stmtBalance;

        // Getters and setters
        public Long getId() {
            return id;
        }

        public void setId(Long id) {
            this.id = id;
        }

        public String getStmtAcctNum() {
            return stmtAcctNum;
        }

        public void setStmtAcctNum(String stmtAcctNum) {
            this.stmtAcctNum = stmtAcctNum;
        }

        public String getStmtDescription() {
            return stmtDescription;
        }

        public void setStmtDescription(String stmtDescription) {
            this.stmtDescription = stmtDescription;
        }

        public BigDecimal getStmtAmount() {
            return stmtAmount;
        }

        public void setStmtAmount(BigDecimal stmtAmount) {
            this.stmtAmount = stmtAmount;
        }

        public BigDecimal getStmtBalance() {
            return stmtBalance;
        }

        public void setStmtBalance(BigDecimal stmtBalance) {
            this.stmtBalance = stmtBalance;
        }
    }

    /**
     * Repository interface for the `stmtout` table.
     */
    public interface StmtoutRepository extends JpaRepository<Stmtout, Long> {
        // Custom query methods can be added here if needed
    }
}