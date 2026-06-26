using System;
using System.ComponentModel.DataAnnotations;

namespace INTCALC.Domain
{
    public class IntcalcDomainModels
    {
        public class CalculationRequest
        {
            /// <summary>
            /// Account Number
            /// </summary>
            [MaxLength(10)] // PIC X(10)
            public string AccountNumber { get; set; }

            /// <summary>
            /// Transaction Amount
            /// </summary>
            public decimal TransactionAmount { get; set; } // PIC S9(13)V99 COMP-3

            /// <summary>
            /// Interest Rate
            /// </summary>
            public decimal InterestRate { get; set; } // PIC S9(5)V99 COMP-3

            /// <summary>
            /// Calculation Type
            /// </summary>
            public CalculationTypeEnum CalculationType { get; set; } // 88-level equivalent

            /// <summary>
            /// Calculation Period
            /// </summary>
            public int CalculationPeriod { get; set; } // PIC 9(4)
        }

        public class CalculationResult
        {
            /// <summary>
            /// Result Amount
            /// </summary>
            public decimal ResultAmount { get; set; } // PIC S9(13)V99 COMP-3

            /// <summary>
            /// Status Code
            /// </summary>
            [MaxLength(2)] // PIC X(2)
            public string StatusCode { get; set; }

            /// <summary>
            /// Status Message
            /// </summary>
            [MaxLength(50)] // PIC X(50)
            public string StatusMessage { get; set; }
        }

        public enum CalculationTypeEnum
        {
            /// <summary>
            /// Simple Interest
            /// </summary>
            SimpleInterest, // 88 SIMPLE-INTEREST VALUE 'S'

            /// <summary>
            /// Compound Interest
            /// </summary>
            CompoundInterest // 88 COMPOUND-INTEREST VALUE 'C'
        }
    }
}