using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TXNPOST.Domain;
using TXNPOST.Application.Interfaces;

namespace TXNPOST.Infrastructure
{
    public class TxnpostDbContext : DbContext
    {
        public DbSet<TxnRecord> Txnin { get; set; }
        public DbSet<AuditRecord> Auditout { get; set; }
        public DbSet<TxnLedgerRecord> TxnLedger { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection string here
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TxnRecord>(entity =>
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

            modelBuilder.Entity<AuditRecord>(entity =>
            {
                entity.ToTable("auditout", "public");

                entity.Property(e => e.AuditId)
                    .HasColumnName("audit_id")
                    .HasMaxLength(15); // Assuming COBOL: AUDIT-ID PIC X(15)

                entity.Property(e => e.AuditTimestamp)
                    .HasColumnName("audit_timestamp")
                    .HasColumnType("timestamptz"); // Assuming COBOL: AUDIT-TIMESTAMP PIC 9(14)

                entity.Property(e => e.AuditDetails)
                    .HasColumnName("audit_details")
                    .HasMaxLength(255); // Assuming COBOL: AUDIT-DETAILS PIC X(255)
            });

            modelBuilder.Entity<TxnLedgerRecord>(entity =>
            {
                entity.ToTable("txn_ledger", "public");

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasColumnType("integer"); // Adjusted to match expected numeric type

                entity.Property(e => e.Coalesce)
                    .HasColumnName("coalesce")
                    .HasColumnType("text");

                entity.Property(e => e.WsVelocityCount)
                    .HasColumnName("ws_velocity_count")
                    .HasColumnType("integer"); // Adjusted to match expected numeric type

                entity.Property(e => e.WsVelocityAmt)
                    .HasColumnName("ws_velocity_amt")
                    .HasPrecision(15, 2); // Adjusted to match COMP-3 precision

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasMaxLength(10); // COBOL: TXN-ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.WsCurrentAcct)
                    .HasColumnName("ws_current_acct")
                    .HasMaxLength(10); // Assuming COBOL: WS-CURRENT-ACCT PIC 9(10)

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasMaxLength(12); // COBOL: TXN-ID PIC 9(12)

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasMaxLength(20); // Assuming COBOL: ACCT PIC X(20)

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(13, 2); // Adjusted to match COMP-3 precision

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(3); // COBOL: CODE PIC X(03)

                entity.Property(e => e.PostedAt)
                    .HasColumnName("posted_at")
                    .HasColumnType("timestamptz");

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasPrecision(13, 2); // Adjusted to match COMP-3 precision

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasMaxLength(3); // COBOL: TXN-CODE PIC X(03)
            });
        }
    }

    public class TxnRecord
    {
        public string TxnId { get; set; } // COBOL: TXN-ID PIC 9(12)
        public string TxnAcctNumber { get; set; } // COBOL: TXN-ACCT-NUMBER PIC 9(10)
        public DateTime TxnPostDate { get; set; } // COBOL: TXN-POST-DATE PIC 9(8)
        public DateTime TxnEffectiveDate { get; set; } // COBOL: TXN-EFFECTIVE-DATE PIC 9(8)
        public decimal TxnAmount { get; set; } // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3
        public string TxnCode { get; set; } // COBOL: TXN-CODE PIC X(03)
        public string TxnChannel { get; set; } // COBOL: TXN-CHANNEL PIC X(04)
        public string TxnRefNumber { get; set; } // COBOL: TXN-REF-NUMBER PIC X(20)
        public string TxnDescription { get; set; } // COBOL: TXN-DESCRIPTION PIC X(40)
        public string TxnStatus { get; set; } // COBOL: TXN-STATUS PIC X(01)
        public decimal TxnBalanceAfter { get; set; } // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3
        public string TxnUserId { get; set; } // COBOL: TXN-USER-ID PIC X(08)
    }

    public class AuditRecord
    {
        public string AuditId { get; set; } // Assuming COBOL: AUDIT-ID PIC X(15)
        public DateTime AuditTimestamp { get; set; } // Assuming COBOL: AUDIT-TIMESTAMP PIC 9(14)
        public string AuditDetails { get; set; } // Assuming COBOL: AUDIT-DETAILS PIC X(255)
    }

    public class TxnLedgerRecord
    {
        public int Count { get; set; } // Adjusted to match expected numeric type
        public string Coalesce { get; set; }
        public int WsVelocityCount { get; set; } // Adjusted to match expected numeric type
        public decimal WsVelocityAmt { get; set; } // Adjusted to match COMP-3 precision
        public string TxnAcctNumber { get; set; } // COBOL: TXN-ACCT-NUMBER PIC 9(10)
        public string WsCurrentAcct { get; set; } // Assuming COBOL: WS-CURRENT-ACCT PIC 9(10)
        public string TxnId { get; set; } // COBOL: TXN-ID PIC 9(12)
        public string Acct { get; set; } // Assuming COBOL: ACCT PIC X(20)
        public decimal Amount { get; set; } // Adjusted to match COMP-3 precision
        public string Code { get; set; } // COBOL: CODE PIC X(03)
        public DateTime PostedAt { get; set; }
        public decimal TxnAmount { get; set; } // Adjusted to match COMP-3 precision
        public string TxnCode { get; set; } // COBOL: TXN-CODE PIC X(03)
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly TxnpostDbContext _context;

        public TransactionRepository(TxnpostDbContext context)
        {
            _context = context;
        }

        public TxnpostDomainModels.Transaction ReadTransaction(string transactionId)
        {
            var txnRecord = _context.Txnin.Find(transactionId);
            if (txnRecord == null) return null;

            return new TxnpostDomainModels.Transaction
            {
                TransactionId = txnRecord.TxnId,
                Amount = txnRecord.TxnAmount,
                TransactionDate = txnRecord.TxnPostDate,
                Status = Enum.Parse<TxnpostDomainModels.TransactionStatus>(txnRecord.TxnStatus),
                Account = new TxnpostDomainModels.AccountDetails
                {
                    AccountNumber = txnRecord.TxnAcctNumber,
                    Balance = txnRecord.TxnBalanceAfter
                }
            };
        }

        public void WriteTransaction(TxnpostDomainModels.Transaction transaction)
        {
            var txnRecord = new TxnRecord
            {
                TxnId = transaction.TransactionId,
                TxnAmount = transaction.Amount,
                TxnPostDate = transaction.TransactionDate,
                TxnStatus = transaction.Status.ToString(),
                TxnAcctNumber = transaction.Account.AccountNumber,
                TxnBalanceAfter = transaction.Account.Balance
            };

            _context.Txnin.Add(txnRecord);
            _context.SaveChanges();
        }

        public IEnumerable<TxnpostDomainModels.Transaction> ReadAllTransactions()
        {
            foreach (var txnRecord in _context.Txnin)
            {
                yield return new TxnpostDomainModels.Transaction
                {
                    TransactionId = txnRecord.TxnId,
                    Amount = txnRecord.TxnAmount,
                    TransactionDate = txnRecord.TxnPostDate,
                    Status = Enum.Parse<TxnpostDomainModels.TransactionStatus>(txnRecord.TxnStatus),
                    Account = new TxnpostDomainModels.AccountDetails
                    {
                        AccountNumber = txnRecord.TxnAcctNumber,
                        Balance = txnRecord.TxnBalanceAfter
                    }
                };
            }
        }

        public void UpdateTransaction(TxnpostDomainModels.Transaction transaction)
        {
            var txnRecord = _context.Txnin.Find(transaction.TransactionId);
            if (txnRecord != null)
            {
                txnRecord.TxnAmount = transaction.Amount;
                txnRecord.TxnPostDate = transaction.TransactionDate;
                txnRecord.TxnStatus = transaction.Status.ToString();
                txnRecord.TxnAcctNumber = transaction.Account.AccountNumber;
                txnRecord.TxnBalanceAfter = transaction.Account.Balance;

                _context.SaveChanges();
            }
        }

        public void DeleteTransaction(string transactionId)
        {
            var txnRecord = _context.Txnin.Find(transactionId);
            if (txnRecord != null)
            {
                _context.Txnin.Remove(txnRecord);
                _context.SaveChanges();
            }
        }

        public bool TransactionExists(string transactionId)
        {
            return _context.Txnin.Any(t => t.TxnId == transactionId);
        }
    }

    public class CicsService : ICicsService
    {
        private readonly TxnpostDbContext _context;

        public CicsService(TxnpostDbContext context)
        {
            _context = context;
        }

        public void CommitTransaction(string transactionId)
        {
            // Implementation for committing a transaction
        }

        public void RollbackTransaction(string transactionId)
        {
            // Implementation for rolling back a transaction
        }

        public TxnpostDomainModels.AccountDetails FetchAccountDetails(string accountNumber)
        {
            // Implementation for fetching account details
            return null;
        }

        public void UpdateAccountBalance(string accountNumber, decimal newBalance)
        {
            // Implementation for updating account balance
        }

        public void SendNotification(string accountNumber, string message)
        {
            // Implementation for sending notifications
        }
    }
}