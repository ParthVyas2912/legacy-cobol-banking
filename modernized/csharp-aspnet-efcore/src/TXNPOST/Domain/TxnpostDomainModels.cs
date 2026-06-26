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

            [MaxLength(20)]
            public string AccountNumber { get; set; } // PIC X(20)

            [Range(-9999999999999.99, 9999999999999.99)]
            public decimal TransactionAmount { get; set; } // PIC S9(13)V99 COMP-3

            public TransactionType TransactionType { get; set; } // 88-level values for transaction type

            [DataType(DataType.Date)]
            public DateTime TransactionDate { get; set; } // PIC 9(8) - YYYYMMDD

            [DataType(DataType.Time)]
            public TimeSpan TransactionTime { get; set; } // PIC 9(6) - HHMMSS

            [Range(-9999999999999.99, 9999999999999.99)]
            public decimal BalanceAfterTransaction { get; set; } // PIC S9(13)V99 COMP-3

            [MaxLength(50)]
            public string Description { get; set; } // PIC X(50)

            public TransactionStatus Status { get; set; } // 88-level values for transaction status
        }

        public enum TransactionType
        {
            Credit, // 88 CREDIT VALUE 'C'
            Debit   // 88 DEBIT VALUE 'D'
        }

        public enum TransactionStatus
        {
            Pending,    // 88 PENDING VALUE 'P'
            Completed,  // 88 COMPLETED VALUE 'C'
            Failed      // 88 FAILED VALUE 'F'
        }
    }
}