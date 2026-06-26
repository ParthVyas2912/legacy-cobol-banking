using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AUDITSRV.Domain
{
    public class AuditsrvDomainModels
    {
        public class AuditRecord
        {
            /// <summary>
            /// Audit ID
            /// </summary>
            [MaxLength(10)]
            public string AuditId { get; set; } // PIC X(10)

            /// <summary>
            /// Audit Date
            /// </summary>
            public DateTime AuditDate { get; set; } // PIC 9(8)

            /// <summary>
            /// Audit Amount
            /// </summary>
            public decimal AuditAmount { get; set; } // PIC 9(7)V99 COMP-3

            /// <summary>
            /// Audit Status
            /// </summary>
            public AuditStatusEnum AuditStatus { get; set; } // 88-level equivalent

            /// <summary>
            /// Audit Details
            /// </summary>
            public AuditDetails Details { get; set; } // GROUP item

            /// <summary>
            /// Audit Notes
            /// </summary>
            [MaxLength(50)]
            public string AuditNotes { get; set; } // PIC X(50)
        }

        public class AuditDetails
        {
            /// <summary>
            /// Detail ID
            /// </summary>
            [MaxLength(5)]
            public string DetailId { get; set; } // PIC X(5)

            /// <summary>
            /// Detail Description
            /// </summary>
            [MaxLength(30)]
            public string DetailDescription { get; set; } // PIC X(30)

            /// <summary>
            /// Detail Amount
            /// </summary>
            public decimal DetailAmount { get; set; } // PIC 9(5)V99 COMP-3
        }

        public enum AuditStatusEnum
        {
            /// <summary>
            /// Pending Status
            /// </summary>
            Pending = 1, // 88-level value

            /// <summary>
            /// Completed Status
            /// </summary>
            Completed = 2, // 88-level value

            /// <summary>
            /// Cancelled Status
            /// </summary>
            Cancelled = 3 // 88-level value
        }
    }
}