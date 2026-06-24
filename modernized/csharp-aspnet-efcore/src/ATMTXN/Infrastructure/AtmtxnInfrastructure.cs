using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ATMTXN.Domain;
using ATMTXN.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ATMTXN.Infrastructure
{
    public class AtmtxnDbContext : DbContext
    {
        public DbSet<AtmtxnDomainModels.Transaction> Transactions { get; set; }

        public AtmtxnDbContext(DbContextOptions<AtmtxnDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AtmtxnDomainModels.Transaction>(entity =>
            {
                entity.ToTable("transactions");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(12); // COBOL: TRANSACTION-ID PIC X(12)

                entity.Property(e => e.CardNumber)
                    .HasColumnName("card_number")
                    .HasMaxLength(16); // COBOL: CARD-NUMBER PIC X(16)

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasPrecision(15, 2); // COBOL: TRANSACTION-AMOUNT PIC 9(13)V99 COMP-3

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasMaxLength(10); // COBOL: TRANSACTION-DATE PIC X(10)

                entity.Property(e => e.TransactionTime)
                    .HasColumnName("transaction_time")
                    .HasMaxLength(8); // COBOL: TRANSACTION-TIME PIC X(8)

                entity.Property(e => e.CurrencyCode)
                    .HasColumnName("currency_code")
                    .HasMaxLength(3); // COBOL: CURRENCY-CODE PIC X(3)

                entity.Property(e => e.MerchantName)
                    .HasColumnName("merchant_name")
                    .HasMaxLength(20); // COBOL: MERCHANT-NAME PIC X(20)

                entity.Property(e => e.MerchantAddress)
                    .HasColumnName("merchant_address")
                    .HasMaxLength(30); // COBOL: MERCHANT-ADDRESS PIC X(30)

                entity.Property(e => e.MerchantCity)
                    .HasColumnName("merchant_city")
                    .HasMaxLength(15); // COBOL: MERCHANT-CITY PIC X(15)

                entity.Property(e => e.MerchantState)
                    .HasColumnName("merchant_state")
                    .HasMaxLength(2); // COBOL: MERCHANT-STATE PIC X(2)

                entity.Property(e => e.MerchantPostalCode)
                    .HasColumnName("merchant_postal_code")
                    .HasMaxLength(10); // COBOL: MERCHANT-POSTAL-CODE PIC X(10)

                entity.Property(e => e.MerchantCountryCode)
                    .HasColumnName("merchant_country_code")
                    .HasMaxLength(3); // COBOL: MERCHANT-COUNTRY-CODE PIC X(3)

                entity.Property(e => e.TransactionType)
                    .HasColumnName("transaction_type")
                    .HasMaxLength(1); // COBOL: TRANSACTION-TYPE PIC X(1)

                entity.Property(e => e.TransactionStatus)
                    .HasColumnName("transaction_status")
                    .HasMaxLength(1); // COBOL: TRANSACTION-STATUS PIC X(1)

                entity.Property(e => e.ApprovalCode)
                    .HasColumnName("approval_code")
                    .HasMaxLength(6); // COBOL: APPROVAL-CODE PIC X(6)

                entity.Property(e => e.TerminalId)
                    .HasColumnName("terminal_id")
                    .HasMaxLength(15); // COBOL: TERMINAL-ID PIC X(15)

                entity.Property(e => e.AcquirerId)
                    .HasColumnName("acquirer_id")
                    .HasMaxLength(15); // COBOL: ACQUIRER-ID PIC X(15)

                entity.Property(e => e.IssuerId)
                    .HasColumnName("issuer_id")
                    .HasMaxLength(15); // COBOL: ISSUER-ID PIC X(15)

                entity.Property(e => e.AdditionalData)
                    .HasColumnName("additional_data")
                    .HasMaxLength(50); // COBOL: ADDITIONAL-DATA PIC X(50)

                entity.Property(e => e.ReversalFlag)
                    .HasColumnName("reversal_flag")
                    .HasMaxLength(1); // COBOL: REVERSAL-FLAG PIC X(1)

                entity.Property(e => e.ReversalAmount)
                    .HasColumnName("reversal_amount")
                    .HasPrecision(15, 2); // COBOL: REVERSAL-AMOUNT PIC 9(13)V99 COMP-3

                entity.Property(e => e.ReversalDate)
                    .HasColumnName("reversal_date")
                    .HasMaxLength(10); // COBOL: REVERSAL-DATE PIC X(10)

                entity.Property(e => e.ReversalTime)
                    .HasColumnName("reversal_time")
                    .HasMaxLength(8); // COBOL: REVERSAL-TIME PIC X(8)

                entity.Property(e => e.FraudFlag)
                    .HasColumnName("fraud_flag")
                    .HasMaxLength(1); // COBOL: FRAUD-FLAG PIC X(1)

                entity.Property(e => e.DisputeFlag)
                    .HasColumnName("dispute_flag")
                    .HasMaxLength(1); // COBOL: DISPUTE-FLAG PIC X(1)

                entity.Property(e => e.SettlementFlag)
                    .HasColumnName("settlement_flag")
                    .HasMaxLength(1); // COBOL: SETTLEMENT-FLAG PIC X(1)

                entity.Property(e => e.SettlementDate)
                    .HasColumnName("settlement_date")
                    .HasMaxLength(10); // COBOL: SETTLEMENT-DATE PIC X(10)

                entity.Property(e => e.AdjustmentFlag)
                    .HasColumnName("adjustment_flag")
                    .HasMaxLength(1); // COBOL: ADJUSTMENT-FLAG PIC X(1)

                entity.Property(e => e.AdjustmentAmount)
                    .HasColumnName("adjustment_amount")
                    .HasPrecision(15, 2); // COBOL: ADJUSTMENT-AMOUNT PIC 9(13)V99 COMP-3

                entity.Property(e => e.AdjustmentDate)
                    .HasColumnName("adjustment_date")
                    .HasMaxLength(10); // COBOL: ADJUSTMENT-DATE PIC X(10)

                entity.Property(e => e.AdjustmentTime)
                    .HasColumnName("adjustment_time")
                    .HasMaxLength(8); // COBOL: ADJUSTMENT-TIME PIC X(8)

                entity.Property(e => e.ErrorFlag)
                    .HasColumnName("error_flag")
                    .HasMaxLength(1); // COBOL: ERROR-FLAG PIC X(1)

                entity.Property(e => e.ErrorDescription)
                    .HasColumnName("error_description")
                    .HasMaxLength(50); // COBOL: ERROR-DESCRIPTION PIC X(50)

                entity.Property(e => e.RetryFlag)
                    .HasColumnName("retry_flag")
                    .HasMaxLength(1); // COBOL: RETRY-FLAG PIC X(1)

                entity.Property(e => e.RetryDate)
                    .HasColumnName("retry_date")
                    .HasMaxLength(10); // COBOL: RETRY-DATE PIC X(10)

                entity.Property(e => e.RetryTime)
                    .HasColumnName("retry_time")
                    .HasMaxLength(8); // COBOL: RETRY-TIME PIC X(8)

                entity.Property(e => e.BatchFlag)
                    .HasColumnName("batch_flag")
                    .HasMaxLength(1); // COBOL: BATCH-FLAG PIC X(1)

                entity.Property(e => e.BatchDate)
                    .HasColumnName("batch_date")
                    .HasMaxLength(10); // COBOL: BATCH-DATE PIC X(10)

                entity.Property(e => e.BatchTime)
                    .HasColumnName("batch_time")
                    .HasMaxLength(8); // COBOL: BATCH-TIME PIC X(8)

                entity.Property(e => e.OfflineFlag)
                    .HasColumnName("offline_flag")
                    .HasMaxLength(1); // COBOL: OFFLINE-FLAG PIC X(1)

                entity.Property(e => e.ReconciliationFlag)
                    .HasColumnName("reconciliation_flag")
                    .HasMaxLength(1); // COBOL: RECONCILIATION-FLAG PIC X(1)

                entity.Property(e => e.ReconciliationDate)
                    .HasColumnName("reconciliation_date")
                    .HasMaxLength(10); // COBOL: RECONCILIATION-DATE PIC X(10)

                entity.Property(e => e.ReconciliationTime)
                    .HasColumnName("reconciliation_time")
                    .HasMaxLength(8); // COBOL: RECONCILIATION-TIME PIC X(8)

                entity.Property(e => e.ChargebackFlag)
                    .HasColumnName("chargeback_flag")
                    .HasMaxLength(1); // COBOL: CHARGEBACK-FLAG PIC X(1)

                entity.Property(e => e.ChargebackDate)
                    .HasColumnName("chargeback_date")
                    .HasMaxLength(10); // COBOL: CHARGEBACK-DATE PIC X(10)

                entity.Property(e => e.ChargebackTime)
                    .HasColumnName("chargeback_time")
                    .HasMaxLength(8); // COBOL: CHARGEBACK-TIME PIC X(8)

                entity.Property(e => e.RefundFlag)
                    .HasColumnName("refund_flag")
                    .HasMaxLength(1); // COBOL: REFUND-FLAG PIC X(1)

                entity.Property(e => e.RefundAmount)
                    .HasColumnName("refund_amount")
                    .HasPrecision(15, 2); // COBOL: REFUND-AMOUNT PIC 9(13)V99 COMP-3

                entity.Property(e => e.RefundDate)
                    .HasColumnName("refund_date")
                    .HasMaxLength(10); // COBOL: REFUND-DATE PIC X(10)

                entity.Property(e => e.RefundTime)
                    .HasColumnName("refund_time")
                    .HasMaxLength(8); // COBOL: REFUND-TIME PIC X(8)

                entity.Property(e => e.ReversalReasonCode)
                    .HasColumnName("reversal_reason_code")
                    .HasMaxLength(1); // COBOL: REVERSAL-REASON-CODE PIC X(1)

                entity.Property(e => e.AdjustmentReasonCode)
                    .HasColumnName("adjustment_reason_code")
                    .HasMaxLength(1); // COBOL: ADJUSTMENT-REASON-CODE PIC X(1)

                entity.Property(e => e.ErrorReasonCode)
                    .HasColumnName("error_reason_code")
                    .HasMaxLength(1); // COBOL: ERROR-REASON-CODE PIC X(1)

                entity.Property(e => e.RetryReasonCode)
                    .HasColumnName("retry_reason_code")
                    .HasMaxLength(1); // COBOL: RETRY-REASON-CODE PIC X(1)

                entity.Property(e => e.BatchReasonCode)
                    .HasColumnName("batch_reason_code")
                    .HasMaxLength(1); // COBOL: BATCH-REASON-CODE PIC X(1)

                entity.Property(e => e.OfflineReasonCode)
                    .HasColumnName("offline_reason_code")
                    .HasMaxLength(1); // COBOL: OFFLINE-REASON-CODE PIC X(1)

                entity.Property(e => e.ReconciliationReasonCode)
                    .HasColumnName("reconciliation_reason_code")
                    .HasMaxLength(1); // COBOL: RECONCILIATION-REASON-CODE PIC X(1)

                entity.Property(e => e.ChargebackReasonCode)
                    .HasColumnName("chargeback_reason_code")
                    .HasMaxLength(1); // COBOL: CHARGEBACK-REASON-CODE PIC X(1)

                entity.Property(e => e.RefundReasonCode)
                    .HasColumnName("refund_reason_code")
                    .HasMaxLength(1); // COBOL: REFUND-REASON-CODE PIC X(1)
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

        public async Task<AtmtxnDomainModels.Transaction?> GetTransactionByIdAsync(string transactionId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.TransactionId == transactionId);
        }

        public async Task<IEnumerable<AtmtxnDomainModels.Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddTransactionAsync(AtmtxnDomainModels.Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransactionAsync(AtmtxnDomainModels.Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(string transactionId)
        {
            var transaction = await _context.Transactions.SingleOrDefaultAsync(t => t.TransactionId == transactionId);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddAtmtxnInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AtmtxnDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<ITransactionRepository, TransactionRepository>();
        }
    }
}