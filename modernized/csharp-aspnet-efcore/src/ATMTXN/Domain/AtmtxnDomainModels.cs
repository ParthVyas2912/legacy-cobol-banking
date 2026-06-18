using System;
using System.ComponentModel.DataAnnotations;

namespace ATMTXN.Domain
{
    public class AtmtxnDomainModels
    {
        public class TransactionRecord
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            [MaxLength(16)]
            public string AccountNumber { get; set; } // PIC X(16)

            [MaxLength(3)]
            public string TransactionType { get; set; } // PIC X(3)

            [Precision(15, 2)]
            public decimal TransactionAmount { get; set; } // PIC 9(13)V99 COMP-3

            [MaxLength(8)]
            public string TransactionDate { get; set; } // PIC X(8)

            [MaxLength(6)]
            public string TransactionTime { get; set; } // PIC X(6)

            public TransactionStatus Status { get; set; } // 88-level equivalent

            public LocationDetails Location { get; set; } // GROUP item

            public class LocationDetails
            {
                [MaxLength(20)]
                public string City { get; set; } // PIC X(20)

                [MaxLength(2)]
                public string State { get; set; } // PIC X(2)

                [MaxLength(10)]
                public string CountryCode { get; set; } // PIC X(10)
            }
        }

        public enum TransactionStatus
        {
            Approved, // 88 Approved VALUE 'A'
            Declined, // 88 Declined VALUE 'D'
            Pending,  // 88 Pending VALUE 'P'
            Reversed  // 88 Reversed VALUE 'R'
        }
    }
}