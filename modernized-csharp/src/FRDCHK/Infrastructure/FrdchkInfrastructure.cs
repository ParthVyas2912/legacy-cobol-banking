using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FRDCHK.Domain;

namespace FRDCHK.Infrastructure
{
    /// <summary>
    /// DbContext for the FRDCHK application.
    /// </summary>
    public class FrdchkDbContext : DbContext
    {
        public DbSet<TxnLedger> TxnLedgers { get; set; }
        public DbSet<FraudFlag> FraudFlags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure the database connection string here
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapping for TxnLedger table
            modelBuilder.Entity<TxnLedger>(entity =>
            {
                entity.ToTable("txn_ledger", "public");

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30); // COBOL: COUNT PIC X(30)

                entity.Property(e => e.Coalesce)
                    .HasColumnName("coalesce")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30); // COBOL: COALESCE PIC X(30)

                entity.Property(e => e.WsVelocityCount)
                    .HasColumnName("ws_velocity_count")
                    .HasColumnType("NUMERIC")
                    .HasPrecision(5, 0); // COBOL: WS_VELOCITY_COUNT PIC 9(05) COMP-3

                entity.Property(e => e.WsVelocityAmt)
                    .HasColumnName("ws_velocity_amt")
                    .HasColumnType("NUMERIC")
                    .HasPrecision(13, 2); // COBOL: WS_VELOCITY_AMT PIC S9(11)V99 COMP-3

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30); // COBOL: TXN_ACCT_NUMBER PIC X(30)

                entity.Property(e => e.WsCurrentAcct)
                    .HasColumnName("ws_current_acct")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30); // COBOL: WS_CURRENT_ACCT PIC X(30)

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30); // COBOL: TXN_ID PIC X(30)

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30); // COBOL: ACCT PIC X(30)

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("NUMERIC")
                    .HasPrecision(15, 2); // COBOL: AMOUNT PIC S9(13)V99 COMP-3

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30); // COBOL: CODE PIC X(30)

                entity.Property(e => e.PostedAt)
                    .HasColumnName("posted_at")
                    .HasColumnType("TIMESTAMPTZ"); // COBOL: POSTED_AT PIC X(30)

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasColumnType("NUMERIC")
                    .HasPrecision(13, 2); // COBOL: TXN_AMOUNT PIC S9(11)V99 COMP-3

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30); // COBOL: TXN_CODE PIC X(30)
            });

            // Mapping for FraudFlags table
            modelBuilder.Entity<FraudFlag>(entity =>
            {
                entity.ToTable("fraud_flags", "public");

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30); // COBOL: ACCT PIC X(30)

                entity.Property(e => e.Score)
                    .HasColumnName("score")
                    .HasColumnType("NUMERIC")
                    .HasPrecision(3, 0); // COBOL: SCORE PIC 9(03) COMP-3

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30); // COBOL: REASON PIC X(30)

                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("TIMESTAMPTZ"); // COBOL: TS PIC X(30)

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30); // COBOL: TXN_ACCT_NUMBER PIC X(30)

                entity.Property(e => e.WsRiskScore)
                    .HasColumnName("ws_risk_score")
                    .HasColumnType("NUMERIC")
                    .HasPrecision(3, 0); // COBOL: WS_RISK_SCORE PIC 9(03) COMP-3
            });
        }
    }

    /// <summary>
    /// Repository for TxnLedger operations.
    /// </summary>
    public class TxnLedgerRepository
    {
        private readonly FrdchkDbContext _context;

        public TxnLedgerRepository(FrdchkDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TxnLedger txnLedger)
        {
            await _context.TxnLedgers.AddAsync(txnLedger);
            await _context.SaveChangesAsync();
        }

        public async Task<TxnLedger> GetByIdAsync(string txnId)
        {
            return await _context.TxnLedgers.FindAsync(txnId);
        }
    }

    /// <summary>
    /// Repository for FraudFlags operations.
    /// </summary>
    public class FraudFlagsRepository
    {
        private readonly FrdchkDbContext _context;

        public FraudFlagsRepository(FrdchkDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FraudFlag fraudFlag)
        {
            await _context.FraudFlags.AddAsync(fraudFlag);
            await _context.SaveChangesAsync();
        }

        public async Task<FraudFlag> GetByAcctAsync(string acct)
        {
            return await _context.FraudFlags.FindAsync(acct);
        }
    }

    /// <summary>
    /// Entity representing the TxnLedger table.
    /// </summary>
    public class TxnLedger
    {
        public string Count { get; set; }
        public string Coalesce { get; set; }
        public int WsVelocityCount { get; set; }
        public decimal WsVelocityAmt { get; set; }
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

    /// <summary>
    /// Entity representing the FraudFlags table.
    /// </summary>
    public class FraudFlag
    {
        public string Acct { get; set; }
        public int Score { get; set; }
        public string Reason { get; set; }
        public DateTime Ts { get; set; }
        public string TxnAcctNumber { get; set; }
        public int WsRiskScore { get; set; }
    }
}