using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KYCSYNC.Domain
{
    public class KycsyncDomainModels
    {
        public class CustomerRecord
        {
            [MaxLength(20)] // PIC X(20)
            public string CustomerId { get; set; }

            [MaxLength(50)] // PIC X(50)
            public string CustomerName { get; set; }

            [MaxLength(10)] // PIC X(10)
            public string CustomerType { get; set; }

            public Address CustomerAddress { get; set; }

            public List<Account> Accounts { get; set; } = new List<Account>();

            public class Address
            {
                [MaxLength(100)] // PIC X(100)
                public string Street { get; set; }

                [MaxLength(50)] // PIC X(50)
                public string City { get; set; }

                [MaxLength(20)] // PIC X(20)
                public string State { get; set; }

                [MaxLength(10)] // PIC X(10)
                public string ZipCode { get; set; }
            }

            public class Account
            {
                [MaxLength(15)] // PIC X(15)
                public string AccountNumber { get; set; }

                [MaxLength(10)] // PIC X(10)
                public string AccountType { get; set; }

                public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3
            }
        }

        public class TransactionRecord
        {
            [MaxLength(20)] // PIC X(20)
            public string TransactionId { get; set; }

            [MaxLength(20)] // PIC X(20)
            public string CustomerId { get; set; }

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            public decimal Amount { get; set; } // PIC S9(13)V99 COMP-3

            [MaxLength(3)] // PIC X(3)
            public string Currency { get; set; }

            [MaxLength(50)] // PIC X(50)
            public string Description { get; set; }
        }

        public class KycStatus
        {
            [MaxLength(20)] // PIC X(20)
            public string CustomerId { get; set; }

            public KycLevel Level { get; set; }

            public enum KycLevel
            {
                Basic, // 88 BASIC VALUE
                Advanced, // 88 ADVANCED VALUE
                Verified // 88 VERIFIED VALUE
            }
        }
    }
}