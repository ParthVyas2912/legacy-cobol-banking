using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FRDCHK.Domain;
using FRDCHK.Application.Interfaces;

namespace FRDCHK.Infrastructure
{
    public class FrdchkDbContext : DbContext
    {
        public FrdchkDbContext(DbContextOptions<FrdchkDbContext> options) : base(options)
        {
        }

        public DbSet<TxnLedger> TxnLedgers { get; set; }
        public DbSet<FraudFlags> FraudFlags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapping for TxnLedger
            modelBuilder.Entity<TxnLedger>(entity =>
            {
                entity.ToTable("txn_ledger", "public");

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: COUNT PIC X(30)

                entity.Property(e => e.Coalesce)
                    .HasColumnName("coalesce")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: COALESCE PIC X(30)

                entity.Property(e => e.WsVelocityCount)
                    .HasColumnName("ws_velocity_count")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: WS_VELOCITY_COUNT PIC X(30)

                entity.Property(e => e.WsVelocityAmt)
                    .HasColumnName("ws_velocity_amt")
                    .HasColumnType("DECIMAL(15,2)")
                    .HasPrecision(15, 2);
                // COBOL: WS_VELOCITY_AMT PIC S9(13)V99 COMP-3

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: TXN_ACCT_NUMBER PIC X(30)

                entity.Property(e => e.WsCurrentAcct)
                    .HasColumnName("ws_current_acct")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: WS_CURRENT_ACCT PIC X(30)

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: TXN_ID PIC X(30)

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: ACCT PIC X(30)

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("DECIMAL(15,2)")
                    .HasPrecision(15, 2);
                // COBOL: AMOUNT PIC S9(13)V99 COMP-3

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: CODE PIC X(30)

                entity.Property(e => e.PostedAt)
                    .HasColumnName("posted_at")
                    .HasColumnType("TIMESTAMP");
                // COBOL: POSTED_AT PIC 9(8)

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasColumnType("DECIMAL(15,2)")
                    .HasPrecision(15, 2);
                // COBOL: TXN_AMOUNT PIC S9(13)V99 COMP-3

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: TXN_CODE PIC X(30)
            });

            // Mapping for FraudFlags
            modelBuilder.Entity<FraudFlags>(entity =>
            {
                entity.ToTable("fraud_flags", "public");

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: ACCT PIC X(30)

                entity.Property(e => e.Score)
                    .HasColumnName("score")
                    .HasColumnType("DECIMAL(15,2)")
                    .HasPrecision(15, 2);
                // COBOL: SCORE PIC S9(13)V99 COMP-3

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: REASON PIC X(30)

                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("TIMESTAMP");
                // COBOL: TS PIC 9(8)

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: TXN_ACCT_NUMBER PIC X(30)

                entity.Property(e => e.WsRiskScore)
                    .HasColumnName("ws_risk_score")
                    .HasColumnType("DECIMAL(15,2)")
                    .HasPrecision(15, 2);
                // COBOL: WS_RISK_SCORE PIC S9(13)V99 COMP-3
            });
        }
    }

    public class TxnLedger
    {
        public string Count { get; set; }
        public string Coalesce { get; set; }
        public string WsVelocityCount { get; set; }
        public decimal WsVelocityAmt { get; set; }
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

    public class FraudFlags
    {
        public string Acct { get; set; }
        public decimal Score { get; set; }
        public string Reason { get; set; }
        public DateTime Ts { get; set; }
        public string TxnAcctNumber { get; set; }
        public decimal WsRiskScore { get; set; }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly FrdchkDbContext _context;

        public TransactionRepository(FrdchkDbContext context)
        {
            _context = context;
        }

        public async Task<FrdchkDomainModels.TransactionRecord> ReadTransactionAsync()
        {
            var record = await _context.TxnLedgers.FirstOrDefaultAsync();
            if (record == null)
            {
                return null;
            }

            return new FrdchkDomainModels.TransactionRecord
            {
                Count = record.Count,
                Coalesce = record.Coalesce,
                WsVelocityCount = record.WsVelocityCount,
                WsVelocityAmt = record.WsVelocityAmt,
                TxnAcctNumber = record.TxnAcctNumber,
                WsCurrentAcct = record.WsCurrentAcct,
                TxnId = record.TxnId,
                Acct = record.Acct,
                Amount = record.Amount,
                Code = record.Code,
                PostedAt = record.PostedAt,
                TxnAmount = record.TxnAmount,
                TxnCode = record.TxnCode
            };
        }

        public async Task WriteTransactionAsync(FrdchkDomainModels.TransactionRecord transactionRecord)
        {
            var entity = new TxnLedger
            {
                Count = transactionRecord.Count,
                Coalesce = transactionRecord.Coalesce,
                WsVelocityCount = transactionRecord.WsVelocityCount,
                WsVelocityAmt = transactionRecord.WsVelocityAmt,
                TxnAcctNumber = transactionRecord.TxnAcctNumber,
                WsCurrentAcct = transactionRecord.WsCurrentAcct,
                TxnId = transactionRecord.TxnId,
                Acct = transactionRecord.Acct,
                Amount = transactionRecord.Amount,
                Code = transactionRecord.Code,
                PostedAt = transactionRecord.PostedAt,
                TxnAmount = transactionRecord.TxnAmount,
                TxnCode = transactionRecord.TxnCode
            };

            _context.TxnLedgers.Add(entity);
            await _context.SaveChangesAsync();
        }
    }

    public class CicsService : ICicsService
    {
        public async Task<bool> ValidateTransactionAsync(FrdchkDomainModels.TransactionRecord transactionRecord)
        {
            // Simulate a CICS call to validate the transaction
            await Task.Delay(100); // Simulate network latency
            return true;
        }

        public async Task UpdateTransactionStatusAsync(FrdchkDomainModels.TransactionRecord transactionRecord)
        {
            // Simulate a CICS call to update the transaction status
            await Task.Delay(100); // Simulate network latency
        }
    }
}