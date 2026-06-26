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

            [Range(-9999999999999.99, 9999999999999.99)]
            public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatus Status { get; set; } // 88-level equivalent

            public AccountHolder Holder { get; set; } // GROUP item

            public Transaction[] Transactions { get; set; } // OCCURS 10 TIMES
        }

        public class AccountHolder
        {
            [MaxLength(30)]
            public string Name { get; set; } // PIC X(30)

            [MaxLength(15)]
            public string PhoneNumber { get; set; } // PIC X(15)

            public Address Address { get; set; } // GROUP item
        }

        public class Address
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

        public class Transaction
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            [Range(-999999999.99, 999999999.99)]
            public decimal Amount { get; set; } // PIC S9(9)V99 COMP-3

            [RegularExpression(@"\d{8}")]
            public DateTime TransactionDate { get; set; } // PIC 9(8)

            public TransactionType Type { get; set; } // 88-level equivalent
        }

        public enum AccountStatus
        {
            Active, // 88 ACTIVE VALUE 'A'
            Inactive, // 88 INACTIVE VALUE 'I'
            Closed // 88 CLOSED VALUE 'C'
        }

        public enum TransactionType
        {
            Deposit, // 88 DEPOSIT VALUE 'D'
            Withdrawal, // 88 WITHDRAWAL VALUE 'W'
            Transfer // 88 TRANSFER VALUE 'T'
        }
    }
}