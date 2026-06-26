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

            public decimal TransactionAmount { get; set; } // PIC 9(7)V99 COMP-3

            public TransactionTypeEnum TransactionType { get; set; } // PIC X(1)

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            public AccountDetails Account { get; set; }

            public class AccountDetails
            {
                [MaxLength(15)]
                public string AccountNumber { get; set; } // PIC X(15)

                public decimal AccountBalance { get; set; } // PIC 9(10)V99 COMP-3
            }

            public enum TransactionTypeEnum
            {
                Credit = 1, // 88-level for Credit
                Debit = 2   // 88-level for Debit
            }
        }

        public class BatchRecord
        {
            [MaxLength(6)]
            public string BatchId { get; set; } // PIC X(6)

            public int RecordCount { get; set; } // PIC 9(5)

            public TransactionRecord[] Transactions { get; set; } // OCCURS 100 TIMES
        }
    }
}