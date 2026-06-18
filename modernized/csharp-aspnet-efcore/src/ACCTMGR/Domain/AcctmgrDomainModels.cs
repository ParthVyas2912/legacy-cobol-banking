using System;
using System.ComponentModel.DataAnnotations;

namespace ACCTMGR.Domain
{
    public class AcctmgrDomainModels
    {
        public class Account
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string AccountName { get; set; } // PIC X(30)

            [Range(typeof(decimal), "-9999999999999.99", "9999999999999.99")]
            public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatus Status { get; set; } // 88-level equivalent

            public Address AccountAddress { get; set; } // GROUP item

            [MaxLength(10)]
            public Transaction[] Transactions { get; set; } // OCCURS 10 TIMES
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

        public class Transaction
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            [DataType(DataType.Date)]
            public DateTime TransactionDate { get; set; } // PIC 9(8)

            [Range(typeof(decimal), "-999999999.99", "999999999.99")]
            public decimal Amount { get; set; } // PIC S9(9)V99 COMP-3

            [MaxLength(20)]
            public string Description { get; set; } // PIC X(20)
        }

        public enum AccountStatus
        {
            Active, // 88-level value
            Inactive, // 88-level value
            Closed // 88-level value
        }
    }
}