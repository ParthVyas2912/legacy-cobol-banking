using System;
using System.ComponentModel.DataAnnotations;

namespace TXNPOST.Domain
{
    public class TxnpostDomainModels
    {
        public class TransactionRecord
        {
            [Required]
            [MaxLength(10)]
            public string TransactionId { get; set; } // PIC X(10)

            [Required]
            [MaxLength(20)]
            public string AccountNumber { get; set; } // PIC X(20)

            [Required]
            [Precision(15, 2)]
            public decimal TransactionAmount { get; set; } // PIC S9(13)V99 COMP-3

            [Required]
            [MaxLength(8)]
            public string TransactionDate { get; set; } // PIC X(8)

            [Required]
            [MaxLength(6)]
            public string TransactionTime { get; set; } // PIC X(6)

            [Required]
            [MaxLength(3)]
            public string CurrencyCode { get; set; } // PIC X(3)

            [Required]
            [MaxLength(1)]
            public string TransactionType { get; set; } // PIC X(1)

            [Required]
            [MaxLength(30)]
            public string Description { get; set; } // PIC X(30)

            [Required]
            [Precision(15, 2)]
            public decimal BalanceAfterTransaction { get; set; } // PIC S9(13)V99 COMP-3

            [Required]
            public TransactionStatus Status { get; set; } // 88-level equivalent

            [Required]
            public TransactionFlags Flags { get; set; } // GROUP item equivalent
        }

        public enum TransactionStatus
        {
            Pending, // 88 PENDING VALUE 'P'
            Completed, // 88 COMPLETED VALUE 'C'
            Failed // 88 FAILED VALUE 'F'
        }

        public class TransactionFlags
        {
            [Required]
            public bool IsReversed { get; set; } // 88 REVERSED VALUE 'Y'

            [Required]
            public bool IsInternational { get; set; } // 88 INTERNATIONAL VALUE 'Y'

            [Required]
            public bool IsFraudulent { get; set; } // 88 FRAUDULENT VALUE 'Y'
        }
    }
}