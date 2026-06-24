using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Added for PrecisionAttribute

namespace TXNPOST.Domain
{
    public class TxnpostDomainModels
    {
        public class Transaction
        {
            [MaxLength(12)] // PIC X(12)
            public string TransactionId { get; set; } = string.Empty;

            [MaxLength(10)] // PIC X(10)
            public string AccountNumber { get; set; } = string.Empty;

            [Column(TypeName = "decimal(15,2)")] // Replacing Precision with Column attribute
            public decimal TransactionAmount { get; set; } = 0;

            [MaxLength(8)] // PIC X(8)
            public string TransactionDate { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string TransactionType { get; set; } = string.Empty;

            public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

            public List<TransactionDetail> Details { get; set; } = new List<TransactionDetail>();
        }

        public class TransactionDetail
        {
            [MaxLength(12)] // PIC X(12)
            public string DetailId { get; set; } = string.Empty;

            [Column(TypeName = "decimal(13,2)")] // Replacing Precision with Column attribute
            public decimal DetailAmount { get; set; } = 0;

            [MaxLength(20)] // PIC X(20)
            public string Description { get; set; } = string.Empty;
        }

        public enum TransactionStatus
        {
            Pending, // 88 PENDING VALUE 'P'
            Completed, // 88 COMPLETED VALUE 'C'
            Failed // 88 FAILED VALUE 'F'
        }
    }
}