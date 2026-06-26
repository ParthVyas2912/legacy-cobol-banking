using System;
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
            public string UserId { get; set; }

            // PIC X(50)
            [MaxLength(50)]
            public string Action { get; set; }

            // PIC X(100)
            [MaxLength(100)]
            public string Description { get; set; }

            // PIC 9(5)V99 COMP-3
            public decimal Amount { get; set; } // Precision (7, 2)

            // PIC X(1)
            [MaxLength(1)]
            public string Status { get; set; }

            // 88-level for Status
            public enum StatusEnum
            {
                // Value 'A'
                Active = 'A',
                // Value 'I'
                Inactive = 'I',
                // Value 'P'
                Pending = 'P'
            }

            // PIC X(30)
            [MaxLength(30)]
            public string Comments { get; set; }

            // GROUP: Address
            public AddressGroup Address { get; set; }

            public class AddressGroup
            {
                // PIC X(50)
                [MaxLength(50)]
                public string Street { get; set; }

                // PIC X(30)
                [MaxLength(30)]
                public string City { get; set; }

                // PIC X(2)
                [MaxLength(2)]
                public string State { get; set; }

                // PIC 9(5)
                public int ZipCode { get; set; }
            }

            // OCCURS 5 TIMES
            public AuditDetail[] Details { get; set; } = new AuditDetail[5];

            public class AuditDetail
            {
                // PIC X(10)
                [MaxLength(10)]
                public string DetailId { get; set; }

                // PIC X(50)
                [MaxLength(50)]
                public string DetailDescription { get; set; }

                // PIC 9(5)V99 COMP-3
                public decimal DetailAmount { get; set; } // Precision (7, 2)
            }
        }
    }
}