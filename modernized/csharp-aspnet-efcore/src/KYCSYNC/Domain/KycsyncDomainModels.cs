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
            public string CustomerName { get; set; } // PIC X(30)

            public Address CustomerAddress { get; set; } // GROUP item

            public decimal AccountBalance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatusEnum AccountStatus { get; set; } // 88-level

            public Transaction[] Transactions { get; set; } // OCCURS 10 TIMES

            public class Address
            {
                [MaxLength(50)]
                public string Street { get; set; } // PIC X(50)

                [MaxLength(20)]
                public string City { get; set; } // PIC X(20)

                [MaxLength(10)]
                public string PostalCode { get; set; } // PIC X(10)
            }

            public class Transaction
            {
                [MaxLength(15)]
                public string TransactionId { get; set; } // PIC X(15)

                public decimal TransactionAmount { get; set; } // PIC S9(11)V99 COMP-3

                public DateTime TransactionDate { get; set; } // PIC 9(8) (YYYYMMDD format)
            }

            public enum AccountStatusEnum
            {
                Active, // 88 ACTIVE VALUE 'A'
                Inactive, // 88 INACTIVE VALUE 'I'
                Suspended // 88 SUSPENDED VALUE 'S'
            }
        }
    }
}