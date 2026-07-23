using System;
using System.ComponentModel.DataAnnotations;

namespace KYCSYNC.Domain
{
    public class KycsyncDomainModels
    {
        public class Customer
        {
            [MaxLength(10)] // PIC X(10)
            public string CustomerId { get; set; }

            [MaxLength(30)] // PIC X(30)
            public string CustomerName { get; set; }

            [MaxLength(50)] // PIC X(50)
            public string Address { get; set; }

            [MaxLength(15)] // PIC X(15)
            public string PhoneNumber { get; set; }

            [MaxLength(20)] // PIC X(20)
            public string Email { get; set; }

            [Precision(15, 2)] // PIC S9(13)V99 COMP-3
            public decimal AccountBalance { get; set; }

            public CustomerStatusEnum CustomerStatus { get; set; } // 88-level equivalent

            [DataType(DataType.Date)] // PIC 9(8) (YYYYMMDD)
            public DateTime DateOfBirth { get; set; }

            [MaxLength(5)] // OCCURS 5 TIMES
            public Account[] Accounts { get; set; }
        }

        public class Account
        {
            [MaxLength(12)] // PIC X(12)
            public string AccountNumber { get; set; }

            [Precision(15, 2)] // PIC S9(13)V99 COMP-3
            public decimal Balance { get; set; }

            public AccountTypeEnum AccountType { get; set; } // 88-level equivalent
        }

        public enum CustomerStatusEnum
        {
            Active, // 88 ACTIVE VALUE 'A'
            Inactive // 88 INACTIVE VALUE 'I'
        }

        public enum AccountTypeEnum
        {
            Savings, // 88 SAVINGS VALUE 'S'
            Checking // 88 CHECKING VALUE 'C'
        }
    }
}