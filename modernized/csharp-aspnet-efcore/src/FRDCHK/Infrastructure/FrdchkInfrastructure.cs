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

    public class FrdchkRepository : IFrdchkRepository
    {
        private readonly FrdchkDbContext _context;

        public FrdchkRepository(FrdchkDbContext context)
        {
            _context = context;
        }

        public FrdchkDomainModels.CustomerRecord ReadCustomerRecord(string customerId)
        {
            var customerRecord = _context.TxnLedgers
                .Where(t => t.TxnAcctNumber == customerId)
                .Select(t => new FrdchkDomainModels.CustomerRecord
                {
                    CustomerId = t.TxnAcctNumber,
                    AccountNumber = t.Acct,
                    Transactions = _context.TxnLedgers
                        .Where(txn => txn.TxnAcctNumber == customerId)
                        .Select(txn => new FrdchkDomainModels.Transaction
                        {
                            TransactionId = txn.TxnId,
                            Amount = decimal.Parse(txn.Amount),
                            Code = txn.Code,
                            PostedAt = txn.PostedAt
                        }).ToList()
                }).FirstOrDefault();

            if (customerRecord == null)
            {
                throw new KeyNotFoundException($"Customer record with ID {customerId} not found.");
            }

            return customerRecord;
        }

        public void WriteCustomerRecord(FrdchkDomainModels.CustomerRecord customerRecord)
        {
            var existingRecord = _context.TxnLedgers
                .Where(t => t.TxnAcctNumber == customerRecord.CustomerId)
                .ToList();

            if (existingRecord.Any())
            {
                _context.TxnLedgers.RemoveRange(existingRecord);
            }

            foreach (var transaction in customerRecord.Transactions)
            {
                _context.TxnLedgers.Add(new TxnLedger
                {
                    TxnAcctNumber = customerRecord.CustomerId,
                    Acct = customerRecord.AccountNumber,
                    TxnId = transaction.TransactionId,
                    Amount = transaction.Amount.ToString(),
                    Code = transaction.Code,
                    PostedAt = transaction.PostedAt
                });
            }

            _context.SaveChanges();
        }

        public void DeleteCustomerRecord(string customerId)
        {
            var recordsToDelete = _context.TxnLedgers
                .Where(t => t.TxnAcctNumber == customerId)
                .ToList();

            if (!recordsToDelete.Any())
            {
                throw new KeyNotFoundException($"Customer record with ID {customerId} not found.");
            }

            _context.TxnLedgers.RemoveRange(recordsToDelete);
            _context.SaveChanges();
        }

        public List<FrdchkDomainModels.Account> ReadFlaggedAccounts()
        {
            return _context.FraudFlags
                .Select(f => new FrdchkDomainModels.Account
                {
                    AccountNumber = f.Acct,
                    RiskScore = decimal.Parse(f.Score),
                    Reason = f.Reason,
                    Timestamp = f.Ts
                }).ToList();
        }

        public void UpdateAccountBalance(string accountNumber, decimal newBalance)
        {
            var account = _context.TxnLedgers
                .FirstOrDefault(t => t.Acct == accountNumber);

            if (account == null)
            {
                throw new KeyNotFoundException($"Account with number {accountNumber} not found.");
            }

            account.Amount = newBalance.ToString();
            _context.SaveChanges();
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