using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STMTRPT.Domain
{
    public class StmtrptDomainModels
    {
        public class StatementReport
        {
            [Precision(15, 2)]
            public decimal AccountBalance { get; set; } // PIC S9(13)V99 COMP-3

            [MaxLength(30)]
            public string AccountHolderName { get; set; } // PIC X(30)

            public int AccountNumber { get; set; } // PIC 9(10)

            public List<Transaction> Transactions { get; set; } = new List<Transaction>(); // OCCURS 10 TIMES

            public AccountStatusEnum AccountStatus { get; set; } // 88-level equivalent

            public class Transaction
            {
                [Precision(11, 2)]
                public decimal TransactionAmount { get; set; } // PIC S9(9)V99 COMP-3

                [MaxLength(10)]
                public string TransactionDate { get; set; } // PIC X(10)

                [MaxLength(50)]
                public string TransactionDescription { get; set; } // PIC X(50)
            }

            public enum AccountStatusEnum
            {
                Active, // 88 ACTIVE VALUE 'A'
                Inactive, // 88 INACTIVE VALUE 'I'
                Closed // 88 CLOSED VALUE 'C'
            }
        }
    }
}