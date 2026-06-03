using System;
using System.ComponentModel.DataAnnotations;

namespace ATMTXN.Domain
{
    public class AtmtxnDomainModels
    {
        public class TransactionRecord
        {
            /// <summary>
            /// Transaction ID
            /// PIC X(12)
            /// </summary>
            [MaxLength(12)]
            public string TransactionId { get; set; }

            /// <summary>
            /// Account Number
            /// PIC X(16)
            /// </summary>
            [MaxLength(16)]
            public string AccountNumber { get; set; }

            /// <summary>
            /// Transaction Type
            /// PIC X(1)
            /// </summary>
            [MaxLength(1)]
            public string TransactionType { get; set; }

            /// <summary>
            /// Transaction Amount
            /// PIC S9(11)V99 COMP-3
            /// </summary>
            [Range(-99999999999.99, 99999999999.99)]
            public decimal TransactionAmount { get; set; } // Precision: 13, Scale: 2

            /// <summary>
            /// Transaction Date
            /// PIC 9(8)
            /// </summary>
            [Range(0, 99999999)]
            public int TransactionDate { get; set; }

            /// <summary>
            /// Transaction Time
            /// PIC 9(6)
            /// </summary>
            [Range(0, 999999)]
            public int TransactionTime { get; set; }

            /// <summary>
            /// Transaction Balance After
            /// PIC S9(13)V99 COMP-3
            /// </summary>
            [Range(-999999999999.99, 999999999999.99)]
            public decimal TransactionBalanceAfter { get; set; } // Precision: 15, Scale: 2

            /// <summary>
            /// Transaction Status
            /// PIC X(1)
            /// </summary>
            [MaxLength(1)]
            public string TransactionStatus { get; set; }

            /// <summary>
            /// Transaction Error Code
            /// PIC X(4)
            /// </summary>
            [MaxLength(4)]
            public string TransactionErrorCode { get; set; }
        }

        public enum TransactionTypeEnum
        {
            /// <summary>
            /// Deposit Transaction
            /// </summary>
            Deposit = 'D',

            /// <summary>
            /// Withdrawal Transaction
            /// </summary>
            Withdrawal = 'W',

            /// <summary>
            /// Transfer Transaction
            /// </summary>
            Transfer = 'T'
        }

        public enum TransactionStatusEnum
        {
            /// <summary>
            /// Transaction Successful
            /// </summary>
            Successful = 'S',

            /// <summary>
            /// Transaction Failed
            /// </summary>
            Failed = 'F',

            /// <summary>
            /// Transaction Pending
            /// </summary>
            Pending = 'P'
        }
    }
}