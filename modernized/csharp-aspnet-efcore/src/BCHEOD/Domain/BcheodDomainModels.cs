using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BCHEOD.Domain
{
    public class BcheodDomainModels
    {
        public class Policy
        {
            // PIC X(10)
            [MaxLength(10)]
            public string PolicyNumber { get; set; }

            // PIC 9(8)
            public int PolicyEffectiveDate { get; set; }

            // PIC S9(13)V99 COMP-3
            public decimal PolicyAmount { get; set; }

            // PIC X(30)
            [MaxLength(30)]
            public string PolicyHolderName { get; set; }

            // PIC 9(3)
            public int PolicyType { get; set; }

            // PIC X(1)
            [MaxLength(1)]
            public string PolicyStatus { get; set; }

            // PIC 9(5)V9(2) COMP-3
            public decimal PremiumAmount { get; set; }

            // PIC 9(4)
            public int PolicyDurationInYears { get; set; }

            // PIC X(15)
            [MaxLength(15)]
            public string AgentCode { get; set; }

            // PIC 9(2)
            public int RegionCode { get; set; }

            // OCCURS 12 TIMES
            public List<PolicyPayment> Payments { get; set; } = new List<PolicyPayment>(12);
        }

        public class PolicyPayment
        {
            // PIC 9(8)
            public int PaymentDate { get; set; }

            // PIC S9(13)V99 COMP-3
            public decimal PaymentAmount { get; set; }

            // PIC X(1)
            [MaxLength(1)]
            public string PaymentStatus { get; set; }
        }

        public enum PolicyStatus
        {
            // 88-level values
            Active = 1, // PIC X(1) VALUE 'A'
            Inactive = 2, // PIC X(1) VALUE 'I'
            Cancelled = 3 // PIC X(1) VALUE 'C'
        }
    }
}