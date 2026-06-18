using System;
using System.ComponentModel.DataAnnotations;

namespace FRDCHK.Domain
{
    public class FrdchkDomainModels
    {
        public class TransactionRecord
        {
            [MaxLength(10)]
            public string TransactionId { get; set; } // PIC X(10)

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            [Precision(15, 2)]
            public decimal TransactionAmount { get; set; } // PIC S9(13)V99 COMP-3

            public TransactionStatusEnum TransactionStatus { get; set; } // 88-level equivalent

            public class TransactionDetails
            {
                [MaxLength(50)]
                public string Description { get; set; } // PIC X(50)

                [MaxLength(20)]
                public string MerchantCode { get; set; } // PIC X(20)

                [Range(0, 99999)]
                public int ItemCount { get; set; } // PIC 9(5)
            }

            public TransactionDetails Details { get; set; }
        }

        public enum TransactionStatusEnum
        {
            Approved, // 88 Approved
            Declined, // 88 Declined
            Pending   // 88 Pending
        }
    }
}