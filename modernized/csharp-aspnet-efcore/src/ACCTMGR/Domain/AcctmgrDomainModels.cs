using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACCTMGR.Domain
{
    public class AccountManager
    {
        [MaxLength(10)] // PIC X(10)
        public string AccountNumber { get; set; } = string.Empty;

        [MaxLength(30)] // PIC X(30)
        public string AccountName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(15,2)")] // PIC S9(13)V99 COMP-3
        public decimal AccountBalance { get; set; } = 0.00m;

        [MaxLength(3)] // PIC X(3)
        public string AccountType { get; set; } = string.Empty;

        [MaxLength(1)] // PIC X(1)
        public string AccountStatus { get; set; } = string.Empty;

        public AccountStatusEnum Status { get; set; } = AccountStatusEnum.Unknown;

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public class Transaction
        {
            [MaxLength(6)] // PIC X(6)
            public string TransactionId { get; set; } = string.Empty;

            [Column(TypeName = "decimal(13,2)")] // PIC S9(11)V99 COMP-3
            public decimal TransactionAmount { get; set; } = 0.00m;

            [MaxLength(8)] // PIC X(8)
            public string TransactionDate { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string TransactionType { get; set; } = string.Empty;

            public TransactionTypeEnum Type { get; set; } = TransactionTypeEnum.Unknown;
        }

        public enum AccountStatusEnum
        {
            Unknown,
            Active, // VALUE 'A'
            Inactive, // VALUE 'I'
            Closed // VALUE 'C'
        }

        public enum TransactionTypeEnum
        {
            Unknown,
            Credit, // VALUE 'C'
            Debit // VALUE 'D'
        }
    }
}