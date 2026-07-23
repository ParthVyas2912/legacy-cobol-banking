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
                    .HasMaxLength(12)
                    .IsFixedLength(); // Ensure fixed length for PIC 9(12)

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasMaxLength(10)
                    .IsFixedLength(); // Ensure fixed length for PIC 9(10)

                entity.Property(e => e.TxnPostDate)
                    .HasColumnName("txn_post_date")
                    .HasColumnType("date"); // Correct mapping for PIC 9(8)

                entity.Property(e => e.TxnEffectiveDate)
                    .HasColumnName("txn_effective_date")
                    .HasColumnType("date"); // Correct mapping for PIC 9(8)

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasColumnType("decimal(13, 2)"); // Correct precision for PIC S9(11)V99 COMP-3

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasMaxLength(3)
                    .IsFixedLength(); // Ensure fixed length for PIC X(03)

                entity.Property(e => e.TxnChannel)
                    .HasColumnName("txn_channel")
                    .HasMaxLength(4)
                    .IsFixedLength(); // Ensure fixed length for PIC X(04)

                entity.Property(e => e.TxnRefNumber)
                    .HasColumnName("txn_ref_number")
                    .HasMaxLength(20)
                    .IsFixedLength(); // Ensure fixed length for PIC X(20)

                entity.Property(e => e.TxnDescription)
                    .HasColumnName("txn_description")
                    .HasMaxLength(40)
                    .IsFixedLength(); // Ensure fixed length for PIC X(40)

                entity.Property(e => e.TxnStatus)
                    .HasColumnName("txn_status")
                    .HasMaxLength(1)
                    .IsFixedLength(); // Ensure fixed length for PIC X(01)

                entity.Property(e => e.TxnBalanceAfter)
                    .HasColumnName("txn_balance_after")
                    .HasColumnType("decimal(15, 2)"); // Correct precision for PIC S9(13)V99 COMP-3

                entity.Property(e => e.TxnUserId)
                    .HasColumnName("txn_user_id")
                    .HasMaxLength(8)
                    .IsFixedLength(); // Ensure fixed length for PIC X(08)
            });

            modelBuilder.Entity<Auditout>(entity =>
            {
                entity.ToTable("auditout", "public");
                // No specific column mappings provided for Auditout
            });

            modelBuilder.Entity<TxnLedger>(entity =>
            {
                entity.ToTable("txn_ledger", "public");

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasColumnType("text");

                entity.Property(e => e.Coalesce)
                    .HasColumnName("coalesce")
                    .HasColumnType("text");

                entity.Property(e => e.WsVelocityCount)
                    .HasColumnName("ws_velocity_count")
                    .HasColumnType("text");

                entity.Property(e => e.WsVelocityAmt)
                    .HasColumnName("ws_velocity_amt")
                    .HasColumnType("text");

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasMaxLength(10)
                    .IsFixedLength(); // Ensure fixed length for PIC 9(10)

                entity.Property(e => e.WsCurrentAcct)
                    .HasColumnName("ws_current_acct")
                    .HasColumnType("text");

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasMaxLength(12)
                    .IsFixedLength(); // Ensure fixed length for PIC 9(12)

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasColumnType("text");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("decimal(13, 2)"); // Correct precision for PIC S9(11)V99 COMP-3

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(3)
                    .IsFixedLength(); // Ensure fixed length for PIC X(03)

                entity.Property(e => e.PostedAt)
                    .HasColumnName("posted_at")
                    .HasColumnType("timestamptz");

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasColumnType("decimal(13, 2)"); // Correct precision for PIC S9(11)V99 COMP-3

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasMaxLength(3)
                    .IsFixedLength(); // Ensure fixed length for PIC X(03)
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
        // No specific fields provided for Auditout
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
        public decimal Amount { get; set; }
        public string Code { get; set; }
        public DateTime PostedAt { get; set; }
        public decimal TxnAmount { get; set; }
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

            return new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = entity.TxnId,
                TransactionAmount = entity.TxnAmount,
                TransactionDate = entity.TxnPostDate,
                TransactionStatus = Enum.Parse<TxnpostDomainModels.TransactionRecord.TransactionStatusEnum>(entity.TxnStatus),
                Account = new TxnpostDomainModels.TransactionRecord.AccountDetails
                {
                    AccountNumber = entity.TxnAcctNumber,
                    AccountBalance = entity.TxnBalanceAfter
                }
            };
        }

        public void WriteTransaction(TxnpostDomainModels.TransactionRecord transaction)
        {
            var entity = new Txnin
            {
                TxnId = transaction.TransactionId,
                TxnAmount = transaction.TransactionAmount,
                TxnPostDate = transaction.TransactionDate,
                TxnStatus = transaction.TransactionStatus.ToString(),
                TxnAcctNumber = transaction.Account.AccountNumber,
                TxnBalanceAfter = transaction.Account.AccountBalance
            };

            _context.Txnin.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateTransaction(TxnpostDomainModels.TransactionRecord transaction)
        {
            var entity = _context.Txnin.FirstOrDefault(t => t.TxnId == transaction.TransactionId);
            if (entity == null) throw new InvalidOperationException("Transaction not found.");

            entity.TxnAmount = transaction.TransactionAmount;
            entity.TxnPostDate = transaction.TransactionDate;
            entity.TxnStatus = transaction.TransactionStatus.ToString();
            entity.TxnAcctNumber = transaction.Account.AccountNumber;
            entity.TxnBalanceAfter = transaction.Account.AccountBalance;

            _context.SaveChanges();
        }

        public void DeleteTransaction(string transactionId)
        {
            var entity = _context.Txnin.FirstOrDefault(t => t.TxnId == transactionId);
            if (entity == null) throw new InvalidOperationException("Transaction not found.");

            _context.Txnin.Remove(entity);
            _context.SaveChanges();
        }
    }

    public class BatchRepository : IBatchRepository
    {
        private readonly TxnpostDbContext _context;

        public BatchRepository(TxnpostDbContext context)
        {
            _context = context;
        }

        public TxnpostDomainModels.BatchRecord ReadBatchById(string batchId)
        {
            var entity = _context.Auditout.FirstOrDefault(a => a.BatchId == batchId);
            if (entity == null) return null;

            return new TxnpostDomainModels.BatchRecord
            {
                BatchId = entity.BatchId,
                BatchDate = entity.BatchDate,
                BatchStatus = Enum.Parse<TxnpostDomainModels.BatchRecord.BatchStatusEnum>(entity.BatchStatus),
                Transactions = entity.Transactions.Select(t => new TxnpostDomainModels.TransactionRecord
                {
                    TransactionId = t.TransactionId,
                    TransactionAmount = t.TransactionAmount,
                    TransactionDate = t.TransactionDate,
                    TransactionStatus = Enum.Parse<TxnpostDomainModels.TransactionRecord.TransactionStatusEnum>(t.TransactionStatus),
                    Account = new TxnpostDomainModels.TransactionRecord.AccountDetails
                    {
                        AccountNumber = t.AccountNumber,
                        AccountBalance = t.AccountBalance
                    }
                }).ToList()
            };
        }

        public void WriteBatch(TxnpostDomainModels.BatchRecord batch)
        {
            var entity = new Auditout
            {
                BatchId = batch.BatchId,
                BatchDate = batch.BatchDate,
                BatchStatus = batch.BatchStatus.ToString(),
                Transactions = batch.Transactions.Select(t => new Txnin
                {
                    TxnId = t.TransactionId,
                    TxnAmount = t.TransactionAmount,
                    TxnPostDate = t.TransactionDate,
                    TxnStatus = t.TransactionStatus.ToString(),
                    TxnAcctNumber = t.Account.AccountNumber,
                    TxnBalanceAfter = t.Account.AccountBalance
                }).ToList()
            };

            _context.Auditout.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateBatch(TxnpostDomainModels.BatchRecord batch)
        {
            var entity = _context.Auditout.FirstOrDefault(a => a.BatchId == batch.BatchId);
            if (entity == null) throw new InvalidOperationException("Batch not found.");

            entity.BatchDate = batch.BatchDate;
            entity.BatchStatus = batch.BatchStatus.ToString();
            entity.Transactions = batch.Transactions.Select(t => new Txnin
            {
                TxnId = t.TransactionId,
                TxnAmount = t.TransactionAmount,
                TxnPostDate = t.TransactionDate,
                TxnStatus = t.TransactionStatus.ToString(),
                TxnAcctNumber = t.Account.AccountNumber,
                TxnBalanceAfter = t.Account.AccountBalance
            }).ToList();

            _context.SaveChanges();
        }

        public void DeleteBatch(string batchId)
        {
            var entity = _context.Auditout.FirstOrDefault(a => a.BatchId == batchId);
            if (entity == null) throw new InvalidOperationException("Batch not found.");

            _context.Auditout.Remove(entity);
            _context.SaveChanges();
        }
    }
}