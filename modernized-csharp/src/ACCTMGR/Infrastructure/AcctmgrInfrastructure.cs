using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ACCTMGR.Domain;
using ACCTMGR.Application.Interfaces;

namespace ACCTMGR.Infrastructure
{
    public class AcctmgrDbContext : DbContext
    {
        public DbSet<AcctmgrDomainModels.AccountRecord> AccountRecords { get; set; }
        public DbSet<AcctmgrDomainModels.CustomerRecord> CustomerRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("YourConnectionStringHere");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcctmgrDomainModels.AccountRecord>(entity =>
            {
                entity.ToTable("acctfile", "public");
                entity.HasKey(e => e.AccountNumber);

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("acct_number")
                    .HasMaxLength(20); // COBOL: ACCT-NUMBER PIC X(20)

                entity.Property(e => e.CustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasMaxLength(20); // COBOL: ACCT-CUSTOMER-ID PIC X(20)

                entity.Property(e => e.OpenedDate)
                    .HasColumnName("acct_opened_date")
                    .HasColumnType("date"); // COBOL: ACCT-OPENED-DATE PIC 9(8)

                entity.Property(e => e.AccountType)
                    .HasColumnName("acct_type")
                    .HasMaxLength(2); // COBOL: ACCT-TYPE PIC X(2)

                entity.Property(e => e.AccountStatus)
                    .HasColumnName("acct_status")
                    .HasMaxLength(1); // COBOL: ACCT-STATUS PIC X(1)

                entity.Property(e => e.AccountBalance)
                    .HasColumnName("acct_balance")
                    .HasPrecision(15, 2); // COBOL: ACCT-BALANCE PIC S9(13)V99 COMP-3

                entity.Property(e => e.AvailableBalance)
                    .HasColumnName("acct_avail_bal")
                    .HasPrecision(15, 2); // COBOL: ACCT-AVAIL-BAL PIC S9(13)V99 COMP-3

                entity.Property(e => e.InterestRate)
                    .HasColumnName("acct_int_rate")
                    .HasPrecision(6, 4); // COBOL: ACCT-INT-RATE PIC 9(2)V9(4) COMP-3

                entity.Property(e => e.LastTransactionDate)
                    .HasColumnName("acct_last_txn_dt")
                    .HasColumnType("date"); // COBOL: ACCT-LAST-TXN-DT PIC 9(8)

                entity.Property(e => e.Currency)
                    .HasColumnName("acct_currency")
                    .HasMaxLength(3); // COBOL: ACCT-CURRENCY PIC X(3)

                entity.Property(e => e.BranchCode)
                    .HasColumnName("acct_branch_code")
                    .HasPrecision(5, 0); // COBOL: ACCT-BRANCH-CODE PIC 9(5)

                entity.Property(e => e.Flags)
                    .HasColumnName("acct_flags")
                    .HasMaxLength(4); // COBOL: ACCT-FLAGS PIC X(4)

                entity.Property(e => e.Filler)
                    .HasColumnName("acct_filler")
                    .HasMaxLength(50); // COBOL: ACCT-FILLER PIC X(50)
            });

            modelBuilder.Entity<AcctmgrDomainModels.CustomerRecord>(entity =>
            {
                entity.ToTable("customer", "public");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("acct_customer_id")
                    .HasMaxLength(20); // COBOL: ACCT-CUSTOMER-ID PIC X(20)

                entity.Property(e => e.CustomerName)
                    .HasColumnName("customer_name")
                    .HasMaxLength(50); // COBOL: CUSTOMER-NAME PIC X(50)

                entity.Property(e => e.RiskScore)
                    .HasColumnName("cust_risk_score")
                    .HasPrecision(3, 0); // COBOL: CUST-RISK-SCORE PIC 9(03) COMP-3

                entity.Property(e => e.AccountCount)
                    .HasColumnName("cust_acct_count")
                    .HasPrecision(3, 0); // COBOL: CUST-ACCT-COUNT PIC 9(03) COMP-3

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(100); // COBOL: ADDRESS PIC X(100)

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(15); // COBOL: PHONE-NUMBER PIC X(15)

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50); // COBOL: EMAIL PIC X(50)
            });
        }
    }

    public class AccountFileRepository : IAccountFileRepository
    {
        private readonly AcctmgrDbContext _dbContext;

        public AccountFileRepository(AcctmgrDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AcctmgrDomainModels.AccountRecord ReadAccount(string accountNumber)
        {
            return _dbContext.AccountRecords.SingleOrDefault(a => a.AccountNumber == accountNumber);
        }

        public void WriteAccount(AcctmgrDomainModels.AccountRecord accountRecord)
        {
            _dbContext.AccountRecords.Add(accountRecord);
            _dbContext.SaveChanges();
        }

        public void RewriteAccount(AcctmgrDomainModels.AccountRecord accountRecord)
        {
            var existingRecord = _dbContext.AccountRecords.SingleOrDefault(a => a.AccountNumber == accountRecord.AccountNumber);
            if (existingRecord != null)
            {
                _dbContext.Entry(existingRecord).CurrentValues.SetValues(accountRecord);
                _dbContext.SaveChanges();
            }
        }
    }

    public class CicsService : ICicsService
    {
        public void Return()
        {
            // Implementation for returning control to the CICS environment.
        }

        public void WriteRecord(AcctmgrDomainModels.AccountRecord record)
        {
            // Implementation for writing a record to the CICS environment.
        }

        public AcctmgrDomainModels.AccountRecord ReadRecord(string accountNumber)
        {
            // Implementation for reading a record from the CICS environment.
            return null;
        }

        public void RewriteRecord(AcctmgrDomainModels.AccountRecord record)
        {
            // Implementation for rewriting a record in the CICS environment.
        }
    }
}