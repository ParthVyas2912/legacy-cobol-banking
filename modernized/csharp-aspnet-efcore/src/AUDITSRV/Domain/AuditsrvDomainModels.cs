using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AUDITSRV.Domain
{
    public class AuditsrvDomainModels
    {
        public class AuditRecord
        {
            // PIC X(10)
            [MaxLength(10)]
            public string RecordId { get; set; }

            // PIC 9(8)
            public int AuditDate { get; set; }

            // PIC 9(6)
            public int AuditTime { get; set; }

            // PIC X(20)
            [MaxLength(20)]
            public string AuditorName { get; set; }

            // PIC S9(13)V99 COMP-3
            public decimal AuditAmount { get; set; }

            // PIC X(50)
            [MaxLength(50)]
            public string AuditDescription { get; set; }

            // PIC 9(5)
            public int AuditCode { get; set; }

            // PIC X(1)
            [MaxLength(1)]
            public string AuditStatus { get; set; }

            // PIC X(1)
            [MaxLength(1)]
            public string AuditType { get; set; }

            // OCCURS 5 TIMES
            public List<AuditDetail> AuditDetails { get; set; } = new List<AuditDetail>(5);
        }

        public class AuditDetail
        {
            // PIC X(10)
            [MaxLength(10)]
            public string DetailId { get; set; }

            // PIC X(30)
            [MaxLength(30)]
            public string DetailDescription { get; set; }

            // PIC S9(7)V99 COMP-3
            public decimal DetailAmount { get; set; }
        }

        public enum AuditStatusEnum
        {
            // 88-level values
            // PIC X(1) VALUE 'A'
            Active = 'A',
            // PIC X(1) VALUE 'I'
            Inactive = 'I',
            // PIC X(1) VALUE 'C'
            Closed = 'C'
        }

        public enum AuditTypeEnum
        {
            // 88-level values
            // PIC X(1) VALUE 'F'
            Financial = 'F',
            // PIC X(1) VALUE 'O'
            Operational = 'O',
            // PIC X(1) VALUE 'C'
            Compliance = 'C'
        }
    }
}