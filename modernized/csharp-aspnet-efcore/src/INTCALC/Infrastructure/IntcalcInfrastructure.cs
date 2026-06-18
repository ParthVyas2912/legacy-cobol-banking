using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace INTCALC.Infrastructure
{
    public class IntcalcDbContext : DbContext
    {
        public DbSet<Acctfile> Acctfiles { get; set; }
        public DbSet<Intjrnl> IntjrnlRecords { get; set; }

        public IntcalcDbContext(DbContextOptions<IntcalcDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        [Key]
        [MaxLength(10)]
        public string AcctNumber { get; set; } // COBOL: ACCT-NUMBER PIC 9(10)

        [MaxLength(8)]
        public string AcctCustomerId { get; set; } // COBOL: ACCT-CUSTOMER-ID PIC 9(8)

        public DateTime AcctOpenedDate { get; set; } // COBOL: ACCT-OPENED-DATE PIC 9(8)

        [MaxLength(2)]
        public string AcctType { get; set; } // COBOL: ACCT-TYPE PIC X(02)

        [MaxLength(1)]
        public string AcctStatus { get; set; } // COBOL: ACCT-STATUS PIC X(01)

        [Precision(15, 2)]
        public decimal AcctBalance { get; set; } // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3

        [Precision(15, 2)]
        public decimal AcctAvailBal { get; set; } // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3

        [Precision(6, 4)]
        public decimal AcctIntRate { get; set; } // COBOL: ACCT-INT-RATE PIC 9(2)V9(4) COMP-3

        public DateTime AcctLastTxnDt { get; set; } // COBOL: ACCT-LAST-TXN-DT PIC 9(8)

        [MaxLength(3)]
        public string AcctCurrency { get; set; } // COBOL: ACCT-CURRENCY PIC X(03)

        [MaxLength(4)]
        public string AcctBranchCode { get; set; } // COBOL: ACCT-BRANCH-CODE PIC 9(4)

        [MaxLength(4)]
        public string AcctFlags { get; set; } // COBOL: ACCT-FLAGS PIC X(04)

        [MaxLength(50)]
        public string AcctFiller { get; set; } // COBOL: ACCT-FILLER PIC X(50)
    }

    public class Intjrnl
    {
        [Key]
        [MaxLength(12)]
        public string TxnId { get; set; } // COBOL: TXN-ID PIC 9(12)

        [MaxLength(10)]
        public string TxnAcctNumber { get; set; } // COBOL: TXN-ACCT-NUMBER PIC 9(10)

        public DateTime TxnPostDate { get; set; } // COBOL: TXN-POST-DATE PIC 9(8)

        public DateTime TxnEffectiveDate { get; set; } // COBOL: TXN-EFFECTIVE-DATE PIC 9(8)

        [Precision(13, 2)]
        public decimal TxnAmount { get; set; } // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3

        [MaxLength(3)]
        public string TxnCode { get; set; } // COBOL: TXN-CODE PIC X(03)

        [MaxLength(4)]
        public string TxnChannel { get; set; } // COBOL: TXN-CHANNEL PIC X(04)

        [MaxLength(20)]
        public string TxnRefNumber { get; set; } // COBOL: TXN-REF-NUMBER PIC X(20)

        [MaxLength(40)]
        public string TxnDescription { get; set; } // COBOL: TXN-DESCRIPTION PIC X(40)

        [MaxLength(1)]
        public string TxnStatus { get; set; } // COBOL: TXN-STATUS PIC X(01)

        [Precision(15, 2)]
        public decimal TxnBalanceAfter { get; set; } // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3

        [MaxLength(8)]
        public string TxnUserId { get; set; } // COBOL: TXN-USER-ID PIC X(08)
    }
}