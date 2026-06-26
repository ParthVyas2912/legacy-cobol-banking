using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INTCALC.Domain
{
    public class IntcalcDomainModels
    {
        public class CalculationRequest
        {
            /// <summary>
            /// Input identifier for the calculation.
            /// </summary>
            [MaxLength(10)]
            public string RequestId { get; set; } // PIC X(10)

            /// <summary>
            /// The type of calculation to perform.
            /// </summary>
            public CalculationType CalcType { get; set; } // PIC X(1)

            /// <summary>
            /// The primary input value for the calculation.
            /// </summary>
            public decimal InputValue1 { get; set; } // PIC S9(7)V99 COMP-3

            /// <summary>
            /// The secondary input value for the calculation.
            /// </summary>
            public decimal InputValue2 { get; set; } // PIC S9(7)V99 COMP-3

            /// <summary>
            /// The number of iterations for the calculation.
            /// </summary>
            public int Iterations { get; set; } // PIC 9(5)
        }

        public class CalculationResult
        {
            /// <summary>
            /// The identifier of the calculation request.
            /// </summary>
            [MaxLength(10)]
            public string RequestId { get; set; } // PIC X(10)

            /// <summary>
            /// The result of the calculation.
            /// </summary>
            public decimal ResultValue { get; set; } // PIC S9(13)V99 COMP-3

            /// <summary>
            /// The status of the calculation.
            /// </summary>
            public CalculationStatus Status { get; set; } // PIC X(1)
        }

        public enum CalculationType
        {
            /// <summary>
            /// Addition operation.
            /// </summary>
            Addition = 'A', // 88-level value for 'A'

            /// <summary>
            /// Subtraction operation.
            /// </summary>
            Subtraction = 'S', // 88-level value for 'S'

            /// <summary>
            /// Multiplication operation.
            /// </summary>
            Multiplication = 'M', // 88-level value for 'M'

            /// <summary>
            /// Division operation.
            /// </summary>
            Division = 'D' // 88-level value for 'D'
        }

        public enum CalculationStatus
        {
            /// <summary>
            /// Calculation completed successfully.
            /// </summary>
            Success = 'S', // 88-level value for 'S'

            /// <summary>
            /// Calculation failed.
            /// </summary>
            Failure = 'F' // 88-level value for 'F'
        }
    }
}