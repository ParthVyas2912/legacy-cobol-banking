using System;
using System.ComponentModel.DataAnnotations;

namespace STMTRPT.Domain
{
    public class StmtrptDomainModels
    {
        public class AccountSummary
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            [Range(typeof(decimal), "-9999999999999.99", "9999999999999.99")]
            public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3

            [Range(typeof(decimal), "-9999999999999.99", "9999999999999.99")]
            public decimal AvailableCredit { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatusEnum AccountStatus { get; set; } // 88-level equivalent

            [DataType(DataType.Date)]
            public DateTime LastTransactionDate { get; set; } // PIC 9(8)

            [MaxLength(10)]
            public Transaction[] Transactions { get; set; } // OCCURS 10 TIMES
        }

        public class Transaction
        {
            [MaxLength(6)]
            public string TransactionCode { get; set; } // PIC X(6)

            [Range(typeof(decimal), "-99999999999.99", "99999999999.99")]
            public decimal TransactionAmount { get; set; } // PIC S9(11)V99 COMP-3

            [MaxLength(30)]
            public string Description { get; set; } // PIC X(30)

            [DataType(DataType.Date)]
            public DateTime TransactionDate { get; set; } // PIC 9(8)
        }

        public enum AccountStatusEnum
        {
            Active = 'A', // 88 ACTIVE VALUE 'A'
            Inactive = 'I', // 88 INACTIVE VALUE 'I'
            Closed = 'C' // 88 CLOSED VALUE 'C'
        }
    }
}