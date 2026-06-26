using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using INTCALC.Domain;

namespace INTCALC.Infrastructure
{
    public class IntcalcDbContext : DbContext
    {
        public DbSet<Acctfile> Acctfiles { get; set; }
        public DbSet<Intjrnl> Intjrnl { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AcctfileConfiguration());
            modelBuilder.ApplyConfiguration(new IntjrnlConfiguration());
        }
    }

    public class Acctfile
    {
        public string AcctNumber { get; set; } // COBOL: ACCT-NUMBER PIC 9(10)
        public string AcctCustomerId { get; set; } // COBOL: ACCT-CUSTOMER-ID PIC 9(8)
        public DateTime AcctOpenedDate { get; set; } // COBOL: ACCT-OPENED-DATE PIC 9(8)
        public string AcctType { get; set; } // COBOL: ACCT-TYPE PIC X(02)
        public string AcctStatus { get; set; } // COBOL: ACCT-STATUS PIC X(01)
        public decimal AcctBalance { get; set; } // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3
        public decimal AcctAvailBal { get; set; } // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3
        public decimal AcctIntRate { get; set; } // COBOL: ACCT-INT-RATE PIC 9(2)V9(4)
        public DateTime AcctLastTxnDt { get; set; } // COBOL: ACCT-LAST-TXN-DT PIC 9(8)
        public string AcctCurrency { get; set; } // COBOL: ACCT-CURRENCY PIC X(03)
        public string AcctBranchCode { get; set; } // COBOL: ACCT-BRANCH-CODE PIC 9(4)
        public string AcctFlags { get; set; } // COBOL: ACCT-FLAGS PIC X(04)
        public string AcctFiller { get; set; } // COBOL: ACCT-FILLER PIC X(50)
    }

    public class AcctfileConfiguration : IEntityTypeConfiguration<Acctfile>
    {
        public void Configure(EntityTypeBuilder<Acctfile> builder)
        {
            builder.ToTable("acctfile", "public");

            builder.HasKey(a => a.AcctNumber);

            builder.Property(a => a.AcctNumber)
                .HasColumnName("acct_number")
                .HasMaxLength(10); // COBOL: ACCT-NUMBER PIC 9(10)

            builder.Property(a => a.AcctCustomerId)
                .HasColumnName("acct_customer_id")
                .HasMaxLength(8); // COBOL: ACCT-CUSTOMER-ID PIC 9(8)

            builder.Property(a => a.AcctOpenedDate)
                .HasColumnName("acct_opened_date")
                .HasColumnType("date"); // Ensure correct mapping for PIC 9(8)

            builder.Property(a => a.AcctType)
                .HasColumnName("acct_type")
                .HasMaxLength(2); // COBOL: ACCT-TYPE PIC X(02)

            builder.Property(a => a.AcctStatus)
                .HasColumnName("acct_status")
                .HasMaxLength(1); // COBOL: ACCT-STATUS PIC X(01)

            builder.Property(a => a.AcctBalance)
                .HasColumnName("acct_balance")
                .HasPrecision(15, 2); // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3

            builder.Property(a => a.AcctAvailBal)
                .HasColumnName("acct_avail_bal")
                .HasPrecision(15, 2); // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3

            builder.Property(a => a.AcctIntRate)
                .HasColumnName("acct_int_rate")
                .HasPrecision(6, 4); // COBOL: ACCT-INT-RATE PIC 9(2)V9(4)

            builder.Property(a => a.AcctLastTxnDt)
                .HasColumnName("acct_last_txn_dt")
                .HasColumnType("date"); // Ensure correct mapping for PIC 9(8)

            builder.Property(a => a.AcctCurrency)
                .HasColumnName("acct_currency")
                .HasMaxLength(3); // COBOL: ACCT-CURRENCY PIC X(03)

            builder.Property(a => a.AcctBranchCode)
                .HasColumnName("acct_branch_code")
                .HasMaxLength(4); // COBOL: ACCT-BRANCH-CODE PIC 9(4)

            builder.Property(a => a.AcctFlags)
                .HasColumnName("acct_flags")
                .HasMaxLength(4); // COBOL: ACCT-FLAGS PIC X(04)

            builder.Property(a => a.AcctFiller)
                .HasColumnName("acct_filler")
                .HasMaxLength(50); // COBOL: ACCT-FILLER PIC X(50)
        }
    }

    public class Intjrnl
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

    public class IntjrnlConfiguration : IEntityTypeConfiguration<Intjrnl>
    {
        public void Configure(EntityTypeBuilder<Intjrnl> builder)
        {
            builder.ToTable("intjrnl", "public");

            builder.HasKey(t => t.TxnId);

            builder.Property(t => t.TxnId)
                .HasColumnName("txn_id")
                .HasMaxLength(12); // COBOL: TXN-ID PIC 9(12)

            builder.Property(t => t.TxnAcctNumber)
                .HasColumnName("txn_acct_number")
                .HasMaxLength(10); // COBOL: TXN-ACCT-NUMBER PIC 9(10)

            builder.Property(t => t.TxnPostDate)
                .HasColumnName("txn_post_date")
                .HasColumnType("date"); // Ensure correct mapping for PIC 9(8)

            builder.Property(t => t.TxnEffectiveDate)
                .HasColumnName("txn_effective_date")
                .HasColumnType("date"); // Ensure correct mapping for PIC 9(8)

            builder.Property(t => t.TxnAmount)
                .HasColumnName("txn_amount")
                .HasPrecision(13, 2); // COBOL: TXN-AMOUNT PIC S9(11)V99 COMP-3

            builder.Property(t => t.TxnCode)
                .HasColumnName("txn_code")
                .HasMaxLength(3); // COBOL: TXN-CODE PIC X(03)

            builder.Property(t => t.TxnChannel)
                .HasColumnName("txn_channel")
                .HasMaxLength(4); // COBOL: TXN-CHANNEL PIC X(04)

            builder.Property(t => t.TxnRefNumber)
                .HasColumnName("txn_ref_number")
                .HasMaxLength(20); // COBOL: TXN-REF-NUMBER PIC X(20)

            builder.Property(t => t.TxnDescription)
                .HasColumnName("txn_description")
                .HasMaxLength(40); // COBOL: TXN-DESCRIPTION PIC X(40)

            builder.Property(t => t.TxnStatus)
                .HasColumnName("txn_status")
                .HasMaxLength(1); // COBOL: TXN-STATUS PIC X(01)

            builder.Property(t => t.TxnBalanceAfter)
                .HasColumnName("txn_balance_after")
                .HasPrecision(15, 2); // COBOL: TXN-BALANCE-AFTER PIC S9(13)V99 COMP-3

            builder.Property(t => t.TxnUserId)
                .HasColumnName("txn_user_id")
                .HasMaxLength(8); // COBOL: TXN-USER-ID PIC X(08)
        }
    }
}