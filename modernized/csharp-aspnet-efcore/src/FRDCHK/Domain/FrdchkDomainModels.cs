using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FRDCHK.Domain
{
    public class FrdchkDomainModels
    {
        public class TransactionRecord
        {
            [MaxLength(10)]
            public string TransactionId { get; set; } // PIC X(10)

            [MaxLength(20)]
            public string AccountNumber { get; set; } // PIC X(20)

            [Precision(15, 2)]
            public decimal TransactionAmount { get; set; } // PIC 9(13)V99 COMP-3

            public TransactionStatus Status { get; set; } // 88-level equivalent

            [DataType(DataType.Date)]
            public DateTime TransactionDate { get; set; } // PIC 9(8) (YYYYMMDD)

            public List<ItemDetail> ItemDetails { get; set; } = new List<ItemDetail>(); // OCCURS 10 TIMES
        }

        public class ItemDetail
        {
            [MaxLength(15)]
            public string ItemCode { get; set; } // PIC X(15)

            [Precision(9, 2)]
            public decimal ItemAmount { get; set; } // PIC 9(7)V99 COMP-3

            [Range(0, 99999)]
            public int Quantity { get; set; } // PIC 9(5)
        }

        public enum TransactionStatus
        {
            Pending, // 88-level value for "Pending"
            Completed, // 88-level value for "Completed"
            Rejected // 88-level value for "Rejected"
        }
    }
}