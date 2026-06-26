using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STMTRPT.Domain
{
    public class StmtrptDomainModels
    {
        public class StatementReport
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            public decimal Balance { get; set; } // PIC 9(13)V99 COMP-3

            public DateTime StatementDate { get; set; } // PIC 9(8)

            public List<Transaction> Transactions { get; set; } = new List<Transaction>(); // OCCURS 100 TIMES

            public class Transaction
            {
                [MaxLength(6)]
                public string TransactionCode { get; set; } // PIC X(6)

                public decimal Amount { get; set; } // PIC 9(11)V99 COMP-3

                [MaxLength(30)]
                public string Description { get; set; } // PIC X(30)

                public TransactionType Type { get; set; } // 88-level equivalent

                public enum TransactionType
                {
                    Credit, // 88 CREDIT VALUE 'C'
                    Debit   // 88 DEBIT VALUE 'D'
                }
            }
        }
    }
}