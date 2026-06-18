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

            public DateTime AuditTimestamp { get; set; } // PIC 9(8) for date (YYYYMMDD) and PIC 9(6) for time (HHMMSS)

            [MaxLength(50)]
            public string Action { get; set; } // PIC X(50)

            public AuditDetails Details { get; set; } // GROUP item for detailed audit information
        }

        public class AuditDetails
        {
            [MaxLength(100)]
            public string Description { get; set; } // PIC X(100)

            [MaxLength(30)]
            public string EntityName { get; set; } // PIC X(30)

            [Range(-9999999999999.99, 9999999999999.99)]
            public decimal Amount { get; set; } // PIC S9(13)V99 COMP-3

            public AuditStatus Status { get; set; } // 88-level condition for audit status
        }

        public enum AuditStatus
        {
            Pending, // 88 PENDING VALUE 'P'
            Completed, // 88 COMPLETED VALUE 'C'
            Failed // 88 FAILED VALUE 'F'
        }
    }
}