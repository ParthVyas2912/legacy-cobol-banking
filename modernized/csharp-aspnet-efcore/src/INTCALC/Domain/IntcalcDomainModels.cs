using System;
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

            public CalculationType CalculationType { get; set; } // 88-level equivalent
        }

        public class CalculationOutput
        {
            public decimal TotalInterest { get; set; } // PIC S9(13)V99 COMP-3

            public decimal TotalAmount { get; set; } // PIC S9(13)V99 COMP-3

            [MaxLength(1)]
            public string Status { get; set; } // PIC X(1)

            public StatusCode StatusCode { get; set; } // 88-level equivalent
        }

        public enum CalculationType
        {
            SimpleInterest, // 88 SIMPLE-INTEREST VALUE 'S'
            CompoundInterest // 88 COMPOUND-INTEREST VALUE 'C'
        }

        public enum StatusCode
        {
            Success, // 88 SUCCESS VALUE '0'
            Failure // 88 FAILURE VALUE '1'
        }
    }
}