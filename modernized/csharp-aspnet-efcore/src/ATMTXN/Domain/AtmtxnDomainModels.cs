using System;
using System.ComponentModel.DataAnnotations;

namespace ATMTXN.Domain
{
    public class AtmtxnDomainModels
    {
        public class Transaction
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            public decimal TransactionAmount { get; set; } // PIC 9(9)V99 COMP-3

            public TransactionType TransactionType { get; set; } // 88-level equivalent

            public AccountDetails Account { get; set; } // GROUP item

            public DateTime TransactionDate { get; set; } // PIC 9(8) (YYYYMMDD)

            public TimeSpan TransactionTime { get; set; } // PIC 9(6) (HHMMSS)
        }

        public class AccountDetails
        {
            [MaxLength(16)]
            public string AccountNumber { get; set; } // PIC X(16)

            public decimal AccountBalance { get; set; } // PIC 9(11)V99 COMP-3
        }

        public enum TransactionType
        {
            Withdrawal, // 88-level value
            Deposit,    // 88-level value
            Transfer    // 88-level value
        }
    }
}