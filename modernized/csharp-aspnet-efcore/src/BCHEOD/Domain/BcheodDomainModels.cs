using System;
using System.ComponentModel.DataAnnotations;

namespace BCHEOD.Domain
{
    public class BcheodDomainModels
    {
        public class Account
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string AccountName { get; set; } // PIC X(30)

            [Precision(15, 2)]
            public decimal Balance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatus Status { get; set; } // 88-level values mapped to enum

            public Address AccountAddress { get; set; } // GROUP item

            public Transaction[] Transactions { get; set; } // OCCURS 10 TIMES
        }

        public class Address
        {
            [MaxLength(50)]
            public string Street { get; set; } // PIC X(50)

            [MaxLength(20)]
            public string City { get; set; } // PIC X(20)

            [MaxLength(10)]
            public string ZipCode { get; set; } // PIC X(10)
        }

        public class Transaction
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            [DataType(DataType.Date)]
            public DateTime TransactionDate { get; set; } // PIC 9(8)

            [Precision(13, 2)]
            public decimal Amount { get; set; } // PIC S9(11)V99 COMP-3

            [MaxLength(3)]
            public string Currency { get; set; } // PIC X(3)

            public TransactionType Type { get; set; } // 88-level values mapped to enum
        }

        public enum AccountStatus
        {
            Active, // 88 ACTIVE VALUE 'A'
            Inactive, // 88 INACTIVE VALUE 'I'
            Closed // 88 CLOSED VALUE 'C'
        }

        public enum TransactionType
        {
            Credit, // 88 CREDIT VALUE 'C'
            Debit // 88 DEBIT VALUE 'D'
        }
    }
}