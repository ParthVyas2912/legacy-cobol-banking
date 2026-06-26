using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TXNPOST.Application.Interfaces;
using TXNPOST.Domain;

namespace TXNPOST.Infrastructure
{
    public class TxnpostDbContext : DbContext
    {
        public DbSet<Txnin> Txnin { get; set; }
        public DbSet<Auditout> Auditout { get; set; }
        public DbSet<TxnLedger> TxnLedger { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Txnin>(entity =>
            {
                entity.ToTable("txnin", "public");

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasMaxLength(12); // COBOL: TXN-ID PIC 9(12)

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasMaxLength(10); // COBOL: TXN-ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.TxnPostDate)
                    .HasColumnName("txn_post_date")
                    .HasColumnType("DATE"); // COBOL: TXN-POST-DATE PIC 9(8)

                entity.Property(e => e.TxnEffectiveDate)
                    .HasColumnName("txn_effective_date")
                    .HasColumnType("DATE"); // COBOL: TXN-EFFECTIVE-DATE PIC 9(8)

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasPrecision(13, 2); // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasMaxLength(3); // COBOL: TXN-CODE PIC X(03)

                entity.Property(e => e.TxnChannel)
                    .HasColumnName("txn_channel")
                    .HasMaxLength(4); // COBOL: TXN-CHANNEL PIC X(04)

                entity.Property(e => e.TxnRefNumber)
                    .HasColumnName("txn_ref_number")
                    .HasMaxLength(20); // COBOL: TXN-REF-NUMBER PIC X(20)

                entity.Property(e => e.TxnDescription)
                    .HasColumnName("txn_description")
                    .HasMaxLength(40); // COBOL: TXN-DESCRIPTION PIC X(40)

                entity.Property(e => e.TxnStatus)
                    .HasColumnName("txn_status")
                    .HasMaxLength(1); // COBOL: TXN-STATUS PIC X(01)

                entity.Property(e => e.TxnBalanceAfter)
                    .HasColumnName("txn_balance_after")
                    .HasPrecision(15, 2); // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3

                entity.Property(e => e.TxnUserId)
                    .HasColumnName("txn_user_id")
                    .HasMaxLength(8); // COBOL: TXN-USER-ID PIC X(08)
            });

            modelBuilder.Entity<Auditout>(entity =>
            {
                entity.ToTable("auditout", "public");
                // No column mappings provided for Auditout in the data-source mapping
            });

            modelBuilder.Entity<TxnLedger>(entity =>
            {
                entity.ToTable("txn_ledger", "public");

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasColumnType("TEXT");

                entity.Property(e => e.Coalesce)
                    .HasColumnName("coalesce")
                    .HasColumnType("TEXT");

                entity.Property(e => e.WsVelocityCount)
                    .HasColumnName("ws_velocity_count")
                    .HasColumnType("TEXT");

                entity.Property(e => e.WsVelocityAmt)
                    .HasColumnName("ws_velocity_amt")
                    .HasColumnType("TEXT");

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasColumnType("TEXT");

                entity.Property(e => e.WsCurrentAcct)
                    .HasColumnName("ws_current_acct")
                    .HasColumnType("TEXT");

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasColumnType("TEXT");

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasColumnType("TEXT");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("TEXT");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("TEXT");

                entity.Property(e => e.PostedAt)
                    .HasColumnName("posted_at")
                    .HasColumnType("TIMESTAMPTZ");

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasColumnType("TEXT");

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasColumnType("TEXT");
            });
        }
    }

    public class Txnin
    {
        public string TxnId { get; set; }
        public string TxnAcctNumber { get; set; }
        public DateTime TxnPostDate { get; set; }
        public DateTime TxnEffectiveDate { get; set; }
        public decimal TxnAmount { get; set; }
        public string TxnCode { get; set; }
        public string TxnChannel { get; set; }
        public string TxnRefNumber { get; set; }
        public string TxnDescription { get; set; }
        public string TxnStatus { get; set; }
        public decimal TxnBalanceAfter { get; set; }
        public string TxnUserId { get; set; }
    }

    public class Auditout
    {
        // No column mappings provided for Auditout in the data-source mapping
    }

    public class TxnLedger
    {
        public string Count { get; set; }
        public string Coalesce { get; set; }
        public string WsVelocityCount { get; set; }
        public string WsVelocityAmt { get; set; }
        public string TxnAcctNumber { get; set; }
        public string WsCurrentAcct { get; set; }
        public string TxnId { get; set; }
        public string Acct { get; set; }
        public string Amount { get; set; }
        public string Code { get; set; }
        public DateTime PostedAt { get; set; }
        public string TxnAmount { get; set; }
        public string TxnCode { get; set; }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly TxnpostDbContext _context;

        public TransactionRepository(TxnpostDbContext context)
        {
            _context = context;
        }

        public TxnpostDomainModels.TransactionRecord ReadTransactionById(string transactionId)
        {
            var entity = _context.Txnin.FirstOrDefault(t => t.TxnId == transactionId);
            if (entity == null) return null;

            return MapToDomain(entity);
        }

        public void WriteTransaction(TxnpostDomainModels.TransactionRecord transactionRecord)
        {
            var entity = MapToEntity(transactionRecord);
            _context.Txnin.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateTransaction(TxnpostDomainModels.TransactionRecord transactionRecord)
        {
            var entity = _context.Txnin.FirstOrDefault(t => t.TxnId == transactionRecord.TransactionId);
            if (entity == null) return;

            MapToEntity(transactionRecord, entity);
            _context.SaveChanges();
        }

        public void DeleteTransaction(string transactionId)
        {
            var entity = _context.Txnin.FirstOrDefault(t => t.TxnId == transactionId);
            if (entity == null) return;

            _context.Txnin.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<TxnpostDomainModels.TransactionRecord> ReadAllTransactions()
        {
            return _context.Txnin.Select(MapToDomain).ToList();
        }

        public IEnumerable<TxnpostDomainModels.TransactionRecord> SearchTransactionsByType(TxnpostDomainModels.TransactionTypeEnum transactionType)
        {
            var typeCode = transactionType == TxnpostDomainModels.TransactionTypeEnum.Debit ? "D" : "C";
            return _context.Txnin.Where(t => t.TxnCode == typeCode).Select(MapToDomain).ToList();
        }

        private static TxnpostDomainModels.TransactionRecord MapToDomain(Txnin entity)
        {
            return new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = entity.TxnId,
                TransactionAmount = entity.TxnAmount,
                AccountNumber = entity.TxnAcctNumber,
                TransactionDate = entity.TxnPostDate,
                Status = entity.TxnStatus == "P"
                    ? TxnpostDomainModels.StatusEnum.Pending
                    : entity.TxnStatus == "C"
                        ? TxnpostDomainModels.StatusEnum.Completed
                        : TxnpostDomainModels.StatusEnum.Failed,
                AdditionalDetails = new TxnpostDomainModels.TransactionRecord.NestedGroup
                {
                    Description = entity.TxnDescription,
                    FeeAmount = entity.TxnBalanceAfter
                }
            };
        }

        private static Txnin MapToEntity(TxnpostDomainModels.TransactionRecord domain, Txnin entity = null)
        {
            if (entity == null) entity = new Txnin();

            entity.TxnId = domain.TransactionId;
            entity.TxnAmount = domain.TransactionAmount;
            entity.TxnAcctNumber = domain.AccountNumber;
            entity.TxnPostDate = domain.TransactionDate;
            entity.TxnStatus = domain.Status == TxnpostDomainModels.StatusEnum.Pending
                ? "P"
                : domain.Status == TxnpostDomainModels.StatusEnum.Completed
                    ? "C"
                    : "F";
            entity.TxnDescription = domain.AdditionalDetails?.Description;
            entity.TxnBalanceAfter = domain.AdditionalDetails?.FeeAmount ?? 0;

            return entity;
        }
    }
}