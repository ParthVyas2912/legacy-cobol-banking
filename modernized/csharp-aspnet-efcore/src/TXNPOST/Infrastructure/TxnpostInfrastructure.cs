using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TXNPOST.Domain;
using TXNPOST.Application.Interfaces;

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

                entity.Property(e => e.BatchId)
                    .HasColumnName("batch_id")
                    .HasMaxLength(8); // COBOL: BATCH-ID PIC X(8)

                entity.Property(e => e.TotalAmount)
                    .HasColumnName("total_amount")
                    .HasPrecision(11, 2); // COBOL: TOTAL-AMOUNT PIC S9(9)V99 COMP-3

                entity.Property(e => e.RecordCount)
                    .HasColumnName("record_count")
                    .HasPrecision(5); // COBOL: RECORD-COUNT PIC 9(5)

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("DATE"); // COBOL: CREATED-DATE PIC 9(8)
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

    public class TransactionRepository : ITransactionRepository
    {
        private readonly TxnpostDbContext _context;

        public TransactionRepository(TxnpostDbContext context)
        {
            _context = context;
        }

        public TxnpostDomainModels.Transaction GetTransactionById(string transactionId)
        {
            return _context.Txnin
                .Where(txn => txn.TxnId == transactionId)
                .Select(txn => new TxnpostDomainModels.Transaction
                {
                    TransactionId = txn.TxnId,
                    AccountNumber = txn.TxnAcctNumber,
                    PostDate = txn.TxnPostDate,
                    EffectiveDate = txn.TxnEffectiveDate,
                    Amount = txn.TxnAmount,
                    Code = txn.TxnCode,
                    Channel = txn.TxnChannel,
                    ReferenceNumber = txn.TxnRefNumber,
                    Description = txn.TxnDescription,
                    Status = (TxnpostDomainModels.Transaction.StatusEnum)Enum.Parse(typeof(TxnpostDomainModels.Transaction.StatusEnum), txn.TxnStatus),
                    BalanceAfter = txn.TxnBalanceAfter
                })
                .FirstOrDefault();
        }

        public void SaveTransaction(TxnpostDomainModels.Transaction transaction)
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
                TxnStatus = transaction.Status.ToString(),
                TxnBalanceAfter = transaction.BalanceAfter
            };

            _context.Txnin.Add(txnin);
            _context.SaveChanges();
        }

        public void UpdateTransactionStatus(string transactionId, TxnpostDomainModels.Transaction.StatusEnum status)
        {
            var txnin = _context.Txnin.FirstOrDefault(txn => txn.TxnId == transactionId);
            if (txnin != null)
            {
                txnin.TxnStatus = status.ToString();
                _context.SaveChanges();
            }
        }

        public void DeleteTransaction(string transactionId)
        {
            var txnin = _context.Txnin.FirstOrDefault(txn => txn.TxnId == transactionId);
            if (txnin != null)
            {
                _context.Txnin.Remove(txnin);
                _context.SaveChanges();
            }
        }
    }

    public class BatchRepository : IBatchRepository
    {
        private readonly TxnpostDbContext _context;

        public BatchRepository(TxnpostDbContext context)
        {
            _context = context;
        }

        public TxnpostDomainModels.Batch GetBatchById(string batchId)
        {
            return _context.Auditout
                .Where(batch => batch.BatchId == batchId)
                .Select(batch => new TxnpostDomainModels.Batch
                {
                    BatchId = batch.BatchId,
                    TotalAmount = batch.TotalAmount,
                    RecordCount = batch.RecordCount
                })
                .FirstOrDefault();
        }

        public void SaveBatch(TxnpostDomainModels.Batch batch)
        {
            var auditout = new Auditout
            {
                BatchId = batch.BatchId,
                TotalAmount = batch.TotalAmount,
                RecordCount = batch.RecordCount
            };

            _context.Auditout.Add(auditout);
            _context.SaveChanges();
        }

        public void UpdateBatchTotals(string batchId, decimal totalAmount, int recordCount)
        {
            var auditout = _context.Auditout.FirstOrDefault(batch => batch.BatchId == batchId);
            if (auditout != null)
            {
                auditout.TotalAmount = totalAmount;
                auditout.RecordCount = recordCount;
                _context.SaveChanges();
            }
        }

        public void DeleteBatch(string batchId)
        {
            var auditout = _context.Auditout.FirstOrDefault(batch => batch.BatchId == batchId);
            if (auditout != null)
            {
                _context.Auditout.Remove(auditout);
                _context.SaveChanges();
            }
        }

        public TxnpostDomainModels.Batch[] GetBatchesByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.Auditout
                .Where(batch => batch.CreatedDate >= startDate && batch.CreatedDate <= endDate)
                .Select(batch => new TxnpostDomainModels.Batch
                {
                    BatchId = batch.BatchId,
                    TotalAmount = batch.TotalAmount,
                    RecordCount = batch.RecordCount
                })
                .ToArray();
        }
    }
}