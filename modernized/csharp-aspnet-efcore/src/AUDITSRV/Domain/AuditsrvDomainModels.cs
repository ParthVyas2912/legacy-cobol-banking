using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AUDITSRV.Domain
{
    public class AuditsrvDomainModels
    {
        public class AuditRecord
        {
            [MaxLength(10)]
            public string RecordId { get; set; } // PIC X(10)

            [MaxLength(20)]
            public string AuditType { get; set; } // PIC X(20)

            [Precision(15, 2)]
            public decimal Amount { get; set; } // PIC 9(13)V99 COMP-3

            public AuditStatus Status { get; set; } // 88-level values

            public AuditDetails Details { get; set; } // GROUP item
        }

        public class AuditDetails
        {
            [MaxLength(30)]
            public string Description { get; set; } // PIC X(30)

            [MaxLength(10)]
            public string CreatedBy { get; set; } // PIC X(10)

            [Column(TypeName = "date")]
            public DateTime CreatedDate { get; set; } // PIC 9(8) - Date format assumed

            [MaxLength(10)]
            public string UpdatedBy { get; set; } // PIC X(10)

            [Column(TypeName = "date")]
            public DateTime UpdatedDate { get; set; } // PIC 9(8) - Date format assumed
        }

        public enum AuditStatus
        {
            Pending, // 88 PENDING VALUE 'P'
            Approved, // 88 APPROVED VALUE 'A'
            Rejected // 88 REJECTED VALUE 'R'
        }
    }
}