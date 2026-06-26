using System;
using System.ComponentModel.DataAnnotations;

namespace STMTRPT.Domain
{
    public class StmtrptDomainModels
    {
        public class StatementReport
        {
            [MaxLength(10)]
            public string ReportId { get; set; } // PIC X(10)

            public ReportHeader Header { get; set; }

            public ReportDetail[] Details { get; set; } // OCCURS 100 TIMES

            public ReportFooter Footer { get; set; }
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

        public class ReportDetail
        {
            [MaxLength(15)]
            public string AccountNumber { get; set; } // PIC X(15)

            [MaxLength(30)]
            public string AccountName { get; set; } // PIC X(30)

            [Range(-99999999999.99, 99999999999.99)]
            public decimal TransactionAmount { get; set; } // PIC S9(11)V99 COMP-3

            [MaxLength(10)]
            public string TransactionDate { get; set; } // PIC X(10)

            [MaxLength(50)]
            public string TransactionDescription { get; set; } // PIC X(50)
        }

        public class ReportFooter
        {
            [Range(0, 99999)]
            public int TotalRecords { get; set; } // PIC 9(5)

            [Range(-9999999999999.99, 9999999999999.99)]
            public decimal TotalAmount { get; set; } // PIC S9(13)V99 COMP-3
        }
    }
}