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
            public int EffectiveDate { get; set; }

            // PIC 9(8)
            public int ExpirationDate { get; set; }

            // PIC S9(13)V99 COMP-3
            public decimal PremiumAmount { get; set; }

            // PIC X(30)
            [MaxLength(30)]
            public string InsuredName { get; set; }

            // PIC X(15)
            [MaxLength(15)]
            public string InsuredPhone { get; set; }

            // PIC X(50)
            [MaxLength(50)]
            public string InsuredAddress { get; set; }

            public Coverage[] Coverages { get; set; } // OCCURS 5 TIMES
        }

        public class Coverage
        {
            // PIC X(5)
            [MaxLength(5)]
            public string CoverageCode { get; set; }

            // PIC S9(11)V99 COMP-3
            public decimal CoverageLimit { get; set; }

            // PIC S9(11)V99 COMP-3
            public decimal Deductible { get; set; }
        }

        public class Claim
        {
            // PIC X(10)
            [MaxLength(10)]
            public string ClaimNumber { get; set; }

            // PIC 9(8)
            public int LossDate { get; set; }

            // PIC S9(13)V99 COMP-3
            public decimal ClaimAmount { get; set; }

            // PIC X(30)
            [MaxLength(30)]
            public string ClaimantName { get; set; }

            // PIC X(15)
            [MaxLength(15)]
            public string ClaimantPhone { get; set; }

            // PIC X(50)
            [MaxLength(50)]
            public string ClaimantAddress { get; set; }

            public ClaimStatus Status { get; set; }
        }

        public enum ClaimStatus
        {
            // 88-level values
            Open, // VALUE "O"
            Closed, // VALUE "C"
            Pending // VALUE "P"
        }
    }
}