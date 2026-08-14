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
            /// Unique identifier for the calculation request
            /// </summary>
            [MaxLength(20)] // PIC X(20)
            public string RequestId { get; set; }

            /// <summary>
            /// Customer identifier
            /// </summary>
            [MaxLength(10)] // PIC X(10)
            public string CustomerId { get; set; }

            /// <summary>
            /// Calculation type
            /// </summary>
            public CalculationType CalcType { get; set; } // 88-level equivalent

            /// <summary>
            /// Amount to be calculated
            /// </summary>
            public decimal Amount { get; set; } // PIC S9(13)V99 COMP-3

            /// <summary>
            /// Date of the calculation request
            /// </summary>
            [MaxLength(8)] // PIC X(8)
            public string RequestDate { get; set; }

            /// <summary>
            /// List of calculation details
            /// </summary>
            public List<CalculationDetail> Details { get; set; } = new List<CalculationDetail>();
        }

        public class CalculationDetail
        {
            /// <summary>
            /// Detail sequence number
            /// </summary>
            public int SequenceNumber { get; set; } // PIC 9(5)

            /// <summary>
            /// Description of the calculation detail
            /// </summary>
            [MaxLength(50)] // PIC X(50)
            public string Description { get; set; }

            /// <summary>
            /// Detail amount
            /// </summary>
            public decimal DetailAmount { get; set; } // PIC S9(11)V99 COMP-3
        }

        public enum CalculationType
        {
            /// <summary>
            /// Type A calculation
            /// </summary>
            TypeA, // 88-level value

            /// <summary>
            /// Type B calculation
            /// </summary>
            TypeB, // 88-level value

            /// <summary>
            /// Type C calculation
            /// </summary>
            TypeC // 88-level value
        }
    }
}