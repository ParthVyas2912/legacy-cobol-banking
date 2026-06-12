using System;
using System.ComponentModel.DataAnnotations;

namespace TXNPOST.Domain
{
    public class TxnpostDomainModels
    {
        public class Transaction
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            public decimal Amount { get; set; } // PIC 9(7)V99 COMP-3

            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            public DateTime TransactionDate { get; set; } // PIC 9(8) for YYYYMMDD

            public StatusEnum Status { get; set; } // 88-level equivalent

            public NestedGroup AdditionalDetails { get; set; } // GROUP item

            public class NestedGroup
            {
                [MaxLength(20)]
                public string Description { get; set; } // PIC X(20)

                public decimal Fee { get; set; } // PIC 9(5)V99 COMP-3
            }

            public enum TransactionTypeEnum
            {
                Credit, // 88 CREDIT VALUE
                Debit   // 88 DEBIT VALUE
            }

            public enum StatusEnum
            {
                Pending,   // 88 PENDING VALUE
                Completed, // 88 COMPLETED VALUE
                Failed     // 88 FAILED VALUE
            }
        }

        public class Batch
        {
            [MaxLength(8)]
            public string BatchId { get; set; } // PIC X(8)

            public int RecordCount { get; set; } // PIC 9(5)

            public decimal TotalAmount { get; set; } // PIC 9(9)V99 COMP-3

            public Transaction[] Transactions { get; set; } // OCCURS 100 TIMES
        }
    }
}