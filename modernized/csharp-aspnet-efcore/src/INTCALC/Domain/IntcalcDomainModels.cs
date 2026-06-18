using System;
using System.ComponentModel.DataAnnotations;

namespace INTCALC.Domain
{
    public class IntcalcDomainModels
    {
        public class Account
        {
            [MaxLength(10)]
            public string AccountNumber { get; set; } // PIC X(10)

            [MaxLength(30)]
            public string AccountName { get; set; } // PIC X(30)

            [Precision(5, 2)]
            public decimal InterestRate { get; set; } // PIC 9(3)V9(2) COMP-3

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

        public class Transaction
        {
            [MaxLength(12)]
            public string TransactionId { get; set; } // PIC X(12)

            public DateTime TransactionDate { get; set; } // PIC 9(8)

            [Precision(13, 2)]
            public decimal Amount { get; set; } // PIC S9(11)V99 COMP-3

            [MaxLength(3)]
            public string CurrencyCode { get; set; } // PIC X(3)

            public TransactionType Type { get; set; } // 88-level equivalent
        }

        public enum TransactionType
        {
            Deposit, // 88 DEPOSIT VALUE 'D'
            Withdrawal, // 88 WITHDRAWAL VALUE 'W'
            Interest // 88 INTEREST VALUE 'I'
        }

        public class Customer
        {
            [MaxLength(15)]
            public string CustomerId { get; set; } // PIC X(15)

            [MaxLength(50)]
            public string CustomerName { get; set; } // PIC X(50)

            public Address CustomerAddress { get; set; } // GROUP item
        }

        public class Address
        {
            [MaxLength(50)]
            public string Street { get; set; } // PIC X(50)

            [MaxLength(30)]
            public string City { get; set; } // PIC X(30)

            [MaxLength(20)]
            public string State { get; set; } // PIC X(20)

            [MaxLength(10)]
            public string ZipCode { get; set; } // PIC X(10)
        }

        public class InterestCalculation
        {
            [Precision(15, 2)]
            public decimal PrincipalAmount { get; set; } // PIC S9(13)V99 COMP-3

            [Precision(13, 2)]
            public decimal CalculatedInterest { get; set; } // PIC S9(11)V99 COMP-3

            [Precision(15, 2)]
            public decimal TotalAmount { get; set; } // PIC S9(13)V99 COMP-3
        }
    }
}