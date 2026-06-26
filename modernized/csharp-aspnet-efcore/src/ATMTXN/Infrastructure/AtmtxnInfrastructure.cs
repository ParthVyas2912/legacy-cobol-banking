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
        public DbSet<AtmtxnDomainModels.TransactionRecord> Transactions { get; set; }
        public DbSet<AtmtxnDomainModels.CustomerRecord> Customers { get; set; }
        public DbSet<AtmtxnDomainModels.AtmMachineRecord> AtmMachines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AtmtxnDomainModels.TransactionRecord>(entity =>
            {
                entity.ToTable("transactions");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .IsRequired(); // COBOL: TransactionId PIC 9(9)

                entity.Property(e => e.AccountNumber)
                    .HasColumnName("account_number")
                    .HasMaxLength(20)
                    .IsRequired(); // COBOL: AccountNumber PIC X(20)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(15, 2)
                    .IsRequired(); // COBOL: TransactionAmount PIC S9(13)V99 COMP-3

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .HasMaxLength(10)
                    .IsRequired(); // COBOL: TransactionType PIC X(10)

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .IsRequired(); // COBOL: TransactionDate PIC 9(8)

                entity.Property(e => e.TransactionTime)
                    .HasColumnName("transaction_time")
                    .IsRequired(); // COBOL: TransactionTime PIC 9(6)

                entity.Property(e => e.MerchantName)
                    .HasColumnName("merchant_name")
                    .HasMaxLength(50); // COBOL: MerchantName PIC X(50)

                entity.Property(e => e.MerchantLocation)
                    .HasColumnName("merchant_location")
                    .HasMaxLength(30); // COBOL: MerchantLocation PIC X(30)

                entity.Property(e => e.CurrencyCode)
                    .HasColumnName("currency_code")
                    .HasMaxLength(3)
                    .IsRequired(); // COBOL: CurrencyCode PIC X(3)

                entity.Property(e => e.AvailableBalance)
                    .HasColumnName("available_balance")
                    .HasPrecision(15, 2); // COBOL: AvailableBalance PIC S9(13)V99 COMP-3

                entity.Property(e => e.LedgerBalance)
                    .HasColumnName("ledger_balance")
                    .HasPrecision(15, 2); // COBOL: LedgerBalance PIC S9(13)V99 COMP-3

                entity.Property(e => e.TransactionStatus)
                    .HasColumnName("transaction_status")
                    .HasMaxLength(1)
                    .IsRequired(); // COBOL: TransactionStatus PIC X(1)

                entity.Property(e => e.Remarks)
                    .HasColumnName("remarks")
                    .HasMaxLength(100); // COBOL: Remarks PIC X(100)
            });

            modelBuilder.Entity<AtmtxnDomainModels.CustomerRecord>(entity =>
            {
                entity.ToTable("customers");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasMaxLength(20)
                    .IsRequired(); // COBOL: CustomerId PIC X(20)

                entity.Property(e => e.CustomerName)
                    .HasColumnName("customer_name")
                    .HasMaxLength(50)
                    .IsRequired(); // COBOL: CustomerName PIC X(50)

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(100); // COBOL: Address PIC X(100)

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(15); // COBOL: PhoneNumber PIC X(15)

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50); // COBOL: Email PIC X(50)

                entity.Property(e => e.CustomerStatus)
                    .HasColumnName("customer_status")
                    .HasMaxLength(1)
                    .IsRequired(); // COBOL: CustomerStatus PIC X(1)
            });

            modelBuilder.Entity<AtmtxnDomainModels.AtmMachineRecord>(entity =>
            {
                entity.ToTable("atm_machines");

                entity.Property(e => e.AtmId)
                    .HasColumnName("atm_id")
                    .HasMaxLength(10)
                    .IsRequired(); // COBOL: AtmId PIC X(10)

                entity.Property(e => e.Location)
                    .HasColumnName("location")
                    .HasMaxLength(50)
                    .IsRequired(); // COBOL: Location PIC X(50)

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(1)
                    .IsRequired(); // COBOL: Status PIC X(1)

                entity.Property(e => e.CashAvailable)
                    .HasColumnName("cash_available")
                    .HasPrecision(15, 2)
                    .IsRequired(); // COBOL: CashAvailable PIC S9(13)V99 COMP-3
            });
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AtmtxnDbContext _context;

        public TransactionRepository(AtmtxnDbContext context)
        {
            _context = context;
        }

        public AtmtxnDomainModels.TransactionRecord GetTransactionById(int transactionId)
        {
            return _context.Transactions.Find(transactionId);
        }

        public void SaveTransaction(AtmtxnDomainModels.TransactionRecord transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public List<AtmtxnDomainModels.TransactionRecord> GetAllTransactions()
        {
            return _context.Transactions.ToList();
        }

        public void DeleteTransaction(int transactionId)
        {
            var transaction = _context.Transactions.Find(transactionId);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                _context.SaveChanges();
            }
        }
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly AtmtxnDbContext _context;

        public CustomerRepository(AtmtxnDbContext context)
        {
            _context = context;
        }

        public AtmtxnDomainModels.CustomerRecord GetCustomerById(string customerId)
        {
            return _context.Customers.Find(customerId);
        }

        public void SaveCustomer(AtmtxnDomainModels.CustomerRecord customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public List<AtmtxnDomainModels.CustomerRecord> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public void DeleteCustomer(string customerId)
        {
            var customer = _context.Customers.Find(customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }
    }

    public class AtmMachineRepository : IAtmMachineRepository
    {
        private readonly AtmtxnDbContext _context;

        public AtmMachineRepository(AtmtxnDbContext context)
        {
            _context = context;
        }

        public AtmtxnDomainModels.AtmMachineRecord GetAtmById(string atmId)
        {
            return _context.AtmMachines.Find(atmId);
        }

        public void SaveAtm(AtmtxnDomainModels.AtmMachineRecord atmMachine)
        {
            _context.AtmMachines.Add(atmMachine);
            _context.SaveChanges();
        }

        public List<AtmtxnDomainModels.AtmMachineRecord> GetAllAtms()
        {
            return _context.AtmMachines.ToList();
        }

        public void DeleteAtm(string atmId)
        {
            var atm = _context.AtmMachines.Find(atmId);
            if (atm != null)
            {
                _context.AtmMachines.Remove(atm);
                _context.SaveChanges();
            }
        }
    }
}