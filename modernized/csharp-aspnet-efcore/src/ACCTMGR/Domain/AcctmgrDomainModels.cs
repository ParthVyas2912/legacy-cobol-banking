using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ACCTMGR.Domain
{
    public class AcctmgrDomainModels
    {
        public class Account
        {
            /// <summary>
            /// Account Number
            /// </summary>
            [MaxLength(10)] // PIC X(10)
            public string AccountNumber { get; set; }

            /// <summary>
            /// Account Balance
            /// </summary>
            // PIC S9(13)V99 COMP-3
            public decimal AccountBalance { get; set; }

            /// <summary>
            /// Account Type
            /// </summary>
            [MaxLength(2)] // PIC X(2)
            public string AccountType { get; set; }

            /// <summary>
            /// Account Status
            /// </summary>
            public AccountStatusEnum AccountStatus { get; set; }

            /// <summary>
            /// Transaction History
            /// </summary>
            public List<Transaction> TransactionHistory { get; set; }
        }

        public class Transaction
        {
            /// <summary>
            /// Transaction ID
            /// </summary>
            [MaxLength(12)] // PIC X(12)
            public string TransactionId { get; set; }

            /// <summary>
            /// Transaction Amount
            /// </summary>
            // PIC S9(11)V99 COMP-3
            public decimal TransactionAmount { get; set; }

            /// <summary>
            /// Transaction Date
            /// </summary>
            [MaxLength(8)] // PIC X(8)
            public string TransactionDate { get; set; }

            /// <summary>
            /// Transaction Type
            /// </summary>
            public TransactionTypeEnum TransactionType { get; set; }
        }

        public enum AccountStatusEnum
        {
            /// <summary>
            /// Active Account
            /// </summary>
            Active = 1, // 88 ACTIVE VALUE '1'

            /// <summary>
            /// Inactive Account
            /// </summary>
            Inactive = 2 // 88 INACTIVE VALUE '2'
        }

        public enum TransactionTypeEnum
        {
            /// <summary>
            /// Credit Transaction
            /// </summary>
            Credit = 1, // 88 CREDIT VALUE '1'

            /// <summary>
            /// Debit Transaction
            /// </summary>
            Debit = 2 // 88 DEBIT VALUE '2'
        }
    }
}