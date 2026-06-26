using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FRDCHK.Domain
{
    public class FrdchkDomainModels
    {
        public class FraudCheckRequest
        {
            [MaxLength(10)] // PIC X(10)
            public string RequestId { get; set; }

            [MaxLength(20)] // PIC X(20)
            public string CustomerName { get; set; }

            [MaxLength(15)] // PIC X(15)
            public string CustomerAccountNumber { get; set; }

            [Range(-9999999999999.99, 9999999999999.99)] // PIC S9(13)V99 COMP-3
            public decimal TransactionAmount { get; set; }

            [MaxLength(3)] // PIC X(3)
            public string CurrencyCode { get; set; }

            [DataType(DataType.Date)] // PIC 9(8)
            public DateTime TransactionDate { get; set; }

            public TransactionTypeEnum TransactionType { get; set; } // 88-level equivalent

            public List<FraudIndicator> FraudIndicators { get; set; } // OCCURS 10 TIMES

            public class FraudIndicator
            {
                [MaxLength(5)] // PIC X(5)
                public string IndicatorCode { get; set; }

                [MaxLength(50)] // PIC X(50)
                public string IndicatorDescription { get; set; }
            }

            public enum TransactionTypeEnum
            {
                Debit, // 88-level value
                Credit // 88-level value
            }
        }

        public class FraudCheckResponse
        {
            [MaxLength(10)] // PIC X(10)
            public string RequestId { get; set; }

            public FraudCheckResultEnum Result { get; set; } // 88-level equivalent

            [MaxLength(100)] // PIC X(100)
            public string ResultDescription { get; set; }

            public enum FraudCheckResultEnum
            {
                Passed, // 88-level value
                Failed, // 88-level value
                Pending // 88-level value
            }
        }
    }
}