using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATMTXN.Domain
{
    public class AtmtxnDomainModels
    {
        public class Transaction
        {
            [MaxLength(12)] // PIC X(12)
            public string TransactionId { get; set; } = string.Empty;

            [MaxLength(16)] // PIC X(16)
            public string CardNumber { get; set; } = string.Empty;

            [Column(TypeName = "decimal(15,2)")] // PIC 9(13)V99 COMP-3
            public decimal TransactionAmount { get; set; }

            [MaxLength(10)] // PIC X(10)
            public string TransactionDate { get; set; } = string.Empty;

            [MaxLength(8)] // PIC X(8)
            public string TransactionTime { get; set; } = string.Empty;

            [MaxLength(3)] // PIC X(3)
            public string CurrencyCode { get; set; } = string.Empty;

            [MaxLength(20)] // PIC X(20)
            public string MerchantName { get; set; } = string.Empty;

            [MaxLength(30)] // PIC X(30)
            public string MerchantAddress { get; set; } = string.Empty;

            [MaxLength(15)] // PIC X(15)
            public string MerchantCity { get; set; } = string.Empty;

            [MaxLength(2)] // PIC X(2)
            public string MerchantState { get; set; } = string.Empty;

            [MaxLength(10)] // PIC X(10)
            public string MerchantPostalCode { get; set; } = string.Empty;

            [MaxLength(3)] // PIC X(3)
            public string MerchantCountryCode { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string TransactionType { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string TransactionStatus { get; set; } = string.Empty;

            [MaxLength(6)] // PIC X(6)
            public string ApprovalCode { get; set; } = string.Empty;

            [MaxLength(15)] // PIC X(15)
            public string TerminalId { get; set; } = string.Empty;

            [MaxLength(15)] // PIC X(15)
            public string AcquirerId { get; set; } = string.Empty;

            [MaxLength(15)] // PIC X(15)
            public string IssuerId { get; set; } = string.Empty;

            [MaxLength(50)] // PIC X(50)
            public string AdditionalData { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string ReversalFlag { get; set; } = string.Empty;

            [Column(TypeName = "decimal(15,2)")] // PIC 9(13)V99 COMP-3
            public decimal ReversalAmount { get; set; }

            [MaxLength(10)] // PIC X(10)
            public string ReversalDate { get; set; } = string.Empty;

            [MaxLength(8)] // PIC X(8)
            public string ReversalTime { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string FraudFlag { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string DisputeFlag { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string SettlementFlag { get; set; } = string.Empty;

            [MaxLength(10)] // PIC X(10)
            public string SettlementDate { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string AdjustmentFlag { get; set; } = string.Empty;

            [Column(TypeName = "decimal(15,2)")] // PIC 9(13)V99 COMP-3
            public decimal AdjustmentAmount { get; set; }

            [MaxLength(10)] // PIC X(10)
            public string AdjustmentDate { get; set; } = string.Empty;

            [MaxLength(8)] // PIC X(8)
            public string AdjustmentTime { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string ErrorFlag { get; set; } = string.Empty;

            [MaxLength(50)] // PIC X(50)
            public string ErrorDescription { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string RetryFlag { get; set; } = string.Empty;

            [MaxLength(10)] // PIC X(10)
            public string RetryDate { get; set; } = string.Empty;

            [MaxLength(8)] // PIC X(8)
            public string RetryTime { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string BatchFlag { get; set; } = string.Empty;

            [MaxLength(10)] // PIC X(10)
            public string BatchDate { get; set; } = string.Empty;

            [MaxLength(8)] // PIC X(8)
            public string BatchTime { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string OfflineFlag { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string ReconciliationFlag { get; set; } = string.Empty;

            [MaxLength(10)] // PIC X(10)
            public string ReconciliationDate { get; set; } = string.Empty;

            [MaxLength(8)] // PIC X(8)
            public string ReconciliationTime { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string ChargebackFlag { get; set; } = string.Empty;

            [MaxLength(10)] // PIC X(10)
            public string ChargebackDate { get; set; } = string.Empty;

            [MaxLength(8)] // PIC X(8)
            public string ChargebackTime { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string RefundFlag { get; set; } = string.Empty;

            [Column(TypeName = "decimal(15,2)")] // PIC 9(13)V99 COMP-3
            public decimal RefundAmount { get; set; }

            [MaxLength(10)] // PIC X(10)
            public string RefundDate { get; set; } = string.Empty;

            [MaxLength(8)] // PIC X(8)
            public string RefundTime { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string ReversalReasonCode { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string AdjustmentReasonCode { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string ErrorReasonCode { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string RetryReasonCode { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string BatchReasonCode { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string OfflineReasonCode { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string ReconciliationReasonCode { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string ChargebackReasonCode { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string RefundReasonCode { get; set; } = string.Empty;
        }
    }
}