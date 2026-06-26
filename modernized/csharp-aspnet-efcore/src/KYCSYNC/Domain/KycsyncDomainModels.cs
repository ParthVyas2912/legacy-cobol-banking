using System;
using System.ComponentModel.DataAnnotations;

namespace KYCSYNC.Domain
{
    public class KycsyncDomainModels
    {
        public class Customer
        {
            [MaxLength(20)]
            public string CustomerId { get; set; } // PIC X(20)

            [MaxLength(50)]
            public string FirstName { get; set; } // PIC X(50)

            [MaxLength(50)]
            public string LastName { get; set; } // PIC X(50)

            public DateTime DateOfBirth { get; set; } // PIC 9(8) (YYYYMMDD)

            public Address CustomerAddress { get; set; }

            public ContactDetails ContactInfo { get; set; }
        }

        public class Address
        {
            [MaxLength(100)]
            public string Street { get; set; } // PIC X(100)

            [MaxLength(50)]
            public string City { get; set; } // PIC X(50)

            [MaxLength(20)]
            public string State { get; set; } // PIC X(20)

            [MaxLength(10)]
            public string PostalCode { get; set; } // PIC X(10)

            [MaxLength(50)]
            public string Country { get; set; } // PIC X(50)
        }

        public class ContactDetails
        {
            [MaxLength(15)]
            public string PhoneNumber { get; set; } // PIC X(15)

            [MaxLength(100)]
            public string EmailAddress { get; set; } // PIC X(100)
        }

        public class KycStatus
        {
            public KycStatusEnum Status { get; set; } // PIC X(1)

            public DateTime LastUpdated { get; set; } // PIC 9(8) (YYYYMMDD)
        }

        public enum KycStatusEnum
        {
            NotStarted = 'N', // 88-level value for "Not Started"
            InProgress = 'I', // 88-level value for "In Progress"
            Completed = 'C',  // 88-level value for "Completed"
            Failed = 'F'      // 88-level value for "Failed"
        }

        public class Transaction
        {
            [MaxLength(20)]
            public string TransactionId { get; set; } // PIC X(20)

            [Range(typeof(decimal), "-9999999999999.99", "9999999999999.99")]
            public decimal Amount { get; set; } // PIC S9(13)V99 COMP-3

            public DateTime TransactionDate { get; set; } // PIC 9(8) (YYYYMMDD)

            [MaxLength(3)]
            public string CurrencyCode { get; set; } // PIC X(3)

            public TransactionTypeEnum TransactionType { get; set; } // PIC X(1)
        }

        public enum TransactionTypeEnum
        {
            Credit = 'C', // 88-level value for "Credit"
            Debit = 'D'   // 88-level value for "Debit"
        }

        public class KycSyncRequest
        {
            public Customer CustomerDetails { get; set; }

            public KycStatus KycStatusDetails { get; set; }

            [MaxLength(100)]
            public Transaction[] Transactions { get; set; } // OCCURS 100 TIMES
        }
    }
}