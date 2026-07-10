using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STMTRPT.Domain
{
    public class StmtrptDomainModels
    {
        public class StatementReport
        {
            [MaxLength(10)] // PIC X(10)
            public string ReportId { get; set; }

            [MaxLength(30)] // PIC X(30)
            public string CustomerName { get; set; }

            [MaxLength(15)] // PIC X(15)
            public string AccountNumber { get; set; }

            [Precision(15, 2)] // PIC S9(13)V99 COMP-3
            public decimal AccountBalance { get; set; }

            [DataType(DataType.Date)] // PIC 9(8)
            public DateTime ReportDate { get; set; }

            public List<Transaction> Transactions { get; set; } = new List<Transaction>(); // OCCURS 10 TIMES

            public class Transaction
            {
                [MaxLength(12)] // PIC X(12)
                public string TransactionId { get; set; }

                [Precision(11, 2)] // PIC S9(9)V99 COMP-3
                public decimal TransactionAmount { get; set; }

                [MaxLength(1)] // PIC X(1)
                public string TransactionType { get; set; }

                public TransactionTypeEnum TransactionTypeEnum { get; set; } // 88-level equivalent

                [DataType(DataType.Date)] // PIC 9(8)
                public DateTime TransactionDate { get; set; }
            }

            public enum TransactionTypeEnum
            {
                Credit, // 88 CREDIT VALUE "C"
                Debit // 88 DEBIT VALUE "D"
            }
        }
    }
}