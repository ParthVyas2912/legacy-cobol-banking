using System;
using System.ComponentModel.DataAnnotations;

namespace BCHEOD.Domain
{
    public class BcheodDomainModels
    {
        public class Policy
        {
            [MaxLength(10)]
            public string PolicyNumber { get; set; } // PIC X(10)

            public Insured Insured { get; set; }
            public Coverage[] Coverages { get; set; } = new Coverage[5]; // OCCURS 5 TIMES
        }

        public class Insured
        {
            [MaxLength(30)]
            public string Name { get; set; } // PIC X(30)

            [MaxLength(15)]
            public string Address { get; set; } // PIC X(15)

            [MaxLength(10)]
            public string PhoneNumber { get; set; } // PIC X(10)

            [Range(0, 999.9)]
            public decimal Age { get; set; } // PIC 9(3)V9(1) COMP-3
        }

        public class Coverage
        {
            [MaxLength(5)]
            public string CoverageCode { get; set; } // PIC X(5)

            [Range(-9999999.99, 9999999.99)]
            public decimal CoverageAmount { get; set; } // PIC S9(7)V99 COMP-3

            public CoverageType CoverageType { get; set; } // 88-level
        }

        public enum CoverageType
        {
            Basic, // VALUE 'BASIC'
            Comprehensive, // VALUE 'COMP'
            Collision // VALUE 'COLL'
        }
    }
}