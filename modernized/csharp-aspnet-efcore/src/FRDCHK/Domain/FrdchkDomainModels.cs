using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FRDCHK.Domain
{
    public class FrdchkDomainModels
    {
        public class FraudCheckRequest
        {
            [MaxLength(10)]
            public string RequestId { get; set; } // PIC X(10)

            public DateTime RequestDate { get; set; } // PIC 9(8)

            [MaxLength(20)]
            public string CustomerName { get; set; } // PIC X(20)

            public decimal TransactionAmount { get; set; } // PIC 9(13)V99 COMP-3

            public FraudCheckDetails[] Details { get; set; } // OCCURS 10 TIMES
        }

        public class FraudCheckDetails
        {
            [MaxLength(15)]
            public string TransactionId { get; set; } // PIC X(15)

            public decimal TransactionValue { get; set; } // PIC 9(11)V99 COMP-3

            public TransactionStatus Status { get; set; } // 88-level equivalent
        }

        public enum TransactionStatus
        {
            Approved, // 88 APPROVED VALUE 'A'
            Rejected, // 88 REJECTED VALUE 'R'
            Pending   // 88 PENDING VALUE 'P'
        }
    }
}