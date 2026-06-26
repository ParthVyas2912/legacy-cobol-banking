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

            public decimal TotalAmount { get; set; } // PIC 9(13)V99 COMP-3

            public int RecordCount { get; set; } // PIC 9(5)

            public ReportDetails[] Details { get; set; } = new ReportDetails[10]; // OCCURS 10 TIMES

            public class ReportDetails
            {
                [MaxLength(20)]
                public string AccountNumber { get; set; } // PIC X(20)

                public decimal TransactionAmount { get; set; } // PIC 9(11)V99 COMP-3

                public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

                public DateTime TransactionDate { get; set; } // PIC 9(8)

                public enum TransactionTypeEnum
                {
                    Credit, // 88 CREDIT VALUE 'C'
                    Debit   // 88 DEBIT VALUE 'D'
                }
            }
        }
    }
}