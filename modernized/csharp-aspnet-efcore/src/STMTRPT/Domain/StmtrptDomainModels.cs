using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STMTRPT.Domain
{
    public class StmtrptDomainModels
    {
        public class AccountSummary
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string AccountName { get; set; } // PIC X(30)

            public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatus Status { get; set; } // 88-level equivalent

            public List<TransactionDetail> Transactions { get; set; } // OCCURS 100 TIMES
        }

        public class TransactionDetail
        {
            [MaxLength(6)]
            public string TransactionDate { get; set; } // PIC X(6)

            [MaxLength(12)]
            public string TransactionDescription { get; set; } // PIC X(12)

            public decimal TransactionAmount { get; set; } // PIC S9(9)V99 COMP-3

            public TransactionType Type { get; set; } // 88-level equivalent
        }

        public enum AccountStatus
        {
            Active, // 88 ACTIVE VALUE 'A'
            Inactive, // 88 INACTIVE VALUE 'I'
            Closed // 88 CLOSED VALUE 'C'
        }

        public enum TransactionType
        {
            Debit, // 88 DEBIT VALUE 'D'
            Credit // 88 CREDIT VALUE 'C'
        }
    }
}