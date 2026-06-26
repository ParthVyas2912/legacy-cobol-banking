using System;
using System.ComponentModel.DataAnnotations;

namespace FRDCHK.Domain
{
    public class FrdchkDomainModels
    {
        public class Customer
        {
            [MaxLength(10)]
            public string CustomerId { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string CustomerName { get; set; } // PIC X(30)

            public Address CustomerAddress { get; set; }

            public ContactDetails CustomerContactDetails { get; set; }

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

        public class ContactDetails
        {
            [MaxLength(15)]
            public string PhoneNumber { get; set; } // PIC X(15)

            [MaxLength(50)]
            public string Email { get; set; } // PIC X(50)
        }

        public class Account
        {
            [MaxLength(12)]
            public string AccountNumber { get; set; } // PIC X(12)

            [Range(typeof(decimal), "-9999999999999.99", "9999999999999.99")]
            public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountType AccountType { get; set; }
        }

        public enum AccountType
        {
            Checking, // 88-level value
            Savings,  // 88-level value
            Credit    // 88-level value
        }
    }
}