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

    public class FraudFlags
    {
        public string Acct { get; set; }
        public string Score { get; set; }
        public string Reason { get; set; }
        public DateTime Ts { get; set; }
        public string TxnAcctNumber { get; set; }
        public string WsRiskScore { get; set; }
    }

    public class FrdchkRepository : IFrdchkRepository
    {
        private readonly FrdchkDbContext _context;

        public FrdchkRepository(FrdchkDbContext context)
        {
            _context = context;
        }

        public FrdchkDomainModels.FraudCheckRequest ReadFraudCheckData(string requestId)
        {
            var txnLedger = _context.TxnLedgers.FirstOrDefault(t => t.TxnId == requestId);
            if (txnLedger == null)
            {
                throw new KeyNotFoundException($"Transaction with ID {requestId} not found.");
            }

            var fraudFlags = _context.FraudFlags
                .Where(f => f.TxnAcctNumber == txnLedger.TxnAcctNumber)
                .ToList();

            return new FrdchkDomainModels.FraudCheckRequest
            {
                TxnId = txnLedger.TxnId,
                TxnAcctNumber = txnLedger.TxnAcctNumber,
                Amount = txnLedger.Amount,
                FraudFlags = fraudFlags.Select(f => new FrdchkDomainModels.FraudFlag
                {
                    Acct = f.Acct,
                    Score = f.Score,
                    Reason = f.Reason,
                    Ts = f.Ts
                }).ToList()
            };
        }

        public void WriteFraudCheckData(FrdchkDomainModels.FraudCheckRequest request)
        {
            var txnLedger = new TxnLedger
            {
                TxnId = request.TxnId,
                TxnAcctNumber = request.TxnAcctNumber,
                Amount = request.Amount,
                PostedAt = DateTime.UtcNow
            };

            _context.TxnLedgers.Add(txnLedger);

            foreach (var fraudFlag in request.FraudFlags)
            {
                var fraudFlagEntity = new FraudFlags
                {
                    Acct = fraudFlag.Acct,
                    Score = fraudFlag.Score,
                    Reason = fraudFlag.Reason,
                    Ts = fraudFlag.Ts,
                    TxnAcctNumber = request.TxnAcctNumber
                };

                _context.FraudFlags.Add(fraudFlagEntity);
            }

            _context.SaveChanges();
        }

        public void DeleteFraudCheckData(string requestId)
        {
            var txnLedger = _context.TxnLedgers.FirstOrDefault(t => t.TxnId == requestId);
            if (txnLedger == null)
            {
                throw new KeyNotFoundException($"Transaction with ID {requestId} not found.");
            }

            var fraudFlags = _context.FraudFlags
                .Where(f => f.TxnAcctNumber == txnLedger.TxnAcctNumber)
                .ToList();

            _context.FraudFlags.RemoveRange(fraudFlags);
            _context.TxnLedgers.Remove(txnLedger);

            _context.SaveChanges();
        }
    }

    public class FrdchkCicsService : IFrdchkCicsService
    {
        public string SendFraudCheckRequest(FrdchkDomainModels.FraudCheckRequest request)
        {
            // Simulate sending a fraud check request to CICS and returning a request ID
            return Guid.NewGuid().ToString();
        }

        public FrdchkDomainModels.TransactionStatus GetFraudCheckStatus(string requestId)
        {
            // Simulate retrieving fraud check status from CICS
            return new FrdchkDomainModels.TransactionStatus
            {
                RequestId = requestId,
                Status = "Completed",
                Message = "Fraud check completed successfully."
            };
        }
    }
}