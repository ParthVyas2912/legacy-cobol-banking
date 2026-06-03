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
        public DbSet<TxnLedger> TxnLedger { get; set; }
        public DbSet<Txnin> Txnin { get; set; }
        public DbSet<Auditout> Auditout { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

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
                    .HasMaxLength(8); // COBOL: TXN-POST-DATE PIC 9(8)

                entity.Property(e => e.TxnEffectiveDate)
                    .HasColumnName("txn_effective_date")
                    .HasMaxLength(8); // COBOL: TXN-EFFECTIVE-DATE PIC 9(8)

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
                    .HasMaxLength(10); // COBOL: AUDIT-ID PIC X(10)

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(20); // COBOL: ACCOUNT-NUMBER PIC X(20)

                entity.Property(e => e.AuditAmount)
                    .HasColumnName("audit_amount")
                    .HasPrecision(15, 2); // COBOL: AUDIT-AMOUNT PIC S9(13)V99 COMP-3

                entity.Property(e => e.AuditDate)
                    .HasColumnName("audit_date")
                    .HasMaxLength(8); // COBOL: AUDIT-DATE PIC X(8)

                entity.Property(e => e.AuditTime)
                    .HasColumnName("audit_time")
                    .HasMaxLength(6); // COBOL: AUDIT-TIME PIC X(6)

                entity.Property(e => e.AuditDescription)
                    .HasColumnName("audit_description")
                    .HasMaxLength(50); // COBOL: AUDIT-DESCRIPTION PIC X(50)
            });

            modelBuilder.Entity<TxnLedger>(entity =>
            {
                entity.ToTable("txn_ledger", "public");

                entity.Property(e => e.LedgerId)
                    .HasColumnName("ledger_id")
                    .HasMaxLength(10); // COBOL: LEDGER-ID PIC X(10)

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(20); // COBOL: ACCOUNT-NUMBER PIC X(20)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(15, 2); // COBOL: TRANSACTION-AMOUNT PIC S9(13)V99 COMP-3

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasMaxLength(8); // COBOL: TRANSACTION-DATE PIC X(8)

                entity.Property(e => e.TransactionTime)
                    .HasColumnName("transaction_time")
                    .HasMaxLength(6); // COBOL: TRANSACTION-TIME PIC X(6)

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(50); // COBOL: DESCRIPTION PIC X(50)
            });
        }
    }

    public class TransactionInputRepository : ITransactionInputRepository
    {
        private readonly TxnpostDbContext _context;

        public TransactionInputRepository(TxnpostDbContext context)
        {
            _context = context;
        }

        public TxnpostDomainModels.TransactionRecord[] ReadTransactions()
        {
            return _context.Txnin.Select(txn => new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = txn.TxnId,
                AccountNumber = txn.TxnAcctNumber,
                TransactionAmount = txn.TxnAmount,
                TransactionDate = txn.TxnPostDate,
                Description = txn.TxnDescription
            }).ToArray();
        }
    }

    public class AuditOutputRepository : IAuditOutputRepository
    {
        private readonly TxnpostDbContext _context;

        public AuditOutputRepository(TxnpostDbContext context)
        {
            _context = context;
        }

        public void WriteAuditRecord(TxnpostDomainModels.AuditOutput auditOutput)
        {
            var auditEntity = new Auditout
            {
                AuditId = auditOutput.AuditId,
                AccountNumber = auditOutput.AccountNumber,
                AuditAmount = auditOutput.AuditAmount,
                AuditDate = auditOutput.AuditDate,
                AuditTime = auditOutput.AuditTime,
                AuditDescription = auditOutput.AuditDescription
            };
            _context.Auditout.Add(auditEntity);
            _context.SaveChanges();
        }
    }

    public class TransactionLedgerService : ITransactionLedgerService
    {
        private readonly TxnpostDbContext _context;

        public TransactionLedgerService(TxnpostDbContext context)
        {
            _context = context;
        }

        public void UpdateLedger(TxnpostDomainModels.TransactionRecord transaction)
        {
            var ledgerEntry = new TxnLedger
            {
                LedgerId = Guid.NewGuid().ToString(),
                AccountNumber = transaction.AccountNumber,
                TransactionAmount = transaction.TransactionAmount,
                TransactionDate = transaction.TransactionDate,
                TransactionTime = DateTime.Now.ToString("HHmmss"),
                Description = transaction.Description
            };
            _context.TxnLedger.Add(ledgerEntry);
            _context.SaveChanges();
        }
    }

    public class WorkingStorageService : IWorkingStorageService
    {
        private readonly Dictionary<string, decimal> _workingStorage = new();

        public void UpdateRecordCount(decimal count)
        {
            _workingStorage["RecordCount"] = count;
        }

        public void UpdateFailCount(decimal count)
        {
            _workingStorage["FailCount"] = count;
        }
    }

    public class Txnin
    {
        public string TxnId { get; set; } // COBOL: TXN-ID PIC 9(12)
        public string TxnAcctNumber { get; set; } // COBOL: TXN-ACCT-NUMBER PIC 9(10)
        public string TxnPostDate { get; set; } // COBOL: TXN-POST-DATE PIC 9(8)
        public string TxnEffectiveDate { get; set; } // COBOL: TXN-EFFECTIVE-DATE PIC 9(8)
        public decimal TxnAmount { get; set; } // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3
        public string TxnCode { get; set; } // COBOL: TXN-CODE PIC X(03)
        public string TxnChannel { get; set; } // COBOL: TXN-CHANNEL PIC X(04)
        public string TxnRefNumber { get; set; } // COBOL: TXN-REF-NUMBER PIC X(20)
        public string TxnDescription { get; set; } // COBOL: TXN-DESCRIPTION PIC X(40)
        public string TxnStatus { get; set; } // COBOL: TXN-STATUS PIC X(01)
        public decimal TxnBalanceAfter { get; set; } // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3
        public string TxnUserId { get; set; } // COBOL: TXN-USER-ID PIC X(08)
    }

    public class Auditout
    {
        public string AuditId { get; set; } // COBOL: AUDIT-ID PIC X(10)
        public string AccountNumber { get; set; } // COBOL: ACCOUNT-NUMBER PIC X(20)
        public decimal AuditAmount { get; set; } // COBOL: AUDIT-AMOUNT PIC S9(13)V99 COMP-3
        public string AuditDate { get; set; } // COBOL: AUDIT-DATE PIC X(8)
        public string AuditTime { get; set; } // COBOL: AUDIT-TIME PIC X(6)
        public string AuditDescription { get; set; } // COBOL: AUDIT-DESCRIPTION PIC X(50)
    }

    public class TxnLedger
    {
        public string LedgerId { get; set; } // COBOL: LEDGER-ID PIC X(10)
        public string AccountNumber { get; set; } // COBOL: ACCOUNT-NUMBER PIC X(20)
        public decimal TransactionAmount { get; set; } // COBOL: TRANSACTION-AMOUNT PIC S9(13)V99 COMP-3
        public string TransactionDate { get; set; } // COBOL: TRANSACTION-DATE PIC X(8)
        public string TransactionTime { get; set; } // COBOL: TRANSACTION-TIME PIC X(6)
        public string Description { get; set; } // COBOL: DESCRIPTION PIC X(50)
    }
}