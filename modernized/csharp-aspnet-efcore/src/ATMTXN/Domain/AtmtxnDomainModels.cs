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
            /// </summary>
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            /// <summary>
            /// Account Number
            /// </summary>
            [MaxLength(16)]
            public string AccountNumber { get; set; } // PIC X(16)

            /// <summary>
            /// Transaction Amount
            /// </summary>
            public decimal TransactionAmount { get; set; } // PIC S9(13)V99 COMP-3

            /// <summary>
            /// Transaction Date
            /// </summary>
            [MaxLength(8)]
            public string TransactionDate { get; set; } // PIC X(8)

            /// <summary>
            /// Transaction Time
            /// </summary>
            [MaxLength(6)]
            public string TransactionTime { get; set; } // PIC X(6)

            /// <summary>
            /// Transaction Type
            /// </summary>
            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            /// <summary>
            /// Status Code
            /// </summary>
            public StatusCodeEnum StatusCode { get; set; } // 88-level equivalent

            /// <summary>
            /// ATM Location
            /// </summary>
            [MaxLength(20)]
            public string AtmLocation { get; set; } // PIC X(20)
        }

        public enum TransactionTypeEnum
        {
            Withdrawal, // 88 WITHDRAWAL VALUE 'W'
            Deposit,    // 88 DEPOSIT VALUE 'D'
            Inquiry     // 88 INQUIRY VALUE 'I'
        }

        public enum StatusCodeEnum
        {
            Success,    // 88 SUCCESS VALUE '00'
            InsufficientFunds, // 88 INSUFFICIENT-FUNDS VALUE '01'
            InvalidAccount,    // 88 INVALID-ACCOUNT VALUE '02'
            SystemError        // 88 SYSTEM-ERROR VALUE '99'
        }
    }
}