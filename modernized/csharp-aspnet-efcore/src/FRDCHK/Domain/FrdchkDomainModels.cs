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
            public string CustomerId { get; set; } // PIC X(20)

            public decimal TransactionAmount { get; set; } // PIC 9(7)V99 COMP-3

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            public FraudCheckDetails Details { get; set; }
        }

        public class FraudCheckDetails
        {
            [MaxLength(50)]
            public string MerchantName { get; set; } // PIC X(50)

            [MaxLength(3)]
            public string CurrencyCode { get; set; } // PIC X(3)

            public decimal ExchangeRate { get; set; } // PIC 9(5)V999 COMP-3

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            public List<FraudIndicator> Indicators { get; set; } // OCCURS 10 TIMES
        }

        public class FraudIndicator
        {
            [MaxLength(30)]
            public string IndicatorName { get; set; } // PIC X(30)

            public decimal IndicatorValue { get; set; } // PIC 9(5)V99 COMP-3
        }

        public enum TransactionTypeEnum
        {
            Purchase, // 88-level value
            Refund,   // 88-level value
            Transfer  // 88-level value
        }
    }
}