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

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            public StatusEnum Status { get; set; } // 88-level equivalent

            public TransactionDetails Details { get; set; } // GROUP item
        }

        public class TransactionDetails
        {
            [MaxLength(50)]
            public string Description { get; set; } // PIC X(50)

            [MaxLength(3)]
            public string CurrencyCode { get; set; } // PIC X(3)

            [Precision(9, 4)]
            public decimal ExchangeRate { get; set; } // PIC S9(5)V9(4) COMP-3
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