package src.KYCSYNC.Infrastructure;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.domain.Example;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.data.domain.Sort.Order;
import org.springframework.data.jpa.domain.Specification;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EntityManager;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

import java.util.List;
import java.util.Optional;

/**
 * Infrastructure for KYCSYNC program.
 * Includes ORM mappings, repositories, and configuration.
 */
@Configuration
public class KycsyncInfrastructure {

    @Bean
    public KycAuditRepository kycAuditRepository(EntityManager entityManager) {
        return new KycAuditRepositoryImpl(entityManager);
    }

    @Entity
    @Table(name = "kyc_audit", schema = "public")
    public static class KycAudit {

        @Id
        @GeneratedValue(strategy = GenerationType.IDENTITY)
        @Column(name = "id")
        private Long id;

        @Column(name = "cust_id", length = 255, nullable = false)
        // COBOL: CUST-ID PIC X(255)
        private String custId;

        @Column(name = "cust_kyc_status", length = 26, nullable = false)
        // COBOL: CUST-KYC-STATUS PIC X(26)
        private String custKycStatus;

        @Column(name = "cust_risk_score", length = 255, nullable = false)
        // COBOL: CUST-RISK-SCORE PIC X(255)
        private String custRiskScore;

        // Getters and Setters
        public Long getId() {
            return id;
        }

        public void setId(Long id) {
            this.id = id;
        }

        public String getCustId() {
            return custId;
        }

        public void setCustId(String custId) {
            this.custId = custId;
        }

        public String getCustKycStatus() {
            return custKycStatus;
        }

        public void setCustKycStatus(String custKycStatus) {
            this.custKycStatus = custKycStatus;
        }

        public String getCustRiskScore() {
            return custRiskScore;
        }

        public void setCustRiskScore(String custRiskScore) {
            this.custRiskScore = custRiskScore;
        }
    }

    @Repository
    public interface KycAuditRepository extends JpaRepository<KycAudit, Long> {

        @Query("SELECT k FROM KycAudit k WHERE k.custId = :custId")
        Optional<KycAudit> findByCustId(String custId);
    }

    public static class KycAuditRepositoryImpl implements KycAuditRepository {

        private final EntityManager entityManager;

        public KycAuditRepositoryImpl(EntityManager entityManager) {
            this.entityManager = entityManager;
        }

        @Override
        public <S extends KycAudit> S save(S entity) {
            entityManager.persist(entity);
            return entity;
        }

        @Override
        public <S extends KycAudit> S saveAndFlush(S entity) {
            entityManager.persist(entity);
            entityManager.flush();
            return entity;
        }

        @Override
        public void delete(KycAudit entity) {
            entityManager.remove(entity);
        }

        @Override
        public void deleteAll() {
            entityManager.createQuery("DELETE FROM KycAudit").executeUpdate();
        }

        @Override
        public void deleteAll(Iterable<? extends KycAudit> entities) {
            for (KycAudit entity : entities) {
                entityManager.remove(entity);
            }
        }

        @Override
        public Optional<KycAudit> findById(Long id) {
            return Optional.ofNullable(entityManager.find(KycAudit.class, id));
        }

        @Override
        public boolean existsById(Long id) {
            return findById(id).isPresent();
        }

        @Override
        public List<KycAudit> findAll() {
            return entityManager.createQuery("SELECT k FROM KycAudit k", KycAudit.class).getResultList();
        }

        @Override
        public List<KycAudit> findAllById(Iterable<Long> ids) {
            return entityManager.createQuery("SELECT k FROM KycAudit k WHERE k.id IN :ids", KycAudit.class)
                    .setParameter("ids", ids)
                    .getResultList();
        }

        @Override
        public long count() {
            return entityManager.createQuery("SELECT COUNT(k) FROM KycAudit k", Long.class).getSingleResult();
        }

        @Override
        public void deleteById(Long id) {
            KycAudit entity = entityManager.find(KycAudit.class, id);
            if (entity != null) {
                entityManager.remove(entity);
            }
        }

        @Override
        public void deleteAllInBatch() {
            entityManager.createQuery("DELETE FROM KycAudit").executeUpdate();
        }

        @Override
        public void deleteAllById(Iterable<? extends Long> ids) {
            entityManager.createQuery("DELETE FROM KycAudit k WHERE k.id IN :ids")
                    .setParameter("ids", ids)
                    .executeUpdate();
        }

        @Override
        public void flush() {
            entityManager.flush();
        }

        @Override
        public <S extends KycAudit> List<S> saveAll(Iterable<S> entities) {
            for (S entity : entities) {
                entityManager.persist(entity);
            }
            return (List<S>) entities;
        }

        @Override
        public void deleteAllInBatch(Iterable<KycAudit> entities) {
            for (KycAudit entity : entities) {
                entityManager.remove(entity);
            }
        }

        @Override
        public List<KycAudit> findAll(Sort sort) {
            StringBuilder queryString = new StringBuilder("SELECT k FROM KycAudit k");
            if (sort.isSorted()) {
                queryString.append(" ORDER BY ");
                for (Order order : sort) {
                    queryString.append("k.").append(order.getProperty()).append(" ").append(order.getDirection().name()).append(", ");
                }
                queryString.setLength(queryString.length() - 2); // Remove trailing comma and space
            }
            return entityManager.createQuery(queryString.toString(), KycAudit.class).getResultList();
        }

        @Override
        public Page<KycAudit> findAll(Pageable pageable) {
            List<KycAudit> resultList = entityManager.createQuery("SELECT k FROM KycAudit k", KycAudit.class)
                    .setFirstResult((int) pageable.getOffset())
                    .setMaxResults(pageable.getPageSize())
                    .getResultList();
            long total = count();
            return new PageImpl<>(resultList, pageable, total);
        }

        @Override
        public <S extends KycAudit> Optional<S> findOne(Example<S> example) {
            List<S> results = entityManager.createQuery("SELECT k FROM KycAudit k WHERE k = :example", (Class<S>) example.getProbeType())
                    .setParameter("example", example.getProbe())
                    .getResultList();
            return results.isEmpty() ? Optional.empty() : Optional.of(results.get(0));
        }

        @Override
        public <S extends KycAudit> Page<S> findAll(Example<S> example, Pageable pageable) {
            List<S> resultList = entityManager.createQuery("SELECT k FROM KycAudit k WHERE k = :example", (Class<S>) example.getProbeType())
                    .setParameter("example", example.getProbe())
                    .setFirstResult((int) pageable.getOffset())
                    .setMaxResults(pageable.getPageSize())
                    .getResultList();
            long total = count(example);
            return new PageImpl<>(resultList, pageable, total);
        }

        @Override
        public <S extends KycAudit> long count(Example<S> example) {
            return entityManager.createQuery("SELECT COUNT(k) FROM KycAudit k WHERE k = :example", Long.class)
                    .setParameter("example", example.getProbe())
                    .getSingleResult();
        }

        @Override
        public <S extends KycAudit> boolean exists(Example<S> example) {
            return count(example) > 0;
        }

        @Override
        public List<KycAudit> findAll(Specification<KycAudit> spec) {
            return entityManager.createQuery("SELECT k FROM KycAudit k WHERE " + spec.toPredicate(), KycAudit.class)
                    .getResultList();
        }

        @Override
        public Page<KycAudit> findAll(Specification<KycAudit> spec, Pageable pageable) {
            List<KycAudit> resultList = entityManager.createQuery("SELECT k FROM KycAudit k WHERE " + spec.toPredicate(), KycAudit.class)
                    .setFirstResult((int) pageable.getOffset())
                    .setMaxResults(pageable.getPageSize())
                    .getResultList();
            long total = count(spec);
            return new PageImpl<>(resultList, pageable, total);
        }

        @Override
        public List<KycAudit> findAll(Specification<KycAudit> spec, Sort sort) {
            StringBuilder queryString = new StringBuilder("SELECT k FROM KycAudit k WHERE " + spec.toPredicate());
            if (sort.isSorted()) {
                queryString.append(" ORDER BY ");
                for (Order order : sort) {
                    queryString.append("k.").append(order.getProperty()).append(" ").append(order.getDirection().name()).append(", ");
                }
                queryString.setLength(queryString.length() - 2); // Remove trailing comma and space
            }
            return entityManager.createQuery(queryString.toString(), KycAudit.class).getResultList();
        }

        @Override
        public long count(Specification<KycAudit> spec) {
            return entityManager.createQuery("SELECT COUNT(k) FROM KycAudit k WHERE " + spec.toPredicate(), Long.class)
                    .getSingleResult();
        }
    }
}