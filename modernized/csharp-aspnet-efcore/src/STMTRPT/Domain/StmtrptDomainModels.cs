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
            public string ReportId { get; set; } // PIC X(10)

            public DateTime ReportDate { get; set; } // PIC 9(8) (Assuming YYYYMMDD format)

            public decimal TotalAmount { get; set; } // PIC S9(13)V99 COMP-3

            public List<Transaction> Transactions { get; set; } = new List<Transaction>(); // OCCURS 10 TIMES
        }

        public class Transaction
        {
            [MaxLength(20)]
            public string TransactionId { get; set; } // PIC X(20)

            public decimal Amount { get; set; } // PIC S9(11)V99 COMP-3

            public TransactionType Type { get; set; } // 88-level equivalent
        }

        public enum TransactionType
        {
            Credit, // 88 CREDIT VALUE 'C'
            Debit   // 88 DEBIT VALUE 'D'
        }
    }
}