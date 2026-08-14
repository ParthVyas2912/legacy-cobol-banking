using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TXNPOST.Domain
{
    public class TxnpostDomainModels
    {
        public class TransactionRecord
        {
            [MaxLength(10)]
            public string TransactionId { get; set; } // PIC X(10)

            public decimal TransactionAmount { get; set; } // PIC 9(7)V99 COMP-3

            [MaxLength(20)]
            public string AccountNumber { get; set; } // PIC X(20)

            public TransactionStatus Status { get; set; } // 88-level equivalent

            public DateTime TransactionDate { get; set; } // PIC 9(8) (YYYYMMDD)

            public List<TransactionDetail> Details { get; set; } = new List<TransactionDetail>(); // OCCURS 10 TIMES
        }

        public class TransactionDetail
        {
            [MaxLength(5)]
            public string DetailCode { get; set; } // PIC X(5)

            public decimal DetailAmount { get; set; } // PIC 9(5)V99 COMP-3
        }

        public enum TransactionStatus
        {
            Pending, // 88-level value
            Completed, // 88-level value
            Failed // 88-level value
        }
    }
}