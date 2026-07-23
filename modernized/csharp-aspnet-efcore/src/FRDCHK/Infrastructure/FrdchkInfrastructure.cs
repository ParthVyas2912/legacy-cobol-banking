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
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: COUNT PIC X(30)

                entity.Property(e => e.Coalesce)
                    .HasColumnName("coalesce")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: COALESCE PIC X(30)

                entity.Property(e => e.WsVelocityCount)
                    .HasColumnName("ws_velocity_count")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: WS_VELOCITY_COUNT PIC X(30)

                entity.Property(e => e.WsVelocityAmt)
                    .HasColumnName("ws_velocity_amt")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: WS_VELOCITY_AMT PIC X(30)

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: TXN_ACCT_NUMBER PIC X(30)

                entity.Property(e => e.WsCurrentAcct)
                    .HasColumnName("ws_current_acct")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: WS_CURRENT_ACCT PIC X(30)

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: TXN_ID PIC X(30)

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: ACCT PIC X(30)

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: AMOUNT PIC X(30)

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: CODE PIC X(30)

                entity.Property(e => e.PostedAt)
                    .HasColumnName("posted_at")
                    .HasColumnType("timestamp with time zone");
                // COBOL: POSTED_AT PIC X(30)

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: TXN_AMOUNT PIC X(30)

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: TXN_CODE PIC X(30)
            });

            modelBuilder.Entity<FraudFlags>(entity =>
            {
                entity.ToTable("fraud_flags", "public");

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: ACCT PIC X(30)

                entity.Property(e => e.Score)
                    .HasColumnName("score")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: SCORE PIC X(30)

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: REASON PIC X(30)

                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("timestamp with time zone");
                // COBOL: TS PIC X(30)

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasMaxLength(30)
                    .IsUnicode(false);
                // COBOL: TXN_ACCT_NUMBER PIC X(30)

                entity.Property(e => e.WsRiskScore)
                    .HasColumnName("ws_risk_score")
                    .HasMaxLength(30)
                    .IsUnicode(false);
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

    public class CustomerRecordRepository : ICustomerRecordRepository
    {
        private readonly FrdchkDbContext _context;

        public CustomerRecordRepository(FrdchkDbContext context)
        {
            _context = context;
        }

        public FrdchkDomainModels.CustomerRecord ReadCustomerRecord(string customerId)
        {
            return _context.TxnLedgers
                .Where(t => t.TxnAcctNumber == customerId)
                .Select(t => new FrdchkDomainModels.CustomerRecord
                {
                    CustomerId = t.TxnAcctNumber,
                    Transactions = _context.TxnLedgers
                        .Where(txn => txn.TxnAcctNumber == customerId)
                        .Select(txn => new FrdchkDomainModels.Transaction
                        {
                            TransactionId = txn.TxnId,
                            Amount = txn.Amount,
                            Code = txn.Code,
                            PostedAt = txn.PostedAt
                        }).ToList()
                }).FirstOrDefault();
        }

        public void WriteCustomerRecord(FrdchkDomainModels.CustomerRecord customerRecord)
        {
            foreach (var transaction in customerRecord.Transactions)
            {
                var txnLedger = new TxnLedger
                {
                    TxnAcctNumber = customerRecord.CustomerId,
                    TxnId = transaction.TransactionId,
                    Amount = transaction.Amount,
                    Code = transaction.Code,
                    PostedAt = transaction.PostedAt
                };
                _context.TxnLedgers.Add(txnLedger);
            }
            _context.SaveChanges();
        }

        public void DeleteCustomerRecord(string customerId)
        {
            var records = _context.TxnLedgers.Where(t => t.TxnAcctNumber == customerId).ToList();
            _context.TxnLedgers.RemoveRange(records);
            _context.SaveChanges();
        }

        public void UpdateCustomerRecord(FrdchkDomainModels.CustomerRecord customerRecord)
        {
            var existingRecords = _context.TxnLedgers.Where(t => t.TxnAcctNumber == customerRecord.CustomerId).ToList();
            _context.TxnLedgers.RemoveRange(existingRecords);

            foreach (var transaction in customerRecord.Transactions)
            {
                var txnLedger = new TxnLedger
                {
                    TxnAcctNumber = customerRecord.CustomerId,
                    TxnId = transaction.TransactionId,
                    Amount = transaction.Amount,
                    Code = transaction.Code,
                    PostedAt = transaction.PostedAt
                };
                _context.TxnLedgers.Add(txnLedger);
            }
            _context.SaveChanges();
        }

        public bool CustomerRecordExists(string customerId)
        {
            return _context.TxnLedgers.Any(t => t.TxnAcctNumber == customerId);
        }
    }

    public class FraudCheckService : IFraudCheckService
    {
        private readonly FrdchkDbContext _context;

        public FraudCheckService(FrdchkDbContext context)
        {
            _context = context;
        }

        public bool PerformFraudCheck(FrdchkDomainModels.CustomerRecord customerRecord)
        {
            var fraudFlags = _context.FraudFlags
                .Where(f => f.TxnAcctNumber == customerRecord.CustomerId)
                .ToList();

            return fraudFlags.Any(f => f.Score == "HIGH");
        }

        public void LogFraudAlert(string customerId, string alertMessage)
        {
            var fraudFlag = new FraudFlags
            {
                Acct = customerId,
                Reason = alertMessage,
                Ts = DateTime.UtcNow,
                Score = "HIGH"
            };
            _context.FraudFlags.Add(fraudFlag);
            _context.SaveChanges();
        }

        public List<FrdchkDomainModels.Transaction> GetFraudHistory(string customerId)
        {
            return _context.FraudFlags
                .Where(f => f.TxnAcctNumber == customerId)
                .Select(f => new FrdchkDomainModels.Transaction
                {
                    TransactionId = f.TxnAcctNumber,
                    Amount = "N/A",
                    Code = f.Reason,
                    PostedAt = f.Ts
                }).ToList();
        }

        public void EscalateFraudCase(string customerId, string caseDetails)
        {
            var fraudFlag = new FraudFlags
            {
                Acct = customerId,
                Reason = caseDetails,
                Ts = DateTime.UtcNow,
                Score = "CRITICAL"
            };
            _context.FraudFlags.Add(fraudFlag);
            _context.SaveChanges();
        }

        public string GetFraudCaseStatus(string customerId)
        {
            var fraudFlag = _context.FraudFlags
                .Where(f => f.TxnAcctNumber == customerId)
                .OrderByDescending(f => f.Ts)
                .FirstOrDefault();

            return fraudFlag?.Score ?? "NO_RECORD";
        }
    }
}