using System;
using System.ComponentModel.DataAnnotations;

namespace AUDITSRV.Domain
{
    public class AuditFile
    {
        /// <summary>
        /// Primary key for the audit record.
        /// PIC X(20)
        /// </summary>
        [MaxLength(20)]
        public string AuditKey { get; set; }

        /// <summary>
        /// Account number associated with the audit record.
        /// PIC 9(18)
        /// </summary>
        public long AuditAcctNumber { get; set; }

        /// <summary>
        /// Action performed in the audit record.
        /// PIC X(16)
        /// </summary>
        [MaxLength(16)]
        public string AuditAction { get; set; }

        /// <summary>
        /// User associated with the audit record.
        /// PIC X(8)
        /// </summary>
        [MaxLength(8)]
        public string AuditUser { get; set; }

        /// <summary>
        /// Timestamp of the audit record.
        /// PIC X(26)
        /// </summary>
        [MaxLength(26)]
        public string AuditTs { get; set; }

        /// <summary>
        /// Detailed information about the audit record.
        /// PIC X(120)
        /// </summary>
        [MaxLength(120)]
        public string AuditDetail { get; set; }
    }
}