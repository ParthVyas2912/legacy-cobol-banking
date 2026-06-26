using System;
using System.ComponentModel.DataAnnotations;

namespace INTCALC.Domain
{
    public class IntcalcDomainModels
    {
        public class Customer
        {
            [MaxLength(10)]
            public string CustomerId { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string CustomerName { get; set; } // PIC X(30)

            public Address CustomerAddress { get; set; } // GROUP item for address

            public decimal AccountBalance { get; set; } // PIC S9(13)V99 COMP-3

            public AccountStatusEnum AccountStatus { get; set; } // 88-level values for account status
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

        public enum AccountStatusEnum
        {
            Active, // 88 ACTIVE VALUE 'A'
            Inactive, // 88 INACTIVE VALUE 'I'
            Closed // 88 CLOSED VALUE 'C'
        }

        public class Transaction
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            public DateTime TransactionDate { get; set; } // PIC 9(8) (YYYYMMDD)

            public decimal TransactionAmount { get; set; } // PIC S9(11)V99 COMP-3

            public TransactionTypeEnum TransactionType { get; set; } // 88-level values for transaction type
        }

        public enum TransactionTypeEnum
        {
            Deposit, // 88 DEPOSIT VALUE 'D'
            Withdrawal, // 88 WITHDRAWAL VALUE 'W'
            Transfer // 88 TRANSFER VALUE 'T'
        }

        public class Account
        {
            [MaxLength(15)]
            public string AccountNumber { get; set; } // PIC X(15)

            public decimal CurrentBalance { get; set; } // PIC S9(13)V99 COMP-3

            public Transaction[] Transactions { get; set; } // OCCURS 100 TIMES
        }
    }
}