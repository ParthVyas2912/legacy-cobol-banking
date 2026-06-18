using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using STMTRPT.Application.Interfaces;
using STMTRPT.Domain;

namespace STMTRPT.Infrastructure
{
    public class StmtrptDbContext : DbContext
    {
        public DbSet<Stmtout> Stmtouts { get; set; }

        public StmtrptDbContext(DbContextOptions<StmtrptDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stmtout>(entity =>
            {
                entity.ToTable("stmtout", "public");

                entity.Property(e => e.StmtAcctNum)
                    .HasColumnName("stmt_acct_num")
                    .HasMaxLength(10) // COBOL: STMT-ACCT-NUM PIC 9(10)
                    .IsRequired();

                entity.Property(e => e.StmtDescription)
                    .HasColumnName("stmt_description")
                    .HasMaxLength(40) // COBOL: STMT-DESCRIPTION PIC X(40)
                    .IsRequired();

                entity.Property(e => e.StmtAmount)
                    .HasColumnName("stmt_amount")
                    .HasPrecision(13, 2); // COBOL: STMT-AMOUNT PIC -Z(11)9.99

                entity.Property(e => e.StmtBalance)
                    .HasColumnName("stmt_balance")
                    .HasPrecision(15, 2); // COBOL: STMT-BALANCE PIC -Z(13)9.99
            });
        }
    }

    public class Stmtout
    {
        public string StmtAcctNum { get; set; } // COBOL: STMT-ACCT-NUM PIC 9(10)
        public string StmtDescription { get; set; } // COBOL: STMT-DESCRIPTION PIC X(40)
        public decimal StmtAmount { get; set; } // COBOL: STMT-AMOUNT PIC -Z(11)9.99
        public decimal StmtBalance { get; set; } // COBOL: STMT-BALANCE PIC -Z(13)9.99
    }

    public class StmtrptRepository : IStmtrptRepository
    {
        private readonly StmtrptDbContext _context;

        public StmtrptRepository(StmtrptDbContext context)
        {
            _context = context;
        }

        public List<StmtrptDomainModels.AccountSummary> ReadAccountSummaries()
        {
            var stmtouts = _context.Stmtouts.ToList();
            var accountSummaries = new List<StmtrptDomainModels.AccountSummary>();

            foreach (var stmtout in stmtouts)
            {
                accountSummaries.Add(new StmtrptDomainModels.AccountSummary
                {
                    AccountNumber = stmtout.StmtAcctNum,
                    AccountName = stmtout.StmtDescription,
                    Balance = stmtout.StmtBalance,
                    Status = StmtrptDomainModels.AccountStatus.Active // Defaulting to Active as no status mapping is provided
                });
            }

            return accountSummaries;
        }

        public void WriteAccountSummaries(List<StmtrptDomainModels.AccountSummary> accountSummaries)
        {
            foreach (var accountSummary in accountSummaries)
            {
                var stmtout = new Stmtout
                {
                    StmtAcctNum = accountSummary.AccountNumber,
                    StmtDescription = accountSummary.AccountName,
                    StmtBalance = accountSummary.Balance,
                    StmtAmount = 0 // Assuming no direct mapping for amount in AccountSummary
                };

                _context.Stmtouts.Add(stmtout);
            }

            _context.SaveChanges();
        }

        public List<StmtrptDomainModels.TransactionDetail> ReadTransactionDetails(string accountNumber)
        {
            // Assuming transactions are stored in the same table for simplicity
            var stmtouts = _context.Stmtouts.Where(s => s.StmtAcctNum == accountNumber).ToList();
            var transactionDetails = new List<StmtrptDomainModels.TransactionDetail>();

            foreach (var stmtout in stmtouts)
            {
                transactionDetails.Add(new StmtrptDomainModels.TransactionDetail
                {
                    TransactionDate = "000000", // Placeholder as no date mapping is provided
                    TransactionDescription = stmtout.StmtDescription,
                    TransactionAmount = stmtout.StmtAmount,
                    Type = stmtout.StmtAmount < 0 ? StmtrptDomainModels.TransactionType.Debit : StmtrptDomainModels.TransactionType.Credit
                });
            }

            return transactionDetails;
        }

        public void WriteTransactionDetails(string accountNumber, List<StmtrptDomainModels.TransactionDetail> transactionDetails)
        {
            foreach (var transaction in transactionDetails)
            {
                var stmtout = new Stmtout
                {
                    StmtAcctNum = accountNumber,
                    StmtDescription = transaction.TransactionDescription,
                    StmtAmount = transaction.TransactionAmount,
                    StmtBalance = 0 // Assuming balance is calculated elsewhere
                };

                _context.Stmtouts.Add(stmtout);
            }

            _context.SaveChanges();
        }
    }
}