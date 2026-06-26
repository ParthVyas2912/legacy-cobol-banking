using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ACCTMGR.Domain
{
    public class AcctmgrDomainModels
    {
        public class Account
        {
            [Required]
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            [Required]
            public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3

            [Required]
            [MaxLength(30)]
            public string AccountHolderName { get; set; } // PIC X(30)

            [Required]
            public AccountStatus Status { get; set; } // 88-level equivalent

            [Required]
            public DateTime OpenDate { get; set; } // PIC 9(8) (YYYYMMDD)

            [Required]
            public DateTime? CloseDate { get; set; } // PIC 9(8) (YYYYMMDD), nullable for open accounts

            [Required]
            public List<Transaction> Transactions { get; set; } = new List<Transaction>(); // OCCURS n TIMES
        }

        public class Transaction
        {
            [Required]
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            [Required]
            public decimal Amount { get; set; } // PIC S9(11)V99 COMP-3

            [Required]
            public DateTime TransactionDate { get; set; } // PIC 9(8) (YYYYMMDD)

            [Required]
            public TransactionType Type { get; set; } // 88-level equivalent
        }

        public enum AccountStatus
        {
            Active, // 88 ACTIVE VALUE 'A'
            Closed, // 88 CLOSED VALUE 'C'
            Suspended // 88 SUSPENDED VALUE 'S'
        }

        public enum TransactionType
        {
            Deposit, // 88 DEPOSIT VALUE 'D'
            Withdrawal, // 88 WITHDRAWAL VALUE 'W'
            Transfer // 88 TRANSFER VALUE 'T'
        }
    }
}