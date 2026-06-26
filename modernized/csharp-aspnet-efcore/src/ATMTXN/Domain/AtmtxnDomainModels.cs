using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ATMTXN.Domain
{
    public class AtmtxnDomainModels
    {
        public class TransactionRecord
        {
            // PIC 9(9)
            [Range(0, 999999999)]
            public int TransactionId { get; set; }

            // PIC X(20)
            [MaxLength(20)]
            public string AccountNumber { get; set; }

            // PIC S9(13)V99 COMP-3
            public decimal TransactionAmount { get; set; } // Precision (15, 2)

            // PIC X(10)
            [MaxLength(10)]
            public string TransactionType { get; set; }

            // PIC 9(8)
            [Range(0, 99999999)]
            public int TransactionDate { get; set; } // Format: YYYYMMDD

            // PIC 9(6)
            [Range(0, 999999)]
            public int TransactionTime { get; set; } // Format: HHMMSS

            // PIC X(50)
            [MaxLength(50)]
            public string MerchantName { get; set; }

            // PIC X(30)
            [MaxLength(30)]
            public string MerchantLocation { get; set; }

            // PIC X(3)
            [MaxLength(3)]
            public string CurrencyCode { get; set; }

            // PIC S9(13)V99 COMP-3
            public decimal AvailableBalance { get; set; } // Precision (15, 2)

            // PIC S9(13)V99 COMP-3
            public decimal LedgerBalance { get; set; } // Precision (15, 2)

            // PIC X(1)
            [MaxLength(1)]
            public string TransactionStatus { get; set; }

            // PIC X(100)
            [MaxLength(100)]
            public string Remarks { get; set; }
        }

        public class CustomerRecord
        {
            // PIC X(20)
            [MaxLength(20)]
            public string CustomerId { get; set; }

            // PIC X(50)
            [MaxLength(50)]
            public string CustomerName { get; set; }

            // PIC X(100)
            [MaxLength(100)]
            public string Address { get; set; }

            // PIC X(15)
            [MaxLength(15)]
            public string PhoneNumber { get; set; }

            // PIC X(50)
            [MaxLength(50)]
            public string Email { get; set; }

            // PIC X(1)
            [MaxLength(1)]
            public string CustomerStatus { get; set; }
        }

        public class AtmMachineRecord
        {
            // PIC X(10)
            [MaxLength(10)]
            public string AtmId { get; set; }

            // PIC X(50)
            [MaxLength(50)]
            public string Location { get; set; }

            // PIC X(1)
            [MaxLength(1)]
            public string Status { get; set; }

            // PIC S9(13)V99 COMP-3
            public decimal CashAvailable { get; set; } // Precision (15, 2)
        }

        public class TransactionLog
        {
            // OCCURS 100 TIMES
            public List<TransactionRecord> Transactions { get; set; } = new List<TransactionRecord>(100);
        }
    }
}