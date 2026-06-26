using System;
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
                    .HasColumnType("date"); // COBOL: TXN-POST-DATE PIC 9(8)

                entity.Property(e => e.TxnEffectiveDate)
                    .HasColumnName("txn_effective_date")
                    .HasColumnType("date"); // COBOL: TXN-EFFECTIVE-DATE PIC 9(8)

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

                entity.Property(e => e.BatchId)
                    .HasColumnName("batch_id")
                    .HasMaxLength(15); // Assuming COBOL: BATCH-ID PIC X(15)
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
                    .HasColumnType("text");

                entity.Property(e => e.WsCurrentAcct)
                    .HasColumnName("ws_current_acct")
                    .HasColumnType("text");

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasColumnType("text");

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasColumnType("text");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("text");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("text");

                entity.Property(e => e.PostedAt)
                    .HasColumnName("posted_at")
                    .HasColumnType("timestamptz");

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasColumnType("text");

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasColumnType("text");
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
        public string BatchId { get; set; }
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

    public class TxnpostRepository : ITxnpostRepository
    {
        private readonly TxnpostDbContext _context;

        public TxnpostRepository(TxnpostDbContext context)
        {
            _context = context;
        }

        public TxnpostDomainModels.TransactionRecord ReadTransaction(string transactionId)
        {
            var transaction = _context.Txnin
                .Where(t => t.TxnId == transactionId)
                .Select(t => new TxnpostDomainModels.TransactionRecord
                {
                    TransactionId = t.TxnId,
                    AccountNumber = t.TxnAcctNumber,
                    PostDate = t.TxnPostDate,
                    EffectiveDate = t.TxnEffectiveDate,
                    Amount = t.TxnAmount,
                    Code = t.TxnCode,
                    Channel = t.TxnChannel,
                    ReferenceNumber = t.TxnRefNumber,
                    Description = t.TxnDescription,
                    Status = t.TxnStatus,
                    BalanceAfter = t.TxnBalanceAfter,
                    UserId = t.TxnUserId
                })
                .FirstOrDefault();

            if (transaction == null)
            {
                throw new InvalidOperationException($"Transaction with ID {transactionId} not found.");
            }

            return transaction;
        }

        public void WriteTransaction(TxnpostDomainModels.TransactionRecord transaction)
        {
            var txnin = new Txnin
            {
                TxnId = transaction.TransactionId,
                TxnAcctNumber = transaction.AccountNumber,
                TxnPostDate = transaction.PostDate,
                TxnEffectiveDate = transaction.EffectiveDate,
                TxnAmount = transaction.Amount,
                TxnCode = transaction.Code,
                TxnChannel = transaction.Channel,
                TxnRefNumber = transaction.ReferenceNumber,
                TxnDescription = transaction.Description,
                TxnStatus = transaction.Status,
                TxnBalanceAfter = transaction.BalanceAfter,
                TxnUserId = transaction.UserId
            };

            _context.Txnin.Add(txnin);
            _context.SaveChanges();
        }

        public TxnpostDomainModels.BatchRecord ReadBatch(string batchId)
        {
            var batch = _context.Auditout
                .Where(a => a.BatchId == batchId)
                .Select(a => new TxnpostDomainModels.BatchRecord
                {
                    BatchId = a.BatchId,
                    Transactions = _context.Txnin
                        .Where(t => t.BatchId == batchId)
                        .Select(t => new TxnpostDomainModels.TransactionRecord
                        {
                            TransactionId = t.TxnId,
                            AccountNumber = t.TxnAcctNumber,
                            PostDate = t.TxnPostDate,
                            EffectiveDate = t.TxnEffectiveDate,
                            Amount = t.TxnAmount,
                            Code = t.TxnCode,
                            Channel = t.TxnChannel,
                            ReferenceNumber = t.TxnRefNumber,
                            Description = t.TxnDescription,
                            Status = t.TxnStatus,
                            BalanceAfter = t.TxnBalanceAfter,
                            UserId = t.TxnUserId
                        }).ToList()
                })
                .FirstOrDefault();

            if (batch == null)
            {
                throw new InvalidOperationException($"Batch with ID {batchId} not found.");
            }

            return batch;
        }

        public void WriteBatch(TxnpostDomainModels.BatchRecord batch)
        {
            foreach (var transaction in batch.Transactions)
            {
                WriteTransaction(transaction);
            }
        }
    }
}