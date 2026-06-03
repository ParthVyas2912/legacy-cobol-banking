using System;
using System.ComponentModel.DataAnnotations;

namespace INTCALC.Domain
{
    public class IntcalcDomainModels
    {
        /// <summary>
        /// Represents the daily rate.
        /// COBOL: PIC 9(2)V9(8) COMP-3
        /// </summary>
        public decimal WsDailyRate { get; set; } // Precision: 10, Scale: 8

        /// <summary>
        /// Represents the daily interest.
        /// COBOL: PIC S9(13)V99 COMP-3
        /// </summary>
        public decimal WsDailyInterest { get; set; } // Precision: 15, Scale: 2

        /// <summary>
        /// Represents the new balance.
        /// COBOL: PIC S9(13)V99 COMP-3
        /// </summary>
        public decimal WsNewBalance { get; set; } // Precision: 15, Scale: 2

        /// <summary>
        /// Represents the next transaction ID.
        /// COBOL: PIC 9(12) COMP-3
        /// </summary>
        public decimal WsNextTxnId { get; set; } // Precision: 12, Scale: 0
    }
}