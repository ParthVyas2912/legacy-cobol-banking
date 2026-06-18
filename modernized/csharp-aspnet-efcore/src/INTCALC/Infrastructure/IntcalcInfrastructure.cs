using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using INTCALC.Application.Interfaces;
using INTCALC.Domain;

namespace INTCALC.Infrastructure
{
    public class IntcalcDbContext : DbContext
    {
        public DbSet<Acctfile> Acctfiles { get; set; }
        public DbSet<Intjrnl> Intjrnl { get; set; }

        public IntcalcDbContext(DbContextOptions<IntcalcDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Acctfile>(entity =>
            {
                entity.ToTable("acctfile", "public");

                entity.HasKey(e => e.AcctNumber);

                entity.Property(e => e.AcctNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(10); // COBOL: ACCT-NUMBER PIC 9(10)

                entity.Property(e => e.AcctCustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasMaxLength(8); // COBOL: ACCT-CUSTOMER-ID PIC 9(8)

                entity.Property(e => e.AcctOpenedDate)
                    .HasColumnName("acct_opened_date")
                    .HasColumnType("date"); // COBOL: ACCT-OPENED-DATE PIC 9(8)

                entity.Property(e => e.AcctType)
                    .HasColumnName("acct_type")
                    .HasMaxLength(2); // COBOL: ACCT-TYPE PIC X(02)

                entity.Property(e => e.AcctStatus)
                    .HasColumnName("acct_status")
                    .HasMaxLength(1); // COBOL: ACCT-STATUS PIC X(01)

                entity.Property(e => e.AcctBalance)
                    .HasColumnName("acct_balance")
                    .HasPrecision(15, 2); // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3

                entity.Property(e => e.AcctAvailBal)
                    .HasColumnName("acct_avail_bal")
                    .HasPrecision(15, 2); // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3

                entity.Property(e => e.AcctIntRate)
                    .HasColumnName("acct_int_rate")
                    .HasPrecision(6, 4); // COBOL: ACCT-INT-RATE PIC 9(2)V9(4) COMP-3

                entity.Property(e => e.AcctLastTxnDt)
                    .HasColumnName("acct_last_txn_dt")
                    .HasColumnType("date"); // COBOL: ACCT-LAST-TXN-DT PIC 9(8)

                entity.Property(e => e.AcctCurrency)
                    .HasColumnName("acct_currency")
                    .HasMaxLength(3); // COBOL: ACCT-CURRENCY PIC X(03)

                entity.Property(e => e.AcctBranchCode)
                    .HasColumnName("acct_branch_code")
                    .HasMaxLength(4); // COBOL: ACCT-BRANCH-CODE PIC 9(4)

                entity.Property(e => e.AcctFlags)
                    .HasColumnName("acct_flags")
                    .HasMaxLength(4); // COBOL: ACCT-FLAGS PIC X(04)

                entity.Property(e => e.AcctFiller)
                    .HasColumnName("acct_filler")
                    .HasMaxLength(50); // COBOL: ACCT-FILLER PIC X(50)
            });

            modelBuilder.Entity<Intjrnl>(entity =>
            {
                entity.ToTable("intjrnl", "public");

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
        }
    }

    public class Acctfile
    {
        [Column("acct_number")]
        public string AcctNumber { get; set; } // COBOL: ACCT-NUMBER PIC 9(10)

        [Column("acct_customer_id")]
        public string AcctCustomerId { get; set; } // COBOL: ACCT-CUSTOMER-ID PIC 9(8)

        [Column("acct_opened_date")]
        public DateTime AcctOpenedDate { get; set; } // COBOL: ACCT-OPENED-DATE PIC 9(8)

        [Column("acct_type")]
        public string AcctType { get; set; } // COBOL: ACCT-TYPE PIC X(02)

        [Column("acct_status")]
        public string AcctStatus { get; set; } // COBOL: ACCT-STATUS PIC X(01)

        [Column("acct_balance")]
        [Precision(15, 2)]
        public decimal AcctBalance { get; set; } // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3

        [Column("acct_avail_bal")]
        [Precision(15, 2)]
        public decimal AcctAvailBal { get; set; } // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3

        [Column("acct_int_rate")]
        [Precision(6, 4)]
        public decimal AcctIntRate { get; set; } // COBOL: ACCT-INT-RATE PIC 9(2)V9(4) COMP-3

        [Column("acct_last_txn_dt")]
        public DateTime AcctLastTxnDt { get; set; } // COBOL: ACCT-LAST-TXN-DT PIC 9(8)

        [Column("acct_currency")]
        public string AcctCurrency { get; set; } // COBOL: ACCT-CURRENCY PIC X(03)

        [Column("acct_branch_code")]
        public string AcctBranchCode { get; set; } // COBOL: ACCT-BRANCH-CODE PIC 9(4)

        [Column("acct_flags")]
        public string AcctFlags { get; set; } // COBOL: ACCT-FLAGS PIC X(04)

        [Column("acct_filler")]
        public string AcctFiller { get; set; } // COBOL: ACCT-FILLER PIC X(50)
    }

    public class Intjrnl
    {
        [Column("txn_id")]
        public string TxnId { get; set; } // COBOL: TXN-ID PIC 9(12)

        [Column("txn_acct_number")]
        public string TxnAcctNumber { get; set; } // COBOL: TXN-ACCT-NUMBER PIC 9(10)

        [Column("txn_post_date")]
        public DateTime TxnPostDate { get; set; } // COBOL: TXN-POST-DATE PIC 9(8)

        [Column("txn_effective_date")]
        public DateTime TxnEffectiveDate { get; set; } // COBOL: TXN-EFFECTIVE-DATE PIC 9(8)

        [Column("txn_amount")]
        [Precision(13, 2)]
        public decimal TxnAmount { get; set; } // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3

        [Column("txn_code")]
        public string TxnCode { get; set; } // COBOL: TXN-CODE PIC X(03)

        [Column("txn_channel")]
        public string TxnChannel { get; set; } // COBOL: TXN-CHANNEL PIC X(04)

        [Column("txn_ref_number")]
        public string TxnRefNumber { get; set; } // COBOL: TXN-REF-NUMBER PIC X(20)

        [Column("txn_description")]
        public string TxnDescription { get; set; } // COBOL: TXN-DESCRIPTION PIC X(40)

        [Column("txn_status")]
        public string TxnStatus { get; set; } // COBOL: TXN-STATUS PIC X(01)

        [Column("txn_balance_after")]
        [Precision(15, 2)]
        public decimal TxnBalanceAfter { get; set; } // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3

        [Column("txn_user_id")]
        public string TxnUserId { get; set; } // COBOL: TXN-USER-ID PIC X(08)
    }

    public class InterestCalculationRepository : IInterestCalculationRepository
    {
        private readonly IntcalcDbContext _context;
        private int _currentRecordIndex = 0;

        public InterestCalculationRepository(IntcalcDbContext context)
        {
            _context = context;
        }

        public async Task<IntcalcDomainModels.InterestCalculationRecord> ReadNextRecordAsync()
        {
            var records = await _context.Acctfiles
                .OrderBy(a => a.AcctNumber)
                .Skip(_currentRecordIndex)
                .Take(1)
                .ToListAsync();

            if (records.Count == 0)
            {
                return null;
            }

            _currentRecordIndex++;

            var record = records.First();
            return new IntcalcDomainModels.InterestCalculationRecord
            {
                AcctNumber = record.AcctNumber,
                AcctCustomerId = record.AcctCustomerId,
                AcctOpenedDate = record.AcctOpenedDate,
                AcctType = record.AcctType,
                AcctStatus = record.AcctStatus,
                AcctBalance = record.AcctBalance,
                AcctAvailBal = record.AcctAvailBal,
                AcctIntRate = record.AcctIntRate,
                AcctLastTxnDt = record.AcctLastTxnDt,
                AcctCurrency = record.AcctCurrency,
                AcctBranchCode = record.AcctBranchCode,
                AcctFlags = record.AcctFlags,
                AcctFiller = record.AcctFiller
            };
        }

        public async Task WriteRecordAsync(IntcalcDomainModels.InterestCalculationRecord record)
        {
            var intjrnl = new Intjrnl
            {
                TxnId = record.TxnId,
                TxnAcctNumber = record.TxnAcctNumber,
                TxnPostDate = record.TxnPostDate,
                TxnEffectiveDate = record.TxnEffectiveDate,
                TxnAmount = record.TxnAmount,
                TxnCode = record.TxnCode,
                TxnChannel = record.TxnChannel,
                TxnRefNumber = record.TxnRefNumber,
                TxnDescription = record.TxnDescription,
                TxnStatus = record.TxnStatus,
                TxnBalanceAfter = record.TxnBalanceAfter,
                TxnUserId = record.TxnUserId
            };

            await _context.Intjrnl.AddAsync(intjrnl);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasMoreRecordsAsync()
        {
            var totalRecords = await _context.Acctfiles.CountAsync();
            return _currentRecordIndex < totalRecords;
        }
    }
}