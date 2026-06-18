using System;
using System.ComponentModel.DataAnnotations;

namespace ACCTMGR.Domain
{
    public class AcctmgrDomainModels
    {
        public class Account
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string AccountName { get; set; } // PIC X(30)

            [Precision(15, 2)]
            public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatus Status { get; set; } // 88-level equivalent

            public Transaction[] Transactions { get; set; } = new Transaction[10]; // OCCURS 10 TIMES
        }

        public class Transaction
        {
            [MaxLength(6)]
            public string TransactionCode { get; set; } // PIC X(6)

            [DataType(DataType.Date)]
            public DateTime TransactionDate { get; set; } // PIC 9(8)

            [Precision(13, 2)]
            public decimal Amount { get; set; } // PIC S9(11)V99 COMP-3

            [MaxLength(50)]
            public string Description { get; set; } // PIC X(50)
        }

        public enum AccountStatus
        {
            Active, // 88 ACTIVE VALUE 'A'
            Inactive, // 88 INACTIVE VALUE 'I'
            Closed // 88 CLOSED VALUE 'C'
        }
    }
}