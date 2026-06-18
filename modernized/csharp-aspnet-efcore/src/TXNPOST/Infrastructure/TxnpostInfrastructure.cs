using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TXNPOST.Application.Interfaces;
using TXNPOST.Domain;

namespace TXNPOST.Infrastructure
{
    public class TxnpostDbContext : DbContext
    {
        public TxnpostDbContext(DbContextOptions<TxnpostDbContext> options) : base(options) { }

        public DbSet<Txnin> Txnin { get; set; }
        public DbSet<Auditout> Auditout { get; set; }
        public DbSet<TxnLedger> TxnLedger { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Txnin>(entity =>
            {
                entity.ToTable("txnin", "public");
                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasColumnType("char(12)"); // Corrected to match COBOL: TXN-ID PIC 9(12)

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasColumnType("char(10)"); // Corrected to match COBOL: TXN-ACCT-NUMBER PIC 9(10)

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
                    .HasColumnType("char(3)"); // Corrected to match COBOL: TXN-CODE PIC X(03)

                entity.Property(e => e.TxnChannel)
                    .HasColumnName("txn_channel")
                    .HasColumnType("char(4)"); // Corrected to match COBOL: TXN-CHANNEL PIC X(04)

                entity.Property(e => e.TxnRefNumber)
                    .HasColumnName("txn_ref_number")
                    .HasColumnType("char(20)"); // Corrected to match COBOL: TXN-REF-NUMBER PIC X(20)

                entity.Property(e => e.TxnDescription)
                    .HasColumnName("txn_description")
                    .HasColumnType("char(40)"); // Corrected to match COBOL: TXN-DESCRIPTION PIC X(40)

                entity.Property(e => e.TxnStatus)
                    .HasColumnName("txn_status")
                    .HasColumnType("char(1)"); // Corrected to match COBOL: TXN-STATUS PIC X(01)

                entity.Property(e => e.TxnBalanceAfter)
                    .HasColumnName("txn_balance_after")
                    .HasPrecision(15, 2); // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3

                entity.Property(e => e.TxnUserId)
                    .HasColumnName("txn_user_id")
                    .HasColumnType("char(8)"); // Corrected to match COBOL: TXN-USER-ID PIC X(08)
            });

            modelBuilder.Entity<Auditout>(entity =>
            {
                entity.ToTable("auditout", "public");
                // No column mappings provided for AUDITOUT, leaving as is.
            });

            modelBuilder.Entity<TxnLedger>(entity =>
            {
                entity.ToTable("txn_ledger", "public");

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasMaxLength(10); // Assuming COUNT is PIC 9(10)

                entity.Property(e => e.Coalesce)
                    .HasColumnName("coalesce")
                    .HasMaxLength(10); // Assuming COALESCE is PIC 9(10)

                entity.Property(e => e.WsVelocityCount)
                    .HasColumnName("ws_velocity_count")
                    .HasMaxLength(10); // Assuming WS_VELOCITY_COUNT is PIC 9(10)

                entity.Property(e => e.WsVelocityAmt)
                    .HasColumnName("ws_velocity_amt")
                    .HasPrecision(13, 2); // Assuming WS_VELOCITY_AMT is PIC S9(11)V99 COMP-3

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasColumnType("char(10)"); // Corrected to match COBOL: TXN_ACCT_NUMBER PIC 9(10)

                entity.Property(e => e.WsCurrentAcct)
                    .HasColumnName("ws_current_acct")
                    .HasColumnType("char(10)"); // Assuming WS_CURRENT_ACCT is PIC 9(10)

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasColumnType("char(12)"); // Corrected to match COBOL: TXN_ID PIC 9(12)

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasColumnType("char(10)"); // Assuming ACCT is PIC 9(10)

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(13, 2); // Assuming AMOUNT is PIC S9(11)V99 COMP-3

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("char(3)"); // Assuming CODE is PIC X(03)

                entity.Property(e => e.PostedAt)
                    .HasColumnName("posted_at")
                    .HasColumnType("timestamptz"); // COBOL: POSTED_AT

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasPrecision(13, 2); // Corrected to match COBOL: TXN_AMOUNT PIC S9(11)V99 COMP-3

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasColumnType("char(3)"); // Corrected to match COBOL: TXN_CODE PIC X(03)
            });
        }
    }

    public class Txnin
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

    public class Auditout
    {
        // No column mappings provided for AUDITOUT
    }

    public class TxnLedger
    {
        public string Count { get; set; } // COBOL: COUNT
        public string Coalesce { get; set; } // COBOL: COALESCE
        public string WsVelocityCount { get; set; } // COBOL: WS_VELOCITY_COUNT
        public decimal WsVelocityAmt { get; set; } // COBOL: WS_VELOCITY_AMT PIC S9(11)V99 COMP-3
        public string TxnAcctNumber { get; set; } // COBOL: TXN_ACCT_NUMBER PIC 9(10)
        public string WsCurrentAcct { get; set; } // COBOL: WS_CURRENT_ACCT PIC 9(10)
        public string TxnId { get; set; } // COBOL: TXN_ID PIC 9(12)
        public string Acct { get; set; } // COBOL: ACCT PIC 9(10)
        public decimal Amount { get; set; } // COBOL: AMOUNT PIC S9(11)V99 COMP-3
        public string Code { get; set; } // COBOL: CODE PIC X(03)
        public DateTime PostedAt { get; set; } // COBOL: POSTED_AT
        public decimal TxnAmount { get; set; } // COBOL: TXN_AMOUNT PIC S9(11)V99 COMP-3
        public string TxnCode { get; set; } // COBOL: TXN_CODE PIC X(03)
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly TxnpostDbContext _context;

        public TransactionRepository(TxnpostDbContext context)
        {
            _context = context;
        }

        public async Task<TxnpostDomainModels.TransactionRecord> GetTransactionByIdAsync(string transactionId)
        {
            var txn = await _context.Txnin.FirstOrDefaultAsync(t => t.TxnId == transactionId);
            if (txn == null) return null;

            return new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = txn.TxnId,
                AccountNumber = txn.TxnAcctNumber,
                TransactionAmount = txn.TxnAmount,
                TransactionDate = txn.TxnPostDate.ToString("yyyyMMdd"),
                Status = Enum.Parse<TxnpostDomainModels.StatusEnum>(txn.TxnStatus)
            };
        }

        public async Task<IEnumerable<TxnpostDomainModels.TransactionRecord>> GetPendingTransactionsAsync()
        {
            var pendingTxns = await _context.Txnin
                .Where(t => t.TxnStatus == "P")
                .ToListAsync();

            return pendingTxns.Select(txn => new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = txn.TxnId,
                AccountNumber = txn.TxnAcctNumber,
                TransactionAmount = txn.TxnAmount,
                TransactionDate = txn.TxnPostDate.ToString("yyyyMMdd"),
                Status = Enum.Parse<TxnpostDomainModels.StatusEnum>(txn.TxnStatus)
            });
        }

        public async Task SaveTransactionAsync(TxnpostDomainModels.TransactionRecord transactionRecord)
        {
            var txn = new Txnin
            {
                TxnId = transactionRecord.TransactionId,
                TxnAcctNumber = transactionRecord.AccountNumber,
                TxnAmount = transactionRecord.TransactionAmount,
                TxnPostDate = DateTime.ParseExact(transactionRecord.TransactionDate, "yyyyMMdd", null),
                TxnStatus = transactionRecord.Status.ToString().Substring(0, 1)
            };

            _context.Txnin.Add(txn);
            await _context.SaveChangesAsync();
        }
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly TxnpostDbContext _context;

        public AccountRepository(TxnpostDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetAccountBalanceAsync(string accountNumber)
        {
            var account = await _context.TxnLedger
                .Where(a => a.TxnAcctNumber == accountNumber)
                .OrderByDescending(a => a.PostedAt)
                .FirstOrDefaultAsync();

            return account != null ? decimal.Parse(account.Amount) : 0m;
        }

        public async Task UpdateAccountBalanceAsync(string accountNumber, decimal newBalance)
        {
            var account = await _context.TxnLedger
                .Where(a => a.TxnAcctNumber == accountNumber)
                .OrderByDescending(a => a.PostedAt)
                .FirstOrDefaultAsync();

            if (account != null)
            {
                account.Amount = newBalance.ToString();
                _context.TxnLedger.Update(account);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class TransactionLogService : ITransactionLogService
    {
        private readonly TxnpostDbContext _context;

        public TransactionLogService(TxnpostDbContext context)
        {
            _context = context;
        }

        public async Task LogTransactionAsync(TxnpostDomainModels.TransactionRecord transactionRecord)
        {
            var auditRecord = new Auditout
            {
                // Populate audit record fields based on transactionRecord
                // No specific fields provided in the mapping
            };

            _context.Auditout.Add(auditRecord);
            await _context.SaveChangesAsync();
        }
    }

    public class CicsService : ICicsService
    {
        public async Task<bool> PostTransactionToCicsAsync(TxnpostDomainModels.TransactionRecord transactionRecord)
        {
            // Simulate a call to an external CICS service
            await Task.Delay(100); // Simulate network latency
            return true; // Assume the transaction was posted successfully
        }
    }
}