using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore; // Added for PrecisionAttribute

namespace AUDITSRV.Domain
{
    public class AuditsrvDomainModels
    {
        public class AuditRecord
        {
            [MaxLength(10)] // PIC X(10)
            public string RecordId { get; set; } = string.Empty;

            [MaxLength(20)] // PIC X(20)
            public string UserName { get; set; } = string.Empty;

            [MaxLength(8)] // PIC X(8)
            public string ActionCode { get; set; } = string.Empty;

            [Precision(15, 2)] // PIC 9(13)V99 COMP-3
            public decimal Amount { get; set; } = 0m;

            [MaxLength(10)] // PIC X(10)
            public string Timestamp { get; set; } = string.Empty;

            public AuditStatus Status { get; set; } = AuditStatus.Pending;

            public List<AuditDetail> Details { get; set; } = new List<AuditDetail>();
        }

        public class AuditDetail
        {
            [MaxLength(5)] // PIC X(5)
            public string DetailCode { get; set; } = string.Empty;

            [MaxLength(50)] // PIC X(50)
            public string Description { get; set; } = string.Empty;

            [Precision(10, 2)] // PIC 9(8)V99 COMP-3
            public decimal DetailAmount { get; set; } = 0m;
        }

        public enum AuditStatus
        {
            Pending, // 88 PENDING VALUE 'P'
            Approved, // 88 APPROVED VALUE 'A'
            Rejected // 88 REJECTED VALUE 'R'
        }
    }
}