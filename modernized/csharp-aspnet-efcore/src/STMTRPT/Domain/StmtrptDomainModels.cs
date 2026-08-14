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

            [MaxLength(20)]
            public string CustomerName { get; set; } // PIC X(20)

            public decimal AccountBalance { get; set; } // PIC 9(13)V99 COMP-3

            public DateTime ReportDate { get; set; } // PIC 9(8) (Assumed to be YYYYMMDD)

            public List<Transaction> Transactions { get; set; } // OCCURS 10 TIMES
        }

        public class Transaction
        {
            [MaxLength(15)]
            public string TransactionId { get; set; } // PIC X(15)

            public decimal TransactionAmount { get; set; } // PIC 9(10)V99 COMP-3

            public TransactionType Type { get; set; } // 88-level equivalent

            public DateTime TransactionDate { get; set; } // PIC 9(8) (Assumed to be YYYYMMDD)
        }

        public enum TransactionType
        {
            Credit, // 88-level value for credit
            Debit   // 88-level value for debit
        }
    }
}