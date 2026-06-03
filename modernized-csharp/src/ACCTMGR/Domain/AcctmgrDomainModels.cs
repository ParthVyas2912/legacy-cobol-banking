using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ACCTMGR.Domain
{
    public class AcctmgrDomainModels
    {
        public class AccountRecord
        {
            [Required]
            [MaxLength(20)]
            public string AccountNumber { get; set; } // PIC X(20)

            [Required]
            [MaxLength(20)]
            public string CustomerId { get; set; } // PIC X(20)

            [Required]
            public DateTime OpenedDate { get; set; } // PIC 9(8) (YYYYMMDD)

            [Required]
            [MaxLength(2)]
            public string AccountType { get; set; } // PIC X(2)

            [Required]
            [MaxLength(1)]
            public string AccountStatus { get; set; } // PIC X(1)

            [Required]
            [Range(-9999999999999.99, 9999999999999.99)]
            public decimal AccountBalance { get; set; } // PIC S9(13)V99 COMP-3

            [Required]
            [Range(-9999999999999.99, 9999999999999.99)]
            public decimal AvailableBalance { get; set; } // PIC S9(13)V99 COMP-3

            [Required]
            [Range(0.00, 99.9999)]
            public decimal InterestRate { get; set; } // PIC 9(2)V9(4) COMP-3

            [Required]
            public DateTime LastTransactionDate { get; set; } // PIC 9(8) (YYYYMMDD)

            [Required]
            [MaxLength(3)]
            public string Currency { get; set; } // PIC X(3)

            [Required]
            [Range(0, 99999)]
            public long BranchCode { get; set; } // PIC 9(5)

            [Required]
            [MaxLength(4)]
            public string Flags { get; set; } // PIC X(4)

            [MaxLength(50)]
            public string Filler { get; set; } // PIC X(50)
        }

        public class CustomerRecord
        {
            [Required]
            [MaxLength(20)]
            public string CustomerId { get; set; } // PIC X(20)

            [Required]
            [MaxLength(50)]
            public string CustomerName { get; set; } // PIC X(50)

            [Required]
            [Range(0, 999)]
            public decimal RiskScore { get; set; } // PIC 9(03) COMP-3

            [Required]
            [Range(0, 999)]
            public decimal AccountCount { get; set; } // PIC 9(03) COMP-3

            [MaxLength(100)]
            public string Address { get; set; } // PIC X(100)

            [MaxLength(15)]
            public string PhoneNumber { get; set; } // PIC X(15)

            [MaxLength(50)]
            public string Email { get; set; } // PIC X(50)
        }
    }
}