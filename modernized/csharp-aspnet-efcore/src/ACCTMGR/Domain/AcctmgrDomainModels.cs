using System;
using System.ComponentModel.DataAnnotations;

namespace ACCTMGR.Domain
{
    public class AccountManager
    {
        public CustomerRecord[] Customers { get; set; } // OCCURS 100 TIMES

        public class CustomerRecord
        {
            [MaxLength(10)]
            public string CustomerId { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string CustomerName { get; set; } // PIC X(30)

            public AddressRecord Address { get; set; }

            public AccountRecord[] Accounts { get; set; } // OCCURS 5 TIMES
        }

        public class AddressRecord
        {
            [MaxLength(50)]
            public string Street { get; set; } // PIC X(50)

            [MaxLength(20)]
            public string City { get; set; } // PIC X(20)

            [MaxLength(2)]
            public string State { get; set; } // PIC X(2)

            [MaxLength(10)]
            public string ZipCode { get; set; } // PIC X(10)
        }

        public class AccountRecord
        {
            [MaxLength(12)]
            public string AccountNumber { get; set; } // PIC X(12)

            [Precision(15, 2)]
            public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatus Status { get; set; } // 88-level equivalent
        }

        public enum AccountStatus
        {
            Active, // 88 ACTIVE VALUE 'A'
            Inactive, // 88 INACTIVE VALUE 'I'
            Closed // 88 CLOSED VALUE 'C'
        }
    }
}