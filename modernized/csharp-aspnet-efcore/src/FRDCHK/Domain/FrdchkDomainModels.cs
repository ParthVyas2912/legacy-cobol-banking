using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Added for PrecisionAttribute

namespace FRDCHK.Domain
{
    public class FrdchkDomainModels
    {
        public class FraudCheckRecord
        {
            [MaxLength(10)]
            public string RecordId { get; set; } = string.Empty; // PIC X(10)

            [MaxLength(20)]
            public string CustomerName { get; set; } = string.Empty; // PIC X(20)

            [Column(TypeName = "decimal(15,2)")] // Replaced Precision with Column attribute
            public decimal TransactionAmount { get; set; } // PIC 9(13)V99 COMP-3

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            public FraudStatusEnum FraudStatus { get; set; } // 88-level equivalent

            public List<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>(); // OCCURS 10 TIMES

            public class TransactionDetail
            {
                [MaxLength(15)]
                public string DetailId { get; set; } = string.Empty; // PIC X(15)

                [Column(TypeName = "decimal(12,2)")] // Replaced Precision with Column attribute
                public decimal Amount { get; set; } // PIC 9(10)V99 COMP-3

                [MaxLength(30)]
                public string Description { get; set; } = string.Empty; // PIC X(30)
            }
        }

        public enum FraudStatusEnum
        {
            Unknown = 0, // Default value
            Active = 1, // VALUE 'A'
            Inactive = 2 // VALUE 'I'
        }
    }
}