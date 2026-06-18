using System;
using System.ComponentModel.DataAnnotations;

namespace INTCALC.Domain
{
    public class IntcalcDomainModels
    {
        public class CalculationInput
        {
            [MaxLength(10)]
            public string CustomerId { get; set; } // PIC X(10)

            [MaxLength(20)]
            public string AccountNumber { get; set; } // PIC X(20)

            public decimal LoanAmount { get; set; } // PIC 9(11)V99 COMP-3

            public decimal InterestRate { get; set; } // PIC 9(3)V99 COMP-3

            public int LoanTermMonths { get; set; } // PIC 9(3)

            public PaymentFrequencyEnum PaymentFrequency { get; set; } // 88-level equivalent
        }

        public class CalculationResult
        {
            public decimal MonthlyPayment { get; set; } // PIC 9(11)V99 COMP-3

            public decimal TotalInterest { get; set; } // PIC 9(11)V99 COMP-3

            public decimal TotalPayment { get; set; } // PIC 9(11)V99 COMP-3
        }

        public enum PaymentFrequencyEnum
        {
            Monthly, // 88 Monthly VALUE 'M'
            Quarterly, // 88 Quarterly VALUE 'Q'
            Annually // 88 Annually VALUE 'A'
        }
    }
}