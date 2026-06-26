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

            public TransactionTypeEnum TransactionType { get; set; } // PIC X(1)

            public DateTime TransactionDate { get; set; } // PIC 9(8)

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
            Credit = 'C', // PIC X(1) value 'C'
            Debit = 'D'   // PIC X(1) value 'D'
        }

        public enum StatusEnum
        {
            Pending = 0, // 88-level equivalent for pending
            Completed = 1 // 88-level equivalent for completed
        }
    }
}