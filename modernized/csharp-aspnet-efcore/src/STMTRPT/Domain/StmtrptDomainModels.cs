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

            [MaxLength(30)]
            public string AccountHolderName { get; set; } // PIC X(30)

            [Precision(15, 2)]
            public decimal AccountBalance { get; set; } // PIC S9(13)V99 COMP-3

            public Transaction[] Transactions { get; set; } // OCCURS 100 TIMES
        }

        public class Transaction
        {
            [MaxLength(6)]
            public string TransactionDate { get; set; } // PIC X(6)

            [MaxLength(10)]
            public string TransactionCode { get; set; } // PIC X(10)

            [Precision(13, 2)]
            public decimal TransactionAmount { get; set; } // PIC S9(11)V99 COMP-3

            [MaxLength(50)]
            public string Description { get; set; } // PIC X(50)
        }

        public class ReportHeader
        {
            [MaxLength(20)]
            public string ReportTitle { get; set; } // PIC X(20)

            [MaxLength(8)]
            public string ReportDate { get; set; } // PIC X(8)

            [MaxLength(6)]
            public string ReportTime { get; set; } // PIC X(6)
        }

        public class ReportFooter
        {
            [Range(0, 99999)]
            public int TotalAccounts { get; set; } // PIC 9(5)

            [Precision(17, 2)]
            public decimal TotalBalance { get; set; } // PIC S9(15)V99 COMP-3

            [Range(0, 9999999)]
            public int TotalTransactions { get; set; } // PIC 9(7)
        }

        public class StatementReport
        {
            public ReportHeader Header { get; set; }

            [MaxLength(50)]
            public AccountSummary[] AccountSummaries { get; set; } // OCCURS 50 TIMES

            public ReportFooter Footer { get; set; }
        }
    }
}