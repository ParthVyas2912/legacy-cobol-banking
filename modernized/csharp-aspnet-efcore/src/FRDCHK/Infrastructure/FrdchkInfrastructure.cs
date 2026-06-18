using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FRDCHK.Application.Interfaces;
using FRDCHK.Domain;

namespace FRDCHK.Infrastructure
{
    public class FrdchkDbContext : DbContext
    {
        public DbSet<TxnLedger> TxnLedgers { get; set; }
        public DbSet<FraudFlags> FraudFlags { get; set; }

        public FrdchkDbContext(DbContextOptions<FrdchkDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TxnLedger>(entity =>
            {
                entity.ToTable("txn_ledger", "public");

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: COUNT PIC X(30)

                entity.Property(e => e.Coalesce)
                    .HasColumnName("coalesce")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: COALESCE PIC X(30)

                entity.Property(e => e.WsVelocityCount)
                    .HasColumnName("ws_velocity_count")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: WS_VELOCITY_COUNT PIC X(30)

                entity.Property(e => e.WsVelocityAmt)
                    .HasColumnName("ws_velocity_amt")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: WS_VELOCITY_AMT PIC X(30)

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: TXN_ACCT_NUMBER PIC X(30)

                entity.Property(e => e.WsCurrentAcct)
                    .HasColumnName("ws_current_acct")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: WS_CURRENT_ACCT PIC X(30)

                entity.Property(e => e.TxnId)
                    .HasColumnName("txn_id")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: TXN_ID PIC X(30)

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: ACCT PIC X(30)

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: AMOUNT PIC X(30)

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: CODE PIC X(30)

                entity.Property(e => e.PostedAt)
                    .HasColumnName("posted_at")
                    .HasColumnType("TIMESTAMPTZ");
                // COBOL: POSTED_AT PIC TIMESTAMPTZ

                entity.Property(e => e.TxnAmount)
                    .HasColumnName("txn_amount")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: TXN_AMOUNT PIC X(30)

                entity.Property(e => e.TxnCode)
                    .HasColumnName("txn_code")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: TXN_CODE PIC X(30)
            });

            modelBuilder.Entity<FraudFlags>(entity =>
            {
                entity.ToTable("fraud_flags", "public");

                entity.Property(e => e.Acct)
                    .HasColumnName("acct")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: ACCT PIC X(30)

                entity.Property(e => e.Score)
                    .HasColumnName("score")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: SCORE PIC X(30)

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: REASON PIC X(30)

                entity.Property(e => e.Ts)
                    .HasColumnName("ts")
                    .HasColumnType("TIMESTAMPTZ");
                // COBOL: TS PIC TIMESTAMPTZ

                entity.Property(e => e.TxnAcctNumber)
                    .HasColumnName("txn_acct_number")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: TXN_ACCT_NUMBER PIC X(30)

                entity.Property(e => e.WsRiskScore)
                    .HasColumnName("ws_risk_score")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(30);
                // COBOL: WS_RISK_SCORE PIC X(30)
            });
        }
    }

    public class TxnLedger
    {
        public string Count { get; set; }
        public string Coalesce { get; set; }
        public string WsVelocityCount { get; set; }
        public string WsVelocityAmt { get; set; }
        public string TxnAcctNumber { get; set; }
        public string WsCurrentAcct { get; set; }
        public string TxnId { get; set; }
        public string Acct { get; set; }
        public string Amount { get; set; }
        public string Code { get; set; }
        public DateTime? PostedAt { get; set; }
        public string TxnAmount { get; set; }
        public string TxnCode { get; set; }
    }

    public class FraudFlags
    {
        public string Acct { get; set; }
        public string Score { get; set; }
        public string Reason { get; set; }
        public DateTime? Ts { get; set; }
        public string TxnAcctNumber { get; set; }
        public string WsRiskScore { get; set; }
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly FrdchkDbContext _context;

        public CustomerRepository(FrdchkDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FrdchkDomainModels.CustomerRecord>> GetAllCustomersAsync()
        {
            return await _context.Set<FrdchkDomainModels.CustomerRecord>().ToListAsync();
        }

        public async Task<FrdchkDomainModels.CustomerRecord> GetCustomerByIdAsync(string customerId)
        {
            return await _context.Set<FrdchkDomainModels.CustomerRecord>()
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task AddCustomerAsync(FrdchkDomainModels.CustomerRecord customerRecord)
        {
            await _context.Set<FrdchkDomainModels.CustomerRecord>().AddAsync(customerRecord);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(FrdchkDomainModels.CustomerRecord customerRecord)
        {
            _context.Set<FrdchkDomainModels.CustomerRecord>().Update(customerRecord);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(string customerId)
        {
            var customer = await _context.Set<FrdchkDomainModels.CustomerRecord>()
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (customer != null)
            {
                _context.Set<FrdchkDomainModels.CustomerRecord>().Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly FrdchkDbContext _context;

        public AccountRepository(FrdchkDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FrdchkDomainModels.Account>> GetAccountsByCustomerIdAsync(string customerId)
        {
            return await _context.Set<FrdchkDomainModels.Account>()
                .Where(a => a.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task AddAccountAsync(string customerId, FrdchkDomainModels.Account account)
        {
            account.CustomerId = customerId;
            await _context.Set<FrdchkDomainModels.Account>().AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(string customerId, FrdchkDomainModels.Account account)
        {
            var existingAccount = await _context.Set<FrdchkDomainModels.Account>()
                .FirstOrDefaultAsync(a => a.CustomerId == customerId && a.AccountNumber == account.AccountNumber);
            if (existingAccount != null)
            {
                existingAccount.Balance = account.Balance;
                existingAccount.AccountType = account.AccountType;
                _context.Set<FrdchkDomainModels.Account>().Update(existingAccount);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAccountAsync(string customerId, string accountNumber)
        {
            var account = await _context.Set<FrdchkDomainModels.Account>()
                .FirstOrDefaultAsync(a => a.CustomerId == customerId && a.AccountNumber == accountNumber);
            if (account != null)
            {
                _context.Set<FrdchkDomainModels.Account>().Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class CicsService : ICicsService
    {
        public async Task<string> CallCicsTransactionAsync(string transactionCode, Dictionary<string, string> parameters)
        {
            // Simulate a CICS transaction call
            await Task.Delay(100); // Simulate network latency
            return $"Transaction {transactionCode} executed with parameters: {string.Join(", ", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"))}";
        }
    }
}