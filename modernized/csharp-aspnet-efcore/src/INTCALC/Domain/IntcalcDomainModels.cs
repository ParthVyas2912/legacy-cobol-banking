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

            public decimal PrincipalAmount { get; set; } // PIC 9(7)V99 COMP-3

            public decimal InterestRate { get; set; } // PIC 9(3)V99 COMP-3

            public int TermInMonths { get; set; } // PIC 9(3)

            public CalculationType CalculationType { get; set; } // 88-level equivalent
        }

        public class CalculationResult
        {
            public decimal TotalInterest { get; set; } // PIC 9(7)V99 COMP-3

            public decimal TotalAmount { get; set; } // PIC 9(7)V99 COMP-3

            [MaxLength(20)]
            public string StatusMessage { get; set; } // PIC X(20)
        }

        public enum CalculationType
        {
            SimpleInterest, // 88 SIMPLE-INTEREST VALUE 'S'
            CompoundInterest // 88 COMPOUND-INTEREST VALUE 'C'
        }
    }
}