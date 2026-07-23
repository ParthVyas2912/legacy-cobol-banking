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

            [MaxLength(3)]
            public string CurrencyCode { get; set; } // PIC X(3)

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            public TransactionStatusEnum TransactionStatus { get; set; } // 88-level equivalent

            public AccountDetails Account { get; set; } // GROUP item

            public class AccountDetails
            {
                [MaxLength(15)]
                public string AccountNumber { get; set; } // PIC X(15)

                [MaxLength(30)]
                public string AccountHolderName { get; set; } // PIC X(30)

                public decimal AccountBalance { get; set; } // PIC 9(10)V99 COMP-3
            }

            public enum TransactionStatusEnum
            {
                Pending, // 88-level value for pending
                Completed, // 88-level value for completed
                Failed // 88-level value for failed
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