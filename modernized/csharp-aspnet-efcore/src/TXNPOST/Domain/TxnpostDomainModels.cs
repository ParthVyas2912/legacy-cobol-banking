using System;
using System.ComponentModel.DataAnnotations;

namespace TXNPOST.Domain
{
    public class TxnpostDomainModels
    {
        public class Transaction
        {
            [Required]
            [MaxLength(10)]
            public string TransactionId { get; set; } // PIC X(10)

            [Required]
            [MaxLength(20)]
            public string AccountNumber { get; set; } // PIC X(20)

            [Required]
            public decimal TransactionAmount { get; set; } // PIC S9(13)V99 COMP-3

            [Required]
            public DateTime TransactionDate { get; set; } // PIC 9(8)

            [Required]
            [MaxLength(3)]
            public string CurrencyCode { get; set; } // PIC X(3)

            [Required]
            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            [Required]
            public StatusEnum Status { get; set; } // 88-level equivalent

            public class TransactionDetails
            {
                [Required]
                [MaxLength(50)]
                public string Description { get; set; } // PIC X(50)

                [Required]
                public decimal FeeAmount { get; set; } // PIC S9(7)V99 COMP-3

                [Required]
                public decimal TaxAmount { get; set; } // PIC S9(7)V99 COMP-3
            }

            public TransactionDetails Details { get; set; }
        }

        public enum TransactionTypeEnum
        {
            Credit, // 88-level value
            Debit   // 88-level value
        }

        public enum StatusEnum
        {
            Pending,   // 88-level value
            Completed, // 88-level value
            Failed     // 88-level value
        }
    }
}