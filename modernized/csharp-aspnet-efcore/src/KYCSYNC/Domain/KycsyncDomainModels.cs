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

            public Address CustomerAddress { get; set; }

            public ContactInfo CustomerContactInfo { get; set; }

            public Account[] Accounts { get; set; } = new Account[5]; // OCCURS 5 TIMES
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

        public class ContactInfo
        {
            [MaxLength(15)]
            public string PhoneNumber { get; set; } // PIC X(15)

            [MaxLength(50)]
            public string Email { get; set; } // PIC X(50)
        }

        public class Account
        {
            [MaxLength(20)]
            public string AccountNumber { get; set; } // PIC X(20)

            [MaxLength(10)]
            public string AccountType { get; set; } // PIC X(10)

            [Precision(15, 2)]
            public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3

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