using System;
using System.ComponentModel.DataAnnotations;

namespace TXNPOST.Domain
{
    public class TxnpostDomainModels
    {
        public class TransactionRecord
        {
            [MaxLength(10)]
            public string TransactionId { get; set; } // PIC X(10)

            [MaxLength(20)]
            public string AccountNumber { get; set; } // PIC X(20)

            [Precision(15, 2)]
            public decimal TransactionAmount { get; set; } // PIC S9(13)V99 COMP-3

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            [RegularExpression(@"\d{8}")]
            public string TransactionDate { get; set; } // PIC 9(8)

            public StatusEnum Status { get; set; } // 88-level equivalent
        }

        public enum TransactionTypeEnum
        {
            Credit, // 88 CREDIT VALUE 'C'
            Debit   // 88 DEBIT VALUE 'D'
        }

        public enum StatusEnum
        {
            Pending,   // 88 PENDING VALUE 'P'
            Completed, // 88 COMPLETED VALUE 'C'
            Failed     // 88 FAILED VALUE 'F'
        }
    }
}