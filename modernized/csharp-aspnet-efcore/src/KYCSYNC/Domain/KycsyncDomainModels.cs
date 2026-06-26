using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KYCSYNC.Domain
{
    public class KycsyncDomainModels
    {
        public class CustomerRecord
        {
            [MaxLength(10)] // PIC X(10)
            public string CustomerId { get; set; }

            [MaxLength(30)] // PIC X(30)
            public string CustomerName { get; set; }

            [MaxLength(15)] // PIC X(15)
            public string PhoneNumber { get; set; }

            [MaxLength(50)] // PIC X(50)
            public string EmailAddress { get; set; }

            public AddressRecord Address { get; set; }

            public List<AccountRecord> Accounts { get; set; } = new List<AccountRecord>();
        }

        public class AddressRecord
        {
            [MaxLength(50)] // PIC X(50)
            public string StreetAddress { get; set; }

            [MaxLength(30)] // PIC X(30)
            public string City { get; set; }

            [MaxLength(20)] // PIC X(20)
            public string State { get; set; }

            [MaxLength(10)] // PIC X(10)
            public string ZipCode { get; set; }

            [MaxLength(20)] // PIC X(20)
            public string Country { get; set; }
        }

        public class AccountRecord
        {
            [MaxLength(12)] // PIC X(12)
            public string AccountNumber { get; set; }

            [MaxLength(20)] // PIC X(20)
            public string AccountType { get; set; }

            [Range(typeof(decimal), "-9999999999999.99", "9999999999999.99")] // PIC S9(13)V99 COMP-3
            public decimal Balance { get; set; }

            public AccountStatusEnum AccountStatus { get; set; }
        }

        public enum AccountStatusEnum
        {
            Active, // 88 ACTIVE VALUE 'A'
            Inactive, // 88 INACTIVE VALUE 'I'
            Closed // 88 CLOSED VALUE 'C'
        }
    }
}