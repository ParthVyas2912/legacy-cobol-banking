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
            public string AccountNumber { get; set; } = string.Empty; // PIC X(10)

            [MaxLength(30)]
            public string AccountHolderName { get; set; } = string.Empty; // PIC X(30)

            public decimal CurrentBalance { get; set; } = 0m; // PIC S9(13)V99 COMP-3

            public decimal AvailableBalance { get; set; } = 0m; // PIC S9(13)V99 COMP-3

            public List<Transaction> Transactions { get; set; } = new List<Transaction>(); // OCCURS 100 TIMES

            public class Transaction
            {
                [MaxLength(8)]
                public string TransactionDate { get; set; } = string.Empty; // PIC X(8)

                [MaxLength(6)]
                public string TransactionTime { get; set; } = string.Empty; // PIC X(6)

                [MaxLength(15)]
                public string Description { get; set; } = string.Empty; // PIC X(15)

                public decimal Amount { get; set; } = 0m; // PIC S9(11)V99 COMP-3

                public TransactionType Type { get; set; } = TransactionType.Unknown; // 88-Level mapping

                public enum TransactionType
                {
                    Unknown = 0,
                    Credit = 1, // VALUE 'C'
                    Debit = 2   // VALUE 'D'
                }
            }
        }
    }
}