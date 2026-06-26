using System;
using System.ComponentModel.DataAnnotations;

namespace TXNPOST.Domain
{
    public class TxnpostDomainModels
    {
        public class TransactionRecord
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            public decimal TransactionAmount { get; set; } // PIC 9(7)V99 COMP-3

            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            public DateTime TransactionDate { get; set; } // PIC 9(8) (YYYYMMDD)

            public TimeSpan TransactionTime { get; set; } // PIC 9(6) (HHMMSS)

            public StatusEnum Status { get; set; } // 88-level equivalent

            public NestedGroup AdditionalDetails { get; set; } // GROUP item

            public class NestedGroup
            {
                [MaxLength(20)]
                public string Description { get; set; } // PIC X(20)

                public decimal FeeAmount { get; set; } // PIC 9(5)V99 COMP-3
            }
        }

        public enum TransactionTypeEnum
        {
            Debit, // 88 DEBIT VALUE 'D'
            Credit // 88 CREDIT VALUE 'C'
        }

        public enum StatusEnum
        {
            Pending, // 88 PENDING VALUE 'P'
            Completed, // 88 COMPLETED VALUE 'C'
            Failed // 88 FAILED VALUE 'F'
        }
    }
}