using System;
using System.ComponentModel.DataAnnotations;

namespace AUDITSRV.Domain
{
    public class AuditsrvDomainModels
    {
        public class AuditRecord
        {
            [MaxLength(10)]
            public string AuditId { get; set; } // PIC X(10)

            [MaxLength(20)]
            public string UserId { get; set; } // PIC X(20)

            [MaxLength(8)]
            public string ActionDate { get; set; } // PIC X(8)

            [MaxLength(6)]
            public string ActionTime { get; set; } // PIC X(6)

            [MaxLength(50)]
            public string ActionType { get; set; } // PIC X(50)

            [MaxLength(100)]
            public string ActionDescription { get; set; } // PIC X(100)

            [MaxLength(15)]
            public string IpAddress { get; set; } // PIC X(15)

            [MaxLength(10)]
            public string TerminalId { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string ApplicationId { get; set; } // PIC X(30)

            [MaxLength(10)]
            public string SessionId { get; set; } // PIC X(10)

            [MaxLength(20)]
            public string ObjectId { get; set; } // PIC X(20)

            [MaxLength(10)]
            public string ObjectType { get; set; } // PIC X(10)

            [MaxLength(255)]
            public string AdditionalInfo { get; set; } // PIC X(255)

            [Precision(15, 2)]
            public decimal Amount { get; set; } // PIC S9(13)V99 COMP-3
        }

        public class AuditSummary
        {
            [MaxLength(10)]
            public string SummaryId { get; set; } // PIC X(10)

            [Range(0, 99999)]
            public int TotalRecords { get; set; } // PIC 9(5)

            [Precision(15, 2)]
            public decimal TotalAmount { get; set; } // PIC S9(13)V99 COMP-3
        }

        public enum ActionTypeEnum
        {
            Create, // 88 CREATE
            Update, // 88 UPDATE
            Delete, // 88 DELETE
            View    // 88 VIEW
        }
    }
}