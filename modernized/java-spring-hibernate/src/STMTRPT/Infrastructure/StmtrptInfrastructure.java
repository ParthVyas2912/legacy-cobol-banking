package src.STMTRPT.Infrastructure;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Bean;
import jakarta.persistence.Entity;
import jakarta.persistence.Table;
import jakarta.persistence.Id;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Column;
import jakarta.persistence.EntityManager;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.transaction.annotation.Transactional;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

/**
 * Infrastructure for STMTRPT program.
 */
@Configuration
public class StmtrptInfrastructure {

    @Bean
    public StmtoutRepository stmtoutRepository(EntityManager entityManager) {
        return new StmtoutRepositoryImpl(entityManager);
    }

    @Entity
    @Table(name = "stmtout", schema = "public")
    public static class Stmtout {

        @Id
        @GeneratedValue(strategy = GenerationType.IDENTITY)
        @Column(name = "id", nullable = false)
        private Long id;

        @Column(name = "stmt_acct_num", length = 10, nullable = false)
        // COBOL: STMT-ACCT-NUM PIC 9(10)
        private String stmtAcctNum;

        @Column(name = "stmt_description", length = 40)
        // COBOL: STMT-DESCRIPTION PIC X(40)
        private String stmtDescription;

        @Column(name = "stmt_amount", precision = 13, scale = 2)
        // COBOL: STMT-AMOUNT PIC -Z(11)9.99
        private BigDecimal stmtAmount;

        @Column(name = "stmt_balance", precision = 15, scale = 2)
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

    @Repository
    public interface StmtoutRepository extends JpaRepository<Stmtout, Long> {

        @Query("SELECT s FROM Stmtout s WHERE s.stmtAcctNum = :accountNumber")
        Stmtout findByAccountNumber(String accountNumber);
    }

    public static class StmtoutRepositoryImpl implements StmtoutRepository {

        private final EntityManager entityManager;

        @Autowired
        public StmtoutRepositoryImpl(EntityManager entityManager) {
            this.entityManager = entityManager;
        }

        @Override
        public Stmtout findByAccountNumber(String accountNumber) {
            return entityManager.createQuery("SELECT s FROM Stmtout s WHERE s.stmtAcctNum = :accountNumber", Stmtout.class)
                    .setParameter("accountNumber", accountNumber)
                    .getSingleResult();
        }

        @Override
        public <S extends Stmtout> S save(S entity) {
            entityManager.persist(entity);
            return entity;
        }

        @Override
        public <S extends Stmtout> S saveAndFlush(S entity) {
            S savedEntity = save(entity);
            entityManager.flush();
            return savedEntity;
        }

        @Override
        public void deleteById(Long id) {
            Stmtout entity = entityManager.find(Stmtout.class, id);
            if (entity != null) {
                entityManager.remove(entity);
            }
        }

        @Override
        public void delete(Stmtout entity) {
            entityManager.remove(entity);
        }

        @Override
        public void deleteAll() {
            entityManager.createQuery("DELETE FROM Stmtout").executeUpdate();
        }

        @Override
        public Stmtout getOne(Long id) {
            return entityManager.getReference(Stmtout.class, id);
        }

        @Override
        public Stmtout getById(Long id) {
            return entityManager.find(Stmtout.class, id);
        }

        @Override
        public boolean existsById(Long id) {
            return entityManager.find(Stmtout.class, id) != null;
        }

        @Override
        public long count() {
            return entityManager.createQuery("SELECT COUNT(s) FROM Stmtout s", Long.class).getSingleResult();
        }

        @Override
        public void deleteAllById(Iterable<? extends Long> ids) {
            ids.forEach(this::deleteById);
        }

        @Override
        public void deleteAll(Iterable<? extends Stmtout> entities) {
            entities.forEach(this::delete);
        }

        @Override
        public void deleteAllInBatch() {
            entityManager.createQuery("DELETE FROM Stmtout").executeUpdate();
        }

        @Override
        public void deleteAllByIdInBatch(Iterable<Long> ids) {
            ids.forEach(this::deleteById);
        }

        @Override
        public void deleteAllInBatch(Iterable<Stmtout> entities) {
            entities.forEach(this::delete);
        }

        @Override
        public <S extends Stmtout> Iterable<S> saveAll(Iterable<S> entities) {
            entities.forEach(this::save);
            return entities;
        }

        @Override
        public void flush() {
            entityManager.flush();
        }

        @Override
        public void refresh(Stmtout entity) {
            entityManager.refresh(entity);
        }

        @Override
        public void refreshAll(Iterable<Stmtout> entities) {
            entities.forEach(entityManager::refresh);
        }

        @Override
        public void detach(Stmtout entity) {
            entityManager.detach(entity);
        }

        @Override
        public void detachAll(Iterable<Stmtout> entities) {
            entities.forEach(entityManager::detach);
        }

        @Override
        public <S extends Stmtout> S merge(S entity) {
            return entityManager.merge(entity);
        }

        @Override
        public <S extends Stmtout> Iterable<S> mergeAll(Iterable<S> entities) {
            List<S> mergedEntities = new ArrayList<>();
            for (S entity : entities) {
                mergedEntities.add(merge(entity));
            }
            return mergedEntities;
        }

        @Override
        public void clear() {
            entityManager.clear();
        }

        @Override
        public void close() {
            entityManager.close();
        }

        @Override
        public boolean isOpen() {
            return entityManager.isOpen();
        }
    }
}