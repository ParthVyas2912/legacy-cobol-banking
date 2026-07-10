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
        public DbSet<TransactionRecordEntity> TransactionRecords { get; set; }
        public DbSet<FeeDetailsEntity> FeeDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionRecordEntity>(entity =>
            {
                entity.ToTable("transaction_records");

                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(12); // COBOL: TRANSACTION-ID PIC X(12)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(9, 2); // COBOL: TRANSACTION-AMOUNT PIC 9(7)V99 COMP-3

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .IsRequired(); // Ensures non-nullable, as COBOL doesn't allow nulls for this field

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasColumnType("date"); // COBOL: TRANSACTION-DATE PIC 9(8)

                entity.OwnsOne(e => e.Account, account =>
                {
                    account.Property(a => a.AccountNumber)
                        .HasColumnName("account_number")
                        .HasMaxLength(10); // COBOL: ACCOUNT-NUMBER PIC X(10)

                    account.Property(a => a.AccountHolderName)
                        .HasColumnName("account_holder_name")
                        .HasMaxLength(30); // COBOL: ACCOUNT-HOLDER-NAME PIC X(30)
                });

                entity.HasMany(e => e.Fees)
                    .WithOne()
                    .HasForeignKey("TransactionId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FeeDetailsEntity>(entity =>
            {
                entity.ToTable("fee_details");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .IsRequired(); // Ensures non-nullable, as COBOL doesn't allow nulls for this field

                entity.Property(e => e.FeeAmount)
                    .HasColumnName("fee_amount")
                    .HasPrecision(7, 2); // COBOL: FEE-AMOUNT PIC 9(5)V99 COMP-3

                entity.Property(e => e.FeeDescription)
                    .HasColumnName("fee_description")
                    .HasMaxLength(20); // COBOL: FEE-DESCRIPTION PIC X(20)
            });
        }
    }

    public class TransactionRecordEntity
    {
        public string TransactionId { get; set; } // COBOL: TRANSACTION-ID PIC X(12)
        public decimal TransactionAmount { get; set; } // COBOL: TRANSACTION-AMOUNT PIC 9(7)V99 COMP-3
        public int TransactionType { get; set; } // COBOL: TRANSACTION-TYPE (88-level equivalent)
        public DateTime TransactionDate { get; set; } // COBOL: TRANSACTION-DATE PIC 9(8)
        public AccountDetails Account { get; set; } // COBOL: GROUP item
        public List<FeeDetailsEntity> Fees { get; set; } = new List<FeeDetailsEntity>(); // COBOL: OCCURS 5 TIMES
    }

    public class FeeDetailsEntity
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public decimal FeeAmount { get; set; } // COBOL: FEE-AMOUNT PIC 9(5)V99 COMP-3
        public string FeeDescription { get; set; } // COBOL: FEE-DESCRIPTION PIC X(20)
    }

    public class AccountDetails
    {
        public string AccountNumber { get; set; } // COBOL: ACCOUNT-NUMBER PIC X(10)
        public string AccountHolderName { get; set; } // COBOL: ACCOUNT-HOLDER-NAME PIC X(30)
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AtmtxnDbContext _context;

        public TransactionRepository(AtmtxnDbContext context)
        {
            _context = context;
        }

        public AtmtxnDomainModels.TransactionRecord ReadTransaction(string transactionId)
        {
            var entity = _context.TransactionRecords
                .Include(t => t.Fees)
                .FirstOrDefault(t => t.TransactionId == transactionId);

            if (entity == null) return null;

            return MapToDomain(entity);
        }

        public void WriteTransaction(AtmtxnDomainModels.TransactionRecord transaction)
        {
            var entity = MapToEntity(transaction);
            _context.TransactionRecords.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateTransaction(AtmtxnDomainModels.TransactionRecord transaction)
        {
            var entity = MapToEntity(transaction);
            _context.TransactionRecords.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteTransaction(string transactionId)
        {
            var entity = _context.TransactionRecords
                .Include(t => t.Fees)
                .FirstOrDefault(t => t.TransactionId == transactionId);

            if (entity != null)
            {
                _context.TransactionRecords.Remove(entity);
                _context.SaveChanges();
            }
        }

        private AtmtxnDomainModels.TransactionRecord MapToDomain(TransactionRecordEntity entity)
        {
            return new AtmtxnDomainModels.TransactionRecord
            {
                TransactionId = entity.TransactionId,
                TransactionAmount = entity.TransactionAmount,
                TransactionType = (AtmtxnDomainModels.TransactionRecord.TransactionTypeEnum)entity.TransactionType,
                TransactionDate = entity.TransactionDate,
                Account = new AtmtxnDomainModels.TransactionRecord.AccountDetails
                {
                    AccountNumber = entity.Account.AccountNumber,
                    AccountHolderName = entity.Account.AccountHolderName
                },
                Fees = entity.Fees.Select(f => new AtmtxnDomainModels.TransactionRecord.FeeDetails
                {
                    FeeAmount = f.FeeAmount,
                    FeeDescription = f.FeeDescription
                }).ToList()
            };
        }

        private TransactionRecordEntity MapToEntity(AtmtxnDomainModels.TransactionRecord domain)
        {
            return new TransactionRecordEntity
            {
                TransactionId = domain.TransactionId,
                TransactionAmount = domain.TransactionAmount,
                TransactionType = (int)domain.TransactionType,
                TransactionDate = domain.TransactionDate,
                Account = new AccountDetails
                {
                    AccountNumber = domain.Account.AccountNumber,
                    AccountHolderName = domain.Account.AccountHolderName
                },
                Fees = domain.Fees.Select(f => new FeeDetailsEntity
                {
                    FeeAmount = f.FeeAmount,
                    FeeDescription = f.FeeDescription
                }).ToList()
            };
        }
    }
}