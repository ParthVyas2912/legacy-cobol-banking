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
        public DbSet<TxnpostDomainModels.Transaction> Transactions { get; set; }
        public DbSet<Txnin> Txnins { get; set; }
        public DbSet<Auditout> Auditouts { get; set; }
        public DbSet<TxnLedger> TxnLedgers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Txnin>(entity =>
            {
                entity.ToTable("txnin", "public");

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasColumnType("decimal(12,0)"); // Corrected to match COBOL: TXN-ID PIC 9(12)

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasColumnType("decimal(10,0)"); // Corrected to match COBOL: TXN-ACCT-NUMBER PIC 9(10)

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

                entity.Property(e => e.AuditId)
                    .HasColumnName("audit_id")
                    .HasColumnType("decimal(10,0)"); // Assuming COBOL: AUDIT-ID PIC 9(10)

                entity.Property(e => e.AuditTimestamp)
                    .HasColumnName("audit_timestamp")
                    .HasColumnType("timestamptz"); // Assuming COBOL: AUDIT-TIMESTAMP PIC 9(14)

                entity.Property(e => e.AuditDetails)
                    .HasColumnName("audit_details")
                    .HasMaxLength(255); // Assuming COBOL: AUDIT-DETAILS PIC X(255)
            });

            modelBuilder.Entity<TxnLedger>(entity =>
            {
                entity.ToTable("txn_ledger", "public");

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasColumnType("decimal(10,0)"); // Assuming COBOL: COUNT PIC 9(10)

                entity.Property(e => e.Coalesce)
                    .HasColumnName("coalesce")
                    .HasMaxLength(50); // Assuming COBOL: COALESCE PIC X(50)

                entity.Property(e => e.WsVelocityCount)
                    .HasColumnName("ws_velocity_count")
                    .HasColumnType("decimal(10,0)"); // Assuming COBOL: WS-VELOCITY-COUNT PIC 9(10)

                entity.Property(e => e.WsVelocityAmt)
                    .HasColumnName("ws_velocity_amt")
                    .HasPrecision(13, 2); // Assuming COBOL: WS-VELOCITY-AMT PIC S9(11)V99 COMP-3

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasColumnType("decimal(10,0)"); // Assuming COBOL: TXN-ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.WsCurrentAcct)
                    .HasColumnName("ws_current_acct")
                    .HasMaxLength(20); // Assuming COBOL: WS-CURRENT-ACCT PIC X(20)

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasColumnType("decimal(12,0)"); // Assuming COBOL: TXN-ID PIC 9(12)

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasMaxLength(20); // Assuming COBOL: ACCT PIC X(20)

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(13, 2); // Assuming COBOL: AMOUNT PIC S9(11)V99 COMP-3

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(3); // Assuming COBOL: CODE PIC X(03)

                entity.Property(e => e.PostedAt)
                    .HasColumnName("posted_at")
                    .HasColumnType("timestamptz"); // COBOL: POSTED-AT

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasPrecision(13, 2); // Assuming COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasMaxLength(3); // Assuming COBOL: TXN-CODE PIC X(03)
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
            return _context.Transactions.SingleOrDefault(t => t.Id == transactionId);
        }

        public void SaveTransaction(TxnpostDomainModels.Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public void UpdateTransaction(TxnpostDomainModels.Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }

        public void DeleteTransaction(string transactionId)
        {
            var transaction = _context.Transactions.SingleOrDefault(t => t.Id == transactionId);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                _context.SaveChanges();
            }
        }

        public List<TxnpostDomainModels.Transaction> GetTransactionsByAccountId(string accountId)
        {
            return _context.Transactions.Where(t => t.AccountId == accountId).ToList();
        }
    }

    public class CicsService : ICicsService
    {
        public void Commit()
        {
            // Simulate committing a transaction in the CICS environment
            Console.WriteLine("Transaction committed in CICS environment.");
        }

        public void Rollback()
        {
            // Simulate rolling back a transaction in the CICS environment
            Console.WriteLine("Transaction rolled back in CICS environment.");
        }

        public void SendMessage(string message)
        {
            // Simulate sending a message to the CICS queue
            Console.WriteLine($"Message sent to CICS queue: {message}");
        }

        public string ReceiveMessage()
        {
            // Simulate receiving a message from the CICS queue
            Console.WriteLine("Message received from CICS queue.");
            return "Sample message from CICS queue.";
        }

        public TxnpostDomainModels.TransactionStatus GetTransactionStatus(string transactionId)
        {
            // Simulate retrieving the status of a transaction in the CICS environment
            Console.WriteLine($"Retrieving transaction status for ID: {transactionId}");
            return new TxnpostDomainModels.TransactionStatus
            {
                TransactionId = transactionId,
                Status = "Completed",
                Timestamp = DateTime.UtcNow
            };
        }
    }
}