using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BCHEOD.Domain
{
    public class BcheodDomainModels
    {
        public class Policy
        {
            [MaxLength(10)]
            public string PolicyNumber { get; set; } // PIC X(10)

            public decimal PremiumAmount { get; set; } // PIC 9(7)V99 COMP-3

            public int PolicyTerm { get; set; } // PIC 9(3)

            public PolicyStatus Status { get; set; } // PIC X(1)

            public Insured InsuredDetails { get; set; } // GROUP item for insured details

            public List<Beneficiary> Beneficiaries { get; set; } // OCCURS 5 TIMES
        }

        public class Insured
        {
            [MaxLength(30)]
            public string Name { get; set; } // PIC X(30)

            public DateTime DateOfBirth { get; set; } // PIC 9(8)

            [MaxLength(1)]
            public string Gender { get; set; } // PIC X(1)

            public decimal CoverageAmount { get; set; } // PIC 9(9)V99 COMP-3
        }

        public class Beneficiary
        {
            [MaxLength(30)]
            public string Name { get; set; } // PIC X(30)

            public decimal SharePercentage { get; set; } // PIC 9(3)V99 COMP-3
        }

        public enum PolicyStatus
        {
            Active = 'A', // 88-level value for Active
            Lapsed = 'L', // 88-level value for Lapsed
            Cancelled = 'C' // 88-level value for Cancelled
        }
    }
}