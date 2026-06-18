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
            public string ActionDescription { get; set; } // PIC X(50)

            [MaxLength(15)]
            public string IpAddress { get; set; } // PIC X(15)

            [MaxLength(30)]
            public string HostName { get; set; } // PIC X(30)

            [Precision(13, 2)]
            public decimal Amount { get; set; } // PIC S9(13)V99 COMP-3

            public AuditActionType ActionType { get; set; } // 88-level equivalent

            public AuditStatus Status { get; set; } // 88-level equivalent
        }

        public enum AuditActionType
        {
            Create, // 88 CREATE VALUE 'C'
            Update, // 88 UPDATE VALUE 'U'
            Delete  // 88 DELETE VALUE 'D'
        }

        public enum AuditStatus
        {
            Pending,   // 88 PENDING VALUE 'P'
            Completed, // 88 COMPLETED VALUE 'C'
            Failed     // 88 FAILED VALUE 'F'
        }
    }
}