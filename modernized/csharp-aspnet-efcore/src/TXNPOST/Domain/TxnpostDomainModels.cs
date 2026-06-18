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

            public decimal Amount { get; set; } // PIC S9(13)V99 COMP-3

            [MaxLength(10)]
            public string CurrencyCode { get; set; } // PIC X(10)

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            public TransactionStatus Status { get; set; } // 88-level equivalent

            public AccountDetails Account { get; set; } // GROUP item

            public TransactionDetails[] Details { get; set; } // OCCURS n TIMES
        }

        public class AccountDetails
        {
            [MaxLength(20)]
            public string AccountNumber { get; set; } // PIC X(20)

            [MaxLength(30)]
            public string AccountName { get; set; } // PIC X(30)

            public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3
        }

        public class TransactionDetails
        {
            [MaxLength(15)]
            public string ItemCode { get; set; } // PIC X(15)

            public int Quantity { get; set; } // PIC 9(5)

            public decimal UnitPrice { get; set; } // PIC S9(7)V99 COMP-3
        }

        public enum TransactionStatus
        {
            Pending, // 88-level value
            Completed, // 88-level value
            Failed // 88-level value
        }
    }
}