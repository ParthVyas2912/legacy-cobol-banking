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
            public string AuditId { get; set; } = string.Empty;

            [MaxLength(20)] // PIC X(20)
            public string UserId { get; set; } = string.Empty;

            [MaxLength(8)] // PIC X(8)
            public string ActionDate { get; set; } = string.Empty;

            [MaxLength(6)] // PIC X(6)
            public string ActionTime { get; set; } = string.Empty;

            [MaxLength(50)] // PIC X(50)
            public string ActionDescription { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string ActionType { get; set; } = string.Empty;

            [MaxLength(1)] // PIC X(1)
            public string ActionStatus { get; set; } = string.Empty;

            [Precision(15, 2)] // PIC S9(13)V99 COMP-3
            public decimal Amount { get; set; } = 0.0m;

            [MaxLength(30)] // PIC X(30)
            public string ReferenceId { get; set; } = string.Empty;

            [MaxLength(100)] // PIC X(100)
            public string Comments { get; set; } = string.Empty;

            public AuditDetails Details { get; set; } = new AuditDetails();

            public class AuditDetails
            {
                [MaxLength(15)] // PIC X(15)
                public string DetailId { get; set; } = string.Empty;

                [MaxLength(50)] // PIC X(50)
                public string DetailDescription { get; set; } = string.Empty;

                [Precision(10, 2)] // PIC S9(8)V99 COMP-3
                public decimal DetailAmount { get; set; } = 0.0m;

                [MaxLength(1)] // PIC X(1)
                public string DetailStatus { get; set; } = string.Empty;
            }
        }

        public class AuditSummary
        {
            [MaxLength(10)] // PIC X(10)
            public string SummaryId { get; set; } = string.Empty;

            [MaxLength(20)] // PIC X(20)
            public string SummaryDescription { get; set; } = string.Empty;

            [Precision(15, 2)] // PIC S9(13)V99 COMP-3
            public decimal TotalAmount { get; set; } = 0.0m;

            public List<AuditRecord> Records { get; set; } = new List<AuditRecord>();
        }

        public enum ActionType
        {
            [Display(Name = "Create")]
            Create = 'C', // VALUE 'C'

            [Display(Name = "Update")]
            Update = 'U', // VALUE 'U'

            [Display(Name = "Delete")]
            Delete = 'D' // VALUE 'D'
        }

        public enum ActionStatus
        {
            [Display(Name = "Pending")]
            Pending = 'P', // VALUE 'P'

            [Display(Name = "Completed")]
            Completed = 'C', // VALUE 'C'

            [Display(Name = "Failed")]
            Failed = 'F' // VALUE 'F'
        }
    }
}