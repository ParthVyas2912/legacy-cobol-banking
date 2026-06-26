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
            [MaxLength(10)] // PIC X(10)
            public string AuditId { get; set; }

            /// <summary>
            /// Audit Date
            /// </summary>
            public DateTime AuditDate { get; set; } // PIC 9(8)

            /// <summary>
            /// Audit Amount
            /// </summary>
            [Range(typeof(decimal), "-9999999999999.99", "9999999999999.99")] // PIC S9(13)V99 COMP-3
            public decimal AuditAmount { get; set; }
        }
    }
}