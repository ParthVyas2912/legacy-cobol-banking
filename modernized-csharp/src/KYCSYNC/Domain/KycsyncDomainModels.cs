using System;
using System.ComponentModel.DataAnnotations;

namespace KYCSYNC.Domain
{
    public class KycsyncDomainModels
    {
        public class CustomerRecord
        {
            /// <summary>
            /// Customer Risk Score
            /// PIC 9(03) COMP-3
            /// </summary>
            [Range(0, 999)]
            public decimal CustRiskScore { get; set; } // PIC 9(03) COMP-3

            /// <summary>
            /// Customer Account Count
            /// PIC 9(03) COMP-3
            /// </summary>
            [Range(0, 999)]
            public decimal CustAcctCount { get; set; } // PIC 9(03) COMP-3
        }

        public class WorkStorage
        {
            /// <summary>
            /// Work Storage Next Count
            /// PIC 9(09) COMP-3
            /// </summary>
            [Range(0, 999999999)]
            public decimal WsNextCount { get; set; } // PIC 9(09) COMP-3
        }
    }
}