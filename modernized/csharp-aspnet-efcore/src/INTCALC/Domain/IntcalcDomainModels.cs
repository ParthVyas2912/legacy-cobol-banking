using System;
using System.ComponentModel.DataAnnotations;

namespace INTCALC.Domain
{
    public class IntcalcDomainModels
    {
        public class CalculationRequest
        {
            [MaxLength(10)] // PIC X(10)
            public string RequestId { get; set; }

            [MaxLength(20)] // PIC X(20)
            public string CustomerName { get; set; }

            [Range(0, 999999999.99)] // PIC 9(9)V99 COMP-3
            public decimal LoanAmount { get; set; }

            [Range(0, 999.99)] // PIC 9(3)V99 COMP-3
            public decimal InterestRate { get; set; }

            [Range(0, 999)] // PIC 9(3)
            public int LoanTermMonths { get; set; }

            public PaymentFrequencyEnum PaymentFrequency { get; set; } // 88-level equivalent

            public LoanTypeEnum LoanType { get; set; } // 88-level equivalent

            public class PaymentSchedule
            {
                [RegularExpression(@"\d{8}")] // PIC 9(8) (YYYYMMDD)
                public DateTime PaymentDate { get; set; }

                [Range(0, 999999999.99)] // PIC 9(9)V99 COMP-3
                public decimal PaymentAmount { get; set; }

                [Range(0, 999999999.99)] // PIC 9(9)V99 COMP-3
                public decimal PrincipalComponent { get; set; }

                [Range(0, 999999999.99)] // PIC 9(9)V99 COMP-3
                public decimal InterestComponent { get; set; }
            }

            [MaxLength(360)] // Assuming OCCURS 360 TIMES
            public PaymentSchedule[] PaymentSchedules { get; set; }
        }

        public enum PaymentFrequencyEnum
        {
            Monthly, // 88-level value
            Quarterly, // 88-level value
            Annually // 88-level value
        }

        public enum LoanTypeEnum
        {
            FixedRate, // 88-level value
            VariableRate // 88-level value
        }
    }
}