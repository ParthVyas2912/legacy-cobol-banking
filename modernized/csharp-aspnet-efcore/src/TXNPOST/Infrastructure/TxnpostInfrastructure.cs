using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TXNPOST.Application.Interfaces;
using TXNPOST.Domain;

namespace TXNPOST.Infrastructure
{
    public class TxnpostDbContext : DbContext
    {
        public DbSet<Txnin> Txnin { get; set; }
        public DbSet<Auditout> Auditout { get; set; }
        public DbSet<TxnLedger> TxnLedger { get; set; }

        public TxnpostDbContext(DbContextOptions<TxnpostDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Txnin>(entity =>
            {
                entity.ToTable("txnin", "public");

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasMaxLength(12)
                    .IsFixedLength(); // COBOL: TXN-ID PIC 9(12)

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasMaxLength(10)
                    .IsFixedLength(); // COBOL: TXN-ACCT-NUMBER PIC 9(10)

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
                    .HasMaxLength(3)
                    .IsFixedLength(); // COBOL: TXN-CODE PIC X(03)

                entity.Property(e => e.TxnChannel)
                    .HasColumnName("txn_channel")
                    .HasMaxLength(4)
                    .IsFixedLength(); // COBOL: TXN-CHANNEL PIC X(04)

                entity.Property(e => e.TxnRefNumber)
                    .HasColumnName("txn_ref_number")
                    .HasMaxLength(20)
                    .IsFixedLength(); // COBOL: TXN-REF-NUMBER PIC X(20)

                entity.Property(e => e.TxnDescription)
                    .HasColumnName("txn_description")
                    .HasMaxLength(40)
                    .IsFixedLength(); // COBOL: TXN-DESCRIPTION PIC X(40)

                entity.Property(e => e.TxnStatus)
                    .HasColumnName("txn_status")
                    .HasMaxLength(1)
                    .IsFixedLength(); // COBOL: TXN-STATUS PIC X(01)

                entity.Property(e => e.TxnBalanceAfter)
                    .HasColumnName("txn_balance_after")
                    .HasPrecision(15, 2); // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3

                entity.Property(e => e.TxnUserId)
                    .HasColumnName("txn_user_id")
                    .HasMaxLength(8)
                    .IsFixedLength(); // COBOL: TXN-USER-ID PIC X(08)
            });

            modelBuilder.Entity<Auditout>(entity =>
            {
                entity.ToTable("auditout", "public");
                // No column mappings provided for AUDITOUT
            });

            modelBuilder.Entity<TxnLedger>(entity =>
            {
                entity.ToTable("txn_ledger", "public");

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasMaxLength(10); // Assuming a reasonable length for this field

                entity.Property(e => e.Coalesce)
                    .HasColumnName("coalesce")
                    .HasMaxLength(10); // Assuming a reasonable length for this field

                entity.Property(e => e.WsVelocityCount)
                    .HasColumnName("ws_velocity_count")
                    .HasMaxLength(10); // Assuming a reasonable length for this field

                entity.Property(e => e.WsVelocityAmt)
                    .HasColumnName("ws_velocity_amt")
                    .HasPrecision(13, 2); // Assuming COMP-3 precision for amounts

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasMaxLength(10)
                    .IsFixedLength(); // COBOL: TXN-ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.WsCurrentAcct)
                    .HasColumnName("ws_current_acct")
                    .HasMaxLength(10); // Assuming a reasonable length for this field

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasMaxLength(12)
                    .IsFixedLength(); // COBOL: TXN-ID PIC 9(12)

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasMaxLength(10); // Assuming a reasonable length for this field

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(13, 2); // Assuming COMP-3 precision for amounts

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(3)
                    .IsFixedLength(); // COBOL: TXN-CODE PIC X(03)

                entity.Property(e => e.PostedAt)
                    .HasColumnName("posted_at")
                    .HasColumnType("timestamptz");

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasPrecision(13, 2); // Assuming COMP-3 precision for amounts

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasMaxLength(3)
                    .IsFixedLength(); // COBOL: TXN-CODE PIC X(03)
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
        public string Count { get; set; }
        public string Coalesce { get; set; }
        public string WsVelocityCount { get; set; }
        public decimal WsVelocityAmt { get; set; } // Adjusted to match COMP-3 precision
        public string TxnAcctNumber { get; set; }
        public string WsCurrentAcct { get; set; }
        public string TxnId { get; set; }
        public string Acct { get; set; }
        public decimal Amount { get; set; } // Adjusted to match COMP-3 precision
        public string Code { get; set; }
        public DateTime PostedAt { get; set; }
        public decimal TxnAmount { get; set; } // Adjusted to match COMP-3 precision
        public string TxnCode { get; set; }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly TxnpostDbContext _context;

        public TransactionRepository(TxnpostDbContext context)
        {
            _context = context;
        }

        public async Task<TxnpostDomainModels.TransactionRecord> ReadTransactionAsync(string transactionId)
        {
            var txn = await _context.Txnin.FirstOrDefaultAsync(t => t.TxnId == transactionId);
            if (txn == null) return null;

            return new TxnpostDomainModels.TransactionRecord
            {
                TransactionId = txn.TxnId,
                AccountNumber = txn.TxnAcctNumber,
                TransactionAmount = txn.TxnAmount,
                TransactionDate = txn.TxnPostDate.ToString("yyyyMMdd"),
                Description = txn.TxnDescription,
                BalanceAfterTransaction = txn.TxnBalanceAfter,
                Status = Enum.Parse<TxnpostDomainModels.TransactionStatus>(txn.TxnStatus)
            };
        }

        public async Task WriteTransactionAsync(TxnpostDomainModels.TransactionRecord transaction)
        {
            var txn = new Txnin
            {
                TxnId = transaction.TransactionId,
                TxnAcctNumber = transaction.AccountNumber,
                TxnAmount = transaction.TransactionAmount,
                TxnPostDate = DateTime.ParseExact(transaction.TransactionDate, "yyyyMMdd", null),
                TxnDescription = transaction.Description,
                TxnBalanceAfter = transaction.BalanceAfterTransaction,
                TxnStatus = transaction.Status.ToString()
            };

            _context.Txnin.Add(txn);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransactionAsync(TxnpostDomainModels.TransactionRecord transaction)
        {
            var txn = await _context.Txnin.FirstOrDefaultAsync(t => t.TxnId == transaction.TransactionId);
            if (txn == null) throw new InvalidOperationException("Transaction not found.");

            txn.TxnAcctNumber = transaction.AccountNumber;
            txn.TxnAmount = transaction.TransactionAmount;
            txn.TxnPostDate = DateTime.ParseExact(transaction.TransactionDate, "yyyyMMdd", null);
            txn.TxnDescription = transaction.Description;
            txn.TxnBalanceAfter = transaction.BalanceAfterTransaction;
            txn.TxnStatus = transaction.Status.ToString();

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
            var txn = await _context.Txnin
                .FirstOrDefaultAsync(t => t.TxnAcctNumber == accountNumber);

            if (txn == null) throw new InvalidOperationException("Account not found.");

            return txn.TxnBalanceAfter;
        }

        public async Task UpdateAccountBalanceAsync(string accountNumber, decimal newBalance)
        {
            var txn = await _context.Txnin
                .FirstOrDefaultAsync(t => t.TxnAcctNumber == accountNumber);

            if (txn == null) throw new InvalidOperationException("Account not found.");

            txn.TxnBalanceAfter = newBalance;
            await _context.SaveChangesAsync();
        }
    }

    public class CicsService : ICicsService
    {
        public async Task<string> CallExternalSystemAsync(string requestPayload)
        {
            // Simulate calling an external system
            await Task.Delay(100); // Simulate network latency
            return "Response from external system";
        }
    }
}