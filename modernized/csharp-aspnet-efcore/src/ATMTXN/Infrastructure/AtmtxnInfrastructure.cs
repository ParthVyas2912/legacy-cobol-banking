using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ATMTXN.Domain;
using ATMTXN.Application.Interfaces;

namespace ATMTXN.Infrastructure
{
    public class AtmtxnDbContext : DbContext
    {
        public DbSet<AtmtxnDomainModels.Transaction> Transactions { get; set; }

        public AtmtxnDbContext(DbContextOptions<AtmtxnDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AtmtxnDomainModels.Transaction>(entity =>
            {
                entity.ToTable("transactions");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(12); // COBOL: TRANSACTION-ID PIC X(12)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(11, 2); // COBOL: TRANSACTION-AMOUNT PIC 9(9)V99 COMP-3

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasColumnType("date"); // COBOL: TRANSACTION-DATE PIC 9(8)

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .HasConversion<int>(); // Enum stored as integer

                entity.OwnsOne(e => e.Account, account =>
                {
                    account.Property(a => a.AccountNumber)
                        .HasColumnName("account_number")
                        .HasMaxLength(10); // COBOL: ACCOUNT-NUMBER PIC X(10)

                    account.Property(a => a.AccountHolderName)
                        .HasColumnName("account_holder_name")
                        .HasMaxLength(20); // COBOL: ACCOUNT-HOLDER-NAME PIC X(20)

                    account.Property(a => a.AccountBalance)
                        .HasColumnName("account_balance")
                        .HasPrecision(13, 2); // COBOL: ACCOUNT-BALANCE PIC 9(11)V99 COMP-3
                });
            });
        }
    }

    public class AtmtxnRepository : IAtmtxnRepository
    {
        private readonly AtmtxnDbContext _dbContext;

        public AtmtxnRepository(AtmtxnDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AtmtxnDomainModels.Transaction ReadTransaction(string transactionId)
        {
            return _dbContext.Transactions
                .AsNoTracking()
                .FirstOrDefault(t => t.TransactionId == transactionId);
        }

        public void WriteTransaction(AtmtxnDomainModels.Transaction transaction)
        {
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
        }

        public void UpdateAccountBalance(string accountNumber, decimal newBalance)
        {
            var transaction = _dbContext.Transactions
                .Include(t => t.Account)
                .FirstOrDefault(t => t.Account.AccountNumber == accountNumber);

            if (transaction != null)
            {
                transaction.Account.AccountBalance = newBalance;
                _dbContext.SaveChanges();
            }
        }

        public AtmtxnDomainModels.Transaction.AccountDetails GetAccountDetails(string accountNumber)
        {
            var transaction = _dbContext.Transactions
                .AsNoTracking()
                .FirstOrDefault(t => t.Account.AccountNumber == accountNumber);

            return transaction?.Account;
        }
    }

    public class AtmtxnCicsService : IAtmtxnCicsService
    {
        public bool SendTransactionRequest(AtmtxnDomainModels.Transaction transaction)
        {
            // Simulate sending a transaction request to the CICS system
            // In a real implementation, this would involve communication with the CICS system
            return true;
        }

        public string ReceiveCicsResponse()
        {
            // Simulate receiving a response from the CICS system
            // In a real implementation, this would involve communication with the CICS system
            return "CICS response received successfully.";
        }
    }
}