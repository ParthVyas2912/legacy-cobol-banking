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

            [MaxLength(16)]
            public string AccountNumber { get; set; } // PIC X(16)

            [Precision(15, 2)]
            public decimal TransactionAmount { get; set; } // PIC 9(13)V99 COMP-3

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            [MaxLength(6)]
            public string TransactionTime { get; set; } // PIC X(6)

            [MaxLength(3)]
            public string CurrencyCode { get; set; } // PIC X(3)

            [Precision(15, 2)]
            public decimal BalanceAfterTransaction { get; set; } // PIC 9(13)V99 COMP-3

            public StatusEnum TransactionStatus { get; set; } // 88-level equivalent
        }

        public enum TransactionTypeEnum
        {
            Withdrawal, // 88 WITHDRAWAL VALUE 'W'
            Deposit,    // 88 DEPOSIT VALUE 'D'
            Transfer    // 88 TRANSFER VALUE 'T'
        }

        public enum StatusEnum
        {
            Success,    // 88 SUCCESS VALUE 'S'
            Failed,     // 88 FAILED VALUE 'F'
            Pending     // 88 PENDING VALUE 'P'
        }
    }
}