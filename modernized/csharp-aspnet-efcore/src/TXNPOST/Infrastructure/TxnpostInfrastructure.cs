using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TXNPOST.Application.Interfaces;
using TXNPOST.Domain;

namespace TXNPOST.Infrastructure
{
    public class TxnpostDbContext : DbContext
    {
        public DbSet<Txnin> Txnins { get; set; }
        public DbSet<Auditout> Auditouts { get; set; }
        public DbSet<TxnLedger> TxnLedgers { get; set; }

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
                    .HasColumnType("NUMERIC(13, 2)"); // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3

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
                    .HasColumnType("NUMERIC(15, 2)"); // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3

                entity.Property(e => e.TxnUserId)
                    .HasColumnName("txn_user_id")
                    .HasMaxLength(8); // COBOL: TXN-USER-ID PIC X(08)
            });

            modelBuilder.Entity<Auditout>(entity =>
            {
                entity.ToTable("auditout", "public");

                // Assuming Auditout fields based on domain knowledge
                entity.Property(e => e.AuditId)
                    .HasColumnName("audit_id")
                    .HasMaxLength(10); // Example: AUDIT-ID PIC 9(10)

                entity.Property(e => e.AuditTimestamp)
                    .HasColumnName("audit_timestamp")
                    .HasColumnType("TIMESTAMPTZ"); // Example: AUDIT-TIMESTAMP PIC 9(14)

                entity.Property(e => e.AuditDetails)
                    .HasColumnName("audit_details")
                    .HasMaxLength(255); // Example: AUDIT-DETAILS PIC X(255)
            });

            modelBuilder.Entity<TxnLedger>(entity =>
            {
                entity.ToTable("txn_ledger", "public");

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasColumnType("NUMERIC(10, 0)"); // Adjusted to match COBOL precision

                entity.Property(e => e.Coalesce)
                    .HasColumnName("coalesce")
                    .HasMaxLength(50); // Example: COALESCE PIC X(50)

                entity.Property(e => e.WsVelocityCount)
                    .HasColumnName("ws_velocity_count")
                    .HasColumnType("NUMERIC(10, 0)"); // Adjusted to match COBOL precision

                entity.Property(e => e.WsVelocityAmt)
                    .HasColumnName("ws_velocity_amt")
                    .HasColumnType("NUMERIC(15, 2)"); // Adjusted to match COBOL precision

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasMaxLength(10); // COBOL: TXN-ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.WsCurrentAcct)
                    .HasColumnName("ws_current_acct")
                    .HasMaxLength(10); // Example: WS-CURRENT-ACCT PIC 9(10)

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasMaxLength(12); // COBOL: TXN-ID PIC 9(12)

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasMaxLength(10); // Example: ACCT PIC 9(10)

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("NUMERIC(13, 2)"); // COBOL: AMOUNT PIC S9(11)V99 COMP-3

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(3); // COBOL: CODE PIC X(03)

                entity.Property(e => e.PostedAt)
                    .HasColumnName("posted_at")
                    .HasColumnType("TIMESTAMPTZ"); // COBOL: POSTED-AT PIC 9(14)

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasColumnType("NUMERIC(13, 2)"); // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasMaxLength(3); // COBOL: TXN-CODE PIC X(03)
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
        public string AuditId { get; set; }
        public DateTime AuditTimestamp { get; set; }
        public string AuditDetails { get; set; }
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

        public TxnpostDomainModels.TransactionRecord ReadTransaction()
        {
            var transaction = _context.Txnins.FirstOrDefault();
            if (transaction == null)
            {
                throw new InvalidOperationException("No transaction found.");
            }

            return new TxnpostDomainModels.TransactionRecord
            {
                TxnId = transaction.TxnId,
                TxnAcctNumber = transaction.TxnAcctNumber,
                TxnPostDate = transaction.TxnPostDate,
                TxnEffectiveDate = transaction.TxnEffectiveDate,
                TxnAmount = transaction.TxnAmount,
                TxnCode = transaction.TxnCode,
                TxnChannel = transaction.TxnChannel,
                TxnRefNumber = transaction.TxnRefNumber,
                TxnDescription = transaction.TxnDescription,
                TxnStatus = transaction.TxnStatus,
                TxnBalanceAfter = transaction.TxnBalanceAfter,
                TxnUserId = transaction.TxnUserId
            };
        }

        public void WriteTransaction(TxnpostDomainModels.TransactionRecord transaction)
        {
            var txnin = new Txnin
            {
                TxnId = transaction.TxnId,
                TxnAcctNumber = transaction.TxnAcctNumber,
                TxnPostDate = transaction.TxnPostDate,
                TxnEffectiveDate = transaction.TxnEffectiveDate,
                TxnAmount = transaction.TxnAmount,
                TxnCode = transaction.TxnCode,
                TxnChannel = transaction.TxnChannel,
                TxnRefNumber = transaction.TxnRefNumber,
                TxnDescription = transaction.TxnDescription,
                TxnStatus = transaction.TxnStatus,
                TxnBalanceAfter = transaction.TxnBalanceAfter,
                TxnUserId = transaction.TxnUserId
            };

            _context.Txnins.Add(txnin);
            _context.SaveChanges();
        }

        public void UpdateTransaction(TxnpostDomainModels.TransactionRecord transaction)
        {
            var existingTransaction = _context.Txnins.FirstOrDefault(t => t.TxnId == transaction.TxnId);
            if (existingTransaction == null)
            {
                throw new InvalidOperationException("Transaction not found.");
            }

            existingTransaction.TxnAcctNumber = transaction.TxnAcctNumber;
            existingTransaction.TxnPostDate = transaction.TxnPostDate;
            existingTransaction.TxnEffectiveDate = transaction.TxnEffectiveDate;
            existingTransaction.TxnAmount = transaction.TxnAmount;
            existingTransaction.TxnCode = transaction.TxnCode;
            existingTransaction.TxnChannel = transaction.TxnChannel;
            existingTransaction.TxnRefNumber = transaction.TxnRefNumber;
            existingTransaction.TxnDescription = transaction.TxnDescription;
            existingTransaction.TxnStatus = transaction.TxnStatus;
            existingTransaction.TxnBalanceAfter = transaction.TxnBalanceAfter;
            existingTransaction.TxnUserId = transaction.TxnUserId;

            _context.SaveChanges();
        }

        public void DeleteTransaction(string transactionId)
        {
            var transaction = _context.Txnins.FirstOrDefault(t => t.TxnId == transactionId);
            if (transaction == null)
            {
                throw new InvalidOperationException("Transaction not found.");
            }

            _context.Txnins.Remove(transaction);
            _context.SaveChanges();
        }
    }

    public class CicsTransactionService : ICicsTransactionService
    {
        public void SendTransactionToCics(TxnpostDomainModels.TransactionRecord transaction)
        {
            // Simulate sending a transaction to the CICS system
            Console.WriteLine($"Transaction {transaction.TxnId} sent to CICS.");
        }

        public TxnpostDomainModels.TransactionStatus GetTransactionStatus(string transactionId)
        {
            // Simulate retrieving the status of a transaction from the CICS system
            return new TxnpostDomainModels.TransactionStatus
            {
                TxnId = transactionId,
                Status = "Completed"
            };
        }

        public void CancelTransactionInCics(string transactionId)
        {
            // Simulate canceling a transaction in the CICS system
            Console.WriteLine($"Transaction {transactionId} canceled in CICS.");
        }
    }
}