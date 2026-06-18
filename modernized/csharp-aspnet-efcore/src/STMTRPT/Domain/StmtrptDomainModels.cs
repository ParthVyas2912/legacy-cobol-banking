using System;
using System.ComponentModel.DataAnnotations;

namespace STMTRPT.Domain
{
    public class StmtrptDomainModels
    {
        public class AccountRecord
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string AccountName { get; set; } // PIC X(30)

            [Precision(15, 2)]
            public decimal AccountBalance { get; set; } // PIC S9(13)V99 COMP-3

            [Precision(15, 2)]
            public decimal AvailableBalance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatusEnum AccountStatus { get; set; } // 88-level equivalent

            public TransactionRecord[] Transactions { get; set; } = new TransactionRecord[10]; // OCCURS 10 TIMES
        }

        public class TransactionRecord
        {
            [MaxLength(6)]
            public string TransactionDate { get; set; } // PIC X(6)

            [MaxLength(12)]
            public string TransactionDescription { get; set; } // PIC X(12)

            [Precision(13, 2)]
            public decimal TransactionAmount { get; set; } // PIC S9(11)V99 COMP-3

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent
        }

        public enum AccountStatusEnum
        {
            Active, // 88 ACTIVE VALUE 'A'
            Inactive, // 88 INACTIVE VALUE 'I'
            Closed // 88 CLOSED VALUE 'C'
        }

        public enum TransactionTypeEnum
        {
            Debit, // 88 DEBIT VALUE 'D'
            Credit // 88 CREDIT VALUE 'C'
        }
    }
}