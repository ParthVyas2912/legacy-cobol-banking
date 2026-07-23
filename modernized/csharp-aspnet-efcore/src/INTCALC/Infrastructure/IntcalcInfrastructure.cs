using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using INTCALC.Domain;
using INTCALC.Application.Interfaces;

namespace INTCALC.Infrastructure
{
    public class IntcalcDbContext : DbContext
    {
        public DbSet<Acctfile> Acctfiles { get; set; }
        public DbSet<Intjrnl> Intjrnl { get; set; }

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
                    .HasColumnType("DATE"); // COBOL: ACCT-OPENED-DATE PIC 9(8)

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
                    .HasPrecision(6, 4); // COBOL: ACCT-INT-RATE PIC 9(2)V9(4)

                entity.Property(e => e.AcctLastTxnDt)
                    .HasColumnName("acct_last_txn_dt")
                    .HasColumnType("DATE"); // COBOL: ACCT-LAST-TXN-DT PIC 9(8)

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
                    .HasColumnType("DATE"); // COBOL: TXN-POST-DATE PIC 9(8)

                entity.Property(e => e.TxnEffectiveDate)
                    .HasColumnName("txn_effective_date")
                    .HasColumnType("DATE"); // COBOL: TXN-EFFECTIVE-DATE PIC 9(8)

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

    public class IntcalcRepository : IIntcalcRepository
    {
        private readonly IntcalcDbContext _context;

        public IntcalcRepository(IntcalcDbContext context)
        {
            _context = context;
        }

        public IntcalcDomainModels.CalculationRequest ReadCalculationRequest(string requestId)
        {
            var request = _context.Acctfiles
                .Where(a => a.AcctNumber == requestId)
                .Select(a => new IntcalcDomainModels.CalculationRequest
                {
                    AccountNumber = a.AcctNumber,
                    CustomerId = a.AcctCustomerId,
                    Balance = a.AcctBalance,
                    InterestRate = a.AcctIntRate,
                    LastTransactionDate = a.AcctLastTxnDt
                })
                .FirstOrDefault();

            if (request == null)
            {
                throw new KeyNotFoundException($"Calculation request with ID {requestId} not found.");
            }

            return request;
        }

        public void WriteCalculationResult(string requestId, decimal result)
        {
            var account = _context.Acctfiles.FirstOrDefault(a => a.AcctNumber == requestId);

            if (account == null)
            {
                throw new KeyNotFoundException($"Account with ID {requestId} not found.");
            }

            account.AcctBalance += result;
            _context.SaveChanges();
        }
    }

    public class IntcalcCicsService : IIntcalcCicsService
    {
        public IntcalcDomainModels.InterestDetails RetrieveExternalData(IntcalcDomainModels.CalculationRequest request)
        {
            // Simulate external data retrieval
            return new IntcalcDomainModels.InterestDetails
            {
                AccountNumber = request.AccountNumber,
                InterestRate = request.InterestRate,
                Balance = request.Balance,
                CalculatedInterest = request.Balance * request.InterestRate / 100
            };
        }

        public void LogCalculationProcess(string requestId, string status)
        {
            Console.WriteLine($"Request ID: {requestId}, Status: {status}");
        }
    }

    public class Acctfile
    {
        public string AcctNumber { get; set; }
        public string AcctCustomerId { get; set; }
        public DateTime AcctOpenedDate { get; set; }
        public string AcctType { get; set; }
        public string AcctStatus { get; set; }
        public decimal AcctBalance { get; set; }
        public decimal AcctAvailBal { get; set; }
        public decimal AcctIntRate { get; set; }
        public DateTime AcctLastTxnDt { get; set; }
        public string AcctCurrency { get; set; }
        public string AcctBranchCode { get; set; }
        public string AcctFlags { get; set; }
        public string AcctFiller { get; set; }
    }

    public class Intjrnl
    {
        public string TxnId { get; set; }
        public string TxnAcctNumber { get; set; }
        public DateTime TxnPostDate { get; set; }
        public DateTime TxnEffectiveDate { get; set; }
        public decimal TxnAmount { get; set; }
        public string TxnCode { get; set; }
        public string TxnChannel { get; set; }
        public string TxnRefNumber { get; set; }
        public string TxnDescription { get; set; }
        public string TxnStatus { get; set; }
        public decimal TxnBalanceAfter { get; set; }
        public string TxnUserId { get; set; }
    }
}