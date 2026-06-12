using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FRDCHK.Domain;
using FRDCHK.Application.Interfaces;

namespace FRDCHK.Infrastructure
{
    public class FrdchkDbContext : DbContext
    {
        public DbSet<TxnLedger> TxnLedgers { get; set; }
        public DbSet<FraudFlags> FraudFlags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure the database connection string here
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: WS_VELOCITY_AMT PIC X(30)

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
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: AMOUNT PIC X(30)

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: CODE PIC X(30)

                entity.Property(e => e.PostedAt)
                    .HasColumnName("posted_at")
                    .HasColumnType("TIMESTAMPTZ");
                // COBOL: POSTED_AT PIC X(30)

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: TXN_AMOUNT PIC X(30)

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: TXN_CODE PIC X(30)
            });

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
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: SCORE PIC X(30)

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: REASON PIC X(30)

                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("TIMESTAMPTZ");
                // COBOL: TS PIC X(30)

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: TXN_ACCT_NUMBER PIC X(30)

                entity.Property(e => e.WsRiskScore)
                    .HasColumnName("ws_risk_score")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: WS_RISK_SCORE PIC X(30)
            });
        }
    }

    public class FraudCheckRepository : IFraudCheckRepository
    {
        private readonly FrdchkDbContext _context;

        public FraudCheckRepository(FrdchkDbContext context)
        {
            _context = context;
        }

        public FrdchkDomainModels.FraudCheckRequest ReadFraudCheckRequest(string requestId)
        {
            var request = _context.TxnLedgers
                .Where(t => t.TxnId == requestId)
                .Select(t => new FrdchkDomainModels.FraudCheckRequest
                {
                    TxnId = t.TxnId,
                    TxnAcctNumber = t.TxnAcctNumber,
                    Amount = t.Amount,
                    Code = t.Code,
                    PostedAt = t.PostedAt
                })
                .FirstOrDefault();

            if (request == null)
            {
                throw new KeyNotFoundException($"Fraud check request with ID {requestId} not found.");
            }

            return request;
        }

        public void WriteFraudCheckRequest(FrdchkDomainModels.FraudCheckRequest request)
        {
            var txnLedger = new TxnLedger
            {
                TxnId = request.TxnId,
                TxnAcctNumber = request.TxnAcctNumber,
                Amount = request.Amount,
                Code = request.Code,
                PostedAt = request.PostedAt
            };

            _context.TxnLedgers.Add(txnLedger);
            _context.SaveChanges();
        }

        public void UpdateFraudCheckStatus(string requestId, FrdchkDomainModels.FraudCheckStatus status)
        {
            var txnLedger = _context.TxnLedgers.FirstOrDefault(t => t.TxnId == requestId);

            if (txnLedger == null)
            {
                throw new KeyNotFoundException($"Fraud check request with ID {requestId} not found.");
            }

            txnLedger.Code = status.StatusCode;
            _context.SaveChanges();
        }

        public void DeleteFraudCheckRequest(string requestId)
        {
            var txnLedger = _context.TxnLedgers.FirstOrDefault(t => t.TxnId == requestId);

            if (txnLedger == null)
            {
                throw new KeyNotFoundException($"Fraud check request with ID {requestId} not found.");
            }

            _context.TxnLedgers.Remove(txnLedger);
            _context.SaveChanges();
        }
    }

    public class CicsService : ICicsService
    {
        public string ExecuteCicsTransaction(string transactionId, FrdchkDomainModels.FraudCheckRequest request)
        {
            // Simulate a CICS transaction execution
            return $"Transaction {transactionId} executed successfully for account {request.TxnAcctNumber}.";
        }

        public string GetCicsTransactionStatus(string transactionId)
        {
            // Simulate retrieving the status of a CICS transaction
            return $"Transaction {transactionId} is completed.";
        }
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
        public DateTime? PostedAt { get; set; }
        public string TxnAmount { get; set; }
        public string TxnCode { get; set; }
    }

    public class FraudFlags
    {
        public string Acct { get; set; }
        public string Score { get; set; }
        public string Reason { get; set; }
        public DateTime? Ts { get; set; }
        public string TxnAcctNumber { get; set; }
        public string WsRiskScore { get; set; }
    }
}