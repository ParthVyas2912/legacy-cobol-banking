using System;
using System.ComponentModel.DataAnnotations;

namespace AUDITSRV.Domain
{
    public class AuditsrvDomainModels
    {
        public class AuditRecord
        {
            [Required]
            [MaxLength(10)]
            public string AuditId { get; set; } // PIC X(10)

            [Required]
            [MaxLength(20)]
            public string UserId { get; set; } // PIC X(20)

            [Required]
            public DateTime AuditTimestamp { get; set; } // PIC 9(8) for date (YYYYMMDD) + PIC 9(6) for time (HHMMSS)

            [Required]
            [MaxLength(50)]
            public string Action { get; set; } // PIC X(50)

            [Required]
            [MaxLength(100)]
            public string Description { get; set; } // PIC X(100)

            [Required]
            public decimal Amount { get; set; } // PIC S9(13)V99 COMP-3

            [Required]
            public AuditStatus Status { get; set; } // 88-level equivalent

            public AuditDetails Details { get; set; } // Nested group item
        }

        public class AuditDetails
        {
            [Required]
            [MaxLength(30)]
            public string ReferenceId { get; set; } // PIC X(30)

            [Required]
            [MaxLength(15)]
            public string SourceSystem { get; set; } // PIC X(15)

            [Required]
            [MaxLength(15)]
            public string TargetSystem { get; set; } // PIC X(15)

            [Required]
            public decimal ExchangeRate { get; set; } // PIC S9(5)V9(4) COMP-3
        }

        public enum AuditStatus
        {
            Pending, // 88-level value
            Completed, // 88-level value
            Failed // 88-level value
        }
    }
}