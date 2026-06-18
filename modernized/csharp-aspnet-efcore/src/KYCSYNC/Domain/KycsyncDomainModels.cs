using System;
using System.ComponentModel.DataAnnotations;

namespace KYCSYNC.Domain
{
    public class KycsyncDomainModels
    {
        public class Customer
        {
            [MaxLength(10)]
            public string CustomerId { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string FirstName { get; set; } // PIC X(30)

            [MaxLength(30)]
            public string LastName { get; set; } // PIC X(30)

            public Address CustomerAddress { get; set; }

            [Range(typeof(decimal), "-9999999999999.99", "9999999999999.99")]
            public decimal AccountBalance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatus Status { get; set; } // 88-level equivalent
        }

        public class Address
        {
            [MaxLength(50)]
            public string Street { get; set; } // PIC X(50)

            [MaxLength(30)]
            public string City { get; set; } // PIC X(30)

            [MaxLength(2)]
            public string State { get; set; } // PIC X(2)

            [MaxLength(10)]
            public string ZipCode { get; set; } // PIC X(10)
        }

        public enum AccountStatus
        {
            Active, // 88 ACTIVE VALUE 'A'
            Inactive, // 88 INACTIVE VALUE 'I'
            Suspended // 88 SUSPENDED VALUE 'S'
        }
    }
}