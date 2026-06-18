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

            [MaxLength(30)]
            public string PolicyHolderName { get; set; } // PIC X(30)

            [Range(0, 9999999.99)]
            public decimal PolicyAmount { get; set; } // PIC 9(7)V99 COMP-3

            [DataType(DataType.Date)]
            public DateTime PolicyStartDate { get; set; } // PIC 9(8)

            [DataType(DataType.Date)]
            public DateTime PolicyEndDate { get; set; } // PIC 9(8)

            public PolicyStatus Status { get; set; } // 88-level equivalent
        }

        public enum PolicyStatus
        {
            Active, // 88 ACTIVE VALUE 'A'
            Lapsed, // 88 LAPSED VALUE 'L'
            Cancelled // 88 CANCELLED VALUE 'C'
        }

        public class Claim
        {
            [MaxLength(15)]
            public string ClaimNumber { get; set; } // PIC X(15)

            [Range(0, 999999999.99)]
            public decimal ClaimAmount { get; set; } // PIC 9(9)V99 COMP-3

            [DataType(DataType.Date)]
            public DateTime ClaimDate { get; set; } // PIC 9(8)

            public ClaimStatus Status { get; set; } // 88-level equivalent
        }

        public enum ClaimStatus
        {
            Open, // 88 OPEN VALUE 'O'
            Closed, // 88 CLOSED VALUE 'C'
            Pending // 88 PENDING VALUE 'P'
        }

        public class Insured
        {
            [MaxLength(20)]
            public string InsuredId { get; set; } // PIC X(20)

            [MaxLength(30)]
            public string InsuredName { get; set; } // PIC X(30)

            public Address InsuredAddress { get; set; } // GROUP item
        }

        public class Address
        {
            [MaxLength(50)]
            public string Street { get; set; } // PIC X(50)

            [MaxLength(30)]
            public string City { get; set; } // PIC X(30)

            [MaxLength(20)]
            public string State { get; set; } // PIC X(20)

            [MaxLength(10)]
            public string ZipCode { get; set; } // PIC X(10)
        }

        public class PolicyDetails
        {
            public Policy PolicyInfo { get; set; } // GROUP item

            public List<Claim> Claims { get; set; } = new List<Claim>(); // OCCURS n TIMES
        }
    }
}