using System;
using System.ComponentModel.DataAnnotations;

namespace ATMTXN.Domain
{
    public class AtmtxnDomainModels
    {
        public class Transaction
        {
            [Required]
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            [Required]
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            [Required]
            [Precision(15, 2)]
            public decimal TransactionAmount { get; set; } // PIC S9(13)V99 COMP-3

            [Required]
            [MaxLength(6)]
            public string TransactionDate { get; set; } // PIC X(6)

            [Required]
            [MaxLength(6)]
            public string TransactionTime { get; set; } // PIC X(6)

            [Required]
            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            [Required]
            public TransactionStatusEnum TransactionStatus { get; set; } // 88-level equivalent

            public class TransactionDetails
            {
                [Required]
                [MaxLength(20)]
                public string MerchantName { get; set; } // PIC X(20)

                [Required]
                [MaxLength(15)]
                public string MerchantId { get; set; } // PIC X(15)

                [Required]
                [Precision(15, 2)]
                public decimal MerchantAmount { get; set; } // PIC S9(13)V99 COMP-3
            }

            public TransactionDetails Details { get; set; }
        }

        public enum TransactionTypeEnum
        {
            Withdrawal, // 88 WITHDRAWAL VALUE 'W'
            Deposit,    // 88 DEPOSIT VALUE 'D'
            Transfer    // 88 TRANSFER VALUE 'T'
        }

        public enum TransactionStatusEnum
        {
            Pending,    // 88 PENDING VALUE 'P'
            Completed,  // 88 COMPLETED VALUE 'C'
            Failed      // 88 FAILED VALUE 'F'
        }
    }
}