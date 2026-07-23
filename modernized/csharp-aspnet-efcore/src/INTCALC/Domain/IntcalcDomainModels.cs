using System;
using System.ComponentModel.DataAnnotations;

namespace INTCALC.Domain
{
    public class IntcalcDomainModels
    {
        public class CalculationRequest
        {
            [MaxLength(10)]
            public string RequestId { get; set; } // PIC X(10)

            public DateTime RequestDate { get; set; } // PIC 9(8) (Assuming YYYYMMDD format)

            public decimal Amount { get; set; } // PIC S9(13)V99 COMP-3

            public CalculationType CalculationType { get; set; } // 88-level equivalent

            public InterestDetails InterestDetails { get; set; } // GROUP item

            public Adjustment[] Adjustments { get; set; } // OCCURS 10 TIMES
        }

        public class InterestDetails
        {
            public decimal InterestRate { get; set; } // PIC S9(3)V99 COMP-3

            public int TermInMonths { get; set; } // PIC 9(3)

            [MaxLength(3)]
            public string CurrencyCode { get; set; } // PIC X(3)
        }

        public class Adjustment
        {
            public decimal AdjustmentAmount { get; set; } // PIC S9(7)V99 COMP-3

            [MaxLength(20)]
            public string AdjustmentReason { get; set; } // PIC X(20)
        }

        public enum CalculationType
        {
            SimpleInterest, // 88 SIMPLE-INTEREST VALUE 'S'
            CompoundInterest // 88 COMPOUND-INTEREST VALUE 'C'
        }
    }
}