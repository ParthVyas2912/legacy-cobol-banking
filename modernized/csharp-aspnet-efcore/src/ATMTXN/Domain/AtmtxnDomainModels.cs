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
            [MaxLength(12)] // PIC X(12)
            public string TransactionId { get; set; }

            /// <summary>
            /// Account Number
            /// </summary>
            [MaxLength(16)] // PIC X(16)
            public string AccountNumber { get; set; }

            /// <summary>
            /// Transaction Amount
            /// </summary>
            [Range(-9999999999999.99, 9999999999999.99)] // PIC S9(13)V99 COMP-3
            public decimal TransactionAmount { get; set; }

            /// <summary>
            /// Transaction Date
            /// </summary>
            [MaxLength(8)] // PIC X(8)
            public string TransactionDate { get; set; }

            /// <summary>
            /// Transaction Time
            /// </summary>
            [MaxLength(6)] // PIC X(6)
            public string TransactionTime { get; set; }

            /// <summary>
            /// Transaction Type
            /// </summary>
            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            /// <summary>
            /// ATM Location
            /// </summary>
            [MaxLength(50)] // PIC X(50)
            public string AtmLocation { get; set; }

            /// <summary>
            /// Status Code
            /// </summary>
            public StatusCodeEnum StatusCode { get; set; } // 88-level equivalent
        }

        public enum TransactionTypeEnum
        {
            Withdrawal, // 88 WITHDRAWAL VALUE 'W'
            Deposit,    // 88 DEPOSIT VALUE 'D'
            Inquiry,    // 88 INQUIRY VALUE 'I'
            Transfer    // 88 TRANSFER VALUE 'T'
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