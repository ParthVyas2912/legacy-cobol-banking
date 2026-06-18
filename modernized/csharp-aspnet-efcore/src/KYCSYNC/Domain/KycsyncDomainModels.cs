using System;
using System.ComponentModel.DataAnnotations;

namespace KYCSYNC.Domain
{
    public class KycsyncDomainModels
    {
        public class CustomerRecord
        {
            [MaxLength(10)]
            public string CustomerId { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string FirstName { get; set; } // PIC X(30)

            [MaxLength(30)]
            public string LastName { get; set; } // PIC X(30)

            [MaxLength(1)]
            public string Gender { get; set; } // PIC X(1)

            [MaxLength(10)]
            public string DateOfBirth { get; set; } // PIC X(10)

            public Address Address { get; set; }

            public ContactDetails ContactDetails { get; set; }

            public EmploymentDetails EmploymentDetails { get; set; }
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
            public string PostalCode { get; set; } // PIC X(10)

            [MaxLength(2)]
            public string CountryCode { get; set; } // PIC X(2)
        }

        public class ContactDetails
        {
            [MaxLength(15)]
            public string PhoneNumber { get; set; } // PIC X(15)

            [MaxLength(50)]
            public string Email { get; set; } // PIC X(50)
        }

        public class EmploymentDetails
        {
            [MaxLength(30)]
            public string EmployerName { get; set; } // PIC X(30)

            [MaxLength(50)]
            public string JobTitle { get; set; } // PIC X(50)

            [Precision(11, 2)]
            public decimal AnnualIncome { get; set; } // PIC 9(9)V99 COMP-3
        }

        public class KycStatus
        {
            public KycStatusEnum Status { get; set; } // 88-level equivalent

            public enum KycStatusEnum
            {
                NotVerified, // 88 NOT-VERIFIED VALUE 'N'
                Verified,    // 88 VERIFIED VALUE 'Y'
                Pending      // 88 PENDING VALUE 'P'
            }
        }
    }
}