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

            [MaxLength(20)]
            public string CustomerName { get; set; } // PIC X(20)

            public decimal TransactionAmount { get; set; } // PIC 9(7)V99 COMP-3

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            public FraudCheckDetails Details { get; set; } // GROUP item
        }

        public class FraudCheckDetails
        {
            [MaxLength(15)]
            public string MerchantId { get; set; } // PIC X(15)

            [MaxLength(30)]
            public string MerchantName { get; set; } // PIC X(30)

            public decimal MerchantRating { get; set; } // PIC 9(3)V9(2) COMP-3

            public FraudCheckStatus Status { get; set; } // 88-level equivalent
        }

        public enum FraudCheckStatus
        {
            Pending, // VALUE 'PENDING'
            Approved, // VALUE 'APPROVED'
            Rejected // VALUE 'REJECTED'
        }
    }
}