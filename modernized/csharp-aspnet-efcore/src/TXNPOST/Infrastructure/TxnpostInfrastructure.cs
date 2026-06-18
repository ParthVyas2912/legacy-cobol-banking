using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TXNPOST.Application.Interfaces;
using TXNPOST.Domain;

namespace TXNPOST.Infrastructure
{
    public class TxnpostDbContext : DbContext
    {
        public DbSet<TxnRecord> Txnin { get; set; }
        public DbSet<AuditRecord> Auditout { get; set; }
        public DbSet<TxnLedgerRecord> TxnLedger { get; set; }

        public TxnpostDbContext(DbContextOptions<TxnpostDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TxninEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AuditoutEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TxnLedgerEntityConfiguration());
        }
    }

    public class TxninEntityConfiguration : IEntityTypeConfiguration<TxnRecord>
    {
        public void Configure(EntityTypeBuilder<TxnRecord> builder)
        {
            builder.ToTable("txnin", "public");

            builder.Property(e => e.TxnId)
                .HasColumnName("txn_id")
                .HasMaxLength(12); // COBOL: TXN-ID PIC 9(12)

            builder.Property(e => e.TxnAcctNumber)
                .HasColumnName("txn_acct_number")
                .HasMaxLength(10); // COBOL: TXN-ACCT-NUMBER PIC 9(10)

            builder.Property(e => e.TxnPostDate)
                .HasColumnName("txn_post_date")
                .HasColumnType("date"); // COBOL: TXN-POST-DATE PIC 9(8)

            builder.Property(e => e.TxnEffectiveDate)
                .HasColumnName("txn_effective_date")
                .HasColumnType("date"); // COBOL: TXN-EFFECTIVE-DATE PIC 9(8)

            builder.Property(e => e.TxnAmount)
                .HasColumnName("txn_amount")
                .HasPrecision(13, 2); // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3

            builder.Property(e => e.TxnCode)
                .HasColumnName("txn_code")
                .HasMaxLength(3); // COBOL: TXN-CODE PIC X(03)

            builder.Property(e => e.TxnChannel)
                .HasColumnName("txn_channel")
                .HasMaxLength(4); // COBOL: TXN-CHANNEL PIC X(04)

            builder.Property(e => e.TxnRefNumber)
                .HasColumnName("txn_ref_number")
                .HasMaxLength(20); // COBOL: TXN-REF-NUMBER PIC X(20)

            builder.Property(e => e.TxnDescription)
                .HasColumnName("txn_description")
                .HasMaxLength(40); // COBOL: TXN-DESCRIPTION PIC X(40)

            builder.Property(e => e.TxnStatus)
                .HasColumnName("txn_status")
                .HasMaxLength(1); // COBOL: TXN-STATUS PIC X(01)

            builder.Property(e => e.TxnBalanceAfter)
                .HasColumnName("txn_balance_after")
                .HasPrecision(15, 2); // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3

            builder.Property(e => e.TxnUserId)
                .HasColumnName("txn_user_id")
                .HasMaxLength(8); // COBOL: TXN-USER-ID PIC X(08)
        }
    }

    public class AuditoutEntityConfiguration : IEntityTypeConfiguration<AuditRecord>
    {
        public void Configure(EntityTypeBuilder<AuditRecord> builder)
        {
            builder.ToTable("auditout", "public");
            // No specific mappings provided for AuditRecord
        }
    }

    public class TxnLedgerEntityConfiguration : IEntityTypeConfiguration<TxnLedgerRecord>
    {
        public void Configure(EntityTypeBuilder<TxnLedgerRecord> builder)
        {
            builder.ToTable("txn_ledger", "public");

            builder.Property(e => e.Count)
                .HasColumnName("count")
                .HasMaxLength(10); // Assuming COUNT is PIC X(10)

            builder.Property(e => e.Coalesce)
                .HasColumnName("coalesce")
                .HasMaxLength(10); // Assuming COALESCE is PIC X(10)

            builder.Property(e => e.WsVelocityCount)
                .HasColumnName("ws_velocity_count")
                .HasMaxLength(10); // Assuming WS_VELOCITY_COUNT is PIC X(10)

            builder.Property(e => e.WsVelocityAmt)
                .HasColumnName("ws_velocity_amt")
                .HasPrecision(13, 2); // Assuming WS_VELOCITY_AMT is PIC S9(11)V99 COMP-3

            builder.Property(e => e.TxnAcctNumber)
                .HasColumnName("txn_acct_number")
                .HasMaxLength(10); // Assuming TXN_ACCT_NUMBER is PIC 9(10)

            builder.Property(e => e.WsCurrentAcct)
                .HasColumnName("ws_current_acct")
                .HasMaxLength(10); // Assuming WS_CURRENT_ACCT is PIC X(10)

            builder.Property(e => e.TxnId)
                .HasColumnName("txn_id")
                .HasMaxLength(12); // Assuming TXN_ID is PIC 9(12)

            builder.Property(e => e.Acct)
                .HasColumnName("acct")
                .HasMaxLength(10); // Assuming ACCT is PIC X(10)

            builder.Property(e => e.Amount)
                .HasColumnName("amount")
                .HasPrecision(13, 2); // Assuming AMOUNT is PIC S9(11)V99 COMP-3

            builder.Property(e => e.Code)
                .HasColumnName("code")
                .HasMaxLength(3); // Assuming CODE is PIC X(03)

            builder.Property(e => e.PostedAt)
                .HasColumnName("posted_at")
                .HasColumnType("timestamptz"); // COBOL: POSTED_AT

            builder.Property(e => e.TxnAmount)
                .HasColumnName("txn_amount")
                .HasPrecision(13, 2); // Assuming TXN_AMOUNT is PIC S9(11)V99 COMP-3

            builder.Property(e => e.TxnCode)
                .HasColumnName("txn_code")
                .HasMaxLength(3); // Assuming TXN_CODE is PIC X(03)
        }
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
                TransactionDate = txn.TxnPostDate,
                Status = Enum.Parse<TxnpostDomainModels.StatusEnum>(txn.TxnStatus),
                Details = new TxnpostDomainModels.TransactionDetails
                {
                    Description = txn.TxnDescription
                }
            };
        }

        public async Task WriteTransactionAsync(TxnpostDomainModels.TransactionRecord transactionRecord)
        {
            var txn = new TxnRecord
            {
                TxnId = transactionRecord.TransactionId,
                TxnAcctNumber = transactionRecord.AccountNumber,
                TxnAmount = transactionRecord.TransactionAmount,
                TxnPostDate = transactionRecord.TransactionDate,
                TxnStatus = transactionRecord.Status.ToString(),
                TxnDescription = transactionRecord.Details.Description
            };

            _context.Txnin.Add(txn);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransactionStatusAsync(string transactionId, TxnpostDomainModels.StatusEnum status)
        {
            var txn = await _context.Txnin.FirstOrDefaultAsync(t => t.TxnId == transactionId);
            if (txn != null)
            {
                txn.TxnStatus = status.ToString();
                await _context.SaveChangesAsync();
            }
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
        // No specific mappings provided for AuditRecord
    }

    public class TxnLedgerRecord
    {
        public string Count { get; set; } // COBOL: COUNT
        public string Coalesce { get; set; } // COBOL: COALESCE
        public string WsVelocityCount { get; set; } // COBOL: WS_VELOCITY_COUNT
        public decimal WsVelocityAmt { get; set; } // COBOL: WS_VELOCITY_AMT PIC S9(11)V99 COMP-3
        public string TxnAcctNumber { get; set; } // COBOL: TXN_ACCT_NUMBER PIC 9(10)
        public string WsCurrentAcct { get; set; } // COBOL: WS_CURRENT_ACCT PIC X(10)
        public string TxnId { get; set; } // COBOL: TXN_ID PIC 9(12)
        public string Acct { get; set; } // COBOL: ACCT PIC X(10)
        public decimal Amount { get; set; } // COBOL: AMOUNT PIC S9(11)V99 COMP-3
        public string Code { get; set; } // COBOL: CODE PIC X(03)
        public DateTime PostedAt { get; set; } // COBOL: POSTED_AT
        public decimal TxnAmount { get; set; } // COBOL: TXN_AMOUNT PIC S9(11)V99 COMP-3
        public string TxnCode { get; set; } // COBOL: TXN_CODE PIC X(03)
    }
}