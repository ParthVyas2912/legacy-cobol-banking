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

            public DateTime Timestamp { get; set; } // PIC 9(8) for YYYYMMDD and PIC 9(6) for HHMMSS combined

            [MaxLength(50)]
            public string Action { get; set; } // PIC X(50)

            public decimal Amount { get; set; } // PIC S9(13)V99 COMP-3

            public AuditStatus Status { get; set; } // 88-level equivalent
        }

        public enum AuditStatus
        {
            Success, // 88 SUCCESS VALUE IS 'S'
            Failure  // 88 FAILURE VALUE IS 'F'
        }

        public class AuditDetails
        {
            [MaxLength(100)]
            public string Description { get; set; } // PIC X(100)

            public decimal PreviousBalance { get; set; } // PIC S9(13)V99 COMP-3

            public decimal NewBalance { get; set; } // PIC S9(13)V99 COMP-3
        }

        public class AuditLog
        {
            public AuditRecord[] Records { get; set; } // OCCURS 100 TIMES
        }
    }
}