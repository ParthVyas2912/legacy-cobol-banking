using System;
using System.Collections.Generic;
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

            public DateTime DateOfBirth { get; set; } // PIC 9(8)

            public Address CustomerAddress { get; set; }

            public List<Account> Accounts { get; set; } = new List<Account>();
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

            [MaxLength(20)]
            public string Country { get; set; } // PIC X(20)
        }

        public class Account
        {
            [MaxLength(15)]
            public string AccountNumber { get; set; } // PIC X(15)

            [MaxLength(20)]
            public string AccountType { get; set; } // PIC X(20)

            [Range(typeof(decimal), "0", "9999999999999.99")]
            public decimal Balance { get; set; } // PIC 9(13)V99 COMP-3

            public AccountStatus Status { get; set; }
        }

        public enum AccountStatus
        {
            Active, // 88 ACTIVE VALUE 'A'
            Inactive, // 88 INACTIVE VALUE 'I'
            Closed // 88 CLOSED VALUE 'C'
        }
    }
}