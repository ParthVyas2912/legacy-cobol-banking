using System;
using System.ComponentModel.DataAnnotations;

namespace FRDCHK.Domain
{
    public class FrdchkDomainModels
    {
        /// <summary>
        /// Represents the transaction record.
        /// </summary>
        public class TxnRecord
        {
            /// <summary>
            /// Transaction amount.
            /// </summary>
            /// <remarks>PIC S9(11)V99 COMP-3</remarks>
            [Required]
            public decimal TxnAmount { get; set; } // Precision: 13, Scale: 2

            /// <summary>
            /// Transaction balance after the operation.
            /// </summary>
            /// <remarks>PIC S9(13)V99 COMP-3</remarks>
            [Required]
            public decimal TxnBalanceAfter { get; set; } // Precision: 15, Scale: 2
        }

        /// <summary>
        /// Represents the working storage for velocity and risk calculations.
        /// </summary>
        public class WorkingStorage
        {
            /// <summary>
            /// Velocity count.
            /// </summary>
            /// <remarks>PIC 9(05) COMP-3</remarks>
            [Required]
            public decimal WsVelocityCount { get; set; } // Precision: 5, Scale: 0

            /// <summary>
            /// Velocity amount.
            /// </summary>
            /// <remarks>PIC S9(11)V99 COMP-3</remarks>
            [Required]
            public decimal WsVelocityAmt { get; set; } // Precision: 13, Scale: 2

            /// <summary>
            /// Risk score.
            /// </summary>
            /// <remarks>PIC 9(03) COMP-3</remarks>
            [Required]
            public decimal WsRiskScore { get; set; } // Precision: 3, Scale: 0
        }
    }
}