using System;
using System.ComponentModel.DataAnnotations;

namespace INTCALC.Domain
{
    public class IntcalcDomainModels
    {
        public class InterestCalculationRecord
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            [MaxLength(20)]
            public string CustomerName { get; set; } // PIC X(20)

            [Precision(5, 2)]
            public decimal InterestRate { get; set; } // PIC 9(3)V9(2) COMP-3

            [Precision(9, 2)]
            public decimal PrincipalAmount { get; set; } // PIC 9(7)V9(2) COMP-3

            [Precision(9, 2)]
            public decimal InterestAmount { get; set; } // PIC 9(7)V9(2) COMP-3

            [DataType(DataType.Date)]
            public DateTime CalculationDate { get; set; } // PIC 9(8)

            public PaymentFrequencyEnum PaymentFrequency { get; set; } // 88-level

            public PaymentStatusEnum PaymentStatus { get; set; } // 88-level
        }

        public enum PaymentFrequencyEnum
        {
            Monthly, // 88 Monthly VALUE 'M'
            Quarterly, // 88 Quarterly VALUE 'Q'
            Annually // 88 Annually VALUE 'A'
        }

        public enum PaymentStatusEnum
        {
            Pending, // 88 Pending VALUE 'P'
            Completed, // 88 Completed VALUE 'C'
            Overdue // 88 Overdue VALUE 'O'
        }
    }
}