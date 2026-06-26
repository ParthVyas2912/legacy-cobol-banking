using System;
using System.Collections.Generic;
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

            public decimal TransactionAmount { get; set; } // PIC S9(13)V99 COMP-3

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            public StatusEnum Status { get; set; } // 88-level equivalent

            public List<DetailRecord> Details { get; set; } // OCCURS 10 TIMES

            public class DetailRecord
            {
                [MaxLength(15)]
                public string DetailId { get; set; } // PIC X(15)

                public decimal Amount { get; set; } // PIC S9(11)V99 COMP-3

                [MaxLength(30)]
                public string Description { get; set; } // PIC X(30)
            }
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
}