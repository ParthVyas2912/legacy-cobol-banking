using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BCHEOD.Domain
{
    public class BcheodDomainModels
    {
        public class EodRun
        {
            /// <summary>
            /// Run ID
            /// </summary>
            [Required]
            [MaxLength(10)] // PIC X(10)
            public string RunId { get; set; }

            /// <summary>
            /// Run Date
            /// </summary>
            [Required]
            public DateTime RunDate { get; set; }

            /// <summary>
            /// Status
            /// </summary>
            [Required]
            [MaxLength(1)] // PIC X(1)
            public string Status { get; set; }

            /// <summary>
            /// Amount
            /// </summary>
            [Required]
            [Range(-9999999999999.99, 9999999999999.99)] // PIC S9(13)V99 COMP-3
            public decimal Amount { get; set; }
        }

        public class EodRunDetails
        {
            /// <summary>
            /// Details
            /// </summary>
            [Required]
            [MaxLength(100)] // PIC X(100)
            public string Details { get; set; }
        }
    }
}