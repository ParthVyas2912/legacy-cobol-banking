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

            public DateTime AuditDate { get; set; } // PIC 9(8)

            [MaxLength(50)]
            public string AuditorName { get; set; } // PIC X(50)

            public decimal AuditAmount { get; set; } // PIC 9(13)V99 COMP-3

            public AuditStatus Status { get; set; } // 88-level equivalent

            public AuditDetails Details { get; set; } // Nested group item
        }

        public class AuditDetails
        {
            [MaxLength(20)]
            public string Department { get; set; } // PIC X(20)

            [MaxLength(100)]
            public string Description { get; set; } // PIC X(100)

            public decimal TotalAmount { get; set; } // PIC 9(15)V99 COMP-3
        }

        public enum AuditStatus
        {
            Pending, // 88-level value
            Completed, // 88-level value
            Cancelled // 88-level value
        }
    }
}