using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INTCALC.Domain
{
    public class IntcalcDomainModels
    {
        public class CalculationInput
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            public decimal PrincipalAmount { get; set; } // PIC S9(13)V99 COMP-3

            public decimal InterestRate { get; set; } // PIC S9(3)V99 COMP-3

            public int TermInMonths { get; set; } // PIC 9(3)

            public DateTime StartDate { get; set; } // PIC X(10) (Assuming date format)

            public CalculationType CalcType { get; set; } // 88-level equivalent
        }

        public class CalculationResult
        {
            public decimal TotalInterest { get; set; } // PIC S9(13)V99 COMP-3

            public decimal TotalAmount { get; set; } // PIC S9(13)V99 COMP-3

            public List<AmortizationSchedule> AmortizationSchedules { get; set; } // OCCURS n TIMES
        }

        public class AmortizationSchedule
        {
            public int Month { get; set; } // PIC 9(3)

            public decimal PrincipalPayment { get; set; } // PIC S9(13)V99 COMP-3

            public decimal InterestPayment { get; set; } // PIC S9(13)V99 COMP-3

            public decimal RemainingBalance { get; set; } // PIC S9(13)V99 COMP-3
        }

        public enum CalculationType
        {
            SimpleInterest, // 88-level equivalent for simple interest
            CompoundInterest // 88-level equivalent for compound interest
        }
    }
}